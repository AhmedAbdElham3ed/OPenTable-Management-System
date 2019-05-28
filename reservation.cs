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
    public partial class reservation : UserControl
    {
        my_reservations m_r;
        int res_id;
        string s,res_name, phone, E_mail, date, time, occasion, reservation_state;
        int number_of_people, dining_points;
        bool dining_points_option;

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*
             Code here
             */
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        
        public reservation()
        {
            InitializeComponent();
        }
        public reservation(my_reservations f,int _res_id,string _res_name,string _phone,string _E_mail,string _date,string _time,int _number_of_people,string _occasion,int _dining_points,bool _dining_points_option,string source,string _reservation_state)
        {
            InitializeComponent();
            m_r = f;
            res_id = _res_id;
            res_name = _res_name;
            phone = _phone;
            E_mail = _E_mail;
            date = _date;
            time = _time;
            number_of_people = _number_of_people;
            occasion = _occasion;
            dining_points = _dining_points;
            dining_points_option= _dining_points_option;
            reservation_state = _reservation_state;
            s = source;
        }
        private void reservation_Load(object sender, EventArgs e)
        {
            label8.Text = phone;
            label9.Text = E_mail;
            label4.Text = date;
            label2.Text = time;
            label5.Text = number_of_people.ToString() + " people";
            label3.Text = occasion;
            label6.Text = dining_points.ToString();
            checkBox1.Enabled = false;
            checkBox1.Checked = dining_points_option;
           /* if (s == "My_reservation")
            {
                button2.Visible = false;
            }
            else if (s == "Restaurant_admin")
            {
                if (reservation_state == "unconfirmed")
                    button2.Visible = true;
                else
                    button2.Visible = false;
            }*/
        }
    }
}
