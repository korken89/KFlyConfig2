using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class GetChannelMix : KFlyCommand
    {
        public MixerData Data;

        public GetChannelMix() : base(KFlyCommandType.GetChannelMix)
        {
        }

        public override void ParseData(List<byte> data)
        {
            Data = MixerData.FromBytes(data);
        }

        public override List<Byte> ToTx()
        {
            if (Data == null)
                return CreateTxWithHeader(new List<byte>());
            else
                return CreateTxWithHeader(Data.GetBytes());
        }
    }
}
