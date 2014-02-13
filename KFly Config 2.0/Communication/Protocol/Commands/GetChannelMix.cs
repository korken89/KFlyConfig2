using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class GetChannelMix : KFlyCommand
    {
        private List<byte> _data;

        public GetChannelMix() : base(KFlyCommandType.GetChannelMix)
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
