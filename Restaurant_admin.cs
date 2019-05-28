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
    public partial class Restaurant_admin : Form
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
        string source,resname;
        public Restaurant_admin()
        {

            InitializeComponent();
        }
        public Restaurant_admin(Form1 f,string s,string res_name)
        {
            InitializeComponent();
            past = f;
            source = s;
            resname = res_name;
            button6.Visible = true;
            button7.Visible = false;
            button8.Visible = false;
            button2.Visible = false;
            button12.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
        }
        public Restaurant_admin(all_restaurants f, string s, string res_name)
        {
            InitializeComponent();
            past1 = f;
            source = s;
            resname = res_name;
            button6.Visible = true;
            button7.Visible = false;
            button8.Visible = false;
            button2.Visible = false;
            button12.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
        }
        public Restaurant_admin(making_reservation f, string s, string res_name)
        {
            InitializeComponent();
            past2 = f;
            source = s;
            resname = res_name;
            button6.Visible = true;
            button7.Visible = false;
            button8.Visible = false;
            button2.Visible = false;
            button12.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
        }
        public Restaurant_admin(my_reservations f, string s, string res_name)
        {
            InitializeComponent();
            past3 = f;
            source = s;
            resname = res_name;
            button6.Visible = true;
            button7.Visible = false;
            button8.Visible = false;
            button2.Visible = false;
            button12.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
        }
        public Restaurant_admin(Restaurant_Profile f, string s, string res_name)
        {
            InitializeComponent();
            past4 = f;
            source = s;
            resname = res_name;
            button6.Visible = true;
            button7.Visible = false;
            button8.Visible = false;
            button2.Visible = false;
            button12.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
        }
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Restaurant_admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Restaurant_admin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        public void Restaurant_admin_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            if (button6.Visible == true)
            {
                flowLayoutPanel1.Controls.Add(new Restaurant_Profile_Update(this, resname));
            }
            else if (button7.Visible == true)
            {
                flowLayoutPanel1.Controls.Add(new Menu_Update(this, resname));
            }
            else if (button8.Visible == true)
            {
                flowLayoutPanel1.Controls.Add(new Schedule_Update(this, resname));
            }
            else if (button1.Visible == true)
            {
                flowLayoutPanel1.Controls.Add(new reservations_admin(this, resname));
            }
            else if (button12.Visible == true)
            {
                flowLayoutPanel1.Controls.Add(new Reviews_Report(this, resname));
            }
            else if (button13.Visible == true)
            {
                flowLayoutPanel1.Controls.Add(new signed_in_reservations_report(this, resname));
            }
            else if (button14.Visible == true)
            {
                flowLayoutPanel1.Controls.Add(new signed_out_reservations_report(this, resname));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button6.Visible = true;
            button7.Visible = false;
            button8.Visible = false;
            button2.Visible = false;
            button12.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Controls.Add(new Restaurant_Profile_Update(this,resname));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            button7.Visible = true;
            button8.Visible = false;
            button2.Visible = false;
            button12.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Controls.Add(new Menu_Update(this,resname));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = true;
            button2.Visible = false;
            button12.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Controls.Add(new Schedule_Update(this, resname));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            button2.Visible = true;
            button12.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Controls.Add(new reservations_admin(this, resname));
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Sign_up_button_Click(object sender, EventArgs e)
        {
            log_in.logged_in = false;
            if (source == "Home")
            {
                past.Show();
                past.Form1_Load(past, e);
            }
            else if (source == "Restaurants")
            {
                past1.Show();
                past1.all_restaurants_Load(past1, e);
            }
            else if (source == "Making Reservation")
            {
                past2.Show();
                past2.making_reservation_Load(past2, e);
            }
            else if (source == "My_reservation")
            {
                past3.Show();
                past3.my_reservations_Load(past3, e);
            }
            else if (source == "Restaurant_profile")
            {
                past4.Show();
                past4.Restaurant_Profile_Load(past4, e);
            }
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            button2.Visible = false;
            button12.Visible = true;
            button13.Visible = false;
            button14.Visible = false;
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Controls.Add(new Reviews_Report(this, resname));
        }

        private void button10_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            button2.Visible = false;
            button12.Visible = false;
            button13.Visible = true;
            button14.Visible = false;
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Controls.Add(new signed_in_reservations_report(this, resname));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            button2.Visible = false;
            button12.Visible = false;
            button13.Visible = false;
            button14.Visible = true;
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Controls.Add(new signed_out_reservations_report(this, resname));
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
