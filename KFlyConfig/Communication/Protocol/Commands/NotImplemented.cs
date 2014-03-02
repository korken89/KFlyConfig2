using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
    /// Dummy class for not implemented messages
    /// </summary>
    public class NotImplemented : KFlyCommand
    {
        public NotImplemented(KFlyCommandType type)
            : base(type)
        {
        }

        public override String ToString()
        {
            return "Not implemented: " + Enum.GetName(typeof(KFlyCommandType), Type);
        }
    }
}
