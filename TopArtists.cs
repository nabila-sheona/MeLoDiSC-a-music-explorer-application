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
    public partial class TopArtists : Form
    {
        private OracleConnection connection;
        public TopArtists()
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

        private void TopArtists_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //top album button
            //top album button
            if (comboBox3.SelectedItem == null)
            {
                MessageBox.Show("Please select a criteria.");
                return;
            }

            string criteria = comboBox3.SelectedItem.ToString();
            if (int.TryParse(textBox3.Text.Trim(), out int topN))
            {
                LoadTopAlbums(criteria, topN);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }

        }

        private void LoadTopAlbums(string criteria, int topN)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            try
            {
                OracleCommand cmd = new OracleCommand("GetTopAlbums", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("criteria", OracleDbType.Varchar2).Value = criteria;
                cmd.Parameters.Add("topN", OracleDbType.Int32).Value = topN;
                cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                OracleDataReader reader = cmd.ExecuteReader();
                StringBuilder resultStringBuilder = new StringBuilder();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        resultStringBuilder.Append(reader[i].ToString());
                        if (i < reader.FieldCount - 1)
                            resultStringBuilder.Append(",");
                    }
                    resultStringBuilder.Append(";");
                }

                reader.Close();
                string resultString = resultStringBuilder.ToString();
                PopulateDataGridView(resultString, dataGridView1, new string[] { "Album Name", "Artist Name", "Release Date" });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing query: {ex.Message}");
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //top artist rating button
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a gender.");
                return;
            }

            string gender = comboBox1.SelectedItem.ToString();
            if (int.TryParse(textBox1.Text.Trim(), out int topN))
            {
                LoadTopVocalists(gender, topN);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }

        }

        private void LoadTopVocalists(string gender, int topN)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            try
            {
                OracleCommand command = new OracleCommand("GetTopVocalists", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Define the input parameters
                command.Parameters.Add("p_gender", OracleDbType.Varchar2).Value = gender;
                command.Parameters.Add("p_topN", OracleDbType.Int32).Value = topN;

                // Define the output cursor parameter
                command.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                OracleDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //top artist released song button
            if (int.TryParse(textBox2.Text.Trim(), out int topN))
            {
                LoadTopByMostReleased(topN);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }



        }

        private void LoadTopByMostReleased(int topN)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            try
            {
                OracleCommand cmd = new OracleCommand("GetTopArtistsByReleasedSongs", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("topN", OracleDbType.Int32).Value = topN;
                cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                OracleDataReader reader = cmd.ExecuteReader();
                StringBuilder resultStringBuilder = new StringBuilder();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        resultStringBuilder.Append(reader[i].ToString());
                        if (i < reader.FieldCount - 1)
                            resultStringBuilder.Append(",");
                    }
                    resultStringBuilder.Append(";");
                }

                reader.Close();
                string resultString = resultStringBuilder.ToString();
                PopulateDataGridView(resultString, dataGridView1, new string[] { "Artist Name", "Total Songs" });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing query: {ex.Message}");
            }
        }


        private string ExecuteQuery(string query)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(query, connection);
                OracleDataReader reader = cmd.ExecuteReader();
                StringBuilder resultStringBuilder = new StringBuilder();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        resultStringBuilder.Append(reader[i].ToString());
                        if (i < reader.FieldCount - 1)
                            resultStringBuilder.Append(",");
                    }
                    resultStringBuilder.Append(";");
                }

                reader.Close();
                return resultStringBuilder.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing query: {ex.Message}\nQuery: {query}");
                return null;
            }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //top album
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //top artist rating
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //top artist released songs
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //top albums
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //top artist rating
        }



        private void PopulateDataGridView(string resultString, DataGridView dataGridView, string[] columnHeaders)
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            // Add column headers
            foreach (var header in columnHeaders)
            {
                dataGridView.Columns.Add(header, header);
            }

            if (string.IsNullOrEmpty(resultString))
            {
                return;
            }

            string[] rows = resultString.Split(';');
            foreach (string row in rows)
            {
                if (string.IsNullOrWhiteSpace(row)) continue;
                string[] columns = row.Split(',');
                dataGridView.Rows.Add(columns);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
