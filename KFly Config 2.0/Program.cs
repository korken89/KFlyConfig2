using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KFly_Config_2._0
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            List<byte> test = new List<byte>();
            test.Add(0xA6);
            test.Add(0x19);
            test.Add(0x10);
            test.Add(0xAF);
            test.Add(0x60);
            test.Add(0x74);
            test.Add(0x6C);
            test.Add(0x3F);
            test.Add(0xA6);
            test.Add(0x96);
            test.Add(0x65);
            test.Add(0x3E);
            test.Add(0x09);
            test.Add(0x5C);
            test.Add(0xD7);
            test.Add(0x3D);
            test.Add(0x95);
            test.Add(0xC3);
            test.Add(0x92);
            test.Add(0x3E);
            test.Add(0x50);
            test.Add(0xE2);

            StateMachine state = new StateMachine();

            foreach (byte b in test)
                state.SerialManager(b);

            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1).ToLocalTime());
            uint tim = (uint)t.TotalSeconds;
            Console.WriteLine("Time since 1/1/1970: " + tim + " seconds");
            Console.WriteLine();

            double[,] cal = SensorCalibration.Calibrate(SensorCalibration.TestData, 950);
            LinearAlgebra.printMat(cal);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new KFlyConfig());
        }
    }
}
