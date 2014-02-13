using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    class SetControllerData : KFlyCommand
    {
        private ControllerData _data;

        public SetControllerData(ControllerData data) : base(KFlyCommandType.SetControllerData)
        {
            _data = data;
        }

        public override List<Byte> ToTx()
        {
            return CreateTxWithHeader(_data.ToBytes());
        }
    }
}
