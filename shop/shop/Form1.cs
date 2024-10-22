using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace shop
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Server=DESKTOP-CPRJ6ND;Database=SalesDB;User Id=sa;Password=123;";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string customerName = textBox2.Text;
            decimal totalAmount;

            if (string.IsNullOrEmpty(customerName))
            {
                MessageBox.Show("Customer Name cannot be empty!");
                return;
            }
            if (!decimal.TryParse(textBox3.Text, out totalAmount))
            {
                MessageBox.Show("Invalid Total Amount!");
                return;
            }

            SaveInvoice(customerName, totalAmount);
        }

        private void SaveInvoice(string customerName, decimal totalAmount)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(
                        "INSERT INTO SalesInvoice (CustomerName, InvoiceDate, TotalAmount) " +
                        "VALUES (@CustomerName, GETDATE(), @TotalAmount)", connection))
                    {
                        command.Parameters.AddWithValue("@CustomerName", customerName);
                        command.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Data inserted successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        // Handle text changes in textBox4 for search functionality
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox4.Text;

            if (!string.IsNullOrEmpty(searchText))
            {
                SearchInvoice(searchText);
            }
        }

        private void SearchInvoice(string searchText)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT CustomerName, TotalAmount, InvoiceDate FROM SalesInvoice WHERE CustomerName LIKE @searchText";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            textBox2.Text = reader["CustomerName"].ToString();
                            textBox3.Text = reader["TotalAmount"].ToString();
                            dateTimePicker1.Value = Convert.ToDateTime(reader["InvoiceDate"]);
                        }
                        else
                        {
                            // Clear fields if no matching data found
                            textBox2.Text = "";
                            textBox3.Text = "";
                            dateTimePicker1.Value = DateTime.Now;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }
}
