using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    class SetRegulatorData : KFlyCommand
    {
        private RegulatorData _data;

        public SetRegulatorData(RegulatorData data) : base(KFlyCommandType.SetRegulatorData)
        {
            _data = data;
        }

        public override List<Byte> ToTx()
        {
            return CreateTxWithHeader(_data.ToBytes());
        }
    }
}
