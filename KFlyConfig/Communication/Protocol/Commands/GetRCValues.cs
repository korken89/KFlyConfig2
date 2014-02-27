using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class GetRCValues : KFlyCommand
    {
        private List<byte> _data;

        public GetRCValues() : base(KFlyCommandType.GetRCValues)
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
