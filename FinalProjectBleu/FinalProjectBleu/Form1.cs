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
using FinalProjectBleu;

namespace FinalProjectBleu
{
    public partial class AdminLoginForm : Form
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Initial Catalog=FoodOrderingDB;Integrated Security=True;";
        public static int LoggedCustomerID;

        public AdminLoginForm()
        {
            InitializeComponent();
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Customer");
        }
        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            if (cmbRole.SelectedItem == null)
            {
                lblMessage.Text = "Please select a role.";
                return;
            }

            string role = cmbRole.SelectedItem.ToString();
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (role == "Admin")
                {
                    string query = "SELECT * FROM Admins WHERE Username=@u AND Password=@p";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        this.Hide();
                        new AdminDashboard().Show();
                    }
                    else
                    {
                        lblMessage.Text = "Invalid admin credentials.";
                    }
                }
                else if (role == "Customer")
                {
                    string query = "SELECT CustomerID FROM Customers WHERE Username=@u AND Password=@p";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        int customerID = (int)dr["CustomerID"];
                        this.Hide();
                        new CustomerDashboard(customerID).Show();
                    }
                    else
                    {
                        lblMessage.Text = "Invalid customer credentials.";
                    }
                }
            }
        }
        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new RegisterForm().Show();
        }
        private void AdminLoginForm_Load(object sender, EventArgs e)
        {

        }

      
    }
}

