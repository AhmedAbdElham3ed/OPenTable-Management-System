using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
namespace OpenTable
{
    public partial class Make_Review : UserControl
    {
        Restaurant_Profile past;
        string restaurant_name;
        double rate;
        Bitmap bmp;
        Bitmap bmp1;
        List<Button> buttons = new List<Button>();
        public Make_Review()
        {
            InitializeComponent();
        }
        public Make_Review(Restaurant_Profile r,string res_name,string name,string city)
        {
            InitializeComponent();
            past = r;
            restaurant_name = res_name;
            button2.Text = name;
            label2.Text = city;
            bmp1 = (Bitmap)button8.Image;
            buttons.Add(button7);
            buttons.Add(button3);
            buttons.Add(button4);
            buttons.Add(button5);
            buttons.Add(button6);
        }
        private void return_to_orginal_color(int button_width)
        {
            Color actualColor;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //get the pixel from the scrBitmap image
                    actualColor = bmp1.GetPixel(i, j);
                    //MessageBox.Show(actualColor.ToString());
                    // > 150 because.. Images edges can be of low pixel colr. if we set all pixel color to new then there will be no smoothness left.
                    if (actualColor.A > 150)
                        bmp.SetPixel(i, j, actualColor);
                    //else
                    //bmp.SetPixel(i, j, actualColor);
                }
            }
            
        }
        private void change_color( Color new_color, int button_width)
        {

            Color actualColor;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //get the pixel from the scrBitmap image
                    actualColor = bmp.GetPixel(i, j);
                    //MessageBox.Show(actualColor.ToString());
                    // > 150 because.. Images edges can be of low pixel colr. if we set all pixel color to new then there will be no smoothness left.
                    if (actualColor.A > 150 && i <= button_width)
                        bmp.SetPixel(i, j, new_color);
                    else
                        bmp.SetPixel(i, j, actualColor);
                }
            }
        }
        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            rate = 2;
            for (int i = 0; i < 5; i++)
            {
                bmp = (Bitmap)(buttons[i].Image);
                return_to_orginal_color(buttons[i].Width);
                buttons[i].Refresh();
            }
            for (int i = 0; i < 2; i++)
            {
                bmp = (Bitmap)(buttons[i].Image);
                change_color(Color.FromArgb(218, 55, 67), buttons[i].Width);
                buttons[i].Refresh();
            }
            var relativePoint = button4.PointToClient(Cursor.Position);
            bmp = (Bitmap)(buttons[2].Image);
            change_color(Color.FromArgb(218, 55, 67), relativePoint.X);
            rate += ((double)relativePoint.X / (double)(buttons[2].Width));
            rate = (double)Math.Round((decimal)(rate), 2);
            buttons[2].Refresh();
            // MessageBox.Show(rate.ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rate = 3;
            for (int i = 0; i < 5; i++)
            {
                bmp = (Bitmap)(buttons[i].Image);
                return_to_orginal_color(buttons[i].Width);
                buttons[i].Refresh();
            }
            for (int i = 0; i < 3; i++)
            {
                bmp = (Bitmap)(buttons[i].Image);
                change_color(Color.FromArgb(218, 55, 67), buttons[i].Width);
                buttons[i].Refresh();
            }
            var relativePoint = button5.PointToClient(Cursor.Position);
            bmp = (Bitmap)(buttons[3].Image);
            change_color(Color.FromArgb(218, 55, 67), relativePoint.X);
            rate += ((double)relativePoint.X / (double)(buttons[3].Width));
            rate = (double)Math.Round((decimal)(rate), 2);
            buttons[3].Refresh();
            // MessageBox.Show(rate.ToString());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            rate = 4;
            for (int i = 0; i < 5; i++)
            {
                bmp = (Bitmap)(buttons[i].Image);
                return_to_orginal_color(buttons[i].Width);
                buttons[i].Refresh();
            }
            for (int i = 0; i < 4; i++)
            {
                bmp = (Bitmap)(buttons[i].Image);
                change_color(Color.FromArgb(218, 55, 67), buttons[i].Width);
                buttons[i].Refresh();
            }
            var relativePoint = button6.PointToClient(Cursor.Position);
            bmp = (Bitmap)(buttons[4].Image);
            change_color(Color.FromArgb(218, 55, 67), relativePoint.X);
            rate += ((double)relativePoint.X / (double)(buttons[4].Width));
            rate = (double)Math.Round((decimal)(rate), 2);
            buttons[4].Refresh();
            //MessageBox.Show(rate.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rate = 1;
            for (int i = 0; i < 5; i++)
            {
                bmp = (Bitmap)(buttons[i].Image);
                return_to_orginal_color(buttons[i].Width);
                buttons[i].Refresh();
            }
            for(int i=0;i<1;i++)
            {
                bmp = (Bitmap)(buttons[i].Image);
                change_color(Color.FromArgb(218, 55, 67), buttons[i].Width);
                buttons[i].Refresh();
            }
            var relativePoint = button3.PointToClient(Cursor.Position);
            bmp = (Bitmap)(buttons[1].Image);
            change_color(Color.FromArgb(218, 55, 67), relativePoint.X);
            rate += ((double)relativePoint.X / (double)(buttons[1].Width));
            rate = (double)Math.Round((decimal)(rate), 1);
            buttons[1].Refresh();
            //MessageBox.Show(rate.ToString());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            rate = 0;
            for(int i=0;i<5;i++)
            {
                bmp = (Bitmap)(buttons[i].Image);
                return_to_orginal_color( buttons[i].Width);
                buttons[i].Refresh();
            }
            
            var relativePoint = button7.PointToClient(Cursor.Position);
            bmp = (Bitmap)(buttons[0].Image);
            change_color(Color.FromArgb(218, 55, 67), relativePoint.X);
            rate += ((double)relativePoint.X / (double)(buttons[0].Width));
            rate=(double)Math.Round((decimal)(rate), 1);
            buttons[0].Refresh();
            //MessageBox.Show(rate.ToString());    
        }

        private void Make_Review_Load(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Connection = "Data Source=orcl;user id=hr;password=hr;";
            OracleConnection con = new OracleConnection(Connection);
            con.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = "insert into raterestaurant values('"+restaurant_name+"','"+log_in.User_Email+"','"+richTextBox1.Text+"',"+rate.ToString()+")";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            past.button5_Click(past,e);
        }
    }
}
