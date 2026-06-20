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
        }

        // Load event handler for the UserControl
        private void UC_Jobs_Load(object sender, EventArgs e)
        {
            LoadVehicles();
            LoadServiceTypes();
            LoadMechanics();
            LoadParts();
            LoadJobs();
            ClearForm();
        }

        // Methods to load data into combo boxes and data grids
        private void LoadVehicles()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"SELECT VehicleID, VehicleNumber FROM vehicles 
                         WHERE VehicleID NOT IN (
                             SELECT VehicleID FROM jobs WHERE StatusID != 3 AND StatusID != 4
                         ) ORDER BY VehicleNumber";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbVehicleNo.DataSource = dt;
                cmbVehicleNo.DisplayMember = "VehicleNumber";
                cmbVehicleNo.ValueMember = "VehicleID";
            }
        }

        // Load service types into the combo box
        private void LoadServiceTypes()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = "SELECT ServiceTypeID, ServiceName FROM servicetypes ORDER BY ServiceName";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbServiceType.DataSource = dt;
                cmbServiceType.DisplayMember = "ServiceName";
                cmbServiceType.ValueMember = "ServiceTypeID";
            }
        }

        // Load event handler for when the UserControl becomes visible  refreshes data
        private void UC_Jobs_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                LoadVehicles();
                LoadJobs();
            }
        }

        // Load mechanics into the combo box
        private void LoadMechanics()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT u.UserID, u.Username
                    FROM users u
                    INNER JOIN roles r ON u.RoleID = r.RoleID
                    WHERE r.RoleName = 'Mechanic'
                    AND u.IsActive = 1
                    ORDER BY u.Username";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbMechanic.DataSource = dt;
                cmbMechanic.DisplayMember = "Username";
                cmbMechanic.ValueMember = "UserID";
            }
        }

        // Load parts into the combo box
        private void LoadParts()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT ItemID, ItemName, Price, Quantity
                    FROM stockitems
                    WHERE Quantity > 0
                    ORDER BY ItemName";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbPart.DataSource = dt;
                cmbPart.DisplayMember = "ItemName";
                cmbPart.ValueMember = "ItemID";
            }
        }

        // Load jobs into the data grid (Cleaned from Priority)
        private void LoadJobs()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT
                        j.JobID,
                        v.VehicleNumber,
                        st.ServiceName,
                        u.Username AS Mechanic,
                        IFNULL(j.BillingStatus, 'Not Ready') AS BillingStatus,
                        j.ServiceDate,
                        j.Description
                    FROM jobs j
                    INNER JOIN vehicles v ON j.VehicleID = v.VehicleID
                    INNER JOIN servicetypes st ON j.ServiceTypeID = st.ServiceTypeID
                    LEFT JOIN users u ON j.MechanicID = u.UserID
                    ORDER BY j.JobID DESC";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvJobs.DataSource = dt;
            }
        }

        // Load used parts for the selected job into the data grid
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
                conn.Open();

                string query = @"
                    SELECT
                        si.ItemName,
                        jp.Quantity,
                        jp.Price,
                        (jp.Quantity * jp.Price) AS SubTotal
                    FROM jobparts jp
                    INNER JOIN stockitems si ON jp.ItemID = si.ItemID
                    WHERE jp.JobID = @jobId
                    ORDER BY jp.JobPartID DESC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@jobId", selectedJobId);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvUsedParts.DataSource = dt;

                partsTotal = 0;

                foreach (DataRow row in dt.Rows)
                {
                    partsTotal += Convert.ToDecimal(row["SubTotal"]);
                }

                lblPartsTotal.Text = "Rs. " + partsTotal.ToString("N2");
                btnCompleteJob.Enabled = dt.Rows.Count > 0;
            }
        }
        // Enable parts section when a job is selected or created
        private void EnablePartsSection()
        {
            pnlParts.Enabled = true;
            lblStepInfo.Text = "Step 2: Add used parts or send job to billing";
        }

        // Disable parts section when no job is selected
        private void DisablePartsSection()
        {
            pnlParts.Enabled = false;
            btnCompleteJob.Enabled = false;
            lblStepInfo.Text = "Step 1: Create or select a job first";
        }

        // Event handler for the Save button click - creates a new job 
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbVehicleNo.SelectedValue == null || cmbServiceType.SelectedValue == null)
            {
                MessageBox.Show("Please select vehicle and service type.");
                return;
            }

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO jobs
                        (VehicleID, ServiceTypeID, MechanicID, StatusID, ServiceDate, Description, BillingStatus)
                        VALUES
                        (
                            @vehicleId,
                            @serviceTypeId,
                            @mechanicId,
                            (SELECT StatusID FROM jobstatus WHERE StatusName = 'Pending' LIMIT 1),
                            @serviceDate,
                            @description,
                            'Not Ready'
                         )";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@vehicleId", cmbVehicleNo.SelectedValue);
                    cmd.Parameters.AddWithValue("@serviceTypeId", cmbServiceType.SelectedValue);
                    cmd.Parameters.AddWithValue("@mechanicId", cmbMechanic.SelectedValue == null ? DBNull.Value : cmbMechanic.SelectedValue);
                    cmd.Parameters.AddWithValue("@serviceDate", dtpServiceDate.Value.Date);
                    cmd.Parameters.AddWithValue("@description", txtDescription.Text.Trim());

                    cmd.ExecuteNonQuery();

                    selectedJobId = Convert.ToInt32(cmd.LastInsertedId);
                    lblJobID.Text = "JOB" + selectedJobId.ToString("000");
                }

                EnablePartsSection();
                LoadJobs();
                LoadUsedParts();

                MessageBox.Show("Job created. You can add parts now.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Create job error: " + ex.Message);
            }
        }

        // Event handler for the Add Part button click - adds a part to the selected job
        private void btnAddPart_Click(object sender, EventArgs e)
        {
            if (selectedJobId == 0)
            {
                MessageBox.Show("Please create or select a job first.");
                return;
            }

            if (cmbPart.SelectedValue == null)
            {
                MessageBox.Show("Please select a part.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int qty) || qty <= 0)
            {
                MessageBox.Show("Please enter valid quantity.");
                return;
            }

            try
            {
                int itemId = Convert.ToInt32(cmbPart.SelectedValue);

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();

                    try
                    {
                        string getItemQuery = @"
                            SELECT Price, Quantity
                            FROM stockitems
                            WHERE ItemID = @itemId
                            FOR UPDATE";

                        MySqlCommand itemCmd = new MySqlCommand(getItemQuery, conn, tr);
                        itemCmd.Parameters.AddWithValue("@itemId", itemId);

                        decimal price = 0;
                        int availableQty = 0;

                        using (MySqlDataReader reader = itemCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                price = Convert.ToDecimal(reader["Price"]);
                                availableQty = Convert.ToInt32(reader["Quantity"]);
                            }
                            else
                            {
                                MessageBox.Show("Part not found.");
                                tr.Rollback();
                                return;
                            }
                        }

                        if (qty > availableQty)
                        {
                            MessageBox.Show("Not enough stock. Available: " + availableQty);
                            tr.Rollback();
                            return;
                        }

                        string insertPartQuery = @"
                            INSERT INTO jobparts
                            (JobID, ItemID, Quantity, Price)
                            VALUES
                            (@jobId, @itemId, @qty, @price)";

                        MySqlCommand insertCmd = new MySqlCommand(insertPartQuery, conn, tr);
                        insertCmd.Parameters.AddWithValue("@jobId", selectedJobId);
                        insertCmd.Parameters.AddWithValue("@itemId", itemId);
                        insertCmd.Parameters.AddWithValue("@qty", qty);
                        insertCmd.Parameters.AddWithValue("@price", price);
                        insertCmd.ExecuteNonQuery();

                        string updateStockQuery = @"
                            UPDATE stockitems
                            SET Quantity = Quantity - @qty
                            WHERE ItemID = @itemId";

                        MySqlCommand stockCmd = new MySqlCommand(updateStockQuery, conn, tr);
                        stockCmd.Parameters.AddWithValue("@qty", qty);
                        stockCmd.Parameters.AddWithValue("@itemId", itemId);
                        stockCmd.ExecuteNonQuery();

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }

                txtQuantity.Clear();
                LoadParts();
                LoadUsedParts();

                MessageBox.Show("Part added. You can add more parts or send to billing.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add part error: " + ex.Message);
            }
        }

        // Event handler for the Complete Job button click - sends the job to billing
        private void btnCompleteJob_Click(object sender, EventArgs e)
        {
            if (selectedJobId == 0)
            {
                MessageBox.Show("Please create or select a job first.");
                return;
            }

            if (dgvUsedParts.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one part before sending to billing.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Send this job to cashier billing?",
                "Send to Billing",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        UPDATE jobs
                        SET StatusID = (SELECT StatusID FROM jobstatus WHERE StatusName = 'Completed' LIMIT 1),
                            BillingStatus = 'Ready for Billing',
                            CompletedAt = NOW()
                        WHERE JobID = @jobId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@jobId", selectedJobId);
                    cmd.ExecuteNonQuery();
                }

                lblStepInfo.Text = "Step 3: Job sent to billing";
                MessageBox.Show("Job sent to cashier billing.");

                LoadJobs();
                LoadUsedParts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Send to billing error: " + ex.Message);
            }
        }

        // Event handler for cell click in the jobs data grid
        private void dgvJobs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvJobs.Rows[e.RowIndex];

            selectedJobId = Convert.ToInt32(row.Cells["JobID"].Value);
            lblJobID.Text = "JOB" + selectedJobId.ToString("000");

            cmbVehicleNo.Text = row.Cells["VehicleNumber"].Value?.ToString();
            cmbServiceType.Text = row.Cells["ServiceName"].Value?.ToString();
            cmbMechanic.Text = row.Cells["Mechanic"].Value?.ToString();
            txtDescription.Text = row.Cells["Description"].Value?.ToString();

            if (DateTime.TryParse(row.Cells["ServiceDate"].Value?.ToString(), out DateTime serviceDate))
                dtpServiceDate.Value = serviceDate;

            EnablePartsSection();
            LoadUsedParts();
        }

        // Event handler for the Clear button click - resets the form
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadJobs();
        }

        // Method to clear the form and reset all fields (Cleaned from Priority)
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

            if (cmbVehicleNo.Items.Count > 0)
                cmbVehicleNo.SelectedIndex = 0;

            if (cmbServiceType.Items.Count > 0)
                cmbServiceType.SelectedIndex = 0;

            if (cmbMechanic.Items.Count > 0)
                cmbMechanic.SelectedIndex = 0;

            if (cmbPart.Items.Count > 0)
                cmbPart.SelectedIndex = 0;

            DisablePartsSection();
        }
    }
}
