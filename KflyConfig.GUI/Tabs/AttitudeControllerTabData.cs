using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace KFly.GUI
{
    public class AttitudeControllerTabData: INotifyPropertyChanged
    {
        private ControllerData _adata = new ControllerData();

        public ControllerData AttitudeCData
        {
            get { return _adata; }
            set 
            { 
                _adata = value;
                this.NotifyPropertyChanged("AttitudeCData");
            }
        }

        private ControllerData _rateCData = new ControllerData();

        public ControllerData RateCData
        {
            get { return _rateCData; }
            set 
            { 
                _rateCData = value;
                this.NotifyPropertyChanged("RateCData");
            }
        }

        private LimitCollection _collection = new LimitCollection();

        public LimitCollection LimitCollection
        {
            get { return _collection; }
            set 
            { 
                _collection = value;
                this.NotifyPropertyChanged("LimitCollection");
            }
        }

       

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }

    public class LimitCollection : INotifyPropertyChanged
    {
        private PRYData _attitudeRateLimit = new PRYData();

        public PRYData AttitudeRateLimit
        {
            get { return _attitudeRateLimit; }
            set
            {
                _attitudeRateLimit = value;
                this.NotifyPropertyChanged("AttitudeRateLimit");
            }
        }

        private PRYData _rateLimit = new PRYData();

        public PRYData RateLimit
        {
            get { return _rateLimit; }
            set
            {
                _rateLimit = value;
                this.NotifyPropertyChanged("RateLimit");
            }
        }

        private PRData _angleLimit = new PRData();

        public PRData AngleLimit
        {
            get { return _angleLimit; }
            set
            {
                _angleLimit = value;
                this.NotifyPropertyChanged("AngleLimit");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
