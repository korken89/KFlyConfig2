using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace KFly
{
    public class RawRCData: KFlyConfigurationData
    {
        private static int MAX_NUMBER_OF_INPUTS = 12;

        private Boolean _connected = false;

        public Boolean Connected
        {
            get { return _connected; }
            set { _connected = value; NotifyPropertyChanged("Connected"); }
        }

        private UInt16 _numberOfInputs = 0;

        public UInt16 NumberOfInputs
        {
            get { return _numberOfInputs; }
            set { _numberOfInputs = value; NotifyPropertyChanged("NumberOfInputs"); }
        }

        private UInt16[] _values = new UInt16[MAX_NUMBER_OF_INPUTS];

        public UInt16[] Values
        {
            get { return _values; }
            set { _values = value; NotifyPropertyChanged("values"); }
        }

        
        private UInt16 _rssi = 0;

        public UInt16 Rssi
        {
            get { return _rssi; }
            set { _rssi = value; NotifyPropertyChanged("Rssi"); }
        }


        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count >= 8+MAX_NUMBER_OF_INPUTS*2)
            {
                byte[] b = bytes.ToArray();
                _connected = BitConverter.ToBoolean(b, 0);
                _numberOfInputs = BitConverter.ToUInt16(b, 4);
                for (int i = 0; i < MAX_NUMBER_OF_INPUTS; i++)
                {
                    _values[i] = BitConverter.ToUInt16(b, 6 + i * 2);
                }
                _rssi = BitConverter.ToUInt16(b, 6 + MAX_NUMBER_OF_INPUTS * 2);
            }
        }

        public override bool IsValid
        {
            get
            {
                return _connected && (_rssi > 0);
            }
        }

        public static RawRCData FromBytes(List<byte> bytes)
        {
            var c = new RawRCData();
            c.SetBytes(bytes);
            return c;
        }
       
    }
}
