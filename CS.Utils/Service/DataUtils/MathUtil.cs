using System;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class MathUtil
    {
        public static int DivCeil(int x, int y)
        {
            if (y == 0)
            {
                return 0;
            }
            return (x + y - 1) / y;
        }

        //Good multiple values would be 1, 2, 5, 10
        public static double RoundToMultiple(double val, int multiple)
        {
            var reduced = val / multiple;
            var rr = Math.Round(reduced);
            return rr * multiple;
        }

        public static bool CloseBy(double tolearance, double first, params double[] others)
        {
            double upper = first + tolearance;
            double lower = first - tolearance;
            for (int i = 0; i < others.Length; i++)
            {
                double val = others[i];
                if (val > upper || val < lower)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Given 3 values, val is above maxVal, returns max val, if val is below minVal, returns min val, val otherwise
        /// </summary>
        public static int NormalizeBetween(this int val, int minVal, int maxVal)
        {
            return Math.Max(Math.Min(maxVal, val), minVal);
        }

        public static bool IsBetween(this int val, int minOk, int maxOk)
        {
            return val >= minOk && val <= maxOk;
        }
    }
}
