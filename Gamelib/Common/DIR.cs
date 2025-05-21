using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Gamelib.Common
{
    

    public class DIR
    {
        public int DataLength;
        public List<DirEntry> Entries = new List<DirEntry>();
        public int FileLength;
        public int HashSig;
        public int[] HashTable = new int[0x400];
        public int Signature;

        public DIR(string sFilePath)
        {
            FileStream input = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);
            using (BinaryReader reader = new BinaryReader(input))
            {
                this.Signature = reader.ReadInt32();
                this.FileLength = reader.ReadInt32();
                this.DataLength = reader.ReadInt32();
                reader.BaseStream.Position = this.DataLength;
                this.HashSig = reader.ReadInt32();
                for (int i = 0; i < 0x400; i++)
                {
                    this.HashTable[i] = reader.ReadInt32();
                }
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    int hashOffset = reader.ReadInt32();
                    int dirOffset = reader.ReadInt32();
                    int count = reader.ReadInt32();
                    List<byte> bFileName = new List<byte>();
                    byte item = 0xff;
                    while ((item != 0) || ((bFileName.Count % 4) != 0))
                    {
                        item = reader.ReadByte();
                        bFileName.Add(item);
                    }
                    int position = (int) reader.BaseStream.Position;
                    reader.BaseStream.Position = dirOffset;
                    byte[] data = reader.ReadBytes(count);
                    this.Entries.Add(new DirEntry(data, hashOffset, dirOffset, count, bFileName));
                    reader.BaseStream.Position = position;
                }
            }
            input.Close();
        }

        public static int DirHash(byte[] Data)
        {

            if (Data[0] == 0)
            return Data[0];

            return 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class DirEntry
        {
            public byte[] Data;
            public int HashOffset;
            public int DirOffset;
            public int FileLength;
            public List<byte> bFileName;
            public string sFileName;
            public DirEntry(byte[] Data, int HashOffset, int DirOffset, int FileLength, List<byte> bFileName)
            {
                this.Data = Data;
                this.HashOffset = HashOffset;
                this.DirOffset = DirOffset;
                this.FileLength = FileLength;
                this.bFileName = bFileName;
                int length = 0;
                for (int i = 0; i < bFileName.Count; i++)
                {
                    if ((bFileName[i] == 0) && (length == 0))
                    {
                        length = i;
                        break;
                    }
                }
                this.sFileName = Encoding.ASCII.GetString(bFileName.ToArray());
                this.sFileName = this.sFileName.Substring(0, length);
            }
        }
    }
}

