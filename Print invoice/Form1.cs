using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Print_invoice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Font f = new Font("tahoma", 20, FontStyle.Bold);
        Font f1 = new Font("arial", 17, FontStyle.Bold);
        Point p1, p2, p4, p5;
        Size p6, p7;
        Regex rgx = new Regex("^[A-Za-z]+(\\s[A-Za-z]+)+$");
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("www.google.com");
        }
        int w;
        int id = 1435;
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (rgx.IsMatch(textBox3.Text) == false)
            {
                errorProvider1.Clear();
                textBox3.Focus();
                errorProvider1.SetError(textBox3, "please enter a valide FullName");
                return;
            }
            
            foreach (Control c in this.Controls)
            {
                if(c is TextBox || c is ComboBox)
                {
                    if (c.Text == "")
                    {
                        MessageBox.Show("Please enter all info","",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            
            object[] obj = { comboBox1.SelectedItem, textBox5.Text, textBox4.Text, textBox6.Text };
            dataGridView1.Rows.Add(obj);
            foreach(DataGridViewRow c in dataGridView1.Rows)
            {
                c.Cells[0].Style.ForeColor = Color.Black;
                c.Cells[1].Style.ForeColor = Color.FromArgb(255, 192, 0, 0);
                c.Cells[2].Style.ForeColor = Color.Green;
                c.Cells[3].Style.ForeColor = Color.Navy;
            }
            
            foreach(Control c in this.Controls)
            {
                if(c is TextBox  && c!=textBox1 && c!= textBox2 && c != textBox3)
                {
                    c.Text = "";
                }
                if(c is ComboBox)
                {
                    c.Text = "";
                }
            }
            comboBox1.Focus();
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            textBox3.Focus();
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool check = true;
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (rgx.IsMatch(textBox3.Text) == false)
                {
                    check = false;
                }
                if (check == true)
                {
                    errorProvider1.Clear();
                    e.Handled = true;
                    comboBox1.Focus();
                }
                else
                {
                    errorProvider1.Clear();
                    errorProvider1.SetError(textBox3, "please enter a valide FullName");
                }
            }
        }
        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (comboBox1.Items.Contains(comboBox1.Text))
                {
                    errorProvider1.Clear();
                    e.Handled = true;
                    textBox5.Focus();
                }
                else
                {
                    errorProvider1.Clear();
                    errorProvider1.SetError(comboBox1, "this item is not available");
                }
            }
        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] items = { 5599, 3999, 4599, 10999, 400, 455, 700, 1000, 600, 800 };

            textBox4.Text = items[comboBox1.SelectedIndex].ToString();
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Regex reg = new Regex("^\\d+$");
            if (reg.IsMatch(textBox5.Text)) 
            {
                int a = int.Parse(textBox4.Text) * int.Parse(textBox5.Text);
                textBox6.Text = a.ToString();
            }
            if (textBox5.Text == "")
            {
                textBox6.Text = "";
            }
        }
        Regex regdata = new Regex("^\\d+$");
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 1)
                {
                    if (regdata.IsMatch(dataGridView1.CurrentCell.Value.ToString()) == false)
                    {
                        dataGridView1.CurrentCell.Value = obj.ToString();
                    }
                }
            }
            
        }
        object obj;
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
             obj =dataGridView1.CurrentCell.Value;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count < 1)
            {
               MessageBox.Show("Please enter all info","",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
            Rectangle recimage = new Rectangle(e.PageSettings.PaperSize.Width-(25+200), 20,200, 150) ;
            e.Graphics.DrawImage(pictureBox1.Image,recimage);
            string textheader = "NO#  " + textBox1.Text;
            SizeF str = e.Graphics.MeasureString(textheader, f);
            e.Graphics.DrawString(textheader, f, Brushes.Red, (e.PageSettings.PaperSize.Width / 2) - (str.Width / 2), 20);

            string textdate = "Date :  "+textBox2.Text ;
            SizeF strdate = e.Graphics.MeasureString(textdate, f);
            e.Graphics.DrawString(textdate, f1, Brushes.Black, 25, 20 + str.Height + 60);
            string textname = "Full Name :  " + textBox3.Text;
            e.Graphics.DrawString(textname, f1, Brushes.Black, 25, 15 + str.Height + 60 + strdate.Height);
            PointF t1 = new PointF(25, 30 + str.Height + 60 + strdate.Height+20);
            PointF t2 = new PointF(e.PageSettings.PaperSize.Width - 25, 30 + str.Height + 60 + strdate.Height + 20);
            PointF t3 = new PointF(e.PageSettings.PaperSize.Width - 25, e.PageSettings.PaperSize.Height - 30);
            PointF t4 = new PointF(25, e.PageSettings.PaperSize.Height-30);
            PointF t5 = new PointF(25, 30 + str.Height + 60 + strdate.Height + 20);

            PointF[] tab ={t1,t2,t3,t4,t5};
            e.Graphics.DrawLines(Pens.Black,tab);
            int dis = e.PageSettings.PaperSize.Width - 25-(200);
            e.Graphics.DrawLine(Pens.Black, dis, 30 + str.Height + 60 + strdate.Height + 20, dis, e.PageSettings.PaperSize.Height - 30);
            e.Graphics.DrawLine(Pens.Black, dis - 125, 30 + str.Height + 60 + strdate.Height + 20, dis - 125, e.PageSettings.PaperSize.Height - 30);
            e.Graphics.DrawLine(Pens.Black, dis - (125 * 2), 30 + str.Height + 60 + strdate.Height + 20, dis - (125 * 2), e.PageSettings.PaperSize.Height - 30);

            e.Graphics.DrawLine(Pens.Black, 25, 50 + 30 + str.Height + 60 + strdate.Height + 20, e.PageSettings.PaperSize.Width - 25, 50 + 30 + str.Height + 60 + strdate.Height + 20);
            e.Graphics.DrawString("Model", f1, Brushes.Black, 25 + 150, 35 + str.Height + 60 + strdate.Height + 20);
            e.Graphics.DrawString("Amount", f1, Brushes.Black, dis, 35 + str.Height + 60 + strdate.Height + 20);
            e.Graphics.DrawString("Price", f1, Brushes.Black, dis - 125, 35 + str.Height + 60 + strdate.Height + 20);
            e.Graphics.DrawString("Total", f1, Brushes.Black, dis - (125 * 2), 35 + str.Height + 60 + strdate.Height + 20);

            float y = 35 + str.Height + 60 + strdate.Height + 20;
            int total=0;
            for (int i=0;i<dataGridView1.Rows.Count; i++)
            {
                y += 70;
                e.Graphics.DrawString(dataGridView1.Rows[i].Cells[0].Value.ToString(), f1, Brushes.Green, 25 + 150,y ) ;
                e.Graphics.DrawString(dataGridView1.Rows[i].Cells[1].Value.ToString(), f1, Brushes.Black, dis,y );
                e.Graphics.DrawString(dataGridView1.Rows[i].Cells[2].Value.ToString(), f1, Brushes.Black, dis - 125,y);
                e.Graphics.DrawString(dataGridView1.Rows[i].Cells[3].Value.ToString(), f1, Brushes.Black, dis - (125 * 2),y);
                e.Graphics.DrawLine(Pens.Black, 25, 50+y, e.PageSettings.PaperSize.Width - 25, 50 + y);
                total += int.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                
            }
            y += 70;
            e.Graphics.DrawString("Grand\nTotal", f1, Brushes.Blue, dis - (125 * 2), y);
            e.Graphics.DrawString(total.ToString(), f1, Brushes.Red, dis - 125, y);
            e.Graphics.DrawLine(Pens.Black, 25, 50 + y, e.PageSettings.PaperSize.Width - 25, 50 + y);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c is TextBox && c != textBox1 && c != textBox2)
                {
                    c.Text = "";
                }
                if (c is ComboBox)
                {
                    c.Text = "";
                }
            }
            if (id == 99999999)
            {
                id = 0;
                if (alpha == 'Z')
                {
                    alpha = 'a';
                }
            }
            id++;
            textBox1.Text = string.Format($"{alpha}{id:00000000}");
            dataGridView1.Rows.Clear();
        }
        char alpha = 'A';
        private void Form1_Load(object sender, EventArgs e)
            {
                this.Icon = Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.FriendlyName);

                p1 = label1.Location;
                p2 = label7.Location;

                w = this.Width;

                p4 = textBox4.Location;
                p5 = textBox5.Location;
                p6 = textBox4.Size;
                p7 = textBox5.Size;
                if (id == 99999999)
                {
                    id = 0;
                    if (alpha == 'Z')
                    {
                        alpha = 'a';
                    }
                }
                textBox1.Text = string.Format($"{alpha}{id:00000000}");

                textBox2.Text = DateTime.Now.ToString("dd/MM/yyyy");
                textBox1.DeselectAll();
            }
        private void Form1_Paint(object sender, PaintEventArgs e)
            {
                Point p1 = new Point(15, (textBox3.Location.Y + textBox3.Size.Height + 15));
                Point p2 = new Point(this.Size.Width - 30, (textBox3.Location.Y + textBox3.Size.Height + 15));
                this.CreateGraphics().DrawLine(Pens.Black, p1, p2);
                this.CreateGraphics().DrawLine(Pens.Black, 15, (textBox3.Location.Y + textBox3.Size.Height + 16), this.Size.Width - 30, (textBox3.Location.Y + textBox3.Size.Height + 16));
            
            }
        private void Form1_Resize(object sender, EventArgs e)
            {
                Point p3 = label7.Location;


                foreach (Control c in this.Controls)
                {
                    if (c == linkLabel1)
                    {
                        c.Location = new Point((this.Width / 2) - (c.Width / 2), 42);
                    }
                    if (c == label1)
                    {
                        c.Location = new Point((this.Width / 2) - (c.Width / 2), p1.Y);
                    }
                    if (c == label7)
                    {

                        Point p = comboBox1.Location;
                        int Y = comboBox1.Location.Y + comboBox1.Height + ((p2.Y) - (p.Y + comboBox1.Height));
                        c.Location = new Point(p.X + (comboBox1.Width - c.Width) / 2, Y);
                        //MessageBox.Show(c.Location.X + " " + c.Location.Y + " " + comboBox1.Location.X + " " + comboBox1.Location.Y);
                    }
                    if (c == textBox4)
                    {
                        c.Size = new Size((label7.Location.X) - (label7.Width + 35), (comboBox1.Location.Y) + comboBox1.Height + ((p2.Y) - (comboBox1.Location.Y + comboBox1.Height)));
                    }
                    if (c == textBox5)
                    {
                        c.Location = new Point(label7.Left + label7.Width + 5, textBox4.Location.Y);
                        c.Width = (comboBox1.Left + comboBox1.Width) - (label7.Left + label7.Width + 5);
                    }

                }
        }
    }
}
