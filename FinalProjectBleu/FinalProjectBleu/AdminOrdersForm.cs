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
    public partial class AdminOrdersForm : Form
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Initial Catalog=FoodOrderingDB;Integrated Security=True;";
        public AdminOrdersForm()
        {
            InitializeComponent();
        }
        private void LoadOrders()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // ✅ Show orders with customer username and total
                string query = @"
            SELECT 
                o.OrderID,
                c.Username AS Customer,
                o.OrderDate,
                o.TotalAmount,
                o.Status
            FROM Orders o
            INNER JOIN Customers c ON o.CustomerID = c.CustomerID
            ORDER BY o.OrderDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvOrders.DataSource = dt;
            }
        }

        private void AdminOrdersForm_Load(object sender, EventArgs e)
        {
            LoadOrders();
        }
        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            LoadOrders();
        }
        private void btnUpdateStatus_Click_1(object sender, EventArgs e)
        {
            if (dgvOrders.CurrentRow == null)
            {
                MessageBox.Show("Please select an order to update.");
                return;
            }

            int orderId = Convert.ToInt32(dgvOrders.CurrentRow.Cells["OrderID"].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE Orders SET Status='Completed' WHERE OrderID=@id", conn);
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Order marked as completed!");
            LoadOrders();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AdminDashboard().Show();
        }
    }
}
