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
    public partial class reservations_admin : UserControl
    {
        Restaurant_admin past;
        string resname;

        string Connection = "Data Source=orcl;user id=hr;password=hr;";
        OracleConnection con;
        OracleCommand cmd;
        OracleCommandBuilder builder;
        OracleDataAdapter adapter1, adapter2, adapter3;
        DataSet ds;
        public reservations_admin(Restaurant_admin r,string res_name)
        {
            InitializeComponent();
            past = r;
            resname = res_name;
        }

        private void reservations_admin_Load(object sender, EventArgs e)
        {
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
            ds = new DataSet();
            adapter1 = new OracleDataAdapter("select * from schedule", Connection);
            adapter1.Fill(ds, "schedule");
            adapter2 = new OracleDataAdapter("select * from signedreservation", Connection);
            adapter2.Fill(ds, "signedreservation");
            adapter3 = new OracleDataAdapter("select * from signedoutreservation", Connection);
            adapter3.Fill(ds, "signedoutreservation");
            DataColumn[] d = { ds.Tables[0].Columns[0], ds.Tables[0].Columns[2] };
            ds.Tables[0].PrimaryKey = d;
            DataColumn[] d1 = { ds.Tables[1].Columns[2],ds.Tables[1].Columns[3]};
            ds.Tables[1].PrimaryKey = d1;
            DataColumn[] d2 = { ds.Tables[2].Columns[4], ds.Tables[2].Columns[5] };
            ds.Tables[2].PrimaryKey = d2;
            DataRelation r = new DataRelation("fk", ds.Tables[0].PrimaryKey, ds.Tables[1].PrimaryKey);
            ds.Relations.Add(r);
            r = new DataRelation("sk", ds.Tables[0].PrimaryKey, ds.Tables[2].PrimaryKey);
            ds.Relations.Add(r);
            
            BindingSource bs_master = new BindingSource(ds, "schedule");
            BindingSource bs_child1 = new BindingSource(bs_master,"fk");
            BindingSource bs_child2 = new BindingSource(bs_master, "sk");

            dataGridView1.DataSource = bs_master;
            dataGridView2.DataSource = bs_child1;
            dataGridView3.DataSource = bs_child2;

        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            for (int row = 0; row < dataGridView2.Rows.Count; row++)
            {
                if (dataGridView2.Rows[row].Cells[8].Value.ToString() == "confirmed")
                {
                    dataGridView2.Rows[row].Cells[8].ReadOnly = true;
                }
            }

            for (int row = 0; row < dataGridView3.Rows.Count; row++)
            {
                if (dataGridView3.Rows[row].Cells[9].Value.ToString() == "confirmed")
                {
                    dataGridView3.Rows[row].Cells[9].ReadOnly = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            builder = new OracleCommandBuilder(adapter2);
            adapter2.Update(ds.Tables[1]);

            builder = new OracleCommandBuilder(adapter3);
            adapter3.Update(ds.Tables[2]);

            MessageBox.Show("the data is successfully updated");
        }
    }
}
