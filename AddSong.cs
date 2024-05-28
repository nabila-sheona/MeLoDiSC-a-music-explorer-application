using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace melodisc_a_music_app
{
    public partial class AddSong : Form
    {
        private OracleConnection connection;

        //private List<int> songnumber = new List<int>();
        //private List<string> artist = new List<string>();
        //private List<string> name = new List<string>();
        //private List<string> album = new List<string>();

        public AddSong()
        {
            InitializeComponent();
            connection = new OracleConnection("User Id=melodisc1;Password=melodisc1;Data Source=localhost:1521");
            try
            {
                connection.Open();
                //LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminForm home = new AdminForm();
            home.Show();
            this.Hide();
        }

        private void AddSong_Load(object sender, EventArgs e)
        {
           /*string query = "SELECT username FROM users";
            OracleCommand cmd = new OracleCommand(query, connection);
            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                name.Add(reader.GetString(0));
            }
            reader.Close();
            
            LoadUsers();
           */
        }
        private void LoadUsers()
        {
            /*string query = "SELECT * FROM songs";
            OracleCommand cmd = new OracleCommand(query, connection);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name.Add(reader.GetString(0));
                    artist.Add(reader.GetString(1));
                    album.Add(reader.GetString(2));
                    
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
            */
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string songName = textBox1.Text;
            string artistName = textBox2.Text;
            string albumName = textBox3.Text;
            int songNumber = int.Parse(textBox4.Text);
            string genreName = textBox7.Text;
            double duration = double.Parse(textBox6.Text);
            DateTime releaseDate = dateTimePicker1.Value;


            try
            {
                // Fetch genre_id from genres table
                string genreQuery = "SELECT genre_id FROM genres WHERE genre_name = :genreName";
                OracleCommand genreCmd = new OracleCommand(genreQuery, connection);
                genreCmd.Parameters.Add(new OracleParameter("genreName", genreName));
                int genreId = 0;
                OracleDataReader genreReader = genreCmd.ExecuteReader();
                if (genreReader.Read())
                {
                    genreId = genreReader.GetInt32(0);
                }
                else
                {
                    // If genre does not exist, insert new genre
                    string insertGenreQuery = "INSERT INTO genres (genre_name) VALUES (:genreName)";
                    OracleCommand insertGenreCmd = new OracleCommand(insertGenreQuery, connection);
                    insertGenreCmd.Parameters.Add(new OracleParameter("genreName", genreName));
                    insertGenreCmd.ExecuteNonQuery();



                    // Fetch the newly inserted genre_id
                    genreCmd = new OracleCommand(genreQuery, connection);
                    genreCmd.Parameters.Add(new OracleParameter("genreName", genreName));
                    genreReader = genreCmd.ExecuteReader();


                    if (genreReader.Read())
                    {
                        genreId = genreReader.GetInt32(0);
                    }


                }
                genreReader.Close();





                // Insert new song into songs table
                string insertQuery = "INSERT INTO songs (song_name, artist_name, album_name, song_number, genre_id, duration, release_date) " +
                                     "VALUES (:songName, :artistName, :albumName, :songNumber, :genreId, :duration, :releaseDate)";
                OracleCommand insertCmd = new OracleCommand(insertQuery, connection);
                insertCmd.Parameters.Add(new OracleParameter("songName", songName));
                insertCmd.Parameters.Add(new OracleParameter("artistName", artistName));
                insertCmd.Parameters.Add(new OracleParameter("albumName", albumName));
                insertCmd.Parameters.Add(new OracleParameter("trackNumber", songNumber));
                insertCmd.Parameters.Add(new OracleParameter("genreId", genreId));
                insertCmd.Parameters.Add(new OracleParameter("duration", duration));
                insertCmd.Parameters.Add(new OracleParameter("releaseDate", releaseDate));



                //incrementing no of songs of artist whose new song was added
                string updateArtistQuery = "UPDATE artists SET no_of_songs = no_of_songs + 1 WHERE artist_name = :artistName";
                OracleCommand updateArtistCmd = new OracleCommand(updateArtistQuery, connection);
                updateArtistCmd.Parameters.Add(new OracleParameter("artistName", artistName));

                int rowsAffected = insertCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Song added successfully.");
                    AdminForm f1 = new AdminForm();
                    f1.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Error adding song.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting song: " + ex.Message);
            }



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
