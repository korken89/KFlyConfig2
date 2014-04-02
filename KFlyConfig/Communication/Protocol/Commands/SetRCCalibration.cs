using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    class SetRCCalibration : KFlyCommand
    {
        private RCCalibrationData _data;

        public SetRCCalibration(RCCalibrationData data)
            : base(KFlyCommandType.SetRCCalibration)
        {
            _data = data;
        }

        public override List<Byte> ToTx()
        {
            return CreateTxWithHeader(_data.GetBytes());
        }
    }
}
