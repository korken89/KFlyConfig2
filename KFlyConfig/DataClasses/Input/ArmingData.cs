using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class ArmingData : KFlyConfigurationData
    {
        private UInt16 _stickThreshold;

        public UInt16 StickThreshold
        {
            get { return _stickThreshold; }
            set { _stickThreshold = value; NotifyPropertyChanged("StickThreshold"); }
        }

        private UInt16 _armedMinThrottle;

        public UInt16 ArmedMinThrottle
        {
            get { return _armedMinThrottle; }
            set { _armedMinThrottle = value; NotifyPropertyChanged("ArmedMinThrottle"); }
        }


        private ArmingStickDirection _stickDirection = ArmingStickDirection.STICK_NONE;

        public ArmingStickDirection StickDirection
        {
            get { return _stickDirection; }
            set { _stickDirection = value; NotifyPropertyChanged("StickDirection"); }
        }

        private UInt16 _armStickTime;

        public UInt16 ArmStickTime
        {
            get { return _armStickTime; }
            set { _armStickTime = value; NotifyPropertyChanged("ArmStickTime"); }
        }

        private UInt16 _armZeroThrottleTimeout;

        public UInt16 ArmZeroThrottleTimeout
        {
            get { return _armZeroThrottleTimeout; }
            set { _armZeroThrottleTimeout = value; NotifyPropertyChanged("ArmZeroThrottleTimeout"); }
        }



        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            float v = ((float)_stickThreshold) / 100.0f;
            data.AddRange(BitConverter.GetBytes(v));
            v = ((float)_armedMinThrottle) / 100.0f;
            data.AddRange(BitConverter.GetBytes(v));
            data.Add(Convert.ToByte(_stickDirection));
            data.Add(Convert.ToByte(_armStickTime));
            data.Add(Convert.ToByte(_armZeroThrottleTimeout));
            return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count >= 11)
            {
                byte[] b = bytes.ToArray();
                StickThreshold = Convert.ToUInt16(BitConverter.ToSingle(b, 0)*100);
                ArmedMinThrottle = Convert.ToUInt16(BitConverter.ToSingle(b, 4) * 100);
                StickDirection = (ArmingStickDirection)b[8];
                ArmStickTime = Convert.ToUInt16(b[9]);
                ArmZeroThrottleTimeout = Convert.ToUInt16(b[10]);
            }
        }

        public override Boolean IsValid
        {
            get
            {
                return true;
            }
        }


        public static ArmingData FromBytes(List<byte> bytes)
        {
            var c = new ArmingData();
            c.SetBytes(bytes);
            return c;
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }

}