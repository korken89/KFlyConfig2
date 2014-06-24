using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
    /// A collection of several commands
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

        public override bool AffectsSettingsOnCard()
        {
            Boolean res = false;
            foreach (KFlyCommand cmd in Cmds)
            {
                res = res || cmd.AffectsSettingsOnCard();
                if (cmd.Type == KFlyCommandType.SaveToFlash)
                    res = false;
            }
            return res;
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
