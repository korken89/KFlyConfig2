using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    public class GetDeviceInfo: KFlyCommand
    {

        private KFlyData _data;

        public KFlyData Data
        {
          get { return _data; }
          set { _data = value; }
        }

        public override void ParseData(List<byte> data)
        {
            _data = KFlyData.FromBytes(data);
        }

        public override string ToString()
        {
            if (_data != null)
            {
                return base.ToString() + ":" + _data.ToString();
            }
            else
            {
                return base.ToString();
            }
        }

        public GetDeviceInfo()
            : base(KFlyCommandType.GetDeviceInfo)
        {
        }
    }
}
