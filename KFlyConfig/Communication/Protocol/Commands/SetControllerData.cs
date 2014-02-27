using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
    /// Same for 
    /// SetRateControllerData
    /// SetAttitudeControllerData
    /// SetPositionControllerData
    ///SetVelocityControllerData
    /// </summary>
    public class SetControllerData : KFlyCommand
    {
        public ControllerData Data;

        public SetControllerData(KFlyCommandType setControllerType) : base(setControllerType)
        {
            Data = new ControllerData();
        }
        public SetControllerData(KFlyCommandType setControllerType, ControllerData data)
            : base(setControllerType)
        {
            Data = data;
        }

        public override void ParseData(List<byte> data)
        {
            Data = ControllerData.FromBytes(data);
        }

        public override List<Byte> ToTx()
        {
            return CreateTxWithHeader(Data.GetBytes());
        }
    }
}
