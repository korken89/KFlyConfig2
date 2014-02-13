using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class GetRCCalibration : KFlyCommand
    {
        private List<byte> _data;

        public GetRCCalibration() : base(KFlyCommandType.GetRCCalibration)
        {
        }

        public override void ParseData(List<byte> data)
        {
            _data = data;
        }

        public List<byte> Data
        {
            get { return _data; }
        }
    }
}
