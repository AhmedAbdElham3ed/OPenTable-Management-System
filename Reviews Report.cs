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
using CrystalDecisions.Shared;    
namespace OpenTable
{
    public partial class Reviews_Report : UserControl
    {
        string resname;
        Restaurant_admin past;
        string con = "data source = orcl; user id = hr; password = hr;";
        OracleConnection connection;
        OracleCommand cmd;
        public Reviews_Report(Restaurant_admin r, string res_name)
        {
            InitializeComponent();
            past = r;
            resname = res_name;
        }

        private void Reviews_Report_Load(object sender, EventArgs e)
        {
            CrystalReport1 CR = new CrystalReport1();
            connection = new OracleConnection(con);
            connection.Open();
            cmd = new OracleCommand("select RESNAME from ADMINS where EMAIL='" + log_in.User_Email+"'",connection);
            cmd.CommandType = CommandType.Text;
            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CR.SetParameterValue(0, reader[0].ToString());
            }
            crystalReportViewer1.ReportSource = CR;
            connection.Close();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
