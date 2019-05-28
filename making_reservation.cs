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
    public partial class making_reservation : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        string s;
        string res_name, date, time;
        int dining_points;
        int number_of_people;
        all_restaurants past;
        Restaurant_Profile past1;


        string Connection = "Data Source=orcl;user id=hr;password=hr;";
        OracleConnection con;
        OracleCommand cmd;
        public making_reservation()
        {
            InitializeComponent();
        }
        public making_reservation(string source,all_restaurants a_r,string _res_name,string _date,string _time,int _number_of_people,int _dining_points)
        {
            InitializeComponent();
            past = a_r;
            res_name = _res_name;
            date = _date;
            time = _time;
            number_of_people=_number_of_people;
            dining_points = _dining_points;
            s = source;
        }
        public making_reservation(string source, Restaurant_Profile r, string _res_name, string _date, string _time, int _number_of_people, int _dining_points)
        {
            InitializeComponent();
            past1 = r;
            res_name = _res_name;
            date = _date;
            time = _time;
            number_of_people = _number_of_people;
            dining_points = _dining_points;
            s = source;
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

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            log_in.logged_in = false;
            this.making_reservation_Load(this, e);
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            if (panel4.Visible == true)
                panel4.Visible = false;
            else if (panel4.Visible == false)
                panel4.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            My_Profile m_p = new My_Profile(this, "Making Reservation");
            m_p.Show();
            this.Hide();
        }

        private void Sign_in_button_Click(object sender, EventArgs e)
        {
            Sign_in s = new Sign_in(this, "Making Reservation");
            s.Show();
            this.Hide();
        }

        private void Sign_up_button_Click(object sender, EventArgs e)
        {
            Sign_up s = new Sign_up(this, "Making Reservation");
            s.Show();
            this.Hide();
        }

        private void bunifuImageButton6_Click(object sender, EventArgs e)
        {
            
            if (s == "Restaurants")
            {
                past.Show();
                past.all_restaurants_Load(past, e);
                this.Hide();
            }
            if (s == "Restaurant_profile")
            {
                past1.Show();
                past1.Restaurant_Profile_Load(past1, e);
                this.Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1(this);
            f.Show();
            this.Hide();
        }

        private void making_reservation_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void making_reservation_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if(log_in.logged_in)
            {
                
            }
            if (((bunifuMetroTextbox4.Text=="" || bunifuMetroTextbox3.Text=="")&&(log_in.logged_in==false)) || (bunifuMetroTextbox1.Text == "" || bunifuMetroTextbox5.Text == ""||bunifuDropdown2.selectedIndex==-1))
            {
                MessageBox.Show("please complete your data");
            }
            else
            {
                try
                {
                    long number = Convert.ToInt64(bunifuMetroTextbox1.Text);
                    bool check = true;
                    con = new OracleConnection(Connection);
                    con.Open();
                    cmd = new OracleCommand();
                    cmd.Connection = con;
                    if (log_in.logged_in == false)
                    {
                        cmd.CommandText = "select count(*) from useraccount where useremail ='" + bunifuMetroTextbox5.Text + "'";
                        cmd.CommandType = CommandType.Text;
                        OracleDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            if (Convert.ToInt32(dr[0]) > 0)
                            {
                                check = false;
                                dr.Close();
                                break;
                            }
                            else
                            {
                                dr.Close();
                                cmd.CommandText = "select count(*) from admins where email ='" + bunifuMetroTextbox5.Text + "'";
                                cmd.CommandType = CommandType.Text;
                                dr = cmd.ExecuteReader();
                                while (dr.Read())
                                {
                                    if (Convert.ToInt32(dr[0]) > 0)
                                    {
                                        check = false;
                                        dr.Close();
                                        break;
                                    }
                                }
                            }

                        }
                    }
                    if (check == true)
                    {
                        if (log_in.logged_in)  // user already have email and signed in 
                            cmd.CommandText = "GET_MAX_RESV_ID_SIGNED";
                        else
                            cmd.CommandText = "GET_MAX_RESV_ID_UN_SIGNED";


                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("id", OracleDbType.Int32, ParameterDirection.Output);
                        cmd.ExecuteNonQuery();
                        int id;
                        try
                        {
                            id = Convert.ToInt32(cmd.Parameters["id"].Value.ToString());  // max current 
                            id++; // next one to be used 
                        }
                        catch
                        {
                            id = 1; // first element ever 
                        }

                        // new quary 
                        OracleCommand cmd1 = new OracleCommand();
                        cmd1.Connection = con;
                        //cmd2.CommandText= "insert into signedreservation values(1, 'ahmedaraby', 'aviato', '16-NOV-12', '01068482084', 'birthdate', 45, 33, NULL)";


                        if (log_in.logged_in)  // user already have email and signed in 
                            cmd1.CommandText = "insert into signedreservation values (:id , :email , :res_name ,to_date( :occ_date,'mm/dd/yyyy hh24:mi') , :phone , :occ , :num_p , :diningpointsoption , 'unconfirmed')";
                        else   // un registred user 
                            cmd1.CommandText = "insert into SIGNEDOUTRESERVATION values (:id , :email , :fname , :lname , :res_name , to_date( :occ_date,'mm/dd/yyyy hh24:mi'), :phone , :occ , :num_p , 'unconfirmed')";


                        cmd1.CommandType = CommandType.Text;
                        //MessageBox.Show(id.ToString()+" "+ bunifuMetroTextbox5.Text+" "+ bunifuMetroTextbox4.Text+" " + bunifuMetroTextbox3.Text+" "+ res_name+" "+ date + " " + time+" "+ bunifuMetroTextbox1.Text+ " "+ bunifuDropdown2.selectedValue+ " "+ number_of_people.ToString()+" "+ Convert.ToInt32(checkBox1.Checked).ToString());
                        // parameters   , have to be in the same order 
                        cmd1.Parameters.Add("id", id.ToString());
                        cmd1.Parameters.Add("email", bunifuMetroTextbox5.Text);

                        if (!log_in.logged_in)  // un registred user 
                        {
                            cmd1.Parameters.Add("fname", bunifuMetroTextbox4.Text);
                            cmd1.Parameters.Add("lname", bunifuMetroTextbox3.Text);
                        }

                        cmd1.Parameters.Add("res_name", res_name);
                        cmd1.Parameters.Add("occ_date", date + " " + time);
                        cmd1.Parameters.Add("phone", bunifuMetroTextbox1.Text);
                        cmd1.Parameters.Add("occ", bunifuDropdown2.selectedValue);
                        cmd1.Parameters.Add("num_p", number_of_people.ToString());
                        if (log_in.logged_in)  // registred user 
                            cmd1.Parameters.Add("diningpointsoption", Convert.ToInt32(checkBox1.Checked).ToString());
                        //last parameter in not binded 
                        // end 
                        int num_of_affected_rows = cmd1.ExecuteNonQuery();

                        if (num_of_affected_rows == 1)
                        {
                            //MessageBox.Show( res_name + " " + date + " " + time);
                            //MessageBox.Show(res_name + " " + label4.Text + " " + date + " " + time);
                            cmd.CommandText = "update SCHEDULE set NUMOFTABLES = NUMOFTABLES - 1 where resname ='" + res_name + "' and specificdate = '" + date + "' and specificdateandtime= to_date('" + date + " " + time + "','mm/dd/yyyy hh24:mi')";
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                            Form1 f;
                            if (s == "Restaurants")
                            {
                                f = new Form1(past);
                            }
                            else
                            {
                                f = new Form1(past1);
                            }
                            f.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("sorry the reservation cannot be completed");
                        }
                    }
                    else
                        MessageBox.Show("please enter a valid email");
                }
                catch
                {
                    MessageBox.Show("please enter a valid phone number");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            my_reservations m_r = new my_reservations(this, "Making Reservation");
            m_r.Show();
            this.Hide();
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {

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

        private void button2_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        public void making_reservation_Load(object sender, EventArgs e)
        {
            /*
                 Code here
                 */
            bunifuMetroTextbox4.Text = bunifuMetroTextbox3.Text = bunifuMetroTextbox1.Text = bunifuMetroTextbox5.Text = "";
            checkBox1.Checked = false;
            panel4.Visible = false;
            if (log_in.logged_in == true)
            {
                label3.Text = "hello, " + log_in.firstname;
                label3.Visible = true;
                bunifuImageButton3.Visible = true;
                Sign_in_button.Visible = false;
                Sign_up_button.Visible = false;
                button2.Location = new Point(button2.Location.X, checkBox1.Location.Y + 20);
                
                panel5.Location = new Point (panel5.Location.X,label10.Location.Y);
                this.Height=panel5.Location.Y+panel5.Height;
                bunifuMetroTextbox5.Enabled = false;
                bunifuMetroTextbox5.Text = log_in.User_Email;
                checkBox1.Text = " Yes i want to get " + dining_points.ToString() + " points from this reservation";
            }
            else
            {
                label3.Visible = false;
                bunifuImageButton3.Visible = false;
                Sign_in_button.Visible = true;
                Sign_up_button.Visible = true;
                button2.Location = new Point(button2.Location.X, checkBox1.Location.Y);
                panel5.Location = new Point(panel5.Location.X, label10.Location.Y+70);
                this.Height = panel5.Location.Y+panel5.Height;
                bunifuMetroTextbox5.Enabled = true;
            }
            label1.Text = res_name;
            label4.Text = date;
            label2.Text = time;
            label5.Text = number_of_people.ToString()+" People";

        }
    }
}
