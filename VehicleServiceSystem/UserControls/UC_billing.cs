using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using VehicleServiceSystem.Data;
using System.Drawing;
using System.Drawing.Printing;

namespace VehicleServiceSystem.UserControls
{
    public partial class UC_billing : UserControl
    {
        private int selectedJobId = 0;
        private int currentBillId = 0;
        private decimal partsTotal = 0;

        public UC_billing()
        {
            InitializeComponent();

            btnSearch.Click += btnSearch_Click;
            btnSaveBill.Click += btnSaveBill_Click;
            btnMarkPaid.Click += btnMarkPaid_Click;
            btnClear.Click += btnClear_Click;
            btnPrint.Click += btnPrint_Click;
            dgvReadyJobs.CellClick += dgvReadyJobs_CellClick;

            txtServiceCharge.TextChanged += Amount_TextChanged;
            txtLabourCharge.TextChanged += Amount_TextChanged;
            txtTax.TextChanged += Amount_TextChanged;
        }

        private void UC_billing_Load(object sender, EventArgs e)
        {
            LoadPaymentMethods();
            LoadReadyJobs();
            ClearForm();
        }


        private void LoadPaymentMethods()
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT MethodName FROM paymentmethods ORDER BY MethodID";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbPaymentMethod.DataSource = dt;
                    cmbPaymentMethod.DisplayMember = "MethodName";
                    cmbPaymentMethod.ValueMember = "MethodName";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Payment methods error: " + ex.Message);
            }
        }

 
        private void LoadReadyJobs()
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT j.JobID, v.VehicleNumber, c.CustomerName, c.Phone, j.BillingStatus, j.CompletedAt " +
                                   "FROM jobs j " +
                                   "INNER JOIN vehicles v ON j.VehicleID = v.VehicleID " +
                                   "INNER JOIN customers c ON v.CustomerID = c.CustomerID " +
                                   "WHERE j.BillingStatus = 'Ready for Billing' " +
                                   "ORDER BY j.CompletedAt DESC";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvReadyJobs.DataSource = dt;
                    lblNotificationCount.Text = dt.Rows.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ready jobs error: " + ex.Message);
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearchVehicle.Text.Trim();

            if (search == "")
            {
                LoadReadyJobs();
                return;
            }

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT j.JobID, v.VehicleNumber, c.CustomerName, c.Phone, j.BillingStatus, j.CompletedAt " +
                                   "FROM jobs j " +
                                   "INNER JOIN vehicles v ON j.VehicleID = v.VehicleID " +
                                   "INNER JOIN customers c ON v.CustomerID = c.CustomerID " +
                                   "WHERE j.BillingStatus = 'Ready for Billing' " +
                                   "AND (v.VehicleNumber LIKE @search OR c.CustomerName LIKE @search)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvReadyJobs.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message);
            }
        }

 
        private void dgvReadyJobs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvReadyJobs.Rows[e.RowIndex];
            selectedJobId = Convert.ToInt32(row.Cells["JobID"].Value);
            currentBillId = 0;

            lblCustomerValue.Text = row.Cells["CustomerName"].Value.ToString();
            lblVehicleValue.Text = row.Cells["VehicleNumber"].Value.ToString();
            lblJobValue.Text = "JOB" + selectedJobId.ToString("000");
            lblStatusValue.Text = row.Cells["BillingStatus"].Value.ToString();

            LoadBillItems();
        }

        private void LoadBillItems()
        {
            if (selectedJobId == 0) return;

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT si.ItemName AS PartName, jp.Quantity, jp.Price AS UnitPrice, (jp.Quantity * jp.Price) AS Total " +
                                   "FROM jobparts jp " +
                                   "INNER JOIN stockitems si ON jp.ItemID = si.ItemID " +
                                   "WHERE jp.JobID = @jobId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@jobId", selectedJobId);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvBillItems.DataSource = dt;

           
                    partsTotal = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Total"] != DBNull.Value)
                        {
                            partsTotal = partsTotal + Convert.ToDecimal(row["Total"]);
                        }
                    }

                    txtPartsTotal.Text = partsTotal.ToString("0.00");
                    CalculateTotals();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load bill items error: " + ex.Message);
            }
        }

        private void Amount_TextChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }


        private void CalculateTotals()
        {
            decimal serviceCharge = ToDecimal(txtServiceCharge.Text);
            decimal labourCharge = ToDecimal(txtLabourCharge.Text);
            decimal tax = ToDecimal(txtTax.Text);

            decimal serviceTotal = serviceCharge + labourCharge;
            decimal subTotal = partsTotal + serviceTotal;
            decimal grandTotal = subTotal + tax;

            txtServiceTotal.Text = serviceTotal.ToString("0.00");
            txtSubTotal.Text = subTotal.ToString("0.00");
            txtGrandTotal.Text = grandTotal.ToString("0.00");
        }


        private decimal ToDecimal(string value)
        {
            decimal result;
            if (decimal.TryParse(value, out result))
            {
                return result;
            }
            return 0;
        }


        private void btnSaveBill_Click(object sender, EventArgs e)
        {
            if (selectedJobId == 0 || dgvBillItems.Rows.Count == 0)
            {
                MessageBox.Show("Please select a job with items.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string checkQuery = "SELECT BillID FROM bills WHERE JobID = @jobId LIMIT 1";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@jobId", selectedJobId);
                    object existingBill = checkCmd.ExecuteScalar();

                    if (existingBill != null)
                    {
                        MessageBox.Show("This job already has a bill saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    
                    string insertBillQuery = "INSERT INTO bills (JobID, SubTotal, Tax, GrandTotal, PaymentMethod) " +
                                             "VALUES (@jobId, @subTotal, @tax, @grandTotal, @paymentMethod); " +
                                             "SELECT LAST_INSERT_ID();"; // 

                    MySqlCommand billCmd = new MySqlCommand(insertBillQuery, conn);
                    billCmd.Parameters.AddWithValue("@jobId", selectedJobId);
                    billCmd.Parameters.AddWithValue("@subTotal", ToDecimal(txtSubTotal.Text));
                    billCmd.Parameters.AddWithValue("@tax", ToDecimal(txtTax.Text));
                    billCmd.Parameters.AddWithValue("@grandTotal", ToDecimal(txtGrandTotal.Text));
                    billCmd.Parameters.AddWithValue("@paymentMethod", cmbPaymentMethod.Text);

                    currentBillId = Convert.ToInt32(billCmd.ExecuteScalar());

                    foreach (DataGridViewRow row in dgvBillItems.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string itemQuery = "INSERT INTO billitems (BillID, PartName, Quantity, UnitPrice, Total) " +
                                           "VALUES (@billId, @partName, @qty, @price, @total)";

                        MySqlCommand itemCmd = new MySqlCommand(itemQuery, conn);
                        itemCmd.Parameters.AddWithValue("@billId", currentBillId);
                        itemCmd.Parameters.AddWithValue("@partName", row.Cells["PartName"].Value.ToString());
                        itemCmd.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["Quantity"].Value));
                        itemCmd.Parameters.AddWithValue("@price", Convert.ToDecimal(row.Cells["UnitPrice"].Value));
                        itemCmd.Parameters.AddWithValue("@total", Convert.ToDecimal(row.Cells["Total"].Value));
                        itemCmd.ExecuteNonQuery();
                    }

                    string updateJobQuery = "UPDATE jobs SET BillingStatus = 'Billed' WHERE JobID = @jobId";
                    MySqlCommand updateJobCmd = new MySqlCommand(updateJobQuery, conn);
                    updateJobCmd.Parameters.AddWithValue("@jobId", selectedJobId);
                    updateJobCmd.ExecuteNonQuery();

                    lblStatusValue.Text = "Billed";
                    MessageBox.Show("Bill saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadReadyJobs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save bill error: " + ex.Message);
            }
        }

        private void btnMarkPaid_Click(object sender, EventArgs e)
        {
            if (selectedJobId == 0) return;

            if (currentBillId == 0)
            {
                currentBillId = GetBillIdByJobId(selectedJobId);
            }

            if (currentBillId == 0)
            {
                MessageBox.Show("Please save the bill before marking as paid.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string paymentQuery = "INSERT INTO payments (InvoiceID, MethodID, Amount) " +
                                           "VALUES (@billId, (SELECT MethodID FROM paymentmethods WHERE MethodName = @method LIMIT 1), @amount)";

                    MySqlCommand payCmd = new MySqlCommand(paymentQuery, conn);
                    payCmd.Parameters.AddWithValue("@billId", currentBillId);
                    payCmd.Parameters.AddWithValue("@method", cmbPaymentMethod.Text);
                    payCmd.Parameters.AddWithValue("@amount", ToDecimal(txtGrandTotal.Text));
                    payCmd.ExecuteNonQuery();


                    string updateQuery = "UPDATE jobs SET BillingStatus = 'Paid' WHERE JobID = @jobId";
                    MySqlCommand upCmd = new MySqlCommand(updateQuery, conn);
                    upCmd.Parameters.AddWithValue("@jobId", selectedJobId);
                    upCmd.ExecuteNonQuery();

                    lblStatusValue.Text = "Paid";
                    MessageBox.Show("Payment recorded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadReadyJobs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Payment error: " + ex.Message);
            }
        }

        private int GetBillIdByJobId(int jobId)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT BillID FROM bills WHERE JobID = @jobId LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@jobId", jobId);
                object result = cmd.ExecuteScalar();

                if (result != null) return Convert.ToInt32(result);
                return 0;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (selectedJobId == 0) return;
            if (currentBillId == 0) currentBillId = GetBillIdByJobId(selectedJobId);

            if (currentBillId == 0)
            {
                MessageBox.Show("Please save the bill first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PrintDocument doc = new PrintDocument();
            doc.PrintPage += PrintBillPage;

            PrintPreviewDialog preview = new PrintPreviewDialog();
            preview.Document = doc;
            preview.Width = 900;
            preview.Height = 700;
            preview.ShowDialog();
        }

        private void PrintBillPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font fTitle = new Font("Segoe UI", 16, FontStyle.Bold);
            Font fHeader = new Font("Segoe UI", 11, FontStyle.Bold);
            Font fBody = new Font("Segoe UI", 10, FontStyle.Regular);

            int x = 50;
            int y = 50;

            g.DrawString("VEHICLE SERVICE CENTER", fTitle, Brushes.Black, x, y); y += 40;
            g.DrawString("Service Invoice", fHeader, Brushes.Black, x, y); y += 30;

            g.DrawString("Bill ID: " + currentBillId, fBody, Brushes.Black, x, y); y += 20;
            g.DrawString("Vehicle No: " + lblVehicleValue.Text, fBody, Brushes.Black, x, y); y += 20;
            g.DrawString("Customer: " + lblCustomerValue.Text, fBody, Brushes.Black, x, y); y += 30;

            g.DrawLine(Pens.Black, x, y, 700, y); y += 10;
            g.DrawString("Part Name", fHeader, Brushes.Black, x, y);
            g.DrawString("Total", fHeader, Brushes.Black, x + 400, y); y += 25;
            g.DrawLine(Pens.Black, x, y, 700, y); y += 10;

            foreach (DataGridViewRow row in dgvBillItems.Rows)
            {
                if (row.IsNewRow) continue;
                g.DrawString(row.Cells["PartName"].Value.ToString(), fBody, Brushes.Black, x, y);
                g.DrawString(row.Cells["Total"].Value.ToString(), fBody, Brushes.Black, x + 400, y);
                y += 20;
            }

            y += 20;
            g.DrawString("Grand Total: Rs. " + txtGrandTotal.Text, fHeader, Brushes.Black, x + 300, y); y += 30;
            g.DrawString("Thank You!", fHeader, Brushes.Black, x, y);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadReadyJobs();
        }

        private void ClearForm()
        {
            selectedJobId = 0;
            currentBillId = 0;
            partsTotal = 0;

            txtSearchVehicle.Clear();
            txtServiceCharge.Text = "0";
            txtLabourCharge.Text = "0";
            txtTax.Text = "0";

            txtPartsTotal.Text = "0.00";
            txtServiceTotal.Text = "0.00";
            txtSubTotal.Text = "0.00";
            txtGrandTotal.Text = "0.00";

            lblCustomerValue.Text = "-";
            lblVehicleValue.Text = "-";
            lblJobValue.Text = "-";
            lblStatusValue.Text = "-";

            dgvBillItems.DataSource = null;
        }
    }
}