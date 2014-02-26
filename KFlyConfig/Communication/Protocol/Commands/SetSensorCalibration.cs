using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly.Communication
{
    /// <summary>
    /// Same for 
    /// SetRateControllerData
    /// SetAttitudeControllerData
    /// SetPositionControllerData
    ///SetVelocityControllerData
    /// </summary>
    class SetSensorCalibration : KFlyCommand
    {
        public SensorCalibration Data;

        public SetSensorCalibration() : base(KFlyCommandType.SetSensorCalibration)
        {
            Data = new SensorCalibration();
        }
        public SetSensorCalibration(SensorCalibration data)
            : base(KFlyCommandType.SetSensorCalibration)
        {
            Data = data;
        }

        public override void ParseData(List<byte> data)
        {
            Data = SensorCalibration.FromBytes(data);
        }

        public override List<Byte> ToTx()
        {
            return CreateTxWithHeader(Data.GetBytes());
        }
    }
}
