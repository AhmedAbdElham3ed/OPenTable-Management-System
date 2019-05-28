using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
namespace OpenTable
{
    public partial class unconfirmed_reservation : UserControl
    {
        my_reservations past;
        int res_id, number_of_people,dining_points;
        string res_name, date, time, first_name, last_name, phone, email, occasion;
        bool Dining_points_option;
        string Connection = "Data Source=orcl;user id=hr;password=hr;";
        OracleConnection con;
        OracleCommand cmd;
        private void bunifuDropdown1_onItemSelected(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
             Code here
             */
            con = new OracleConnection(Connection);
            con.Open();
            cmd = new OracleCommand();
            cmd.Connection = con;
            //MessageBox.Show(res_name + " " + bunifuDatepicker1.Value.ToShortDateString() + " " + bunifuDatepicker1.Value.ToShortDateString() + " " + bunifuDropdown1.selectedValue);
            cmd.CommandText = "select count(*) from schedule where resname='"+res_name+"' and specificdate='"+bunifuDatepicker1.Value.ToShortDateString()+ "' and specificdateandtime=to_date('" + bunifuDatepicker1.Value.ToShortDateString() + " " + bunifuDropdown1.selectedValue + "','mm/dd/yyyy hh24:mi') and specificdateandtime>sysdate and numoftables>0";
            cmd.CommandType = CommandType.Text;
            OracleDataReader rd;
            rd = cmd.ExecuteReader();
            if (rd.Read() && Convert.ToInt32(rd[0]) > 0)
            {
                cmd.CommandText = "update SCHEDULE set NUMOFTABLES = NUMOFTABLES + 1 where resname ='" + res_name + "' and specificdate = '" + bunifuDatepicker2.Value.ToShortDateString() + "' and specificdateandtime=to_date('" + date + " " + time + "','mm/dd/yyyy hh24:mi')";
                cmd.ExecuteNonQuery();


                date = bunifuDatepicker1.Value.ToShortDateString();
                time = bunifuDropdown1.selectedValue;
                phone = bunifuMetroTextbox1.Text;
                occasion = bunifuDropdown2.selectedValue;
                number_of_people = Convert.ToInt32(bunifuDropdown3.selectedValue.Split(' ')[0]);
                Dining_points_option = checkBox1.Checked;
                cmd.CommandText = "update SCHEDULE set NUMOFTABLES = NUMOFTABLES - 1 where resname ='" + res_name + "' and specificdate = '" + bunifuDatepicker1.Value.ToShortDateString() + "' and specificdateandtime=to_date('" + date + " " + time + "','mm/dd/yyyy hh24:mi')";
                cmd.ExecuteNonQuery();

                MessageBox.Show(date+" "+time+" "+phone+" "+occasion+" "+number_of_people.ToString()+" "+Convert.ToInt32(Dining_points_option).ToString()+" "+res_id.ToString() );
                cmd.CommandText = "update signedreservation set specificdateandtime=to_date( :occ_date,'mm/dd/yyyy hh24:mi') , phone =:phone , occasion =:occ , numberofpeople=:num_p , diningpointsoption=:diningpointsoption, where reservationid= :res_id";
                cmd.Parameters.Add("occ_date", date + " "+ time);
                cmd.Parameters.Add("phone", phone);
                cmd.Parameters.Add("occasion", occasion);
                cmd.Parameters.Add("num_p", number_of_people.ToString());
                cmd.Parameters.Add("diningpointsoption",Convert.ToInt32(Dining_points_option).ToString());
                cmd.Parameters.Add("id", res_id.ToString());
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
               
                
                this.unconfirmed_reservation_Load(this,e);
            }
            else
            {
                MessageBox.Show("there is no available table with the specified date and time");
            }
            rd.Close();
            con.Close();
            //MessageBox.Show(bunifuDatepicker1.Value.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(res_id.ToString() + " " + res_name + " " + date + " " + time);
            con = new OracleConnection(Connection);
            con.Open();
            cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = "cancel_reservation";
            cmd.CommandType = CommandType.StoredProcedure;
            //MessageBox.Show(res_id.ToString() + " " + res_name.ToString() + " " + date + " " + time);
            cmd.Parameters.Add("res_id",res_id);
            cmd.Parameters.Add("res_name", res_name);
            cmd.Parameters.Add("specific_date", date);
            cmd.Parameters.Add("specific_time", time);
            cmd.ExecuteNonQuery();
            past.my_reservations_Load(past,e);

        }


        public unconfirmed_reservation()
        {
            InitializeComponent();
        }
        public unconfirmed_reservation(my_reservations f,int _res_id,string _res_name,string _date,string _time,int _number_of_people,string _phone,string _occasion,int _dining_points,bool _Dining_points_option)
        {
            InitializeComponent();
            res_name = _res_name;
            date = _date;
            time = _time;
            number_of_people = _number_of_people;
            first_name = log_in.firstname;
            last_name = log_in.lastname;
            phone = _phone;
            email = log_in.User_Email;
            occasion = _occasion;
            Dining_points_option = _Dining_points_option;
            res_id = _res_id;
            dining_points = _dining_points;
            past = f;
        }
        private void unconfirmed_reservation_Load(object sender, EventArgs e)
        {
            
            con = new OracleConnection(Connection);
            con.Open();
            cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = "select maxparty_size from restaurant where resname='" + res_name + "'";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            bunifuDropdown3.Clear();
            if (dr.Read())
            {
                for (int i = 1; i <= Convert.ToInt32(dr[0]); i++)
                {
                    bunifuDropdown3.AddItem(i.ToString() + " people");
                }
            }
            bunifuDatepicker2.Value = Convert.ToDateTime(date);
            label1.Text = res_name;
            bunifuDatepicker1.Value = Convert.ToDateTime(date);
            date = bunifuDatepicker1.Value.ToShortDateString();
            bunifuDropdown3.selectedIndex = number_of_people - 1;
            bunifuDropdown1.selectedIndex = log_in.available_time[time]-1;
            bunifuMetroTextbox4.Text = first_name;
            bunifuMetroTextbox3.Text = last_name;
            bunifuMetroTextbox4.Enabled = false;
            bunifuMetroTextbox3.Enabled = false;
            bunifuMetroTextbox1.Text = phone;
            bunifuMetroTextbox5.Text = email;
            bunifuDropdown2.selectedIndex = log_in.occasions[occasion];
            checkBox1.Text = "Yes i want to get " + dining_points.ToString() + " points from this reservation";
            checkBox1.Checked = Dining_points_option;
        }
    }
}
