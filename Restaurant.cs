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
    public partial class Restaurant : UserControl
    {
        Bitmap bmp;
        List<Button> buttons = new List<Button>();
        all_restaurants past1;
        string Restaurant_name,date="",time="",type="";
        int number_of_people;


        string Connection = "Data Source=orcl;user id=hr;password=hr;";
        OracleConnection con;
        OracleCommand cmd;
        OracleDataReader reader;
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
        public Restaurant()
        {
            InitializeComponent();
        }
        public Restaurant(all_restaurants a_r,string restaurant_name, double rate,int _number_of_reviews, string cusine, string average_price, string location, int Number_of_reservations ,string _date,int _number_of_people,string type_of_search)
        {
            InitializeComponent();
            past1 = a_r;
            Restaurant_name = restaurant_name;
            number_of_people = _number_of_people;
            buttons.Add(button2);
            buttons.Add(button3);
            buttons.Add(button4);
            buttons.Add(button5);
            buttons.Add(button6);
            for (int i = 0; i < (int)rate; i++)
            {
                bmp = (Bitmap)(buttons[i].Image);
                change_color( Color.FromArgb(218, 55, 67), buttons[i].Width);
            }
            if ((int)rate < 5)
            {
                bmp = (Bitmap)(buttons[(int)rate].Image);
                //MessageBox.Show(((int)(buttons[(int)rate].Width * (rate - (double)((int)rate))) + 1).ToString());
                change_color(Color.FromArgb(218, 55, 67), (int)(buttons[(int)rate].Width * (rate - (double)((int)rate))) + 1);

            }
            label1.Text = restaurant_name;
            label3.Text = cusine;
            label5.Text = average_price;
            label6.Text = location;
            if (Number_of_reservations > 0)
                label4.Text = "booked " + Number_of_reservations.ToString() + " times today";
            else
            {
                label4.Text = "";
                bunifuImageButton1.Visible = false;
            }

            if (_number_of_reviews > 0)
                label2.Text = _number_of_reviews.ToString()+" reviews";
            else
                label2.Text = "";

            type = type_of_search;
            if(type== "specified search")
            {
                date = _date.Split(' ')[0];
                time = _date.Split(' ')[1];
                //MessageBox.Show(date+" "+time);
                con = new OracleConnection(Connection);
                con.Open();
                cmd = new OracleCommand("select diningpoints from schedule where resname='"+ Restaurant_name + "' and specificdate = '"+date+ "' and specificdateandtime=to_date('" + date + " " + time + "','mm/dd/yyyy hh24:mi') and to_date('" + date + " " + time + "','mm/dd/yyyy hh24:mi')>sysdate", con);
                cmd.CommandType = CommandType.Text;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                   // MessageBox.Show(date + " " + time);
                    flowLayoutPanel1.Controls.Add(new specific_time(past1, Restaurant_name, date, time, number_of_people, Convert.ToInt32(reader[0])));
                }
                reader.Close();
                con.Close();
            }
            else
            {
                date = _date.Split(' ')[0];
                con = new OracleConnection(Connection);
                con.Open();
                cmd = new OracleCommand("select to_char(specificdateandtime,'mm/dd/yyyy hh24:mi'),diningpoints from schedule where resname='" + Restaurant_name + "' and specificdate = '" + date + "' and specificdateandtime>sysdate and numoftables>0", con);
                cmd.CommandType = CommandType.Text;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    // MessageBox.Show(date + " " + time);
                    flowLayoutPanel1.Controls.Add(new specific_time(past1, Restaurant_name, date,  reader[0].ToString().Split(' ')[1], number_of_people, Convert.ToInt32(reader[1])));
                }
                reader.Close();
                con.Close();
            }
            /*if (type == "specified search")
            {
                for (int i = 0; i < 12; i++)
                {
                    flowLayoutPanel1.Controls.Add(new specific_time(past1, Restaurant_name, date.Split(' ')[0],date.Split(' ')[1], number_of_people, 1000));
                }
            }*/
        }
        
        private void Restaurant_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void panel1_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Restaurant_Profile r = new Restaurant_Profile("Restaurants", past1, Restaurant_name);
            r.Show();
            past1.Hide();
        }
    }
}
