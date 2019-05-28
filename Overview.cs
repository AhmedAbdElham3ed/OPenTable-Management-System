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
    public partial class Overview : UserControl
    {
        string restaurant_name;
        Bitmap bmp;
        List<Button> buttons = new List<Button>();

        string conn = "data source = orcl; user id = hr; password = hr;";
        public Overview()
        {
            InitializeComponent();
        }
        public Overview(string res_name)
        {
            InitializeComponent();
            restaurant_name = res_name;
            buttons.Add(button2);
            buttons.Add(button3);
            buttons.Add(button4);
            buttons.Add(button5);
            buttons.Add(button6);
        }
        private void change_color(Button b, Color new_color, int button_width)
        {

            Color actualColor;
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //get the pixel from the scrBitmap image
                    actualColor = bmp.GetPixel(i, j);
                    //MessageBox.Show(actualColor.ToString());
                    // > 150 because.. Images edges can be of low pixel colr. if we set all pixel color to new then there will be no smoothness left.
                    if (actualColor.A > 150 && i <= button_width)
                        bmp.SetPixel(i, j, new_color);
                    else
                        bmp.SetPixel(i, j, actualColor);
                }
            }
        }
        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Overview_Load(object sender, EventArgs e)
        {
            label8.Text = restaurant_name;
            
            string query = "select * from restaurant where resname='" + restaurant_name + "'";
            OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            DataRow row = ds.Tables[0].Rows[0];
            label4.Text = row[4].ToString();
            label10.Text = row[3].ToString();
            label6.Text = row[7].ToString();
            label7.Text = row[8].ToString();
            label5.Text = row[1].ToString() + ",\n " + row[2].ToString();
            label3.Text = "$" + row[5].ToString() + " to $" + row[6].ToString();

            query = "select * from paymentmethod where resname='" + restaurant_name + "'";
            adapter = new OracleDataAdapter(query, conn);
            ds = new DataSet();
            adapter.Fill(ds);
            // connect with frontend
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                bunifuDropdown1.AddItem(r[1].ToString());
            }
            OracleConnection con = new OracleConnection(conn);
            con.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = "best_Restaurants";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("num_p", 1);
            cmd.Parameters.Add("mn_price", 1);
            cmd.Parameters.Add("mx_price", -1);
            cmd.Parameters.Add("res_cuisine", "null");
            cmd.Parameters.Add("res_name", restaurant_name);
            cmd.Parameters.Add("res_location", "null");
            cmd.Parameters.Add("valid_res", OracleDbType.RefCursor, ParameterDirection.Output);
            OracleDataReader dr = cmd.ExecuteReader();
            double rate = 0;
            if(dr.Read())
            {
                rate = Convert.ToDouble(dr[1]);
                for (int i = 0; i < (int)rate; i++)
                {
                    bmp = (Bitmap)(buttons[i].Image);
                    change_color(buttons[i], Color.FromArgb(218, 55, 67), buttons[i].Width);
                }
                if ((int)rate < 5)
                {
                    bmp = (Bitmap)(buttons[(int)rate].Image);
                    //MessageBox.Show(((int)(buttons[(int)rate].Width * (rate - (double)((int)rate))) + 1).ToString());
                    change_color(buttons[(int)rate], Color.FromArgb(218, 55, 67), (int)(buttons[(int)rate].Width * (rate - (double)((int)rate))) + 1);

                }
                label2.Text = dr[2].ToString() + " reviews";
               
            }
            
            // connect with frontend

        }
    }
}
