using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
    /// The full configuration for KFLY
    /// </summary>
    public class KFLyConfiguration: IKFlyConfigurationData
    {
        public SensorCalibrationData SensorCalibration;

        public AttitudeControllerData AttitudeController;

        
    }
}
