using System;
using System.Collections.Generic;
using System.Numerics;

namespace Cherry.Math3D
{
    public static class Extensions
    {
        public static float ToRadians(this float degrees)
        {
            return (float)(degrees * Math.PI / 180.0);
        }

        public static double ToRadians(this double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        public static float ToDegrees(this float radians)
        {
            return (float)(radians * 180.0 / Math.PI);
        }

        public static double ToDegrees(this double radians)
        {
            return radians * 180.0 / Math.PI;
        }
    }
}