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
    public partial class Food : UserControl
    {
        Menu past; 
        int price;
        string source, restaurant_name, category_name,foodname,discription, food_state;
        string new_foodname, new_description;
        int new_price;
        bool valid = true;
        string Connection = "Data Source=orcl;user id=hr;password=hr;";
        OracleConnection con;
        OracleCommand cmd;

        private void bunifuMetroTextbox1_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void bunifuMetroTextbox2_OnValueChanged(object sender, EventArgs e)
        {

        }

        OracleDataReader dr;
        private void button1_Click(object sender, EventArgs e)
        {

            if(button1.Text=="Delete")
            {
                con = new OracleConnection(Connection);
                con.Open();
                cmd = new OracleCommand();
                cmd.Connection = con;
                cmd.CommandText = "delete from foodcategory  where resname = '" + restaurant_name + "' and categoryname = '" + category_name + "' and foodname = '" + foodname + "'";
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                valid = true;
                if (bunifuMetroTextbox1.Text == "")
                    MessageBox.Show("please complete all the information");
                else
                {
                    new_foodname = bunifuMetroTextbox1.Text;
                    if (richTextBox1.Text == "")
                        new_description = "null";
                    else
                        new_description = richTextBox1.Text;
                    try
                    {
                        if (bunifuMetroTextbox2.Text == "")
                            new_price = 0;
                        else
                        {
                            new_price = Convert.ToInt32(bunifuMetroTextbox2.Text);
                            if (new_price == 0)
                            {
                                MessageBox.Show("please enter a valid price");
                                valid = false;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("please enter a valid price");
                        valid = false;
                    }
                    if (valid == true)
                    {
                        
                        con = new OracleConnection(Connection);
                        con.Open();
                        cmd = new OracleCommand();
                        cmd.Connection = con;
                        if (bunifuMetroTextbox1.Text != foodname)
                        {
                            cmd.CommandText = "select count(*) from foodcategory where resname='" + restaurant_name + "' and categoryname='" + category_name + "' and foodname='" + new_foodname + "' and foodname!='null'";
                            cmd.CommandType = CommandType.Text;
                            dr = cmd.ExecuteReader();
                            if (dr.Read() && Convert.ToInt32(dr[0]) > 0)
                                valid = false;
                            dr.Close();
                        }

                        if (valid == true)
                        {
                            cmd.CommandText = "delete from foodcategory where resname='" + restaurant_name + "' and categoryname='" + category_name + "' and foodname='null'";
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "insert into foodcategory values('"+restaurant_name+"','" + category_name + "','"+new_foodname+"','" + new_description + "','" + new_price + "')";
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                            foodname = new_foodname;
                            discription = new_description;
                            price = new_price;
                            MessageBox.Show("the data is successfully added");
                        }
                        else
                        {
                            MessageBox.Show("the data already exists");
                        }

                        con.Close();
                    }
                }
               
            }
            past.Menu_Load(this, e);
        }

        
        
        public Food()
        {
            InitializeComponent();
        }
        public Food(Menu m,string _source,string res_name,string _category_name,string _foodname,string _discription,int _price,string _food_state)
        {
            InitializeComponent();
            past = m;
            source = _source;
            restaurant_name = res_name;
            category_name = _category_name;
            foodname = _foodname;
            discription = _discription;
            price = _price;
            food_state = _food_state;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            valid = true;
            if (bunifuMetroTextbox1.Text == "")
                MessageBox.Show("please complete all the information");
            else
            {
                new_foodname = bunifuMetroTextbox1.Text;
                if (richTextBox1.Text == "")
                    new_description = "null";
                else
                    new_description = richTextBox1.Text;
                try
                {
                    if (bunifuMetroTextbox2.Text == "")
                        new_price = 0;
                    else
                    {
                        new_price = Convert.ToInt32(bunifuMetroTextbox2.Text);
                        if (new_price == 0)
                        {
                            MessageBox.Show("please enter a valid price");
                            valid = false;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("please enter a valid price");
                    valid = false;
                }
                if (valid == true)
                {
                    con = new OracleConnection(Connection);
                    con.Open();
                    cmd = new OracleCommand();
                    cmd.Connection = con;
                    if (bunifuMetroTextbox1.Text != foodname)
                    {
                        cmd.CommandText = "select count(*) from foodcategory where resname='" + restaurant_name + "' and categoryname='" + category_name + "' and foodname='" + new_foodname + "'";
                        cmd.CommandType = CommandType.Text;
                        dr = cmd.ExecuteReader();
                        if (dr.Read() && Convert.ToInt32(dr[0]) > 0)
                            valid = false;
                        dr.Close();
                    }

                    if (valid == true)
                    { 
                        cmd.CommandText = "update foodcategory set foodname='" + new_foodname + "', description='" + new_description + "', price='" + new_price + "' where resname = '" + restaurant_name + "' and categoryname = '" + category_name + "' and foodname = '" + foodname + "'";
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        foodname = new_foodname;
                        discription = new_description;
                        price = new_price;
                        MessageBox.Show("the data is updated");
                    }
                    else
                    {
                        MessageBox.Show("the data already exists");
                    }
                    
                    con.Close();
                }
            }
            Food_Load(this, e);
        }
        public void Food_Load(object sender, EventArgs e)
        {
            bunifuMetroTextbox1.Text = foodname;
            if (price > 0)
                bunifuMetroTextbox2.Text = "$" + price.ToString();
            else
                bunifuMetroTextbox2.Text = "";
            if (discription!="null")
                richTextBox1.Text = discription;
            else
                richTextBox1.Text = "";

            if (source == "Restaurant_profile")
            {
                button1.Visible = button2.Visible = false;
                bunifuMetroTextbox1.BorderStyle = BorderStyle.None;
                bunifuMetroTextbox2.BorderStyle = BorderStyle.None;
                richTextBox1.BorderStyle = BorderStyle.None;
                bunifuMetroTextbox1.Enabled = false;
                bunifuMetroTextbox2.Enabled = false;
                richTextBox1.ReadOnly = true;
                this.Width += 5;
                bunifuMetroTextbox1.Width += 5;
                bunifuMetroTextbox2.Location = new Point(button2.Location.X + bunifuMetroTextbox1.Width + 5, bunifuMetroTextbox2.Location.Y);
                bunifuMetroTextbox2.Width += 10;
                richTextBox1.Width += 40;
            }
            if (source == "menu_update")
            {
                if (food_state == "old")
                    button1.Visible = button2.Visible = true;
                else
                {
                    button2.Visible = false;
                    button1.Visible = true;
                    button1.Text = "Add";
                }

                this.BorderStyle = BorderStyle.FixedSingle;
                bunifuMetroTextbox1.BorderStyle = BorderStyle.FixedSingle;
                bunifuMetroTextbox2.BorderStyle = BorderStyle.FixedSingle;
                richTextBox1.BorderStyle = BorderStyle.FixedSingle;
                bunifuMetroTextbox1.Enabled = true;
                bunifuMetroTextbox2.Enabled = true;
                richTextBox1.ReadOnly = false;
            }
        }
    }
}
