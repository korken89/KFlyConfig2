using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{

    public class GetArmSettings : KFlyCommand
    {

        public ArmingData Data;

        public GetArmSettings() : 
            base(KFlyCommandType.GetArmSettings)
        {}

        public override void ParseData(List<byte> data)
        {
            Data = ArmingData.FromBytes(data);
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
