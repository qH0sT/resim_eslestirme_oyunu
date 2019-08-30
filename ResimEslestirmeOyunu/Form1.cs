using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ResimEslestirmeOyunu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        List<PictureBox> pcbx_list = new List<PictureBox>();
        string isim_tutucu = "";
        int sinir = 0;
        int puan = 0;
        int i = 0;
        private void pic_click(object pic_Box, EventArgs args)
        {
            timer1.Stop();
            if (sinir < 2) //üst üste 2 den fazla basmayı engellemek için.
            {
                PictureBox pBoxy = pic_Box as PictureBox;

                try
                {
                    using (FileStream fs_im = new FileStream(pBoxy.Tag.ToString(), FileMode.Open, FileAccess.Read))
                    {
                        pBoxy.Image = Image.FromStream(fs_im);
                    }

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

                if (isim_tutucu != string.Empty)
                {
                    if (pBoxy.Name != isim_tutucu)
                    {
                        isim_tutucu = string.Empty;
                    }
                    else
                    {
                        puan += 1;
                        label2.Text = "Puan " + puan.ToString();
                        MessageBox.Show("TEBRİKLER", "Eşleştirme Başarılı");   
                                             
                        IEnumerable<PictureBox> resim = panel1.Controls.Cast<PictureBox>();
                        var imeyc = from x in resim
                                    where x.Name == isim_tutucu
                                    select x;
                        foreach (var f in imeyc)
                        {
                            f.Visible = false;
                        }

                        isim_tutucu = string.Empty;
                    }
                }
                else
                {
                    isim_tutucu = pBoxy.Name;
                }

                sinir += 1;
            }
            timer1.Start();
        }
        //95, 88
        bool tekrar_baslat = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (tekrar_baslat == false)
            {
                OyunuBaslat();
                button1.Text = "Tekrar Başlat";
                tekrar_baslat = true;
            }
            else
            {
                Application.Restart();
            }
           
        }
        private void OyunuBaslat()
        {
           
            while (i < 12)
            {
                if (pcbx_list.Count == 0)
                {
                    PictureBox pcb = new PictureBox();
                    pcb.Name = "Resim_" + i.ToString();
                    pcb.Tag = "X_" + i.ToString();
                    pcb.BackColor = Color.Blue;
                    pcb.Click += new EventHandler(pic_click);
                    pcb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pcb.Left = 12;
                    pcb.Top = 12;
                    pcb.Size = new Size(185, 156);
                    panel1.Controls.Add(pcb);
                    pcbx_list.Add(pcb);
                }
                else
                {
                    PictureBox pcb = new PictureBox();
                    pcb.Name = "Resim_" + i.ToString();
                    pcb.Click += new EventHandler(pic_click);
                    pcb.BackColor = Color.Blue;
                    pcb.Tag = "X_" + i.ToString();
                    pcb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pcb.Size = new Size(185, 156);
                    panel1.Controls.Add(pcb);
                    pcbx_list.Add(pcb);
                    if (i % 4 == 0)
                    {
                        pcb.Top = pcbx_list[i - 4].Top + 160;
                        pcb.Left = pcbx_list[i - 4].Left;
                    }
                    else
                    {
                        pcb.Top = pcbx_list[i - 1].Top;
                        pcb.Left = pcbx_list[i - 1].Left + 190;
                    }

                }
                i = i + 1;
            }
            ResimleriDiz();
        }
        private void ResimleriDiz()
        {
            int say = listBox1.Items.Count - 1;
            string[] resimler = { "Tavşan", "Ayı", "Kedi", "Köpek", "Arı", "Tavuk" };

            for (int h = 0; h < resimler.Length; h++)
            {
                for (int m = 0; m < 2; m++)
                {
                    Random rndm = new Random();
                    int c = Convert.ToInt32(listBox1.Items[rndm.Next(say)]);
                    panel1.Controls[c].Tag = Environment.CurrentDirectory + "\\Resimler\\" + resimler[h] + ".jpg";
                    panel1.Controls[c].Name = resimler[h];
                    //((PictureBox)(panel1.Controls[c])).ImageLocation = Environment.CurrentDirectory + "\\Resimler\\" + resimler[h] + ".jpg";
                    foreach (var b in listBox1.Items.Cast<object>().ToList())
                    {
                        if (c.ToString() == b.ToString())
                        {
                            listBox1.Items.Remove(b);
                        }
                    }
                    say = listBox1.Items.Count - 1;

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            isim_tutucu = "";
            sinir = 0;
            foreach (PictureBox pb in panel1.Controls)
            {
                pb.Image = null;
            }
        }
    }
}
