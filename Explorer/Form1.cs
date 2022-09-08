using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Explorer
{
    public partial class Form1 : Form
    {

        ImageList image_list1 = new ImageList(); // список изображений для хранения малых значков
        ImageList image_list2 = new ImageList(); // список изображений для хранения больших значков
        public Form1()
        {
            InitializeComponent();
            treeView1.ImageList = image_list1;
            // глубина цвета изображений
            image_list1.ColorDepth = ColorDepth.Depth32Bit;

            // установим размер изображения
            image_list1.ImageSize = new Size(16, 16);

            // ассоциируем список маленьких изображений с ListView
            listView1.SmallImageList = image_list1;

            // глубина цвета изображений
            image_list2.ColorDepth = ColorDepth.Depth32Bit;

            // установим размер изображения
            image_list2.ImageSize = new Size(32, 32);

            // ассоциируем список маленьких изображений с ListView
            listView1.LargeImageList = image_list2;
            string[] drive = Directory.GetLogicalDrives();
            foreach (string disk in drive)
            {
                treeView1.Nodes.Add(disk);
            }
        }
        private void AddList(TreeNode node)
        {
            listView1.Items.Clear();
            Icon icon = new Icon(@"../../folder.ICO");
            image_list1.Images.Add(icon);
            image_list2.Images.Add(icon);
            int index = 1;
            string[] files = Directory.GetFiles(node.Text);
            foreach (string file in files)
            {
                icon = Icon.ExtractAssociatedIcon(file);
                image_list1.Images.Add(icon);
                image_list2.Images.Add(icon);
                listView1.Items.Add(file, index++);
            }
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                listView1.Items.Add(node.Nodes[i].Text,0);
            }
        }
        private void FindByNode(TreeNode node, string path)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes.Clear();
            }
            Icon icon = new Icon(@"../../folder.ICO");
            image_list1.Images.Add(icon);
            image_list2.Images.Add(icon);
            int index = 1;
            string[] files = Directory.GetFiles(node.Text);
            try
            {
                string[] direct = Directory.GetDirectories(node.Text);
                foreach (var dir in direct)
                {
                    treeView1.Invoke(new Action(() => node.Nodes.Add(dir)));
                }
                foreach (string file in files)
                {
                    icon = Icon.ExtractAssociatedIcon(file);
                    image_list1.Images.Add(icon);
                    image_list2.Images.Add(icon);
                    treeView1.Invoke(new Action(() => node.Nodes.Add("",file,index++)));
                }
            }
            catch(Exception ex)
            {

            }
            
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            FindByNode(treeView1.SelectedNode, treeView1.SelectedNode.Text);
            AddList(treeView1.SelectedNode);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            FindByNode(treeView1.SelectedNode, treeView1.SelectedNode.Text);
            AddList(treeView1.SelectedNode);
        }

        private void BigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.ImageList = image_list1;
            listView1.StateImageList = image_list1;
        }

        private void LittleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.ImageList = image_list2;
            listView1.StateImageList = image_list2;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                Hide();
                this.notifyIcon1.Visible = true;
            }
        }
    }
}