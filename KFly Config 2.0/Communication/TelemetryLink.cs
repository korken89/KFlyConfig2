using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace KFly.Communication
{
    class TelemetryLink
    {
        private delegate void VoidCallbackType(int data);

        // Property variables
        private string _baudRate = string.Empty;
        private string _parity = string.Empty;
        private string _stopBits = string.Empty;
        private string _dataBits = string.Empty;
        private string _portName = string.Empty;

        private SerialPort comPort = new SerialPort();

        private StateMachine _stateMachine = new StateMachine();
    
        public enum State
        {
            Wait,
            GetDataCount,
            GetData,
            WaitForCRC
        };

        public State CurrentState = State.Wait;

        public string BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; }
        }

        public string PortName
        {
            get { return _portName; }
            set { _portName = value; }
        }

        public TelemetryLink()
        {
            _baudRate = "115200";
            _parity = "None";
            _stopBits = "One";
            _dataBits = "8";
            _portName = string.Empty;
         
            

            // Add event handler
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
        }

        public bool OpenPort()
        {
            try
            {
                if (comPort.IsOpen == true) comPort.Close();

                comPort.BaudRate = int.Parse(_baudRate);
                comPort.DataBits = int.Parse(_dataBits);
                comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), _stopBits);
                comPort.Parity = (Parity)Enum.Parse(typeof(Parity), _parity);
                comPort.PortName = _portName;

                comPort.Open();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ClosePort()
        {
            if (comPort.IsOpen == true) comPort.Close();
        }

        public void SendData(KFlyCommand cmd)
        {
            byte[] data = cmd.ToTx().ToArray();
            comPort.Write(data, 0, data.Length);
        }

        public void SendData(List<KFlyCommand> cmds)
        {
            foreach (KFlyCommand cmd in cmds)
            {
                SendData(cmd);
            }
        }

        private List<byte> _received = new List<byte>();
       
        

        private void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[]buffer = new byte[2048];
            int bytesread = comPort.Read(buffer, 0, 2048);
            for (int i = 0; i < bytesread; i++)
            {
                _stateMachine.SerialManager(buffer[i]);
            }
       }
    }
}
