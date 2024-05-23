using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace melodisc_a_music_app
{
    public partial class SignUp : Form
    {
        private OracleConnection connection;
        private List<string> user = new List<string>();
        public SignUp()
        {
            InitializeComponent();
            connection = new OracleConnection("User Id=melodisc1;Password=melodisc1;Data Source=localhost:1521");
            try
            {
                connection.Open();
                LoadUsernames();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message);
            }
        }

        private void LoadUsernames()
        {
            string query = "SELECT username FROM users";
            OracleCommand cmd = new OracleCommand(query, connection);
            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                user.Add(reader.GetString(0));
            }
            reader.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string phone = textBox2.Text;
            string userName = textBox6.Text;
            string name = textBox1.Text;
            string pass = textBox7.Text;
            string email = textBox3.Text;
            string gender = comboBox1.Text;

            if (userName.Length >= 4 && pass.Length >= 6 && phone.Length > 11)
            {
                if (!user.Contains(userName))
                {
                    try
                    {
                        string query = "INSERT INTO users (name, username, password, phone, email, gender) VALUES (:name, :username, :password, :phone, :email, :gender)";
                        OracleCommand cmd = new OracleCommand(query, connection);
                        cmd.Parameters.Add(new OracleParameter("name", name));
                        cmd.Parameters.Add(new OracleParameter("username", userName));
                        cmd.Parameters.Add(new OracleParameter("password", pass));
                        cmd.Parameters.Add(new OracleParameter("phone", phone));
                        cmd.Parameters.Add(new OracleParameter("email", email));
                        cmd.Parameters.Add(new OracleParameter("gender", gender));
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("User created successfully!");

                        Form1 f1 = new Form1();
                        f1.Show();
                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error inserting into database: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Username not available!");
                }
            }
            else
            {
                MessageBox.Show("Username must be at least 4 characters, password at least 6 characters, and phone number must be at least 12 characters.");
            }
        }

        private void SignUp_Load(object sender, EventArgs e)
        {

            string query = "SELECT username FROM users";
            OracleCommand cmd = new OracleCommand(query, connection);
            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                user.Add(reader.GetString(0));
            }
            reader.Close();
            StreamReader s1 = new StreamReader(@"D:\sophomore\2-2\testmelo\bin\user.txt");
            string line = "";
            while ((line = s1.ReadLine()) != null)
            {


                string[] components = line.Split(" ".ToCharArray());

                user.Add(components[2]);



            }




            s1.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
