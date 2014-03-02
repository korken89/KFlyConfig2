using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class SensorCalibration
    {
        private XYZData _accelerometer_bias = new XYZData();
        private XYZData _accelerometer_gain = new XYZData();
        private XYZData _magnometer__bias = new XYZData();
        private XYZData _magnometer__gain = new XYZData();

        private DateTime _calibrationTime = DateTime.Now;

        

        //Client only
        private XYZData _accelerometer_std_dev = new XYZData();
        private XYZData _magnometer_std_dev = new XYZData();
        
        public XYZData AccelerometerBias
        {
            get
            {
                return _accelerometer_bias;
            }
        }

        public DateTime CalibrationTime
        {
            get { return _calibrationTime; }
            set { _calibrationTime = value; }
        }
        
        public XYZData AccelerometerGain
        {
            get
            {
                return _accelerometer_gain;
            }
        }

        public XYZData AccelerometerStdDev
        {
            get
            {
                return _accelerometer_std_dev;
            }
        }

        public XYZData MagnometerBias
        {
            get
            {
                return _magnometer__bias;
            }
        }

        public XYZData MagnometerGain
        {
            get
            {
                return _magnometer__gain;
            }
        }
        public XYZData MagnometerStdDev
        {
            get
            {
                return _magnometer_std_dev;
            }
        }

        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(AccelerometerBias.GetBytes());
            data.AddRange(AccelerometerGain.GetBytes());
            data.AddRange(MagnometerBias.GetBytes());
            data.AddRange(MagnometerGain.GetBytes());
            data.AddRange(BitConverter.GetBytes(CalibrationTime.ToUnixTimestamp()));
            return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count >= 52)
            {
                AccelerometerBias.SetBytes(bytes.GetRange(0, 12));
                AccelerometerGain.SetBytes(bytes.GetRange(12, 12));
                MagnometerBias.SetBytes(bytes.GetRange(24, 12));
                MagnometerGain.SetBytes(bytes.GetRange(36, 12));
                CalibrationTime = BitConverter.ToInt32(bytes.ToArray(), 48).ToDateTime();
            }
        }

        public Boolean IsValid
        {
            get
            {
                return AccelerometerBias.IsValid && AccelerometerGain.IsValid &&
                    MagnometerBias.IsValid && MagnometerGain.IsValid;
            }
        }


        public static SensorCalibration FromBytes(List<byte> bytes)
        {
            var c = new SensorCalibration();
            c.SetBytes(bytes);
            return c;
        }

        public override string ToString()
        {
            return String.Format("AccBias: {0}\nAccGain: {1}\nMagBias: {2}\nMagGain: {3}", 
                AccelerometerBias, AccelerometerGain, MagnometerBias, MagnometerGain);
        }
       
    }
}
