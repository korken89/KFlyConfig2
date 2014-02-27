using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class DebugMessage: KFlyCommand
    {
        private String _message = "";

        public String Message
        {
            get 
            { 
                return _message;
            }
            set 
            {
                _message = value;
            }
        }

        public override void ParseData(List<byte> data)
        {
            try
            {
                _message = System.Text.ASCIIEncoding.Default.GetString(data.ToArray());
            }
            catch
            {
                _message = "Message not a string";
            }
        }

        public override List<byte> ToTx()
        {
            if (_message != null)
            {
                return this.CreateTxWithHeader(new List<byte>(System.Text.ASCIIEncoding.Default.GetBytes(_message)));
            }
            else
            {
                return base.ToTx();
            }
        }

        public override string ToString()
        {
            return base.ToString() + ":" + Message;
        }
        
        public DebugMessage()
            : base(KFlyCommandType.DebugMessage)
        {
        }

        public DebugMessage(String message)
            : base(KFlyCommandType.DebugMessage)
        {
            Message = message;
        }
    }
}
