using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class ConnectionStatusChanged: KFlyCommand
    {
        public ConnectionStatus Status;
        public ConnectionStatusChanged(ConnectionStatus status)
            : base(KFlyCommandType.ConnectionStatusChanged)
        {
            Status = status;
        }

        public bool IsConnected
        {
            get
            {
                return Status == ConnectionStatus.Connected;
            }
        }
    }
}
