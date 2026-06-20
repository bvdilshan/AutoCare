using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using VehicleServiceSystem.Data;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

namespace VehicleServiceSystem.UserControls
{
    public partial class UC_Reports : UserControl
    {
        public UC_Reports()
        {
            InitializeComponent();

            btnGenerate.Click += btnGenerate_Click;
            btnExportExcel.Click += btnExportExcel_Click;
            btnExportPdf.Click += btnExportPdf_Click;
        }
        // Load event handler to initialize report types and default dates
        private void UC_Reports_Load(object sender, EventArgs e)
        {
            if (cmbReportType.Items.Count > 0)
                cmbReportType.SelectedIndex = 0;

            dtpFromDate.Value = DateTime.Today.AddDays(-30);
            dtpToDate.Value = DateTime.Today;

            LoadSummary();
            GenerateReport();
        }
        // Event handler for the Generate Report button click
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateReport();
            LoadSummary();
        }
        // Method to load summary statistics into the labels
        private void LoadSummary()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                lblSummary1Value.Text = ExecuteScalar(conn,
                    "SELECT COUNT(*) FROM Jobs").ToString();

                lblSummary2Value.Text = ExecuteScalar(conn,
                    "SELECT IFNULL(SUM(GrandTotal),0) FROM Bills").ToString();

                lblSummary3Value.Text = ExecuteScalar(conn, @"
                    SELECT COUNT(*) 
                    FROM Jobs j
                    INNER JOIN JobStatus s ON j.StatusID = s.StatusID
                    WHERE s.StatusName = 'Completed'").ToString();

                lblSummary4Value.Text = ExecuteScalar(conn, @"
                    SELECT COUNT(*) 
                    FROM StockItems 
                    WHERE Quantity <= ReorderLevel").ToString();
            }
        }
        // Helper method to execute a scalar query and return the result
        private object ExecuteScalar(MySqlConnection conn, string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);
            return cmd.ExecuteScalar();
        }
        // Method to generate the report based on the selected report type and date range
        private void GenerateReport()
        {
            string type = cmbReportType.Text;
            DateTime from = dtpFromDate.Value.Date;
            DateTime to = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1);

            string query = "";

            if (type == "Daily Job Report" || type == "Vehicle Service Report")
            {
                
                query = @"
                    SELECT 
                        j.JobID,
                        v.VehicleNumber,
                        c.CustomerName,
                        st.ServiceName,
                        js.StatusName,
                        j.ServiceDate
                    FROM Jobs j
                    INNER JOIN Vehicles v ON j.VehicleID = v.VehicleID
                    INNER JOIN Customers c ON v.CustomerID = c.CustomerID
                    INNER JOIN ServiceTypes st ON j.ServiceTypeID = st.ServiceTypeID
                    INNER JOIN JobStatus js ON j.StatusID = js.StatusID
                    WHERE j.ServiceDate BETWEEN @from AND @to
                    ORDER BY j.ServiceDate DESC";
            }
            else if (type == "Monthly Revenue Report" || type == "Billing Report")
            {
                query = @"
                    SELECT 
                        b.BillID,
                        b.JobID,
                        b.SubTotal,
                        b.Tax,
                        b.GrandTotal
                    FROM Bills b
                    ORDER BY b.BillID DESC";
            }
            else if (type == "Inventory Report")
            {
                query = @"
                    SELECT 
                        si.ItemID,
                        si.ItemName,
                        sc.CategoryName,
                        si.Price,
                        si.Quantity,
                        si.ReorderLevel,
                        si.CreatedAt
                    FROM StockItems si
                    INNER JOIN StockCategories sc ON si.CategoryID = sc.CategoryID
                    ORDER BY si.Quantity ASC";
            }
            else if (type == "Mechanic Performance Report")
            {
                query = @"
                    SELECT 
                        u.Username AS Mechanic,
                        COUNT(j.JobID) AS TotalJobs,
                        SUM(CASE WHEN js.StatusName = 'Completed' THEN 1 ELSE 0 END) AS CompletedJobs
                    FROM Users u
                    INNER JOIN Roles r ON u.RoleID = r.RoleID
                    LEFT JOIN Jobs j ON u.UserID = j.MechanicID
                    LEFT JOIN JobStatus js ON j.StatusID = js.StatusID
                    WHERE r.RoleName = 'Mechanic'
                    GROUP BY u.UserID, u.Username
                    ORDER BY CompletedJobs DESC";
            }
            // Add more report types and their corresponding queries as needed
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn);

                if (query.Contains("@from"))
                {
                    cmd.Parameters.AddWithValue("@from", from);
                    cmd.Parameters.AddWithValue("@to", to);
                }

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvReports.DataSource = dt;
            }
        }
        // Event handler for the Export Excel button click
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (dgvReports.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "CSV File (*.csv)|*.csv";
            save.FileName = "Report.csv";

            if (save.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < dgvReports.Columns.Count; i++)
                {
                    sb.Append(dgvReports.Columns[i].HeaderText);

                    if (i < dgvReports.Columns.Count - 1)
                        sb.Append(",");
                }

                sb.AppendLine();
                
                foreach (DataGridViewRow row in dgvReports.Rows)
                {
                    if (row.IsNewRow) continue;

                    for (int i = 0; i < dgvReports.Columns.Count; i++)
                    {
                        string value = row.Cells[i].Value?.ToString() ?? "";
                        value = value.Replace(",", " ");

                        sb.Append(value);

                        if (i < dgvReports.Columns.Count - 1)
                            sb.Append(",");
                    }

                    sb.AppendLine();
                }

                File.WriteAllText(save.FileName, sb.ToString(), Encoding.UTF8);

                MessageBox.Show("Excel CSV exported successfully.");
            }
        }
        // Event handler for the Export PDF button click
        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (dgvReports.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF File (*.pdf)|*.pdf";
            save.FileName = "Report.pdf";

            if (save.ShowDialog() == DialogResult.OK)
            {
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Vehicle Service Management Report";

                PdfPage page = document.AddPage();
                page.Size = PdfSharpCore.PageSize.A4;
                page.Orientation = PdfSharpCore.PageOrientation.Landscape;

                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont titleFont = new XFont("Arial", 18, XFontStyle.Bold);
                XFont headerFont = new XFont("Arial", 9, XFontStyle.Bold);
                XFont rowFont = new XFont("Arial", 8, XFontStyle.Regular);

                double x = 25;
                double y = 35;
                double colWidth = 105;

                gfx.DrawString("Vehicle Service Management Report", titleFont, XBrushes.Black, x, y);
                y += 30;

                gfx.DrawString("Report Type: " + cmbReportType.Text, rowFont, XBrushes.Black, x, y);
                y += 20;

                for (int i = 0; i < dgvReports.Columns.Count; i++)
                {
                    gfx.DrawString(dgvReports.Columns[i].HeaderText, headerFont, XBrushes.Black, x + (i * colWidth), y);
                }

                y += 20;

                foreach (DataGridViewRow row in dgvReports.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (y > page.Height - 40)
                    {
                        page = document.AddPage();
                        page.Size = PdfSharpCore.PageSize.A4;
                        page.Orientation = PdfSharpCore.PageOrientation.Landscape;
                        gfx = XGraphics.FromPdfPage(page);
                        y = 35;
                    }

                    for (int i = 0; i < dgvReports.Columns.Count; i++)
                    {
                        string value = row.Cells[i].Value?.ToString() ?? "";

                        if (value.Length > 18)
                            value = value.Substring(0, 18);

                        gfx.DrawString(value, rowFont, XBrushes.Black, x + (i * colWidth), y);
                    }

                    y += 18;
                }

                document.Save(save.FileName);
                MessageBox.Show("PDF exported successfully.");
            }
        }
    }
}