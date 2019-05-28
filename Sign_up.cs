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
    public partial class Sign_up : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        Form1 past;
        all_restaurants past1;
        making_reservation past2;
        my_reservations past3;
        Restaurant_Profile past4;
        string s;
        string conn = "data source = orcl; user id = hr; password = hr;";
        OracleConnection connection;
        OracleCommand cmd;
        OracleDataAdapter adapter;
        OracleCommandBuilder builder;
        DataSet ds;

        public Sign_up()
        {
            InitializeComponent();
        }
        public Sign_up(Form1 f,string source)
        {
            InitializeComponent();
            s = source;
            past = f;
        }
        public Sign_up(all_restaurants f, string source)
        {
            InitializeComponent();
            s = source;
            past1 = f;
        }
        public Sign_up(making_reservation f, string source)
        {
            InitializeComponent();
            s = source;
            past2 = f;
        }
        public Sign_up(my_reservations f, string source)
        {
            InitializeComponent();
            s = source;
            past3 = f;
        }
        public Sign_up(Restaurant_Profile f, string source)
        {
            InitializeComponent();
            s = source;
            past4 = f;
        }
        private void bunifuMetroTextbox5_OnValueChanged(object sender, EventArgs e)
        {

        }

        public void Sign_up_Load(object sender, EventArgs e)
        {
            bunifuMetroTextbox1.Text = bunifuMetroTextbox2.Text = bunifuMetroTextbox3.Text = bunifuMetroTextbox4.Text = bunifuMetroTextbox5.Text= "";
            bunifuDropdown4.selectedIndex = 0;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bunifuMetroTextbox1.Text == "" || bunifuMetroTextbox2.Text == "" || bunifuMetroTextbox3.Text == "" || bunifuMetroTextbox4.Text == "" || bunifuMetroTextbox5.Text == "")
            {
                MessageBox.Show("Please You should complete your information");
            }
            else if (bunifuMetroTextbox4.Text != bunifuMetroTextbox5.Text)
            {
                MessageBox.Show("Please The Password should be Same");
                bunifuMetroTextbox4.Text = bunifuMetroTextbox5.Text = "";
            }
            else
            {
                connection = new OracleConnection(conn);
                connection.Open();
                cmd = new OracleCommand("select count(useremail) from useraccount where useremail ='" + bunifuMetroTextbox3.Text + "'", connection);
                cmd.CommandType = CommandType.Text;
                OracleDataReader Reader = cmd.ExecuteReader();
                int check1 =0,check2=0;
                while (Reader.Read())
                    check1 = int.Parse(Reader[0].ToString());
                cmd = new OracleCommand("select count(email) from admins where email ='" + bunifuMetroTextbox3.Text + "'", connection);
                cmd.CommandType = CommandType.Text;
                Reader = cmd.ExecuteReader();
                while (Reader.Read())
                    check2 = int.Parse(Reader[0].ToString());
                if (check1==1||check2==1)
                {
                    MessageBox.Show("Account is already Exist");
                    bunifuMetroTextbox3.Text = "";
                }
                else
                {
                    string query = @"select * from UserAccount";
                    adapter = new OracleDataAdapter(query, conn);
                    ds = new DataSet();
                    adapter.Fill(ds);
                    DataColumn[] d = new DataColumn[1];
                    d[0] = ds.Tables[0].Columns[0]; 
                    ds.Tables[0].PrimaryKey = d;
                    DataRow r = ds.Tables[0].NewRow();
                    r["useremail"] = bunifuMetroTextbox3.Text;
                    r["user_password"] = bunifuMetroTextbox4.Text;
                    r["firstname"] = bunifuMetroTextbox1.Text;
                    r["lastname"] = bunifuMetroTextbox2.Text;
                    r["city"] = bunifuDropdown4.selectedValue;
                    ds.Tables[0].Rows.Add(r);
                    builder = new OracleCommandBuilder(adapter);
                    adapter.Update(ds.Tables[0]);
                    MessageBox.Show("Account is Created");
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
                    Reader.Close();
                    connection.Close();
                    bunifuMetroTextbox1.Text = bunifuMetroTextbox2.Text = bunifuMetroTextbox3.Text = bunifuMetroTextbox4.Text = bunifuMetroTextbox5.Text = "";
                }
            }
           
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if(s=="Home")
            {
                past.Show();
                past.Form1_Load(past,e);
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (s == "Home")
            {
                register_Restaurant r = new register_Restaurant(this,past, s);
                r.Show();
            }
            else if (s == "Restaurants")
            {
                register_Restaurant r = new register_Restaurant(this, past1, s);
                r.Show();
            }
            else if (s == "Making Reservation")
            {
                register_Restaurant r = new register_Restaurant(this, past2, s);
                r.Show();
            }
            else if (s == "My_reservation")
            {
                register_Restaurant r = new register_Restaurant(this, past3, s);
                r.Show();
            }
            else if (s == "Restaurant_profile")
            {
                register_Restaurant r = new register_Restaurant(this, past4, s);
                r.Show();
            }
            this.Hide();
        }

        private void Sign_up_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Sign_up_FormClosing(object sender, FormClosingEventArgs e)
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
