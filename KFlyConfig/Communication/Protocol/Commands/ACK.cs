using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class Ack : KFlyCommand
    {
        public Ack() : base(KFlyCommandType.ACK)
        {
        }
    }
}
