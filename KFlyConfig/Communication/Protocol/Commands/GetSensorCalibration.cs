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
    public class GetSensorCalibration: KFlyCommand
    {

        public SensorCalibration Data;

        public GetSensorCalibration() :
            base(KFlyCommandType.GetSensorCalibration)
        {}

        public override void ParseData(List<byte> data)
        {
            Data = SensorCalibration.FromBytes(data);
        }

        public override List<Byte> ToTx()
        {
            return CreateTxWithHeader(new List<byte>());
        }
    }
}
