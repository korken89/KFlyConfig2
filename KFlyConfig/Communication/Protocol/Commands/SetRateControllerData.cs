using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
    /// Same for 
    /// </summary>
    public class SetRateControllerData : KFlyCommand
    {
        private ControllerData _data = new ControllerData();

        public ControllerData Data
        {
            get { return _data; }
            set { _data = value; }
        }
        private PRYData _rateLimit = new PRYData();

        public PRYData RateLimit
        {
            get { return _rateLimit; }
            set { _rateLimit = value; }
        }



        public SetRateControllerData() : 
            base(KFlyCommandType.SetRateControllerData)
        {
        }

        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(Data.GetBytes());
            data.AddRange(RateLimit.GetBytes());
            return data;
        }

        public override List<Byte> ToTx()
        {
            var data = GetBytes();
            return CreateTxWithHeader(data);
        }
    }
}
