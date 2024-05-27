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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace melodisc_a_music_app
{
    public partial class Artistprofile : Form
    {
        private OracleConnection connection;

        public Artistprofile()
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
            //replace with artist's name
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //list of songs by the artist from songs table
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //list of albums by the artist from artist table
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //insert name of the artist we are looking for

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //by clicking this button, the labels and grids will be replaced with requireds
            string artistName = textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(artistName))
            {
                LoadArtistProfile(artistName);
            }
            else
            {
                MessageBox.Show("Please enter an artist's name.");
            }


        }

        private void LoadArtistProfile(string artistName)
        {
            label1.Text = artistName;
            LoadSongs(artistName);
            LoadAlbums(artistName);
            LoadArtistCounts(artistName);
        }

        private void LoadSongs(string artistName)
        {
            string query = "SELECT song_name AS \"Song Name\", album_name AS \"Album Name\", track_number AS \"Track Number\" FROM songs WHERE artist_name = :artistName";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter("artistName", artistName));

            try
            {
                OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading songs: " + ex.Message);
            }
        }

        private void LoadAlbums(string artistName)
        {
            string query = "SELECT album_name AS \"Album Name\" FROM albums WHERE artist_name = :artistName";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter("artistName", artistName));

            try
            {
                OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView2.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading albums: " + ex.Message);
            }
        }

        private void LoadArtistCounts(string artistName)
        {
            string query = "SELECT no_of_songs, no_of_albums FROM artists WHERE artist_name = :artistName";

            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter("artistName", artistName));

            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int noOfSongs = reader.GetInt32(0);
                    int noOfAlbums = reader.GetInt32(1);
                    label3.Text = $"Number of Songs: {noOfSongs}";
                    label2.Text = $"Number of Albums: {noOfAlbums}";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading artist counts: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //number of songs
        }

        private void label2_Click(object sender, EventArgs e)
        {
            //number of albums
        }
    }
    }




//CREATE TABLE songs (
//song_id INT PRIMARY KEY,
//song_name VARCHAR(255),
//  artist_name VARCHAR(255),
//album_name VARCHAR(255),
//track_number INT
//);

//CREATE TABLE albums (
//  album_id INT PRIMARY KEY,
// album_name VARCHAR(255),
//artist_name VARCHAR(255)
//);

//CREATE TABLE artists (
//  artist_name VARCHAR(255) PRIMARY KEY,
//gender VARCHAR(10),
//email VARCHAR(255),
//birthyear INT,
//no_of_songs INT,
//no_of_albums INT
//);

/*
INSERT INTO artists (artist_name, gender, email, birthyear, no_of_songs, no_of_albums) VALUES ('Taylor Swift', 'Female', 'taylor@swift.com', 1989, 5, 2);
INSERT INTO artists (artist_name, gender, email, birthyear, no_of_songs, no_of_albums) VALUES ('Daya', 'Female', 'daya@daya.com', 1998, 3, 1);
INSERT INTO artists (artist_name, gender, email, birthyear, no_of_songs, no_of_albums) VALUES ('Maroon 5', 'Male', 'contact@maroon5.com', 2001, 4, 2);
INSERT INTO albums (album_id, album_name, artist_name) VALUES (1, '1989', 'Taylor Swift');
INSERT INTO albums (album_id, album_name, artist_name) VALUES (2, 'Reputation', 'Taylor Swift');
INSERT INTO albums (album_id, album_name, artist_name) VALUES (3, 'Sit Still, Look Pretty', 'Daya');
INSERT INTO albums (album_id, album_name, artist_name) VALUES (4, 'Songs About Jane', 'Maroon 5');
INSERT INTO albums (album_id, album_name, artist_name) VALUES (5, 'V', 'Maroon 5');
INSERT INTO songs (song_id, song_name, artist_name, album_name, track_number) VALUES (1, 'Shake It Off', 'Taylor Swift', '1989', 1);
INSERT INTO songs (song_id, song_name, artist_name, album_name, track_number) VALUES (2, 'Blank Space', 'Taylor Swift', '1989', 2);
INSERT INTO songs (song_id, song_name, artist_name, album_name, track_number) VALUES (3, 'Bad Blood', 'Taylor Swift', '1989', 3);
INSERT INTO songs (song_id, song_name, artist_name, album_name, track_number) VALUES (4, 'Look What You Made Me Do', 'Taylor Swift', 'Reputation', 1);
INSERT INTO songs (song_id, song_name, artist_name, album_name, track_number) VALUES (5, '...Ready for It?', 'Taylor Swift', 'Reputation', 2);
INSERT INTO songs (song_id, song_name, artist_name, album_name, track_number) VALUES (6, 'Sit Still, Look Pretty', 'Daya', 'Sit Still, Look Pretty', 1);
INSERT INTO songs (song_id, song_name, artist_name, album_name, track_number) VALUES (7, 'Hide Away', 'Daya', 'Sit Still, Look Pretty', 2);
INSERT INTO songs (song_id, song_name, artist_name, album_name, track_number) VALUES (8, 'Words', 'Daya', 'Sit Still, Look Pretty', 3);
INSERT INTO songs (song_id, song_name, artist_name, album_name, track_number) VALUES (9, 'This Love', 'Maroon 5', 'Songs About Jane', 1);
INSERT INTO songs (song_id, song_name, artist_name, album_name, track_number) VALUES (10, 'She Will Be Loved', 'Maroon 5', 'Songs About Jane', 2);
INSERT INTO songs (song_id, song_name, artist_name, album_name, track_number) VALUES (11, 'Sugar', 'Maroon 5', 'V', 1);
INSERT INTO songs (song_id, song_name, artist_name, album_name, track_number) VALUES (12, 'Animals', 'Maroon 5', 'V', 2);

*/



