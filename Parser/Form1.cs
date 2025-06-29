using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Gamelib.Common;
using Gamelib.Image;

namespace Editor
{
    public partial class Form1 : Form
    {

        private string CurrentDir;
        private BinaryReader reader;
        private List<Sprite> Sprites;
        private List<Frame> Frames;
        private DIR DIR;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CurrentDir = AppDomain.CurrentDomain.BaseDirectory;
        }

        private void ParseDIR(string path)
        {

            DIR = new DIR(path);
            Sprites = new List<Sprite>();
            Frames = new List<Frame>();

            foreach (DIR.DirEntry entry in DIR.Entries)
            {
                listBox1.Items.Add(entry.sFileName);

               

            }

            label2.Text = DIR.Entries.Count.ToString();
            
           

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.Filter = "Worms DIR(*.dir)|*.dir|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                CurrentDir = null;
                ParseDIR(openFileDialog1.FileName);

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            int id = listBox1.SelectedIndex;
            int count = 0;
            CurrentDir = CurrentDir + listBox1.SelectedItem.ToString();
            

            if (id >= 0 && DIR != null)
            {
                
                    if (DIR.Entries[id].sFileName.Contains("bnk"))       
                        {
                            using (Stream stream = new MemoryStream(DIR.Entries[id].Data, false))
                            {

                                reader = new BinaryReader(stream);

                                BNK bnk = new BNK(reader, ref Sprites, ref Frames);
                            BinaryWriter Writer = new BinaryWriter(new MemoryStream());



                                foreach (Frame spr in Frames)
                                {
                                    listBox2.Items.Add("Sprite" + count);
                                   

                                    Bitmap bmp = Frames[id].ToBitmap(ref Writer, new byte[4], false, Sprites[id].CropL, Sprites[id].CropU,
                                        Sprites[id].CropR, Sprites[id].CropD);

                                    bmp.Save("newimage" + count + ".bmp");
                                    count++;

                                }

                                label4.Text = Sprites.Count.ToString();
                                label7.Text = Frames.Count.ToString();
                            }

                        }
                else if (DIR.Entries[id].sFileName.Contains("img"))
                    {

                        IMG img = new IMG(new MemoryStream(DIR.Entries[id].Data, false));
                        pictureBox1.BackgroundImage = img.Bitmap;



                    }
                    else if (DIR.Entries[id].sFileName.Contains("spr"))
                    {
                        byte[] Data = DIR.Entries[id].Data;
                        using (Stream stream = new MemoryStream(Data, false))
                        {

                            reader = new BinaryReader(stream);

                            SPR spr = new SPR(reader, ref Sprites, ref Frames);
                            BinaryWriter Writer = new BinaryWriter(new MemoryStream());
                            //Writer.Write(Data, 0, Data.Length);
                            //Writer.Close();

                            foreach (Frame sp in Frames)
                            {
                                listBox2.Items.Add("Sprite" + count);

                                Bitmap bmp = sp.ToBitmap(ref Writer, new byte[4], true, sp.CropL, sp.CropU,
                                    sp.CropR, sp.CropD);

                                    sp.Bitmap = bmp;

                                    bmp.Save("newimage" + count + ".bmp");
                                    count++;

                                


                            }

                            label4.Text = Sprites.Count.ToString();
                            label7.Text = Frames.Count.ToString();
                        }


                    }

            }

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = listBox2.SelectedIndex;
           

            if (id >= 0)
            {
                if (Frames != null)
                for (int i = 0; i < Frames.Count; i++)
                    if (Frames[i].Size > 0)
                        pictureBox1.BackgroundImage = Frames[i].Bitmap;


            }


        }
    }
}
