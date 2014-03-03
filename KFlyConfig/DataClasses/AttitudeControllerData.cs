using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
    /// The full configuration for KFLY
    /// </summary>
    public class AttitudeControllerData: IKFlyConfigurationData
    {
        public ControllerData Attitude;

        public PRData AngleConstraints;
        
        public PRYData RateContstraints;
    }
}
