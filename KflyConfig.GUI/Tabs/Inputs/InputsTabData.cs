using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace KFly.GUI
{
    public class InputsTabData: INotifyPropertyChanged
    {
        private ArmingData _armingData = new ArmingData();

        public ArmingData ArmingData
        {
            get { return _armingData; }
            set { _armingData = value; this.NotifyPropertyChanged("ArmingData"); }
        }


        private RCCalibrationData _calibrationData = new RCCalibrationData();

        public RCCalibrationData CalibrationData
        {
            get { return _calibrationData; }
            set { _calibrationData = value; this.NotifyPropertyChanged("CalibrationData"); }
        }


   
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
