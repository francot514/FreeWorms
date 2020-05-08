using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Gamelib.Image;


namespace Gamelib.Common
{
   
    public class BNK
    {
        public int FileLength;
        public int Frames;
        public byte[,] Palette;
        public short PaletteCount;
        public int Signature;
        public int Sprites;
        public int Streams;

        public BNK(BinaryReader b, ref List<Sprite> iSprite, ref List<Frame> iFrame)
        {
            this.Signature = b.ReadInt32();
            this.FileLength = b.ReadInt32();
            this.PaletteCount = b.ReadInt16();
            this.Palette = new byte[this.PaletteCount, 3];
            for (int i = 0; i < this.PaletteCount; i++)
            {
                this.Palette[i, 0] = b.ReadByte();
                this.Palette[i, 1] = b.ReadByte();
                this.Palette[i, 2] = b.ReadByte();
            }
            int num2 = (4 - (((int) b.BaseStream.Position) % 4)) % 4;
            for (int j = 0; j < num2; j++)
            {
                b.ReadByte();
            }
            this.Sprites = b.ReadInt32();
            for (int k = 0; k < this.Sprites; k++)
            {
                iSprite.Add(new Sprite(this.Palette, b.ReadInt16(), b.ReadInt16(), b.ReadInt16(), b.ReadInt16(), b.ReadInt16(), b.ReadByte(), b.ReadByte()));
            }
            this.Frames = b.ReadInt32();
            for (int m = 0; m < this.Frames; m++)
            {
                iFrame.Add(new Frame(this.Palette, b.ReadInt16(), b.ReadInt16(), b.ReadInt16(), b.ReadInt16(), b.ReadInt16(), b.ReadInt16()));
            }
            for (int n = 0; n < iSprite.Count; n++)
            {
                iSprite[n].CalculateCrop(ref iFrame);
            }
            this.Streams = b.ReadInt32();
            Stream[] streamArray = new Stream[this.Streams];
            for (int num7 = 0; num7 < this.Streams; num7++)
            {
                streamArray[num7].CompressedStart = b.ReadInt32();
                streamArray[num7].DecompressedLength = b.ReadInt32();
                streamArray[num7].Unknown = b.ReadInt32();
            }
            for (int num8 = 0; num8 < this.Streams; num8++)
            {
                streamArray[num8].Data = new byte[streamArray[num8].DecompressedLength];
                Compression.Decompress(b, ref streamArray[num8].Data);
            }
            for (int num9 = 0; num9 < this.Frames; num9++)
            {
                for (int num10 = 0; num10 < iFrame[num9].Size; num10++)
                {
                    iFrame[num9].Data[num10] = streamArray[iFrame[num9].Stream].Data[iFrame[num9].Position + num10];
                }
            }
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

