using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KFly
{
    public class ControllerData: INotifyPropertyChanged
    {
        private PIData _pitch;
        private PIData _roll;
        private PIData _yaw;

        public ControllerData()
        {
            _pitch = new PIData();
            _pitch.PropertyChanged += _pitchroll_PropertyChanged;
            _roll = new PIData();
            _roll.PropertyChanged += _pitchroll_PropertyChanged;
            _yaw = new PIData();
        }

        void _pitchroll_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_lockPitchRoll)
            {
                PIData target = (sender == _pitch) ? _roll : _pitch;
                PIData source = sender as PIData;
                switch (e.PropertyName)
                {
                    case "PGain":
                        target.PGain = source.PGain;
                        break;
                    case "IGain":
                        target.IGain = source.IGain;
                        break;
                    case "ILimit":
                        target.ILimit = source.ILimit;
                        break;
                }
            }
        }

        private bool _lockPitchRoll = false;

        public bool LockPitchRoll
        {
            get { return _lockPitchRoll; }
            set 
            { 
                _lockPitchRoll = value;
                if (value)
                {
                    _roll.PGain = _pitch.PGain;
                    _roll.IGain = _pitch.IGain;
                    _roll.ILimit = _pitch.ILimit;
                }
            }
        }


        public PIData Pitch
        {
            get
            {
                return _pitch;
            }
            set
            {
                if (_pitch != value)
                {
                    if (_pitch != null)
                        _pitch.PropertyChanged -= _pitchroll_PropertyChanged;
                    _pitch = value;
                    _pitch.PropertyChanged += _pitchroll_PropertyChanged;
                    this.NotifyPropertyChanged("Pitch");
                }
            }
        }

        public PIData Roll
        {
            get
            {
                return _roll;
            }
            set
            {
                if (_roll != value)
                {
                    if (_roll != null)
                        _roll.PropertyChanged -= _pitchroll_PropertyChanged;
                    _roll = value;
                    _roll.PropertyChanged += _pitchroll_PropertyChanged;
                    this.NotifyPropertyChanged("Roll");
                }
            }
        }

        public PIData Yaw
        {
            get
            {
                return _yaw;
            }
            set
            {
                _yaw = value;
                this.NotifyPropertyChanged("Yaw");
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
                _lockPitchRoll = ((Pitch.PGain == Roll.PGain) && (Pitch.IGain == Roll.IGain) && (Pitch.ILimit == Roll.ILimit));
            }
        }
        public static ControllerData FromBytes(List<byte> bytes)
        {
            var c = new ControllerData();
            c.SetBytes(bytes);
            return c;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
       
    }
}
