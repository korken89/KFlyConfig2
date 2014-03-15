using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KFly
{
    public class KFlyData: KFlyConfigurationData
    {
        String STM32F4DeviceId = "";
        String UserId = "";
        String FirmwareVersion = "";
        String BootloaderVersion = "";

        public override bool IsValid
        {
            get
            {
                return true; //Nothing here is important for flight
            }
        }
    }
}
