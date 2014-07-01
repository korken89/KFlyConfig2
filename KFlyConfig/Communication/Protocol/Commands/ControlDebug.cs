using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
    /// </summary>
    public class ControlDebug : KFlyCommand
    {

        public ControlDebugData Data;

        public ControlDebug() : 
            base(KFlyCommandType.ControlDebug)
        {}

        public override void ParseData(List<byte> data)
        {
            Data = ControlDebugData.FromBytes(data);
        }

        public override List<Byte> ToTx()
        {
            if (Data == null)
                return CreateTxWithHeader(new List<byte>());
            else
                return CreateTxWithHeader(Data.GetBytes());
        }
    }
}
