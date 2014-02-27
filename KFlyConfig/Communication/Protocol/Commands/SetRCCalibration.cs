using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    class SetRCCalibration : KFlyCommand
    {
        private ChannelData _data;

        public SetRCCalibration(ChannelData data) : base(KFlyCommandType.SetRCCalibration)
        {
            _data = data;
        }

        public override List<Byte> ToTx()
        {
            return CreateTxWithHeader(_data.ToBytes());
        }
    }
}
