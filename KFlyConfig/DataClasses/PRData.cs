using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace KFly
{
    /// <summary>
    /// Pitch, Roll data
    /// </summary>
    public class PRData
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
    
      

    
        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(Pitch));
            data.AddRange(BitConverter.GetBytes(Roll));
            return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count == 8)
            {
                byte[] data = bytes.ToArray();
                Pitch = BitConverter.ToSingle(data, 0);
                Roll = BitConverter.ToSingle(data, 4);
            }

        }

        public static PRData FromBytes(List<byte> bytes)
        {
            var pi = new PRData();
            pi.SetBytes(bytes);
            return pi;
        }

        public Boolean IsValid
        {
            get
            {
                return !float.IsNaN(Pitch) && !float.IsNaN(Roll);
            }
        }

        public override string ToString()
        {
            return String.Format("pitch: {0}, roll: {1}", Pitch, Roll);
        }
    }
}
