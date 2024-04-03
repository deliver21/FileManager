using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class FormPicture : Form
    {
        public string imagepath;

        public FormPicture()
        {
            InitializeComponent();
        }
        public void SetImageFromForm1(string path)
        { 
            imagepath= path;
            Image image= Image.FromFile(imagepath);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = image;
        }
    }
}
