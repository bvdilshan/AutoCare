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
        }
        // Load event handler for the UserControl
        private void UC_Settings_Load(object sender, EventArgs e)
        {
            LoadRoles();
            LoadUsers();
            ClearForm();
        }
        // This method loads the roles from the database and binds them to the ComboBox
        private void LoadRoles()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = "SELECT RoleName FROM roles ORDER BY RoleID";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbRole.DataSource = dt;
                cmbRole.DisplayMember = "RoleName";
                cmbRole.ValueMember = "RoleName";
            }
        }
        // This method loads the users from the database and binds them to the DataGridView
        private void LoadUsers()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        u.UserID,
                        u.Username,
                        u.Phone,
                        r.RoleName,
                        u.IsActive
                    FROM users u
                    INNER JOIN roles r ON u.RoleID = r.RoleID
                    ORDER BY u.UserID DESC";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvUserList.DataSource = dt;
            }
        }
        // This method retrieves the RoleID based on the RoleName
        private int GetRoleId(string roleName)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = "SELECT RoleID FROM roles WHERE RoleName = @role LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@role", roleName);

                object result = cmd.ExecuteScalar();
                return result == null ? 0 : Convert.ToInt32(result);
            }
        }
        // Event handler for the Add User button click
        private void btnAddUser_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string role = cmbRole.Text.Trim();

            if (username == "" || password == "" || role == "")
            {
                MessageBox.Show("Please fill username, password and role.");
                return;
            }

            int roleId = GetRoleId(role);

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
                using (MySqlCommand check = new MySqlCommand(checkQuery, conn))
                {
                    check.Parameters.AddWithValue("@username", username);
                    int count = Convert.ToInt32(check.ExecuteScalar());
                    if (count>0)
                    {
                        MessageBox.Show("this username already exists !");
                        return;
                    }
                }

                    string query = @"
                    INSERT INTO users
                    (Username, PasswordHash, FullName, Phone, RoleID, IsActive)
                    VALUES
                    (@username, @password, @fullname, @phone, @roleId, 1)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@fullname", username);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@roleId", roleId);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("User saved successfully.");
            LoadUsers();
            ClearForm();
        }
        // Event handler for the Update User button click
        private void btnUpdateUser_Click_1(object sender, EventArgs e)
        {
            if (selectedUserId == 0)
            {
                MessageBox.Show("Please select a user first.");
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string role = cmbRole.Text.Trim();

            if (username == "" || role == "")
            {
                MessageBox.Show("Please fill username and role.");
                return;
            }

            int roleId = GetRoleId(role);

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query;

                if (password == "")
                {
                    query = @"
                        UPDATE users
                        SET Username = @username,
                            FullName = @fullname,
                            Phone = @phone,
                            RoleID = @roleId
                        WHERE UserID = @userId";
                }
                else
                {
                    query = @"
                        UPDATE users
                        SET Username = @username,
                            PasswordHash = @password,
                            FullName = @fullname,
                            Phone = @phone,
                            RoleID = @roleId
                        WHERE UserID = @userId";
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@fullname", username);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@roleId", roleId);
                cmd.Parameters.AddWithValue("@userId", selectedUserId);

                if (password != "")
                    cmd.Parameters.AddWithValue("@password", password);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("User updated successfully.");
            LoadUsers();
            ClearForm();
        }
        // Event handler for the Delete User button click
        private void btnDeleteUser_Click_1(object sender, EventArgs e)
        {
            if (selectedUserId == 0)
            {
                MessageBox.Show("Please select a user first.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Deactivate this user?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = "UPDATE users SET IsActive = 0 WHERE UserID = @userId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", selectedUserId);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("User deactivated successfully.");
            LoadUsers();
            ClearForm();
        }
        // Event handler for the DataGridView cell click to load user details into the form
        private void dgvUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvUserList.Rows[e.RowIndex];

            selectedUserId = Convert.ToInt32(row.Cells["UserID"].Value);
            lblEmpID.Text = "USR" + selectedUserId.ToString("000");

            txtUsername.Text = row.Cells["Username"].Value?.ToString();
            txtPhone.Text = row.Cells["Phone"].Value?.ToString();
            cmbRole.Text = row.Cells["RoleName"].Value?.ToString();

            txtPassword.Clear();
        }
        // Event handler for the Search User button click to filter users based on the search text
        private void btnSearchUser_Click(object sender, EventArgs e)
        {
            string search = txtSearchUser.Text.Trim();

            if (search == "")
            {
                LoadUsers();
                return;
            }

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        u.UserID,
                        u.Username,
                        u.Phone,
                        r.RoleName,
                        u.IsActive
                    FROM users u
                    INNER JOIN roles r ON u.RoleID = r.RoleID
                    WHERE u.Username LIKE @search
                       OR r.RoleName LIKE @search
                    ORDER BY u.UserID DESC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvUserList.DataSource = dt;
            }
        }
        // Event handler for the Clear button click to reset the form fields
        private void btnClearUser_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        // This method clears the form fields and resets the selected user ID
        private void ClearForm()
        {
            selectedUserId = 0;

            lblEmpID.Text = "NEW";
            txtUsername.Clear();
            txtPassword.Clear();
            txtPhone.Clear();
            txtSearchUser.Clear();

            if (cmbRole.Items.Count > 0)
                cmbRole.SelectedIndex = 0;

            txtUsername.Focus();
        }
    }
}