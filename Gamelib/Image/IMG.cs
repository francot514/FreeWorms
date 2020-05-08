using Gamelib.Common;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Gamelib.Image
{
    

    public class IMG
    {
        public byte BitRate;
        public List<byte> Description;
        public int FileLength;
        public bool FlagCompressed;
        public bool FlagPalette;
        public byte Flags;
        public short Height;
        public byte[,] Palette;
        public short PaletteCount;
        public int Signature;
        public int Size;
        public short Width;
        public string Name;


        private Bitmap _bitmap;
        private bool _compressed;
        private static byte[] _defaultHeader = new byte[] { 0x49, 0x4d, 0x47, 0x1a };
        private string _description;

        public IMG(BinaryReader b, ref List<Sprite> iSprite, ref List<Frame> iFrame)
        {
            byte[] buffer;
            this.Description = new List<byte>();
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
            else if (this.BitRate == 1)
            {
                this.PaletteCount = 1;
                this.Palette = new byte[,] { { 0xff, 0xff, 0xff } };
            }
            this.Width = b.ReadInt16();
            this.Height = b.ReadInt16();
            this.Size = this.Width * this.Height;
            if (this.BitRate == 1)
            {
                this.Size /= 8;
            }
            if (this.FlagCompressed)
            {
                buffer = new byte[this.Size];
                Compression.Decompress(b, ref buffer);
            }
            else
            {
                buffer = b.ReadBytes(this.Size);
            }
            iFrame.Add(new Frame(this.Palette, 0, 0, 0, 0, this.Width, this.Height, buffer));
        }

        public IMG(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                byte[] buffer2;
                if (!reader.ReadBytes(4).SequenceEqual<byte>(_defaultHeader))
                {
                    throw new InvalidDataException("Invalid header.");
                }
                reader.ReadInt32();
                byte num = reader.ReadByte();
                if (num > 0x20)
                {
                    string str = reader.ReadString(BinaryStringFormat.ZeroTerminated);
                    this._description = ((char)num) + str;
                    num = reader.ReadByte();
                }
                byte b = reader.ReadByte();
                this._compressed = b.GetBit(6);
                bool bit = b.GetBit(7);
                Color[] colorArray = null;
                if (bit)
                {
                    short num3 = reader.ReadInt16();
                    colorArray = new Color[num3 + 1];
                    colorArray[0] = Color.Black;
                    for (int i = 1; i <= num3; i++)
                    {
                        colorArray[i] = Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                    }
                }
                short width = reader.ReadInt16();
                short height = reader.ReadInt16();
                if (num == 8)
                {
                    if (colorArray == null)
                    {
                        throw new InvalidDataException("Image is palettized, but does not specify a palette.");
                    }
                    this._bitmap = new System.Drawing.Bitmap(width, height, PixelFormat.Format8bppIndexed);
                    ColorPalette palette = this._bitmap.Palette;
                    for (int j = 0; j < colorArray.Length; j++)
                    {
                        palette.Entries[j] = colorArray[j];
                    }
                    this._bitmap.Palette = palette;
                }
                if (this._compressed)
                {
                    buffer2 = new byte[width * height];
                    Compression.Decompress(reader, ref buffer2);
                }
                else
                {
                    buffer2 = reader.ReadBytes(width * height);
                }
                BitmapData bitmapdata = this._bitmap.LockBits(new Rectangle(Point.Empty, this._bitmap.Size), ImageLockMode.WriteOnly, this._bitmap.PixelFormat);
                Marshal.Copy(buffer2, 0, bitmapdata.Scan0, buffer2.Length);
                this._bitmap.UnlockBits(bitmapdata);
            }

            
        }

        public IMG(string fileName) : this(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        {


        }

        public System.Drawing.Bitmap Bitmap
        {
            get
            {
                return this._bitmap;
            }
            set
            {
                this._bitmap = value;
            }
        }

        public bool Compressed
        {
            get
            {
                return this._compressed;
            }
            set
            {
                this._compressed = value;
            }
        }

       

    }
}

