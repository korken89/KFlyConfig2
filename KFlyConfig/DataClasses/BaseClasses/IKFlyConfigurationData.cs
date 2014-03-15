using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace KFly
{
    /// <summary>
    /// 
    /// </summary>
    public interface IKFlyConfigurationData : INotifyPropertyChanged
    {
        /// <summary>
        /// Is the current data in sync with card
        /// </summary>
        Boolean IsInSync
        {
            get;
        }

        /// <summary>
        /// Is the current data valid for flight (as much as we can check theoretical)
        /// </summary>
        Boolean IsValid
        {
            get;
        }

    }
}
