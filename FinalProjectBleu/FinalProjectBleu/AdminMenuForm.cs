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

namespace FinalProjectBleu
{
    public partial class AdminMenuForm : Form
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Initial Catalog=FoodOrderingDB;Integrated Security=True;";
        public AdminMenuForm()
        {
            InitializeComponent();
        }
        private void AdminMenuForm_Load(object sender, EventArgs e)
        {
            LoadMenu();
        }
        private void LoadMenu()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Menu", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvMenu.DataSource = dt;
            }
        }
        private void SearchMenu(string keyword)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Menu WHERE ItemName LIKE @kw OR Category LIKE @kw";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvMenu.DataSource = dt;
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Menu (ItemName, Category, Price) VALUES (@n, @c, @p)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@n", txtItemName.Text);
                cmd.Parameters.AddWithValue("@c", txtCategory.Text);
                cmd.Parameters.AddWithValue("@p", Convert.ToDecimal(txtPrice.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Item added successfully!");
                LoadMenu();
            }
        }
        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (dgvMenu.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvMenu.CurrentRow.Cells["ItemID"].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Menu SET ItemName=@n, Category=@c, Price=@p WHERE ItemID=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@n", txtItemName.Text);
                cmd.Parameters.AddWithValue("@c", txtCategory.Text);
                cmd.Parameters.AddWithValue("@p", Convert.ToDecimal(txtPrice.Text));
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Item updated!");
                LoadMenu();
            }
        }
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dgvMenu.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvMenu.CurrentRow.Cells["ItemID"].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Menu WHERE ItemID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Item deleted!");
                LoadMenu();
            }
        }
        private void btnBack_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new AdminDashboard().Show();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMenu.Rows[e.RowIndex];

                txtItemName.Text = row.Cells["ItemName"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
                txtCategory.Text = row.Cells["Category"].Value.ToString();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
                LoadMenu();
            else
                SearchMenu(keyword);
        }
    }
}
