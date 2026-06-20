using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using VehicleServiceSystem.Data;
using System.Drawing;
using System.Drawing.Printing;

namespace VehicleServiceSystem.UserControls
{
    
    /// This UserControl handles the billing process. 
    /// It loads completed jobs, calculates the total bill, saves invoices to MySQL, 
    /// processes payments, and prints receipts.
    
    public partial class UC_billing : UserControl
    {
        // Track unique identifiers and calculation cache fields
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
        // On load, populate payment methods and ready jobs, and clear any existing form data to ensure a fresh start for billing operations.
        private void UC_billing_Load(object sender, EventArgs e)
        {
            LoadPaymentMethods();
            LoadReadyJobs();
            ClearForm();
        }

        
        /// Pulls active financial channels from database to securely map dropdown parameters.
        
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
                MessageBox.Show("Payment methods load error: " + ex.Message);
            }
        }

        // Fetches all service jobs from the database that are completed and ready to be billed.
        private void LoadReadyJobs()
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            j.JobID,
                            v.VehicleNumber,
                            c.CustomerName,
                            c.Phone,
                            j.BillingStatus,
                            j.CompletedAt
                        FROM jobs j
                        INNER JOIN vehicles v ON j.VehicleID = v.VehicleID
                        INNER JOIN customers c ON v.CustomerID = c.CustomerID
                        WHERE j.BillingStatus = 'Ready for Billing'
                        ORDER BY j.CompletedAt DESC, j.JobID DESC";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvReadyJobs.DataSource = dt;
                    lblNotificationCount.Text = dt.Rows.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ready jobs load error: " + ex.Message);
            }
        }
        // Filters the ready jobs list based on user search input 
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearchVehicle.Text.Trim();

            if (string.IsNullOrWhiteSpace(search))
            {
                LoadReadyJobs();
                return;
            }

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            j.JobID,
                            v.VehicleNumber,
                            c.CustomerName,
                            c.Phone,
                            j.BillingStatus,
                            j.CompletedAt
                        FROM jobs j
                        INNER JOIN vehicles v ON j.VehicleID = v.VehicleID
                        INNER JOIN customers c ON v.CustomerID = c.CustomerID
                        WHERE j.BillingStatus = 'Ready for Billing'
                        AND (
                            j.JobID LIKE @search
                            OR v.VehicleNumber LIKE @search
                            OR c.CustomerName LIKE @search
                            OR c.Phone LIKE @search
                        )
                        ORDER BY j.CompletedAt DESC, j.JobID DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvReadyJobs.DataSource = dt;
                    lblNotificationCount.Text = dt.Rows.Count.ToString();

                    if (dt.Rows.Count == 0)
                        MessageBox.Show("No ready billing job found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message);
            }
        }
        // Displays the selected job's customer and vehicle details when a row is clicked in the grid.
        private void dgvReadyJobs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvReadyJobs.Rows[e.RowIndex];

            selectedJobId = Convert.ToInt32(row.Cells["JobID"].Value);
            currentBillId = 0;

            lblCustomerValue.Text = row.Cells["CustomerName"].Value?.ToString();
            lblVehicleValue.Text = row.Cells["VehicleNumber"].Value?.ToString();
            lblJobValue.Text = "JOB" + selectedJobId.ToString("000");
            lblStatusValue.Text = row.Cells["BillingStatus"].Value?.ToString();

            LoadBillItems();
        }
        // Loads the spare parts used for the selected job and calculates the total cost for parts.       
        private void LoadBillItems()
        {
            if (selectedJobId == 0)
            {
                dgvBillItems.DataSource = null;
                partsTotal = 0;
                txtPartsTotal.Text = "0.00";
                CalculateTotals();
                return;
            }

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT
                            si.ItemName AS PartName,
                            jp.Quantity,
                            jp.Price AS UnitPrice,
                            (jp.Quantity * jp.Price) AS Total
                        FROM jobparts jp
                        INNER JOIN stockitems si ON jp.ItemID = si.ItemID
                        WHERE jp.JobID = @jobId
                        ORDER BY jp.JobPartID ASC";

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
                            partsTotal += Convert.ToDecimal(row["Total"]);
                    }

                    txtPartsTotal.Text = partsTotal.ToString("0.00");
                    CalculateTotals();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bill items load error: " + ex.Message);
            }
        }

        private void Amount_TextChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        // Calculates the final amounts including service charges, labor charges, parts total, and taxes.
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
            if (decimal.TryParse(value, out decimal result))
                return result;

            return 0;
        }
        // Saves the bill details to the database, including the bill summary and individual bill items, and updates the job's billing status.
        private void btnSaveBill_Click(object sender, EventArgs e)
        {
            if (selectedJobId == 0)
            {
                MessageBox.Show("Please select a ready job first.");
                return;
            }

            if (dgvBillItems.Rows.Count == 0)
            {
                MessageBox.Show("No bill items found for this job.");
                return;
            }

            if (cmbPaymentMethod.Text.Trim() == "")
            {
                MessageBox.Show("Please select payment method.");
                return;
            }

            CalculateTotals();

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();

                    try
                    {
                        string checkQuery = "SELECT BillID FROM bills WHERE JobID = @jobId LIMIT 1";

                        MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn, tr);
                        checkCmd.Parameters.AddWithValue("@jobId", selectedJobId);

                        object existingBill = checkCmd.ExecuteScalar();

                        if (existingBill != null && existingBill != DBNull.Value)
                        {
                            currentBillId = Convert.ToInt32(existingBill);
                            MessageBox.Show("This job already has a bill.");
                            tr.Rollback();
                            return;
                        }

                        string billQuery = @"
                            INSERT INTO bills
                            (JobID, SubTotal, Tax, GrandTotal, PaymentMethod)
                            VALUES
                            (@jobId, @subTotal, @tax, @grandTotal, @paymentMethod)";

                        MySqlCommand billCmd = new MySqlCommand(billQuery, conn, tr);
                        billCmd.Parameters.AddWithValue("@jobId", selectedJobId);
                        billCmd.Parameters.AddWithValue("@subTotal", ToDecimal(txtSubTotal.Text));
                        billCmd.Parameters.AddWithValue("@tax", ToDecimal(txtTax.Text));
                        billCmd.Parameters.AddWithValue("@grandTotal", ToDecimal(txtGrandTotal.Text));
                        billCmd.Parameters.AddWithValue("@paymentMethod", cmbPaymentMethod.Text.Trim());
                        billCmd.ExecuteNonQuery();

                        currentBillId = Convert.ToInt32(billCmd.LastInsertedId);

                        foreach (DataGridViewRow row in dgvBillItems.Rows)
                        {
                            if (row.IsNewRow)
                                continue;

                            string itemQuery = @"
                                INSERT INTO billitems
                                (BillID, PartName, Quantity, UnitPrice, Total)
                                VALUES
                                (@billId, @partName, @quantity, @unitPrice, @total)";

                            MySqlCommand itemCmd = new MySqlCommand(itemQuery, conn, tr);
                            itemCmd.Parameters.AddWithValue("@billId", currentBillId);
                            itemCmd.Parameters.AddWithValue("@partName", row.Cells["PartName"].Value?.ToString());
                            itemCmd.Parameters.AddWithValue("@quantity", Convert.ToInt32(row.Cells["Quantity"].Value));
                            itemCmd.Parameters.AddWithValue("@unitPrice", Convert.ToDecimal(row.Cells["UnitPrice"].Value));
                            itemCmd.Parameters.AddWithValue("@total", Convert.ToDecimal(row.Cells["Total"].Value));
                            itemCmd.ExecuteNonQuery();
                        }

                        string updateJobQuery = @"
                            UPDATE jobs
                            SET BillingStatus = 'Billed'
                            WHERE JobID = @jobId";

                        MySqlCommand updateJobCmd = new MySqlCommand(updateJobQuery, conn, tr);
                        updateJobCmd.Parameters.AddWithValue("@jobId", selectedJobId);
                        updateJobCmd.ExecuteNonQuery();

                        tr.Commit();

                        lblStatusValue.Text = "Billed";
                        MessageBox.Show("Bill saved successfully.");

                        LoadReadyJobs();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save bill error: " + ex.Message);
            }
        }
        // Marks the selected job as paid, records the payment in the database, and updates the job status accordingly.
        private void btnMarkPaid_Click(object sender, EventArgs e)
        {
            if (selectedJobId == 0)
            {
                MessageBox.Show("Please select a job first.");
                return;
            }

            if (currentBillId == 0)
            {
                currentBillId = GetBillIdByJobId(selectedJobId);

                if (currentBillId == 0)
                {
                    MessageBox.Show("Please save the bill before payment.");
                    return;
                }
            }

            if (cmbPaymentMethod.Text.Trim() == "")
            {
                MessageBox.Show("Please select payment method.");
                return;
            }

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();

                    try
                    {
                        string checkPaymentQuery = "SELECT PaymentID FROM payments WHERE InvoiceID = @billId LIMIT 1";

                        MySqlCommand checkPaymentCmd = new MySqlCommand(checkPaymentQuery, conn, tr);
                        checkPaymentCmd.Parameters.AddWithValue("@billId", currentBillId);

                        object existingPayment = checkPaymentCmd.ExecuteScalar();

                        if (existingPayment != null && existingPayment != DBNull.Value)
                        {
                            MessageBox.Show("Payment already recorded!");
                            tr.Rollback();
                            return;
                        }

                        string paymentQuery = @"
                            INSERT INTO payments
                            (InvoiceID, MethodID, Amount)
                            VALUES
                            (
                                @billId,
                                (SELECT MethodID FROM paymentmethods WHERE MethodName = @method LIMIT 1),
                                @amount
                            )";

                        MySqlCommand paymentCmd = new MySqlCommand(paymentQuery, conn, tr);
                        paymentCmd.Parameters.AddWithValue("@billId", currentBillId);
                        paymentCmd.Parameters.AddWithValue("@method", cmbPaymentMethod.Text.Trim());
                        paymentCmd.Parameters.AddWithValue("@amount", ToDecimal(txtGrandTotal.Text));
                        paymentCmd.ExecuteNonQuery();

                        string updateJobQuery = @"
                            UPDATE jobs
                            SET BillingStatus = 'Paid'
                            WHERE JobID = @jobId";

                        MySqlCommand updateJobCmd = new MySqlCommand(updateJobQuery, conn, tr);
                        updateJobCmd.Parameters.AddWithValue("@jobId", selectedJobId);
                        updateJobCmd.ExecuteNonQuery();

                        tr.Commit();

                        lblStatusValue.Text = "Paid";
                        MessageBox.Show("Payment completed successfully.");

                        LoadReadyJobs();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Payment error: " + ex.Message);
            }
        }
        // Retrieves the BillID associated with the given JobID, if it exists, to link payments and printouts correctly.
        private int GetBillIdByJobId(int jobId)
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT BillID FROM bills WHERE JobID = @jobId LIMIT 1";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@jobId", jobId);

                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                        return 0;

                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return 0;
            }
        }
        // Generates a print preview of the bill, allowing the user to review the invoice before sending it to the printer.
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (selectedJobId == 0)
            {
                MessageBox.Show("Please select a job first.");
                return;
            }

            if (currentBillId == 0)
                currentBillId = GetBillIdByJobId(selectedJobId);

            if (currentBillId == 0)
            {
                MessageBox.Show("Please save the bill before printing.");
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
        // This method handles the actual drawing of the bill on the print document, formatting it to look like a professional invoice with all relevant details.
        private void PrintBillPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            Font titleFont = new Font("Segoe UI", 18, FontStyle.Bold);
            Font headerFont = new Font("Segoe UI", 11, FontStyle.Bold);
            Font normalFont = new Font("Segoe UI", 10);

            int x = 50;
            int y = 50;

            g.DrawString("VEHICLE SERVICE CENTER", titleFont, Brushes.Black, x, y);
            y += 40;

            g.DrawString("Service Bill", headerFont, Brushes.Black, x, y);
            y += 35;

            g.DrawString("Bill ID: " + currentBillId, normalFont, Brushes.Black, x, y);
            y += 25;

            g.DrawString("Job ID: " + lblJobValue.Text, normalFont, Brushes.Black, x, y);
            y += 25;

            g.DrawString("Customer: " + lblCustomerValue.Text, normalFont, Brushes.Black, x, y);
            y += 25;

            g.DrawString("Vehicle No: " + lblVehicleValue.Text, normalFont, Brushes.Black, x, y);
            y += 25;

            g.DrawString("Date: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm tt"), normalFont, Brushes.Black, x, y);
            y += 35;

            g.DrawLine(Pens.Black, x, y, 760, y);
            y += 15;

            g.DrawString("Part Name", headerFont, Brushes.Black, x, y);
            g.DrawString("Qty", headerFont, Brushes.Black, x + 320, y);
            g.DrawString("Unit Price", headerFont, Brushes.Black, x + 400, y);
            g.DrawString("Total", headerFont, Brushes.Black, x + 550, y);
            y += 25;

            g.DrawLine(Pens.Black, x, y, 760, y);
            y += 15;

            foreach (DataGridViewRow row in dgvBillItems.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string partName = row.Cells["PartName"].Value?.ToString() ?? "";
                string qty = row.Cells["Quantity"].Value?.ToString() ?? "0";
                string unitPrice = row.Cells["UnitPrice"].Value?.ToString() ?? "0";
                string total = row.Cells["Total"].Value?.ToString() ?? "0";

                if (partName.Length > 30)
                    partName = partName.Substring(0, 30);

                g.DrawString(partName, normalFont, Brushes.Black, x, y);
                g.DrawString(qty, normalFont, Brushes.Black, x + 320, y);
                g.DrawString(unitPrice, normalFont, Brushes.Black, x + 400, y);
                g.DrawString(total, normalFont, Brushes.Black, x + 550, y);

                y += 25;
            }

            y += 10;
            g.DrawLine(Pens.Black, x, y, 760, y);
            y += 25;

            g.DrawString("Parts Total: Rs. " + txtPartsTotal.Text, normalFont, Brushes.Black, x + 420, y);
            y += 25;

            g.DrawString("Service Total: Rs. " + txtServiceTotal.Text, normalFont, Brushes.Black, x + 420, y);
            y += 25;

            
   

            g.DrawString("Grand Total: Rs. " + txtGrandTotal.Text, headerFont, Brushes.Black, x + 420, y);
            y += 40;

            g.DrawString("Payment Method: " + cmbPaymentMethod.Text, normalFont, Brushes.Black, x, y);
            y += 40;

            g.DrawString("Thank you!", headerFont, Brushes.Black, x, y);
        }
        // Resets all text boxes, labels, data grids, and variables back to their default empty states.
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadReadyJobs();
        }
        // This method centralizes the logic for clearing the form, ensuring that all relevant fields and variables are reset consistently whenever needed.
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

            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;
        }
    }
}