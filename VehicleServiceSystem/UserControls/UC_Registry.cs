using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using VehicleServiceSystem.Data;
using System.Drawing;

namespace VehicleServiceSystem.UserControls
{
    public partial class UC_Registry : UserControl
    {
        private int selectedCustomerId = 0;
        private int selectedVehicleId = 0;

        public UC_Registry()
        {
            InitializeComponent();
            this.Load += UC_Registry_Load;
            btnSave.Click += btnSave_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            btnSearch.Click += btnSearch_Click;
            dgvRegistry.CellClick += dgvRegistry_CellClick;

            txtContactNo.KeyPress += txtContactNo_KeyPress;
            txtMileage.KeyPress += txtContactNo_KeyPress;
        }

        private void UC_Registry_Load(object sender, EventArgs e)
        {
            LoadRegistryData();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                try
                {
                    conn.Open();

                    // load vehicle brands
                    string queryBrand = "SELECT BrandID, BrandName FROM vehiclebrands ORDER BY BrandName ASC";
                    MySqlDataAdapter daBrand = new MySqlDataAdapter(queryBrand, conn);
                    DataTable dtBrand = new DataTable();
                    daBrand.Fill(dtBrand);

                    cmbVehicleBrand.DataSource = dtBrand;
                    cmbVehicleBrand.DisplayMember = "BrandName";
                    cmbVehicleBrand.ValueMember = "BrandID";

                    // load vehicle types
                    string queryType = "SELECT VehicleTypeID, VehicleTypeName FROM vehicletypes ORDER BY VehicleTypeName ASC";
                    MySqlDataAdapter daType = new MySqlDataAdapter(queryType, conn);
                    DataTable dtType = new DataTable();
                    daType.Fill(dtType);

                    cmbVehicleType.DataSource = dtType;
                    cmbVehicleType.DisplayMember = "VehicleTypeName";
                    cmbVehicleType.ValueMember = "VehicleTypeID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading dropdown data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cmbVehicleBrand.SelectedIndex = -1;
                cmbVehicleType.SelectedIndex = -1;
            }
            ClearForm();
        }

        private void LoadRegistryData()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"SELECT v.VehicleID, c.CustomerID, c.CustomerName, c.Phone, c.Email, c.Address, 
                                 v.VehicleNumber, v.BrandID, v.VehicleTypeID, v.Model, v.Mileage FROM vehicles v 
                                 INNER JOIN customers c ON v.CustomerID = c.CustomerID ORDER BY v.VehicleID DESC";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvRegistry.DataSource = dt;
            }
        }

        // email validation
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
        }

        // input validation
        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(txtCustomerName.Text) || string.IsNullOrEmpty(txtContactNo.Text) ||
                string.IsNullOrEmpty(txtVehicleNo.Text) || cmbVehicleBrand.SelectedIndex == -1 || cmbVehicleType.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all required fields.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtContactNo.Text.Trim().Length != 10)
            {
                MessageBox.Show("Phone number must be exactly 10 digits.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContactNo.Focus();
                return false;
            }

            
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                if (!IsValidEmail(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("The email address you entered is invalid. Please use acorrect format (e.g., vinoth@gmail.com).", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }

            return true; 
        }

        // Insert Vehicle and Customer
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return; 

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    // Insert customer
                    string customerQuery = "INSERT INTO customers (CustomerName, Phone, Email, Address) VALUES (@name, @phone, @email, @address)";
                    MySqlCommand customerCmd = new MySqlCommand(customerQuery, conn);
                    customerCmd.Parameters.AddWithValue("@name", txtCustomerName.Text.Trim());
                    customerCmd.Parameters.AddWithValue("@phone", txtContactNo.Text.Trim());
                    customerCmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    customerCmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                    customerCmd.ExecuteNonQuery();

                    int customerId = Convert.ToInt32(customerCmd.LastInsertedId);
                    int selectedBrandID = Convert.ToInt32(cmbVehicleBrand.SelectedValue);
                    int selectedTypeID = Convert.ToInt32(cmbVehicleType.SelectedValue);

                    // Insert vehicle
                    string vehicleQuery = @"INSERT INTO vehicles (CustomerID, VehicleNumber, BrandID, Model, VehicleTypeID, Mileage) 
                                            VALUES (@customerId, @vehicleNo, @brandId, @model, @typeId, @mileage)";

                    MySqlCommand vehicleCmd = new MySqlCommand(vehicleQuery, conn);
                    vehicleCmd.Parameters.AddWithValue("@customerId", customerId);
                    vehicleCmd.Parameters.AddWithValue("@vehicleNo", txtVehicleNo.Text.Trim());
                    vehicleCmd.Parameters.AddWithValue("@brandId", selectedBrandID);
                    vehicleCmd.Parameters.AddWithValue("@model", txtVehicleModel.Text.Trim());
                    vehicleCmd.Parameters.AddWithValue("@typeId", selectedTypeID);
                    vehicleCmd.Parameters.AddWithValue("@mileage", string.IsNullOrEmpty(txtMileage.Text) ? 0 : Convert.ToInt32(txtMileage.Text));
                    vehicleCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRegistryData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Update data
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedCustomerId == 0 || selectedVehicleId == 0) return;
            if (!ValidateInputs()) return;

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    int selectedBrandID = Convert.ToInt32(cmbVehicleBrand.SelectedValue);
                    int selectedTypeID = Convert.ToInt32(cmbVehicleType.SelectedValue);

                    string query = @"UPDATE customers SET CustomerName = @name, Phone = @phone, Email = @email, Address = @address WHERE CustomerID = @cId;
                                     UPDATE vehicles SET VehicleNumber = @vNo, BrandID = @brandId, VehicleTypeID = @typeId, Model = @model, Mileage = @mileage WHERE VehicleID = @vId;";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", txtCustomerName.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", txtContactNo.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@cId", selectedCustomerId);
                    cmd.Parameters.AddWithValue("@vNo", txtVehicleNo.Text.Trim());
                    cmd.Parameters.AddWithValue("@brandId", selectedBrandID);
                    cmd.Parameters.AddWithValue("@typeId", selectedTypeID);
                    cmd.Parameters.AddWithValue("@model", txtVehicleModel.Text.Trim());
                    cmd.Parameters.AddWithValue("@mileage", string.IsNullOrEmpty(txtMileage.Text) ? 0 : Convert.ToInt32(txtMileage.Text));
                    cmd.Parameters.AddWithValue("@vId", selectedVehicleId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRegistryData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Delete data
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedVehicleId == 0 || selectedCustomerId == 0)
            {
                MessageBox.Show("Please select a record from the list first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this vehicle and customer registration?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    // delete vehicles
                    string deleteVehicleQuery = "DELETE FROM vehicles WHERE VehicleID = @vId";
                    MySqlCommand cmdVehicle = new MySqlCommand(deleteVehicleQuery, conn);
                    cmdVehicle.Parameters.AddWithValue("@vId", selectedVehicleId);
                    cmdVehicle.ExecuteNonQuery();

                    // delete customers
                    string deleteCustomerQuery = "DELETE FROM customers WHERE CustomerID = @cId";
                    MySqlCommand cmdCustomer = new MySqlCommand(deleteCustomerQuery, conn);
                    cmdCustomer.Parameters.AddWithValue("@cId", selectedCustomerId);
                    cmdCustomer.ExecuteNonQuery();
                }

                MessageBox.Show("Deleted successfully from both tables.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRegistryData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot delete this record.: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"SELECT v.VehicleID, c.CustomerID, c.CustomerName, c.Phone, c.Email, c.Address, 
                                v.VehicleNumber, v.BrandID, v.VehicleTypeID, v.Model, v.Mileage 
                                FROM vehicles v 
                                INNER JOIN customers c ON v.CustomerID = c.CustomerID 
                                WHERE v.VehicleNumber LIKE @search";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text.Trim() + "%");
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvRegistry.DataSource = dt;
            }
        }

        private void dgvRegistry_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvRegistry.Rows[e.RowIndex];
            selectedCustomerId = Convert.ToInt32(row.Cells["CustomerID"].Value);
            selectedVehicleId = Convert.ToInt32(row.Cells["VehicleID"].Value);

            txtCustomerId.Text = selectedCustomerId.ToString();
            txtCustomerName.Text = row.Cells["CustomerName"].Value?.ToString();
            txtContactNo.Text = row.Cells["Phone"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value?.ToString();
            txtAddress.Text = row.Cells["Address"].Value?.ToString();
            txtVehicleNo.Text = row.Cells["VehicleNumber"].Value?.ToString();
            txtVehicleModel.Text = row.Cells["Model"].Value?.ToString();
            txtMileage.Text = row.Cells["Mileage"].Value?.ToString();
            btnSave.Enabled = false;

            cmbVehicleBrand.SelectedValue = row.Cells["BrandID"].Value;

            if (dgvRegistry.Columns.Contains("VehicleTypeID"))
            {
                cmbVehicleType.SelectedValue = row.Cells["VehicleTypeID"].Value;
            }
        }

        private void btnClear_Click(object sender, EventArgs e) { ClearForm(); }

        private void ClearForm()
        {
            selectedCustomerId = 0; selectedVehicleId = 0;
            txtCustomerId.Clear(); txtCustomerName.Clear(); txtContactNo.Clear();
            txtEmail.Clear(); txtAddress.Clear(); txtVehicleNo.Clear();
            txtVehicleModel.Clear(); txtMileage.Clear(); txtSearch.Clear();
            cmbVehicleBrand.SelectedIndex = -1;
            cmbVehicleType.SelectedIndex = -1;
            txtCustomerName.Focus();
            btnSave.Enabled = true;
        }

        private void txtContactNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }
    }
}