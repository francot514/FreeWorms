
using System;
using System.Drawing;
using System.IO;

namespace Gamelib.Image
{

    public class Frame
    {
        public short CropD;
        public short CropL;
        public short CropR;
        public short CropU;
        public byte[] Data;
        public int Height;
        public byte[,] Palette;
        public short Position;
        public int Size;
        public short Stream;
        public int Width;
        public Bitmap Bitmap;

        public Frame(byte[,] Palette, short Stream, short Position, short CropL, short CropU, short CropR, short CropD)
        {
            this.Palette = Palette;
            this.Stream = Stream;
            this.Position = Position;
            this.CropL = CropL;
            this.CropU = CropU;
            this.CropR = CropR;
            this.CropD = CropD;
            this.Width = CropR - CropL;
            this.Height = CropD - CropU;
            this.Size = this.Width * this.Height;
            this.Data = new byte[this.Size];
           
        }

        public Frame(byte[,] Palette, short Stream, short Position, short CropL, short CropU, short CropR, short CropD, byte[] Data)
        {
            this.Palette = Palette;
            this.Stream = Stream;
            this.Position = Position;
            this.CropL = CropL;
            this.CropU = CropU;
            this.CropR = CropR;
            this.CropD = CropD;
            this.Width = CropR - CropL;
            this.Height = CropD - CropU;
            this.Size = this.Width * this.Height;
            this.Data = Data;
        }

        public Bitmap ToBitmap(ref BinaryWriter b, byte[] BackColor, bool MakeTransparent, int NewCropL, int NewCropU, int NewCropR, int NewCropD)
        {
            int num = NewCropR - NewCropL;
            int num2 = NewCropD - NewCropU;
            int num3 = num * num2;
            short num4 = 8;
            int num5 = (int) Math.Pow(2.0, (double) num4);
            int num6 = num5 * 4;
            if (num4 > 8)
            {
                num6 = 0;
            }
            float num7 = ((float) num4) / 8f;
            b.Write((short) 0x4d42);
            b.Write((int) ((0x36 + num6) + ((int) (num3 * num7))));
            b.Write((short) 0);
            b.Write((short) 0);
            b.Write((int) (0x36 + num6));
            b.Write(40);
            b.Write(num);
            b.Write(num2);
            b.Write((short) 1);
            b.Write(num4);
            b.Write(0);
            b.Write((int) (num3 * num7));
            b.Write(0x1ec2);
            b.Write(0x1ec2);
            b.Write(num5);
            b.Write(0);
            b.Write(BackColor);
            for (int i = 0; i < 0xff; i++)
            {
                if (i <= this.Palette.GetUpperBound(0))
                {
                    b.Write(this.Palette[i, 2]);
                    b.Write(this.Palette[i, 1]);
                    b.Write(this.Palette[i, 0]);
                    b.Write((byte) 0);
                }
                else
                {
                    b.Write(0);
                }
            }
            num += (4 - (num % 4)) % 4;
            for (int j = num2 - 1; j >= 0; j--)
            {
                for (int k = 0; k < num; k++)
                {
                    if (((k >= (this.CropL - NewCropL)) && (k < (this.CropR - NewCropL))) && ((j >= (this.CropU - NewCropU)) && (j < (this.CropD - NewCropU))))
                    {
                        b.Write(this.Data[((j - (this.CropU - NewCropU)) * this.Width) + (k - (this.CropL - NewCropL))]);
                    }
                    else
                    {
                        b.Write((byte) 0);
                    }
                }
            }
            Bitmap bitmap = new Bitmap(b.BaseStream);
            if (MakeTransparent)
            {
                bitmap.MakeTransparent(Color.FromArgb(BackColor[3], BackColor[2], BackColor[1], BackColor[0]));
            }
            return bitmap;
        }
    }
}

