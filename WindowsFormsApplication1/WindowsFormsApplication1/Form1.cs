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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        // 1-single  2-multi Picture  3-slide Show  4-exit  
        Button b1, b2, b3, b4, upload, Return;
        public Form1()
        {
            upload = new Button();
            upload.Size = new Size(220, 60);
            upload.Text = "Upload";
            Return = new Button();
            Return.Size = new Size(220, 60);
            Return.Text = "Return";
            Return.Font = new Font(Return.Font.FontFamily, 25);
            upload.Location = new Point(300, 80);
            upload.Font = new Font(upload.Font.FontFamily, 25);
            b1 = new Button();
            b1.Size = new Size(220, 60);
            b1.Text = "Single Picture";
            b1.Location = new Point(300, 80);
            b1.Font = new Font(b1.Font.FontFamily, 25);
            b2 = new Button();
            b2.Size = new Size(220, 60);
            b2.Text = "Multi-Picture ";
            b2.Location = new Point(300, 150);
            b2.Font = new Font(b2.Font.FontFamily, 25);
            b3 = new Button();
            b3.Size = new Size(220, 60);
            b3.Text = "slide show";
            b3.Location = new Point(300, 220);
            b3.Font = new Font(b3.Font.FontFamily, 25);
            b4 = new Button();
            b4.Size = new Size(220, 60);
            b4.Text = "Exit";
            b4.Location = new Point(300, 150);
            b4.Font = new Font(b4.Font.FontFamily, 25);
            InitializeComponent();
        }
        public Image imagebox
        {
            set
            {
                this.pictureBox1.Image = value;
                this.pictureBox1.Size = value.Size;
            }
        }
        List<string> picturName = new List<string>();
        List<ListViewItem> list_of_pictures = new List<ListViewItem>();
        private void call()
        {
            list_of_pictures.Clear();
            foreach (var i in listView1.SelectedItems)
            {
                ListViewItem a = (ListViewItem)i;
                list_of_pictures.Add(a);
            }
        }

        private void upload_Click(object sender, EventArgs e) // upload
        {
            timer1.Interval = 1;
            listView1.Visible = true;
            using (OpenFileDialog Open = new OpenFileDialog()
            {
                Multiselect = true,
                ValidateNames = true,
            })
            {
                if (Open.ShowDialog() == DialogResult.OK)
                {
                    picturName.Clear();
                    listView1.Items.Clear();
                    foreach (string name in Open.FileNames)
                    {
                        FileInfo info = new FileInfo(name);
                        picturName.Add(info.FullName);
                        listView1.Items.Add(info.Name, 0);
                    }
                }
            }
            Controls.Add(b1);
            Controls.Add(b2);
            Controls.Add(b3);
            Controls.Add(Return);
            upload.Location = new Point(700, 50);
            b1.Location = new Point(700, 120);
            b2.Location = new Point(700, 190);
            b3.Location = new Point(700, 260);
            Return.Location = new Point(700, 340);
            b4.Location = new Point(700, 410);
            listView1.ItemActivate += new System.EventHandler(this.listView1_ItemActivate);
        }
        private void listView1_ItemActivate(object sender, EventArgs e)// when select an image it will be displayed
        {
            if (listView1.Focused != null)
            {
                Return.Visible = true;
                Return.BringToFront();
                pictureBox1.Visible = true;
                listView1.Visible = false;
                Image img = Image.FromFile(picturName[listView1.FocusedItem.Index]);
                this.imagebox = img;
                FileInfo fi = new FileInfo(picturName[listView1.FocusedItem.Index]);
                this.Text = fi.Name;
            }
        }
        private void Return_Click(object sender, EventArgs e)// Return
        {
            b1.Visible = true;
            b2.Visible = true;
            b3.Visible = true;
            panel1.Visible = false;
            pictureBox1.Visible = false;
            listView1.Visible = true;
            Return.Visible = false;
            // if there is any picture last from the last run to the program 
            foreach (Control i in panel1.Controls)
                panel1.Controls.Remove(i);
            timer1.Stop();
            this.Text = "Pictures";
        }
        private void b1_Click(object sender, EventArgs e)// single picture
        {
            if (listView1.Focused != null)
            {
                Return.Visible = true;
                Return.BringToFront();
                pictureBox1.Visible = true;
                listView1.Visible = false;
                Image img = Image.FromFile(picturName[listView1.FocusedItem.Index]);
                this.imagebox = img;
                FileInfo fi = new FileInfo(picturName[listView1.FocusedItem.Index]);
                this.Text = fi.Name;
            }
        }
        private void b2_Click(object sender, EventArgs e)// multi pictur
        {
            call();
            if (list_of_pictures.Count() > 1)
            {
                Return.Visible = true;
                b4.Visible = true;
                panel1.Visible = true;
                listView1.Visible = false;
                b1.Visible = false;
                b2.Visible = false;
                b3.Visible = false;
                upload.Visible = false;
                b4.BringToFront();
                Return.BringToFront();
                int x = 0, y = 0;
                foreach (var img in list_of_pictures)
                {
                    PictureBox p1 = new PictureBox();
                    p1.Image = Image.FromFile(picturName[img.Index]);
                    p1.Location = new Point(x, y);
                    p1.Size = new Size(200, 200);
                    p1.SizeMode = PictureBoxSizeMode.StretchImage;
                    panel1.Controls.Add(p1);
                    x += 200;
                    if (x > panel1.Width)
                    {
                        x = 0; y += 204;
                    }
                }
                this.Text = "Multi selection";
            }

        }
        int idx = 0;
        private void b3_Click(object sender, EventArgs e) // slide show
        {
            call();
            if (list_of_pictures.Count() > 1)
            {
                b4.Visible = true;
                Return.Visible = true;
                b4.BringToFront();
                Return.BringToFront();
                listView1.Visible = false;
                //show images
                idx = 0;
                timer1.Enabled = true;
                timer1.Tick += new System.EventHandler(OnTimerEvent);
            }
        }
        private void OnTimerEvent(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            if (list_of_pictures.Count() > 1)
            {
                pictureBox1.Visible = true;
                Image img = Image.FromFile(picturName[list_of_pictures[idx].Index]);
                pictureBox1.Image = img;
                FileInfo fi = new FileInfo(picturName[list_of_pictures[idx].Index]);
                Text = fi.Name;
                idx++;
                if (idx == list_of_pictures.Count()) idx = 0;
            }
        }
        private void b4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Text = "Pictures";
            Controls.Remove(button1);
            button1.Dispose();
            Controls.Add(upload);
            Controls.Add(b4);
            b1.Click += b1_Click;
            b2.Click += b2_Click;
            b3.Click += b3_Click;
            b4.Click += b4_Click;
            Return.Click += Return_Click;
            upload.Click += upload_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
