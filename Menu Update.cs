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
    public partial class Menu_Update : UserControl
    {
        string resname,chosen_category="";
        Restaurant_admin past;
        string Connection = "Data Source=orcl;user id=hr;password=hr;";
        OracleConnection con;
        OracleCommand cmd;
        OracleDataReader dr;
        public Menu_Update()
        {
            InitializeComponent();
        }
        public Menu_Update(Restaurant_admin r,string res_name)
        {
            InitializeComponent();
            past = r;
            resname = res_name;
        }
        public void Menu_Update_Load(object sender, EventArgs e)
        {
            bunifuMetroTextbox5.Text = "";
            flowLayoutPanel1.Controls.Clear();
            bunifuDropdown1.Clear();
            chosen_category = "";

            con = new OracleConnection(Connection);
            con.Open();
            cmd = new OracleCommand();
            cmd.CommandText = "select categoryname from foodcategory where resname='"+resname+"' group by categoryname";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            dr= cmd.ExecuteReader();
            while(dr.Read())
            {
                bunifuDropdown1.AddItem(dr[0].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bunifuDropdown1.selectedIndex != -1)
            {
                con = new OracleConnection(Connection);
                con.Open();
                cmd = new OracleCommand();
                cmd.Connection = con;
                cmd.CommandText = "delete from foodcategory where resname='" + resname + "' and categoryname='" + bunifuDropdown1.selectedValue + "'";
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                this.Menu_Update_Load(this, e);
            }
            else
            {
                MessageBox.Show("Please Choose a Category");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (bunifuMetroTextbox5.Text == "")
                MessageBox.Show("please enter a valid category name");
            else
            {
                con = new OracleConnection(Connection);
                con.Open();
                cmd = new OracleCommand();
                cmd.Connection = con;
                cmd.CommandText = "select count(*) from foodcategory where resname='" + resname + "' and categoryname='" + bunifuMetroTextbox5.Text +"'";
                cmd.CommandType = CommandType.Text;
                dr = cmd.ExecuteReader();
                if (dr.Read() && Convert.ToInt32(dr[0]) > 0)
                {
                    MessageBox.Show("this Category already exists");
                    dr.Close();
                }
                else
                {
                    dr.Close();
                    cmd.CommandText = "insert into foodcategory values('" + resname + "','" + bunifuMetroTextbox5.Text + "','null','null',0)";
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    bunifuDropdown1.AddItem(bunifuMetroTextbox5.Text);
                }
                con.Close();
             }
            bunifuMetroTextbox5.Text = "";
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuDropdown1_onItemSelected(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            chosen_category = bunifuDropdown1.selectedValue;
            
            flowLayoutPanel1.Controls.Add(new Menu(this, "menu_update", resname, chosen_category));
        }
    }
}
