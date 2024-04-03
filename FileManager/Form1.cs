using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileManager
{    
    public partial class Form1 : Form
    {
        private string copiedFilePath;
        public static Form1 instance = new Form1();
        public Form1()
        {
            instance = this;
            InitializeComponent();
            PopulateRoothandShortCut(@"C:\Users\tmp");
            textBox1.Text = @"C:\Users\tmp";
            treeView1.NodeMouseDoubleClick += treeView1_NodeMouseDoubleClick;
        }
        private void UpdateTextB1()
        {
            if(listView2.Items.Count >0) 
            {
                //var dirParent = Directory.GetParent(listView2.Items[0].SubItems[4].Text);
                textBox1.Text = "";
                string[] nameformat = listView2.Items[0].SubItems[4].Text.Split('\\');
                for(int i = 0;i < nameformat.Length-1; i++) 
                {
                    nameformat[i] = nameformat[i].Trim()+"\\";
                    textBox1.Text += nameformat[i] ;                    
                }
               
            }
        }
        private string GetFileType(string filePath)
        {
            string extension = Path.GetExtension(filePath);

            // Map file extension to MIME type
            switch (extension.ToLower())
            {
                case ".py":
                    return "Jupyter Source File";
                case ".txt":
                    return "Text document";
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                case ".jpeg":
                case ".png":
                    return "image";
                case ".doc":
                case ".docx":
                    return "DOCX Document";

                // Add more cases as needed
                default:
                    return "application/octet-stream"; // Default MIME type for unknown types
            }
        }
        private void PopulateRoothandShortCut(string path)
        {
            listView2.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (var shortcut in dir.GetDirectories())
            {
                string namefull=shortcut.FullName;
                string name = shortcut.Name;
                string date = shortcut.LastAccessTime.ToString();
                string type;
                if(shortcut.Attributes.HasFlag(FileAttributes.Normal))
                {
                     type = shortcut.Attributes.ToString();
                }
                else { type = "Folder"; }
               
                long size = 0;
                if (File.GetAttributes(shortcut.FullName).HasFlag(FileAttributes.Normal))
                {
                    FileInfo[] fi = shortcut.GetFiles();

                    foreach (var f in fi)
                    {
                        size += f.Length;
                    }
                }
                string[] data = { name, date, type," ",namefull };
                ListViewItem listViewItem = new ListViewItem(data);
                listView2.Items.Add(listViewItem);
            }
            foreach(var shortcut in dir.GetFiles())
            {
                string namefull= shortcut.FullName;
                string name = shortcut.Name;
                string date = shortcut.LastAccessTime.ToString();
                string type=GetFileType(shortcut.FullName);
                FileInfo fi = new FileInfo(shortcut.FullName);
                double size = (fi.Length)*0.001;
                string[] data = { name, date,type, size.ToString()+" KB" ,namefull};
                ListViewItem listViewItem = new ListViewItem(data);
                listView2.Items.Add(listViewItem);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string path=e.Node.Tag.ToString();
            textBox1.Text = path;
            PopulateRoothandShortCut(path);           
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextB1();           
        }
        private void ChildRepository(string path)
        {            
            if(Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                var dirFolder = Directory.GetDirectories(dir.FullName);
                var dirFile = dir.GetFiles(".");
                if (Directory.Exists(path))
                {
                    listView2.Items.Clear();
                    foreach (var d in dirFolder)
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(d);
                        string namefull = directoryInfo.FullName;
                        string name = directoryInfo.Name;
                        string date = directoryInfo.LastAccessTime.ToString();
                        string type = GetFileType(directoryInfo.FullName);
                        //FileInfo fi = new FileInfo(directoryInfo.FullName);
                        //double size = (fi.Length) * 0.001;
                        string[] data = { name, date, type, " ", namefull };
                        ListViewItem listViewItem = new ListViewItem(data);
                        listView2.Items.Add(listViewItem);
                    }
                    if (dir.GetFiles(".").Length > 0)
                    {
                        foreach (var v in dirFile)
                        {
                            string namefull = v.FullName;
                            string name = v.Name;
                            string date = v.LastAccessTime.ToString();
                            string type = GetFileType(v.FullName);
                            double size = v.Length * 0.001;
                            string[] data = { name, date, type, size.ToString() + " KB", namefull };
                            ListViewItem listViewItem = new ListViewItem(data);
                            listView2.Items.Add(listViewItem);
                        }
                    }
                }
               
            }
            else if(File.Exists(path))
            {
                FileInfo file = new FileInfo(path);
                if (/*file.Attributes.HasFlag(FileAttributes.Normal)*/ true)
                {
                    if (file.Extension.ToLower() == ".jpg" || file.Extension.ToLower() == ".jpeg" || file.Extension.ToLower() == ".png")
                    {                      
                        Image image = Image.FromFile(file.FullName);                        
                        FormPicture pictureForm = new FormPicture();
                        //picture.p.Image = image;
                        pictureForm.SetImageFromForm1(file.FullName);
                        pictureForm.Show();
                    }
                    if(file.Extension.ToLower()==".mp3" || file.Extension.ToLower() == ".mp4")
                    {
                        FormMusic musicForm = new FormMusic();
                        musicForm.PathMusic(path);
                        musicForm.Show();
                    }
                }
            }
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {            
            if (listView2.SelectedItems.Count==1)
            {  
                ListViewItem listView = listView2.SelectedItems[0];   
                ChildRepository(listView.SubItems[4].Text);
                if (Directory.Exists(listView.SubItems[4].Text))
                {
                    textBox1.Text = listView.SubItems[4].Text;
                }
                if (File.Exists(listView.SubItems[4].Text))
                {
                    textBox1.Text =Directory.GetParent(listView.SubItems[4].Text).FullName;
                }
            }
        }
        private void ParentRepository(string path)
        {
            string ? rooth = Directory.GetDirectoryRoot(path);
            if(Directory.GetParent(path).Exists && !(Directory.GetParent(path).Equals(rooth)))
            {
                var dirBack = Directory.GetParent(path);
                if(path==Directory.GetDirectoryRoot(path))
                {
                }
                else
                {
                    if (Directory.GetParent(dirBack.FullName).Exists && !Directory.GetParent(dirBack.FullName).Equals(rooth))
                    {
                        bool? check = false;
                        try
                        {
                            DirectoryInfo? dirParent = Directory.GetParent(dirBack.FullName);
                            check = dirParent.Exists;
                            if (check == true)
                            {
                                listView2.Items.Clear();
                                ChildRepository(dirParent.FullName);
                            }
                        }
                        catch { }

                    }
                }

            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if(listView2.Items.Count>0) 
            { 
                string path = listView2.Items[0].SubItems[4].Text;
                ParentRepository(path);
                string link = listView2.Items[0].SubItems[4].Text.ToString();
                textBox1.Text = Directory.GetParent(link).FullName;
            }
            if(listView2.Items.Count==0)
            {
                ParentRepository(textBox1.Text);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Items.Count>0)
            {               
                if(comboBox1.SelectedIndex==0)
                {
                    List<Files> list= new List<Files>();
                    Files f;
                    //listView2=listView2.Items.
                    foreach(ListViewItem item in listView2.Items)
                    {
                        f = new Files();
                        f.name= item.SubItems[0].Text;
                        f.date= item.SubItems[1].Text;
                        f.type= item.SubItems[2].Text;
                        f.size = item.SubItems[3].Text;
                        f.fullname= item.SubItems[4].Text;  
                        list.Add(f);
                    }
                    listView2.Items.Clear();
                    f = new Files();
                    foreach (Files row in f.SortNameAZ(list))
                    {
                        string[] data = { row.name, row.date, row.type, row.size, row.fullname };                        
                        ListViewItem item = new ListViewItem(data);                        
                        listView2.Items.Add(item);

                    }
                }
                if(comboBox1.SelectedIndex==1)
                {
                    List<Files> list = new List<Files>();
                    //listView2=listView2.Items.
                    Files f;
                    foreach (ListViewItem item in listView2.Items)
                    {
                        f = new Files();
                        f.name = item.SubItems[0].Text;
                        f.date = item.SubItems[1].Text;
                        f.type = item.SubItems[2].Text;
                        f.size = item.SubItems[3].Text;
                        f.fullname = item.SubItems[4].Text;
                        list.Add(f);
                    }
                    listView2.Items.Clear();
                    f = new Files();
                    foreach (Files row in f.SortNameZA(list))
                    {
                        string[] data = { row.name, row.date, row.type, row.size, row.fullname };
                        ListViewItem item = new ListViewItem(data);
                        listView2.Items.Add(item);
                    }
                }
                if(comboBox1.SelectedIndex==2)
                {

                    List<Files> list = new List<Files>();
                    //listView2=listView2.Items.
                    Files f;
                    foreach (ListViewItem item in listView2.Items)
                    {
                        f = new Files();
                        f.name = item.SubItems[0].Text;
                        f.date = item.SubItems[1].Text;
                        f.type = item.SubItems[2].Text;
                        f.size = item.SubItems[3].Text;
                        f.fullname = item.SubItems[4].Text;
                        list.Add(f);
                    }
                    listView2.Items.Clear();
                    f = new Files();
                    foreach (Files row in f.Size0N(list))
                    {
                        string[] data = { row.name, row.date, row.type, row.size, row.fullname };
                        ListViewItem item = new ListViewItem(data);
                        listView2.Items.Add(item);
                    }
                }
                if(comboBox1.SelectedIndex== 3)
                {

                    List<Files> list = new List<Files>();
                    //listView2=listView2.Items.
                    Files f;
                    foreach (ListViewItem item in listView2.Items)
                    {
                        f = new Files();
                        f.name = item.SubItems[0].Text;
                        f.date = item.SubItems[1].Text;
                        f.type = item.SubItems[2].Text;
                        f.size = item.SubItems[3].Text;
                        f.fullname = item.SubItems[4].Text;
                        list.Add(f);
                    }
                    listView2.Items.Clear();
                    f = new Files();
                    foreach (Files row in f.SizeN0(list))
                    {
                        string[] data = { row.name, row.date, row.type, row.size, row.fullname };
                        ListViewItem item = new ListViewItem(data);
                        listView2.Items.Add(item);
                    }

                }
            }
        }
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
        }
        private void CreateSubnodes(TreeNode parentNode)
        {
            // Add subnodes dynamically
            TreeNode subNode1 = new TreeNode("Subnode 1");
            DirectoryInfo dir = new DirectoryInfo(parentNode.Tag.ToString());
            foreach (var d in dir.GetDirectories())
            {
                TreeNode node = new TreeNode(d.Name);
                node.Tag = d.FullName;
                parentNode.Nodes.Add(node);
            }

            // Optionally, expand the parent node to show the newly added subnodes
            //parentNode.Expand();
        }
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(treeView1.Nodes.Count>0)
            {
                int level=e.Node.Level;
                CreateSubnodes(e.Node);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listView2.SelectedItems.Count>0)
            {                     
                DialogResult d;
                d=MessageBox.Show("Do you want to delete the selected file(s)\n", "Delete File",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
                if(d==DialogResult.OK)
                {
                    foreach(ListViewItem item in listView2.SelectedItems)
                    {
                        if (Directory.Exists(item.SubItems[4].Text))
                        {
                                Directory.Delete(item.SubItems[4].Text);
                                listView2.Items.Remove(item);
                        }

                        else if (File.Exists(item.SubItems[4].Text))
                        {
                            FileInfo file = new FileInfo(item.SubItems[4].Text);
                            bool check = file.Exists;
                            if (check)
                            {
                                file.Delete();
                            }
                            listView2.Items.Remove(item);
                        }
                        else
                        {
                            MessageBox.Show("OOPS");
                        }
                    }
                }
            }
        }   
        private void CopyDirectory(string sourceDir, string destinationDir)
        {
            // Create the destination directory if it doesn't exist
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            // Copy each file
            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(filePath);
                string destinationPath = Path.Combine(destinationDir, fileName);
                File.Copy(filePath, destinationPath, true);
            }

            // Recursively copy subdirectories
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDir);
                string destinationSubDir = Path.Combine(destinationDir, subDirName);
                CopyDirectory(subDir, destinationSubDir);
            }
        }
        private void CopyFileOrDirectory(string sourcePath, string destinationPath)
        {
            if (File.Exists(sourcePath))
            {
                // Copy a file
                string fileName = Path.GetFileName(sourcePath);
                string destinationFilePath = Path.Combine(destinationPath, fileName);
                File.Copy(sourcePath, destinationFilePath, true);
            }
            else if (Directory.Exists(sourcePath))
            {
                // Copy a directory
                CopyDirectory(sourcePath, destinationPath);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if(listView2.SelectedItems.Count>0)
            {
                foreach (ListViewItem item in listView2.SelectedItems)
                {
                    string selectedPath = item.SubItems[4].Text;
                    copiedFilePath = selectedPath;
                }
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(copiedFilePath))
            {
                string destinationPath=textBox1.Text;
                try
                {
                    CopyFileOrDirectory(copiedFilePath, destinationPath);
                    PopulateRoothandShortCut(destinationPath);
                }
                catch(Exception ex) { }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Files f=new Files();          
            List<Files> files=f.PopulateList(listView2);
            if(textBox2.Text.Length>0)
            {
                listView2.Items.Clear();
                List<Files> filter = f.Search(textBox2.Text, files);
                foreach (var file in filter)
                {
                    int s =f.Search(textBox2.Text, files).Count;
                    string[] data = { file.name ,file.date,file.type,file.size,file.fullname};
                    ListViewItem item = new ListViewItem(data);
                    listView2.Items.Add(item);
                }
            }
            if(textBox1.Text.Length==0 || textBox1.Text.Length ==null)
            {
                foreach (var file in files)
                {
                    string[] data = { f.name, f.date, f.type, f.size, f.fullname };
                    ListViewItem item = new ListViewItem(data);
                    listView2.Items.Add(item);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text!=null && textBox1.Text.Length>0)
            {
                if(Directory.Exists(textBox1.Text))
                {
                    DirectoryInfo dir= new DirectoryInfo(textBox1.Text);
                    textBox2.PlaceholderText = "Search " + dir.Name;
                }
                
            }           
        }

        private void textBox2_MouseHover(object sender, EventArgs e)
        {
            if(e.Equals(false))
            {
                textBox2.Text = string.Empty;
                if (Directory.Exists(textBox1.Text))
                {
                    DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
                    textBox2.PlaceholderText = "Search " + dir.Name;
                }
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {

        }
    }
}
