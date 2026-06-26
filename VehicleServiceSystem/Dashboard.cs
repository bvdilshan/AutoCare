using Guna.UI2.WinForms;
using VehicleServiceSystem.UserControls;

namespace VehicleServiceSystem
{
    public partial class Dashboard : Form
    {
        private string userRole;

        public Dashboard()
        {
            InitializeComponent();
            LoadHome();
        }
        // Overloaded constructor that accepts the user's role and applies permissions accordingly
        public Dashboard(string role)
        {
            InitializeComponent();
            userRole = role;
            ApplyRolePermissions();
            LoadHome();
        }
        // This method loads the home user control into the main panel
        private void LoadHome()
        {
            dbpanel.Controls.Clear();
            UC_Home home = new UC_Home();
            home.Dock = DockStyle.Fill;
            dbpanel.Controls.Add(home);
        }
        // This method sets the visibility of buttons based on the user's role
        private void ApplyRolePermissions()
        {
            if (userRole == "Admin")
            {
                btnHome.Visible = true;
                btnRegistry.Visible = true;
                btnJobs.Visible = true;
                btnStock.Visible = true;
                btnBilling.Visible = true;
                btnReport.Visible = true;
                btnsettings.Visible = true;
            }
            else if (userRole == "Supervisor")
            {
                btnHome.Visible = false;
                btnRegistry.Visible = true;
                btnJobs.Visible = true;
                btnStock.Visible = true;

                btnBilling.Visible = false;
                btnReport.Visible = false;
                btnsettings.Visible = false;
            }
            else if (userRole == "Cashier")
            {
                btnHome.Visible = false;
                btnBilling.Visible = true;
                btnRegistry.Visible = false;
                btnJobs.Visible = false;
                btnStock.Visible = false;
                btnReport.Visible = false;
                btnsettings.Visible = false;
            }
            else if (userRole == "Mechanic")
            {
                btnHome.Visible = false;
                btnJobs.Visible = true;

                btnRegistry.Visible = false;
                btnStock.Visible = false;
                btnBilling.Visible = false;
                btnReport.Visible = false;
                btnsettings.Visible = false;
            }
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            LoadHome();
        }

        private void btnRegistry_Click(object sender, EventArgs e)
        {
            dbpanel.Controls.Clear();
            UC_Registry uc = new UC_Registry();
            uc.Dock = DockStyle.Fill;
            dbpanel.Controls.Add(uc);
        }

        private void btnJobs_Click(object sender, EventArgs e)
        {
            dbpanel.Controls.Clear();
            UC_Jobs uc = new UC_Jobs();
            uc.Dock = DockStyle.Fill;
            dbpanel.Controls.Add(uc);
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            dbpanel.Controls.Clear();
            UC_Stock uc = new UC_Stock();
            uc.Dock = DockStyle.Fill;
            dbpanel.Controls.Add(uc);
        }

        private void btnBilling_Click(object sender, EventArgs e)
        {
            dbpanel.Controls.Clear();
            UC_billing uc = new UC_billing();
            uc.Dock = DockStyle.Fill;
            dbpanel.Controls.Add(uc);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            dbpanel.Controls.Clear();
            UC_Reports uc = new UC_Reports();
            uc.Dock = DockStyle.Fill;
            dbpanel.Controls.Add(uc);
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            dbpanel.Controls.Clear();
            UC_Settings uc = new UC_Settings();
            uc.Dock = DockStyle.Fill;
            dbpanel.Controls.Add(uc);
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {

        }

        private void dbpanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}