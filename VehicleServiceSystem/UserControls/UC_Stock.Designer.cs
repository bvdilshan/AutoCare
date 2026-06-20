namespace VehicleServiceSystem.UserControls
{
    partial class UC_Stock
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();

            mainPanel = new Guna.UI2.WinForms.Guna2Panel();

            lblTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblSubTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();

            txtSearchItem = new Guna.UI2.WinForms.Guna2TextBox();
            btnSearch = new Guna.UI2.WinForms.Guna2Button();

            pnlItemForm = new Guna.UI2.WinForms.Guna2Panel();
            lblFormTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblItemIdTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblItemID = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txtItemName = new Guna.UI2.WinForms.Guna2TextBox();
            txtCategory = new Guna.UI2.WinForms.Guna2TextBox();
            txtPrice = new Guna.UI2.WinForms.Guna2TextBox();
            txtQuantity = new Guna.UI2.WinForms.Guna2TextBox();
            btnAdd = new Guna.UI2.WinForms.Guna2Button();
            btnUpdate = new Guna.UI2.WinForms.Guna2Button();
            btnDelete = new Guna.UI2.WinForms.Guna2Button();
            btnClear = new Guna.UI2.WinForms.Guna2Button();

            pnlTable = new Guna.UI2.WinForms.Guna2Panel();
            lblTableTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            dgvStock = new Guna.UI2.WinForms.Guna2DataGridView();

            mainPanel.SuspendLayout();
            pnlItemForm.SuspendLayout();
            pnlTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStock).BeginInit();
            SuspendLayout();

            // 
            // mainPanel
            // 
            mainPanel.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            mainPanel.Location = new System.Drawing.Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new System.Drawing.Size(1400, 820);
            mainPanel.TabIndex = 0;
            mainPanel.AutoScroll = true;

            // 
            // lblTitle
            // 
            lblTitle.BackColor = System.Drawing.Color.Transparent;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Location = new System.Drawing.Point(26, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(71, 43);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Stock";

            // 
            // lblSubTitle
            // 
            lblSubTitle.BackColor = System.Drawing.Color.Transparent;
            lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblSubTitle.ForeColor = System.Drawing.Color.Gainsboro;
            lblSubTitle.Location = new System.Drawing.Point(28, 58);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new System.Drawing.Size(206, 25);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "Manage inventory and spare parts";

            // 
            // txtSearchItem
            // 
            txtSearchItem.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            txtSearchItem.BorderRadius = 10;
            txtSearchItem.DefaultText = "";
            txtSearchItem.FillColor = System.Drawing.Color.FromArgb(30, 41, 59);
            txtSearchItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSearchItem.ForeColor = System.Drawing.Color.White;
            txtSearchItem.Location = new System.Drawing.Point(930, 25);
            txtSearchItem.Name = "txtSearchItem";
            txtSearchItem.PlaceholderForeColor = System.Drawing.Color.Silver;
            txtSearchItem.PlaceholderText = "Search item by name / category";
            txtSearchItem.SelectedText = "";
            txtSearchItem.Size = new System.Drawing.Size(290, 42);
            txtSearchItem.TabIndex = 2;

            // 
            // btnSearch
            // 
            btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSearch.BorderRadius = 10;
            btnSearch.FillColor = System.Drawing.Color.FromArgb(14, 165, 233);
            btnSearch.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            btnSearch.ForeColor = System.Drawing.Color.White;
            btnSearch.Location = new System.Drawing.Point(1230, 25);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(130, 42);
            btnSearch.TabIndex = 3;
            btnSearch.Text = "Search";

            // 
            // pnlItemForm
            // 
            pnlItemForm.BackColor = System.Drawing.Color.Transparent;
            pnlItemForm.BorderColor = System.Drawing.Color.FromArgb(51, 65, 85);
            pnlItemForm.BorderRadius = 14;
            pnlItemForm.BorderThickness = 1;
            pnlItemForm.FillColor = System.Drawing.Color.FromArgb(30, 41, 59);
            pnlItemForm.Location = new System.Drawing.Point(26, 105);
            pnlItemForm.Name = "pnlItemForm";
            pnlItemForm.Size = new System.Drawing.Size(390, 600);
            pnlItemForm.TabIndex = 4;

            // 
            // lblFormTitle
            // 
            lblFormTitle.BackColor = System.Drawing.Color.Transparent;
            lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblFormTitle.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            lblFormTitle.Location = new System.Drawing.Point(20, 18);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.Size = new System.Drawing.Size(128, 32);
            lblFormTitle.TabIndex = 0;
            lblFormTitle.Text = "Item Details";

            // 
            // lblItemIdTitle
            // 
            lblItemIdTitle.BackColor = System.Drawing.Color.Transparent;
            lblItemIdTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblItemIdTitle.ForeColor = System.Drawing.Color.FromArgb(125, 211, 252);
            lblItemIdTitle.Location = new System.Drawing.Point(20, 70);
            lblItemIdTitle.Name = "lblItemIdTitle";
            lblItemIdTitle.Size = new System.Drawing.Size(56, 25);
            lblItemIdTitle.TabIndex = 1;
            lblItemIdTitle.Text = "Item ID";

            // 
            // lblItemID
            // 
            lblItemID.BackColor = System.Drawing.Color.Transparent;
            lblItemID.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblItemID.ForeColor = System.Drawing.Color.White;
            lblItemID.Location = new System.Drawing.Point(20, 98);
            lblItemID.Name = "lblItemID";
            lblItemID.Size = new System.Drawing.Size(78, 32);
            lblItemID.TabIndex = 2;
            lblItemID.Text = "ITM001";

            // 
            // txtItemName
            // 
            txtItemName.BorderRadius = 10;
            txtItemName.DefaultText = "";
            txtItemName.FillColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtItemName.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtItemName.ForeColor = System.Drawing.Color.White;
            txtItemName.Location = new System.Drawing.Point(20, 155);
            txtItemName.Name = "txtItemName";
            txtItemName.PlaceholderForeColor = System.Drawing.Color.Silver;
            txtItemName.PlaceholderText = "Item Name";
            txtItemName.SelectedText = "";
            txtItemName.Size = new System.Drawing.Size(345, 44);
            txtItemName.TabIndex = 3;

            // 
            // txtCategory
            // 
            txtCategory.BorderRadius = 10;
            txtCategory.DefaultText = "";
            txtCategory.FillColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtCategory.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtCategory.ForeColor = System.Drawing.Color.White;
            txtCategory.Location = new System.Drawing.Point(20, 220);
            txtCategory.Name = "txtCategory";
            txtCategory.PlaceholderForeColor = System.Drawing.Color.Silver;
            txtCategory.PlaceholderText = "Category";
            txtCategory.SelectedText = "";
            txtCategory.Size = new System.Drawing.Size(345, 44);
            txtCategory.TabIndex = 4;

            // 
            // txtPrice
            // 
            txtPrice.BorderRadius = 10;
            txtPrice.DefaultText = "";
            txtPrice.FillColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtPrice.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtPrice.ForeColor = System.Drawing.Color.White;
            txtPrice.Location = new System.Drawing.Point(20, 285);
            txtPrice.Name = "txtPrice";
            txtPrice.PlaceholderForeColor = System.Drawing.Color.Silver;
            txtPrice.PlaceholderText = "Price (Rs)";
            txtPrice.SelectedText = "";
            txtPrice.Size = new System.Drawing.Size(345, 44);
            txtPrice.TabIndex = 5;

            // 
            // txtQuantity
            // 
            txtQuantity.BorderRadius = 10;
            txtQuantity.DefaultText = "";
            txtQuantity.FillColor = System.Drawing.Color.FromArgb(15, 23, 42);
            txtQuantity.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtQuantity.ForeColor = System.Drawing.Color.White;
            txtQuantity.Location = new System.Drawing.Point(20, 350);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.PlaceholderForeColor = System.Drawing.Color.Silver;
            txtQuantity.PlaceholderText = "Quantity";
            txtQuantity.SelectedText = "";
            txtQuantity.Size = new System.Drawing.Size(345, 44);
            txtQuantity.TabIndex = 6;

            // 
            // btnAdd
            // 
            btnAdd.BorderRadius = 10;
            btnAdd.FillColor = System.Drawing.Color.FromArgb(34, 197, 94);
            btnAdd.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            btnAdd.ForeColor = System.Drawing.Color.White;
            btnAdd.Location = new System.Drawing.Point(20, 430);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(160, 45);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "Add Item";

            // 
            // btnUpdate
            // 
            btnUpdate.BorderRadius = 10;
            btnUpdate.FillColor = System.Drawing.Color.FromArgb(59, 130, 246);
            btnUpdate.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            btnUpdate.ForeColor = System.Drawing.Color.White;
            btnUpdate.Location = new System.Drawing.Point(205, 430);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new System.Drawing.Size(160, 45);
            btnUpdate.TabIndex = 8;
            btnUpdate.Text = "Update";

            // 
            // btnDelete
            // 
            btnDelete.BorderRadius = 10;
            btnDelete.FillColor = System.Drawing.Color.FromArgb(239, 68, 68);
            btnDelete.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            btnDelete.ForeColor = System.Drawing.Color.White;
            btnDelete.Location = new System.Drawing.Point(20, 495);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(160, 45);
            btnDelete.TabIndex = 9;
            btnDelete.Text = "Delete";

            // 
            // btnClear
            // 
            btnClear.BorderRadius = 10;
            btnClear.FillColor = System.Drawing.Color.FromArgb(245, 158, 11);
            btnClear.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            btnClear.ForeColor = System.Drawing.Color.White;
            btnClear.Location = new System.Drawing.Point(205, 495);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(160, 45);
            btnClear.TabIndex = 10;
            btnClear.Text = "Clear";

            // 
            // pnlTable
            // 
            pnlTable.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pnlTable.BackColor = System.Drawing.Color.Transparent;
            pnlTable.BorderColor = System.Drawing.Color.FromArgb(51, 65, 85);
            pnlTable.BorderRadius = 14;
            pnlTable.BorderThickness = 1;
            pnlTable.FillColor = System.Drawing.Color.FromArgb(30, 41, 59);
            pnlTable.Location = new System.Drawing.Point(440, 105);
            pnlTable.Name = "pnlTable";
            pnlTable.Size = new System.Drawing.Size(920, 690);
            pnlTable.TabIndex = 5;

            // 
            // lblTableTitle
            // 
            lblTableTitle.BackColor = System.Drawing.Color.Transparent;
            lblTableTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblTableTitle.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            lblTableTitle.Location = new System.Drawing.Point(20, 18);
            lblTableTitle.Name = "lblTableTitle";
            lblTableTitle.Size = new System.Drawing.Size(91, 32);
            lblTableTitle.TabIndex = 0;
            lblTableTitle.Text = "Stock List";

            // 
            // dgvStock
            // 
            dgvStock.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvStock.AllowUserToAddRows = false;
            dgvStock.AllowUserToDeleteRows = false;
            dgvStock.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dgvStock.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvStock.BackgroundColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dgvStock.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvStock.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvStock.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;

            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(14, 165, 233);
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(14, 165, 233);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvStock.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvStock.ColumnHeadersHeight = 40;

            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvStock.DefaultCellStyle = dataGridViewCellStyle3;

            dgvStock.EnableHeadersVisualStyles = false;
            dgvStock.GridColor = System.Drawing.Color.FromArgb(51, 65, 85);
            dgvStock.Location = new System.Drawing.Point(20, 70);
            dgvStock.Name = "dgvStock";
            dgvStock.ReadOnly = true;
            dgvStock.RowHeadersVisible = false;
            dgvStock.RowTemplate.Height = 35;
            dgvStock.Size = new System.Drawing.Size(880, 600);
            dgvStock.TabIndex = 1;
            dgvStock.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dgvStock.ThemeStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dgvStock.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(51, 65, 85);
            dgvStock.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(14, 165, 233);
            dgvStock.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dgvStock.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvStock.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            dgvStock.ThemeStyle.HeaderStyle.Height = 40;
            dgvStock.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            dgvStock.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvStock.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dgvStock.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.White;
            dgvStock.ThemeStyle.RowsStyle.Height = 35;
            dgvStock.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            dgvStock.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.White;

            // add controls
            pnlItemForm.Controls.Add(lblFormTitle);
            pnlItemForm.Controls.Add(lblItemIdTitle);
            pnlItemForm.Controls.Add(lblItemID);
            pnlItemForm.Controls.Add(txtItemName);
            pnlItemForm.Controls.Add(txtCategory);
            pnlItemForm.Controls.Add(txtPrice);
            pnlItemForm.Controls.Add(txtQuantity);
            pnlItemForm.Controls.Add(btnAdd);
            pnlItemForm.Controls.Add(btnUpdate);
            pnlItemForm.Controls.Add(btnDelete);
            pnlItemForm.Controls.Add(btnClear);

            pnlTable.Controls.Add(lblTableTitle);
            pnlTable.Controls.Add(dgvStock);

            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(lblSubTitle);
            mainPanel.Controls.Add(txtSearchItem);
            mainPanel.Controls.Add(btnSearch);
            mainPanel.Controls.Add(pnlItemForm);
            mainPanel.Controls.Add(pnlTable);

            // 
            // UC_Stock
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            Controls.Add(mainPanel);
            Name = "UC_Stock";
            Size = new System.Drawing.Size(1400, 820);
            Load += UC_Stock_Load;

            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            pnlItemForm.ResumeLayout(false);
            pnlItemForm.PerformLayout();
            pnlTable.ResumeLayout(false);
            pnlTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStock).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel mainPanel;

        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSubTitle;

        private Guna.UI2.WinForms.Guna2TextBox txtSearchItem;
        private Guna.UI2.WinForms.Guna2Button btnSearch;

        private Guna.UI2.WinForms.Guna2Panel pnlItemForm;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblFormTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblItemIdTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblItemID;
        private Guna.UI2.WinForms.Guna2TextBox txtItemName;
        private Guna.UI2.WinForms.Guna2TextBox txtCategory;
        private Guna.UI2.WinForms.Guna2TextBox txtPrice;
        private Guna.UI2.WinForms.Guna2TextBox txtQuantity;
        private Guna.UI2.WinForms.Guna2Button btnAdd;
        private Guna.UI2.WinForms.Guna2Button btnUpdate;
        private Guna.UI2.WinForms.Guna2Button btnDelete;
        private Guna.UI2.WinForms.Guna2Button btnClear;

        private Guna.UI2.WinForms.Guna2Panel pnlTable;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTableTitle;
        private Guna.UI2.WinForms.Guna2DataGridView dgvStock;
    }
}