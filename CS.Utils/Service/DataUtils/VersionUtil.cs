using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class VersionUtil
    {
        public static Version CreateVersion(params int[] intParts)
        {
            return intParts.Length switch
            {
                1 => new Version(intParts[0], 0, 0, 0),
                2 => new Version(intParts[0], intParts[1], 0, 0),
                3 => new Version(intParts[0], intParts[1], intParts[2], 0),
                4 => new Version(intParts[0], intParts[1], intParts[2], intParts[3]),
                _ => CreateZero(),
            };
        }
        public static Version CreateZero()
        {
            return new Version(0, 0, 0, 0);
        }
        
        public static bool IsZero(this Version v)
        {
            int[] r = v.GetDataAsArray();
            for(int i = 0; i < r.Length; i++)
            {
                if(r[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }


        public static string ToFormatedString(this Version v, int[] partsPattern, bool skipTrailingZeroes)
        {
            return ToFormatedString(v.GetDataAsArray(), partsPattern, skipTrailingZeroes);
        }

        public static int[] GetDataAsArray(this Version v)
        {
            int[] parts = new[] { v.Major, v.Minor, v.Build, v.Revision };
            return parts;
        }

        public static string AsString(this Version v)
        {
            return string.Join('.', GetDataAsArray(v));
        }

        private static string ToFormatedString(int[] versionParts, int[] partsPattern, bool skipTrailingZeroes)
        {
            List<string> resultParts = new List<string>();
            for (int i = 0; i < partsPattern.Length && i < versionParts.Length; i++)
            {
                int partValue = versionParts[i];
                int partDigits = partsPattern[i];
                string partStr = partValue.ToString().PadLeft(partDigits, '0');
                resultParts.Add(partStr);
            }
            if (skipTrailingZeroes)
            {
                while (resultParts.Count > 1 && resultParts[^1].AllCharEquals('0'))
                {
                    resultParts.RemoveAt(resultParts.Count - 1);
                }
            }
            return string.Join(".", resultParts);
        }

        public static bool TryParse(string versionStr, [NotNullWhen(true)] out Version? version)
        {
            version = null;
            if (string.IsNullOrWhiteSpace(versionStr))
            {
                return false;
            }
            string[] parts = versionStr.Split('.');
            Span<int> iParts = new int[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                if (!int.TryParse(parts[i], out int n))
                {
                    return false;
                }
                iParts[i] = n;
            }

            switch (iParts.Length)
            {
                case 2:
                    version = new Version(iParts[0], iParts[1]);
                    return true;
                case 3:
                    version = new Version(iParts[0], iParts[1], iParts[2]);
                    return true;
                case 4:
                    version = new Version(iParts[0], iParts[1], iParts[2], iParts[3]);
                    return true;
                default:
                    return false;
            }
        }
    }
}
