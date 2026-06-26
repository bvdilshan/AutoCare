using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Windows.Forms.DataVisualization.Charting;
using VehicleServiceSystem.Data;

namespace VehicleServiceSystem
{
    public partial class UC_Home : UserControl
    {
        public UC_Home()
        {
            InitializeComponent();
        }
        // Load event handler for the UserControl
        private void UC_Home_Load(object sender, EventArgs e)
        {
            LoadDashboardData();

            LoadRecentJobs();
        }
        // Method to load data for the dashboard cards
        private void LoadDashboardData()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                lblCard1Value.Text = ExecuteScalar(conn,
                    "SELECT COUNT(*) FROM Jobs").ToString();

                lblCard2Value.Text = ExecuteScalar(conn,
                    "SELECT IFNULL(SUM(GrandTotal),0) FROM Bills").ToString();

                lblCard3Value.Text = ExecuteScalar(conn, @"
                    SELECT COUNT(*) 
                    FROM Jobs j
                    INNER JOIN JobStatus s ON j.StatusID = s.StatusID
                    WHERE s.StatusName = 'Completed'
                    AND DATE(j.ServiceDate) = CURDATE()").ToString();

                lblCard4Value.Text = ExecuteScalar(conn, @"
                    SELECT COUNT(*) 
                    FROM StockItems 
                    WHERE Quantity <= ReorderLevel").ToString();
            }
        }
        // Helper method to execute scalar queries
        private object ExecuteScalar(MySqlConnection conn, string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);
            return cmd.ExecuteScalar();
        }

        // Method to load recent jobs into the DataGridView
        private void LoadRecentJobs()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        j.JobID,
                        c.CustomerName,
                        v.VehicleNumber,
                        s.ServiceName,
                        js.StatusName,
                        j.ServiceDate
                    FROM Jobs j
                    INNER JOIN Vehicles v ON j.VehicleID = v.VehicleID
                    INNER JOIN Customers c ON v.CustomerID = c.CustomerID
                    INNER JOIN ServiceTypes s ON j.ServiceTypeID = s.ServiceTypeID
                    INNER JOIN JobStatus js ON j.StatusID = js.StatusID
                    ORDER BY j.ServiceDate DESC
                    LIMIT 10";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvRecent.DataSource = dt;
            }
        }

        private void pnlCard1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {

        }
    }
}