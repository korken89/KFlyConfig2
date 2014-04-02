using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class SensorData
    {
        private XYZData _accelerometer = new XYZData();
        private XYZData _gyro = new XYZData();
        private XYZData _magnometer = new XYZData();
        private Int32 _pressure = 0;

        public XYZData Accelerometer
        {
            get
            {
                return _accelerometer;
            }
        }

        public XYZData Gyro
        {
            get
            {
                return _gyro;
            }
        }

        public XYZData Magnometer
        {
            get
            {
                return _magnometer;
            }
        }

        public Int32 Pressure
        {
            get
            {
                return _pressure;
            }
        }

        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(Accelerometer.GetBytes());
            data.AddRange(Gyro.GetBytes());
            data.AddRange(Magnometer.GetBytes());
            data.AddRange(BitConverter.GetBytes(Pressure));
            return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count >= 40)
            {
                Accelerometer.SetBytes(bytes.GetRange(0, 12));
                Gyro.SetBytes(bytes.GetRange(12, 12));
                Magnometer.SetBytes(bytes.GetRange(24, 12));
                _pressure = BitConverter.ToInt32(bytes.ToArray(), 36);
            }
        }
        public static SensorData FromBytes(List<byte> bytes)
        {
            var c = new SensorData();
            c.SetBytes(bytes);
            return c;
        }
       
    }
}
