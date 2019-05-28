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
    public partial class all_restaurants : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        Form1 past;
        string s = "",sorting_choice="A-Z", date = DateTime.Now.ToShortDateString(), time = "no specifications", cusine = "no specifications", region = "no specifications";
        int mn_price=-1,mx_price=-1;
        int number_of_people=1;
        bool rangemoney1=false, rangemoney2=false, rangemoney3=false;
        string Connection = "Data Source=orcl;user id=hr;password=hr;";
        OracleConnection con;
        OracleCommand cmd;
        OracleDataReader dr;
        public all_restaurants()
        {
            InitializeComponent();
            
        }

        public all_restaurants(Form1 f,string source)
        {
            InitializeComponent();
            past = f;
            s = source;
            bunifuDropdown2.selectedIndex = 1;
        }
        public all_restaurants(Form1 f, string _source,string _date,string _time,int _number_of_people)
        {
            InitializeComponent();
            past = f;
            s = _source;
            date = _date;
            time = _time;
            number_of_people = _number_of_people;
            bunifuDropdown2.selectedIndex = log_in.available_time[time];
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1(this);
            f.Show();
            this.Hide();
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void all_restaurants_Load(object sender, EventArgs e)
        {
            panel9.Visible = false;
            button9.Text = sorting_choice;
            
            con = new OracleConnection(Connection);
            con.Open();
            cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = "select max(maxparty_size) from restaurant";
            cmd.CommandType = CommandType.Text;
            dr = cmd.ExecuteReader();
            bunifuDropdown1.Clear();
            if (dr.Read())
            {
                try
                {
                    for (int i = 1; i <= Convert.ToInt32(dr[0]); i++)
                    {
                        bunifuDropdown1.AddItem(i.ToString() + " people");
                    }
                    bunifuDropdown1.selectedIndex = number_of_people - 1;
                }
                catch
                {
                    
                }
            }
            dr.Close();
            con.Close();
            bunifuDatepicker1.Value = Convert.ToDateTime(date);
            
            flowLayoutPanel1.Controls.Clear();

            //MessageBox.Show(number_of_people.ToString() + " " + date + " " + time + " " + mn_price + " " + mx_price + " " + region + " " + cusine);
            if (log_in.logged_in == true)
            {
                label3.Text = "hello, " + log_in.firstname;
                label3.Visible = true;
                bunifuImageButton3.Visible = true;
                Sign_in_button.Visible = false;
                Sign_up_button.Visible = false;
                panel3.Visible = false;

            }
            else
            {
                label3.Visible = false;
                bunifuImageButton3.Visible = false;
                Sign_in_button.Visible = true;
                Sign_up_button.Visible = true;
                panel3.Visible = false;
               
            }
            if(sorting_choice=="A-Z")
            {
                show_restaurant_A_Z();
            }
            else
            {
                show_based_on_highest_rated();
            }

            
            /*
             
            */
        }
        private void show_restaurant_A_Z()
        {
            flowLayoutPanel1.Controls.Clear();
            bool tell = false, valid_restaurant = false;
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select resname,cusine,maxprice,reslocation from restaurant order by resname";
            cmd.CommandType = CommandType.Text;
           OracleDataReader dr = cmd.ExecuteReader();
            string resname, cuisine, reslocation,range_money;
            int maxprice;
            while (dr.Read())
            {
                valid_restaurant = false;
                resname = dr[0].ToString();
                OracleDataReader dr2;
                if (time != "no specifications")
                {
                    //MessageBox.Show(res_name + " " + date + " " + time);
                    cmd.CommandText = "select count(*) from schedule where resname='" + resname + "' and specificdate='" + date + "' and specificdateandtime= to_date('" + date + " " + time + "','mm/dd/yyyy hh24:mi')  and  to_date('" + date + " " + time + "','mm/dd/yyyy hh24:mi')>sysdate and numoftables>0";
                    cmd.CommandType = CommandType.Text;
                    dr2 = cmd.ExecuteReader();
                    if (dr2.Read() && Convert.ToInt32(dr2[0]) > 0)
                    {
                        valid_restaurant = true;
                        tell = true; 
                    }
                    dr2.Close();
                }
                else
                {
                    cmd.CommandText = "select count(*) from schedule where resname='" + resname + "' and specificdate='" + date + "' and specificdateandtime>sysdate and numoftables>0";
                    cmd.CommandType = CommandType.Text;
                    dr2 = cmd.ExecuteReader();
                    if (dr2.Read() && Convert.ToInt32(dr2[0]) > 0)
                    {
                        valid_restaurant = true;
                        tell = true;
                    }
                    dr2.Close();
                }
                if (valid_restaurant == true)
                {
                    cuisine = dr[1].ToString();
                    maxprice = Convert.ToInt32(dr[2]);
                    reslocation = dr[3].ToString();

                    if (maxprice <= 30)
                        range_money = "$$";
                    else if (maxprice <= 50)
                        range_money = "$$$";
                    else
                        range_money = "$$$$";
                    cmd = new OracleCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "best_Restaurants";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("num_p", number_of_people);
                    cmd.Parameters.Add("mn_price", mn_price);
                    cmd.Parameters.Add("mx_price", mx_price);
                    cmd.Parameters.Add("res_cuisine", cusine);
                    cmd.Parameters.Add("res_name", resname);
                    cmd.Parameters.Add("res_location", region);
                    cmd.Parameters.Add("valid_res", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleDataReader dr1 = cmd.ExecuteReader();
                    while (dr1.Read())
                    {
                        //MessageBox.Show(dr[0].ToString());
                        double average_rate = Convert.ToDouble(dr1[1]);
                        average_rate = (double)Math.Round((decimal)(average_rate), 2);
                        int number_of_reviews = Convert.ToInt32(dr1[2]);
                        int number_of_signedreservation = 0;
                        int number_of_unsignedreservation = 0;
                        cmd.CommandText = "select count(resname) from signedreservation where resname='" + resname + "' and specificdateandtime>=to_date('" + DateTime.Now.ToShortDateString() + " 00:00','mm/dd/yyyy hh24:mi') and specificdateandtime<=to_date('" + DateTime.Now.ToShortDateString() + " 23:59','mm/dd/yyyy hh24:mi') and( (specificdateandtime>=sysdate) or (specificdateandtime<sysdate and state='confirmed'))";
                        cmd.CommandType = CommandType.Text;
                        dr2 = cmd.ExecuteReader();
                        if (dr2.Read())
                            number_of_signedreservation = Convert.ToInt32(dr2[0]);
                        dr2.Close();
                        cmd.CommandText = "select count(resname) from signedoutreservation where resname='" + resname + "' and specificdateandtime>=to_date('" + DateTime.Now.ToShortDateString() + " 00:00','mm/dd/yyyy hh24:mi') and specificdateandtime<=to_date('" + DateTime.Now.ToShortDateString() + " 23:59','mm/dd/yyyy hh24:mi') and( (specificdateandtime>=sysdate) or (specificdateandtime<sysdate and state='confirmed'))";

                        cmd.CommandType = CommandType.Text;
                        dr2 = cmd.ExecuteReader();
                        if (dr2.Read())
                            number_of_unsignedreservation = Convert.ToInt32(dr2[0]);
                        dr2.Close();

                        if (time != "no specifications")
                        {
                            flowLayoutPanel1.Controls.Add(new Restaurant(this, resname, average_rate, number_of_reviews, cuisine, range_money, reslocation, number_of_signedreservation + number_of_unsignedreservation, date + " " + time, number_of_people, "specified search"));
                        }
                        else
                        {
                            flowLayoutPanel1.Controls.Add(new Restaurant(this, resname, average_rate, number_of_reviews, cuisine, range_money, reslocation, number_of_signedreservation + number_of_unsignedreservation, date + " " + time, number_of_people, "unspecified search"));
                        }
                     }
                    dr1.Close();

                }
            }
            if (tell == false)
                MessageBox.Show("we are sorry there is no resturants that fit your Preferences");
            dr.Close();
             con.Close();
        }
        private void show_based_on_highest_rated()
        {
            flowLayoutPanel1.Controls.Clear();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "best_Restaurants";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("num_p", number_of_people);
            cmd.Parameters.Add("mn_price", mn_price);
            cmd.Parameters.Add("mx_price", mx_price);
            cmd.Parameters.Add("res_cuisine", cusine);
            cmd.Parameters.Add("res_name", "null");
            cmd.Parameters.Add("res_location", region);
            cmd.Parameters.Add("valid_res", OracleDbType.RefCursor, ParameterDirection.Output);

            
            dr = cmd.ExecuteReader();
            bool tell = false, valid_restaurant = false;
            while (dr.Read())
            {
                valid_restaurant = false;

                string range_money = "";
                string res_name = dr[0].ToString();

                OracleDataReader dr2;
                if (time != "no specifications")
                {
                    cmd.CommandText = "select count(*) from schedule where resname='" + res_name + "' and specificdate='" + date + "' and specificdateandtime=to_date('" + date + " " + time + "','mm/dd/yyyy hh24:mi') and to_date('" + date + " " + time + "','mm/dd/yyyy hh24:mi')>sysdate and numoftables>0";
                    cmd.CommandType = CommandType.Text;
                    dr2 = cmd.ExecuteReader();
                    if (dr2.Read() && Convert.ToInt32(dr2[0]) > 0)
                    {
                        tell = true;
                        valid_restaurant = true;
                    }
                    dr2.Close();
                }
                else
                {
                    cmd.CommandText = "select count(*) from schedule where resname='" + res_name + "' and specificdate='" + date + "' and specificdateandtime>sysdate and numoftables>0";
                    cmd.CommandType = CommandType.Text;
                    dr2 = cmd.ExecuteReader();
                    if (dr2.Read() && Convert.ToInt32(dr2[0]) > 0)
                    {
                        tell = true;
                        valid_restaurant = true;
                    }
                    dr2.Close();
                }
                if (valid_restaurant == true)
                {
                    double average_rate = Convert.ToDouble(dr[1]);
                    average_rate = (double)Math.Round((decimal)(average_rate), 2);
                    int number_of_reviews = Convert.ToInt32(dr[2]);
                    int number_of_signedreservation = 0;
                    int number_of_unsignedreservation = 0;
                    cmd.CommandText = "select cusine,maxprice,reslocation from restaurant where resname= '" + res_name + "'";
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader dr1 = cmd.ExecuteReader();
                    while (dr1.Read())
                    {
                        if (Convert.ToInt32(dr1[1]) <= 30)
                            range_money = "$$";
                        else if (Convert.ToInt32(dr1[1]) <= 50)
                            range_money = "$$$";
                        else
                            range_money = "$$$$";
                        cmd.CommandText = "select count(resname) from signedreservation where resname='" + res_name + "' and specificdateandtime>=to_date('" + DateTime.Now.ToShortDateString() + " 00:00','mm/dd/yyyy hh24:mi') and specificdateandtime<=to_date('" + DateTime.Now.ToShortDateString() + " 23:59','mm/dd/yyyy hh24:mi') and( (specificdateandtime>=sysdate) or (specificdateandtime<sysdate and state='confirmed'))";
                        cmd.CommandType = CommandType.Text;
                        dr2 = cmd.ExecuteReader();
                        if (dr2.Read())
                            number_of_signedreservation = Convert.ToInt32(dr2[0]);
                        dr2.Close();
                        cmd.CommandText = "select count(resname) from signedoutreservation where resname='" + res_name + "' and specificdateandtime>=to_date('" + DateTime.Now.ToShortDateString() + " 00:00','mm/dd/yyyy hh24:mi') and specificdateandtime<=to_date('" + DateTime.Now.ToShortDateString() + " 23:59','mm/dd/yyyy hh24:mi') and( (specificdateandtime>=sysdate) or (specificdateandtime<sysdate and state='confirmed'))";

                        cmd.CommandType = CommandType.Text;
                        dr2 = cmd.ExecuteReader();
                        if (dr2.Read())
                            number_of_unsignedreservation = Convert.ToInt32(dr2[0]);
                        dr2.Close();
                        if (time != "no specifications")
                        {
                            flowLayoutPanel1.Controls.Add(new Restaurant(this, res_name, average_rate, number_of_reviews, dr1[0].ToString(), range_money, dr1[2].ToString(), number_of_signedreservation + number_of_unsignedreservation, date + " " + time, number_of_people, "specified search"));
                        }
                        else
                        {
                            flowLayoutPanel1.Controls.Add(new Restaurant(this, res_name, average_rate, number_of_reviews, dr1[0].ToString(), range_money, dr1[2].ToString(), number_of_signedreservation + number_of_unsignedreservation, date + " " + time, number_of_people, "unspecified search"));
                        }
                    }
                    dr1.Close();
                }
            }

            dr.Close();

            if (tell == false)
                MessageBox.Show("we are sorry there is no resturants that fit your Preferences");
            con.Close();
        }
        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            if (panel3.Visible == true)
                panel3.Visible = false;
            else if (panel3.Visible == false)
                panel3.Visible = true;
        }

        private void Sign_in_button_Click(object sender, EventArgs e)
        {
            Sign_in s = new Sign_in(this, "Restaurants");
            s.Show();
            this.Hide();
        }

        private void Sign_up_button_Click(object sender, EventArgs e)
        {
            Sign_up s = new Sign_up(this, "Restaurants");
            s.Show();
            this.Hide();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if (s == "Home")
            {
                past.Show();
                past.Form1_Load(past, e);
                this.Hide();
            }
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            label2.Visible = true;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
            label2.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rangemoney1 = rangemoney3 = false;
            if (rangemoney2 == false)
            {
                mn_price = 31;
                mx_price = 50;
                rangemoney2 = true;
            }
            else
            {
                mn_price = -1;
                mx_price = -1;
                rangemoney2 = false;
            }
            this.all_restaurants_Load(this, e);
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            label4.Visible = true;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
            label4.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            rangemoney1 = rangemoney2 = false;
            if (rangemoney3 == false)
            {
                mn_price = 51;
                mx_price = -1;
                rangemoney3 = true;
            }
            else
            {
                mn_price = -1;
                mx_price = -1;
                rangemoney3 = false;
            }
            this.all_restaurants_Load(this, e);
        }

        private void button8_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            label5.Visible = true;
        }

        private void button8_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
            label5.Visible = false;
        }

        private void all_restaurants_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void all_restaurants_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
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

        private void button6_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button6_MouseLeave(object sender, EventArgs e)
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

        private void Sign_in_button_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Sign_in_button_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
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

        private void button7_Click(object sender, EventArgs e)
        {
            log_in.logged_in = false;
            this.all_restaurants_Load(this, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            My_Profile m_p = new My_Profile(this, "Home");
            m_p.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*
                 Code here
                 */
            date = bunifuDatepicker1.Value.ToShortDateString();
            time= bunifuDropdown2.selectedValue;
            try
            {
                number_of_people = Convert.ToInt32(bunifuDropdown1.selectedValue.Split(' ')[0]);
            }
            catch
            {
                
            }
            this.all_restaurants_Load(this,e);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            my_reservations m_r = new my_reservations(this, "Restaurants");
            m_r.Show();
            this.Hide();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            rangemoney2 =rangemoney3= false;
            if (rangemoney1 == false)
            {
                mn_price = 1;
                mx_price = 30;
                rangemoney1 = true;
            }
            else
            {
                mn_price = -1;
                mx_price = -1;
                rangemoney1 = false;
            }
            this.all_restaurants_Load(this, e);
        }

        private void bunifuDropdown3_onItemSelected(object sender, EventArgs e)
        {
            cusine = bunifuDropdown3.selectedValue;
            this.all_restaurants_Load(this, e);
        }

        private void bunifuDropdown1_onItemSelected(object sender, EventArgs e)
        {
            
        }

        private void bunifuDropdown2_onItemSelected(object sender, EventArgs e)
        {
           
        }

        private void bunifuDatepicker1_onValueChanged(object sender, EventArgs e)
        {
            if(bunifuDatepicker1.Value<DateTime.Now)
            {
                bunifuDatepicker1.Value = DateTime.Now;
            }
            date = bunifuDatepicker1.Value.ToShortDateString();
        }

        private void bunifuImageButton7_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void bunifuImageButton7_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void bunifuImageButton7_MouseClick(object sender, MouseEventArgs e)
        {
           /* if (panel9.Visible == false)
                panel9.Visible = true;
            else
                panel9.Visible = false;*/
        }

        private void button12_Click(object sender, EventArgs e)
        {
            sorting_choice = "A-Z";
            this.all_restaurants_Load(this,e);
        }

        private void bunifuImageButton7_Click(object sender, EventArgs e)
        {
            if (panel9.Visible == false)
                panel9.Visible = true;
            else if (panel9.Visible == true)
                panel9.Visible = false;
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            sorting_choice = "A-Z";
            panel9.Visible = false;
            this.all_restaurants_Load(this, e);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            sorting_choice = "Highest Rated";
            panel9.Visible = false;
            this.all_restaurants_Load(this, e);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            sorting_choice = "Highest Rated";
            this.all_restaurants_Load(this, e);
        }

        private void bunifuImageButton1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void bunifuDropdown4_onItemSelected(object sender, EventArgs e)
        {
            
            region = bunifuDropdown4.selectedValue;
            this.all_restaurants_Load(this, e);
        }

        private void button2_MouseHover(object sender, EventArgs e)
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

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
