using System;
using System.Text;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class Base64Util
    {
        public static string ConvertBytesToBase64(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static byte[] ConvertBase64ToBytes(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        public static string ConvertStringUtf8ToBase64(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return ConvertBytesToBase64(plainTextBytes);
        }

        public static string ConvertBase64ToUtf8(string base64)
        {
            byte[] plainTextBytes = ConvertBase64ToBytes(base64);
            return Encoding.UTF8.GetString(plainTextBytes);
        }


        //public static string BytesToBase64(byte[] data)
        //{
        //    return Encoding.UTF8.GetString(data);
        //}

        //public static byte[] Base64ToBytes(string base64)
        //{
        //    return Encoding.UTF8.GetBytes(base64);
        //}

        //public static string BytesToBase64Ascii(byte[] data)
        //{
        //    return Encoding.ASCII.GetString(data);
        //}

        //public static byte[] Base64ToBytesAscii(string base64)
        //{
        //    return Encoding.ASCII.GetBytes(base64);
        //}

        //public static string Base64StringEncode(string plainText)
        //{
        //    byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        //    return Convert.ToBase64String(plainTextBytes);
        //}

        //public static string Base64StringDecode(string base64EncodedString)
        //{
        //    byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedString);
        //    return Encoding.UTF8.GetString(base64EncodedBytes);
        //}
    }
}
