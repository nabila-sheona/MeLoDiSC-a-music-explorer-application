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
            string criteria = comboBox3.SelectedItem.ToString();
            int topN = int.Parse(textBox3.Text.Trim());
            LoadTopAlbums(criteria, topN);
        }


        private void LoadTopAlbums(string criteria, int topN)
        {
            string resultString = CallFunction("get_top_albums", new OracleParameter("criteria", criteria), new OracleParameter("topN", topN));
            PopulateDataGridView(resultString, dataGridView1, new string[] { "Album Name", "Artist Name", "Release Date" });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //top artist rating button
            string gender = comboBox1.SelectedItem.ToString();
            int topN = int.Parse(textBox1.Text.Trim());
            LoadTopVocalists(gender, topN);

        }

        private void LoadTopVocalists(string gender, int topN)
        {
            string resultString = CallFunction("get_top_artists_by_rating", new OracleParameter("gender", gender), new OracleParameter("topN", topN));
            PopulateDataGridView(resultString, dataGridView1, new string[] { "Artist Name", "Average Rating" });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //top artist released song button

            int topN = int.Parse(textBox2.Text.Trim());
            LoadTopByMostReleased(topN);

        }


        private void LoadTopByMostReleased(int topN)
        {
            string resultString = CallFunction("get_top_artists_by_most_released", new OracleParameter("topN", topN));
            PopulateDataGridView(resultString, dataGridView1, new string[] { "Artist Name", "Total Songs" });
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




        private string CallFunction(string functionName, params OracleParameter[] parameters)
        {
            string query = $"BEGIN :result := {functionName}(:params); END;";
            OracleCommand cmd = new OracleCommand(query, connection);
            cmd.CommandType = CommandType.Text;

            OracleParameter resultParam = new OracleParameter("result", OracleDbType.Varchar2, 32767);
            resultParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(resultParam);

            foreach (var param in parameters)
            {
                cmd.Parameters.Add(param);
            }

            try
            {
                cmd.ExecuteNonQuery();
                return resultParam.Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calling {functionName}: " + ex.Message);
                return null;
            }
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
