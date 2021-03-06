﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class StateMachine
    {

        /* The different states the state machine can have */
        public enum State
        {
            None,
            WaitingForSYNC,
            WaitingForSYNCorCMD,
            ReceivingCommand,
            ReceivingSize,
            ReceivingCRC8,
            ReceivingData,
            ReceivingCRC16
        };

        private State _currentState = State.WaitingForSYNC;

        private List<byte> _recievedData = new List<byte>();
        private int _dataLength = 0;
        private State _savedState = State.None;
        private bool _ack = false;

        private ITelemetryLink _link;

        public StateMachine(ITelemetryLink link)
        {
            _link = link;
        }
      
        public void Reset()
        {
            _currentState = State.WaitingForSYNC;
            _recievedData.Clear();
            _ack = false;
            _savedState = State.None;
            _dataLength = 0;
        }

        public bool Ack
        {
            get { return _ack; }
        }

        public State CurrentState
        {
            get { return _currentState; }
        }
       
        /* This is where the data comes in and the current state is checked */
        public void SerialManager(byte inData)
        {
            if (inData == KFlyCommand.SYNC)
            {
                if ((_currentState != State.WaitingForSYNC) && (_currentState != State.WaitingForSYNCorCMD) && (_currentState != State.ReceivingCommand))
                {
                    _savedState = _currentState;
                    _currentState = State.WaitingForSYNCorCMD;
                    return;
                }
            }

            switch (_currentState)
            {
                case State.WaitingForSYNC:
                    _currentState = WaitingForSyncManager(inData);
                    break;

                case State.WaitingForSYNCorCMD:
                    _currentState = WaitingForSYNCorCMDManager(inData);
                    break;

                case State.ReceivingCommand:
                    _currentState = ReveivingCommandManager(inData);
                    break;

                case State.ReceivingSize:
                    _currentState = ReveivingSizeManager(inData);
                    break;

                case State.ReceivingCRC8:
                    _currentState = ReveivingCRC8Manager(inData);
                    break;

                case State.ReceivingData:
                    _currentState = ReveivingDataManager(inData);
                    break;

                case State.ReceivingCRC16:
                    _currentState = ReveivingCRC16Manager(inData);
                    break;

                default:
                    break;

            }
        }

        /* Checks if the incomming data is SYNC, else continue waiting for it */
        private State WaitingForSyncManager(byte data)
        {
            _recievedData.Clear();

            if (data == KFlyCommand.SYNC)
            {
                _recievedData.Add(data);
                return State.ReceivingCommand;
            }
            else
                return State.WaitingForSYNC;
        }

        /* SYNC recieved, check if it is double SYNC or command */
        private State WaitingForSYNCorCMDManager(byte data)
        {
            State returnState = State.None;

            if (data == KFlyCommand.SYNC)
            {
                switch (_savedState)
                {
                    case State.ReceivingCommand:
                        returnState = ReveivingCommandManager(data);
                        break;

                    case State.ReceivingSize:
                        returnState = ReveivingSizeManager(data);
                        break;

                    case State.ReceivingCRC8:
                        returnState = ReveivingCRC8Manager(data);
                        break;

                    case State.ReceivingData:
                        returnState = ReveivingDataManager(data);
                        break;

                    case State.ReceivingCRC16:
                        returnState = ReveivingCRC16Manager(data);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                _recievedData.Clear();
                _recievedData.Add(KFlyCommand.SYNC);
                returnState = ReveivingCommandManager(data);
            }

            return returnState;
        }

        /* Check what type of command just arrived */
        private State ReveivingCommandManager(byte data)
        {
            State returnState = State.None;

            if (data == KFlyCommand.SYNC)
            {
                returnState = State.ReceivingCommand;
            }
            else
            {
                if ((byte)(data & KFlyCommand.ACK_MASK) != 0)
                    _ack = true;
                else
                    _ack = false;
                
                _recievedData.Add((byte)(data & KFlyCommand.ACK_MASK));
               // LogManager.LogDebugLine("Receiving command:" + ((byte)(data & KFlyCommand.ACK_MASK)).ToString());
                returnState = State.ReceivingSize;
            }

            return returnState;
        }

        /* Size of message */
        private State ReveivingSizeManager(byte data)
        {
            State returnState = State.None;
            byte index = _recievedData[1];

            _recievedData.Add(data);
            returnState = State.ReceivingCRC8;
            _dataLength = data;
            
            return returnState;
        }

        /* Checking first (command and length) checksum */
        private State ReveivingCRC8Manager(byte data)
        {
            State returnState = State.None;

            if (CRC8.GenerateCRC(_recievedData) == data)
            {
                _recievedData.Add(data);

                if (Ack)
                    SendACK();

                if (_dataLength == 0)
                {
                    returnState = State.WaitingForSYNC;
                    Parser(_recievedData);
                }
                else
                {
                    returnState = State.ReceivingData;
                }
            }
            else
            {
                returnState = State.WaitingForSYNC;
            }

            return returnState;
        }

        /* Recieving data package manager */
        private State ReveivingDataManager(byte data)
        {
            State returnState = State.None;

            _recievedData.Add(data);

            if (_recievedData.Count < (_dataLength + 4))
                returnState = State.ReceivingData;
            else
                returnState = State.ReceivingCRC16;

            return returnState;
        }

        /* Recieving second checksum */
        private State ReveivingCRC16Manager(byte data)
        {
            State returnState = State.None;

            _recievedData.Add(data);

            if (_recievedData.Count < (_dataLength + 6))
                returnState = State.ReceivingCRC16;
            else
            {
                returnState = State.WaitingForSYNC;
                int crc = CRC_CCITT.GenerateCRC(_recievedData.GetRange(0, _recievedData.Count - 2));
                byte[] crcb = BitConverter.GetBytes(crc);

                /* Check so CRC is correct */
                if (_recievedData[_recievedData.Count - 1] == crcb[0] && _recievedData[_recievedData.Count - 2] == crcb[1])
                {
                    if (Ack)
                        SendACK();

                    Parser(_recievedData);
                }
                else
                {
                    LogManager.LogDebugLine("Wrong crc");
                }
            }

            return returnState;
        }

        private void SendACK()
        {
            /* Add send ACK/NACK */
        }

        private void Parser(List<byte> message) {
            KFlyCommand cmd = KFlyCommand.Parse(message.GetRange(0, 
                (message.Count > 4)? message.Count - 2 : message.Count));
            if (cmd != null)
            {
                _link.HandleReceived(cmd);
            }
	    }
    }
}
