using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class ConnectionStatistics: KFlyCommand
    {
        public ulong BytesIn;
        public ulong BytesOut;
        public ConnectionStatistics(ulong bIn, ulong bOut)
            : base(KFlyCommandType.ConnectionStatistics)
        {
            BytesIn = bIn;
            BytesOut = bOut;
        }
    }
}
