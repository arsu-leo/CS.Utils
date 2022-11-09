using System.Text.RegularExpressions;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class NumUtil
    {
        private static readonly Regex IntRegexOneOrMoreDigits = new Regex("[0-9]+");
        public static bool ValidStrIntRange(string? str, int min, int max, bool allowEmpty, out int value, int dfltWhenEmpty = 0)
        {

            if (!ValidStrInt(str, allowEmpty, out value, dfltWhenEmpty, true))
            {
                return false;
            }
            return MathUtil.IsBetween(value, min, max);
        }

        public static bool ValidStrInt(string? str, bool allowEmpty, out int value, int dfltWhenEmpty = 0, bool allowNegative = true)
        {
            value = dfltWhenEmpty;
            return string.IsNullOrEmpty(str) ? allowEmpty
                : IntRegexOneOrMoreDigits.IsMatch(str) && int.TryParse(str, out value) && (allowNegative || value >= 0);
        }

        public static bool ValidStrPositiveInt(string? str, out int value, bool allowEmpty = false, int dfltWhenEmpty = 0)
        {
            return ValidStrIntRange(str, 0, int.MaxValue, allowEmpty, out value, dfltWhenEmpty);
        }

        public static bool ValidStrPositiveIntMinOne(string? str, out int value, bool allowEmpty = false, int dfltWhenEmpty = 1)
        {
            return ValidStrIntRange(str, 1, int.MaxValue, allowEmpty, out value, dfltWhenEmpty);
        }
    }
}
