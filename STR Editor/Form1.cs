using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STR_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (!File.Exists(openFileDialog1.FileName))
            {
                MessageBox.Show("File doesn't exists.", "Not Found");
                return;
            }

            Str str = new Str(new MemoryStream(File.ReadAllBytes(openFileDialog1.FileName)));
            ShowStr(str);
        }

        private void ShowStr(Str str)
        {
            listBox1.Items.Clear();
            for (int i = 0; i < str.Layers[0].Textures.Length; i++)
            {
                listBox1.Items.Add(str.Layers[0].Textures[i]);
            }

            pictureBox1.Image = imageList1.Images[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
    }
}
