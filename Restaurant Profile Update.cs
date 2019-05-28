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
    public partial class Restaurant_Profile_Update : UserControl
    {
        Restaurant_admin past;
        string resname;
        int restaurant_index_in_dataset;
        int admin_index_in_dataset;
        List<string> payment_methods=new List<string>();
        OracleDataAdapter adapter,adapter1,adapter2;
        OracleCommandBuilder builder;
        string conn = "data source = orcl; user id = hr; password = hr;";
        OracleConnection connection;
        OracleCommand cmd;
        DataSet ds;
        public Restaurant_Profile_Update(Restaurant_admin r,string res_name)
        {
            InitializeComponent();
            past = r;
            resname = res_name;
        }

        private void Restaurant_Profile_Update_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            payment_methods.Clear();
            bunifuMetroTextbox1.Enabled = false;
            bunifuMetroTextbox10.Text = log_in.User_Email;
            bunifuMetroTextbox9.Text = log_in.password;
            ds = new DataSet();
            ds.Clear();
            string query1 = @"select * from restaurant where resname='"+resname+"'";
            adapter = new OracleDataAdapter(query1, conn);
            adapter.Fill(ds,"restaurants");
            DataColumn[] d1 = new DataColumn[1];
            d1[0] = ds.Tables["restaurants"].Columns[0];
            ds.Tables["restaurants"].PrimaryKey = d1;
            ds.Tables["restaurants"].Columns[0].Unique = true;
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                if (r[0].ToString() == resname)
                {
                    restaurant_index_in_dataset = ds.Tables[0].Rows.IndexOf(r);
                    bunifuMetroTextbox1.Text = resname;
                    bunifuDropdown4.selectedIndex =log_in.locations[ r[2].ToString()]-1;
                    bunifuMetroTextbox8.Text = r[1].ToString();
                    bunifuMetroTextbox3.Text = r[3].ToString();
                    bunifuDropdown3.selectedIndex = log_in.cuisine[r[4].ToString()]-1;
                    bunifuMetroTextbox11.Text = r[7].ToString();
                    bunifuMetroTextbox6.Text = r[8].ToString();
                    bunifuMetroTextbox5.Text = r[5].ToString();
                    bunifuMetroTextbox7.Text = r[6].ToString();
                    break;
                }
            }
            

            string query2 = @"select * from paymentmethod where resname='" + resname + "'";
            adapter1 = new OracleDataAdapter(query2, conn);
            adapter1.Fill(ds, "restaurants_payment");
            DataColumn[] d2 = new DataColumn[2];
            d2[0] = ds.Tables["restaurants_payment"].Columns[0];
            d2[1]= ds.Tables["restaurants_payment"].Columns[1] ;
            ds.Tables["restaurants_payment"].PrimaryKey = d2;
            foreach (DataRow r in ds.Tables["restaurants_payment"].Rows)
            {
                if (r[0].ToString() == resname)
                {
                    richTextBox1.Text += r[1].ToString() + "\n";
                    payment_methods.Add(r[1].ToString());
                }
            }

            string[] arr = new string[2];
            for (int i = 0; i < payment_methods.Count; i++)
            {
                arr[0] = resname;
                arr[1] = payment_methods[i];
                ds.Tables["restaurants_payment"].Rows.Find(arr).Delete();//.RemoveAt(payment_index_in_dataset[i]);
            }

            
            string query3 = @"select * from admins where resname='" + resname + "'";
            adapter2 = new OracleDataAdapter(query3, conn);
            adapter2.Fill(ds, "restaurants_admins");
            DataColumn[] d3 = new DataColumn[1];
            d3[0] = ds.Tables["restaurants_admins"].Columns[0];
            ds.Tables["restaurants_admins"].PrimaryKey = d3;
            ds.Tables["restaurants_admins"].Columns[0].Unique = true;
            ds.Tables["restaurants_admins"].Columns[2].Unique = true;
            foreach (DataRow r in ds.Tables[2].Rows)
            {
                if (r[2].ToString() == resname)
                {
                    admin_index_in_dataset = ds.Tables[2].Rows.IndexOf(r);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
            
            int check = 0, check2 = 0;
            if (bunifuMetroTextbox10.Text != log_in.User_Email)
            {
                connection = new OracleConnection(conn);
                connection.Open();
                cmd = new OracleCommand("select count(email) from ADMINS where email='" + bunifuMetroTextbox10.Text + "'", connection);
                cmd.CommandType = CommandType.Text;

                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    check = int.Parse(reader[0].ToString());
                }
                reader.Close();

                cmd = new OracleCommand("select count(useremail) from useraccount where useremail='" + bunifuMetroTextbox10.Text + "'", connection);
                cmd.CommandType = CommandType.Text;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    check2 = int.Parse(reader[0].ToString());
                }
                reader.Close();
                connection.Close();
            }
            if (check == 1 || check2 == 1)
            {
                MessageBox.Show("The Data is Exist");
            }
            else
            {
                
                DataRow row; 
                if (richTextBox1.Text != "")
                {
                    string[] arr = richTextBox1.Text.Split('\n');
                    string[] primary = new string[2];
                    foreach (string i in arr)
                    {
                        row = ds.Tables["restaurants_payment"].NewRow();
                        row[0] = resname;
                        row[1] = i;
                        primary[0] = resname;
                        primary[1] = i;
                        if (i == "" || ds.Tables["restaurants_payment"].Rows.Contains(primary))
                            continue;
                        ds.Tables["restaurants_payment"].Rows.Add(row);
                    }
                    builder = new OracleCommandBuilder(adapter1);
                    adapter1.Update(ds.Tables[1]);
                }
                ds.Tables["restaurants_admins"].Rows[admin_index_in_dataset][0] = bunifuMetroTextbox10.Text;
                ds.Tables["restaurants_admins"].Rows[admin_index_in_dataset][1] = bunifuMetroTextbox9.Text;
                builder = new OracleCommandBuilder(adapter2);
                adapter2.Update(ds.Tables[2]);

                ds.Tables["restaurants"].Rows[restaurant_index_in_dataset][1] = bunifuMetroTextbox8.Text;
                ds.Tables["restaurants"].Rows[restaurant_index_in_dataset][2] = bunifuDropdown4.selectedValue;
                ds.Tables["restaurants"].Rows[restaurant_index_in_dataset][3] = bunifuMetroTextbox3.Text;
                ds.Tables["restaurants"].Rows[restaurant_index_in_dataset][4] = bunifuDropdown3.selectedValue;
                ds.Tables["restaurants"].Rows[restaurant_index_in_dataset][5] = bunifuMetroTextbox5.Text;
                ds.Tables["restaurants"].Rows[restaurant_index_in_dataset][6] = bunifuMetroTextbox7.Text;
                ds.Tables["restaurants"].Rows[restaurant_index_in_dataset][7] = bunifuMetroTextbox11.Text;
                ds.Tables["restaurants"].Rows[restaurant_index_in_dataset][8] = bunifuMetroTextbox6.Text;

                log_in.User_Email = bunifuMetroTextbox10.Text;
                log_in.password= bunifuMetroTextbox9.Text;
                
                
                builder = new OracleCommandBuilder(adapter);
                adapter.Update(ds.Tables[0]);

            }
            Restaurant_Profile_Update_Load(this, e);
        }
    }
}
