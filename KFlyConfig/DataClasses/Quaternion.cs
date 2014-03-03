using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KFly
{
    public class Quaternion
    {
        private float _x;

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        private float _y;

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        private float _z;

        public float Z
        {
            get { return _z; }
            set { _z = value; }
        }

        private float _w;

        public float W
        {
            get { return _w; }
            set { _w = value; }
        }


        public Quaternion()
        {
        }

        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(W));
            data.AddRange(BitConverter.GetBytes(X));
            data.AddRange(BitConverter.GetBytes(Y));
            data.AddRange(BitConverter.GetBytes(Z));
            return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count == 16)
            {
                byte[] data = bytes.ToArray();
                W = BitConverter.ToSingle(data, 0);
                X = BitConverter.ToSingle(data, 4);
                Y = BitConverter.ToSingle(data, 8);
                Z = BitConverter.ToSingle(data, 12);
            }

        }

        public static Quaternion FromBytes(List<byte> bytes)
        {
            var pi = new Quaternion();
            pi.SetBytes(bytes);
            return pi;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
