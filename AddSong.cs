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
            string genreName = textBox5.Text;
            double duration = double.Parse(textBox6.Text);
            DateTime releaseDate = dateTimePicker1.Value;

            try
            {
                // Fetch genre_id from genres table
                string genreQuery = "SELECT genre_id FROM genres WHERE genre_name = :genreName";
                OracleCommand genreCmd = new OracleCommand(genreQuery, connection);
                genreCmd.Parameters.Add(new OracleParameter("genreName", genreName));
                int genreId;
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
                    genreReader.Read();
                    genreId = genreReader.GetInt32(0);
                }
                genreReader.Close();

                // Fetch artist_id from artists table
                string artistQuery = "SELECT artist_id FROM artists WHERE artist_name = :artistName";
                OracleCommand artistCmd = new OracleCommand(artistQuery, connection);
                artistCmd.Parameters.Add(new OracleParameter("artistName", artistName));
                int artistId = 0;
                OracleDataReader artistReader = artistCmd.ExecuteReader();
                if (artistReader.Read())
                {
                    artistId = artistReader.GetInt32(0);
                }
                else
                {
                    MessageBox.Show("Artist not found.");
                    artistReader.Close();
                    return;
                }
                artistReader.Close();

                // Fetch album_id from albums table
                string albumQuery = "SELECT album_id FROM albums WHERE album_name = :albumName";
                OracleCommand albumCmd = new OracleCommand(albumQuery, connection);
                albumCmd.Parameters.Add(new OracleParameter("albumName", albumName));
                int albumId = 0;
                OracleDataReader albumReader = albumCmd.ExecuteReader();

                if (albumReader.Read())
                {
                    albumId = albumReader.GetInt32(0);
                }
                else
                {
                    MessageBox.Show("Album not found.");
                    albumReader.Close();
                    return;
                }
                albumReader.Close();

                // Insert new song into songs table
                string insertQuery = "INSERT INTO songs (song_name, album_name, song_number, genre_id, duration, release_date, artist_id) " +
                                     "VALUES (:songName, :albumName, :songNumber, :genreId, :duration, :releaseDate, :artistId)";
                OracleCommand insertCmd = new OracleCommand(insertQuery, connection);
                insertCmd.Parameters.Add(new OracleParameter("songName", songName));
                insertCmd.Parameters.Add(new OracleParameter("albumName", albumName));
                insertCmd.Parameters.Add(new OracleParameter("songNumber", songNumber));
                insertCmd.Parameters.Add(new OracleParameter("genreId", genreId));
                insertCmd.Parameters.Add(new OracleParameter("duration", duration));
                insertCmd.Parameters.Add(new OracleParameter("releaseDate", releaseDate));
                insertCmd.Parameters.Add(new OracleParameter("artistId", artistId));

                int rowsAffected = insertCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    // Increment the number of songs and albums for the artist
                    string updateArtistQuery = @"
                UPDATE artists 
                SET no_of_songs = no_of_songs + 1, 
                    no_of_albums = no_of_albums + 
                    (SELECT COUNT(*) FROM albums WHERE artist_id = :artistId)
                WHERE artist_id = :artistId";
                    OracleCommand updateArtistCmd = new OracleCommand(updateArtistQuery, connection);
                    updateArtistCmd.Parameters.Add(new OracleParameter("artistId", artistId));
                    updateArtistCmd.ExecuteNonQuery();

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
