using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    class SetChannelMix : KFlyCommand
    {
        private MixerData _data;

        public SetChannelMix(MixerData data) : base(KFlyCommandType.SetChannelMix)
        {
            _data = data;
        }

        public override List<Byte> ToTx()
        {
            return CreateTxWithHeader(_data.ToBytes());
        }
    }
}
