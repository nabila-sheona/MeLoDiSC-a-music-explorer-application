using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace melodisc_a_music_app
{
    public partial class Browse : Form
    {

        private OracleConnection connection;
       
        public Browse()
        {
            InitializeComponent();
            connection = new OracleConnection("User Id=melodisc1;Password=melodisc1;Data Source=localhost:1521");
            try
            {
                connection.Open();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //shows the users informations
        }
      
        private void Browse_Load(object sender, EventArgs e)
        {
            LoadUsers();

        }


        private void LoadUsers()
        {
            try
            {
                string query;

                if (comboBox1.SelectedItem != null)
                {
                    switch (comboBox1.SelectedItem.ToString())
                    {
                        case "Alphabetical":
                            query = "SELECT name, username, user_id, email, gender FROM users ORDER BY name ASC";
                            break;
                        case "Gender":
                            query = "SELECT gender, name, username, user_id, email FROM users ORDER BY gender ASC, name ASC";
                            break;
                        case "User ID":
                            query = "SELECT user_id, name, username, email, gender FROM users ORDER BY user_id ASC";
                            break;
                        default:
                            query = "SELECT user_id, name, username, email, gender FROM users ORDER BY user_id ASC";
                            break;
                    }
                }
                else
                {
                    // Default query if no selection is made
                    query = "SELECT user_id, name, username, email, gender FROM users ORDER BY user_id ASC";
                }

                OracleCommand cmd = new OracleCommand(query, connection);
                OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridViewUsers.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }
    }
}
