using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using KFly.Logging;
using KFly.Utils;

namespace KFly.Communication
{
    public class ConnectedStateChangedEventArgs
    {
        public bool Connected;

        public ConnectedStateChangedEventArgs(bool state)
        {
            Connected = state;
        }
    }

    public class TelemetrySerialPort
    {
        public delegate void MessageReceivedEvent(byte[] message);
        public event MessageReceivedEvent MessageReceived;

        private SerialPort _serialport;
        private string _portname = "/dev/ttyUSB0";
        private Baudrate _baudrate = Baudrate.Baud_115200;


        private StateMachine _stateMachine = new StateMachine();
    
        private bool gotReadWriteError = true;
        private bool keepconnectionalive = false;
        private Thread _connectionwatcher;

        private bool _isconnected = false;
        private bool _isrunning = true;

        private object _writelock = new object();

        private Thread _receiverthread;
        private Thread _senderthread;


        private bool _debug = false;


        public TelemetrySerialPort()
        {
        }

        public bool IsConnected
        {
            get { return _isconnected && !gotReadWriteError; }
        }

        public bool Debug
        {
            get { return _debug; }
            set { _debug = value; }
        }

        public String PortName
        {
            get
            {
                return _portname;
            }
            set
            {
                _portname = value;
            }
        }

        public Baudrate Baudrate
        {
            get
            {
                return _baudrate;
            }
            set
            {
                _baudrate = value;
            }
        }


        public bool Connect()
        {
            bool success = _open();
            //
            //
            // we use reader loop for Linux/Mono compatibility
            //
            if (_connectionwatcher != null)
            {
                try
                {
                    keepconnectionalive = false;
                    _connectionwatcher.Abort();
                }
                catch { }
            }
            //
            keepconnectionalive = true;
            _connectionwatcher = new Thread(new ThreadStart(delegate()
            {
                gotReadWriteError = !success;
                //
                while (keepconnectionalive)
                {
                    if (gotReadWriteError)
                    {
                        try
                        {
                            _close();
                        }
                        catch (Exception unex)
                        {
                            if (Debug)
                                LogManager.LogErrorLine(unex.Message + "\n" + unex.StackTrace);
                        }
                        Thread.Sleep(5000);
                        if (keepconnectionalive)
                        {
                            try
                            {
                                gotReadWriteError = !_open();
                            }
                            catch (Exception unex)
                            {
                                if (Debug)
                                    LogManager.LogErrorLine(unex.Message + "\n" + unex.StackTrace);
                            }
                        }
                    }
                    //
                    Thread.Sleep(1000);
                }
            }));
            _connectionwatcher.Start();
            return success;
        }

        public void Disconnect()
        {
            keepconnectionalive = false;
            //
            try { _senderthread.Abort(); }
            catch { }
            _senderthread = null;
            try { _receiverthread.Abort(); }
            catch { }
            _receiverthread = null;
            //
            _close();
        }


        public void SendMessage(byte[] message)
        {
            _messagequeue.Enqueue(message);
        }

        private bool _open()
        {
            bool success = false;
            try
            {
                bool tryopen = (_serialport == null);
                if (Environment.OSVersion.Platform.ToString().StartsWith("Win") == false)
                {
                    tryopen = (tryopen && System.IO.File.Exists(_portname));
                }
                if (tryopen)
                {
                    _serialport = new SerialPort();
                    _serialport.PortName = _portname;
                    _serialport.BaudRate = Convert.ToInt32(DisplayValueEnum.GetDescriptionValue(_baudrate));
                    _serialport.ErrorReceived += HanldeErrorReceived;
                }
                if (_serialport.IsOpen == false)
                {
                    _serialport.Open();
                }
                success = true;
            }
            catch (Exception ex)
            {
            }
            if (success)
            {
                Telemetry.Handle(new ConnectionStatusChanged() { Connected = true });
                LogManager.LogInfoLine("Serialport " + _portname + " connected");                   
            }
            else
            {
                LogManager.LogErrorLine("Connection to Serialport " + _portname + " failed");
            }

            _isconnected = success;
            //
            if (success && _receiverthread == null)
            {
                _receiverthread = new Thread(_receiverloop);
                _receiverthread.Start();
                //
                _senderthread = new Thread(_senderLoop);
                _senderthread.Start();
            }
            return success;
        }


        private void _close()
        {
            if (_serialport != null)
            {
                try
                {
                    _serialport.Close();
                    _serialport.ErrorReceived -= HanldeErrorReceived;
                }
                catch
                {
                }
                _serialport = null;
                //
                if (_isconnected)
                {
                    Telemetry.Handle(new ConnectionStatusChanged() { Connected = false });
                }
                //
                _isconnected = false;
            }
        }

        private void HanldeErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            LogManager.LogErrorLine("SerialPortInput ERROR: " + e.EventType.ToString() + " => " + e.ToString());
        }

        private Queue<byte[]> _messagequeue = new Queue<byte[]>();
        private void _senderLoop(object obj)
        {
            _messagequeue.Clear();
            while (_isrunning)
            {
                if (_serialport != null)
                {
                    try
                    {
                        while (_messagequeue.Count > 0)
                        {
                            byte[] message = _messagequeue.Dequeue();
                            //lock (_writelock)
                            try
                            {
                                if (Debug)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("SPO < " + ByteArrayToString(message));
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                _serialport.Write(message, 0, message.Length);
                            }
                            catch (Exception e)
                            {
                                if (Debug)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("SPO ! ERROR: " + e.Message);
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    Thread.Sleep(950);
                }
                Thread.Sleep(50);
            }
        }


        private void _receiverloop()
        {
            while (_isrunning)
            {
                int msglen = 0;
                //
                if (_serialport != null)
                {
                    try
                    {
                        msglen = _serialport.BytesToRead;
                        //
                        if (msglen > 0)
                        {
                            byte[] message = new byte[msglen];
                            //
                            int readbytes = 0;
                            while (_serialport.Read(message, readbytes, msglen - readbytes) <= 0)
                                ; // noop
                            if (Debug)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("SPI > " + ByteArrayToString(message));
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            if (MessageReceived != null)
                            {
                                _runAsync(() =>
                                {
                                    MessageReceived(message);
                                });
                            }
                        }
                        else
                        {
                            Thread.Sleep(50);
                        }
                    }
                    catch (Exception e)
                    {
                        gotReadWriteError = true;
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }


        public String ByteArrayToString(byte[] message)
        {
            String ret = String.Empty;
            foreach (byte b in message)
            {
                ret += b.ToString("X2") + " ";
            }
            return ret.Trim();
        }


        private void _runAsync(Action action)
        {
            Thread t = new Thread(delegate()
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {

                    Console.WriteLine("SerialPortLib ERROR!!!!!! " + e.Message + "\n" + e.StackTrace);

                }
            });
            t.Start();
        }
    }

}

