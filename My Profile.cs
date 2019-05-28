using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
namespace OpenTable
{
    public partial class My_Profile : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        string s;
        all_restaurants past;
        making_reservation past1;
        my_reservations past2;
        Restaurant_Profile past3;
        Form1 f;
        string Connection = "Data Source=orcl;user id=hr;password=hr;";
        OracleConnection con;
        OracleCommand cmd;
        OracleDataReader reader;
        public My_Profile()
        {
            InitializeComponent();
        }
        public My_Profile(Form1 past,string source)
        {
            InitializeComponent();
            f = past;
            s = source;
        }
        public My_Profile(all_restaurants _past, string source)
        {
            InitializeComponent();
            past = _past;
            s = source;
        }
        public My_Profile(making_reservation _past, string source)
        {
            InitializeComponent();
            past1 = _past;
            s = source;
        }
        public My_Profile(my_reservations _past, string source)
        {
            InitializeComponent();
            past2 = _past;
            s = source;
        }
        public My_Profile(Restaurant_Profile _past, string source)
        {
            InitializeComponent();
            past3 = _past;
            s = source;
        }
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            
            if (s == "Home")
            {
                f.Show();
                f.Form1_Load(f, e);
            }
            else if (s == "Restaurants")
            {
                past.Show();
                past.all_restaurants_Load(past, e);
            }
            else if (s == "Making Reservation")
            {
                past1.Show();
                past1.making_reservation_Load(past1, e);
            }
            else if (s == "My_reservation")
            {
                past2.Show();
                past2.my_reservations_Load(past2, e);
            }
            else if (s == "Restaurant_profile")
            {
                past3.Show();
                past3.Restaurant_Profile_Load(past3, e);
            }
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (bunifuMetroTextbox1.Text == "" || bunifuMetroTextbox2.Text == "" || bunifuMetroTextbox3.Text == "" || bunifuMetroTextbox4.Text == ""||bunifuMetroTextbox5.Text=="")
            {
                MessageBox.Show("please make sure that you entered your data in a proper way");
            }
            else if (bunifuMetroTextbox4.Text != bunifuMetroTextbox5.Text)
            {
                MessageBox.Show("passwords don't match");
            }
            else
            {
                con.Open();
                cmd = new OracleCommand("update USERACCOUNT set user_password=:pass , FIRSTNAME=:fname , LASTNAME=:lname,city=:user_city where USEREMAIL=:user_mail", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("pass", bunifuMetroTextbox4.Text);
                cmd.Parameters.Add("fname", bunifuMetroTextbox1.Text);
                cmd.Parameters.Add("lname", bunifuMetroTextbox2.Text);
                cmd.Parameters.Add("user_city", bunifuDropdown4.selectedValue);
                log_in.Location = bunifuDropdown4.selectedValue;
                log_in.firstname = bunifuMetroTextbox1.Text;
                log_in.lastname = bunifuMetroTextbox2.Text;
                log_in.password = bunifuMetroTextbox4.Text;
                cmd.Parameters.Add("user_mail", bunifuMetroTextbox3.Text);
                int num_of_affected_rows = cmd.ExecuteNonQuery();
                if (num_of_affected_rows == 1)
                {
                    MessageBox.Show("your data get updated");
                    if (s == "Home")
                    {
                        f.Show();
                        f.Form1_Load(f, e);
                    }
                    else if (s == "Restaurants")
                    {
                        past.Show();
                        past.all_restaurants_Load(past, e);
                    }
                    else if (s == "Making Reservation")
                    {
                        past1.Show();
                        past1.making_reservation_Load(past1, e);
                    }
                    else if (s == "My_reservation")
                    {
                        past2.Show();
                        past2.my_reservations_Load(past2, e);
                    }
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("there is a problem updating your data please try again later");
                }
                con.Close();
            }
            
        }

        private void My_Profile_Load(object sender, EventArgs e)
        {
            con = new OracleConnection(Connection);
            con.Open();
            cmd = new OracleCommand("select FIRSTNAME,LASTNAME,user_password,city from USERACCOUNT where USEREMAIL='"+log_in.User_Email+"' ", con);
            cmd.CommandType = CommandType.Text;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                bunifuMetroTextbox1.Text = reader[0].ToString();
                bunifuMetroTextbox2.Text = reader[1].ToString();
                bunifuMetroTextbox3.Text = log_in.User_Email;
                bunifuMetroTextbox4.Text = reader[2].ToString();
                bunifuMetroTextbox5.Text = reader[2].ToString();
                bunifuDropdown4.selectedIndex = log_in.locations[ reader[3].ToString()]-1;
            }
            bunifuMetroTextbox3.Enabled = false;
            reader.Close();
            con.Close();

        }

        private void bunifuImageButton2_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void bunifuImageButton2_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void bunifuImageButton1_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void bunifuImageButton1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void My_Profile_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void My_Profile_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void My_Profile_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
