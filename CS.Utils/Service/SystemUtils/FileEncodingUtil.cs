using System;
using System.Text;

namespace ArsuLeo.CS.Utils.Service.SystemUtils
{
    public static class FileEncodingUtil
    {
        public static Encoding? GetEncodingFromFirstBytes(ReadOnlySpan<byte> firstBytes)
        {
            if (firstBytes == null || firstBytes.Length < 4)
            {
                return null;
            }

            // Analyze the BOM
            if (firstBytes[0] == 0x2b && firstBytes[1] == 0x2f && firstBytes[2] == 0x76) return Encoding.UTF7;
            if (firstBytes[0] == 0xef && firstBytes[1] == 0xbb && firstBytes[2] == 0xbf) return Encoding.UTF8;
            if (firstBytes[0] == 0xff && firstBytes[1] == 0xfe && firstBytes[2] == 0 && firstBytes[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (firstBytes[0] == 0xff && firstBytes[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (firstBytes[0] == 0xfe && firstBytes[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (firstBytes[0] == 0 && firstBytes[1] == 0 && firstBytes[2] == 0xfe && firstBytes[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            // We actually have no idea what the encoding is if we reach this point, so
            // you may wish to return null instead of defaulting to ASCII
            //return Encoding.ASCII;
            return null;
        }
    }
}
