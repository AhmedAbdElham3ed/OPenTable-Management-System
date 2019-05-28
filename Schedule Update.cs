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
    public partial class Schedule_Update : UserControl
    {
        Restaurant_admin past;
        string resname;

        string Connection = "Data Source=orcl;user id=hr;password=hr;";
        OracleConnection con;
        OracleCommand cmd;
        OracleDataReader dr,dr1;
        public Schedule_Update()
        {
            InitializeComponent();
        }
        public Schedule_Update(Restaurant_admin r,string res_name)
        {
            InitializeComponent();
            past = r;
            resname = res_name;
        }
        private void Schedule_Update_Load(object sender, EventArgs e)
        {
            bunifuDatepicker1.Value = DateTime.Now;
            bunifuDropdown1.Clear();
            con = new OracleConnection(Connection);
            con.Open();
            cmd = new OracleCommand();
            cmd.Connection = con;
            //MessageBox.Show(resname);
            cmd.CommandText = "select specificdate from schedule where resname='" + resname + "' and specificdateandtime>=sysdate group by specificdate";
            cmd.CommandType = CommandType.Text;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
              //  MessageBox.Show(dr[0].ToString());
                bunifuDropdown1.AddItem(dr[0].ToString());   
            }
            dr.Close();
            con.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            con = new OracleConnection(Connection);
            con.Open();
            cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = "select count(*) from schedule where resname='" + resname + "' and specificdate='" + bunifuDatepicker1.Value.ToShortDateString() + "'";
            cmd.CommandType = CommandType.Text;
            dr = cmd.ExecuteReader();
            if (dr.Read() && Convert.ToInt32(dr[0]) > 0) 
            { 
                MessageBox.Show("this date already exists in the schedule");
                 dr.Close();
            }
            else
            {
                foreach (string i in log_in.available_time.Keys)
                {
                    if (i == "no specifications")
                        continue;
                    cmd.CommandText = "insert into schedule values('" + resname + "','" + bunifuDatepicker1.Value.ToShortDateString() + "',to_date('" + bunifuDatepicker1.Value.ToShortDateString()+" "+ i + "','mm/dd/yyyy hh24:mi'),0,0)";
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                bunifuDropdown1.AddItem(bunifuDatepicker1.Value.ToShortDateString());
            }
            con.Close();
        }

        private void bunifuDropdown1_onItemSelected(object sender, EventArgs e)
        {
            bool check1, check2;
            flowLayoutPanel1.Controls.Clear();
            con = new OracleConnection(Connection);
            con.Open();
            cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = "select to_char( specificdateandtime,'mm/dd/yyyy hh24:mi'),numoftables,diningpoints from schedule where resname='" + resname + "' and specificdate='" + bunifuDropdown1.selectedValue + "' and specificdateandtime>=sysdate order by specificdateandtime";
            cmd.CommandType = CommandType.Text;
            dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                 check1 =check2= false;
                cmd.CommandText = "select count(*) from signedreservation where resname='"+resname+"' and specificdateandtime= to_date('"+dr[0].ToString()+"','mm/dd/yyyy hh24:mi')";
                cmd.CommandType = CommandType.Text;
                dr1 = cmd.ExecuteReader();
                if (dr1.Read() && Convert.ToInt32(dr1[0])>0)
                {
                    check1 = true;
                }
                dr1.Close();
                cmd.CommandText = "select count(*) from signedoutreservation where resname='" + resname + "' and specificdateandtime= to_date('" + dr[0].ToString() + "','mm/dd/yyyy hh24:mi')";
                cmd.CommandType = CommandType.Text;
                dr1 = cmd.ExecuteReader();
                if (dr1.Read() && Convert.ToInt32(dr1[0]) > 0)
                {
                    check2 = true;
                }
                dr1.Close();
                if (check1 == false && check2 == false)
                flowLayoutPanel1.Controls.Add(new Schedule(this,resname,bunifuDropdown1.selectedValue,dr[0].ToString().Split(' ')[1],Convert.ToInt32(dr[1]),Convert.ToInt32(dr[2])));
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuDatepicker1_onValueChanged(object sender, EventArgs e)
        {
            if(bunifuDatepicker1.Value.Year<DateTime.Now.Year||(bunifuDatepicker1.Value.Year == DateTime.Now.Year&& bunifuDatepicker1.Value.Month < DateTime.Now.Month)||(bunifuDatepicker1.Value.Month == DateTime.Now.Month&& bunifuDatepicker1.Value.Day < DateTime.Now.Day))
            {
                MessageBox.Show("please choose a valid date");
                bunifuDatepicker1.Value = DateTime.Now;
            }
        }
    }
}
