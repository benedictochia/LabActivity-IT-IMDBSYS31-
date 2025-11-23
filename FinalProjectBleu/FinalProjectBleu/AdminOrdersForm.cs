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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pending orders: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCompletedOrders()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading completed orders: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToggleOrderStatus(int orderId, string currentStatus)
        {
            string newStatus = currentStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase) ? "Pending" : "Completed";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Orders SET Status=@newStatus WHERE OrderID=@id", conn);
                    cmd.Parameters.AddWithValue("@newStatus", newStatus);
                    cmd.Parameters.AddWithValue("@id", orderId);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Order {orderId} status successfully changed from {currentStatus} to {newStatus}.", "Status Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadPendingOrders();
                    LoadCompletedOrders();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error during status update: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateStatus_Click_1(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = null;
            if (dgvPendingOrders.CurrentRow != null)
            {
                selectedRow = dgvPendingOrders.CurrentRow;
            }
            else if (dgvCompletedOrders.CurrentRow != null)
            {
                selectedRow = dgvCompletedOrders.CurrentRow;
            }

            if (selectedRow == null)
            {
                MessageBox.Show("Please select an order to update its status from either the Pending or Completed lists.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int orderId = Convert.ToInt32(selectedRow.Cells["OrderID"].Value);
            string currentStatus = selectedRow.Cells["Status"].Value.ToString();

            string nextStatus = currentStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase) ? "Pending" : "Completed";

            DialogResult result = MessageBox.Show(
                $"Toggle status for Order ID {orderId}? \n\n" +
                $"Current Status: {currentStatus}\n" +
                $"New Status will be: {nextStatus}",
                "Confirm Status Toggle",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ToggleOrderStatus(orderId, currentStatus);
            }
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