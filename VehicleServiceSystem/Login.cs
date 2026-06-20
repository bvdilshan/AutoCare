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
        // Event handler for the login button click event
        private void signBtn_Click(object sender, EventArgs e)
        {
            string username = unamebox.Text.Trim();
            string password = pwordbox.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter username and password.",
                    "Login Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            u.UserID,
                            u.Username,
                            u.FullName,
                            r.RoleName
                        FROM Users u
                        INNER JOIN Roles r ON u.RoleID = r.RoleID
                        WHERE u.Username = @username
                          AND u.PasswordHash = @password
                          AND u.IsActive = 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = Convert.ToInt32(reader["UserID"]);
                                string role = reader["RoleName"].ToString();
                                string fullName = reader["FullName"].ToString();

                                MessageBox.Show(
                                    "Login successful as " + role,
                                    "Success",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                Dashboard dashboard = new Dashboard(role);
                                dashboard.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show(
                                    "Invalid username or password.",
                                    "Login Failed",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);

                                pwordbox.Clear();
                                pwordbox.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database connection error:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void unamebox_TextChanged(object sender, EventArgs e)
        {
        }

        private void pwordbox_TextChanged(object sender, EventArgs e)
        {
        }

        private void guna2PictureBox1_Click_1(object sender, EventArgs e)
        {
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {
        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {
        }

        private void guna2HtmlLabel5_Click(object sender, EventArgs e)
        {
        }

        private void lblTagline_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
//guna2Button1