
namespace VehicleServiceSystem.UserControls
{
    partial class UC_billing
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle headerStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle rowStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle headerStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle rowStyle2 = new System.Windows.Forms.DataGridViewCellStyle();

            mainPanel = new Guna.UI2.WinForms.Guna2Panel();

            lblTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblSubTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();

            txtSearchVehicle = new Guna.UI2.WinForms.Guna2TextBox();
            btnSearch = new Guna.UI2.WinForms.Guna2Button();

            pnlNotification = new Guna.UI2.WinForms.Guna2Panel();
            lblNotificationTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblNotificationCount = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblNotificationText = new Guna.UI2.WinForms.Guna2HtmlLabel();

            pnlUncollected = new Guna.UI2.WinForms.Guna2Panel();
            lblUncollectedTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            dgvReadyJobs = new Guna.UI2.WinForms.Guna2DataGridView();

            pnlSummary = new Guna.UI2.WinForms.Guna2Panel();
            lblSummaryTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblCustomerCaption = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblVehicleCaption = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblJobCaption = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblStatusCaption = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblCustomerValue = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblVehicleValue = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblJobValue = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblStatusValue = new Guna.UI2.WinForms.Guna2HtmlLabel();

            pnlBillItems = new Guna.UI2.WinForms.Guna2Panel();
            lblBillItemsTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            dgvBillItems = new Guna.UI2.WinForms.Guna2DataGridView();

            pnlService = new Guna.UI2.WinForms.Guna2Panel();
            lblServiceTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblServiceCharge = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txtServiceCharge = new Guna.UI2.WinForms.Guna2TextBox();
            lblLabourCharge = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txtLabourCharge = new Guna.UI2.WinForms.Guna2TextBox();

            pnlPayment = new Guna.UI2.WinForms.Guna2Panel();
            lblPaymentTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblPartsTotal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txtPartsTotal = new Guna.UI2.WinForms.Guna2TextBox();
            lblServiceTotal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txtServiceTotal = new Guna.UI2.WinForms.Guna2TextBox();
            lblSubTotal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txtSubTotal = new Guna.UI2.WinForms.Guna2TextBox();
            lblTax = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txtTax = new Guna.UI2.WinForms.Guna2TextBox();
            lblGrandTotal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txtGrandTotal = new Guna.UI2.WinForms.Guna2TextBox();
            lblPaymentMethod = new Guna.UI2.WinForms.Guna2HtmlLabel();
            cmbPaymentMethod = new Guna.UI2.WinForms.Guna2ComboBox();

            btnSaveBill = new Guna.UI2.WinForms.Guna2Button();
            btnMarkPaid = new Guna.UI2.WinForms.Guna2Button();
            btnClear = new Guna.UI2.WinForms.Guna2Button();

            // Hidden controls for old logic compatibility
            btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            btnCollectJob = new Guna.UI2.WinForms.Guna2Button();
            btnPrint = new Guna.UI2.WinForms.Guna2Button();
            txtWorkDescription = new Guna.UI2.WinForms.Guna2TextBox();
            txtDiscount = new Guna.UI2.WinForms.Guna2TextBox();
            lblWorkDescription = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblDiscount = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblUncollectedHint = new Guna.UI2.WinForms.Guna2HtmlLabel();

            mainPanel.SuspendLayout();
            pnlNotification.SuspendLayout();
            pnlUncollected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReadyJobs).BeginInit();
            pnlSummary.SuspendLayout();
            pnlBillItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBillItems).BeginInit();
            pnlService.SuspendLayout();
            pnlPayment.SuspendLayout();
            SuspendLayout();

            // mainPanel
            mainPanel.AutoScroll = true;
            mainPanel.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(lblSubTitle);
            mainPanel.Controls.Add(txtSearchVehicle);
            mainPanel.Controls.Add(btnSearch);
            mainPanel.Controls.Add(pnlNotification);
            mainPanel.Controls.Add(pnlUncollected);
            mainPanel.Controls.Add(pnlSummary);
            mainPanel.Controls.Add(pnlBillItems);
            mainPanel.Controls.Add(pnlService);
            mainPanel.Controls.Add(pnlPayment);
            mainPanel.Controls.Add(btnSaveBill);
            mainPanel.Controls.Add(btnMarkPaid);
            mainPanel.Controls.Add(btnPrint);
            mainPanel.Controls.Add(btnClear);
            mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            mainPanel.Location = new System.Drawing.Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new System.Drawing.Size(1200, 760);
            mainPanel.TabIndex = 0;

            // lblTitle
            lblTitle.BackColor = System.Drawing.Color.Transparent;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Location = new System.Drawing.Point(25, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(82, 43);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Billing";

            // lblSubTitle
            lblSubTitle.BackColor = System.Drawing.Color.Transparent;
            lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSubTitle.ForeColor = System.Drawing.Color.Gainsboro;
            lblSubTitle.Location = new System.Drawing.Point(27, 60);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new System.Drawing.Size(382, 25);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "Ready jobs, bill items and payment handling";

            // txtSearchVehicle
            txtSearchVehicle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            txtSearchVehicle.BorderRadius = 10;
            txtSearchVehicle.FillColor = System.Drawing.Color.FromArgb(30, 41, 59);
            txtSearchVehicle.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSearchVehicle.ForeColor = System.Drawing.Color.White;
            txtSearchVehicle.Location = new System.Drawing.Point(790, 28);
            txtSearchVehicle.Name = "txtSearchVehicle";
            txtSearchVehicle.PlaceholderForeColor = System.Drawing.Color.Silver;
            txtSearchVehicle.PlaceholderText = "Search Vehicle / Job ID";
            txtSearchVehicle.Size = new System.Drawing.Size(250, 42);
            txtSearchVehicle.TabIndex = 2;

            // btnSearch
            btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSearch.BorderRadius = 10;
            btnSearch.FillColor = System.Drawing.Color.FromArgb(14, 165, 233);
            btnSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnSearch.ForeColor = System.Drawing.Color.White;
            btnSearch.Location = new System.Drawing.Point(1050, 28);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(120, 42);
            btnSearch.TabIndex = 3;
            btnSearch.Text = "Search";

            // pnlNotification
            pnlNotification.BorderColor = System.Drawing.Color.FromArgb(51, 65, 85);
            pnlNotification.BorderRadius = 16;
            pnlNotification.BorderThickness = 1;
            pnlNotification.Controls.Add(lblNotificationTitle);
            pnlNotification.Controls.Add(lblNotificationCount);
            pnlNotification.Controls.Add(lblNotificationText);
            pnlNotification.FillColor = System.Drawing.Color.FromArgb(30, 41, 59);
            pnlNotification.Location = new System.Drawing.Point(25, 105);
            pnlNotification.Name = "pnlNotification";
            pnlNotification.Size = new System.Drawing.Size(260, 120);
            pnlNotification.TabIndex = 4;

            lblNotificationTitle.BackColor = System.Drawing.Color.Transparent;
            lblNotificationTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblNotificationTitle.ForeColor = System.Drawing.Color.FromArgb(125, 211, 252);
            lblNotificationTitle.Location = new System.Drawing.Point(18, 18);
            lblNotificationTitle.Name = "lblNotificationTitle";
            lblNotificationTitle.Size = new System.Drawing.Size(135, 27);
            lblNotificationTitle.TabIndex = 0;
            lblNotificationTitle.Text = "Ready Alerts";

            lblNotificationCount.BackColor = System.Drawing.Color.Transparent;
            lblNotificationCount.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            lblNotificationCount.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            lblNotificationCount.Location = new System.Drawing.Point(18, 45);
            lblNotificationCount.Name = "lblNotificationCount";
            lblNotificationCount.Size = new System.Drawing.Size(55, 64);
            lblNotificationCount.TabIndex = 1;
            lblNotificationCount.Text = "0";

            lblNotificationText.BackColor = System.Drawing.Color.Transparent;
            lblNotificationText.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblNotificationText.ForeColor = System.Drawing.Color.Gainsboro;
            lblNotificationText.Location = new System.Drawing.Point(88, 70);
            lblNotificationText.Name = "lblNotificationText";
            lblNotificationText.Size = new System.Drawing.Size(135, 22);
            lblNotificationText.TabIndex = 2;
            lblNotificationText.Text = "jobs for billing";

            pnlUncollected.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pnlUncollected.BorderColor = System.Drawing.Color.FromArgb(51, 65, 85);
            pnlUncollected.BorderRadius = 16;
            pnlUncollected.BorderThickness = 1;
            pnlUncollected.Controls.Add(lblUncollectedTitle);
            pnlUncollected.Controls.Add(dgvReadyJobs);
            pnlUncollected.FillColor = System.Drawing.Color.FromArgb(30, 41, 59);
            pnlUncollected.Location = new System.Drawing.Point(305, 105);
            pnlUncollected.Name = "pnlUncollected";
            pnlUncollected.Size = new System.Drawing.Size(865, 220);
            pnlUncollected.TabIndex = 5;

            lblUncollectedTitle.BackColor = System.Drawing.Color.Transparent;
            lblUncollectedTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblUncollectedTitle.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            lblUncollectedTitle.Location = new System.Drawing.Point(18, 15);
            lblUncollectedTitle.Name = "lblUncollectedTitle";
            lblUncollectedTitle.Size = new System.Drawing.Size(191, 32);
            lblUncollectedTitle.TabIndex = 0;
            lblUncollectedTitle.Text = "Ready Job Queue";

            dgvReadyJobs.AllowUserToAddRows = false;
            dgvReadyJobs.AllowUserToDeleteRows = false;
            dgvReadyJobs.AllowUserToResizeRows = false;
            dgvReadyJobs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvReadyJobs.BackgroundColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dgvReadyJobs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvReadyJobs.ColumnHeadersHeight = 36;
            headerStyle1.BackColor = System.Drawing.Color.FromArgb(14, 165, 233);
            headerStyle1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            headerStyle1.ForeColor = System.Drawing.Color.White;
            dgvReadyJobs.ColumnHeadersDefaultCellStyle = headerStyle1;
            rowStyle1.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            rowStyle1.Font = new System.Drawing.Font("Segoe UI", 9.2F);
            rowStyle1.ForeColor = System.Drawing.Color.White;
            rowStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            rowStyle1.SelectionForeColor = System.Drawing.Color.White;
            dgvReadyJobs.DefaultCellStyle = rowStyle1;
            dgvReadyJobs.EnableHeadersVisualStyles = false;
            dgvReadyJobs.GridColor = System.Drawing.Color.FromArgb(51, 65, 85);
            dgvReadyJobs.Location = new System.Drawing.Point(18, 58);
            dgvReadyJobs.Name = "dgvReadyJobs";
            dgvReadyJobs.ReadOnly = true;
            dgvReadyJobs.RowHeadersVisible = false;
            dgvReadyJobs.RowTemplate.Height = 32;
            dgvReadyJobs.Size = new System.Drawing.Size(829, 145);
            dgvReadyJobs.TabIndex = 1;
            dgvReadyJobs.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dgvReadyJobs.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.White;
            dgvReadyJobs.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            dgvReadyJobs.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvReadyJobs.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dgvReadyJobs.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.White;
            dgvReadyJobs.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            dgvReadyJobs.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.White;
            // pnlSummary
            pnlSummary.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pnlSummary.BorderColor = System.Drawing.Color.FromArgb(51, 65, 85);
            pnlSummary.BorderRadius = 16;
            pnlSummary.BorderThickness = 1;
            pnlSummary.Controls.Add(lblSummaryTitle);
            pnlSummary.Controls.Add(lblCustomerCaption);
            pnlSummary.Controls.Add(lblVehicleCaption);
            pnlSummary.Controls.Add(lblJobCaption);
            pnlSummary.Controls.Add(lblStatusCaption);
            pnlSummary.Controls.Add(lblCustomerValue);
            pnlSummary.Controls.Add(lblVehicleValue);
            pnlSummary.Controls.Add(lblJobValue);
            pnlSummary.Controls.Add(lblStatusValue);
            pnlSummary.FillColor = System.Drawing.Color.FromArgb(30, 41, 59);
            pnlSummary.Location = new System.Drawing.Point(25, 345);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new System.Drawing.Size(1145, 95);
            pnlSummary.TabIndex = 6;

            lblSummaryTitle.BackColor = System.Drawing.Color.Transparent;
            lblSummaryTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblSummaryTitle.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            lblSummaryTitle.Location = new System.Drawing.Point(18, 12);
            lblSummaryTitle.Name = "lblSummaryTitle";
            lblSummaryTitle.Size = new System.Drawing.Size(128, 30);
            lblSummaryTitle.Text = "Selected Job";

            lblCustomerCaption.BackColor = System.Drawing.Color.Transparent;
            lblCustomerCaption.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblCustomerCaption.ForeColor = System.Drawing.Color.FromArgb(125, 211, 252);
            lblCustomerCaption.Location = new System.Drawing.Point(20, 50);
            lblCustomerCaption.Name = "lblCustomerCaption";
            lblCustomerCaption.Size = new System.Drawing.Size(76, 23);
            lblCustomerCaption.Text = "Customer";

            lblCustomerValue.BackColor = System.Drawing.Color.Transparent;
            lblCustomerValue.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblCustomerValue.ForeColor = System.Drawing.Color.White;
            lblCustomerValue.Location = new System.Drawing.Point(110, 48);
            lblCustomerValue.Name = "lblCustomerValue";
            lblCustomerValue.Size = new System.Drawing.Size(18, 27);
            lblCustomerValue.Text = "-";

            lblVehicleCaption.BackColor = System.Drawing.Color.Transparent;
            lblVehicleCaption.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblVehicleCaption.ForeColor = System.Drawing.Color.FromArgb(125, 211, 252);
            lblVehicleCaption.Location = new System.Drawing.Point(340, 50);
            lblVehicleCaption.Name = "lblVehicleCaption";
            lblVehicleCaption.Size = new System.Drawing.Size(58, 23);
            lblVehicleCaption.Text = "Vehicle";

            lblVehicleValue.BackColor = System.Drawing.Color.Transparent;
            lblVehicleValue.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblVehicleValue.ForeColor = System.Drawing.Color.White;
            lblVehicleValue.Location = new System.Drawing.Point(410, 48);
            lblVehicleValue.Name = "lblVehicleValue";
            lblVehicleValue.Size = new System.Drawing.Size(18, 27);
            lblVehicleValue.Text = "-";

            lblJobCaption.BackColor = System.Drawing.Color.Transparent;
            lblJobCaption.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblJobCaption.ForeColor = System.Drawing.Color.FromArgb(125, 211, 252);
            lblJobCaption.Location = new System.Drawing.Point(630, 50);
            lblJobCaption.Name = "lblJobCaption";
            lblJobCaption.Size = new System.Drawing.Size(49, 23);
            lblJobCaption.Text = "Job ID";

            lblJobValue.BackColor = System.Drawing.Color.Transparent;
            lblJobValue.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblJobValue.ForeColor = System.Drawing.Color.White;
            lblJobValue.Location = new System.Drawing.Point(695, 48);
            lblJobValue.Name = "lblJobValue";
            lblJobValue.Size = new System.Drawing.Size(18, 27);
            lblJobValue.Text = "-";

            lblStatusCaption.BackColor = System.Drawing.Color.Transparent;
            lblStatusCaption.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblStatusCaption.ForeColor = System.Drawing.Color.FromArgb(125, 211, 252);
            lblStatusCaption.Location = new System.Drawing.Point(880, 50);
            lblStatusCaption.Name = "lblStatusCaption";
            lblStatusCaption.Size = new System.Drawing.Size(96, 23);
            lblStatusCaption.Text = "Bill Status";

            lblStatusValue.BackColor = System.Drawing.Color.Transparent;
            lblStatusValue.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblStatusValue.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            lblStatusValue.Location = new System.Drawing.Point(990, 48);
            lblStatusValue.Name = "lblStatusValue";
            lblStatusValue.Size = new System.Drawing.Size(18, 27);
            lblStatusValue.Text = "-";

            pnlBillItems.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            pnlBillItems.BorderColor = System.Drawing.Color.FromArgb(51, 65, 85);
            pnlBillItems.BorderRadius = 16;
            pnlBillItems.BorderThickness = 1;
            pnlBillItems.Controls.Add(lblBillItemsTitle);
            pnlBillItems.Controls.Add(dgvBillItems);
            pnlBillItems.FillColor = System.Drawing.Color.FromArgb(30, 41, 59);
            pnlBillItems.Location = new System.Drawing.Point(25, 460);
            pnlBillItems.Name = "pnlBillItems";
            pnlBillItems.Size = new System.Drawing.Size(500, 210);
            pnlBillItems.TabIndex = 7;

            lblBillItemsTitle.BackColor = System.Drawing.Color.Transparent;
            lblBillItemsTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblBillItemsTitle.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            lblBillItemsTitle.Location = new System.Drawing.Point(18, 15);
            lblBillItemsTitle.Name = "lblBillItemsTitle";
            lblBillItemsTitle.Size = new System.Drawing.Size(113, 32);
            lblBillItemsTitle.Text = "Bill Items";

            dgvBillItems.AllowUserToAddRows = false;
            dgvBillItems.AllowUserToDeleteRows = false;
            dgvBillItems.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvBillItems.BackgroundColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dgvBillItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvBillItems.ColumnHeadersHeight = 36;
            headerStyle2.BackColor = System.Drawing.Color.FromArgb(14, 165, 233);
            headerStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            headerStyle2.ForeColor = System.Drawing.Color.White;
            dgvBillItems.ColumnHeadersDefaultCellStyle = headerStyle2;
            rowStyle2.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            rowStyle2.Font = new System.Drawing.Font("Segoe UI", 9.2F);
            rowStyle2.ForeColor = System.Drawing.Color.White;
            rowStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            rowStyle2.SelectionForeColor = System.Drawing.Color.White;
            dgvBillItems.DefaultCellStyle = rowStyle2;
            dgvBillItems.EnableHeadersVisualStyles = false;
            dgvBillItems.GridColor = System.Drawing.Color.FromArgb(51, 65, 85);
            dgvBillItems.Location = new System.Drawing.Point(18, 58);
            dgvBillItems.Name = "dgvBillItems";
            dgvBillItems.ReadOnly = true;
            dgvBillItems.RowHeadersVisible = false;
            dgvBillItems.RowTemplate.Height = 32;
            dgvBillItems.Size = new System.Drawing.Size(464, 135);
            dgvBillItems.TabIndex = 1;
            dgvBillItems.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dgvBillItems.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.White;
            dgvBillItems.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            dgvBillItems.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvBillItems.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dgvBillItems.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.White;
            dgvBillItems.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            dgvBillItems.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.White;

            // pnlService
            pnlService.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            pnlService.BorderColor = System.Drawing.Color.FromArgb(51, 65, 85);
            pnlService.BorderRadius = 16;
            pnlService.BorderThickness = 1;
            pnlService.Controls.Add(lblServiceTitle);
            pnlService.Controls.Add(lblServiceCharge);
            pnlService.Controls.Add(txtServiceCharge);
            pnlService.Controls.Add(lblLabourCharge);
            pnlService.Controls.Add(txtLabourCharge);
            pnlService.FillColor = System.Drawing.Color.FromArgb(30, 41, 59);
            pnlService.Location = new System.Drawing.Point(545, 460);
            pnlService.Name = "pnlService";
            pnlService.Size = new System.Drawing.Size(270, 210);
            pnlService.TabIndex = 8;

            lblServiceTitle.BackColor = System.Drawing.Color.Transparent;
            lblServiceTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblServiceTitle.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            lblServiceTitle.Location = new System.Drawing.Point(18, 15);
            lblServiceTitle.Name = "lblServiceTitle";
            lblServiceTitle.Size = new System.Drawing.Size(98, 32);
            lblServiceTitle.Text = "Charges";

            lblServiceCharge.BackColor = System.Drawing.Color.Transparent;
            lblServiceCharge.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblServiceCharge.ForeColor = System.Drawing.Color.White;
            lblServiceCharge.Location = new System.Drawing.Point(18, 65);
            lblServiceCharge.Name = "lblServiceCharge";
            lblServiceCharge.Size = new System.Drawing.Size(102, 23);
            lblServiceCharge.Text = "Service Charge";

            txtServiceCharge.BorderRadius = 10;
            txtServiceCharge.DefaultText = "0";
            txtServiceCharge.FillColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtServiceCharge.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtServiceCharge.ForeColor = System.Drawing.Color.White;
            txtServiceCharge.Location = new System.Drawing.Point(18, 92);
            txtServiceCharge.Name = "txtServiceCharge";
            txtServiceCharge.Size = new System.Drawing.Size(230, 38);
            txtServiceCharge.TabIndex = 1;

            lblLabourCharge.BackColor = System.Drawing.Color.Transparent;
            lblLabourCharge.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblLabourCharge.ForeColor = System.Drawing.Color.White;
            lblLabourCharge.Location = new System.Drawing.Point(18, 140);
            lblLabourCharge.Name = "lblLabourCharge";
            lblLabourCharge.Size = new System.Drawing.Size(97, 23);
            lblLabourCharge.Text = "Labour Charge";

            txtLabourCharge.BorderRadius = 10;
            txtLabourCharge.DefaultText = "0";
            txtLabourCharge.FillColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtLabourCharge.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtLabourCharge.ForeColor = System.Drawing.Color.White;
            txtLabourCharge.Location = new System.Drawing.Point(18, 166);
            txtLabourCharge.Name = "txtLabourCharge";
            txtLabourCharge.Size = new System.Drawing.Size(230, 38);
            txtLabourCharge.TabIndex = 2;

            // pnlPayment
            pnlPayment.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            pnlPayment.BorderColor = System.Drawing.Color.FromArgb(51, 65, 85);
            pnlPayment.BorderRadius = 16;
            pnlPayment.BorderThickness = 1;
            pnlPayment.Controls.Add(lblPaymentTitle);
            pnlPayment.Controls.Add(lblPartsTotal);
            pnlPayment.Controls.Add(txtPartsTotal);
            pnlPayment.Controls.Add(lblServiceTotal);
            pnlPayment.Controls.Add(txtServiceTotal);
            pnlPayment.Controls.Add(lblSubTotal);
            pnlPayment.Controls.Add(txtSubTotal);
            pnlPayment.Controls.Add(lblTax);
            pnlPayment.Controls.Add(txtTax);
            pnlPayment.Controls.Add(lblGrandTotal);
            pnlPayment.Controls.Add(txtGrandTotal);
            pnlPayment.Controls.Add(lblPaymentMethod);
            pnlPayment.Controls.Add(cmbPaymentMethod);
            pnlPayment.FillColor = System.Drawing.Color.FromArgb(30, 41, 59);
            pnlPayment.Location = new System.Drawing.Point(835, 460);
            pnlPayment.Name = "pnlPayment";
            pnlPayment.Size = new System.Drawing.Size(335, 210);
            pnlPayment.TabIndex = 9;

            lblPaymentTitle.BackColor = System.Drawing.Color.Transparent;
            lblPaymentTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblPaymentTitle.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            lblPaymentTitle.Location = new System.Drawing.Point(18, 15);
            lblPaymentTitle.Name = "lblPaymentTitle";
            lblPaymentTitle.Size = new System.Drawing.Size(104, 32);
            lblPaymentTitle.Text = "Payment";

            lblPartsTotal.BackColor = System.Drawing.Color.Transparent;
            lblPartsTotal.ForeColor = System.Drawing.Color.White;
            lblPartsTotal.Location = new System.Drawing.Point(18, 55);
            lblPartsTotal.Name = "lblPartsTotal";
            lblPartsTotal.Size = new System.Drawing.Size(70, 22);
            lblPartsTotal.Text = "Parts Total";

            txtPartsTotal.BorderRadius = 8;
            txtPartsTotal.DefaultText = "0";
            txtPartsTotal.FillColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtPartsTotal.ForeColor = System.Drawing.Color.White;
            txtPartsTotal.Location = new System.Drawing.Point(140, 50);
            txtPartsTotal.Name = "txtPartsTotal";
            txtPartsTotal.ReadOnly = true;
            txtPartsTotal.Size = new System.Drawing.Size(170, 28);

            lblServiceTotal.BackColor = System.Drawing.Color.Transparent;
            lblServiceTotal.ForeColor = System.Drawing.Color.White;
            lblServiceTotal.Location = new System.Drawing.Point(18, 85);
            lblServiceTotal.Name = "lblServiceTotal";
            lblServiceTotal.Size = new System.Drawing.Size(83, 22);
            lblServiceTotal.Text = "Service Total";

            txtServiceTotal.BorderRadius = 8;
            txtServiceTotal.DefaultText = "0";
            txtServiceTotal.FillColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtServiceTotal.ForeColor = System.Drawing.Color.White;
            txtServiceTotal.Location = new System.Drawing.Point(140, 80);
            txtServiceTotal.Name = "txtServiceTotal";
            txtServiceTotal.ReadOnly = true;
            txtServiceTotal.Size = new System.Drawing.Size(170, 28);

            lblSubTotal.BackColor = System.Drawing.Color.Transparent;
            lblSubTotal.ForeColor = System.Drawing.Color.White;
            lblSubTotal.Location = new System.Drawing.Point(18, 115);
            lblSubTotal.Name = "lblSubTotal";
            lblSubTotal.Size = new System.Drawing.Size(59, 22);
            lblSubTotal.Text = "Subtotal";

            txtSubTotal.BorderRadius = 8;
            txtSubTotal.DefaultText = "0";
            txtSubTotal.FillColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtSubTotal.ForeColor = System.Drawing.Color.White;
            txtSubTotal.Location = new System.Drawing.Point(140, 110);
            txtSubTotal.Name = "txtSubTotal";
            txtSubTotal.ReadOnly = true;
            txtSubTotal.Size = new System.Drawing.Size(170, 28);

            lblTax.BackColor = System.Drawing.Color.Transparent;
            lblTax.ForeColor = System.Drawing.Color.White;
            lblTax.Location = new System.Drawing.Point(18, 145);
            lblTax.Name = "lblTax";
            lblTax.Size = new System.Drawing.Size(29, 22);
            lblTax.Text = "Tax";

            txtTax.BorderRadius = 8;
            txtTax.DefaultText = "0";
            txtTax.FillColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtTax.ForeColor = System.Drawing.Color.White;
            txtTax.Location = new System.Drawing.Point(140, 140);
            txtTax.Name = "txtTax";
            txtTax.Size = new System.Drawing.Size(170, 28);

            lblGrandTotal.BackColor = System.Drawing.Color.Transparent;
            lblGrandTotal.ForeColor = System.Drawing.Color.White;
            lblGrandTotal.Location = new System.Drawing.Point(18, 175);
            lblGrandTotal.Name = "lblGrandTotal";
            lblGrandTotal.Size = new System.Drawing.Size(78, 22);
            lblGrandTotal.Text = "Grand Total";

            txtGrandTotal.BorderRadius = 8;
            txtGrandTotal.DefaultText = "0";
            txtGrandTotal.FillColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtGrandTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            txtGrandTotal.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            txtGrandTotal.Location = new System.Drawing.Point(140, 170);
            txtGrandTotal.Name = "txtGrandTotal";
            txtGrandTotal.ReadOnly = true;
            txtGrandTotal.Size = new System.Drawing.Size(170, 28);

            lblPaymentMethod.BackColor = System.Drawing.Color.Transparent;
            lblPaymentMethod.ForeColor = System.Drawing.Color.White;
            lblPaymentMethod.Location = new System.Drawing.Point(18, 205);
            lblPaymentMethod.Name = "lblPaymentMethod";
            lblPaymentMethod.Size = new System.Drawing.Size(106, 22);
            lblPaymentMethod.Text = "Payment Method";

            cmbPaymentMethod.BackColor = System.Drawing.Color.Transparent;
            cmbPaymentMethod.BorderRadius = 8;
            cmbPaymentMethod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbPaymentMethod.FillColor = System.Drawing.Color.FromArgb(15, 23, 42);
            cmbPaymentMethod.ForeColor = System.Drawing.Color.White;
            cmbPaymentMethod.ItemHeight = 24;
            cmbPaymentMethod.Location = new System.Drawing.Point(140, 200);
            cmbPaymentMethod.Name = "cmbPaymentMethod";
            cmbPaymentMethod.Size = new System.Drawing.Size(170, 30);

            // buttons
            btnSaveBill.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnSaveBill.BorderRadius = 10;
            btnSaveBill.FillColor = System.Drawing.Color.FromArgb(34, 197, 94);
            btnSaveBill.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            btnSaveBill.ForeColor = System.Drawing.Color.White;
            btnSaveBill.Location = new System.Drawing.Point(25, 695);
            btnSaveBill.Name = "btnSaveBill";
            btnSaveBill.Size = new System.Drawing.Size(150, 45);
            btnSaveBill.TabIndex = 10;
            btnSaveBill.Text = "Save Bill";

            btnMarkPaid.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnMarkPaid.BorderRadius = 10;
            btnMarkPaid.FillColor = System.Drawing.Color.FromArgb(16, 185, 129);
            btnMarkPaid.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            btnMarkPaid.ForeColor = System.Drawing.Color.White;
            btnMarkPaid.Location = new System.Drawing.Point(190, 695);
            btnMarkPaid.Name = "btnMarkPaid";
            btnMarkPaid.Size = new System.Drawing.Size(150, 45);
            btnMarkPaid.TabIndex = 11;
            btnMarkPaid.Text = "Mark Paid";

            btnPrint.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnPrint.BorderRadius = 10;
            btnPrint.FillColor = System.Drawing.Color.FromArgb(59, 130, 246);
            btnPrint.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            btnPrint.ForeColor = System.Drawing.Color.White;
            btnPrint.Location = new System.Drawing.Point(355, 695);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new System.Drawing.Size(130, 45);
            btnPrint.TabIndex = 12;
            btnPrint.Text = "Print Bill";
            btnPrint.Visible = true;

            btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnClear.BorderRadius = 10;
            btnClear.FillColor = System.Drawing.Color.FromArgb(245, 158, 11);
            btnClear.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            btnClear.ForeColor = System.Drawing.Color.White;
            btnClear.Location = new System.Drawing.Point(500, 695);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(130, 45);
            btnClear.TabIndex = 12;
            btnClear.Text = "Clear";

            
            btnRefresh.Visible = false;
            btnCollectJob.Visible = false;
            //btnPrint.Visible = false;
            txtWorkDescription.Visible = false;
            txtDiscount.Visible = false;
            lblWorkDescription.Visible = false;
            lblDiscount.Visible = false;
            lblUncollectedHint.Visible = false;

            txtDiscount.Text = "0";
            txtWorkDescription.Text = "";

            // UC_billing
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            Controls.Add(mainPanel);
            Name = "UC_billing";
            Size = new System.Drawing.Size(1200, 760);
            Load += UC_billing_Load;

            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            pnlNotification.ResumeLayout(false);
            pnlNotification.PerformLayout();
            pnlUncollected.ResumeLayout(false);
            pnlUncollected.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReadyJobs).EndInit();
            pnlSummary.ResumeLayout(false);
            pnlSummary.PerformLayout();
            pnlBillItems.ResumeLayout(false);
            pnlBillItems.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBillItems).EndInit();
            pnlService.ResumeLayout(false);
            pnlService.PerformLayout();
            pnlPayment.ResumeLayout(false);
            pnlPayment.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel mainPanel;

        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSubTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtSearchVehicle;
        private Guna.UI2.WinForms.Guna2Button btnSearch;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;

        private Guna.UI2.WinForms.Guna2Panel pnlNotification;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNotificationTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNotificationCount;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNotificationText;

        private Guna.UI2.WinForms.Guna2Panel pnlUncollected;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblUncollectedTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblUncollectedHint;
        private Guna.UI2.WinForms.Guna2DataGridView dgvReadyJobs;

        private Guna.UI2.WinForms.Guna2Panel pnlSummary;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSummaryTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblCustomerCaption;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblVehicleCaption;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblJobCaption;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatusCaption;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblCustomerValue;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblVehicleValue;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblJobValue;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatusValue;

        private Guna.UI2.WinForms.Guna2Panel pnlBillItems;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblBillItemsTitle;
        private Guna.UI2.WinForms.Guna2DataGridView dgvBillItems;

        private Guna.UI2.WinForms.Guna2Panel pnlService;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblServiceTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblWorkDescription;
        private Guna.UI2.WinForms.Guna2TextBox txtWorkDescription;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblServiceCharge;
        private Guna.UI2.WinForms.Guna2TextBox txtServiceCharge;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblLabourCharge;
        private Guna.UI2.WinForms.Guna2TextBox txtLabourCharge;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDiscount;
        private Guna.UI2.WinForms.Guna2TextBox txtDiscount;

        private Guna.UI2.WinForms.Guna2Panel pnlPayment;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPaymentTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPartsTotal;
        private Guna.UI2.WinForms.Guna2TextBox txtPartsTotal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblServiceTotal;
        private Guna.UI2.WinForms.Guna2TextBox txtServiceTotal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSubTotal;
        private Guna.UI2.WinForms.Guna2TextBox txtSubTotal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTax;
        private Guna.UI2.WinForms.Guna2TextBox txtTax;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblGrandTotal;
        private Guna.UI2.WinForms.Guna2TextBox txtGrandTotal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPaymentMethod;
        private Guna.UI2.WinForms.Guna2ComboBox cmbPaymentMethod;

        private Guna.UI2.WinForms.Guna2Button btnCollectJob;
        private Guna.UI2.WinForms.Guna2Button btnSaveBill;
        private Guna.UI2.WinForms.Guna2Button btnPrint;
        private Guna.UI2.WinForms.Guna2Button btnMarkPaid;
        private Guna.UI2.WinForms.Guna2Button btnClear;
    }
}