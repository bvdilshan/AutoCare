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

        private void UC_Reports_Load(object sender, EventArgs e)
        {
            
            cmbReportType.Items.Clear();
            cmbReportType.Items.AddRange(new object[] {
                "Monthly Revenue Report",
                "Vehicle Service Report",
                "Inventory Stock Report"
            });

            if (cmbReportType.Items.Count > 0)
                cmbReportType.SelectedIndex = 0;

            dtpFromDate.Value = DateTime.Today.AddDays(-30);
            dtpToDate.Value = DateTime.Today;

            LoadSummary();
            GenerateReport();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateReport();
            LoadSummary();
        }

        private void LoadSummary()
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                   
                    lblSummary1Value.Text = ExecuteScalar(conn, "SELECT COUNT(*) FROM jobs").ToString();

           
                    lblSummary2Value.Text = "Rs. " + Convert.ToDecimal(ExecuteScalar(conn, "SELECT IFNULL(SUM(GrandTotal), 0) FROM bills")).ToString("N2");

               
                    lblSummary3Value.Text = ExecuteScalar(conn, "SELECT COUNT(*) FROM jobs WHERE BillingStatus = 'Paid' OR BillingStatus = 'Billed'").ToString();

                
                    lblSummary4Value.Text = ExecuteScalar(conn, "SELECT COUNT(*) FROM stockitems WHERE Quantity <= ReorderLevel").ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Summary error: " + ex.Message);
            }
        }

        private object ExecuteScalar(MySqlConnection conn, string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);
            return cmd.ExecuteScalar();
        }

        private void GenerateReport()
        {
            string type = cmbReportType.Text;
            DateTime from = dtpFromDate.Value.Date;
            DateTime to = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1);

            string query = "";

            if (type == "Vehicle Service Report")
            {
                query = @"SELECT j.JobID, v.VehicleNumber, c.CustomerName, j.BillingStatus, j.ServiceDate 
                          FROM jobs j 
                          INNER JOIN vehicles v ON j.VehicleID = v.VehicleID 
                          INNER JOIN customers c ON v.CustomerID = c.CustomerID 
                          WHERE j.ServiceDate BETWEEN @from AND @to 
                          ORDER BY j.JobID DESC";
            }

            else if (type == "Monthly Revenue Report")
            {
                query = @"SELECT b.BillID, b.JobID, v.VehicleNumber, b.GrandTotal, b.PaymentMethod, b.BillDate
                          FROM bills b
                          INNER JOIN jobs j ON b.JobID = j.JobID
                          INNER JOIN vehicles v ON j.VehicleID = v.VehicleID
                          WHERE b.BillDate BETWEEN @from AND @to
                          ORDER BY b.BillID DESC";
            }

            else if (type == "Inventory Stock Report")
            {
                query = "SELECT ItemID, ItemName, Price, Quantity, ReorderLevel FROM stockitems ORDER BY Quantity ASC";
            }

            if (query == "") return;

            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show("Report generate error: " + ex.Message);
            }
        }


        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (dgvReports.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "CSV File (*.csv)|*.csv";
            save.FileName = cmbReportType.Text.Replace(" ", "_") + "_Report.csv";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < dgvReports.Columns.Count; i++)
                    {
                        sb.Append(dgvReports.Columns[i].HeaderText);
                        if (i < dgvReports.Columns.Count - 1) sb.Append(",");
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

                            if (i < dgvReports.Columns.Count - 1) sb.Append(",");
                        }
                        sb.AppendLine();
                    }

                    File.WriteAllText(save.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Excel CSV exported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Export excel error: " + ex.Message);
                }
            }
        }


        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (dgvReports.Rows.Count == 0)
            {
                MessageBox.Show("No data to export." ,"Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF File (*.pdf)|*.pdf";
            save.FileName = cmbReportType.Text.Replace(" ", "_") + "_Report.pdf";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PdfDocument document = new PdfDocument();
                    document.Info.Title = cmbReportType.Text;

                    PdfPage page = document.AddPage();
                    page.Size = PdfSharpCore.PageSize.A4;
                    page.Orientation = PdfSharpCore.PageOrientation.Landscape;

                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold);
                    XFont headerFont = new XFont("Arial", 10, XFontStyle.Bold);
                    XFont rowFont = new XFont("Arial", 9, XFontStyle.Regular);

                    double x = 20;
                    double y = 40;
                    double colWidth = 130; 

                    gfx.DrawString("Vehicle Service Management System", titleFont, XBrushes.DarkBlue, x, y);
                    y += 25;
                    gfx.DrawString("Report: " + cmbReportType.Text, headerFont, XBrushes.Black, x, y);
                    y += 15;
                    gfx.DrawString("Generated Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), rowFont, XBrushes.Gray, x, y);
                    y += 30;

                    for (int i = 0; i < dgvReports.Columns.Count; i++)
                    {
                        gfx.DrawString(dgvReports.Columns[i].HeaderText, headerFont, XBrushes.Black, x + (i * colWidth), y);
                    }
                    y += 20;

                    foreach (DataGridViewRow row in dgvReports.Rows)
                    {
                        if (row.IsNewRow) continue;

                        for (int i = 0; i < dgvReports.Columns.Count; i++)
                        {
                            string value = row.Cells[i].Value?.ToString() ?? "";
                            if (value.Length > 20) value = value.Substring(0, 20); 

                            gfx.DrawString(value, rowFont, XBrushes.Black, x + (i * colWidth), y);
                        }
                        y += 20;
                    }

                    document.Save(save.FileName);
                    MessageBox.Show("PDF exported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Export PDF error: " + ex.Message);
                }
            }
        }
    }
}