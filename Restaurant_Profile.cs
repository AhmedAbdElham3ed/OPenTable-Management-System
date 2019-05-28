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
    public partial class Restaurant_Profile : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        string s,res_name;
        all_restaurants past;
        Form1 past1;
        string con = "data source = orcl; user id = hr; password = hr;";
        OracleConnection connection;
        OracleCommand cmd;
        public Restaurant_Profile()
        {
            InitializeComponent();
        }
        public Restaurant_Profile(string source,all_restaurants a_r,string _res_name)
        {
            InitializeComponent();
            s = source;
            past = a_r;
            res_name = _res_name;
            button6.Visible = true;
            button7.Visible = false;
            button8.Visible = false;
            flowLayoutPanel1.Controls.Add(new Overview(res_name));
        }
        public Restaurant_Profile(string source, Form1 f, string _res_name)
        {
            InitializeComponent();
            s = source;
            past1 = f;
            res_name = _res_name;
            button6.Visible = true;
            button7.Visible = false;
            button8.Visible = false;
            flowLayoutPanel1.Controls.Add(new Overview(res_name));
        }
        private void button4_Click(object sender, EventArgs e)
        {
            
            button6.Visible = false;
            button7.Visible = true;
            button8.Visible = false;
            flowLayoutPanel1.Controls.Clear();
            
            string query = "select categoryname from foodcategory where resname='" + res_name + "' and foodname!='null' group by categoryname";
            OracleDataAdapter adapter = new OracleDataAdapter(query, con);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            // connect with frontend
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                flowLayoutPanel1.Controls.Add(new Menu(this, "Restaurant_profile", res_name, r[0].ToString()));
            }
            
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        public void Restaurant_Profile_Load(object sender, EventArgs e)
        {
            /*
             Code Here
             */
            label4.Visible = false;
            connection = new OracleConnection(con);
            connection.Open();
            cmd=new OracleCommand();
            cmd.Connection = connection;
            cmd.CommandText = "select maxparty_size from restaurant where resname='"+res_name+"'";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            bunifuDropdown1.Clear();
            if(dr.Read())
            {
                for(int i=1;i<=Convert.ToInt32( dr[0]);i++)
                {
                    bunifuDropdown1.AddItem(i.ToString()+" people");
                }
            }
            connection.Close();
            
            bunifuDropdown2.selectedIndex = 0;
            bunifuDropdown1.selectedIndex = 0;
            bunifuDatepicker1.Value = DateTime.Now;
            label4.Visible = false;
            if (log_in.logged_in == true)
            {
                label3.Text = "hello, " + log_in.firstname;
                label3.Visible = true;
                bunifuImageButton3.Visible = true;
                Sign_in_button.Visible = false;
                Sign_up_button.Visible = false;
                panel6.Visible = false;
            }
            else
            {
                label3.Visible = false;
                bunifuImageButton3.Visible = false;
                Sign_in_button.Visible = true;
                Sign_up_button.Visible = true;
                panel6.Visible = false;
            }
            
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Restaurant_Profile_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Restaurant_Profile_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            if (panel6.Visible == true)
                panel6.Visible = false;
            else if (panel6.Visible == false)
                panel6.Visible = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            my_reservations m_r = new my_reservations(this, "Restaurant_profile");
            m_r.Show();
            this.Hide();
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

        private void button11_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button11_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void button10_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button10_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void button9_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button9_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Sign_in_button_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Sign_in_button_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Sign_up_button_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Sign_up_button_MouseLeave(object sender, EventArgs e)
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

        private void button1_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            button6.Visible = true;
            button7.Visible = false;
            button8.Visible = false;
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Controls.Add(new Overview(res_name));
        }

        public void button5_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = true;
            flowLayoutPanel1.Controls.Clear();
            
            connection = new OracleConnection(con);
            connection.Open();
            cmd = new OracleCommand();
            cmd.Connection = connection;
            if (log_in.logged_in)
            {
                cmd.CommandText = "select count(*) from signedreservation where useremail='" + log_in.User_Email + "' and resname='" + res_name + "' and state='confirmed'";
                cmd.CommandType = CommandType.Text;
                OracleDataReader rd=cmd.ExecuteReader();
                if (rd.Read() && Convert.ToInt32(rd[0]) > 0)
                {
                    cmd.CommandText = "select count(*) from raterestaurant where useremail='" + log_in.User_Email + "' and resname='" + res_name + "'";
                    cmd.CommandType = CommandType.Text;
                    rd = cmd.ExecuteReader();
                    if (rd.Read() && Convert.ToInt32(rd[0]) == 0)
                    {
                        flowLayoutPanel1.Controls.Add(new Make_Review(this, res_name, log_in.firstname, log_in.Location));
                    }
                }
            }
            string query = "select * from raterestaurant where resname='" + res_name + "'";
            OracleDataAdapter adapter = new OracleDataAdapter(query, con);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                 
                
                cmd.CommandText = "select firstname,city from useraccount where useremail='"+r[1].ToString()+"'";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr =cmd.ExecuteReader();
                while(dr.Read())
                {
                    flowLayoutPanel1.Controls.Add(new Review(dr[0].ToString(), dr[1].ToString(), Convert.ToDouble(r[3]), r[2].ToString()));
                }
                dr.Close();
                
            }
            connection.Close();
            //
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1(this);
            f.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            My_Profile m_p = new My_Profile(this, "Restaurant_profile");
            m_p.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            log_in.logged_in = false;
            this.Restaurant_Profile_Load(this, e);
        }

        private void Sign_in_button_Click(object sender, EventArgs e)
        {
            Sign_in s = new Sign_in(this, "Restaurant_profile");
            s.Show();
            this.Hide();
        }

        private void Sign_up_button_Click(object sender, EventArgs e)
        {
            Sign_up s = new Sign_up(this, "Restaurant_profile");
            s.Show();
            this.Hide();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if (s == "Home")
            {
                past1.Show();
                past1.Form1_Load(past1, e);
                this.Hide();
            }
            else if (s == "Restaurants")
            {
                past.Show();
                past.all_restaurants_Load(past, e);
                this.Hide();
            }
            
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connection = new OracleConnection(con);
            connection.Open();
            cmd = new OracleCommand();
            cmd.Connection = connection;
            cmd.CommandText = "select diningpoints from schedule where resname='" + res_name + "' and specificdate='" + bunifuDatepicker1.Value.ToShortDateString().Split(' ')[0] + "' and specificdateandtime=to_date('" + bunifuDatepicker1.Value.ToShortDateString().Split(' ')[0] + " " + bunifuDropdown2.selectedValue + "','mm/dd/yyyy hh24:mi') and to_date('" + bunifuDatepicker1.Value.ToShortDateString().Split(' ')[0] + " " + bunifuDropdown2.selectedValue + "','mm/dd/yyyy hh24:mi')>sysdate and numoftables>0";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                making_reservation m_r = new making_reservation("Restaurant_profile", this, res_name, bunifuDatepicker1.Value.ToShortDateString().Split(' ')[0], bunifuDropdown2.selectedValue, Convert.ToInt32(bunifuDropdown1.selectedValue.Split(' ')[0]),Convert.ToInt32( dr[0]));
                m_r.Show();
                this.Hide();
            }
            else
                label4.Visible = true;
        }
    }
}
