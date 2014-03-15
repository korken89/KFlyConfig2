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

        public KFLyConfiguration(Boolean liveUpdate)
        {
            LiveUpdate = liveUpdate;
        }

        private bool _liveUpdate = false;

        public bool LiveUpdate
        {
            get { return _liveUpdate; }
            set 
            {
                if (value != _liveUpdate)
                {
                    if (_liveUpdate)
                        unsubscribe();
                    _liveUpdate = value;
                    if (_liveUpdate)
                        subscribe();
                }
            }
        }
        private List<TeleSubscription> _subscriptions = new List<TeleSubscription>();
        private void subscribe()
        {
            _subscriptions.Add(Telemetry.Subscribe(KFlyCommandType.GetDeviceInfo, (GetDeviceInfo gdi) =>
                {
                    KFly = gdi.Data;
                }));
        }
        private void unsubscribe()
        {
            foreach (TeleSubscription sub in _subscriptions)
            {
                Telemetry.Unsubscribe(sub);
            }
            _subscriptions.Clear();
        }


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
