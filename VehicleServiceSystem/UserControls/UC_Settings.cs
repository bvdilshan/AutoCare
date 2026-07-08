using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using VehicleServiceSystem.Data;

namespace VehicleServiceSystem.UserControls
{
    public partial class UC_Settings : UserControl
    {
        private int selectedUserId = 0;

        public UC_Settings()
        {
            InitializeComponent();
            btnAddUser.Click += btnAddUser_Click;
            btnUpdateUser.Click += btnUpdateUser_Click;
            btnDeleteUser.Click += btnDeleteUser_Click;
            btnSearchUser.Click += btnSearchUser_Click;
            btnClearUser.Click += btnClearUser_Click;
            dgvUserList.CellClick += dgvUserList_CellClick;
            txtPhone.KeyPress += (s, e) => e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void UC_Settings_Load(object sender, EventArgs e)
        {
            LoadRoles();
            LoadUsers();
            ClearForm();
        }

        private void LoadRoles()
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    string query = "SELECT RoleID, RoleName FROM roles ORDER BY RoleID";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbRole.DataSource = dt;
                    cmbRole.DisplayMember = "RoleName";
                    cmbRole.ValueMember = "RoleID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load roles error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cmbRole.SelectedIndex = -1;
        }

        private void LoadUsers()
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    string query = @"SELECT u.UserID, u.Username, u.Phone, r.RoleName, u.RoleID, u.IsActive 
                                     FROM users u 
                                     INNER JOIN roles r ON u.RoleID = r.RoleID 
                                     ORDER BY u.UserID DESC";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvUserList.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load users error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs(bool isUpdate = false)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all required fields (Username and Role).", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!isUpdate && string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Password is required for a new user.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (!isUpdate && txtPassword.Text.Trim().Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (isUpdate && !string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text.Trim().Length < 8)
            {
                MessageBox.Show("New password must be at least 8 characters long.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(txtPhone.Text) && txtPhone.Text.Trim().Length != 10)
            {
                MessageBox.Show("Phone number must be exactly 10 digits.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            return true;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(isUpdate: false)) return;

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = @username", conn);
                    checkCmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("This username already exists!", "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string query = @"INSERT INTO users (Username, PasswordHash, FullName, Phone, RoleID, IsActive) 
                                     VALUES (@username, @password, @fullname, @phone, @roleId, 1)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@fullname", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@roleId", cmbRole.SelectedValue);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("User saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add user error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            if (selectedUserId == 0) { MessageBox.Show("Please select a user from the list."); return; }
            if (!ValidateInputs(isUpdate: true)) return;

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string password = txtPassword.Text.Trim();

                    string query = "UPDATE users SET Username = @username, Phone = @phone, RoleID = @roleId " +
                                   (password != "" ? ", PasswordHash = @password " : "") +
                                   "WHERE UserID = @userId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@roleId", cmbRole.SelectedValue);
                    cmd.Parameters.AddWithValue("@userId", selectedUserId);

                    if (password != "") cmd.Parameters.AddWithValue("@password", password);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("User updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
                ClearForm();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update user error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (selectedUserId == 0) return;

            if (MessageBox.Show("Deactivate this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE users SET IsActive = 0 WHERE UserID = @userId", conn);
                    cmd.Parameters.AddWithValue("@userId", selectedUserId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("User deactivated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvUserList.Rows[e.RowIndex];
            selectedUserId = Convert.ToInt32(row.Cells["UserID"].Value);

            lblEmpID.Text = "USR" + selectedUserId.ToString("000");
            txtUsername.Text = row.Cells["Username"].Value?.ToString();
            txtPhone.Text = row.Cells["Phone"].Value?.ToString();
            btnAddUser.Enabled = false;

            if (dgvUserList.Columns.Contains("RoleID"))
                cmbRole.SelectedValue = row.Cells["RoleID"].Value;

            txtPassword.Clear();
        }

        private void btnSearchUser_Click(object sender, EventArgs e)
        {
            string search = txtSearchUser.Text.Trim();
            if (search == "") { LoadUsers(); return; }

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    string query = @"SELECT u.UserID, u.Username, u.Phone, r.RoleName, u.RoleID, u.IsActive 
                                     FROM users u 
                                     INNER JOIN roles r ON u.RoleID = r.RoleID 
                                     WHERE u.Username LIKE @search 
                                     ORDER BY u.UserID DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvUserList.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search user error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearUser_Click(object sender, EventArgs e) { ClearForm(); }

        private void ClearForm()
        {
            selectedUserId = 0;
            lblEmpID.Text = "NEW";
            txtUsername.Clear();
            txtPassword.Clear();
            txtPhone.Clear();
            txtSearchUser.Clear();
            cmbRole.SelectedIndex = -1;
            txtUsername.Focus();
            btnAddUser.Enabled = true;
        }
    }
}