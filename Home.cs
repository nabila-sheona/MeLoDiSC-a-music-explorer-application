using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace melodisc_a_music_app
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Explorer otherForm = new Explorer();
            otherForm.Show();
            this.Hide();



        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f2 = new Form1();
            f2.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Explorer otherForm = new Explorer();
            otherForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserProfile otherForm = new UserProfile(UserSession.user_name);
            otherForm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Artistprofile otherForm = new Artistprofile();
            otherForm.Show();
            this.Hide();

        }

        private void Home_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Review otherForm = new Review(UserSession.user_name);
            otherForm.Show();
            this.Hide();
        }
    }
}
