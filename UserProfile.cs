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
    public partial class UserProfile : Form
    {
        private OracleConnection connection;
        private string user_name;

        public UserProfile(string username)
        {
            InitializeComponent();
            this.user_name = username;
            connection = new OracleConnection("User Id=melodisc1;Password=melodisc1;Data Source=localhost:1521");
            try
            {
                connection.Open();
                LoadUserProfile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();

        }


        private void LoadUserProfile()
        {
            
            LoadUserDetails();
            LoadPlaylists();
        }


        private void LoadUserDetails()
        {
            string query = @"
                SELECT name, username, email, phone, gender 
                FROM users 
                WHERE username = :username";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter("username", user_name));

            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    textBox1.Text = reader["name"].ToString();
                    textBox2.Text = reader["username"].ToString();
                    textBox3.Text = reader["email"].ToString();
                    textBox4.Text = reader["phone"].ToString();
                    textBox5.Text = reader["gender"].ToString();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user details: " + ex.Message);
            }
        }

        private void LoadPlaylists()
        {
            string query = @"
                SELECT p.playlist_title AS ""Playlist Name"", p.release_date AS ""Release Date"" 
                FROM playlists p
                JOIN users u ON p.user_id = u.user_id
                WHERE u.username = :username";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter("username", user_name));

            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridViewUsers.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading playlists: " + ex.Message);
            }
        }



        private void label1_Click(object sender, EventArgs e)
        {
            //it shows the name of the user
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //it shows the contents or information of user
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //it shows the playlists user has made
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string newPassword = Prompt.ShowDialog("Enter new password:", "Change Password");
            if (!string.IsNullOrEmpty(newPassword) && newPassword.Length >= 6)
            {
                ChangePassword(newPassword);
            }

            else
            {
                MessageBox.Show("Password has to be 6 characters or more");
            }
        }


        private void ChangePassword(string newPassword)
        {
            string query = "UPDATE users SET password = :newPassword WHERE username = :username";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter("newPassword", newPassword));
            cmd.Parameters.Add(new OracleParameter("username", user_name));

            try
            {
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Password changed successfully.");
                }
                else
                {
                    MessageBox.Show("Error changing password.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error changing password: " + ex.Message);
            }
        }

        private void UserProfile_Load(object sender, EventArgs e)
        {

        }

        private void dataGridViewUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
