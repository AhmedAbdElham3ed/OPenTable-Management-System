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
    public partial class pop_restaurant : UserControl
    {
        Form1 past;
        Bitmap bmp;
        List<Button> buttons=new List<Button>();
        string Restaurant_name;
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
        public pop_restaurant()
        {
            InitializeComponent();
        }
        public pop_restaurant(Form1 f,string  restaurant_name,double rate, int number_of_reviews,string cusine,string average_price,string location,int Number_of_reservations)
        {
            InitializeComponent();
            past = f;
            Restaurant_name = restaurant_name;
            buttons.Add(button2);
            buttons.Add(button3);
            buttons.Add(button4);
            buttons.Add(button5);
            buttons.Add(button6);
            for(int i=0;i<(int)rate;i++)
            {
                bmp = (Bitmap)(buttons[i].Image);
                change_color(buttons[i], Color.FromArgb(218, 55, 67), buttons[i].Width);
            }
            if((int)rate<5)
            {
                bmp = (Bitmap)(buttons[(int)rate].Image);
                //MessageBox.Show(((int)(buttons[(int)rate].Width * (rate - (double)((int)rate))) + 1).ToString());
                change_color(buttons[(int)rate], Color.FromArgb(218, 55, 67), (int)(buttons[(int)rate].Width*(rate-(double)((int)rate)))+1);
                
            }
            label1.Text = restaurant_name;
            if (number_of_reviews > 0)
                label2.Text = number_of_reviews.ToString() + " reviews";
            else
                label2.Text = "";
            label3.Text = cusine;
            label5.Text = average_price;
            label6.Text=  location;
            if(Number_of_reservations>0)
            label4.Text = "booked " + Number_of_reservations.ToString() + " times today";
            else
            {
                label4.Text = "";
                bunifuImageButton1.Visible = false;
            }
        }
        private void label2_Click(object sender, EventArgs e)
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Restaurant_Profile r = new Restaurant_Profile("Home", past, Restaurant_name);
            r.Show();
            past.Hide();
        }
    }
}
