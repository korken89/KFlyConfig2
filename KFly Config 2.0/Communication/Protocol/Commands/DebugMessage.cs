using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class DebugMessage: KFlyCommand
    {
        private List<byte> _data;

        public List<byte> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public String DataAsString
        {
            get { return System.Text.ASCIIEncoding.Default.GetString(_data.ToArray()); }
            set { _data = new List<byte>(System.Text.ASCIIEncoding.Default.GetBytes(value)); }
        }
        
        public DebugMessage()
            : base(KFlyCommandType.DebugMessage)
        {
        }

        public DebugMessage(List<byte> bytes)
            : base(KFlyCommandType.DebugMessage)
        {
            Type = KFlyCommandType.DebugMessage;
            _data = bytes.GetRange(3, bytes.Count - 4);
        }
    }
}
