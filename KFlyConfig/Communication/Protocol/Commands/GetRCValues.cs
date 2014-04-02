using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class GetRCValues : KFlyCommand
    {
        private RawRCData _data;

        public GetRCValues() : base(KFlyCommandType.GetRCValues)
        {
        }

        public override void ParseData(List<byte> data)
        {
            _data = RawRCData.FromBytes(data);
        }

        public RawRCData Data
        {
            get { return _data; }
        }
    }
}
