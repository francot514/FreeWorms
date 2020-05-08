using System;
using System.Collections.Generic;
using System.IO;

namespace Gamelib.Image
{
  

    public class GIF
    {
        public static void ToGif(BinaryWriter b, Sprite iSprite, ref List<Frame> iFrame)
        {
            b.Write(0x38464947);
            b.Write((short) 0x6139);
            b.Write(iSprite.Width);
            b.Write(iSprite.Height);
            b.Write((byte) 0xf7);
            b.Write((byte) 0);
            b.Write((byte) 0);
            for (int i = 0; i < 0xff; i++)
            {
                if (i <= iSprite.Palette.GetUpperBound(0))
                {
                    b.Write(iSprite.Palette[i, 2]);
                    b.Write(iSprite.Palette[i, 1]);
                    b.Write(iSprite.Palette[i, 0]);
                }
                else
                {
                    b.Write((byte) 0);
                    b.Write((byte) 0);
                    b.Write((byte) 0);
                }
            }
            b.Write((byte) 0x21);
            b.Write((byte) 0xff);
            b.Write((byte) 11);
            b.Write(0x5354454e);
            b.Write(0x45504143);
            b.Write((short) 0x2e32);
            b.Write((byte) 0x30);
            b.Write((byte) 3);
            b.Write((byte) 1);
            b.Write((short) 0);
            b.Write((byte) 0);
            for (int j = iSprite.FrameStart; j < (iSprite.FrameStart + iSprite.FrameCount); j++)
            {
                b.Write((byte) 0x21);
                b.Write((byte) 0xf9);
                b.Write((byte) 4);
                b.Write((byte) 8);
                b.Write((short) 0x19);
                b.Write((byte) 0);
                b.Write((byte) 0);
                b.Write((byte) 0x2c);
                b.Write((short) 0);
                b.Write((short) 0);
                b.Write(iSprite.Width);
                b.Write(iSprite.Height);
                b.Write((byte) 0);
                new LZWEncoder(iSprite.Width, iSprite.Height, iFrame[j].Data, 8).Encode(b.BaseStream);
            }
            b.Write((byte) 0x3b);
        }
    }
}

