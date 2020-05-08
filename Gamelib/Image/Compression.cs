
using System;
using System.IO;

namespace Gamelib.Image
{

    public static class Compression
    {
        public static int CopyData(int dOffset, int cOffset, int Repeat, ref byte[] dStream)
        {
            while (Repeat > 0)
            {
                dStream[dOffset] = dStream[dOffset++ - cOffset];
                Repeat--;
            }
            return dOffset;
        }

        public static bool Decompress(BinaryReader b, ref byte[] dStream)
        {
            int num;
            int dOffset = 0;
            while ((num = b.ReadByte()) != -1)
            {
                if ((num & 0x80) == 0)
                {
                    dStream[dOffset++] = (byte) num;
                }
                else
                {
                    int num3 = (num >> 3) & 15;
                    int cOffset = b.ReadByte();
                    if (cOffset == -1)
                    {
                        return false;
                    }
                    cOffset = ((num << 8) | cOffset) & 0x7ff;
                    if (num3 == 0)
                    {
                        if (cOffset == 0)
                        {
                            return false;
                        }
                        int num5 = b.ReadByte();
                        if (num5 == -1)
                        {
                            return false;
                        }
                        dOffset = CopyData(dOffset, cOffset, num5 + 0x12, ref dStream);
                        continue;
                    }
                    dOffset = CopyData(dOffset, cOffset + 1, num3 + 2, ref dStream);
                }
            }
            return true;
        }
    }
}

