using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace KFly
{
    public class XYZData
    {
        public float X;
        public float Y;
        public float Z;

        public String XString
        {
            get
            {
                return X.ToString("0.000",CultureInfo.InvariantCulture);
            }
        }

        public String YString
        {
            get
            {
                return Y.ToString("0.000", CultureInfo.InvariantCulture);
            }
        }

        public String ZString
        {
            get
            {
                return Z.ToString("0.000", CultureInfo.InvariantCulture);
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
            if (bytes.Count == 12)
            {
                byte[] data = bytes.ToArray();
                X = BitConverter.ToSingle(data, 0);
                Y = BitConverter.ToSingle(data, 4);
                Z = BitConverter.ToSingle(data, 8);
            }

        }

        public static XYZData FromBytes(List<byte> bytes)
        {
            var pi = new XYZData();
            pi.SetBytes(bytes);
            return pi;
        }

        public Boolean IsValid
        {
            get
            {
                return !float.IsNaN(X) && !float.IsNaN(Y) && !float.IsNaN(Z);
            }
        }

        public override string ToString()
        {
            return String.Format("x: {0}, y: {1}, z: {2}", X, Y, Z);
        }
    }
}
