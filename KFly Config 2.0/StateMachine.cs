using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly_Config_2._0
{
    class StateMachine
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
        private bool Ack = false;

        /* This is where the data comes in and the current state is checked */
        public void SerialManager(byte inData)
        {
            if (inData == KFlyCommand.SYNC)
            {
                if ((_currentState != State.WaitingForSYNC) && (_currentState != State.WaitingForSYNCorCMD) && (_currentState != State.ReceivingCommand))
                {
                    _savedState = _currentState;
                    _currentState = State.WaitingForSYNCorCMD;
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
                    Ack = true;
                else
                    Ack = false;
                
                _recievedData.Add((byte)(data & KFlyCommand.ACK_MASK));
                returnState = State.ReceivingSize;
            }

            return returnState;
        }

        /* Size of message */
        private State ReveivingSizeManager(byte data)
        {
            State returnState = State.None;
            byte index = _recievedData[1];

            if ((data == KFlyCommand.CommandLength[index]) || (KFlyCommand.CommandLength[index] == 255))
            {
                _recievedData.Add(data);
                returnState = State.ReceivingCRC8;
                _dataLength = data;
            }
            else
            {
                returnState = State.WaitingForSYNC;
            }

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
            }

            return returnState;
        }

        private void SendACK()
        {
            /* Add send ACK/NACK */
        }

        private void Parser(List<byte> message) {
		    Console.WriteLine("Arrived at Parser! :D");
		    Console.Write("Command: ");
		    Console.WriteLine((KFlyCommand.Command)message[1]);
		    Console.Write("Message: ");
            foreach (byte b in message)
            {
                Console.Write("0x" + b.ToString("X2") + " ");
            }
            Console.WriteLine();
            Console.WriteLine();
	    }
    }
}
