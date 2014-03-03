using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    /// <summary>
    /// </summary>
    public class SetSensorCalibration : KFlyCommand
    {
        public SensorCalibrationData Data;

        public SetSensorCalibration() : base(KFlyCommandType.SetSensorCalibration)
        {
            Data = new SensorCalibrationData();
        }
        public SetSensorCalibration(SensorCalibrationData data)
            : base(KFlyCommandType.SetSensorCalibration)
        {
            Data = data;
        }

        public override void ParseData(List<byte> data)
        {
            Data = SensorCalibrationData.FromBytes(data);
        }

        public override List<Byte> ToTx()
        {
            return CreateTxWithHeader(Data.GetBytes());
        }
    }
}
