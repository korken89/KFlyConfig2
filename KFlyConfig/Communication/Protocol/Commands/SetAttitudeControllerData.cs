using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
    /// Same for 
    /// </summary>
    public class SetAttitudeControllerData : KFlyCommand
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
        private PRData _angleLimit = new PRData();

        public PRData AngleLimit
        {
            get { return _angleLimit; }
            set { _angleLimit = value; }
        }


        public SetAttitudeControllerData() : 
            base(KFlyCommandType.SetAttitudeControllerData)
        {
        }

        public List<byte> GetBytes()
        {
            List<byte> data = new List<byte>();
            data.AddRange(Data.GetBytes());
            data.AddRange(RateLimit.GetBytes());
            data.AddRange(AngleLimit.GetBytes());
            return data;
        }

        public override List<Byte> ToTx()
        {
            var data = GetBytes();
            return CreateTxWithHeader(data);
        }
    }
}
