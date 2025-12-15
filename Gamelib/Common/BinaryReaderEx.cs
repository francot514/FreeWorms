using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Gamelib.Common
{
    

    internal static class BinaryReaderEx
    {
        internal static string ReadString(this BinaryReader br, int length)
        {
            return br.ReadString(length, new ASCIIEncoding());
        }

        internal static string ReadString(this BinaryReader br, BinaryStringFormat format)
        {
            return br.ReadString(format, new ASCIIEncoding());
        }

        internal static string ReadString(this BinaryReader br, int length, Encoding encoding)
        {
            return encoding.GetString(br.ReadBytes(length));
        }

        internal static string ReadString(this BinaryReader br, BinaryStringFormat format, Encoding encoding)
        {
            if (format == BinaryStringFormat.VariableLengthPrefix)
            {
                return br.ReadString();
            }
            if (format == BinaryStringFormat.WordLengthPrefix)
            {
                int count = br.ReadInt32();
                return encoding.GetString(br.ReadBytes(count));
            }
            if (format == BinaryStringFormat.ZeroTerminated)
            {
                List<byte> list = new List<byte>();
                for (byte i = br.ReadByte(); i != 0; i = br.ReadByte())
                {
                    list.Add(i);
                }
                return encoding.GetString(list.ToArray());
            }
            if (format == BinaryStringFormat.NoPrefixOrTermination)
            {
                throw new ArgumentException("NoPrefixOrTermination cannot be used for read operations. Specify the length of the string instead to read strings with no prefix or terminator.");
            }
            throw new ArgumentOutOfRangeException("The specified binary string format is invalid.");
        }
    }
}

