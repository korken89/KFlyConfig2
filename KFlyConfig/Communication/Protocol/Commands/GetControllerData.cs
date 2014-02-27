using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
    /// Same for 
    /// GetRateControllerData
    /// GetAttitudeControllerData
    /// GetPositionControllerData
    /// GetVelocityControllerData
    /// </summary>
    public class GetControllerData : KFlyCommand
    {

        public ControllerData Data;

        public GetControllerData(KFlyCommandType getControllerType) : 
            base(getControllerType)
        {}

        public override void ParseData(List<byte> data)
        {
            Data = ControllerData.FromBytes(data);
        }

        public override List<Byte> ToTx()
        {
            if (Data == null)
                return CreateTxWithHeader(new List<byte>());
            else
                return CreateTxWithHeader(Data.GetBytes());
        }
    }
}
