using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;


namespace melodisc_a_music_app
{
    public partial class Form1 : Form
    {
        private OracleConnection connection;
        public static Form1 instance;
        public string usern;
        private List<string> user = new List<string>();
        private List<string> pass = new List<string>();
        private List<string> name = new List<string>();
        public Form1()
        {
            InitializeComponent();
            instance = this;
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
        private void LoadUsers()
        {
            string query = "SELECT name, username, password FROM users";
            OracleCommand cmd = new OracleCommand(query, connection);
            try
            {
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name.Add(reader.GetString(0));
                    user.Add(reader.GetString(1));
                    pass.Add(reader.GetString(2));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
           




    

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            usern = textBox1.Text.Trim();
            string password = textBox2.Text; // No trimming here, passwords might have leading/trailing spaces

            Console.WriteLine($"Login Attempt: Username = '{usern}', Password = '{password}'");

            // Case-insensitive comparison for usernames
            int userIndex = user.FindIndex(u => u.Equals(usern, StringComparison.OrdinalIgnoreCase));

            if (userIndex >= 0 && pass.Count > userIndex && pass[userIndex] == password)
            {
                MessageBox.Show("The username/password is correct");
                // Navigate to another form or functionality as required

                // Clear text boxes for the next login attempt
                textBox1.Text = "";
                textBox2.Text = "";

                Home home = new Home();
                home.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("The username/password is incorrect");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SignUp signUpForm = new SignUp();
            signUpForm.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminLogin();
        }

        private void AdminLogin()
        {
            string adminId = Prompt.ShowDialog("Enter Admin ID:", "Admin Login");
            string password = Prompt.ShowDialog("Enter Password:", "Admin Login");

            if (ValidateAdminCredentials(adminId, password))
            {
                MessageBox.Show("Admin login successful");
                AdminForm adminForm = new AdminForm();
                adminForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Admin ID or Password");
            }
        }

        private bool ValidateAdminCredentials(string adminId, string password)
        {
            string query = "SELECT COUNT(*) FROM admins WHERE admin_id = :adminId AND password = :password";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.Parameters.Add(new OracleParameter("adminId", adminId));
            cmd.Parameters.Add(new OracleParameter("password", password));

            try
            {
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error validating admin credentials: " + ex.Message);
                return false;
            }
        }


    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }

}






//CREATE TABLE admins (
  //  admin_id VARCHAR2(50) PRIMARY KEY,
    //password VARCHAR2(50) NOT NULL
//);

//INSERT INTO admins (admin_id, password) VALUES('sheona', 'sheona');
//INSERT INTO admins (admin_id, password) VALUES('moumita', 'moumita');
//INSERT INTO admins (admin_id, password) VALUES('faiza', 'faiza');
