using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class XYZData
    {
        public float X;
        public float Y;
        public float Z;

        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(X));
            data.AddRange(BitConverter.GetBytes(Y));
            data.AddRange(BitConverter.GetBytes(Z));
            return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count == 12)
            {
                byte[] data = bytes.ToArray();
                X = BitConverter.ToSingle(data, 0);
                Y = BitConverter.ToSingle(data, 3);
                Z = BitConverter.ToSingle(data, 7);
            }

        }

        public static XYZData FromBytes(List<byte> bytes)
        {
            var pi = new XYZData();
            pi.SetBytes(bytes);
            return pi;
        }
    }
}
