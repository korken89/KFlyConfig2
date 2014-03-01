using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using KFly.Utils;

namespace KFly
{
    public class ConnectedStateChangedEventArgs
    {
        public bool Connected;

        public ConnectedStateChangedEventArgs(bool state)
        {
            Connected = state;
        }
    }

    public class TelemetrySerialPort: ITelemetryLink
    {
       
        private SerialPort _serialPort;
        private string _portname = "/dev/ttyUSB0";
        private Baudrate _baudrate = Baudrate.Baud_115200;


        private StateMachine _stateMachine; 

        private static BlockingCollection<KFlyCommand> _sendBuffer = new BlockingCollection<KFlyCommand>(50);
        private static BlockingCollection<KFlyCommand> _receiveBuffer = new BlockingCollection<KFlyCommand>(50);

        private ConnectionStatus _status = ConnectionStatus.Disconnected;
        
    
        private bool _gotReadWriteError = true;

        private ulong _totalOut = 0;
        private ulong _totalIn = 0;
       
        private bool _isOpen = false;
        private bool _isrunning = true;

        private Thread _receiverthread;
        private Thread _senderthread;
        private Thread _connectionThread;
        private DateTime _lastReceivedMsg;
        private static TimeSpan KFLY_TIME_OUT = TimeSpan.FromSeconds(10);
        private static TimeSpan KFLY_TIME_BETWEEN_PING = TimeSpan.FromSeconds(2);


        public TelemetrySerialPort()
        {
            _stateMachine = new StateMachine(this);
        }

        public bool IsConnected
        {
            get { return _status == ConnectionStatus.Connected; }
        }

        public ConnectionStatus Status
        {
            get { return _status; }
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


        public void Connect()
        {
            if (_connectionThread == null)
            {
                //Connection thread handles the opening of the port
                _connectionThread = new Thread(ConnectionThread);
                _connectionThread.Start();
            }
        }

        private void SetStatus(ConnectionStatus status)
        {
            if (status != _status)
            {
                _status = status;
            Telemetry.Handle(new ConnectionStatusChanged(status));
            }
        }

        public void Disconnect()
        {
            if (_connectionThread != null)
            {
                try
                {
                    _connectionThread.Abort();
                }
                catch { };
                _connectionThread = null;
            }

           
            _isOpen = false;
            _gotReadWriteError = false;
            SetStatus(ConnectionStatus.Disconnected);
            LogManager.LogInfoLine("Closing connection to Serialport " + _portname);
               
            //
            try { _senderthread.Abort(); }
            catch { }
            _senderthread = null;
            try { _receiverthread.Abort(); }
            catch { }
            _receiverthread = null;
            //
            ClosePort();
        }


        public void SendMessage(KFlyCommand cmd)
        {
            //Nonblocking add to buffer
            if (!_sendBuffer.TryAdd(cmd))
            {
                LogManager.LogWarningLine("Sendbuffer Full");
            }
        }

        private bool OpenPort()
        {
            bool success = false;
            try
            {
                bool tryopen = (_serialPort == null);
                if (Environment.OSVersion.Platform.ToString().StartsWith("Win") == false)
                {
                    tryopen = (tryopen && System.IO.File.Exists(_portname));
                }
                if (tryopen)
                {
                    _serialPort = new SerialPort();
                    _serialPort.PortName = _portname;
                    _serialPort.BaudRate = (int)(_baudrate);
                    _serialPort.ErrorReceived += HandleErrorReceived;
                    _serialPort.DataReceived += HandleDataReceived;
                }
                if (_serialPort.IsOpen == false)
                {
                    _serialPort.Open();
                }
                success = true;
            }
            catch (Exception)
            {
            }
           
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


        private void ClosePort()
        {
            if (_serialPort != null)
            {
                try
                {
                    _serialPort.Close();
                    _serialPort.ErrorReceived -= HandleErrorReceived;
                    _serialPort.DataReceived -= HandleDataReceived;
                }
                catch
                {
                }
                _serialPort = null;
            }
        }

        private void HandleErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            LogManager.LogErrorLine("SerialPortInput ERROR: " + e.EventType.ToString() + " => " + e.ToString());
            _gotReadWriteError = true;
        }

        private void HandleDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[2048];
            int bytesread = _serialPort.Read(buffer, 0, 2048);
            _totalIn += (uint)bytesread;
            for (int i = 0; i < bytesread; i++)
            {
                _stateMachine.SerialManager(buffer[i]);
            }
        }

        /// <summary>
        /// This thread handles the connection
        /// It waits for the port to be available and then connects
        /// It detects when the port has been disconnected and
        /// then reconnects
        /// </summary>
        private void ConnectionThread()
        {
            _isOpen = false;
            _gotReadWriteError = false;
            while (true)
            {
                LogManager.LogInfoLine("Connecting to Serialport " + _portname);      
                //Ok, port not open, lets try connect until successful
                bool firstFailure = true;
                while (!_isOpen)
                {
                    _isOpen = OpenPort();
                    if ((!_isOpen) && (firstFailure))
                    {
                        LogManager.LogErrorLine("First connection to Serialport " + _portname + " failed. Going into polling mode...");
                        SetStatus(ConnectionStatus.Polling);
                        firstFailure = false;
                    }
                    if (!_isOpen)
                    {
                        Thread.Sleep(3000);
                    }
                }

                //Ok, now open, lets ping and check that we have a Kfly
                LogManager.LogInfoLine("Serialport " + _portname + " connected");
                _lastReceivedMsg = DateTime.Now; //Set so timeout for first ping is right

                //Going into checkforerror mode
                while (!_gotReadWriteError)
                {
                    if ((_lastReceivedMsg + KFLY_TIME_BETWEEN_PING) < DateTime.Now)
                    {
                        if (!_sendBuffer.TryAdd(new Ping()))
                        {
                            LogManager.LogDebugLine("Could not add ping to send buffer");
                        }
                    }
                    Thread.Sleep(KFLY_TIME_BETWEEN_PING);
                    _receiveBuffer.TryAdd(new ConnectionStatistics(_totalIn, _totalOut));
                    if ((_lastReceivedMsg + KFLY_TIME_OUT) < DateTime.Now )
                    {
                        LogManager.LogErrorLine("Communication timeout!");
                        _gotReadWriteError = true;
                    }
                }

                //Error, lets close the port
                LogManager.LogInfoLine("Closing connection to Serialport " + _portname);
                _isOpen = false;
                _gotReadWriteError = false;
               
                ClosePort();
            }
        }

        ManualResetEventSlim _receivedAck = new ManualResetEventSlim(false);

        private void _senderLoop(object obj)
        {
            Clear(_sendBuffer);
            while (!_sendBuffer.IsCompleted)
            {
                KFlyCommand message = _sendBuffer.Take();
                List<byte> data = message.ToTx();
                try
                {
                    if (message.UseAck)
                    {
                        _receivedAck.Reset();
                        var retries = 0;
                        int msEachTry = Convert.ToInt32(message.TimeOut / 3);
                        while (!_receivedAck.IsSet)
                        {
                            retries++;
                            var count = data.Count;
                            _totalOut += (uint)count;
                            LogManager.LogDebugLine("Sending " + message.ToString() + " with ack!");
                            _serialPort.Write(data.ToArray(), 0, count);
                            _receivedAck.Wait(msEachTry);
                            if (retries > 3)
                                break;
                        }
                        if (message.ActionAfterAck != null)
                        {
                            Task.Run(() =>
                                {
                                    try
                                    {
                                        message.ActionAfterAck((_receivedAck.IsSet) ? SendResult.OK : SendResult.TIME_OUT);
                                    }
                                    catch { };
                                });
                        }
                    }
                    else
                    {
                        LogManager.LogDebugLine("Sending " + message.ToString());
                        var count = data.Count;
                        _totalOut += (uint)count;
                        _serialPort.Write(data.ToArray(), 0, count);
                    }
                   
                }
                catch (Exception e)
                {
                    LogManager.LogErrorLine(e.Message);
                    _gotReadWriteError = true;
                }
            }
            LogManager.LogErrorLine("Sendbuffer completed, should not happen");
        }

        private void _receiverloop()
        {
            Clear(_receiveBuffer);
            while (_isrunning)
            {
                KFlyCommand message = _receiveBuffer.Take();
                Telemetry.Handle(message);
            }
        }

        private void Clear<T>(BlockingCollection<T> blockingCollection)
        {
            while (blockingCollection.Count > 0)
            {
                T item;
                blockingCollection.TryTake(out item);
            }
        }

       
        public void HandleReceived(KFlyCommand cmd)
        {
            _lastReceivedMsg = DateTime.Now;
            LogManager.LogDebugLine(cmd.ToString()+ " received");
            if ((cmd.Type == KFlyCommandType.ACK) && (!_receivedAck.IsSet))
            {
                _receivedAck.Set();
            }
            if (_status != ConnectionStatus.Connected)
            {
                LogManager.LogInfoLine("KFly identified");
                SetStatus(ConnectionStatus.Connected);
            }
            else
                _receiveBuffer.Add(cmd);
        }
    }


}

