using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class Ping : KFlyCommand
    {
        public Ping() : base(KFlyCommandType.Ping)
        {
        }
    }
}
