using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using VehicleServiceSystem.Data;

namespace VehicleServiceSystem.UserControls
{
    public partial class UC_Stock : UserControl
    {
        private int selectedItemId = 0;

        public UC_Stock()
        {
            InitializeComponent();
            this.Load += UC_Stock_Load;
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnSearch.Click += btnSearch_Click;
            btnClear.Click += btnClear_Click;
            dgvStock.CellClick += dgvStock_CellClick;

            txtPrice.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
                if ((e.KeyChar == '.') && (txtPrice.Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
            };

            txtQuantity.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
        }

        private void UC_Stock_Load(object sender, EventArgs e)
        {
            LoadCategories(); 
            LoadStock();
            ClearForm();
        }

        private void LoadCategories()
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    string query = "SELECT CategoryID, CategoryName FROM StockCategories ORDER BY CategoryName ASC";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbCategory.DataSource = dt;
                    cmbCategory.DisplayMember = "CategoryName";
                    cmbCategory.ValueMember = "CategoryID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStock()
        {
            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    string query = @"SELECT si.ItemID, si.ItemName, sc.CategoryName, si.Price, si.Quantity, si.ReorderLevel, si.CreatedAt 
                                     FROM StockItems si 
                                     INNER JOIN StockCategories sc ON si.CategoryID = sc.CategoryID 
                                     ORDER BY si.ItemID DESC";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvStock.DataSource = dt;

                    dgvStock.Columns["ItemID"].HeaderText = "Item ID";
                    dgvStock.Columns["ItemName"].HeaderText = "Item Name";
                    dgvStock.Columns["CategoryName"].HeaderText = "Category";
                    dgvStock.Columns["Price"].HeaderText = "Price (Rs)";
                    dgvStock.Columns["Quantity"].HeaderText = "Quantity";
                    dgvStock.Columns["ReorderLevel"].HeaderText = "Reorder Level";
                    dgvStock.Columns["CreatedAt"].HeaderText = "Created At";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load stock error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs(out decimal price, out int quantity)
        {
            price = 0;
            quantity = 0;

            if (string.IsNullOrEmpty(txtItemName.Text.Trim()) || cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter item name and select a category.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), out price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price greater than 0.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out quantity) || quantity < 0)
            {
                MessageBox.Show("Please enter a valid quantity (0 or greater).", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (selectedItemId > 0)
            {
                MessageBox.Show("You are currently in edit mode. Click 'Clear' before adding a new item.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInputs(out decimal price, out int quantity)) return;

            try
            {
                int categoryId = Convert.ToInt32(cmbCategory.SelectedValue);

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO StockItems (ItemName, CategoryID, Price, Quantity, ReorderLevel) 
                                     VALUES (@itemName, @categoryId, @price, @quantity, 5)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@itemName", txtItemName.Text.Trim());
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Stock item added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStock();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add stock error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedItemId == 0)
            {
                MessageBox.Show("Please select an item from the table first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInputs(out decimal price, out int quantity)) return;

            try
            {
                int categoryId = Convert.ToInt32(cmbCategory.SelectedValue);

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE StockItems SET ItemName = @itemName, CategoryID = @categoryId, Price = @price, Quantity = @quantity 
                                     WHERE ItemID = @itemId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@itemName", txtItemName.Text.Trim());
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@itemId", selectedItemId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Stock item updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStock();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update stock error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedItemId == 0)
            {
                MessageBox.Show("Please select an item from the table first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this stock item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM StockItems WHERE ItemID = @itemId", conn);
                    cmd.Parameters.AddWithValue("@itemId", selectedItemId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Stock item deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStock();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot delete this item. It is already used in past vehicle service records.", "Delete Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearchItem.Text.Trim();
            if (search == "") { LoadStock(); return; }

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    string query = @"SELECT si.ItemID, si.ItemName, sc.CategoryName, si.Price, si.Quantity, si.ReorderLevel, si.CreatedAt 
                                     FROM StockItems si 
                                     INNER JOIN StockCategories sc ON si.CategoryID = sc.CategoryID 
                                     WHERE si.ItemName LIKE @search OR sc.CategoryName LIKE @search 
                                     ORDER BY si.ItemID DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvStock.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search stock error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvStock.Rows[e.RowIndex];
            selectedItemId = Convert.ToInt32(row.Cells["ItemID"].Value);

            lblItemID.Text = "ITM" + selectedItemId.ToString("000");
            txtItemName.Text = row.Cells["ItemName"].Value?.ToString();
            txtPrice.Text = row.Cells["Price"].Value?.ToString();
            txtQuantity.Text = row.Cells["Quantity"].Value?.ToString();
            cmbCategory.Text = row.Cells["CategoryName"].Value?.ToString();
            btnAdd.Enabled = false; 
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadStock();
        }

        private void ClearForm()
        {
            selectedItemId = 0;
            lblItemID.Text = "NEW";
            txtItemName.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            txtSearchItem.Clear();
            cmbCategory.SelectedIndex = -1;
            txtItemName.Focus();
            btnAdd.Enabled = true;
        }
    }
}