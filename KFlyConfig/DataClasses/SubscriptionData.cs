using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KFly
{
    public class SubscriptionData: KFlyConfigurationData
    {

        public SubscriptionData()
        {
           
        }

        public enum Cmd
        {
            OFF = 0,
            ON = 1
        }

        private KFlyPort _port;

        public KFlyPort Port
        {
            get { return _port; }
            set { _port = value; NotifyPropertyChanged("Port"); }
        }

        private KFlyCommandType _command;

        public KFlyCommandType KFlyCommand
        {
            get { return _command; }
            set { _command = value; NotifyPropertyChanged("KFlyCommand"); }
        }

        private Cmd _cmd;

        public Cmd Command
        {
            get { return _cmd; }
            set { _cmd = value; NotifyPropertyChanged("Command"); }
        }

        private UInt32 _deltaTime; //ms

        public UInt32 DeltaTime
        {
            get { return _deltaTime; }
            set { _deltaTime = value; NotifyPropertyChanged("DeltaTime"); }
        }

        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.Add((byte)Port);
            data.Add((byte)KFlyCommand);
            data.Add((byte)Command);
            data.AddRange(BitConverter.GetBytes(DeltaTime));
            return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count >= 7)
            {
                Port = (KFlyPort)bytes[0];
                KFlyCommand = (KFlyCommandType)bytes[1];
                Command = (Cmd)bytes[2];
                DeltaTime = BitConverter.ToUInt32(bytes.ToArray(), 3);
            }
        }
        public static SubscriptionData FromBytes(List<byte> bytes)
        {
            var c = new SubscriptionData();
            c.SetBytes(bytes);
            return c;
        }
    }
}
