using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    /// <summary>
    /// Same for 
    /// GetRateControllerData
    /// GetAttitudeControllerData
    /// GetPositionControllerData
    /// GetVelocityControllerData
    /// </summary>
    public class GetRawSensorData : KFlyCommand
    {

        public RawSensorData Data;

        public GetRawSensorData() : 
            base(KFlyCommandType.GetRawSensorData)
        {}

        public override void ParseData(List<byte> data)
        {
            Data = RawSensorData.FromBytes(data);
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
