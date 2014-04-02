﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class RCCalibrationData : KFlyConfigurationData
    {
        private static int MAX_NUMBER_OF_INPUTS = 12;

        private RCInputMode _inputMode = RCInputMode.PWM_INPUT;

        public RCInputMode InputMode
        {
            get { return _inputMode; }
            set { _inputMode = value; NotifyPropertyChanged("InputMode"); }
        }

        private RCInputRole[] _chRoles = new RCInputRole[MAX_NUMBER_OF_INPUTS];

        public RCInputRole[] ChRoles
        {
            get { return _chRoles; }
            set { _chRoles = value; NotifyPropertyChanged("ChRoles"); }
        }

        private RCInputType[] _chTypes = new RCInputType[MAX_NUMBER_OF_INPUTS];

        public RCInputType[] ChTypes
        {
            get { return _chTypes; }
            set { _chTypes = value; NotifyPropertyChanged("ChTypes"); }
        }

        private UInt16[] _chCenters = new UInt16[MAX_NUMBER_OF_INPUTS];

        public UInt16[] ChCenters
        {
            get { return _chCenters; }
            set { _chCenters = value; NotifyPropertyChanged("ChCenters"); }
        }

        private UInt16[] _chTops = new UInt16[MAX_NUMBER_OF_INPUTS];

        public UInt16[] ChTops
        {
            get { return _chTops; }
            set { _chTops = value; NotifyPropertyChanged("ChTops"); }
        }


        private UInt16[] _chBottoms = new UInt16[MAX_NUMBER_OF_INPUTS];

        public UInt16[] ChBottoms
        {
            get { return _chBottoms; }
            set { _chBottoms = value; NotifyPropertyChanged("ChBottoms"); }
        }

      

        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.Add((byte)InputMode);
            foreach (RCInputRole rir in ChRoles)
            {
                data.Add((byte)rir);
            }
            foreach (RCInputType rit in ChTypes)
            {
                data.Add((byte)rit);
            }
            foreach (UInt16 value in ChCenters)
            {
                data.AddRange(BitConverter.GetBytes(value));
            }
            foreach (UInt16 value in ChTops)
            {
                data.AddRange(BitConverter.GetBytes(value));
            }
            foreach (UInt16 value in ChBottoms)
            {
                data.AddRange(BitConverter.GetBytes(value));
            }
            return data;
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count >= 1+ MAX_NUMBER_OF_INPUTS*8)
            {
                byte[] b = bytes.ToArray();
                InputMode = (RCInputMode)b[0];
                int offset = 1;
                for (var i = 0; i < MAX_NUMBER_OF_INPUTS; i++)
                {
                    ChRoles[i] = (RCInputRole)b[i+offset];
                }
                offset += MAX_NUMBER_OF_INPUTS;
                for (var i = 0; i < MAX_NUMBER_OF_INPUTS; i++)
                {
                    ChTypes[i] = (RCInputType)b[i+offset];
                }
                offset += MAX_NUMBER_OF_INPUTS;
                for (var i = 0; i < MAX_NUMBER_OF_INPUTS; i++)
                {
                    ChCenters[i] = BitConverter.ToUInt16(b, i + offset);
                }
                offset += MAX_NUMBER_OF_INPUTS*2;
                for (var i = 0; i < MAX_NUMBER_OF_INPUTS; i++)
                {
                    ChTops[i] = BitConverter.ToUInt16(b, i + offset);
                }
                offset += MAX_NUMBER_OF_INPUTS * 2;
                for (var i = 0; i < MAX_NUMBER_OF_INPUTS; i++)
                {
                    ChBottoms[i] = BitConverter.ToUInt16(b, i + offset);
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


        public static RCCalibrationData FromBytes(List<byte> bytes)
        {
            var c = new RCCalibrationData();
            c.SetBytes(bytes);
            return c;
        }

        public override string ToString()
        {
            return base.ToString();
            // return String.Format("AccBias: {0}\nAccGain: {1}\nMagBias: {2}\nMagGain: {3}", 
            //     AccelerometerBias, AccelerometerGain, MagnometerBias, MagnometerGain);
        }

    }

}