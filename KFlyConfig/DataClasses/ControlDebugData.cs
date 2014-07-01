using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KFly
{
    /*
     {
        vector3f_t rate_reference;
        vector3f_t rate_measured;
        struct {
            float pitch;
            float roll;
            float yaw;
            float throttle;
        } actuator_desired;
        struct {
            float m1;
            float m2;
            float m3;
            float m4;
        } pwm_to_motors;
     } Control_Debug;
     */

    public class ControlDebugData: KFlyConfigurationData
    {
        private XYZData _rateReference;

        public XYZData RateReference
        {
            get { return _rateReference; }
            set { _rateReference = value; NotifyPropertyChanged("RateReference"); }
        }
        private XYZData _rateMeasured;

        public XYZData RateMeasured
        {
            get { return _rateMeasured; }
            set { _rateMeasured = value; NotifyPropertyChanged("RateMeasured"); }
        }
        private PRYTData _actuatorDesired;

        public PRYTData ActuatorDesired
        {
            get { return _actuatorDesired; }
            set { _actuatorDesired = value; NotifyPropertyChanged("ActuatorDesired"); }
        }
        private PRYTData _pwm_to_motors; //pitch = m1 ... and so on

        public PRYTData PwmToMotors
        {
            get { return _pwm_to_motors; }
            set { _pwm_to_motors = value; NotifyPropertyChanged("PwmToMotors"); }
        }


        public ControlDebugData()
        {
            _rateReference = new XYZData();
            _rateMeasured = new XYZData();
            _actuatorDesired = new PRYTData();
            _pwm_to_motors = new PRYTData();
        }

       

        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(RateReference.GetBytes());
            data.AddRange(RateMeasured.GetBytes());
            data.AddRange(ActuatorDesired.GetBytes());
            data.AddRange(PwmToMotors.GetBytes());
            return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count >= 36)
            {
                RateReference.SetBytes(bytes.GetRange(0, 12));
                RateMeasured.SetBytes(bytes.GetRange(12, 12));
                ActuatorDesired.SetBytes(bytes.GetRange(24, 16));
                PwmToMotors.SetBytes(bytes.GetRange(40, 16));
            }
        }
        public static ControlDebugData FromBytes(List<byte> bytes)
        {
            var c = new ControlDebugData();
            c.SetBytes(bytes);
            return c;
        }
    }
}
