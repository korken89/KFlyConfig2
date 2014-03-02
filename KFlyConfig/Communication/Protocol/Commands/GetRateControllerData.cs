using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
   
    /// </summary>
    public class GetRateControllerData : KFlyCommand
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
      
        public GetRateControllerData() : 
            base(KFlyCommandType.GetRateControllerData)
        {}

        public override void ParseData(List<byte> bytes)
        {
            if (bytes.Count >= 48)
            {
                _data.SetBytes(bytes.GetRange(0, 36));
                _rateLimit.SetBytes(bytes.GetRange(36, 12));
            }
        }

        public override List<Byte> ToTx()
        {
            return CreateTxWithHeader(new List<byte>()); //Do not send data from client
        }
    }
}
