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
            //shows the list of songs by the artist from songs table
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //shows the list of albums by the artist from artist table
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //inserts name of the artist we are looking for

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
            string query = @"
        SELECT s.song_name AS ""Song Name"", s.album_name AS ""Album Name"", s.song_number AS ""Song Number"", g.genre_name AS ""Genre"", s.release_date AS ""Release Date""
        FROM songs s
        JOIN artists ar ON s.artist_id = ar.artist_id
        JOIN genres g ON s.genre_id = g.genre_id
        WHERE ar.artist_name = :artistName";

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
            string query = @"
                SELECT al.album_name AS ""Album Name"", al.release_date AS ""Release Date""
                FROM albums al
                JOIN artists ar ON al.artist_id = ar.artist_id
                WHERE ar.artist_name = :artistName";

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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Artistprofile_Load(object sender, EventArgs e)
        {

        }
    }
    }





