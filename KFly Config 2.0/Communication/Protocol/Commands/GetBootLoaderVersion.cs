using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    public class GetBootLoaderVersion: KFlyCommand
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
      
        public GetBootLoaderVersion()
            : base(KFlyCommandType.GetBootloaderVersion)
        {
        }
    }
}
