using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
    /// Dummy class for not implemented messages
    /// </summary>
    public class CmdCollection : KFlyCommand
    {
        public IEnumerable<KFlyCommand> Cmds;

        public CmdCollection(IEnumerable<KFlyCommand> cmds) : base(KFlyCommandType.None)
        {
            Cmds = cmds;
        }
        public CmdCollection(params KFlyCommand[] cmds)
            : base(KFlyCommandType.None)
        {
            Cmds = cmds;
        }

        public override String ToString()
        {
            String res = "Cmd collection: ";
            foreach (KFlyCommand cmd in Cmds)
            {
                res += "\n" + cmd.ToString();
            }
            return res;
        }
    }
}
