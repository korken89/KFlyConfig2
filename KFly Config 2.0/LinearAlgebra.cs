using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly_Config_2._0
{
    class LinearAlgebra
    {
        public static double[,] RandomMatrix(uint row, uint col, double scale)
        {
            double[,] ret = new double[row, col];
		    Random r = new Random();
		
		    for(int i = 0; i < row; i++)
			    for(int j = 0; j < col; j++)
                    ret[i,j] = (r.NextDouble()*2  - 1) * scale;
		
		    return ret;
	    }

        public static double[,] MatrixMultiply(double[,] a, double[,] b)
        {
            int aRows = a.GetLength(0);
            int aColumns = a.GetLength(1);
            int bColumns = b.GetLength(1);

            double[,] resultant = new double[aRows, bColumns];

            for (int i = 0; i < aRows; i++)
                for (int j = 0; j < bColumns; j++)
                    for (int k = 0; k < aColumns; k++)
                        resultant[i, j] += a[i, k] * b[k, j];

            return resultant;
        }

        public static double[,] MatrixTranspose(double[,] a)
        {
            int aRows = a.GetLength(0);
            int aColumns = a.GetLength(1);

            double[,] resultant = new double[aColumns, aRows];

            for (int i = 0; i < aRows; i++)
                for (int j = 0; j < aColumns; j++)
                    resultant[j, i] = a[i, j];

            return resultant;
        }

        public static double[,] MatrixAdd(double[,] a, double[,] b)
        {
            int aRows = a.GetLength(0);
            int aColumns = a.GetLength(1);

            double[,] resultant = new double[aRows, aColumns];

            for (int i = 0; i < aRows; i++)
                for (int j = 0; j < aColumns; j++)
                    resultant[i, j] = a[i, j] + b[i, j];

            return resultant;
        }

        public static double[,] MatrixSub(double[,] a, double[,] b)
        {
            int aRows = a.GetLength(0);
            int aColumns = a.GetLength(1);

            double[,] resultant = new double[aRows, 1];

            for (int i = 0; i < aRows; i++)
                for (int j = 0; j < aColumns; j++)
                        resultant[i, j] = a[i, j] - b[i, j];

            return resultant;
        }

        public static double[,] InvertMatrix(double[,] M)
        {
		    if (M.GetLength(0) != M.GetLength(1))
                throw new System.ArgumentException("Matrix must be square.");

            double[,] L = new double[M.GetLength(0), M.GetLength(0)];
            double[,] U = new double[M.GetLength(0), M.GetLength(0)];
		    
		    LUFactorize(M, L, U);

            //if (Math.Abs(UDeterminant(U)) < 1e-15)
            //    throw new System.ArgumentException("Matrix determinant is very small, likley it does not have an inverse.");

		    InvertLMatrix(L);
		    InvertUMatrix(U);

		    return MultiplyULMatrix(U, L);
	    }

        public static void printMat(double[,] mat)
        {
            int aRows = mat.GetLength(0);
            int aColumns = mat.GetLength(1);

            for (int i = 0; i < aRows; i++)
            {
                for (int j = 0; j < aColumns; j++)
                {
                    Console.Write(mat[i,j].ToString("e4"));

                    if (j != aColumns-1)
                        Console.Write(", ");
                }

                Console.WriteLine();
            }	
	    }

        double norms(double[,] matrix)
        {
            int i,j;
            int n = matrix.GetLength(0);
            double sum = 0;
            for(i = 0; i < n; i++)
                for(j = 0; j < n; j++)
                    sum += Math.Pow(matrix[i, j], 2);

            return Math.Sqrt(sum);
        }

        public static double UDeterminant(double[,] U)
        {
            double det = 1.0f;

            for (int i = 0; i < U.GetLength(0); i++)
            {
                det *= U[i, i];
            }

            return det;
        }

        public static void LUFactorize(double[,] a, double[,] L, double[,] U)
        {
            int k, i, j, n;
            n = a.GetLength(0);

            for (k = 0; k < n; k++)
            {
                L[k,k] = 1;

                for (i = k + 1; i < n; i++)
                {
                    L[i,k] = a[i,k] / a[k,k];

                    for (j = k + 1; j < n; j++)
                        a[i,j] = a[i,j] - L[i,k] * a[k,j];

                }
                for (j = k; j < n; j++)
                    U[k,j] = a[k,j];
            }
        }

        public static void InvertLMatrix(double[,] l)
        {
            double sum;
		    int i,j,k;
		    int n = l.GetLength(0);
		
		    for (j = 0; j < n; j++)
            {
			    for (i = j + 1; i < n; i++)
                {
				    sum = 0.0f;
				    for (k = j; k < i; k++)
					    sum -= l[i,k]*l[k,j];
				
				    l[i,j] = sum;
			    }
		    }
	    }

        public static void InvertUMatrix(double[,] u)
        {
            double sum;
		    int i,j,k;
		    int n = u.GetLength(0);
		
		    for (j = n - 1; j > -1; j--)
			    u[j,j] = 1.0f/u[j,j];
		
		    for (j = n - 1; j > -1; j--)
            {
			    for (i = j - 1; i > -1; i--)
                {
				    sum = 0.0f;
				
				    for (k = j; k > i; k--)
					    sum -= u[i,k]*u[k,j];
				
				    u[i,j] = sum*u[i,i];

			    }
		    }
	    }
	
	    /* This function multiplies a Upper triangular matrix with a Lower triangular matrix */
        public static double[,] MultiplyULMatrix(double[,] a, double[,] b)
        { 
		    int n = a.GetLength(0);

            double[,] resultant = new double[n, n];
		   
		
		    /* *
		     * 
		     * Illustration why we don't have to multiply and add every element:
		     * x is some number in the matrix, but the x:es are not necessarily the same.
		     * 
		     * | x x x x x |   | x 0 0 0 0 |
		     * | 0 x x x x |   | x x 0 0 0 |
		     * | 0 0 x x x | * | x x x 0 0 | = ... 
		     * | 0 0 0 x x |   | x x x x 0 |
		     * | 0 0 0 0 x |   | x x x x x |
		     * 
		     * Many zeroes that don't have to be taken into account when multiplying!
		     * 
		     * */
		
		
		
		    for(int i = 0; i < n; i++)
            {
			    for(int j = 0; j < n; j++)
                {
				    resultant[i,j] = 0.0f;
				    int k;
				
				    if (j < i)
					    k = i;
				    else
					    k = j;
				
				    for(; k < n; k++)
					    resultant[i,j] += a[i,k] * b[k,j];
			    }
		    }
		   
		    return resultant;
	    }
    }
}
