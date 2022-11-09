
using System;
using System.Globalization;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class DateFormatUtil
    {
        public const string MACHINE_TIME_FORMAT = "HHmmss";
        public const string MACHINE_DATE_FORMAT = "yyyyMMdd";
        public const string MACHINE_FORMAT = MACHINE_DATE_FORMAT + MACHINE_TIME_FORMAT;

        public const string SERIALIZED_DATE_FORMAT = "yyyy-MM-dd";
        public const string SERIALIZED_TIME_FORMAT = "HH:mm:ss";
        public const string SERIEALIZED_FORMAT = SERIALIZED_DATE_FORMAT + "T" + SERIALIZED_TIME_FORMAT;

        public const string SERIEALIZED_FORMAT_WITH_TIMEZONE = SERIEALIZED_FORMAT + "K";

        public static string Format(DateTime date, string format)
        {
            return Format(date, format, DateTime.Now);
        }
        public static string Format(DateTime date, string format, IFormatProvider? pv)
        {
            return Format(date, format, pv, DateTime.Now);
        }

        public static string Format(DateTime date, string format, DateTime elapsedReferenceDate)
        {
            string cFormat = MapFormat(date, format, elapsedReferenceDate);
            string result = date.ToString(cFormat, CultureInfo.InvariantCulture);
            return result;
        }

        public static string Format(DateTime date, string format, IFormatProvider? pv, DateTime elapsedReferenceDate)
        {
            string cFormat = MapFormat(date, format, elapsedReferenceDate);
            return date.ToString(cFormat, pv);
        }

        private static int CountStartChar(string str, char ch)
        {
            int n = 0;
            while (n < str.Length && str[n] == ch)
            {
                n++;
            }
            return n;
        }
        private static string MapFormat(DateTime date, string format, DateTime elapsedReferenceDate)
        {
            int daysDiff = Math.Abs(GetDaysDif(date, elapsedReferenceDate));

            string cFormat = "";

            int numDayOfWeek = (int)date.DayOfWeek;
            numDayOfWeek = numDayOfWeek == 0 ? 7 : numDayOfWeek;
            int weekOfYear = ISOWeek.GetWeekOfYear(date);
            char charDayOfWeek = (char)('A' + (numDayOfWeek - 1));
            int n;
            while (format.Length > 0)
            {
                char firstChar = format[0];
                n = 1;
                switch (firstChar)
                {
                    case '"':
                        var pos = format.IndexOf('"', 1);
                        if (pos < 0) //No closing " found
                        {
                            cFormat += $"\"{format.Substring(1)}\"";
                        }
                        else
                        {
                            cFormat += format.Substring(0, pos);
                            n = pos + 1;
                        }
                        break;
                    case 'd':
                        n = CountStartChar(format, firstChar);
                        if (n == 1)
                        {
                            cFormat = $"\"{date.Day}\"";
                        }
                        else
                        {
                            if (n > 4)
                            {
                                n = 4;
                            }
                            cFormat += new string(firstChar, n);
                        }
                        break;
                    case 'M': //Month M, MM, MMM, MMMM
                        n = CountStartChar(format, firstChar);
                        if (n == 1)
                        {
                            cFormat = $"\"{date.Month}\"";
                        }
                        else
                        {
                            if (n > 4)
                            {
                                n = 4;
                            }
                            cFormat += new string(firstChar, n);
                        }
                        break;
                    case 'y':
                    case 'H':
                    case 'h':
                    case 'm':
                    case 's':
                    case 'f':
                        cFormat += firstChar;
                        break;
                    case 'W': // week number
                        n = CountStartChar(format, firstChar);
                        if (n > 2)
                        {
                            n = 2;
                        }
                        string wContent = weekOfYear.ToString().PadLeft(n, '0');
                        cFormat += "\"" + wContent + "\"";
                        break;
                    case 'A': //Day of the week char
                        cFormat += $"\"{ charDayOfWeek}\"";
                        break;
                    case 'a': //Day of the week num
                        cFormat += $"\"{numDayOfWeek }\"";
                        break;
                    case 'x':
                        cFormat += $"\"{(daysDiff % 1000).ToString().PadLeft(3, '0')}\"";
                        break;
                    case 'X':
                        cFormat += $"\"{(daysDiff % 10000).ToString().PadLeft(4, '0')}\"";
                        break;
                    default:
                        cFormat += $"\"{firstChar}\"";
                        break;
                }
                format = format.Substring(n);
            }
            return cFormat;
        }

        public static string ToSerializedDateWithTimeZone(DateTime dt)
        {
            string str = dt.ToString(SERIEALIZED_FORMAT_WITH_TIMEZONE, CultureInfo.InvariantCulture);
            return str;
        }

        public static string ToSerializedDateNoTimeZone(DateTime dt)
        {
            string str = dt.ToString(SERIEALIZED_FORMAT, CultureInfo.InvariantCulture);
            return str;
        }

        public static string ToMachineDate(DateTime dt)
        {
            return dt.ToString(MACHINE_DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        public static string ToMachineDateTime(DateTime dt)
        {
            return dt.ToString(MACHINE_FORMAT, CultureInfo.InvariantCulture);
        }

        private static int GetDaysDif(DateTime from, DateTime to) // expected to > from
        {
            DateTime startDate, endDate;
            bool negative = false;
            if (to > from)
            {
                startDate = from;
                endDate = to;
            }
            else
            {
                startDate = to;
                endDate = from;
                negative = true;
            }
            int daysDiff = (int)Math.Round((endDate - startDate).TotalDays);
            if (negative)
            {
                daysDiff = -daysDiff;
            }
            return daysDiff;
        }

        public static bool TryParseMachineDate(string str, out DateTime val)
        {
            return DateTime.TryParseExact(str, MACHINE_DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out val);
        }

        public static bool TryParseSerializedDateTimeNoTimezone(string str, out DateTime val)
        {
            return DateTime.TryParseExact(str, SERIEALIZED_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out val);
        }

        public static bool TryParseSerializedDateTimeWithTimezone(string str, out DateTime val)
        {
            return DateTime.TryParseExact(str, SERIEALIZED_FORMAT_WITH_TIMEZONE, CultureInfo.InvariantCulture, DateTimeStyles.None, out val);
        }

        public static bool IsMinValue(this DateTime date)
        {
            return date == DateTime.MinValue;
        }
    }
}
