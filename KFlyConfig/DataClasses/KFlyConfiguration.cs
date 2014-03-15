using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
    /// The full configuration for KFLY
    /// </summary>
    public class KFLyConfiguration: KFlyGroupConfigurationData
    {
        #region CardSpecific data
        //Version and Id information
        private KFlyData _kFly;

        [KFlySyncCheck]
        public KFlyData KFly
        {
            get { return _kFly; }
            set 
            {
                if (_kFly != value)
                {
                    _kFly = value;
                    NotifyPropertyChanged("KFly", value);
                }
            }
        }
      
        //Current sensorcalibration
        private SensorCalibrationData _sensorCalibration;

        [KFlySyncCheck]
        public SensorCalibrationData SensorCalibration
        {
            get { return _sensorCalibration; }
            set {
                if (_sensorCalibration != value)
                {
                    _sensorCalibration = value;
                    NotifyPropertyChanged("SensorCalibration", value);
                } 
            }
        }

        #endregion


        public AttitudeControllerData AttitudeController;

        
    }
}
