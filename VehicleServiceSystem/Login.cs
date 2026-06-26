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

            // මුද්‍රපදය (Password) ටයිප් කරද්දී වෙනත් අයට නොපෙනී '*' ලෙස පෙන්වීමට
            pwordbox.PasswordChar = '*';
        }

        // 1. ඇතුළත් වීමේ බොත්තම (Login Button) ක්ලික් කළ විට
        private void signBtn_Click(object sender, EventArgs e)
        {
            string username = unamebox.Text.Trim();
            string password = pwordbox.Text.Trim();

            // හිස්තැන් තිබේදැයි පරික්ෂා කිරීම (Validation)
            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter username and password.", "Login Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    // පරිශීලකයා නිවැරදිද, සක්‍රීයද (IsActive=1) සහ ඔහුගේ Role එක මොකක්ද කියා බලන Query එක
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
                            // දත්ත ගැලපේ නම් (User කෙනෙක් හමු වුණා නම්)
                            if (reader.Read())
                            {
                                string role = reader["RoleName"].ToString();
                                string fullName = reader["FullName"].ToString();

                                MessageBox.Show("Login successful as " + role, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Dashboard එකට අදාළ Role එක යවා Dashboard එක විවෘත කිරීම
                                Dashboard dashboard = new Dashboard(role);
                                dashboard.Show();
                                this.Hide(); // Login Form එක සඟවා තැබීම
                            }
                            else
                            {
                                // දත්ත වැරදි නම්
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

        //Exit Button
        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}