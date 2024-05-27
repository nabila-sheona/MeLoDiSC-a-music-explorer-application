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

        private List<int> songnumber = new List<int>();
        private List<string> artist = new List<string>();
        private List<string> name = new List<string>();
        private List<string> album = new List<string>();

        public AddSong()
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

        private void button1_Click(object sender, EventArgs e)
        {
            AdminForm home = new AdminForm();
            home.Show();
            this.Hide();
        }

        private void AddSong_Load(object sender, EventArgs e)
        {
            string query = "SELECT username FROM users";
            OracleCommand cmd = new OracleCommand(query, connection);
            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                name.Add(reader.GetString(0));
            }
            reader.Close();
            
            LoadUsers();
        }
        private void LoadUsers()
        {
            string query = "SELECT * FROM songs";
            OracleCommand cmd = new OracleCommand(query, connection);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name.Add(reader.GetString(0));
                    artist.Add(reader.GetString(1));
                    album.Add(reader.GetString(2));
                    //SoapYear.Add(reader.GetString(3));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
