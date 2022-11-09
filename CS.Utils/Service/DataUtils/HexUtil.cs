using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class HexUtil
    {
        public static readonly char[] HEXMAP = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public static string HexVal2str(byte ch)
        {
            int hIndex = (ch & 0xF0) >> 4;
            int lIndex = ch & 0x0F;

            string result = "";

            result += HEXMAP[hIndex];
            result += HEXMAP[lIndex];

            return result;
        }
        public static string[] Hex2strSpan(ReadOnlySpan<byte> bytes)
        {
            string[] result = new string[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                result[i] = HexVal2str(bytes[i]);
            }
            return result;
        }
        public static IEnumerable<string> Hex2strEnu(IEnumerable<byte> bytes)
        {
            return bytes.Select(HexVal2str);
        }

        //public static IEnumerable<string> Hex2strEnu(IEnumerable<byte> bytes, int count)
        //{
        //    string[] result = new string[count];
        //    var en = bytes.GetEnumerator();
        //    int i = 0;
        //    while (en.MoveNext())
        //    {
        //        result[i] = Hex2str(en.Current);
        //        i++;
        //    }
        //    return result;
        //}

        public static string Hex2strSpan(ReadOnlySpan<byte> bytes, string separator)
        {
            IEnumerable<string> tmp = Hex2strSpan(bytes);
            return string.Join(separator, tmp);
        }

        public static string Hex2strSpan(ReadOnlySpan<byte> bytes, string separator, string prependElements)
        {
            IEnumerable<string> tmp = Hex2strSpan(bytes);
            tmp = tmp.Select(element => prependElements + element);
            return string.Join(separator, tmp);
        }

        public static string Hex2str(IEnumerable<byte> bytes, string separator, string prependElements)
        {
            StringBuilder result = new StringBuilder();
            using (IEnumerator<byte> en = bytes.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    result.Append(prependElements);
                    result.Append(en.Current);
                    while (en.MoveNext())
                    {
                        result.Append(separator);
                        result.Append(prependElements);
                        result.Append(HexVal2str(en.Current));
                    }
                }
            }
            return result.ToString();
        }

        public static string Hex2str(IEnumerable<byte> bytes, string separator)
        {
            StringBuilder result = new StringBuilder();
            using (IEnumerator<byte> en = bytes.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    result.Append(en.Current);
                    while (en.MoveNext())
                    {
                        result.Append(separator);
                        result.Append(HexVal2str(en.Current));
                    }
                }
            }
            return result.ToString();
        }

        public static string Hex2str(IEnumerable<byte> bytes, string separator, string prependElements, int newLineEvery)
        {
            int used = 0;
            IEnumerable<string> tmp = new List<string>();
            string txtNewLine = Environment.NewLine;

            while (used < bytes.Count())
            {
                int start = used;
                int end = Math.Min(used + newLineEvery, bytes.Count());
                IEnumerable<byte> byteLine = bytes.Skip(start).Take(end - start);
                string intermediate = Hex2str(byteLine, separator, prependElements);
                tmp.Append(intermediate);
                used += byteLine.Count();
            }

            return string.Join(txtNewLine, tmp);
        }

        public static int HexVal(char ch)
        {
            if (ch >= 'a' && ch <= 'f')
            {
                ch = (char)(ch + 'A' - 'a');
            }
            int index = HEXMAP.Length - 1;
            while (index > 0 && HEXMAP[index] != ch)
            {
                index--;
            }

            return index;
        }

        public static int HexValPair(string st)
        {
            if (st.Length > 2)
            {
                throw new ArgumentException("Invalid value \"" + st + "\", expected 2 char length", nameof(st));
            }
            char high = st.Length > 1 ? st[0] : '0';
            char low = st.Length > 0 ? st[1] : '0';
            int highVal = HexVal(high);
            highVal <<= 4;
            int lowVal = HexVal(low);
            return highVal + lowVal;
        }

        public static int HexVal(string chars, bool compensateIfOdd = true)
        {
            int acc = 0;
            if (compensateIfOdd && chars.Length % 2 == 1)
            {
                chars = "0" + chars;
            }
            for (int i = 0; i + 1 < chars.Length; i += 2)
            {
                char high = chars[i];
                char low = chars[i + 1];
                int highVal = HexVal(high);
                int lowVal = HexVal(low);
                int pairVal = lowVal + (highVal << 4);
                acc = pairVal + (acc << 8);
            }
            return acc;
        }

        public static string AsHex(this byte n, string format = "X")
        {
            return n.ToString(format);
        }

        public static string TryReadHexAsAscii(string st, string separator = "", bool compensateIfOdd = true, bool alpha = true, bool num = true, bool symb = true, bool explicitUnDecodableHexPrepend = true)
        {
            if (compensateIfOdd && st.Length % 2 == 1)
            {
                st = "0" + st;
            }
            StringBuilder sb = new StringBuilder();
            Span<byte> sp = stackalloc byte[1];
            for (int i = 0; i + 1 < st.Length; i += 2)
            {
                if (i > 0)
                {
                    sb.Append(separator);
                }
                char high = st[i];
                char low = st[i + 1];
                int lowVal = HexVal(low);
                int highVal = HexVal(high);
                int pairVal = lowVal + (highVal << 4);

                bool tryDecode =
                    (pairVal >= 65 && pairVal <= 90 || pairVal >= 97 && pairVal <= 122) && alpha
                    || pairVal >= 48 && pairVal <= 57 && num
                    || pairVal >= 32 && pairVal <= 126 && symb;
                if (tryDecode)
                {
                    sp[0] = (byte)pairVal;
                    string v = Encoding.ASCII.GetString(sp);
                    sb.Append(v);
                }
                else
                {
                    if (explicitUnDecodableHexPrepend)
                    {
                        sb.Append("0x");
                    }
                    sb.Append(high);
                    sb.Append(low);
                }
            }
            return sb.ToString();
        }
    }
}
