using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

namespace Cherry.Math3D
{
    public static class Matrix3DExtensions
    {
        // Convert a System.Numerics.Matrix4x4 to a float[16] array.
        public static float[] ToArray(this Matrix4x4 matrix)
        {
            return new float[]
            {
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44
            };
        }

        // Convert a Matrix3D.Matrix44 to a System.Numerics.Matrix4x4.
        public static Matrix4x4 ToMatrix4x4(this Matrix3D.Matrix44 src)
        {
            Matrix4x4 dst = Matrix4x4.Identity;

            dst.M11 = src.MtxF2D[0, 0];
            dst.M12 = src.MtxF2D[0, 1];
            dst.M13 = src.MtxF2D[0, 2];
            dst.M14 = src.MtxF2D[0, 3];

            dst.M21 = src.MtxF2D[1, 0];
            dst.M22 = src.MtxF2D[1, 1];
            dst.M23 = src.MtxF2D[1, 2];
            dst.M24 = src.MtxF2D[1, 3];

            dst.M31 = src.MtxF2D[2, 0];
            dst.M32 = src.MtxF2D[2, 1];
            dst.M33 = src.MtxF2D[2, 2];
            dst.M34 = src.MtxF2D[2, 3];

            dst.M41 = src.MtxF2D[3, 0];
            dst.M42 = src.MtxF2D[3, 1];
            dst.M43 = src.MtxF2D[3, 2];
            dst.M44 = src.MtxF2D[3, 3];

            return dst;
        }

        public static float[] ToOneDimensionalArray(this float[,] src)
        {
            float[] dst = new float[src.GetLength(0) * src.GetLength(1)];
            for (int i = 0; i < src.GetLength(0); i++)
            {
                for (int j = 0; j < src.GetLength(1); j++)
                {
                    dst[i * src.GetLength(1) + j] = src[i, j];
                }
            }
            return dst;
        }

        public static float[,] ToTwoDimensionalArray(this float[] src)
        {
            float[,] dst = new float[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    dst[i, j] = src[i * 4 + j];
                }
            }
            return dst;
        }
    }
    public class Matrix3D
    {
        public class Matrix44
        {
            public static float[] Identity = new float[16]
            {
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f
            };

            public float[] MtxF1D { get; set; }
            public float[,] MtxF2D { get; set; }

            public Matrix4x4 MtxF4x4 { get; set; }

            public Matrix44()
            {
                MtxF1D = Identity;
                MtxF2D = Identity.ToTwoDimensionalArray();
                MtxF4x4 = Matrix4x4.Identity;
            }

            public static Matrix44 Projection(float width, float height, float near, float far)
            {
                Matrix44 dst = new Matrix44();

                // Initialize Projection Matrix
                float aspect = width / height;
                float fov = Convert.ToSingle(1.0f / MathF.Tan(60.0f * (MathF.PI / 180.0f) * 0.5f));
                float iNF = 1.0f / (near - far);

                // Construct Matrix
                dst.MtxF1D[0] = fov / aspect; dst.MtxF1D[4] = 0.0f; dst.MtxF1D[8] = 0.0f; dst.MtxF1D[12] = 0.0f;
                dst.MtxF1D[1] = 0.0f; dst.MtxF1D[5] = fov; dst.MtxF1D[9] = 0.0f; dst.MtxF1D[13] = 0.0f;
                dst.MtxF1D[2] = 0.0f; dst.MtxF1D[6] = 0.0f; dst.MtxF1D[10] = (near + far) * iNF; dst.MtxF1D[14] = 2.0f * far * near * iNF;
                dst.MtxF1D[3] = 0.0f; dst.MtxF1D[7] = 0.0f; dst.MtxF1D[11] = -1.0f; dst.MtxF1D[15] = 0.0f;
                
                // Populate other class members.
                dst.MtxF2D = dst.MtxF1D.ToTwoDimensionalArray();
                dst.MtxF4x4 = dst.ToMatrix4x4();

                return dst;
            }
        }
    }
}