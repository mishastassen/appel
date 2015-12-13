using System;
using System.Collections;

public class Matrix
{
    public int n;
    public int[,] mat;
    private static System.Random rng = new System.Random(); // [seed=1010, n=3]
                                                            // bounds on entries matrix
    private int lowerbound = -5;
    private int upperbound = 6; // upperbound is exclusive

    public Matrix(int nn)
    {
        n = nn;
        mat = new int[n, n];
        InitMatrix();
    }

    // TODO improve
    private int CalcDeterminant()
    {
        if (n != 3)
            throw new NotImplementedException();
        int res = 0;
        res += mat[0, 0] * (mat[1, 1] * mat[2, 2] - mat[2, 1] * mat[1, 2]);
        res -= mat[1, 0] * (mat[0, 1] * mat[2, 2] - mat[2, 1] * mat[0, 2]);
        res += mat[2, 0] * (mat[0, 1] * mat[1, 2] - mat[1, 1] * mat[0, 2]);
        return res;
    }

    public float InitMatrix()
    {
        float[,] imat;
        float ret;
    again:
        do
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    mat[i, j] = rng.Next(lowerbound, upperbound);

            for (int i = 0; i < n; i++)
            {
                bool containsNegative = false;
                bool containsPositive = false;
                for (int j = 0; j < n; j++)
                {
                    if (mat[i, j] < 0)
                        containsNegative = true;
                    if (mat[j, i] > 0)
                        containsPositive = true;
                }
                if (!containsNegative || !containsPositive)
                    goto again;
            }

            var mat2 = new float[4, 4];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    mat2[1 + i, 1 + j] = (float)mat[i, j];
            mat2[0, 1] = mat2[0, 2] = mat2[0, 3] = 1.0f;
            mat2[1, 0] = mat2[2, 0] = mat2[3, 0] = 1.0f;
            mat2[0, 0] = 0.0f;
            imat = invert(mat2);
            /*for (int x = 0; x < 4; x += 1)
            {
                for (int y = 0; y < 4; y += 1)
                    Console.Write(imat[x, y] + "\t");
                Console.WriteLine("\n");
            }*/
            ret = imat[0, 0];
        } while (ret >= 0 || CalcDeterminant() == 0 || (imat[0, 1] < 0.01 || imat[0, 2] < 0.01 || imat[0, 3] < 0.01 || imat[1, 0] < 0.01 || imat[2, 0] < 0.01 || imat[3, 0] < 0.01));
        return ret;
    }

    public static float[,] invert(float[,] mat)
    {
        var ret = new float[4, 4];

        var s0 = mat[0, 0] * mat[1, 1] - mat[1, 0] * mat[0, 1];
        var s1 = mat[0, 0] * mat[1, 2] - mat[1, 0] * mat[0, 2];
        var s2 = mat[0, 0] * mat[1, 3] - mat[1, 0] * mat[0, 3];
        var s3 = mat[0, 1] * mat[1, 2] - mat[1, 1] * mat[0, 2];
        var s4 = mat[0, 1] * mat[1, 3] - mat[1, 1] * mat[0, 3];
        var s5 = mat[0, 2] * mat[1, 3] - mat[1, 2] * mat[0, 3];

        var c5 = mat[2, 2] * mat[3, 3] - mat[3, 2] * mat[2, 3];
        var c4 = mat[2, 1] * mat[3, 3] - mat[3, 1] * mat[2, 3];
        var c3 = mat[2, 1] * mat[3, 2] - mat[3, 1] * mat[2, 2];
        var c2 = mat[2, 0] * mat[3, 3] - mat[3, 0] * mat[2, 3];
        var c1 = mat[2, 0] * mat[3, 2] - mat[3, 0] * mat[2, 2];
        var c0 = mat[2, 0] * mat[3, 1] - mat[3, 0] * mat[2, 1];

        var invdet = 1 / (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);

        ret[0, 0] = (mat[1, 1] * c5 - mat[1, 2] * c4 + mat[1, 3] * c3) * invdet;
        ret[0, 1] = (-mat[0, 1] * c5 + mat[0, 2] * c4 - mat[0, 3] * c3) * invdet;
        ret[0, 2] = (mat[3, 1] * s5 - mat[3, 2] * s4 + mat[3, 3] * s3) * invdet;
        ret[0, 3] = (-mat[2, 1] * s5 + mat[2, 2] * s4 - mat[2, 3] * s3) * invdet;

        ret[1, 0] = (-mat[1, 0] * c5 + mat[1, 2] * c2 - mat[1, 3] * c1) * invdet;
        ret[1, 1] = (mat[0, 0] * c5 - mat[0, 2] * c2 + mat[0, 3] * c1) * invdet;
        ret[1, 2] = (-mat[3, 0] * s5 + mat[3, 2] * s2 - mat[3, 3] * s1) * invdet;
        ret[1, 3] = (mat[2, 0] * s5 - mat[2, 2] * s2 + mat[2, 3] * s1) * invdet;

        ret[2, 0] = (mat[1, 0] * c4 - mat[1, 1] * c2 + mat[1, 3] * c0) * invdet;
        ret[2, 1] = (-mat[0, 0] * c4 + mat[0, 1] * c2 - mat[0, 3] * c0) * invdet;
        ret[2, 2] = (mat[3, 0] * s4 - mat[3, 1] * s2 + mat[3, 3] * s0) * invdet;
        ret[2, 3] = (-mat[2, 0] * s4 + mat[2, 1] * s2 - mat[2, 3] * s0) * invdet;

        ret[3, 0] = (-mat[1, 0] * c3 + mat[1, 1] * c1 - mat[1, 2] * c0) * invdet;
        ret[3, 1] = (mat[0, 0] * c3 - mat[0, 1] * c1 + mat[0, 2] * c0) * invdet;
        ret[3, 2] = (-mat[3, 0] * s3 + mat[3, 1] * s1 - mat[3, 2] * s0) * invdet;
        ret[3, 3] = (mat[2, 0] * s3 - mat[2, 1] * s1 + mat[2, 2] * s0) * invdet;

        return ret;
    }
}
