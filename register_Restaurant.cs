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
    public partial class register_Restaurant : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        Form1 past1;
        all_restaurants past2;
        making_reservation past3;
        my_reservations past4;
        Restaurant_Profile past5;
        string s;
        Sign_up temp_past;


        string Connection = "Data Source=orcl;user id=hr;password=hr;";
        public register_Restaurant()
        {
            InitializeComponent();
        }
        public register_Restaurant(Sign_up s_u,Form1 f,string source)
        {
            InitializeComponent();
            temp_past = s_u;
            past1 = f;
            s = source;
        }
        public register_Restaurant(Sign_up s_u, all_restaurants f, string source)
        {
            InitializeComponent();
            temp_past = s_u;
            past2 = f;
            s = source;
        }
        public register_Restaurant(Sign_up s_u, making_reservation f, string source)
        {
            InitializeComponent();
            temp_past = s_u;
            past3 = f;
            s = source;
        }
        public register_Restaurant(Sign_up s_u, my_reservations f, string source)
        {
            InitializeComponent();
            temp_past = s_u;
            past4 = f;
            s = source;
        }
        public register_Restaurant(Sign_up s_u, Restaurant_Profile f, string source)
        {
            InitializeComponent();
            temp_past = s_u;
            past5 = f;
            s = source;
        }
        private void register_Restaurant_Load(object sender, EventArgs e)
        {
            bunifuDropdown3.selectedIndex = 1;
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            temp_past.Show();
            temp_past.Sign_up_Load(this, e);
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            string name = bunifuMetroTextbox1.Text;
            string city = bunifuDropdown4.selectedValue;
            string street = bunifuMetroTextbox8.Text;
            string phone = bunifuMetroTextbox3.Text;
            string cuisine = bunifuDropdown3.selectedValue;
            string party_size = bunifuMetroTextbox11.Text;
            string min_price = bunifuMetroTextbox5.Text;
            string max_price = bunifuMetroTextbox7.Text;
            string exe_chef = bunifuMetroTextbox6.Text;
            string admin_email = bunifuMetroTextbox10.Text;
            string admin_password = bunifuMetroTextbox9.Text;
            string payment_method = richTextBox1.Text;
            OracleConnection con = new OracleConnection(Connection);
            con.Open();
            OracleCommand cmd = new OracleCommand("select count(email) from ADMINS where email='" + admin_email + "'", con);
            cmd.CommandType = CommandType.Text;

            OracleDataReader reader = cmd.ExecuteReader();
            int check = 0;
            int check2 = 0;
            int check3 = 0;
            while (reader.Read())
            {
                check = int.Parse(reader[0].ToString());
            }
            reader.Close();
            cmd = new OracleCommand("select count(RESNAME) from RESTAURANT where RESNAME='" + name + "'", con);
            cmd.CommandType = CommandType.Text;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                check2 = int.Parse(reader[0].ToString());
            }
            reader.Close();
            cmd = new OracleCommand("select count(useremail) from useraccount where useremail='" + admin_email + "'", con);
            cmd.CommandType = CommandType.Text;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                check3 = int.Parse(reader[0].ToString());
            }
            if (check == 1 || check2 == 1 || check3 == 1)
            {
                MessageBox.Show("The Data is Exist");
            }
            else
            {
                OracleCommand cmd3 = new OracleCommand("insert_into_res_admin_pay", con);
                cmd3.CommandType = CommandType.StoredProcedure;
                // in parameters 
                cmd3.Parameters.Add("res_name", name);
                cmd3.Parameters.Add("street", street);
                cmd3.Parameters.Add("res_location", city);  // city is location 
                cmd3.Parameters.Add("phone", phone);
                cmd3.Parameters.Add("cusine", cuisine);
                cmd3.Parameters.Add("minp", Convert.ToInt32 (min_price));
                cmd3.Parameters.Add("maxp", Convert.ToInt32(max_price));
                cmd3.Parameters.Add("maxpartysize", Convert.ToInt32(party_size));
                cmd3.Parameters.Add("exe_chef", exe_chef);
                cmd3.Parameters.Add("admin_email", admin_email);
                cmd3.Parameters.Add("admin_pass", admin_password);
                // cmd.Parameters.Add("pay", payment_method);
                int r = cmd3.ExecuteNonQuery();
                if (richTextBox1.Text != "")
                {
                    string[] arr = richTextBox1.Text.Split('\n');
                    foreach (string i in arr)
                    {
                        if (i == "")
                            continue;
                        OracleCommand cmda = new OracleCommand("insert into PAYMENTMETHOD values('" + name + "','" + i + "')", con);
                        cmda.CommandType = CommandType.Text;
                        cmda.ExecuteNonQuery();
                    }
                }
                    if (s == "Home")
                    {
                        past1.Show();
                        past1.Form1_Load(past1, e);
                    }
                    else if (s == "Restaurants")
                    {
                        past2.Show();
                        past2.all_restaurants_Load(past2, e);
                    }
                    else if (s == "Making Reservation")
                    {
                        past3.Show();
                        past3.making_reservation_Load(past3, e);
                    }
                    else if (s == "My_reservation")
                    {
                        past4.Show();
                        past4.my_reservations_Load(past4, e);
                    }
                    else if (s == "Restaurant_profile")
                    {
                        past5.Show();
                        past5.Restaurant_Profile_Load(past5, e);
                    }
                    this.Hide();
                }
            con.Close();
        }

        private void bunifuMetroTextbox11_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void register_Restaurant_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void register_Restaurant_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void register_Restaurant_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuMetroTextbox5_OnValueChanged(object sender, EventArgs e)
        {

        }
    }
}
