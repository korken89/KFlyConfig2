using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class MixerData : KFlyConfigurationData
    {
         private static int MAX_NUMBER_OF_OUTPUTS = 8;

         private Int16[] _throttle = new Int16[MAX_NUMBER_OF_OUTPUTS];

         public Int16[] Throttle
         {
             get { return _throttle; }
             set { _throttle = value; NotifyPropertyChanged("Throttle"); }
         }

         private Int16[] _pitch = new Int16[MAX_NUMBER_OF_OUTPUTS];

         public Int16[] Pitch
         {
             get { return _pitch; }
             set { _pitch = value; NotifyPropertyChanged("Pitch"); }
         }

         private Int16[] _roll = new Int16[MAX_NUMBER_OF_OUTPUTS];

         public Int16[] Roll
         {
             get { return _roll; }
             set { _roll = value; NotifyPropertyChanged("Roll"); }
         }


         private Int16[] _yaw = new Int16[MAX_NUMBER_OF_OUTPUTS];

         public Int16[] Yaw
         {
             get { return _yaw; }
             set { _yaw = value; NotifyPropertyChanged("Yaw"); }
         }

        public MixerData()
         {

         }


        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            foreach (Int16 value in Throttle)
            {
                float v = value / 1000;
                data.AddRange(BitConverter.GetBytes(v));
            }
            foreach (Int16 value in Pitch)
            {
                float v = value / 1000;
                data.AddRange(BitConverter.GetBytes(v));
            }
            foreach (Int16 value in Roll)
            {
                float v = value / 1000;
                data.AddRange(BitConverter.GetBytes(v));
            }
            foreach (Int16 value in Yaw)
            {
                float v = value / 1000;
                data.AddRange(BitConverter.GetBytes(v));
            } return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count >= MAX_NUMBER_OF_OUTPUTS * 16)
            {
                byte[] b = bytes.ToArray();
                int offset = 0;
                for (var i = 0; i < MAX_NUMBER_OF_OUTPUTS; i++)
                {
                    float v = BitConverter.ToSingle(b, i * 4 + offset);
                    Throttle[i] = Convert.ToInt16((v * 1000));
                }
                offset += MAX_NUMBER_OF_OUTPUTS * 2;
                for (var i = 0; i < MAX_NUMBER_OF_OUTPUTS; i++)
                {
                    float v = BitConverter.ToSingle(b, i * 4 + offset);
                    Pitch[i] = Convert.ToInt16((v * 1000));
                }
                offset += MAX_NUMBER_OF_OUTPUTS * 2;
                for (var i = 0; i < MAX_NUMBER_OF_OUTPUTS; i++)
                {
                    float v = BitConverter.ToSingle(b, i * 4 + offset);
                    Roll[i] = Convert.ToInt16((v * 1000));
                }
                offset += MAX_NUMBER_OF_OUTPUTS * 2;
                for (var i = 0; i < MAX_NUMBER_OF_OUTPUTS; i++)
                {
                    float v = BitConverter.ToSingle(b, i * 4 + offset);
                    Yaw[i] = Convert.ToInt16((v * 1000));
                }
              
            }
        }

        public override Boolean IsValid
        {
            get
            {
                return true;
            }
        }

        public static MixerData FromBytes(List<byte> bytes)
        {
            var c = new MixerData();
            c.SetBytes(bytes);
            return c;
        }
    }
}
