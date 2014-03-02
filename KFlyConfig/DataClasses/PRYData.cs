using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace KFly
{
    /// <summary>
    /// Pitch, Roll, Yaw data
    /// </summary>
    public class PRYData
    {
        private float _pitch;

        public float Pitch
        {
            get { return _pitch; }
            set { _pitch = value; }
        }
        private float _roll;

        public float Roll
        {
            get { return _roll; }
            set { _roll = value; }
        }
        private float _yaw;

        public float Yaw
        {
            get { return _yaw; }
            set { _yaw = value; }
        }

        

        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(Pitch));
            data.AddRange(BitConverter.GetBytes(Roll));
            data.AddRange(BitConverter.GetBytes(Yaw));
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
            }

        }

        public static PRYData FromBytes(List<byte> bytes)
        {
            var pi = new PRYData();
            pi.SetBytes(bytes);
            return pi;
        }

        public Boolean IsValid
        {
            get
            {
                return !float.IsNaN(Pitch) && !float.IsNaN(Roll) && !float.IsNaN(Yaw);
            }
        }

        public override string ToString()
        {
            return String.Format("pitch: {0}, roll: {1}, yaw: {2}", Pitch, Roll, Yaw);
        }
    }
}
