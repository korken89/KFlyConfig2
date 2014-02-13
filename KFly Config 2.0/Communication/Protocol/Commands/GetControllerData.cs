using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    class GetControllerData : KFlyCommand
    {

        private List<byte> _data;

        public GetControllerData() : base(KFlyCommandType.GetControllerData)
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
