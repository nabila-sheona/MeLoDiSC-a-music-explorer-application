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
    public partial class PlaylistCreation : Form
    {
        private OracleConnection connection;
        private string username;
        private int userId;
        public PlaylistCreation(string username)
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

        private void PlaylistCreation_Load(object sender, EventArgs e)
        {
            LoadSongs();
            LoadPlaylists();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Home f2 = new Home();
            f2.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string playlistTitle = textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(playlistTitle))
            {
                CreatePlaylist(playlistTitle);
            }
            else
            {
                MessageBox.Show("Please enter a playlist title.");
            }
        }


        private void CreatePlaylist(string playlistTitle)
        {
            string query = "INSERT INTO playlists (playlist_id, user_id, playlist_title, release_date) VALUES (playlist_id_seq.NEXTVAL, :user_id, :playlist_title, :release_date)";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter("user_id", userId));
            cmd.Parameters.Add(new OracleParameter("playlist_title", playlistTitle));
            cmd.Parameters.Add(new OracleParameter("release_date", DateTime.Now));

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Playlist created successfully.");
                LoadPlaylists();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating playlist: " + ex.Message);
            }
        }


        private void LoadPlaylists()
        {
            string query = "SELECT playlist_id, playlist_title FROM playlists WHERE user_id = :user_id";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter("user_id", userId));

            try
            {
                OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                listBox1.DataSource = dataTable;
                listBox1.DisplayMember = "playlist_title";
                listBox1.ValueMember = "playlist_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading playlists: " + ex.Message);
            }
        }


        private void checkedListBoxSongs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int playlistId = (int)listBox1.SelectedValue;
            int songId = Convert.ToInt32(((DataRowView)checkedListBox1.Items[e.Index]).Row["song_id"]); 
            if (e.NewValue == CheckState.Checked)
            {
                AddSongToPlaylist(playlistId, songId);
            }
           
        }

        private void AddSongToPlaylist(int playlistId, int songId)
        {
            string query = "INSERT INTO playlist_song (playlist_id, song_id) VALUES (:playlist_id, :song_id)";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter("playlist_id", playlistId));
            cmd.Parameters.Add(new OracleParameter("song_id", songId));

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding song to playlist: " + ex.Message);
            }
        }


        private void ShowPlaylistSongs(int playlistId)
        {
            string query = @"
                SELECT s.song_name
                FROM playlist_song ps
                JOIN songs s ON ps.song_id = s.song_id
                WHERE ps.playlist_id = :playlist_id";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter("playlist_id", playlistId));

            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                listBox1.Items.Clear();
                int songNumber = 1;
                while (reader.Read())
                {
                    listBox1.Items.Add($"{songNumber}. {reader["song_name"].ToString()}");
                    songNumber++;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading playlist songs: " + ex.Message);
            }
        }


        private void LoadSongs()
        {
            string query = "SELECT song_id, song_name, album_name FROM songs";
            OracleCommand cmd = new OracleCommand(query, connection);

            try
            {
                OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                checkedListBox1.DataSource = dataTable;
                checkedListBox1.DisplayMember = "song_name";
                checkedListBox1.ValueMember = "song_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading songs: " + ex.Message);
            }
        }
    }
}
