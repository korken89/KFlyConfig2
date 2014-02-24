using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class ConnectionStatusChanged: KFlyCommand
    {
        public Boolean Connected = false;

       
        public ConnectionStatusChanged()
            : base(KFlyCommandType.ConnectionStatusChanged)
        {
        }
    }
}
