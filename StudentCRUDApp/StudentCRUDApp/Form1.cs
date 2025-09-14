using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentCRUDApp
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Initial Catalog=StudentDB;Integrated Security=True;";
        public Form1()
        {
            InitializeComponent();
        }
        private void LoadStudents()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Students", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int studentID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["StudentID"].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Students WHERE StudentID=@StudentID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentID", studentID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Student deleted successfully!");
                    LoadStudents();
                }
            }
            else
            {
                MessageBox.Show("Please select a student to delete.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtbFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtbLastName.Text = row.Cells["LastName"].Value.ToString();
                txtbAge.Text = row.Cells["Age"].Value.ToString();
                txtbCourse.Text = row.Cells["Course"].Value.ToString();
            }
        }

        private void txtbLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Students (FirstName, LastName, Age, Course) VALUES (@FirstName, @LastName, @Age, @Course)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", txtbFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", txtbLastName.Text);
                cmd.Parameters.AddWithValue("@Age", int.Parse(txtbAge.Text));
                cmd.Parameters.AddWithValue("@Course", txtbCourse.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Student added successfully!");
                LoadStudents();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int studentID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["StudentID"].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Students SET FirstName=@FirstName, LastName=@LastName, Age=@Age, Course=@Course WHERE StudentID=@StudentID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FirstName", txtbFirstName.Text);
                    cmd.Parameters.AddWithValue("@LastName", txtbLastName.Text);
                    cmd.Parameters.AddWithValue("@Age", int.Parse(txtbAge.Text));
                    cmd.Parameters.AddWithValue("@Course", txtbCourse.Text);
                    cmd.Parameters.AddWithValue("@StudentID", studentID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Student updated successfully!");
                    LoadStudents();
                }
            }
            else
            {
                MessageBox.Show("Please select a student to edit.");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadStudents();
        }
    }
}
