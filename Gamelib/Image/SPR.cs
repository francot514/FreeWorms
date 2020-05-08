using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Gamelib.Image
{
    

    public class SPR
    {
        public short AnimationFlags;
        public byte BitRate;
        public List<byte> Description = new List<byte>();
        public int FileLength;
        public bool FlagCompressed;
        public bool FlagPalette;
        public byte Flags;
        public short FrameCount;
        public short FrameRate;
        public short Height;
        public byte[,] Palette;
        public short PaletteCount;
        public int Signature;
        public int Streams;
        public short Unknown4;
        public short Width;
        public List<Sprite> Sprites;
        public List<Frame> Frames;

        public SPR(BinaryReader b, ref List<Sprite> iSprite, ref List<Frame> iFrame)
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
            if (this.FlagCompressed)
            {
                this.Streams = b.ReadInt16();
                if (this.Description.Count == 0)
                {
                    int num3 = (4 - (((int) b.BaseStream.Position) % 4)) % 4;
                    for (int num4 = 0; num4 < num3; num4++)
                    {
                        b.ReadByte();
                    }
                }
                Stream[] streamArray = new Stream[this.Streams];
                for (int k = 0; k < this.Streams; k++)
                {
                    streamArray[k].CompressedStart = b.ReadInt32();
                    streamArray[k].Unknown = b.ReadInt32();
                    streamArray[k].DecompressedLength = b.ReadInt32();
                }
                this.Unknown4 = b.ReadInt16();
                this.FrameRate = b.ReadInt16();
                this.AnimationFlags = b.ReadInt16();
                this.Width = b.ReadInt16();
                this.Height = b.ReadInt16();
                this.FrameCount = b.ReadInt16();
                for (int m = 0; m < this.FrameCount; m++)
                {
                    short position = b.ReadInt16();
                    b.ReadByte();
                    byte stream = b.ReadByte();
                    iFrame.Add(new Frame(this.Palette, stream, position, b.ReadInt16(), b.ReadInt16(), b.ReadInt16(), b.ReadInt16()));
                }
                for (int n = 0; n < this.Streams; n++)
                {
                    streamArray[n].Data = new byte[streamArray[n].DecompressedLength];
                    Compression.Decompress(b, ref streamArray[n].Data);
                }
                for (int num10 = 0; num10 < this.FrameCount; num10++)
                {
                    for (int num11 = 0; num11 < iFrame[num10].Size; num11++)
                    {
                        //if (streamArray[iFrame[num10].Stream].Data[(iFrame[num10].Position - 1) + num11] <= streamArray[iFrame[num10].Stream].Data.Length)
                        iFrame[num10].Data[num11] = streamArray[iFrame[num10].Stream].Data[(iFrame[num10].Position - 1) + num11];
                    }
                }
            }
            else
            {
                this.Unknown4 = b.ReadInt16();
                this.FrameRate = b.ReadInt16();
                this.AnimationFlags = b.ReadInt16();
                this.Width = b.ReadInt16();
                this.Height = b.ReadInt16();
                this.FrameCount = b.ReadInt16();
                int num12 = (4 - (((int) b.BaseStream.Position) % 4)) % 4;
                for (int num13 = 0; num13 < num12; num13++)
                {
                    b.ReadByte();
                }
                for (int num14 = 0; num14 < this.FrameCount; num14++)
                {
                    iFrame.Add(new Frame(this.Palette, 0, (short) b.ReadInt32(), b.ReadInt16(), b.ReadInt16(), b.ReadInt16(), b.ReadInt16()));
                }
                for (int num15 = 0; num15 < this.FrameCount; num15++)
                {
                    iFrame[num15].Data = b.ReadBytes(iFrame[num15].Size);
                }
            }
            iSprite.Add(new Sprite(this.Palette, this.AnimationFlags, this.Width, this.Height, 0, this.FrameCount, 0, (byte) this.FrameRate));
            iSprite[iSprite.Count - 1].CalculateCrop(ref iFrame);
            if (this.Description.Count != 0)
            {
                iSprite[iSprite.Count - 1].Description = this.Description;
            }

            Sprites = iSprite;
            Frames = iFrame;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream
        {
            public int DecompressedLength;
            public int CompressedStart;
            public int Unknown;
            public byte[] Data;
        }
    }
}

