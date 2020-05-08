
using System;
using System.Runtime.CompilerServices;

namespace Gamelib.Common
{
    

    internal static class ByteEx
    {
        internal static bool GetBit(this byte b, int bitNumber)
        {
            return ((b & (((int) 1) << bitNumber)) != 0);
        }

        internal static byte SetBit(this byte b, int bitNumber, bool set)
        {
            if (set)
            {
                return (b = (byte) (b | ((byte) (((int) 1) << bitNumber))));
            }
            return (b = (byte) (b & ((byte) ~(((int) 1) << bitNumber))));
        }
    }
}

