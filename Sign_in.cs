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
    public partial class Sign_in : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        string Connection = "Data Source=orcl;user id=hr;password=hr;";
        OracleConnection con;
        OracleDataReader dr,dr1;
        OracleCommand cmd;
        string s;
        Form1 past;
        all_restaurants past1;
        making_reservation past2;
        my_reservations past3;
        Restaurant_Profile past4;
        public Sign_in()
        {
            InitializeComponent();
        }
        public Sign_in(Form1 f, string source)
        {
            InitializeComponent();
            s = source;
            past = f;
        }
        public Sign_in(all_restaurants f, string source)
        {
            InitializeComponent();
            s = source;
            past1 = f;
        }
        public Sign_in(making_reservation f, string source)
        {
            InitializeComponent();
            s = source;
            past2 = f;
        }
        public Sign_in(my_reservations f, string source)
        {
            InitializeComponent();
            s = source;
            past3 = f;
        }
        public Sign_in(Restaurant_Profile f, string source)
        {
            InitializeComponent();
            s = source;
            past4 = f;
        }
        private void Sign_in_Load(object sender, EventArgs e)
        {
            con = new OracleConnection(Connection);
            
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

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if (s == "Home")
            {
                past.Show();
                past.Form1_Load(past, e);
            }
            else if (s == "Restaurants")
            {
                past1.Show();
                past1.all_restaurants_Load(past1, e);
            }
            else if (s == "Making Reservation")
            {
                past2.Show();
                past2.making_reservation_Load(past2, e);
            }
            else if (s == "My_reservation")
            {
                past3.Show();
                past3.my_reservations_Load(past3, e);
            }
            else if (s == "Restaurant_profile")
            {
                past4.Show();
                past4.Restaurant_Profile_Load(past4, e);
            }
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bunifuMetroTextbox1.Text == "" || bunifuMetroTextbox2.Text == "")
                MessageBox.Show("Please You Should Complete information");
            else
            {
                con.Open();
                string useremail = bunifuMetroTextbox1.Text;
                string pass = bunifuMetroTextbox2.Text;
                cmd = new OracleCommand("select * from USERACCOUNT where USEREMAIL=:useremail and user_password=:pass", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("useremail", useremail);
                cmd.Parameters.Add("pass", pass);
                 dr = cmd.ExecuteReader();
                bool yep = false, yep1 = false;
                if (dr.Read())
                    yep = true;
                else
                {
                    cmd.CommandText = "select * from ADMINs where EMAIL=:useremail and admin_Password=:pass";
                    cmd.CommandType = CommandType.Text;
                    
                    dr1 = cmd.ExecuteReader();
                    if (dr1.Read())
                    {
                        yep1 = true;
                        Restaurant_admin r;
                        log_in.logged_in = true;
                        log_in.User_Email = useremail;
                        log_in.password = pass;
                        if (s == "Home")
                        {
                            r = new Restaurant_admin(past, "Home", dr1[2].ToString());
                            r.Show();
                        }
                        else if (s == "Restaurants")
                        {
                            r = new Restaurant_admin(past1, "Restaurants", dr1[2].ToString());
                            r.Show();
                        }
                        else if (s == "Making Reservation")
                        {
                            r = new Restaurant_admin(past2, "Making Reservation", dr1[2].ToString());
                            r.Show();
                        }
                        else if (s == "My_reservation")
                        {
                            r = new Restaurant_admin(past3, "My_reservation", dr1[2].ToString());
                            r.Show();
                        }
                        else if (s == "Restaurant_profile")
                        {
                            r = new Restaurant_admin(past4, "Restaurant_profile", dr1[2].ToString());
                            r.Show();
                        }
                        this.Hide();
                    }
                }
                if (yep&&!yep1)
                {
                    
                    log_in.firstname = dr[2].ToString();
                    log_in.lastname = dr[3].ToString();
                    log_in.Location = dr[4].ToString();
                    log_in.logged_in = true;
                    log_in.User_Email = bunifuMetroTextbox1.Text;
                    if (s == "Home")
                    {
                        past.Show();
                        past.Form1_Load(past, e);
                    }
                    else if (s == "Restaurants")
                    {
                        past1.Show();
                        past1.all_restaurants_Load(past1, e);
                    }
                    else if (s == "Making Reservation")
                    {
                        past2.Show();
                        past2.making_reservation_Load(past2, e);
                    }
                    else if (s == "My_reservation")
                    {
                        past3.Show();
                        past3.my_reservations_Load(past3, e);
                    }
                    else if(s== "Restaurant_profile")
                    {
                        past4.Show();
                        past4.Restaurant_Profile_Load(past4, e);
                    }
                    this.Hide();
                }
                else if(!yep&&!yep1)
                    MessageBox.Show("Useremail or Password is not correct");
                dr.Close();
                con.Close();
            }
            bunifuMetroTextbox1.Text = "";
            bunifuMetroTextbox2.Text = "";
        }
        private void Sign_in_FormClosed(object sender, FormClosedEventArgs e)
        {
            con.Dispose();
            Application.Exit();
        }

        private void Sign_in_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Sign_in_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
