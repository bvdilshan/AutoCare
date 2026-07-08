using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using VehicleServiceSystem.Data;

namespace VehicleServiceSystem
{
    public partial class UC_Home : UserControl
    {
        public UC_Home()
        {
            InitializeComponent();
            this.Load += UC_Home_Load; 
        }
      
        private void UC_Home_Load(object sender, EventArgs e)
        {
            LoadDashboardData();
            LoadRecentJobs();
        }

        
        private void LoadDashboardData()
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                   
                    MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) FROM Jobs", conn);
                    lblCard1Value.Text = cmd1.ExecuteScalar().ToString();

                   
                    MySqlCommand cmd2 = new MySqlCommand("SELECT FORMAT(IFNULL(SUM(GrandTotal), 0), 2) FROM Bills", conn);
                    lblCard2Value.Text = "Rs. " + cmd2.ExecuteScalar().ToString();

                   
                    string queryTodayJobs = @"SELECT COUNT(*) FROM Jobs 
                                              WHERE BillingStatus = 'Ready for Billing' 
                                              AND DATE(CompletedAt) = CURDATE()";
                    MySqlCommand cmd3 = new MySqlCommand(queryTodayJobs, conn);
                    lblCard3Value.Text = cmd3.ExecuteScalar().ToString();

                    
                    MySqlCommand cmd4 = new MySqlCommand("SELECT COUNT(*) FROM StockItems WHERE Quantity <= ReorderLevel", conn);
                    lblCard4Value.Text = cmd4.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dashboard loading error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRecentJobs()
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    
                    string query = @"SELECT j.JobID, v.VehicleNumber, c.CustomerName, c.Phone, j.BillingStatus, j.CompletedAt 
                                     FROM jobs j 
                                     INNER JOIN vehicles v ON j.VehicleID = v.VehicleID 
                                     INNER JOIN customers c ON v.CustomerID = c.CustomerID 
                                     ORDER BY j.JobID DESC 
                                     LIMIT 10";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvRecent.DataSource = dt;

                    dgvRecent.Columns["JobID"].HeaderText = "Job ID";
                    dgvRecent.Columns["VehicleNumber"].HeaderText = "Vehicle No";
                    dgvRecent.Columns["CustomerName"].HeaderText = "Customer";
                    dgvRecent.Columns["Phone"].HeaderText = "Contact No";
                    dgvRecent.Columns["BillingStatus"].HeaderText = "Status";
                    dgvRecent.Columns["CompletedAt"].HeaderText = "Date/Time";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Recent jobs loading error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}