using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Xml;

namespace KFly
{
    public static class SixPointSensorCalibration
    {
       

        public static SensorCalibrationData Calibrate(IEnumerable<RawSensorData> measurepoints)
        {
            try
            {
                var count = measurepoints.Count();
                double[,] accdata = new double[count, 3];
                double[,] magdata = new double[count, 3];
                int i = 0;
                foreach (RawSensorData rsd in measurepoints)
                {
                    accdata[i, 0] = Convert.ToSingle(rsd.Accelerometer.X);
                    accdata[i, 1] = Convert.ToSingle(rsd.Accelerometer.Y);
                    accdata[i, 2] = Convert.ToSingle(rsd.Accelerometer.Z);
                    magdata[i, 0] = Convert.ToSingle(rsd.Magnometer.X);
                    magdata[i, 1] = Convert.ToSingle(rsd.Magnometer.Y);
                    magdata[i, 2] = Convert.ToSingle(rsd.Magnometer.Z);
                    i++;
                }
                var accres = Calibrate(accdata, 0);
                var magres = Calibrate(magdata, 0);

                SensorCalibrationData sc = new SensorCalibrationData();
                sc.AccelerometerBias.X = Convert.ToSingle(accres[0, 0]);
                sc.AccelerometerBias.Y = Convert.ToSingle(accres[1, 0]);
                sc.AccelerometerBias.Z = Convert.ToSingle(accres[2, 0]);
                sc.AccelerometerGain.X = 1/Convert.ToSingle(accres[3, 0]);
                sc.AccelerometerGain.Y = 1/Convert.ToSingle(accres[4, 0]);
                sc.AccelerometerGain.Z = 1/Convert.ToSingle(accres[5, 0]);
                sc.MagnometerBias.X = Convert.ToSingle(magres[0, 0]);
                sc.MagnometerBias.Y = Convert.ToSingle(magres[1, 0]);
                sc.MagnometerBias.Z = Convert.ToSingle(magres[2, 0]);
                sc.MagnometerGain.X = 1/Convert.ToSingle(magres[3, 0]);
                sc.MagnometerGain.Y = 1/Convert.ToSingle(magres[4, 0]);
                sc.MagnometerGain.Z = 1/Convert.ToSingle(magres[5, 0]);
                return sc;
            }
            catch
            {
                return null;
            }
       }
        /* 
         * Uses Gaus-Newton non-linear solver to find biases and gains.
         * Input: Acceleration matrix on the form {ax, ay, az}
         * Optional: Starting guess of gain, else it will calculate the average gain and use that
         */
        public static double[,] Calibrate(double[,] acc)
        {
            return Calibrate(acc, 0);
        }

        public static double[,] Calibrate(double[,] acc, double gain)
        {
            /*
             * Formula for Gauss-Newton solver:
             * theta(k+1) = theta(k) + inv(H'*H)*H'*[y - yHAT(theta_k)]
             */

            int rows = acc.GetLength(0);

            /* Find starting guess of gain (average absolute value) */
            if (gain == 0)
            {
                for (int i = 0; i < rows; i++)
                    gain += Math.Sqrt(acc[i, 0] * acc[i, 0] + acc[i, 1] * acc[i, 1] + acc[i, 2] * acc[i, 2]);

                gain = gain / rows;
            }

            /* Starting guess of Theta: no bias and 'gain' LSB/g */
            double[,] theta = new double[6, 1] { { 0 }, { 0 }, { 0 }, { gain }, { gain }, { gain } };

            double[,] yVec = new double[rows, 1];

            for (int i = 0; i < rows; i++)
                yVec[i, 0] = 1;


            /* Fix so the abort is based on the step-size */
            for (int i = 0; i < 50; i++)
            {
                double[,] H = getH(acc, theta[0, 0], theta[1, 0], theta[2, 0], theta[3, 0], theta[4, 0], theta[5, 0]);
                double[,] yDiff = LinearAlgebra.MatrixSub(yVec, yHAT(acc, theta[0, 0], theta[1, 0], theta[2, 0], theta[3, 0], theta[4, 0], theta[5, 0]));
                double[,] hInv = LinearAlgebra.InvertMatrix(LinearAlgebra.MatrixMultiply(LinearAlgebra.MatrixTranspose(H), H));
                double[,] step = LinearAlgebra.MatrixMultiply(hInv, LinearAlgebra.MatrixTranspose(H));
                step = LinearAlgebra.MatrixMultiply(step, yDiff);

                theta = LinearAlgebra.MatrixAdd(theta, step);
            }



            return theta;
        }

        private static double[,] getH(double[,] acc, double mx, double my, double mz, double gx, double gy, double gz)
        {
            /* H = [-(2.*ax - 2.*mx)./gx.^2, -(2.*ay - 2.*my)./gy.^2, -(2.*az - 2.*mz)./gz.^2,
             * -(2.*(ax - mx).^2)./gx.^3, -(2.*(ay - my).^2)./gy.^3,   -(2.*(az - mz).^2)./gz.^3];*/
            int rows = acc.GetLength(0);
            int cols = acc.GetLength(1);

            double[,] ret = new double[rows, 6];

            for (int i = 0; i < rows; i++)
            {
                double ax = acc[i, 0];
                double ay = acc[i, 1];
                double az = acc[i, 2];

                ret[i, 0] = -2 * (ax - mx) / (gx * gx);
                ret[i, 1] = -2 * (ay - my) / (gy * gy);
                ret[i, 2] = -2 * (az - mz) / (gz * gz);

                ret[i, 3] = -2 * (ax - mx) * (ax - mx) / (gx * gx * gx);
                ret[i, 4] = -2 * (ay - my) * (ay - my) / (gy * gy * gy);
                ret[i, 5] = -2 * (az - mz) * (az - mz) / (gz * gz * gz);
            }

            return ret;
        }

        private static double[,] yHAT(double[,] acc, double mx, double my, double mz, double gx, double gy, double gz)
        {
            /* a = (ax - mx).^2./gx.^2 + (ay - my).^2./gy.^2 + (az - mz).^2./gz.^2; */
            int rows = acc.GetLength(0);

            double[,] ret = new double[rows, 1];

            for (int i = 0; i < rows; i++)
            {
                double ax = acc[i, 0];
                double ay = acc[i, 1];
                double az = acc[i, 2];

                ret[i, 0] = (ax - mx) * (ax - mx) / (gx * gx) + (ay - my) * (ay - my) / (gy * gy) + (az - mz) * (az - mz) / (gz * gz);
            }

            return ret;
        }
    }
}
