using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class ControllerData
    {
        private PIData _pitch = new PIData();
        private PIData _roll = new PIData();
        private PIData _yaw = new PIData();

        public PIData Pitch
        {
            get
            {
                return _pitch;
            }
        }

        public PIData Roll
        {
            get
            {
                return _roll;
            }
        }

        public PIData Yaw
        {
            get
            {
                return _yaw;
            }
        }



        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(Pitch.GetBytes());
            data.AddRange(Roll.GetBytes());
            data.AddRange(Yaw.GetBytes());
            return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count >= 36)
            {
                Pitch.SetBytes(bytes.GetRange(0, 12));
                Roll.SetBytes(bytes.GetRange(12, 12));
                Yaw.SetBytes(bytes.GetRange(24, 12));
            }
        }
        public static ControllerData FromBytes(List<byte> bytes)
        {
            var c = new ControllerData();
            c.SetBytes(bytes);
            return c;
        }
       
    }
}
