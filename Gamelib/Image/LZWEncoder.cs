using System;
using System.IO;

namespace Gamelib.Image
{
    

    public class LZWEncoder
    {
        private int a_count;
        private byte[] accum = new byte[0x100];
        private static readonly int BITS = 12;
        private bool clear_flg;
        private int ClearCode;
        private int[] codetab = new int[HSIZE];
        private int cur_accum;
        private int cur_bits;
        private int curPixel;
        private static readonly int EOF = -1;
        private int EOFCode;
        private int free_ent;
        private int g_init_bits;
        private int hsize = HSIZE;
        private static readonly int HSIZE = 0x138b;
        private int[] htab = new int[HSIZE];
        private int imgH;
        private int imgW;
        private int initCodeSize;
        private int[] masks = new int[] { 
            0, 1, 3, 7, 15, 0x1f, 0x3f, 0x7f, 0xff, 0x1ff, 0x3ff, 0x7ff, 0xfff, 0x1fff, 0x3fff, 0x7fff, 
            0xffff
         };
        private int maxbits = BITS;
        private int maxcode;
        private int maxmaxcode = (((int) 1) << BITS);
        private int n_bits;
        private byte[] pixAry;
        private int remaining;

        public LZWEncoder(int width, int height, byte[] pixels, int color_depth)
        {
            this.imgW = width;
            this.imgH = height;
            this.pixAry = pixels;
            this.initCodeSize = Math.Max(2, color_depth);
        }

        private void Add(byte c, System.IO.Stream outs)
        {
            this.accum[this.a_count++] = c;
            if (this.a_count >= 0xfe)
            {
                this.Flush(outs);
            }
        }

        private void ClearTable(System.IO.Stream outs)
        {
            this.ResetCodeTable(this.hsize);
            this.free_ent = this.ClearCode + 2;
            this.clear_flg = true;
            this.Output(this.ClearCode, outs);
        }

        private void Compress(int init_bits, System.IO.Stream outs)
        {
            int num3;
            this.g_init_bits = init_bits;
            this.clear_flg = false;
            this.n_bits = this.g_init_bits;
            this.maxcode = this.MaxCode(this.n_bits);
            this.ClearCode = ((int) 1) << (init_bits - 1);
            this.EOFCode = this.ClearCode + 1;
            this.free_ent = this.ClearCode + 2;
            this.a_count = 0;
            int code = this.NextPixel();
            int num7 = 0;
            int hsize = this.hsize;
            while (hsize < 0x10000)
            {
                num7++;
                hsize *= 2;
            }
            num7 = 8 - num7;
            int num6 = this.hsize;
            this.ResetCodeTable(num6);
            this.Output(this.ClearCode, outs);
        Label_0166:
            while ((num3 = this.NextPixel()) != EOF)
            {
                hsize = (num3 << this.maxbits) + code;
                int index = (num3 << num7) ^ code;
                if (this.htab[index] == hsize)
                {
                    code = this.codetab[index];
                }
                else
                {
                    if (this.htab[index] >= 0)
                    {
                        int num5 = num6 - index;
                        if (index == 0)
                        {
                            num5 = 1;
                        }
                        do
                        {
                            index -= num5;
                            if (index < 0)
                            {
                                index += num6;
                            }
                            if (this.htab[index] == hsize)
                            {
                                code = this.codetab[index];
                                goto Label_0166;
                            }
                        }
                        while (this.htab[index] >= 0);
                    }
                    this.Output(code, outs);
                    code = num3;
                    if (this.free_ent < this.maxmaxcode)
                    {
                        this.codetab[index] = this.free_ent++;
                        this.htab[index] = hsize;
                    }
                    else
                    {
                        this.ClearTable(outs);
                    }
                }
            }
            this.Output(code, outs);
            this.Output(this.EOFCode, outs);
        }

        public void Encode(System.IO.Stream os)
        {
            os.WriteByte(Convert.ToByte(this.initCodeSize));
            this.remaining = this.imgW * this.imgH;
            this.curPixel = 0;
            this.Compress(this.initCodeSize + 1, os);
            os.WriteByte(0);
        }

        private void Flush(System.IO.Stream outs)
        {
            if (this.a_count > 0)
            {
                outs.WriteByte(Convert.ToByte(this.a_count));
                outs.Write(this.accum, 0, this.a_count);
                this.a_count = 0;
            }
        }

        private int MaxCode(int n_bits)
        {
            return ((((int) 1) << n_bits) - 1);
        }

        private int NextPixel()
        {
            if (this.remaining == 0)
            {
                return EOF;
            }
            this.remaining--;
            int num = this.curPixel + 1;
            if (num < this.pixAry.GetUpperBound(0))
            {
                byte num2 = this.pixAry[this.curPixel++];
                return (num2 & 0xff);
            }
            return 0xff;
        }

        private void Output(int code, System.IO.Stream outs)
        {
            this.cur_accum &= this.masks[this.cur_bits];
            if (this.cur_bits > 0)
            {
                this.cur_accum |= code << this.cur_bits;
            }
            else
            {
                this.cur_accum = code;
            }
            this.cur_bits += this.n_bits;
            while (this.cur_bits >= 8)
            {
                this.Add((byte) (this.cur_accum & 0xff), outs);
                this.cur_accum = this.cur_accum >> 8;
                this.cur_bits -= 8;
            }
            if ((this.free_ent > this.maxcode) || this.clear_flg)
            {
                if (this.clear_flg)
                {
                    this.maxcode = this.MaxCode(this.n_bits = this.g_init_bits);
                    this.clear_flg = false;
                }
                else
                {
                    this.n_bits++;
                    if (this.n_bits == this.maxbits)
                    {
                        this.maxcode = this.maxmaxcode;
                    }
                    else
                    {
                        this.maxcode = this.MaxCode(this.n_bits);
                    }
                }
            }
            if (code == this.EOFCode)
            {
                while (this.cur_bits > 0)
                {
                    this.Add((byte) (this.cur_accum & 0xff), outs);
                    this.cur_accum = this.cur_accum >> 8;
                    this.cur_bits -= 8;
                }
                this.Flush(outs);
            }
        }

        private void ResetCodeTable(int hsize)
        {
            for (int i = 0; i < hsize; i++)
            {
                this.htab[i] = -1;
            }
        }
    }
}

