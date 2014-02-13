using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    class GetRegulatorData : KFlyCommand
    {

        private List<byte> _data;

        public GetRegulatorData() : base(KFlyCommandType.GetRegulatorData)
        {
        }

        public override void ParseRx(List<byte> data)
        {
            _data = data;
        }

        public List<byte> Data
        {
            get { return _data; }
        }
    }
}
