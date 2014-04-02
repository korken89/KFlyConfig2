using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class GetRCCalibration : KFlyCommand
    {
        private RCCalibrationData _data;

        public GetRCCalibration() : base(KFlyCommandType.GetRCCalibration)
        {
        }

        public override void ParseData(List<byte> data)
        {
            _data = RCCalibrationData.FromBytes(data);
        }

        public RCCalibrationData Data
        {
            get { return _data; }
        }
    }
}
