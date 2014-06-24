using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class SaveStatusChanged: KFlyCommand
    {
        public Boolean Saved;
        public SaveStatusChanged(Boolean saved)
            : base(KFlyCommandType.SaveStatusChanged)
        {
            Saved = saved;
        }
    }
}
