using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace KFly
{
    public class SixPointsCalibrationData: INotifyPropertyChanged
    {
        public enum SubSteps
        {
            NotStarted = 0,
            Working = 1,
            Error = 2,
            Finished = 3
        }

        private uint _currentStep = 0;

        private SubSteps[] _subSteps = new SubSteps[7];
        private ConcurrentBag<RawSensorData>[] _data = new ConcurrentBag<RawSensorData>[7];


        private SensorCalibrationData _currentResult = new SensorCalibrationData();

        public SensorCalibrationData CurrentResult
        {
            get { return _currentResult; }
            set 
            { 
                _currentResult = value;
                NotifyPropertyChanged("CurrentResult");
            }
        }

        public void FullReset()
        {
            for (int i = 0; i < 7; i++)
            {
                _subSteps[i] = SubSteps.NotStarted;
                _data[i] = new ConcurrentBag<RawSensorData>();
            }
            _currentStep = 0;
        }

        public IEnumerable<RawSensorData> RawData
        {
            get
            {
                for (int i = 0; i < 6; i++)
                {
                    foreach (RawSensorData rsd in _data[i])
                    {
                        yield return rsd;
                    }
                }
            }
        }

        public SubSteps[] Subs
        {
            get
            {
                return _subSteps;
            }
        }

        public SixPointsCalibrationData()
        {
            CurrentResult = new SensorCalibrationData();
            for (var i = 0; i < 7; i++)
            {
                _subSteps[i] = SubSteps.NotStarted;
                _data[i] = new ConcurrentBag<RawSensorData>();
            }
        }

        public SubSteps CurrentSubStep
        {
            get
            {
                return _subSteps[_currentStep];
            }
            set
            {
                _subSteps[_currentStep] = value;
            }
        }

        public uint CurrentStep
        {
            get
            {
                return _currentStep;
            }
            set
            {
                _currentStep = Math.Min(6, value);
            }
        }

        public ConcurrentBag<RawSensorData> CurrentDataBag
        {
            get
            {
                return _data[_currentStep];
            }
        }


        private void ClearDataBag(ConcurrentBag<RawSensorData> bag)
        {
            RawSensorData someItem;
            while (!bag.IsEmpty)
            {
                bag.TryTake(out someItem);
            }
        }

        public void ClearCurrentDataBag()
        {
            ClearDataBag(_data[_currentStep]);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
