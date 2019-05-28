using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenTable
{
    public partial class specific_time : UserControl
    {
        int number_of_people=2,dining_points;
        all_restaurants past;
        string res_name,date,time;
        public specific_time()
        {
            InitializeComponent();
        }
        public specific_time(all_restaurants a_r, string _res_name,string _date, string _time,int _number_of_people, int _points)
        {
            InitializeComponent();
            button2.Text = _time;
            if (_points == 0)
            {
                this.Height = button2.Height + 10;
            }
            else
            {
                label1.Text = "+ " + _points.ToString();
            }
            res_name = _res_name;
            number_of_people = _number_of_people;
            date = _date;
            time = _time;
            dining_points = _points;
            past = a_r;
        }
        private void specific_time_Load(object sender, EventArgs e)
        {

        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(res_name + " " + date + " " + time);
            making_reservation m = new making_reservation("Restaurants",past,res_name,date,time,number_of_people, dining_points);
            m.Show();
            past.Hide();
        }
    }
}
