using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Gamelib.Common
{
   

    internal static class BinaryWriterEx
    {
        internal static void Write(this BinaryWriter bw, string value, BinaryStringFormat format)
        {
            bw.Write(value, format, new ASCIIEncoding());
        }

        internal static void Write(this BinaryWriter bw, string value, BinaryStringFormat format, Encoding encoding)
        {
            if (format == BinaryStringFormat.VariableLengthPrefix)
            {
                bw.Write(value);
            }
            else if (format == BinaryStringFormat.WordLengthPrefix)
            {
                bw.Write(value.Length);
                bw.Write(encoding.GetBytes(value));
            }
            else if (format == BinaryStringFormat.ZeroTerminated)
            {
                bw.Write(encoding.GetBytes(value));
                bw.Write((byte) 0);
            }
            else if (format == BinaryStringFormat.NoPrefixOrTermination)
            {
                bw.Write(encoding.GetBytes(value));
            }
        }
    }
}

