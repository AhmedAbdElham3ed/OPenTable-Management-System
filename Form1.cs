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
    
    public partial class Form1 : Form
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
        Restaurant_Profile past3;
        my_reservations past4;
        string Connection = "Data Source=orcl;user id=hr;password=hr;";
        OracleConnection con;
        OracleCommand cmd;
        public Form1()
        {
            InitializeComponent();
            log_in.store();
            con = new OracleConnection(Connection);
            con.Open();
            cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = "delete from signedreservation where state='unconfirmed' and specificdateandtime<sysdate";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from signedoutreservation where state='unconfirmed' and specificdateandtime<sysdate";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }
        public Form1(Form1 a)
        {
            InitializeComponent();
            past = a;
        }
        public Form1(all_restaurants a)
        {
            InitializeComponent();
            past1 = a;
        }
        public Form1(making_reservation a)
        {
            InitializeComponent();
            past2 = a;
        }
        public Form1(Restaurant_Profile a)
        {
            InitializeComponent();
            past3 = a;
        }
        public Form1(my_reservations a)
        {
            InitializeComponent();
            past4 = a;
        }
        public void Form1_Load(object sender, EventArgs e)
        {
            /*
             Code here
             */
            
            con = new OracleConnection(Connection);
            con.Open();
            cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = "select max(maxparty_size) from restaurant";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            bunifuDropdown1.Clear();
            if (dr.Read())
            {
                try
                {
                    for (int i = 1; i <= Convert.ToInt32(dr[0]); i++)
                    {
                        bunifuDropdown1.AddItem(i.ToString() + " people");
                    }
                    bunifuDropdown1.selectedIndex = 0;
                }
                catch
                {

                }
            }
            this.Width = 1250;
            label1.Location =new Point((panel1.Width / 2)-(label1.Width/2),panel1.Location.Y+panel1.Height+15);
            bunifuDropdown2.selectedIndex = 0;
            bunifuDatepicker1.Value = DateTime.Now;
            flowLayoutPanel1.Controls.Clear();

            
            cmd.Connection = con;
            cmd.CommandText = "best_Restaurants";  
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("num_p", 1);
            cmd.Parameters.Add("mn_price", 1);
            cmd.Parameters.Add("mx_price", -1);
            cmd.Parameters.Add("res_cuisine", "no specifications");
            cmd.Parameters.Add("res_name","null");
            if (log_in.logged_in == true)
            {
                label2.Text = "Popular restaurants in " + log_in.Location;
                label3.Text = "hello, " + log_in.firstname;
                label3.Visible = true;
                bunifuImageButton3.Visible = true;
                Sign_in_button.Visible = false;
                Sign_up_button.Visible = false;
                panel3.Visible = false;
                cmd.Parameters.Add("res_location", log_in.Location);

            }
            else
            {
                label2.Text = "Popular restaurants in undefined" ;
                label3.Visible = false;
                bunifuImageButton3.Visible = false;
                Sign_in_button.Visible = true;
                Sign_up_button.Visible = true;
                panel3.Visible = false;
                cmd.Parameters.Add("res_location", "no specifications");

            }
            
            cmd.Parameters.Add("valid_res", OracleDbType.RefCursor, ParameterDirection.Output);
            // bind the date to reader
            dr = cmd.ExecuteReader();
            bool tell = false;
            while (dr.Read())
            {
                tell = true;
                string range_money="";
                //MessageBox.Show(dr[0].ToString());
                string res_name = dr[0].ToString();
                double average_rate = Convert.ToDouble(dr[1]);
                average_rate = (double)Math.Round((decimal)(average_rate), 2);
                int number_of_reviews = Convert.ToInt32(dr[2]);
                int number_of_signedreservation = 0;
                int number_of_unsignedreservation = 0;
                //MessageBox.Show(res_name);
                cmd = new OracleCommand();
                cmd.Connection = con;
                cmd.CommandText = "restaurant_data";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("res_name",res_name);
                cmd.Parameters.Add("res_cuisine",OracleDbType.Varchar2,50, OracleCollectionType.None, ParameterDirection.Output);
                cmd.Parameters.Add("mx_price", OracleDbType.Int32, ParameterDirection.Output);
                cmd.Parameters.Add("res_location", OracleDbType.Varchar2,50, OracleCollectionType.None, ParameterDirection.Output);
                cmd.ExecuteNonQuery();
                
                    if (Convert.ToInt32(cmd.Parameters["mx_price"].Value.ToString()) <= 30)
                        range_money = "$$";
                    else if (Convert.ToInt32(Convert.ToInt32(cmd.Parameters["mx_price"].Value.ToString())) <= 50)
                        range_money = "$$$";
                    else
                        range_money = "$$$$";
                    cmd.CommandText = "select count(resname) from signedreservation where resname='" + res_name + "' and specificdateandtime>=to_date('" + DateTime.Now.ToShortDateString() + " 00:00','mm/dd/yyyy hh24:mi') and specificdateandtime<=to_date('" + DateTime.Now.ToShortDateString() + " 23:59','mm/dd/yyyy hh24:mi') and( specificdateandtime>=sysdate or (specificdateandtime<sysdate and state='confirmed'))";
                cmd.CommandType = CommandType.Text;
                    OracleDataReader dr2 = cmd.ExecuteReader();
                    if (dr2.Read())
                        number_of_signedreservation = Convert.ToInt32(dr2[0]);
                    dr2.Close();
                    cmd.CommandText = "select count(resname) from signedoutreservation where resname='" + res_name + "' and specificdateandtime>=to_date('" + DateTime.Now.ToShortDateString() + " 00:00','mm/dd/yyyy hh24:mi') and specificdateandtime<=to_date('" + DateTime.Now.ToShortDateString() + " 23:59','mm/dd/yyyy hh24:mi') and( specificdateandtime>=sysdate or (specificdateandtime<sysdate and state='confirmed'))";
                cmd.CommandType = CommandType.Text;
                    dr2 = cmd.ExecuteReader();
                    if (dr2.Read())
                        number_of_unsignedreservation = Convert.ToInt32(dr2[0]);
                    flowLayoutPanel1.Controls.Add(new pop_restaurant(this, res_name, average_rate, number_of_reviews,cmd.Parameters["res_cuisine"].Value.ToString(), range_money, cmd.Parameters["res_location"].Value.ToString(), number_of_signedreservation+number_of_unsignedreservation ));
                
                //MessageBox.Show(res_name + " " + average_rate.ToString() + " " + number_of_reviews.ToString() +" "+(Convert.ToInt32( dr[3])+ Convert.ToInt32(dr[3])).ToString());
                //cmd.CommandText="select "
            }
            dr.Close();

            if (tell == false)
                MessageBox.Show("currently there is no available restaurant");
            con.Close();
        }

        private void Form1_MouseHover(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void bunifuDropdown1_onItemSelected(object sender, EventArgs e)
        {

        }

        private void bunifuDatepicker1_onValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            
        }

        private void Sign_in_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Sign_in_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Sign_up_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Sign_up_MouseLeave(object sender, EventArgs e)
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Form1_Load(this,e);           
        }


        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuImageButton2_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void bunifuImageButton2_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Sign_up_Click(object sender, EventArgs e)
        {
            Sign_up s = new Sign_up(this,"Home");
            s.Show();
            this.Hide();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void Sign_in_button_Click(object sender, EventArgs e)
        {
            Sign_in s = new Sign_in(this,"Home");
            s.Show();
            this.Hide();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            if (panel3.Visible == true)
                panel3.Visible = false;
            else if (panel3.Visible == false)
                panel3.Visible = true;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            My_Profile m_p = new My_Profile(this, "Home");
            m_p.Show();
            this.Hide();
        }

        private void button6_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button6_MouseLeave(object sender, EventArgs e)
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

        private void button5_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            log_in.logged_in = false;
            this.Form1_Load(this, e);
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(  bunifuDropdown2.selectedValue);
            int number_of_people=0;
            if (bunifuDropdown1.selectedIndex!=-1)
                number_of_people = Convert.ToInt32(bunifuDropdown1.selectedValue.Split(' ')[0]);
            all_restaurants a = new all_restaurants(this,"Home",bunifuDatepicker1.Value.ToShortDateString(),bunifuDropdown2.selectedValue,number_of_people);
            a.Show();
            this.Hide();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*
                CODE HERE
            */
            all_restaurants f = new all_restaurants(this,"Home");
            f.Show();
            this.Hide();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            my_reservations m_r = new my_reservations(this,"Home");
            m_r.Show();
            this.Hide();
        }

        private void bunifuDropdown2_onItemSelected(object sender, EventArgs e)
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            string query;
            if (bunifuMetroTextbox1.Text == "")
            {
                query = "select restaurant.resname,round(avg(nvl(rate,0)),2),count(raterestaurant.resname) from restaurant,raterestaurant where restaurant.resname = raterestaurant.resname(+) group by restaurant.resname order by sum(nvl(rate,0)) desc";
                label2.Text = "Popular restaurants in undefined";
            }
            else
            {
                query = "select restaurant.resname,round(avg(nvl(rate,0)),2),count(raterestaurant.resname) from restaurant,raterestaurant where restaurant.resname = raterestaurant.resname(+) and LOWER(restaurant.reslocation)=LOWER('" + bunifuMetroTextbox1.Text + "') group by restaurant.resname order by sum(nvl(rate,0)) desc";
                label2.Text = "Popular restaurants in " + bunifuMetroTextbox1.Text;
            }
                
                
                OracleDataAdapter adapter = new OracleDataAdapter(query, Connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                bool tell = false;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    tell = true;
                    string range_money = "";
                    //MessageBox.Show(dr[0].ToString());
                    string res_name = r[0].ToString();
                    double average_rate = Convert.ToDouble(r[1]);
                    average_rate = (double)Math.Round((decimal)(average_rate), 2);
                    int number_of_reviews = Convert.ToInt32(r[2]);
                    int number_of_signedreservation = 0;
                    int number_of_unsignedreservation = 0;
                    //MessageBox.Show(res_name);
                    con = new OracleConnection(Connection);
                    con.Open();
                    cmd = new OracleCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "restaurant_data";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("res_name", res_name);
                    cmd.Parameters.Add("res_cuisine", OracleDbType.Varchar2, 50, OracleCollectionType.None, ParameterDirection.Output);
                    cmd.Parameters.Add("mx_price", OracleDbType.Int32, ParameterDirection.Output);
                    cmd.Parameters.Add("res_location", OracleDbType.Varchar2, 50, OracleCollectionType.None, ParameterDirection.Output);
                    cmd.ExecuteNonQuery();

                    if (Convert.ToInt32(cmd.Parameters["mx_price"].Value.ToString()) <= 30)
                        range_money = "$$";
                    else if (Convert.ToInt32(Convert.ToInt32(cmd.Parameters["mx_price"].Value.ToString())) <= 50)
                        range_money = "$$$";
                    else
                        range_money = "$$$$";
                    cmd.CommandText = "select count(resname) from signedreservation where resname='" + res_name + "'";
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader dr2 = cmd.ExecuteReader();
                    if (dr2.Read())
                        number_of_signedreservation = Convert.ToInt32(dr2[0]);
                    dr2.Close();
                    cmd.CommandText = "select count(resname) from signedoutreservation where resname='" + res_name + "'";
                    cmd.CommandType = CommandType.Text;
                    dr2 = cmd.ExecuteReader();
                    if (dr2.Read())
                        number_of_unsignedreservation = Convert.ToInt32(dr2[0]);
                    flowLayoutPanel1.Controls.Add(new pop_restaurant(this, res_name, average_rate, number_of_reviews, cmd.Parameters["res_cuisine"].Value.ToString(), range_money, cmd.Parameters["res_location"].Value.ToString(), number_of_signedreservation + number_of_unsignedreservation));

                    //MessageBox.Show(res_name + " " + average_rate.ToString() + " " + number_of_reviews.ToString() +" "+(Convert.ToInt32( dr[3])+ Convert.ToInt32(dr[3])).ToString());
                    //cmd.CommandText="select "

                }

                    if (tell == false)
                        MessageBox.Show("we are sorry there is no resturants that fit your Preferences");
                
            
            // connect with frontend
            
        }
    }
}
