using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class CalibrateRCCenters : KFlyCommand
    {
        public CalibrateRCCenters() : base(KFlyCommandType.CalibrateRCCenters)
        {
        }
    }
}
