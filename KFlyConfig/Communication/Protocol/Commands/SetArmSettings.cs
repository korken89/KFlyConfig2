using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{

    public class SetArmSettings : KFlyCommand
    {

        public ArmingData Data;

        public SetArmSettings(ArmingData data) : 
            base(KFlyCommandType.SetArmSettings)
        {
            Data = data;
        }

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
