using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Gamelib.Image
{
 

    public class Sprite
    {
        public int CropD;
        public int CropL;
        public int CropR;
        public int CropU;
        public List<byte> Description = new List<byte>();
        public bool FlagLoop;
        public bool FlagReverse;
        public short Flags;
        public short FrameCount;
        public byte FrameRate;
        public short FrameStart;
        public short Height;
        public byte[,] Palette;
        public byte Unknown;
        public short Width;

        public Sprite(byte[,] Palette, short Flags, short Width, short Height, short FrameStart, short FrameCount, byte Unknown, byte FrameRate)
        {
            this.Palette = Palette;
            this.Flags = Flags;
            this.Width = Width;
            this.Height = Height;
            this.FrameStart = FrameStart;
            this.FrameCount = FrameCount;
            this.Unknown = Unknown;
            this.FrameRate = FrameRate;
            if ((Flags == 1) || (Flags == 3))
            {
                this.FlagLoop = true;
            }
            if ((Flags == 2) || (Flags == 3))
            {
                this.FlagReverse = true;
            }
        }

        public void CalculateCrop(ref List<Frame> iFrame)
        {
            this.CropL = this.Width;
            this.CropU = this.Height;
            this.CropR = 0;
            this.CropD = 0;
            for (int i = 0; i < this.FrameCount; i++)
            {
                if (iFrame[this.FrameStart + i].Width != 0)
                {
                    if (iFrame[this.FrameStart + i].CropL < this.CropL)
                    {
                        this.CropL = iFrame[this.FrameStart + i].CropL;
                    }
                    if (iFrame[this.FrameStart + i].CropU < this.CropU)
                    {
                        this.CropU = iFrame[this.FrameStart + i].CropU;
                    }
                    if (iFrame[this.FrameStart + i].CropR > this.CropR)
                    {
                        this.CropR = iFrame[this.FrameStart + i].CropR;
                    }
                    if (iFrame[this.FrameStart + i].CropD > this.CropD)
                    {
                        this.CropD = iFrame[this.FrameStart + i].CropD;
                    }
                }
            }
        }

        public Bitmap ToBitmap(ref BinaryWriter b, ref List<Frame> iFrame, byte[] BackColor, bool MakeTransparent, int NewCropL, int NewCropU, int NewCropR, int NewCropD)
        {
            if (NewCropL > this.CropL)
            {
                NewCropL = this.CropL;
            }
            if (NewCropU > this.CropU)
            {
                NewCropU = this.CropU;
            }
            if (NewCropR < this.CropR)
            {
                NewCropR = this.CropR;
            }
            if (NewCropD < this.CropD)
            {
                NewCropD = this.CropD;
            }
            int num = NewCropR - NewCropL;
            int num2 = NewCropD - NewCropU;
            int num3 = (num * num2) * this.FrameCount;
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
            b.Write((int) (num2 * this.FrameCount));
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
            for (int j = (this.FrameStart + this.FrameCount) - 1; j >= this.FrameStart; j--)
            {
                for (int k = num2 - 1; k >= 0; k--)
                {
                    for (int m = 0; m < num; m++)
                    {
                        if (((m >= (iFrame[j].CropL - NewCropL)) && (m < (iFrame[j].CropR - NewCropL))) && ((k >= (iFrame[j].CropU - NewCropU)) && (k < (iFrame[j].CropD - NewCropU))))
                        {
                            b.Write(iFrame[j].Data[((k - (iFrame[j].CropU - NewCropU)) * iFrame[j].Width) + (m - (iFrame[j].CropL - NewCropL))]);
                        }
                        else
                        {
                            b.Write((byte) 0);
                        }
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

