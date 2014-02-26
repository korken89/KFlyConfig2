using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class PIData
    {
        public float PGain;
        public float IGain;
        public float ILimit;

        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(PGain));
            data.AddRange(BitConverter.GetBytes(IGain));
            data.AddRange(BitConverter.GetBytes(ILimit));
            return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count == 12)
            {
                byte[] data = bytes.ToArray();
                PGain = BitConverter.ToSingle(data, 0);
                IGain = BitConverter.ToSingle(data, 4);
                ILimit = BitConverter.ToSingle(data, 8);
            }

        }

        public static PIData FromBytes(List<byte> bytes)
        {
            var pi = new PIData();
            pi.SetBytes(bytes);
            return pi;
        }
    }
}
