using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{

    public class SetDeviceString : KFlyCommand
    {

        public String Data;

        public SetDeviceString(String data) : 
            base(KFlyCommandType.SetDeviceString)
        {
            Data = data.Substring(0, Math.Min(100,data.Length));
        }

        public override void ParseData(List<byte> data)
        {
            Data = data.ToString();
        }

        public override List<Byte> ToTx()
        {
            if (Data == null)
                return CreateTxWithHeader(new List<byte>());
            else
                return CreateTxWithHeader(new List<byte>(System.Text.ASCIIEncoding.Default.GetBytes(Data)));
        }
    }
}
