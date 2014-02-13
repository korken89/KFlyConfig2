using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class StartRCCalibration : KFlyCommand
    {
        public StartRCCalibration() : base(KFlyCommandType.StartRCCalibration)
        {
        }
    }
}
