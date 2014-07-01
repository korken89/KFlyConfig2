using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace KFly
{
    /// <summary>
    /// Pitch, Roll, Yaw, Throttle data
    /// </summary>
    public class PRYTData: KFlyConfigurationData
    {
        private float _pitch;

        public float Pitch
        {
            get { return _pitch; }
            set { _pitch = value; NotifyPropertyChanged("Pitch"); }
        }
        private float _roll;

        public float Roll
        {
            get { return _roll; }
            set { _roll = value; NotifyPropertyChanged("Roll"); }
        }
        private float _yaw;

        public float Yaw
        {
            get { return _yaw; }
            set { _yaw = value; NotifyPropertyChanged("Yaw"); }
        }

        private float _throttle;

        public float Throttle
        {
            get { return _throttle; }
            set { _throttle = value; NotifyPropertyChanged("Throttle"); }
        }


        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(Pitch));
            data.AddRange(BitConverter.GetBytes(Roll));
            data.AddRange(BitConverter.GetBytes(Yaw));
            data.AddRange(BitConverter.GetBytes(Throttle));
            return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count == 12)
            {
                byte[] data = bytes.ToArray();
                Pitch = BitConverter.ToSingle(data, 0);
                Roll = BitConverter.ToSingle(data, 4);
                Yaw = BitConverter.ToSingle(data, 8);
                Throttle = BitConverter.ToSingle(data, 12);
            }

        }

        public static PRYTData FromBytes(List<byte> bytes)
        {
            var pi = new PRYTData();
            pi.SetBytes(bytes);
            return pi;
        }

        public Boolean IsValid
        {
            get
            {
                return !float.IsNaN(Pitch) && !float.IsNaN(Roll) && !float.IsNaN(Yaw) && !float.IsNaN(Throttle);
            }
        }

        public override string ToString()
        {
            return String.Format("pitch: {0}, roll: {1}, yaw: {2}, throttle: {3}", Pitch, Roll, Yaw, Throttle);
        }
    }
}
