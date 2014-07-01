using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
   
    /// </summary>
    public class ManageSubscriptions : KFlyCommand
    {

        private SubscriptionData _data = new SubscriptionData();

        public SubscriptionData Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public ManageSubscriptions() : 
            base(KFlyCommandType.ManageSubscriptions)
        {}

        public override void ParseData(List<byte> bytes)
        {
             _data.SetBytes(bytes);
        }

        public override List<Byte> ToTx()
        {
            if (_data != null)
            {
                return CreateTxWithHeader(_data.GetBytes()); //Do not send data from client
            }
            else
            {
                return CreateTxWithHeader(new List<byte>()); //Do not send data from client
            }
        }
    }
}
