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

namespace melodisc_a_music_app
{
    public partial class AddAlbum : Form
    {
        private OracleConnection connection;

        public AddAlbum()
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

        private void button1_Click(object sender, EventArgs e)
        {
            AdminForm home = new AdminForm();
            home.Show();
            this.Hide();
        }

        private void AddAlbum_Load(object sender, EventArgs e)
        {
            
        }
        private void LoadUsers()
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string albumName = textBox1.Text;
            string artistName = textBox2.Text;
            DateTime releaseDate = dateTimePicker1.Value;

            try
            {
                string artistQuery = "SELECT artist_id FROM artists WHERE artist_name = :artistName";
                OracleCommand artistCmd = new OracleCommand(artistQuery, connection);
                artistCmd.Parameters.Add(new OracleParameter("artistName", artistName));
                int artistId;
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



                string insertQuery = "INSERT INTO albums (album_name, artist_id, release_date) " +
                             "VALUES (:albumName, :artistId, :releaseDate)";
                OracleCommand insertCmd = new OracleCommand(insertQuery, connection);
                insertCmd.Parameters.Add(new OracleParameter("albumName", albumName));
                insertCmd.Parameters.Add(new OracleParameter("artistId", artistId));
                insertCmd.Parameters.Add(new OracleParameter("releaseDate", releaseDate));


                //incrementing no of albums of artist whose new album was added
                string updateArtistQuery = "UPDATE artists SET no_of_albums = no_of_albums + 1 WHERE artist_name = :artistName";
                OracleCommand updateArtistCmd = new OracleCommand(updateArtistQuery, connection);
                updateArtistCmd.Parameters.Add(new OracleParameter("artistName", artistName));



                int rowsAffected = insertCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Album added successfully.");
                    AdminForm f1 = new AdminForm();
                    f1.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Error adding album.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting album: " + ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
