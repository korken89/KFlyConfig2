using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class RawSensorData
    {
        private XYZDataInt16 _accelerometer = new XYZDataInt16();
        private XYZDataInt16 _gyro = new XYZDataInt16();
        private XYZDataInt16 _magnometer = new XYZDataInt16();
        private Int32 _pressure = 0;

        public XYZDataInt16 Accelerometer
        {
            get
            {
                return _accelerometer;
            }
        }

        public XYZDataInt16 Gyro
        {
            get
            {
                return _gyro;
            }
        }

        public XYZDataInt16 Magnometer
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
            if (bytes.Count >= 22)
            {
                Accelerometer.SetBytes(bytes.GetRange(0, 6));
                Gyro.SetBytes(bytes.GetRange(6, 6));
                Magnometer.SetBytes(bytes.GetRange(12, 6));
                _pressure = BitConverter.ToInt32(bytes.ToArray(), 18);
            }
        }
        public static RawSensorData FromBytes(List<byte> bytes)
        {
            var c = new RawSensorData();
            c.SetBytes(bytes);
            return c;
        }
       
    }
}
