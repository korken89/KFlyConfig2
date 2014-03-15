using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace KFly
{

    /// <summary>
    /// A KFlyConfigurationData
    /// </summary>
    public class KFlyConfigurationData : IKFlyConfigurationData, INotifyPropertyChanged
    {

        private Boolean _isInSync = false;
        public Boolean IsInSync
        {
            get { return _isInSync; }
            set 
            { 
                _isInSync = value;
                NotifyPropertyChanged("IsInSync");
            }
        }

        public virtual Boolean IsValid
        {
            get { return true; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
