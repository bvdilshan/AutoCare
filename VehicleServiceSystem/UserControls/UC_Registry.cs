using System;
using System.Data;
using System.Text.RegularExpressions; 
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using VehicleServiceSystem.Data;

namespace VehicleServiceSystem.UserControls
{
    public partial class UC_Registry : UserControl
    {
        private int selectedCustomerId = 0;
        private int selectedVehicleId = 0;

        public UC_Registry()
        {
            InitializeComponent();

            btnSave.Click += btnSave_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            btnSearch.Click += btnSearch_Click;
            dgvRegistry.CellClick += dgvRegistry_CellClick;


            txtContactNo.KeyPress += txtContactNo_KeyPress;
            txtMileage.KeyPress += txtContactNo_KeyPress;
        }
        // Load event handler for the UserControl
        private void UC_Registry_Load(object sender, EventArgs e)
        {
            LoadRegistryData();
            ClearForm();
        }
        // Method to load registry data into the DataGridView
        private void LoadRegistryData()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                SELECT
                    c.CustomerID,
                    v.VehicleID,
                    c.CustomerName,
                    c.Phone,
                    c.Email,
                    c.Address,
                    v.VehicleNumber,
                    vb.BrandName,
                    v.Model,
                    vt.VehicleTypeName,
                    v.Mileage
                FROM Vehicles v
                INNER JOIN Customers c ON v.CustomerID = c.CustomerID
                LEFT JOIN VehicleBrands vb ON v.BrandID = vb.BrandID
                LEFT JOIN VehicleTypes vt ON v.VehicleTypeID = vt.VehicleTypeID
                ORDER BY v.VehicleID DESC";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvRegistry.DataSource = dt;

                if (dgvRegistry.Columns.Contains("CustomerID"))
                    dgvRegistry.Columns["CustomerID"].HeaderText = "Customer ID";

                if (dgvRegistry.Columns.Contains("VehicleID"))
                    dgvRegistry.Columns["VehicleID"].Visible = false;
            }
        }
        // Method to get or create a brand ID based on the brand name

        private int GetOrCreateBrandId(string brandName)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                // Check if the brand already exists
                string selectQuery = "SELECT BrandID FROM VehicleBrands WHERE BrandName = @brand";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                selectCmd.Parameters.AddWithValue("@brand", brandName);

                object result = selectCmd.ExecuteScalar();

                if (result != null)
                    return Convert.ToInt32(result);

                string insertQuery = "INSERT INTO VehicleBrands (BrandName) VALUES (@brand)";
                MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@brand", brandName);
                insertCmd.ExecuteNonQuery();

                return Convert.ToInt32(insertCmd.LastInsertedId);
            }
        }
        // Method to get the vehicle type ID based on the type name
        private int GetVehicleTypeId(string typeName)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                // Check if the vehicle type exists
                string query = "SELECT VehicleTypeID FROM VehicleTypes WHERE VehicleTypeName = @type";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@type", typeName);

                object result = cmd.ExecuteScalar();
                return result == null ? 0 : Convert.ToInt32(result);
            }
        }
        // Method to validate email format
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return true;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
        // Event handler for the Save button click event
        private void btnSave_Click(object sender, EventArgs e)
        {
            string customerName = txtCustomerName.Text.Trim();
            string phone = txtContactNo.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtAddress.Text.Trim();

            string vehicleNo = txtVehicleNo.Text.Trim();
            string brand = txtVehicleBrand.Text.Trim();
            string model = txtVehicleModel.Text.Trim();
            string vehicleType = cmbVehicleType.Text.Trim();

            int mileage = 0;

            int.TryParse(txtMileage.Text.Trim(), out mileage);

            if (customerName == "" || phone == "" || vehicleNo == "" || brand == "" || model == "")
            {
                MessageBox.Show("Please fill all required customer and vehicle details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (phone.Length < 10)
            {
                MessageBox.Show("Please enter a valid 10-digit contact number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter valid email address (e.g., example@gmail.com).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int brandId = GetOrCreateBrandId(brand);
                int vehicleTypeId = GetVehicleTypeId(vehicleType);

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    MySqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        string customerQuery = @"
                            INSERT INTO Customers (CustomerName, Phone, Email, Address)
                            VALUES (@name, @phone, @email, @address)";

                        MySqlCommand customerCmd = new MySqlCommand(customerQuery, conn, transaction);
                        customerCmd.Parameters.AddWithValue("@name", customerName);
                        customerCmd.Parameters.AddWithValue("@phone", phone);
                        customerCmd.Parameters.AddWithValue("@email", email);
                        customerCmd.Parameters.AddWithValue("@address", address);
                        customerCmd.ExecuteNonQuery();

                        int customerId = Convert.ToInt32(customerCmd.LastInsertedId);

                        string vehicleQuery = @"
                            INSERT INTO Vehicles 
                            (CustomerID, VehicleNumber, BrandID, Model, VehicleTypeID, Mileage)
                            VALUES
                            (@customerId, @vehicleNo, @brandId, @model, @vehicleTypeId, @mileage)";

                        MySqlCommand vehicleCmd = new MySqlCommand(vehicleQuery, conn, transaction);
                        vehicleCmd.Parameters.AddWithValue("@customerId", customerId);
                        vehicleCmd.Parameters.AddWithValue("@vehicleNo", vehicleNo);
                        vehicleCmd.Parameters.AddWithValue("@brandId", brandId);
                        vehicleCmd.Parameters.AddWithValue("@model", model);
                        vehicleCmd.Parameters.AddWithValue("@vehicleTypeId", vehicleTypeId);
                        vehicleCmd.Parameters.AddWithValue("@mileage", mileage);
                        vehicleCmd.ExecuteNonQuery();

                        transaction.Commit();

                        MessageBox.Show("Registry saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadRegistryData();
                        ClearForm();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Event handler for the Update button click event
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == 0 || selectedVehicleId == 0)
            {
                MessageBox.Show("Please select a record from the table.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string email = txtEmail.Text.Trim();
            string phone = txtContactNo.Text.Trim();

            if (phone.Length < 10)
            {
                MessageBox.Show("Please enter a valid 10-digit contact number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int brandId = GetOrCreateBrandId(txtVehicleBrand.Text.Trim());
                int vehicleTypeId = GetVehicleTypeId(cmbVehicleType.Text.Trim());

                int mileage = 0;
                int.TryParse(txtMileage.Text.Trim(), out mileage);

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        UPDATE Customers
                        SET CustomerName = @customerName,
                            Phone = @phone,
                            Email = @email,
                            Address = @address
                        WHERE CustomerID = @customerId;

                        UPDATE Vehicles
                        SET VehicleNumber = @vehicleNo,
                            BrandID = @brandId,
                            Model = @model,
                            VehicleTypeID = @vehicleTypeId,
                            Mileage = @mileage
                        WHERE VehicleID = @vehicleId;";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@customerName", txtCustomerName.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@customerId", selectedCustomerId);

                    cmd.Parameters.AddWithValue("@vehicleNo", txtVehicleNo.Text.Trim());
                    cmd.Parameters.AddWithValue("@brandId", brandId);
                    cmd.Parameters.AddWithValue("@model", txtVehicleModel.Text.Trim());
                    cmd.Parameters.AddWithValue("@vehicleTypeId", vehicleTypeId);
                    cmd.Parameters.AddWithValue("@mileage", mileage);
                    cmd.Parameters.AddWithValue("@vehicleId", selectedVehicleId);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Registry updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRegistryData();
                ClearForm();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedVehicleId == 0)
            {
                MessageBox.Show("Please select a vehicle from the table.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this vehicle and all its related history?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = "DELETE FROM vehicles WHERE VehicleID = @vehicleId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@vehicleId", selectedVehicleId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Vehicle deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRegistryData();
                ClearForm();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Cannot delete this vehicle: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Event handler for the Search button click event
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT
                        c.CustomerID,
                        v.VehicleID,
                        c.CustomerName,
                        c.Phone,
                        c.Email,
                        c.Address,
                        v.VehicleNumber,
                        vb.BrandName,
                        v.Model,
                        vt.VehicleTypeName,
                        v.Mileage
                    FROM Vehicles v
                    INNER JOIN Customers c ON v.CustomerID = c.CustomerID
                    LEFT JOIN VehicleBrands vb ON v.BrandID = vb.BrandID
                    LEFT JOIN VehicleTypes vt ON v.VehicleTypeID = vt.VehicleTypeID
                    WHERE c.CustomerID LIKE @search
                       OR c.CustomerName LIKE @search
                       OR v.VehicleNumber LIKE @search
                    ORDER BY v.VehicleID DESC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvRegistry.DataSource = dt;
            }
        }
        // Event handler for the DataGridView cell click event
        private void dgvRegistry_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvRegistry.Rows[e.RowIndex];

            selectedCustomerId = Convert.ToInt32(row.Cells["CustomerID"].Value);
            selectedVehicleId = Convert.ToInt32(row.Cells["VehicleID"].Value);

            txtCustomerId.Text = selectedCustomerId.ToString();
            txtCustomerName.Text = row.Cells["CustomerName"].Value?.ToString();
            txtContactNo.Text = row.Cells["Phone"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value?.ToString();
            txtAddress.Text = row.Cells["Address"].Value?.ToString();

            txtVehicleNo.Text = row.Cells["VehicleNumber"].Value?.ToString();
            txtVehicleBrand.Text = row.Cells["BrandName"].Value?.ToString();
            txtVehicleModel.Text = row.Cells["Model"].Value?.ToString();
            cmbVehicleType.Text = row.Cells["VehicleTypeName"].Value?.ToString();
            txtMileage.Text = row.Cells["Mileage"].Value?.ToString();
        }
        // Event handler for the Clear button click event
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            selectedCustomerId = 0;
            selectedVehicleId = 0;

            txtCustomerId.Clear();
            txtCustomerName.Clear();
            txtContactNo.Clear();
            txtEmail.Clear();
            txtAddress.Clear();

            txtVehicleNo.Clear();
            txtVehicleBrand.Clear();
            txtVehicleModel.Clear();
            txtMileage.Clear();
            txtSearch.Clear();

            txtCustomerName.Focus();
        }
        // Event handler to restrict input to digits only for contact number and mileage
        private void txtContactNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void lblMileage_Click(object sender, EventArgs e)
        {

        }
    }
}