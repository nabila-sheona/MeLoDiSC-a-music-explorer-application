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

namespace melodisc_a_music_app
{
    public partial class Explorer : Form
    {
        private OracleConnection connection;
        private string username; // Store the username
        private int userId;
        public Explorer(string username)
        {
            this.username = username;
            InitializeComponent();          
            connection = new OracleConnection("User Id=melodisc1;Password=melodisc1;Data Source=localhost:1521");
            try
            {
                connection.Open();
                LoadUserId();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message);
            }
        }



        private void LoadUserId()
        {
            string query = "SELECT user_id FROM users WHERE username = :username";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter("username", username));

            try
            {
                userId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching user ID: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Home f2 = new Home();
            f2.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //for songs 
            string criteria = comboBox1.SelectedItem.ToString();
            LoadSongs(criteria);
        }

        private void LoadSongs(string criteria)
        {
            string query;
            if (criteria == "Alphabetically")
            {
                query = "SELECT * FROM songs ORDER BY song_name";
            }
            else if (criteria == "Genre-based")
            {
                query = "SELECT * FROM songs ORDER BY genre_id";
            }
            else if (criteria == "Release Date")
            {
                query = "SELECT * FROM songs ORDER BY release_date";
            }
            else
            {
                query = "SELECT * FROM songs ORDER BY song_name";
            }

            ExecuteQueryAndPopulateDataGridView(query, dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //for albums
            string criteria = comboBox2.SelectedItem.ToString();
            LoadAlbums(criteria);
        }

        private void LoadAlbums(string criteria)
        {
            string query;
            if (criteria == "Alphabetically")
            {
                query = "SELECT * FROM albums ORDER BY album_name";
            }
            else if (criteria == "Release Date")
            {
                query = "SELECT * FROM albums ORDER BY release_date";
            }
            else
            {
                query = "SELECT * FROM albums ORDER BY album_name";
            }

            ExecuteQueryAndPopulateDataGridView(query, dataGridView1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //for playlists
            string criteria = comboBox3.SelectedItem.ToString();
            LoadPlaylists(criteria);
        }


        private void LoadPlaylists(string criteria)
        {
            string query;
            if (criteria == "Alphabetically")
            {
                query = "SELECT * FROM playlists ORDER BY playlist_title WHERE user_id = :userId";
            }
            else if ( criteria =="Release Date")
            {
                query = "SELECT * FROM playlists ORDER BY release_date WHERE user_id = :userId";
            }
            else
            {
                query = "SELECT * FROM playlists ORDER BY playlist_title WHERE user_id = :userId";
            }

            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter("userId", userId));

            try
            {
                OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading playlists: " + ex.Message);
            }
        }


        private void ExecuteQueryAndPopulateDataGridView(string query, DataGridView dataGridView)
        {
            try
            {

                OracleDataAdapter dataAdapter = new OracleDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing query: " + ex.Message);
            }
        }

    }
}
