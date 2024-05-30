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

namespace melodisc_a_music_app
{
    public partial class Review : Form
    {
        private OracleConnection connection;
        private OracleCommand command;
        private OracleDataReader reader;
        private string username; // Store the username
        private int userId;
        private int songId;// Store the user_id
        public Review(string username)
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

        private void Review_Load(object sender, EventArgs e)
        {

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
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //rating of song by user from 1-10
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //write the song i want to search for
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //clicking this button will seqrch the song in db and put the name and album in labels
            //if review exists alreadys for the user id, will display that and wont be chnaged
            //else post button will be true
            string songName = textBox1.Text;

            try
            {
                string query = "SELECT s.song_id, s.album_name, a.artist_name, r.rating, r.review " +
                               "FROM songs s " +
                               "INNER JOIN artists a ON s.artist_id = a.artist_id " +
                               "LEFT JOIN reviews r ON s.song_id = r.song_id AND r.user_id = :userId " +
                               "WHERE s.song_name = :songName";
                command = new OracleCommand(query, connection);
                command.Parameters.Add(new OracleParameter("userId", userId));
                command.Parameters.Add(new OracleParameter("songName", songName));

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    songId = reader.GetInt32(0);
                    label3.Text = reader.GetString(1); // Display album name
                    label2.Text = reader.GetString(2); // Display artist name

                    if (!reader.IsDBNull(3) && !reader.IsDBNull(4))
                    {
                        textBox2.Text = reader.GetInt32(3).ToString(); // Display existing rating
                        richTextBox1.Text = reader.GetString(4); // Display existing review
                        textBox2.Enabled = false;
                        richTextBox1.Enabled = false;
                        MessageBox.Show("You have already reviewed this song :D");
                        button3.Enabled = false; // Disable post button
                    }
                    else
                    {
                        textBox2.Text = "";
                        richTextBox1.Text = "";
                        textBox2.Enabled = true;
                        richTextBox1.Enabled = true;
                        button3.Enabled = true; // Enable post button
                    }
                }
                else
                {
                    MessageBox.Show("Song not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                reader.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //navigate to home
            Home home=new Home();
            home.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //post the review in reviews db
            int rating;
            if (int.TryParse(textBox2.Text, out rating) && rating >= 1 && rating <= 10)
            {
                string reviewText = richTextBox1.Text;

                try
                {
                    // Check if the review already exists before inserting
                    string checkQuery = "SELECT COUNT(*) FROM reviews WHERE user_id = :userId AND song_id = :songId";
                    command = new OracleCommand(checkQuery, connection);
                    command.Parameters.Add(new OracleParameter("userId", userId));
                    command.Parameters.Add(new OracleParameter("songId", songId));

                    int reviewCount = Convert.ToInt32(command.ExecuteScalar());

                    if (reviewCount == 0)
                    {
                        // Insert the review
                        string insertQuery = "INSERT INTO reviews (review_id, user_id, song_id, rating, review_date, review) " +
                                             "VALUES (review_id_seq.NEXTVAL, :userId, :songId, :rating, SYSDATE, :review)";
                        command = new OracleCommand(insertQuery, connection);
                        command.Parameters.Add(new OracleParameter("userId", userId));
                        command.Parameters.Add(new OracleParameter("songId", songId));
                        command.Parameters.Add(new OracleParameter("rating", rating));
                        command.Parameters.Add(new OracleParameter("review", reviewText));

                        int rowsUpdated = command.ExecuteNonQuery();

                        if (rowsUpdated > 0)
                        {
                            MessageBox.Show("Review posted successfully!");
                            textBox2.Enabled = false;
                            richTextBox1.Enabled = false;
                            
                            button3.Enabled = false; // Disable post button
                        }
                        else
                        {
                            MessageBox.Show("Failed to post the review.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("You have already reviewed this song.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid rating between 1 and 10.");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void label2_Click(object sender, EventArgs e)
        {
            //display name of artist
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //display name of album
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //reviwew by the user of the song
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
