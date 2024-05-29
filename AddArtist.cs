using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace melodisc_a_music_app
{
    public partial class AddArtist : Form
    {
        private OracleConnection connection;


        //private List<int> year = new List<int>();
        //private List<string> email = new List<string>();
        //private List<string> name = new List<string>();
        //private List<string> gender = new List<string>();
        public AddArtist()
        {
            InitializeComponent();
            connection = new OracleConnection("User Id=melodisc1;Password=melodisc1;Data Source=localhost:1521");
            try
            {
                connection.Open();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //name
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminForm home = new AdminForm();
            home.Show();
            this.Hide();
        }

        private void AddArtist_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }
        private void LoadUsers()
        {
            /*
            string query = "SELECT * FROM artists";
            OracleCommand cmd = new OracleCommand(query, connection);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name.Add(reader.GetString(0));
                    gender.Add(reader.GetString(1));
                    email.Add(reader.GetString(2));
                   // year.Add(reader.GetString(3));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }*/
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //name
        }

        private void label2_Click(object sender, EventArgs e)
        {
           // email
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //gender
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string artistName = textBox1.Text;
            string email = textBox2.Text;
            string gender = textBox3.Text;
            int birthYear = int.Parse(textBox4.Text);
            int noOfSongs = int.Parse(textBox6.Text);
            int noOfAlbums = int.Parse(textBox7.Text);
            string biography = richTextBox1.Text;

            try
            {
                // Insert new artist into artists table
                string insertQuery = "INSERT INTO artists (artist_name, gender, email, birthyear, no_of_songs, no_of_albums, biography) " +
                                     "VALUES (:artistName, :gender, :email, :birthYear, :noOfSongs, :noOfAlbums, :biography)";

                OracleCommand insertCmd = new OracleCommand(insertQuery, connection);
                insertCmd.Parameters.Add(new OracleParameter("artistName", artistName));
                insertCmd.Parameters.Add(new OracleParameter("gender", gender));
                insertCmd.Parameters.Add(new OracleParameter("email", email));
                insertCmd.Parameters.Add(new OracleParameter("birthYear", birthYear));
                insertCmd.Parameters.Add(new OracleParameter("noOfSongs", noOfSongs));
                insertCmd.Parameters.Add(new OracleParameter("noOfAlbums", noOfAlbums));
                insertCmd.Parameters.Add(new OracleParameter("biography", OracleDbType.Clob)).Value = biography;

                int rowsAffected = insertCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Artist added successfully.");
                    AdminForm f1 = new AdminForm();
                    f1.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Error adding artist.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting artist: " + ex.Message);
            }

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //birthyear
        }

        private void label5_Click_1(object sender, EventArgs e)
        {
            //birthyear
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //gender
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           //email
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
