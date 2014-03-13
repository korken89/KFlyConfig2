using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
   
    /// </summary>
    public class GetEstimationAttitude: KFlyCommand
    {

        private Quaternion _data = new Quaternion();

        public Quaternion Data
        {
            get { return _data; }
            set { _data = value; }
        }
       

        public GetEstimationAttitude() : 
            base(KFlyCommandType.GetEstimationAttitude)
        {}

        public override void ParseData(List<byte> bytes)
        {
            if (bytes.Count >= 16)
            {
                _data.SetBytes(bytes.GetRange(0, 16));
            }
        }

        public override List<Byte> ToTx()
        {
            return CreateTxWithHeader(new List<byte>()); //Do not send data from client
        }
    }
}
