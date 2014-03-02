using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KFly
{
    public class PIData: INotifyPropertyChanged
    {
        private float _pGain;

        public float PGain
        {
            get { return _pGain; }
            set 
            { 
                if (_pGain != value)
                {
                    _pGain = value;
                    this.NotifyPropertyChanged("PGain");
                }
            }
        }
        private float _iGain;

        public float IGain
        {
            get { return _iGain; }
            set
            {
                if (_iGain != value)
                {
                    _iGain = value;
                    this.NotifyPropertyChanged("IGain");
                }
            }
        }
        private float _iLimit;

        public float ILimit
        {
            get { return _iLimit; }
            set
            {
                if (_iLimit != value)
                {
                    _iLimit = value;
                    this.NotifyPropertyChanged("ILimit");
                }
            }
        }

        public PIData()
        {
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
