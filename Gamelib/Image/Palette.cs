using System;
using System.Drawing;
using System.IO;

namespace Gamelib.Image
{
  

    public class Palette
    {
        public byte[,] Data;

        public Palette(byte[,] Data)
        {
            this.Data = Data;
        }

        public static Bitmap ToBitmap(byte[,] Palette, byte[] BackColor)
        {
            MemoryStream output = new MemoryStream(0x4436);
            using (BinaryWriter writer = new BinaryWriter(output))
            {
                writer.Write((short) 0x4d42);
                writer.Write(0x4436);
                writer.Write((short) 0);
                writer.Write((short) 0);
                writer.Write(0x436);
                writer.Write(40);
                writer.Write(0x80);
                writer.Write(0x80);
                writer.Write((short) 1);
                writer.Write((short) 8);
                writer.Write(0);
                writer.Write(0x4000);
                writer.Write(0x1ec2);
                writer.Write(0x1ec2);
                writer.Write(0x100);
                writer.Write(0);
                writer.Write(BackColor);
                for (int i = 0; i < 0xff; i++)
                {
                    if (i <= Palette.GetUpperBound(0))
                    {
                        writer.Write(Palette[i, 2]);
                        writer.Write(Palette[i, 1]);
                        writer.Write(Palette[i, 0]);
                        writer.Write((byte) 0);
                    }
                    else
                    {
                        writer.Write(0);
                    }
                }
                for (int j = 15; j >= 0; j--)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        for (int m = 0; m < 0x10; m++)
                        {
                            for (int n = 0; n < 8; n++)
                            {
                                writer.Write((byte) ((j * 0x10) + m));
                            }
                        }
                    }
                }
                return new Bitmap(output);
            }
        }
    }
}

