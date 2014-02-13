using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class StopRCCalibration : KFlyCommand
    {
        public StopRCCalibration() : base(KFlyCommandType.StopRCCalibration)
        {
        }
    }
}
