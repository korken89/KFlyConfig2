using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace KFly
{
    public class XYZDataInt16
    {
        public Int16 X;
        public Int16 Y;
        public Int16 Z;

        public String XString
        {
            get
            {
                return X.ToString(CultureInfo.InvariantCulture);
            }
        }

        public String YString
        {
            get
            {
                return Y.ToString(CultureInfo.InvariantCulture);
            }
        }

        public String ZString
        {
            get
            {
                return Z.ToString(CultureInfo.InvariantCulture);
            }
        }

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
            if (bytes.Count == 6)
            {
                byte[] data = bytes.ToArray();
                X = BitConverter.ToInt16(data, 0);
                Y = BitConverter.ToInt16(data, 2);
                Z = BitConverter.ToInt16(data, 4);
            }

        }

        public static XYZDataInt16 FromBytes(List<byte> bytes)
        {
            var pi = new XYZDataInt16();
            pi.SetBytes(bytes);
            return pi;
        }

        public override string ToString()
        {
            return String.Format("x: {0}, y: {1}, z: {2}", X, Y, Z);
        }
    }
}
