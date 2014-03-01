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

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


    }
}
