using System;
using System.Data;
using System.Data.SqlClient;
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

        private void AdminOrdersForm_Load(object sender, EventArgs e)
        {
            LoadPendingOrders();
            LoadCompletedOrders();
        }

    
        private void LoadPendingOrders()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                SELECT o.OrderID, c.Username AS Customer, o.OrderDate, o.TotalAmount, o.Status
                FROM Orders o
                INNER JOIN Customers c ON o.CustomerID = c.CustomerID
                WHERE o.Status = 'Pending'
                ORDER BY o.OrderDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPendingOrders.DataSource = dt;
            }
        }

       
        private void LoadCompletedOrders()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                SELECT o.OrderID, c.Username AS Customer, o.OrderDate, o.TotalAmount, o.Status
                FROM Orders o
                INNER JOIN Customers c ON o.CustomerID = c.CustomerID
                WHERE o.Status = 'Completed'
                ORDER BY o.OrderDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCompletedOrders.DataSource = dt;
            }
        }

      
        private void btnUpdateStatus_Click_1(object sender, EventArgs e)
        {
            if (dgvPendingOrders.CurrentRow == null)
            {
                MessageBox.Show("Please select a pending order to update.");
                return;
            }

            int orderId = Convert.ToInt32(dgvPendingOrders.CurrentRow.Cells["OrderID"].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Orders SET Status='Completed' WHERE OrderID=@id", conn);
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Order marked as completed!");
            LoadPendingOrders();
            LoadCompletedOrders();
        }

       
        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            LoadPendingOrders();
            LoadCompletedOrders();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AdminDashboard().Show();
        }
    }
}
