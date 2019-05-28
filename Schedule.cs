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
    public partial class Schedule : UserControl
    {
        Schedule_Update past;
        string resname, specific_date, specific_time;
        int number_of_tables,dining_points;

        string Connection = "Data Source=orcl;user id=hr;password=hr;";
        OracleConnection con;
        OracleCommand cmd;
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                dining_points = Convert.ToInt32(bunifuMetroTextbox2.Text);
                number_of_tables = Convert.ToInt32( bunifuDropdown1.selectedValue.Split(' ')[0]);
                if (number_of_tables >= 0 && dining_points >= 0)
                {
                    con = new OracleConnection(Connection);
                    con.Open();
                    cmd = new OracleCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "update schedule set numoftables=" + number_of_tables.ToString() + ", diningpoints=" + dining_points + " where resname = '" + resname + "' and specificdate = '" + specific_date + "' and specificdateandtime=to_date('" + specific_date + " " + specific_time + "','mm/dd/yyyy hh24:mi')";
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("the data is successfully updated");
                }
                else
                {
                    MessageBox.Show("please enter valid data");
                }
            }
            catch
            {
                MessageBox.Show("please enter valid data");
            }
            this.Schedule_Load(this, e);
        }

        public Schedule()
        {
            InitializeComponent();
        }
        public Schedule(Schedule_Update f,string res_name,string _specific_date,string _specific_time,int _number_of_tables,int _dining_points)
        {
            InitializeComponent();
            past = f;
            resname = res_name;
            specific_date = _specific_date;
            specific_time = _specific_time;
            number_of_tables = _number_of_tables;
            dining_points = _dining_points;
        }
        private void Schedule_Load(object sender, EventArgs e)
        {
            label1.Text = specific_time;
            bunifuDropdown1.selectedIndex = number_of_tables;
            bunifuMetroTextbox2.Text = dining_points.ToString();
            bunifuMetroTextbox2.BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
