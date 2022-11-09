using System;
using System.Collections.Generic;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class BitUtil
    {
        // Fast access table to get a byte with only the n bit to 1
        private static readonly byte[] bitMaskBitsFromRightToLeft = new byte[8] { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };

        public static List<byte> ToByteList(List<bool> line)
        {
            List<byte> r = new List<byte>(MathUtil.DivCeil(line.Count, 8));
            byte i = 7;
            byte tmpByte = 0x00;
            foreach (bool b in line)
            {
                tmpByte = SetBitFromRightToLeft(tmpByte, i, b);
                if (i == 0)
                {
                    r.Add(tmpByte);
                    tmpByte = 0x00;
                    i = 7;
                }
                else
                {
                    i--;
                }
            }
            if (i != 0)
            {
                r.Add(tmpByte);
            }
            return r;
        }

        public static byte SetBitFromRightToLeft(byte srcByte, byte bitFromRightToleft, bool on)
        {
            return on
                ? SetBitOnFromRightToLeft(srcByte, bitFromRightToleft)
                : SetBitOffFromRightToleft(srcByte, bitFromRightToleft);
        }

        public static byte SetBitOnFromRightToLeft(byte srcByte, byte bitFromRightToLeft)
        {
            byte mask = bitMaskBitsFromRightToLeft[bitFromRightToLeft];
            srcByte = (byte)(srcByte | mask);

            return srcByte;
        }

        public static byte SetBitOffFromRightToleft(byte srcByte, byte bitFromRightToLeft)
        {
            byte mask = (byte)(~SingleBitOnFromRightToLeft(bitFromRightToLeft));
            srcByte = (byte)(srcByte & mask);

            return srcByte;
        }

        public static byte SingleBitOnFromRightToLeft(int bitFromRightToleft)
        {
            ValidateBit(bitFromRightToleft);
            return bitMaskBitsFromRightToLeft[bitFromRightToleft];
        }

        private static void ValidateBit(int bit)
        {
            if (bit < 0 || bit > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(bit), $"Invalid param \"{nameof(bit)}\" number({bit})");
            }
        }

        public static bool IsBitOnFromRightToLeft(byte b, byte bitPosFromRightToLeft)
        {
            byte mask = SingleBitOnFromRightToLeft(bitPosFromRightToLeft);

            return (b & mask) > 0x00;
        }

        public static bool IsBitSetStartingFromTheRight(int value, int bitPos)
        {
            return (value & (1 << bitPos)) != 0;
        }

        public static bool IsBitSetStartingFromTheRight(byte value, byte bitPos)
        {
            return (value & (1 << bitPos)) != 0;
        }
    }


}
