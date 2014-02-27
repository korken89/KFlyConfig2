using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KFly
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1).ToLocalTime());
            uint tim = (uint)t.TotalSeconds;
            Console.WriteLine("Time since 1/1/1970: " + tim + " seconds");
            Console.WriteLine();

         //   double[,] cal = SensorCalibration.Calibrate(SensorCalibration.TestData, 950);
         //   LinearAlgebra.printMat(cal);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new KFlyConfig());
        }
    }
}
