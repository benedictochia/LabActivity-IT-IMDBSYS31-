﻿using System;
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
    public partial class CustomerOrderForm : Form
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Initial Catalog=FoodOrderingDB;Integrated Security=True;";
        private DataTable cartTable = new DataTable();
        private int currentCustomerID;
        public CustomerOrderForm(int customerID)
        {
            InitializeComponent();
            currentCustomerID = customerID;
        }

        private void CustomerOrderForm_Load(object sender, EventArgs e)
        {
            LoadMenu();
            SetupCartTable();
        }
        private void LoadMenu()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT ItemID, ItemName, Category, Price FROM Menu", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvMenu.DataSource = dt;
            }
        }
        private void SetupCartTable()
        {
            cartTable.Columns.Add("ItemID", typeof(int));
            cartTable.Columns.Add("ItemName", typeof(string));
            cartTable.Columns.Add("Price", typeof(decimal));
            cartTable.Columns.Add("Quantity", typeof(int));
            cartTable.Columns.Add("Subtotal", typeof(decimal));

            dgvCart.DataSource = cartTable;
        }
        private void btnAddToCart_Click_1(object sender, EventArgs e)
        {
            if (dgvMenu.CurrentRow == null)
            {
                MessageBox.Show("Select an item to add.");
                return;
            }

            int itemId = Convert.ToInt32(dgvMenu.CurrentRow.Cells["ItemID"].Value);
            string itemName = dgvMenu.CurrentRow.Cells["ItemName"].Value.ToString();
            decimal price = Convert.ToDecimal(dgvMenu.CurrentRow.Cells["Price"].Value);

            foreach (DataRow row in cartTable.Rows)
            {
                if ((int)row["ItemID"] == itemId)
                {
                    row["Quantity"] = (int)row["Quantity"] + 1;
                    row["Subtotal"] = (decimal)row["Price"] * (int)row["Quantity"];
                    UpdateTotal();
                    return;
                }
            }

            cartTable.Rows.Add(itemId, itemName, price, 1, price);
            UpdateTotal();
        }

        private void btnRemoveItem_Click_1(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow != null)
            {
                dgvCart.Rows.RemoveAt(dgvCart.CurrentRow.Index);
                UpdateTotal();
            }
        }

        private void UpdateTotal()
        {
            decimal total = 0;
            foreach (DataRow row in cartTable.Rows)
            {
                total += (decimal)row["Subtotal"];
            }
            lblTotal.Text = "Total: ₱" + total.ToString("0.00");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cartTable.Rows.Count == 0)
            {
                MessageBox.Show("Your cart is empty!");
                return;
            }

            decimal total = 0;
            foreach (DataRow row in cartTable.Rows)
            {
                total += (decimal)row["Subtotal"];
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // ✅ Make sure currentCustomerID is valid
                if (currentCustomerID <= 0)
                {
                    MessageBox.Show("Invalid customer session. Please log in again.");
                    return;
                }

                // ✅ Insert into Orders table
                string orderQuery = "INSERT INTO Orders (CustomerID, TotalAmount, OrderDate) OUTPUT INSERTED.OrderID VALUES (@cid, @total, GETDATE())";
                SqlCommand cmdOrder = new SqlCommand(orderQuery, conn);
                cmdOrder.Parameters.AddWithValue("@cid", currentCustomerID);
                cmdOrder.Parameters.AddWithValue("@total", total);

                int orderId = (int)cmdOrder.ExecuteScalar();

                // ✅ Insert each order item
                foreach (DataRow row in cartTable.Rows)
                {
                    string itemQuery = "INSERT INTO OrderItems (OrderID, ItemID, Quantity, Subtotal) VALUES (@oid, @iid, @q, @s)";
                    SqlCommand cmdItem = new SqlCommand(itemQuery, conn);
                    cmdItem.Parameters.AddWithValue("@oid", orderId);
                    cmdItem.Parameters.AddWithValue("@iid", (int)row["ItemID"]);
                    cmdItem.Parameters.AddWithValue("@q", (int)row["Quantity"]);
                    cmdItem.Parameters.AddWithValue("@s", (decimal)row["Subtotal"]);
                    cmdItem.ExecuteNonQuery();
                }

                MessageBox.Show("Order placed successfully!");
                cartTable.Clear();
                UpdateTotal();
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void returnToDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            new CustomerDashboard(currentCustomerID).Show();
        }

        
    }
}
