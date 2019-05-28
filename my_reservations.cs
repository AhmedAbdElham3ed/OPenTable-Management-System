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
    public partial class my_reservations : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        string s;
        Form1 past;
        all_restaurants past1;
        making_reservation past2;
        Restaurant_Profile past3;
        public my_reservations()
        {
            InitializeComponent();
        }
        public my_reservations(Form1 f,string source)
        {
            InitializeComponent();
            s = source;
            past = f;
        }
        public my_reservations(all_restaurants f,string source)
        {
            InitializeComponent();
            s = source;
            past1 = f;
        }
        public my_reservations(making_reservation f,string source)
        {
            InitializeComponent();
            s = source;
            past2 = f;
        }
        public my_reservations(Restaurant_Profile f, string source)
        {
            InitializeComponent();
            s = source;
            past3 = f;
        }
        private void button12_Click(object sender, EventArgs e)
        {
            this.my_reservations_Load(this, e);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        public void my_reservations_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            int number_of_confirmedreservation = 0,number_of_unconfirmedreservation = 0,number_of_diningpoints=0; 
            string conn = "data source = orcl; user id = hr; password = hr;";
            OracleConnection con = new OracleConnection(conn);
            con.Open();
            string querystring = "select reservationid, resname, to_char(specificdateandtime, 'mm/dd/yyyy hh24:mi'),phone, occasion, numberofpeople, diningpointsoption, state from signedreservation where useremail='" + log_in.User_Email+ "' and (state='confirmed' or(state='unconfirmed' and specificdateandtime>=sysdate)) order by specificdateandtime DESC ";
            OracleCommand cmd = new OracleCommand(querystring, con);
            cmd.CommandType = CommandType.Text;
            OracleDataReader rd = cmd.ExecuteReader(),rd1;
            while (rd.Read()) 
            {
                cmd = new OracleCommand("select diningpoints from schedule where resname='" + rd[1].ToString() + "' and specificdateandtime=to_date('" + rd[2].ToString().Split()[0] + " " + rd[2].ToString().Split()[1] + "','mm/dd/yyyy hh24:mi')", con);
                cmd.CommandType = CommandType.Text;
                if (rd[7].ToString() == "unconfirmed")
                {
                    number_of_unconfirmedreservation++;
                    rd1 = cmd.ExecuteReader();
                    while (rd1.Read())
                    {
                        flowLayoutPanel1.Controls.Add(new unconfirmed_reservation(this, Convert.ToInt32(rd[0]), rd[1].ToString(), rd[2].ToString().Split(' ')[0], rd[2].ToString().Split(' ')[1], Convert.ToInt32(rd[5]),  rd[3].ToString(), rd[4].ToString(), Convert.ToInt32(rd1[0]), Convert.ToBoolean(rd[6])));
                    }
                    rd1.Close();
                    
                }
                else 
                {
                    number_of_confirmedreservation++;
                    rd1 = cmd.ExecuteReader();
                    while (rd1.Read())
                    {
                        number_of_diningpoints += Convert.ToInt32(rd1[0]);
                        flowLayoutPanel1.Controls.Add(new reservation(this, Convert.ToInt32(rd[0]), rd[1].ToString(), rd[3].ToString(), log_in.User_Email, rd[2].ToString().Split(' ')[0], rd[2].ToString().Split(' ')[1], Convert.ToInt32(rd[5]), rd[4].ToString(),    Convert.ToInt32(rd1[0]), Convert.ToBoolean(rd[6]), "My_reservation","confirmed"));
                    }
                    rd1.Close();
                }
            }
            rd.Close();
            // connect with frontend
            con.Close();

            label5.Text = number_of_unconfirmedreservation.ToString();
            label6.Text = number_of_confirmedreservation.ToString();
            label1.Text = number_of_diningpoints.ToString();
            label3.Text = "hello, " + log_in.firstname;
            label3.Visible = true;
            bunifuImageButton3.Visible = true;
           
            
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            if (panel4.Visible == true)
                panel4.Visible = false;
            else if (panel4.Visible == false)
                panel4.Visible = true;
        }

        private void Sign_in_button_Click(object sender, EventArgs e)
        {
            Sign_in s = new Sign_in(this, "My_reservation");
            s.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            log_in.logged_in = false;
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
            else if (s == "Restaurant_profile")
            {
                past3.Show();
                past3.Restaurant_Profile_Load(past3, e);
            }
            
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            My_Profile m_p = new My_Profile(this, "My_reservation");
            m_p.Show();
            this.Hide();
        }

        private void bunifuImageButton6_Click(object sender, EventArgs e)
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
            else if (s == "Restaurant_profile")
            {
                past3.Show();
                past3.Restaurant_Profile_Load(past3, e);
            }
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1(this);
            f.Show();
            this.Hide();
        }

        private void Sign_up_button_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuImageButton2_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void bunifuImageButton2_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void bunifuImageButton3_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void bunifuImageButton3_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void button12_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button12_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void button7_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void bunifuImageButton6_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void bunifuImageButton6_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void my_reservations_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void my_reservations_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
