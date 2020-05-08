using System;
using System.Collections.Generic;
using System.IO;
using Gamelib.Image;

namespace Gamelib.Image
{
   

    public class FNT
    {
        public byte BitRate;
        public byte[] Char;
        public List<byte> Description = new List<byte>();
        public short Dimension;
        public int FileLength;
        public bool FlagCompressed;
        public bool FlagPalette;
        public byte Flags;
        public short FrameCount;
        public byte[,] Palette;
        public short PaletteCount;
        public int Signature;

        public FNT(BinaryReader b, ref List<Sprite> iSprite, ref List<Frame> iFrame)
        {
            this.Signature = b.ReadInt32();
            this.FileLength = b.ReadInt32();
            this.BitRate = b.ReadByte();
            if ((this.BitRate != 1) && (this.BitRate != 8))
            {
                System.IO.Stream baseStream = b.BaseStream;
                baseStream.Position -= 1L;
                for (int i = 0; i < 0x100; i++)
                {
                    this.Description.Add(b.ReadByte());
                    if (this.Description[i] == 0)
                    {
                        break;
                    }
                }
                this.BitRate = b.ReadByte();
            }
            this.Flags = b.ReadByte();
            if ((this.Flags == 0x80) || (this.Flags == 0xc0))
            {
                this.FlagPalette = true;
            }
            if ((this.Flags == 0x40) || (this.Flags == 0xc0))
            {
                this.FlagCompressed = true;
            }
            if (this.FlagPalette)
            {
                this.PaletteCount = b.ReadInt16();
                this.Palette = new byte[this.PaletteCount, 3];
                for (int j = 0; j < this.PaletteCount; j++)
                {
                    this.Palette[j, 0] = b.ReadByte();
                    this.Palette[j, 1] = b.ReadByte();
                    this.Palette[j, 2] = b.ReadByte();
                }
            }
            this.Char = b.ReadBytes(0x100);
            this.Dimension = b.ReadInt16();
            this.FrameCount = b.ReadInt16();
            if (!this.FlagCompressed)
            {
                if (this.Description.Count == 0)
                {
                    int num3 = (4 - (((int) b.BaseStream.Position) % 4)) % 4;
                    for (int k = 0; k < num3; k++)
                    {
                        b.ReadByte();
                    }
                    for (int m = 0; m < this.FrameCount; m++)
                    {
                        short cropL = b.ReadInt16();
                        short cropU = b.ReadInt16();
                        short num8 = b.ReadInt16();
                        short num9 = b.ReadInt16();
                        iFrame.Add(new Frame(this.Palette, 0, 0, cropL, cropU, (short) (cropL + num8), (short) (cropU + num9)));
                        iFrame[m].Position = (short) b.ReadInt32();
                    }
                    for (int n = 0; n < this.FrameCount; n++)
                    {
                        iFrame[n].Data = b.ReadBytes(iFrame[n].Size);
                    }
                }
                else
                {
                    for (int num11 = 0; num11 < this.FrameCount; num11++)
                    {
                        iFrame.Add(new Frame(this.Palette, 0, 0, b.ReadInt16(), b.ReadInt16(), b.ReadInt16(), b.ReadInt16()));
                        iFrame[num11].Data = b.ReadBytes(iFrame[num11].Size);
                    }
                }
            }
        }
    }
}

