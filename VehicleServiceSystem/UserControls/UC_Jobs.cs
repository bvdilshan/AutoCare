using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using VehicleServiceSystem.Data;

namespace VehicleServiceSystem.UserControls
{
    public partial class UC_Jobs : UserControl
    {
        private int selectedJobId = 0;
        private decimal partsTotal = 0;

        public UC_Jobs()
        {
            InitializeComponent();
            btnSave.Click += btnSave_Click;
            btnClear.Click += btnClear_Click;
            btnAddPart.Click += btnAddPart_Click;
            btnCompleteJob.Click += btnCompleteJob_Click;
            dgvJobs.CellClick += dgvJobs_CellClick;
            this.VisibleChanged += UC_Jobs_VisibleChanged;
            txtQuantity.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
        }

        private void UC_Jobs_Load(object sender, EventArgs e)
        {
            LoadVehicles();
            LoadServiceTypes();
            LoadMechanics();
            LoadParts();
            LoadJobs();
            ClearForm();
        }

        private void LoadVehicles()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                string query = "SELECT VehicleID, VehicleNumber FROM vehicles ORDER BY VehicleNumber";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbVehicleNo.DataSource = dt;
                cmbVehicleNo.DisplayMember = "VehicleNumber";
                cmbVehicleNo.ValueMember = "VehicleID";

            }
            cmbVehicleNo.SelectedIndex = -1;
        }

        private void LoadServiceTypes()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT ServiceTypeID, ServiceName FROM servicetypes ORDER BY ServiceName", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbServiceType.DataSource = dt;
                cmbServiceType.DisplayMember = "ServiceName";
                cmbServiceType.ValueMember = "ServiceTypeID";
            }
        }
        
        private void UC_Jobs_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible) { LoadVehicles(); LoadJobs(); ClearForm(); }
        }

        private void LoadMechanics()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"SELECT u.UserID, u.Username 
                         FROM users u
                         INNER JOIN roles r ON u.RoleID = r.RoleID
                         WHERE r.RoleName = 'Mechanic' AND u.IsActive = 1 
                         ORDER BY u.Username";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbMechanic.DataSource = dt;
                cmbMechanic.DisplayMember = "Username";
                cmbMechanic.ValueMember = "UserID";
            }
        }

        private void LoadParts()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT ItemID, ItemName FROM stockitems WHERE Quantity > 0 ORDER BY ItemName", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbPart.DataSource = dt;
                cmbPart.DisplayMember = "ItemName";
                cmbPart.ValueMember = "ItemID";
            }
        }

        private void LoadJobs()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"SELECT JobID, VehicleID, ServiceTypeID, MechanicID, StatusID, 
                                 BillingStatus, ServiceDate, Description FROM jobs ORDER BY JobID DESC";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvJobs.DataSource = dt;
            }
        }

        private void LoadUsedParts()
        {
            if (selectedJobId == 0)
            {
                dgvUsedParts.DataSource = null;
                partsTotal = 0;
                lblPartsTotal.Text = "Rs. 0.00";
                btnCompleteJob.Enabled = false;
                return;
            }

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                string query = "SELECT JobPartID, JobID, ItemID, Quantity, Price, (Quantity * Price) AS SubTotal FROM jobparts WHERE JobID = @jobId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@jobId", selectedJobId);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvUsedParts.DataSource = dt;

                partsTotal = 0;
                foreach (DataRow row in dt.Rows)
                    partsTotal += Convert.ToDecimal(row["SubTotal"]);

                lblPartsTotal.Text = "Rs. " + partsTotal.ToString("N2");
                btnCompleteJob.Enabled = dt.Rows.Count > 0;
            }
        }

        private void EnablePartsSection()
        {
            pnlParts.Enabled = true;
            lblStepInfo.Text = "-> Add used parts or send job to billing";
        }

        private void DisablePartsSection()
        {
            pnlParts.Enabled = false;
            btnCompleteJob.Enabled = false;
            lblStepInfo.Text = "-> Create or select a job first";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbVehicleNo.SelectedValue == null || cmbServiceType.SelectedValue == null)
            {
                MessageBox.Show("Please select vehicle and service type.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO jobs (VehicleID, ServiceTypeID, MechanicID, StatusID, ServiceDate, Description, BillingStatus)
                                    VALUES (@vehicleId, @serviceTypeId, @mechanicId, 
                                    (SELECT StatusID FROM jobstatus WHERE StatusName = 'Pending'), 
                                    @serviceDate, @description, 'Not Ready')";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@vehicleId", cmbVehicleNo.SelectedValue);
                    cmd.Parameters.AddWithValue("@serviceTypeId", cmbServiceType.SelectedValue);
                    cmd.Parameters.AddWithValue("@mechanicId", cmbMechanic.SelectedValue ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@serviceDate", dtpServiceDate.Value.Date);
                    cmd.Parameters.AddWithValue("@description", txtDescription.Text.Trim());
                    cmd.ExecuteNonQuery();

                    selectedJobId = Convert.ToInt32(cmd.LastInsertedId);
                    lblJobID.Text = "JOB" + selectedJobId.ToString("000");
                }

                EnablePartsSection();
                LoadJobs();
                LoadUsedParts();
                MessageBox.Show("Job created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        
        private void btnAddPart_Click(object sender, EventArgs e)
        {
            if (selectedJobId == 0 || cmbPart.SelectedValue == null || !int.TryParse(txtQuantity.Text.Trim(), out int qty) || qty <= 0)
            {
                MessageBox.Show("Please check Job selection, Part selection or Quantity.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int itemId = Convert.ToInt32(cmbPart.SelectedValue);

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    MySqlCommand stockCheckCmd = new MySqlCommand("SELECT Price, Quantity FROM stockitems WHERE ItemID = @itemId", conn);
                    stockCheckCmd.Parameters.AddWithValue("@itemId", itemId);

                    using (MySqlDataReader reader = stockCheckCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal price = Convert.ToDecimal(reader["Price"]);
                            int currentStock = Convert.ToInt32(reader["Quantity"]);

                            if (qty > currentStock)
                            {
                                MessageBox.Show($"Insufficient stock! Only {currentStock} items left in stock.", "Stock Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtQuantity.Focus();
                                return; 
                            }


                            reader.Close();
                            string insertQuery = "INSERT INTO jobparts (JobID, ItemID, Quantity, Price) VALUES (@jobId, @itemId, @qty, @price)";
                            MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                            insertCmd.Parameters.AddWithValue("@jobId", selectedJobId);
                            insertCmd.Parameters.AddWithValue("@itemId", itemId);
                            insertCmd.Parameters.AddWithValue("@qty", qty);
                            insertCmd.Parameters.AddWithValue("@price", price);
                            insertCmd.ExecuteNonQuery();

                            string updateStockQuery = "UPDATE stockitems SET Quantity = Quantity - @qty WHERE ItemID = @itemId";
                            MySqlCommand stockCmd = new MySqlCommand(updateStockQuery, conn);
                            stockCmd.Parameters.AddWithValue("@qty", qty);
                            stockCmd.Parameters.AddWithValue("@itemId", itemId);
                            stockCmd.ExecuteNonQuery();
                        }
                    }
                }

                txtQuantity.Clear();
                LoadParts();
                LoadUsedParts();
                MessageBox.Show("Part added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnCompleteJob_Click(object sender, EventArgs e)
        {
            if (selectedJobId == 0) return;

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE jobs SET 
                                     StatusID = (SELECT StatusID FROM jobstatus WHERE StatusName = 'Completed'), 
                                     BillingStatus = 'Ready for Billing', 
                                     CompletedAt = NOW() 
                                     WHERE JobID = @jobId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@jobId", selectedJobId);
                    cmd.ExecuteNonQuery();
                }

                lblStepInfo.Text = "-> Job sent to billing";
                MessageBox.Show("Job status updated to Ready for Billing.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadJobs();
                LoadUsedParts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgvJobs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvJobs.Rows[e.RowIndex];
            selectedJobId = Convert.ToInt32(row.Cells["JobID"].Value);
            lblJobID.Text = "JOB" + selectedJobId.ToString("000");

            cmbVehicleNo.SelectedValue = row.Cells["VehicleID"].Value;
            cmbServiceType.SelectedValue = row.Cells["ServiceTypeID"].Value;
            cmbMechanic.SelectedValue = row.Cells["MechanicID"].Value ?? DBNull.Value;
            txtDescription.Text = row.Cells["Description"].Value?.ToString();

            if (DateTime.TryParse(row.Cells["ServiceDate"].Value?.ToString(), out DateTime serviceDate))
                dtpServiceDate.Value = serviceDate;

            EnablePartsSection();
            LoadUsedParts();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadJobs();
        }

        private void ClearForm()
        {
            selectedJobId = 0;
            partsTotal = 0;
            lblJobID.Text = "NEW";
            lblPartsTotal.Text = "Rs. 0.00";
            txtDescription.Clear();
            txtQuantity.Clear();
            dtpServiceDate.Value = DateTime.Now;
            dgvUsedParts.DataSource = null;

            cmbVehicleNo.SelectedIndex = -1;
            cmbServiceType.SelectedIndex = -1;
            cmbMechanic.SelectedIndex = -1;
            cmbPart.SelectedIndex = -1;

            DisablePartsSection();
        }
    }
}