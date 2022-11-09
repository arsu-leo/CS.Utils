using System;
using System.Linq;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class BuffUtil
    {

        //public static int ReadInt(IEnumerable<byte> data, int start, int size)
        //{
        //    return ReadInt(data.Skip(start).Take(size));
        //}
        //public static int ReadInt(IEnumerable<byte> data, int start, int size, bool bigEndian)
        //{
        //    return ReadInt(data.Skip(start).Take(size), bigEndian);
        //}
        //public static int ReadInt(IEnumerable<byte> data, bool bigEndian)
        //{
        //    if (data.Count() <= 0)
        //    {
        //        return -1;
        //    }
        //    IEnumerable<byte> dataLittleEndian = bigEndian ? data.Reverse() : data;

        //    int value = 0;
        //    foreach (byte b in dataLittleEndian)
        //    {
        //        value <<= 8;
        //        value += b;
        //    }

        //    return value;
        //}

        //public static int ReadInt(IEnumerable<byte> data)
        //{
        //    return ReadInt(data, true);
        //}

        public static int ReadIntSpanBigEndian(ReadOnlySpan<byte> data)
        {
            return ReadIntSpan(data, true);
        }

        public static long ReadLongSpan(ReadOnlySpan<byte> data, bool bigEndian)
        {
            if (data.Length <= 0)
            {
                return -1;
            }
            ReadOnlySpan<byte> bigEndianData = bigEndian ? data : data.GetReverse();
            long value = bigEndianData[^1];
            for (int i = bigEndianData.Length - 2; i >= 0; i--)
            {
                value <<= 8;
                value += bigEndianData[i];
            }
            return value;
        }
        public static int ReadIntSpan(ReadOnlySpan<byte> data, bool bigEndian)
        {
            if (data.Length <= 0)
            {
                return -1;
            }
            ReadOnlySpan<byte> bigEndianData = bigEndian ? data : data.GetReverse();
            int value = bigEndianData[^1];
            for (int i = bigEndianData.Length - 2; i >= 0; i--)
            {
                value <<= 8;
                value += bigEndianData[i];
            }
            return value;
            //ReadOnlySpan<byte> dataLittleEndian = bigEndian
            //    ? data.GetReverse()
            //    : data;
            //int value = 0;
            //foreach (byte b in dataLittleEndian)
            //{
            //    value <<= 8;
            //    value += b;
            //}

            //return value;
        }

        public static string ReadStringSpan(ReadOnlySpan<byte> data)
        {
            Span<char> chr = data.Length > 256
                 ? new char[data.Length]
                 : stackalloc char[data.Length];
            int pos = 0;
            while (pos < data.Length && data[pos] != 0x00)
            {
                chr[pos] = (char)data[pos];
                pos++;
            }
            return new string(chr.Slice(0, pos));

            //char[] chs = data.Select((b) => { return (char)b; }).TakeWhile((ch) => ch != 0x00).ToArray();
            //return new string(chs);
        }

        public static string ReadStringSpan(ReadOnlySpan<byte> data, int start, int length)
        {
            return ReadStringSpan(data.Slice(start, length));
        }

        //public static string ReadString(IEnumerable<byte> data)
        //{
        //    char[] chs = data.Select((b) => { return (char)b; }).TakeWhile((ch) => ch != 0x00).ToArray();
        //    return new string(chs);
        //}

        //public static string ReadString(IEnumerable<byte> data, int start, int length)
        //{
        //    return ReadString(data.Skip(start).Take(length));
        //}

        //public static Span<byte> UIntToData(uint value, int bytes, bool bigEndian)
        //{
        //    if (bytes < 1)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(bytes), $"Invalid {nameof(bytes)} value({bytes})");
        //    }
        //    Span<byte> bigEndianData = CreateBuffer(bytes, 0x00);
        //    _ = WriteUIntIntoSpan(bigEndianData, value, bytes, bigEndian);
        //    return bigEndianData;
        //}

        public static int FillN(Span<byte> dest, byte value)
        {
            dest.Fill(value);
            return dest.Length;
        }
        public static int FillN(Span<byte> dest, byte value, int start = 0, int maxLength = int.MaxValue)
        {
            return FillN(dest.Slice(start, Math.Min(dest.Length, maxLength)), value);
        }

        public static int WriteUIntIntoSpan(Span<byte> dest, uint value, int writeBytesCount, bool bigEndian)
        {
            if (writeBytesCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(writeBytesCount), $"Invalid {nameof(writeBytesCount)} value({writeBytesCount})");
            }
            if (writeBytesCount > dest.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(writeBytesCount), $"Invalid dest size {dest.Length} < {writeBytesCount}");
            }
            if (value == 0)
            {
                dest.Slice(writeBytesCount).Fill(0x00);
                return writeBytesCount;
            }
            int index = 0;
            do
            {
                byte num = (byte)(0xFF & value);
                dest[index] = num;
                value >>= 8;
                index++;
            }
            while (value != 0 && index < dest.Length);

            if (!bigEndian)
            {
                dest.Slice(writeBytesCount).Reverse();
            }
            return writeBytesCount;
        }

        //public static byte UIntToByte(uint value)
        //{
        //    if (value == 0)
        //    {
        //        return 0x00;
        //    }
        //    return (byte)(0xFF & value);
        //}

        public static byte IntToByte(int value)
        {
            if (value == 0)
            {
                return 0x00;
            }
            return (byte)(0xFF & value);
        }


        //public static Span<byte> StringToData(string str)
        //{
        //    return StringToData(str, str.Length, (byte)' ');
        //}

        public static int WriteStringToByteSpan(Span<byte> dest, string str)
        {
            int written = 0;
            while (written < dest.Length && written < str.Length)
            {
                dest[written] = (byte)str[written];
                written++;
            }
            return written;
        }

        public static int WriteStringToByteSpan(Span<byte> buffer, string content, int fixSize, byte fill)
        {
            if (buffer.Length < fixSize)
            {
                throw new ArgumentException($"Destinator is smaller than content (Dest: {buffer.Length}, size: {fixSize})");
            }
            //Ensure we are not using a bigger buffer
            if (buffer.Length > fixSize)
            {
                buffer = buffer.Slice(0, fixSize);
            }
            //Write fill on to other bytes
            if (buffer.Length > content.Length)
            {
                buffer.Slice(content.Length).Fill(fill);
            }
            _ = WriteStringToByteSpan(buffer, content);
            return fixSize;
        }

        public static Span<byte> StringToByteData(string str, int fixSize, byte fill)
        {
            Span<byte> result = new byte[fixSize];
            _ = WriteStringToByteSpan(result, str, fixSize, fill);
            return result;
        }

        public static Span<byte> CreateByteBuffer(int size, byte value)
        {
            return CreateBuffer(size, value);
        }

        public static Span<T> CreateBuffer<T>(int size, T value)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), $"Param \"{nameof(size)}\"can't be below zero, \"{size}\" given");
            }
            Span<T> line = new T[size];
            line.Fill(value);
            return line;
        }

        //public static Span<byte> CreateBuffer(int size)
        //{
        //    return CreateBuffer(size, 0x00);
        //}
    }
}
