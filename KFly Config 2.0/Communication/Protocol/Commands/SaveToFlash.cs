using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class SaveToFlash : KFlyCommand
    {
        public SaveToFlash() : base(KFlyCommandType.SaveToFlash)
        {
        }
    }
}
