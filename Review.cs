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
    public partial class Review : UserControl
    {
        Bitmap bmp;
        List<Button> buttons = new List<Button>();
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
        public Review()
        {
            InitializeComponent();
        }
        public Review(string _name,string _city,double _rate,string _discription)
        {
            InitializeComponent();
            button2.Text = _name;
            label1.Text = _city;
            richTextBox1.Text = _discription;
            buttons.Add(button1);
            buttons.Add(button3);
            buttons.Add(button4);
            buttons.Add(button5);
            buttons.Add(button6);
            for (int i = 0; i < (int)_rate; i++)
            {
                bmp = (Bitmap)(buttons[i].Image);
                change_color(buttons[i], Color.FromArgb(218, 55, 67), buttons[i].Width);
            }
            if ((int)_rate < 5)
            {
                bmp = (Bitmap)(buttons[(int)_rate].Image);
                //MessageBox.Show(((int)(buttons[(int)rate].Width * (rate - (double)((int)rate))) + 1).ToString());
                change_color(buttons[(int)_rate], Color.FromArgb(218, 55, 67), (int)(buttons[(int)_rate].Width * (_rate - (double)((int)_rate))) + 1);

            }
        }
        private void Review_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
