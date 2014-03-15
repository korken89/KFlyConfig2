using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class MotorData
    {
        private XYZData _position = new XYZData();
        private Quaternion _attitude = new Quaternion();
        private Boolean _ccw = false;


        public XYZData Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Boolean Ccw
        {
            get { return _ccw; }
            set { _ccw = value; }
        }
   
        public Quaternion Attitude
        {
            get { return _attitude; }
            set { _attitude = value; }
        }

      

        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(_position.GetBytes());
            data.AddRange(_attitude.GetBytes());
            data.AddRange(BitConverter.GetBytes(_ccw));
            return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count >= 29)
            {
                _position.SetBytes(bytes.GetRange(0, 12));
                _attitude.SetBytes(bytes.GetRange(12, 16));
                _ccw = BitConverter.ToBoolean(bytes.ToArray(), 28);
            }
        }
        public static MotorData FromBytes(List<byte> bytes)
        {
            var c = new MotorData();
            c.SetBytes(bytes);
            return c;
        }
       
    }
}
