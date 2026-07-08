using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using VehicleServiceSystem.Data;

namespace VehicleServiceSystem
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            pwordbox.PasswordChar = '*';
        }

        private void signBtn_Click(object sender, EventArgs e)
        {
            string username = unamebox.Text.Trim();
            string password = pwordbox.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter username and password.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                pwordbox.Focus();
                return;
            }

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT u.UserID, u.Username, u.FullName, r.RoleName " +
                                   "FROM Users u " +
                                   "INNER JOIN Roles r ON u.RoleID = r.RoleID " +
                                   "WHERE u.Username = @username AND u.PasswordHash = @password AND u.IsActive = 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string role = reader["RoleName"].ToString();
                                string fullName = reader["FullName"].ToString();

                                MessageBox.Show("Login successful as " + role, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                Dashboard dashboard = new Dashboard(role);
                                dashboard.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                pwordbox.Clear();
                                pwordbox.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}