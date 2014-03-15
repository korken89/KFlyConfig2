using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace KFly
{

    /// <summary>
    /// A KFlyGroupConfigurationData includes one or several other configurationdata objects
    /// </summary>
    public class KFlyGroupConfigurationData : IKFlyConfigurationData
    {
        private Dictionary<String, IKFlyConfigurationData> _observedProperties = new Dictionary<string, IKFlyConfigurationData>();

        public KFlyGroupConfigurationData()
        {
            foreach (PropertyInfo pi in KFlySyncCheck.GetAllSyncProperties(this.GetType()))
            {
                IKFlyConfigurationData data = pi.GetValue(this) as IKFlyConfigurationData;
                if (data != null)
                {
                    data.PropertyChanged += OnSubPropertyChanged;
                }
                _observedProperties.Add(pi.Name, data);
            }
            UpdateSyncStatus();
            UpdateValidStatus();
        }

        private Boolean _isInSync = false;
        public Boolean IsInSync
        {
            get { return _isInSync; }
        }

        private Boolean _isValid = false;
        public virtual Boolean IsValid
        {
            get { return _isValid; }
        }


        private void HandleSubKFlyConfigurationData(String property, IKFlyConfigurationData newData)
        {
            IKFlyConfigurationData oldData = _observedProperties[property];
            if (oldData != newData)
            {
                if (oldData != null)
                {
                    oldData.PropertyChanged -= OnSubPropertyChanged;
                }
                if (newData != null)
                {
                    newData.PropertyChanged += OnSubPropertyChanged;
                }
                UpdateSyncStatus();
                UpdateValidStatus();
            }
        }

        private void OnSubPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "IsInSync")
            {
                UpdateSyncStatus();
            }
            if (args.PropertyName == "IsValid")
            {
                UpdateValidStatus();
            }
        }

        private void UpdateSyncStatus()
        {
            bool isInSync = true;
            foreach (IKFlyConfigurationData kcd in _observedProperties.Values)
            {
                if (kcd != null)
                {
                    isInSync = isInSync && kcd.IsInSync;
                }
            }
            if (_isInSync != isInSync)
            {
                _isInSync = isInSync;
                NotifyPropertyChanged("IsInSync");
            }

        }

        protected virtual void UpdateValidStatus()
        {
            bool isValid = true;
            foreach (IKFlyConfigurationData kcd in _observedProperties.Values)
            {
                if (kcd != null)
                {
                    isValid = isValid && kcd.IsValid;
                }
            }
            if (_isValid != isValid)
            {
                _isValid = isValid;
                NotifyPropertyChanged("IsValid");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName, IKFlyConfigurationData value)
        {

            HandleSubKFlyConfigurationData(propName, value);

            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

          public void NotifyPropertyChanged(string propName)
        {
           if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
