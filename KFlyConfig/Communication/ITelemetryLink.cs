using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFly
{
    public interface ITelemetryLink
    {
        void HandleReceived(KFlyCommand cmd);
    }
}
