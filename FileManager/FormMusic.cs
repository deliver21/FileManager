using MyMp3Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class FormMusic : Form
    {
        private string path;
        public Mp3Player mplayer = new Mp3Player();
        public FormMusic()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mplayer.Play();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mplayer.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Mp3 Files| *.mp3";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    mplayer.Open(ofd.FileName);
                }
            }
        }
        public void PathMusic(string path)
        {
            this.path = path;  
            mplayer.Open(path);
        }
    }
}
