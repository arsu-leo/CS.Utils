using ArsuLeo.CS.Utils.Model.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class StringUtil
    {
        public const string Cr = "\r";
        public const string Lf = "\n";
        public const string CrLf = Cr + Lf;
        public static string CreateEnumeration(string concatString, string lastConcatString, IEnumerable<string> elements)
        {
            int count = elements.Count();
            if (count == 0)
            {
                return string.Empty;
            }
            if (count == 1)
            {
                return elements.ElementAt(0);
            }
            return $"{string.Join(concatString, elements.Take(count - 1))} {lastConcatString} {elements.ElementAt(count - 1)}";
        }

        public static readonly string[] NewLineChars = new string[] { CrLf, Cr, Lf };
        public static readonly Regex NewLineRegex = new Regex(string.Join('|', NewLineChars.Select(s => "(?:" + s + ")")));
        public static string[] SplitStringByAnyNewLines(this string str, StringSplitOptions op = StringSplitOptions.None)
        {
            return str.Split(NewLineChars, op);
        }

        public static int CountMatchingCharsFromStart(this string str1, string str2)
        {
            int i = 0;
            while (i < str1.Length && i < str2.Length)
            {
                i++;
            }
            return i;
        }

        public static bool StartsAndEndsWithChar(this string str, char ch)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return str[0] == ch && str[^1] == ch;
        }

        public static string TrimStartSeq(this string str, string seq, out int count)
        {
            count = 0;
            while (str.StartsWith(seq))
            {
                str = str.Substring(seq.Length);
                count++;
            }
            return str;
        }

        public static string Repeat(this string str, int count)
        {
            return string.Concat(Enumerable.Repeat(str, count));
        }

        public static bool AllCharEquals(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return true;
            }
            for (int i = 0; i < str.Length - 1; i++)
            {
                if (str[i] != str[i + 1])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool AllCharEquals(this string str, char ch)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != ch)
                {
                    return false;
                }
            }
            return true;
        }

        public static string ToStringNvl(this object obj, string dflt = "")
        {
            string? str = obj.ToString();
            return str ?? dflt;
        }

        public static string Nvl(string? str, string dflt = "")
        {
            return str ?? dflt;
        }

        public static string NvlEmpty(string? str, string dflt = "")
        {
            return string.IsNullOrEmpty(str) ? dflt : str;
        }

        public static List<int> GetOccurrencesLocationsOf(this string str, string what, bool jumpSizeOfWhatWhenOcurrenceFound = true, int stopOnOccurrence = 0)
        {
            List<int> result = new List<int>();
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(what))
            {
                return result;
            }

            for (int i = 0, matchedChars = 0; i < str.Length - what.Length + 1; i++)
            {
                if (str[i] == what[matchedChars])
                {
                    matchedChars++;
                    if (what.Length == matchedChars)
                    {
                        result.Add(i - what.Length - 1);
                        if (result.Count == stopOnOccurrence)
                        {
                            return result;
                        }
                        matchedChars = 0;
                        if (!jumpSizeOfWhatWhenOcurrenceFound)
                        {
                            i -= what.Length;
                        }
                    }
                }
                else
                {
                    i -= matchedChars;
                    matchedChars = 0;
                }
            }
            return result;
        }

        public static List<int> GetOccurrencesLocationsOf(this StringBuilder str, string what, bool jumpSizeOfWhatWhenOcurrenceFound = true, int stopOnOccurrence = 0)
        {
            List<int> result = new List<int>();
            if (str is null || str.Length == 0 || string.IsNullOrEmpty(what))
            {
                return result;
            }

            for (int i = 0, matchedChars = 0; i < str.Length - what.Length + 1; i++)
            {
                if (str[i] == what[matchedChars])
                {
                    matchedChars++;
                    if (what.Length == matchedChars)
                    {
                        result.Add(i - what.Length - 1);
                        if (result.Count == stopOnOccurrence)
                        {
                            return result;
                        }
                        matchedChars = 0;
                        if (!jumpSizeOfWhatWhenOcurrenceFound)
                        {
                            i -= what.Length;
                        }
                    }
                }
                else
                {
                    i -= matchedChars;
                    matchedChars = 0;
                }
            }
            return result;
        }

        public static int CountOccurrencesOf(this string str, string what, bool jumpSizeOfWhatWhenOcurrenceFound = true)
        {
            return GetOccurrencesLocationsOf(str, what, jumpSizeOfWhatWhenOcurrenceFound).Count;
            /*if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(what))
            {
                return 0;
            }
            char firstCharFind = what[0];
            int result = 0;
            for (int i = 0; i < str.Length - what.Length; i++)
            {
                if (str[i] == firstCharFind)
                {
                    bool allCharsMatches = true;
                    for (int j = 1; j < what.Length && allCharsMatches && i + j < str.Length; j++)
                    {
                        allCharsMatches = allCharsMatches && str[i + j] == what[j];
                    }
                    if (allCharsMatches)
                    {
                        result++;
                        if (jumpSizeOfWhatWhenOcurrenceFound)
                        {
                            i += what.Length - 1;
                        }
                    }
                }
            }
            return result;*/
        }

        public static int IndexOfOccurence(this string str, string what, int occurrenceNumber)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(what))
            {
                return -1;
            }
            if (occurrenceNumber < 1)
            {
                occurrenceNumber = 1;
            }

            List<int> r = GetOccurrencesLocationsOf(str, what, true, occurrenceNumber);
            if (r.Count < occurrenceNumber)
            {
                return -1;
            }
            return r[occurrenceNumber];
            /*int foundTimes = 0;
            int startIndex = 0;
            while (startIndex < str.Length)
            {
                int foundIndex = str.IndexOf(what, startIndex);
                if (foundIndex < 0)
                {
                    return -1;
                }
                foundTimes++;
                if (foundTimes == occurrenceNumber)
                {
                    return foundIndex;
                }
                startIndex = foundIndex + 1;
            }
            return -1;*/
        }
        //public static IEnumerable<string> StringSplitWithSizeLimit(string str, int size)
        //{
        //    List<string> result = new List<string>();

        //    string[] lines = SplitStringByAnyNewLines(str);
        //    foreach (string s in lines)
        //    {
        //        result.AddRange(StringSplitByCharLimit(s, size));
        //    }
        //    return result;
        //}

        //private static IEnumerable<string> StringSplitByCharLimit(string str, int size)
        //{
        //    List<string> result = new List<string>();
        //    int st = 0;
        //    do
        //    {
        //        int take = str.Length - st;
        //        result.Add(str.Substring(st, Math.Min(size, take)));
        //        st += size;
        //    }
        //    while (st < str.Length);
        //    return result;
        //}

        private static readonly Dictionary<string, string> NonStandardChars = new Dictionary<string, string>
        {
            { "äæǽ", "ae" },
            { "öœ", "oe" },
            { "ü", "ue" },
            { "Ä", "Ae" },
            { "Ü", "Ue" },
            { "Ö", "Oe" },
            { "ÀÁÂÃÄÅǺĀĂĄǍΑΆẢẠẦẪẨẬẰẮẴẲẶА", "A" },
            { "àáâãåǻāăąǎªαάảạầấẫẩậằắẵẳặа", "a" },
            { "Б", "B" },
            { "б", "b" },
            { "ÇĆĈĊČ", "C" },
            { "çćĉċč", "c" },
            { "Д", "D" },
            { "д", "d" },
            { "ÐĎĐΔ", "Dj" },
            { "ðďđδ", "dj" },
            { "ÈÉÊËĒĔĖĘĚΕΈẼẺẸỀẾỄỂỆЕЭ", "E" },
            { "èéêëēĕėęěέεẽẻẹềếễểệеэ", "e" },
            { "Ф", "F" },
            { "ф", "f" },
            { "ĜĞĠĢΓГҐ", "G" },
            { "ĝğġģγгґ", "g" },
            { "ĤĦ", "H" },
            { "ĥħ", "h" },
            { "ÌÍÎÏĨĪĬǏĮİΗΉΊΙΪỈỊИЫ", "I" },
            { "ìíîïĩīĭǐįıηήίιϊỉịиыї", "i" },
            { "Ĵ", "J" },
            { "ĵ", "j" },
            { "ĶΚК", "K" },
            { "ķκк", "k" },
            { "ĹĻĽĿŁΛЛ", "L" },
            { "ĺļľŀłλл", "l" },
            { "М", "M" },
            { "м", "m" },
            { "ÑŃŅŇΝН", "N" },
            { "ñńņňŉνн", "n" },
            { "ÒÓÔÕŌŎǑŐƠØǾΟΌΩΏỎỌỒỐỖỔỘỜỚỠỞỢО", "O" },
            { "òóôõōŏǒőơøǿºοόωώỏọồốỗổộờớỡởợо", "o" },
            { "П", "P" },
            { "п", "p" },
            { "ŔŖŘΡР", "R" },
            { "ŕŗřρр", "r" },
            { "ŚŜŞȘŠΣС", "S" },
            { "śŝşșšſσςс", "s" },
            { "ȚŢŤŦτТ", "T" },
            { "țţťŧт", "t" },
            { "ÙÚÛŨŪŬŮŰŲƯǓǕǗǙǛŨỦỤỪỨỮỬỰУ", "U" },
            { "ùúûũūŭůűųưǔǖǘǚǜυύϋủụừứữửựу", "u" },
            { "ÝŸŶΥΎΫỲỸỶỴЙ", "Y" },
            { "ýÿŷỳỹỷỵй", "y" },
            { "В", "V" },
            { "в", "v" },
            { "Ŵ", "W" },
            { "ŵ", "w" },
            { "ŹŻŽΖЗ", "Z" },
            { "źżžζз", "z" },
            { "ÆǼ", "AE" },
            { "ß", "ss" },
            { "Ĳ", "IJ" },
            { "ĳ", "ij" },
            { "Œ", "OE" },
            { "ƒ", "f" },
            { "ξ", "ks" },
            { "π", "p" },
            { "β", "v" },
            { "μ", "m" },
            { "ψ", "ps" },
            { "Ё", "Yo" },
            { "ё", "yo" },
            { "Є", "Ye" },
            { "є", "ye" },
            { "Ї", "Yi" },
            { "Ж", "Zh" },
            { "ж", "zh" },
            { "Х", "Kh" },
            { "х", "kh" },
            { "Ц", "Ts" },
            { "ц", "ts" },
            { "Ч", "Ch" },
            { "ч", "ch" },
            { "Ш", "Sh" },
            { "ш", "sh" },
            { "Щ", "Shch" },
            { "щ", "shch" },
            { "ЪъЬь", "" },
            { "Ю", "Yu" },
            { "ю", "yu" },
            { "Я", "Ya" },
            { "я", "ya" },
        };

        public static char RemoveDiacritics(this char c)
        {
            foreach (KeyValuePair<string, string> entry in NonStandardChars)
            {
                if (entry.Key.IndexOf(c) != -1)
                {
                    return entry.Value[0];
                }
            }
            return c;
        }

        public static string RemoveNonPrintableAscii(this string s)
        {
            return Regex.Replace(s, @"[^\u0020-\u007E]", string.Empty);
        }

        public static string ReplaceNonAscii(this string s)
        {
            //StringBuilder sb = new StringBuilder ();
            string text = "";

            foreach (char c in s)
            {
                bool found = false;

                foreach (KeyValuePair<string, string> entry in NonStandardChars)
                {
                    if (entry.Key.IndexOf(c) != -1)
                    {
                        text += entry.Value;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    text += c;
                }
            }

            return RemoveNonPrintableAscii(text);
        }

        private const string RandomChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string Random(int length)
        {
            if (length < 1)
            {
                throw new ArgumentException($"Length must be a positiver number, {length} given");
            }
            char[] chs = new char[length];
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = r.Next(0, RandomChars.Length - 1);
                char ch = RandomChars[index];
                chs[i] = ch;
            }
            return new string(chs);
        }

        public static bool IsCharAlphaLower(this char ch)
        {
            return ch >= 'a' && ch <= 'z';
        }

        public static bool IsCharAlphaUpper(this char ch)
        {
            return ch >= 'A' && ch <= 'Z';
        }

        public static bool IsCharNumber(this char ch)
        {
            return ch >= '0' && ch <= '9';
        }

        public static bool IsCharAlphaNumber(this char ch)
        {
            return ch.IsCharAlphaLower() || ch.IsCharAlphaUpper() || ch.IsCharNumber();
        }

        public static bool AllCharsAreNumbers(this string str)
        {
            return str.All(IsCharNumber);
        }

        public static bool AllCharsAreAlfaNumbers(this string str)
        {
            return str.All(IsCharAlphaNumber);
        }

        public static string Random(int length, ICollection<string> colisionList)
        {
            string result;
            do
            {
                result = Random(length);
            } while (colisionList.Contains(result));

            return result;
        }

        ///// <summary>
        ///// Create a StringNotNull object, throws if s is null
        ///// </summary>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //public static StringNotNull ToStringNotNull(this string s)
        //{
        //    return StringNotNull.Create(s);
        //}

        //public static StringNotNull ToStringNotNull(this string s, string dflt)
        //{
        //    return StringNotNull.Create(s ?? dflt);
        //}

        /// <summary>
        /// Substring func that doesn't throw when start < str.length even if start + length >= str.lenght
        /// It will return the substring from start to Min(str.Length - start, length)
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="startIndex">start index</param>
        /// <param name="length">max length</param>
        /// <returns></returns>
        public static string SubstringMax(this string str, int startIndex, int length)
        {
            return str.Substring(startIndex, Math.Min(str.Length - startIndex, length));
        }

        public static string SubstringEnd(this string s, int length)
        {
            length = Math.Min(length, s.Length);
            return s.Substring(s.Length - length, length);
        }

        /// <summary>
        /// Replaces elements in str where substrings "{name}" are replaced by value
        /// </summary>
        /// <param name="str">use string</param>
        /// <param name="name">name that will be search, must not provide braces</param>
        /// <param name="value">value to set</param>
        /// <returns>Interpolated string</returns>
        public static string Interpolate(this string str, string name, string value)
        {
            //ToDo: Escape
            return str.Replace("{" + name + "}", value);
        }

        /// <summary>
        /// Alias for Interpolate
        /// </summary>
        public static string It(this string str, string name, string value) => Interpolate(str, name, value);

        public static string Interpolate(this string str, params StrPair[] items)
        {
            string result = str;
            foreach (StrPair pair in items)
            {
                result = Interpolate(result, pair.Key, pair.Value);
            }
            return result;
        }

        public static string Wrap(this string str, string bounds)
        {
            return Wrap(str, bounds, bounds);
        }

        public static string Wrap(this string str, string startBound, string endBound)
        {
            return $"{startBound}{str}{endBound}";
        }


        public static string StringIsNullOrEmptyThen(in string str, in string dflt)
        {
            return string.IsNullOrEmpty(str) ? dflt : str;
        }

        public static string StringIsNullThen(in string str, in string dflt)
        {
            return str ?? dflt;
        }

        public static int LeadingCharCount(in string str) => LeadingCharCount(str, out _);
        public static int LeadingCharCount(in string str, out char ch)
        {
            ch = string.IsNullOrEmpty(str)
                ? (char)0
                : str[0];
            int count = 0;
            while (count < str.Length && str[count] == ch)
            {
                count++;
            }
            return count;
        }

        public static int LeadingCharCount(in char ch, in string str)
        {
            int result = LeadingCharCount(str, out char match);
            return ch == match ? result : 0;
        }

        public static string LeadingPad(in string str, in int count, in char ch = ' ')
        {
            int currentLeading = LeadingCharCount(ch, str);
            if (currentLeading >= count)
            {
                return str;
            }
            string addLeading = new string(ch, count - currentLeading);
            return addLeading + str;
        }

        public static string Base64StringEncode(this string plainText)
        {
            return Base64Util.ConvertStringUtf8ToBase64(plainText);
        }

        public static string Base64StringDecode(this string base64EncodedString)
        {
            return Base64Util.ConvertBase64ToUtf8(base64EncodedString);
        }

        public static string Join(this IEnumerable<string> en, string separator)
        {
            return string.Join(separator, en);
        }
    }
}
