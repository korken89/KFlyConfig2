using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class GetFirmwareVersion: KFlyCommand
    {

        public String Version;

        public override void ParseData(List<byte> data)
        {
            Version = System.Text.ASCIIEncoding.Default.GetString(data.ToArray());
        }

        public override string ToString()
        {
            return base.ToString() + ":" + Version;
        }
      
        public GetFirmwareVersion()
            : base(KFlyCommandType.GetFirmwareVersion)
        {
        }
    }
}
