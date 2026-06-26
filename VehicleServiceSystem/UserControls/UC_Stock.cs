using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using VehicleServiceSystem.Data;

namespace VehicleServiceSystem.UserControls
{
    public partial class UC_Stock : UserControl
    {
        // තෝරාගත් බඩුවේ (Item) ID එක මතක තබා ගැනීමට (0 නම් අලුත් අයිතමයක්)
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

            // 💡 මිල සඳහා: ඉලක්කම්, බැක්ස්පේස් සහ එක තිතක් (.) විතරයි (Minus ලකුණ සම්පූර්ණයෙන්ම බ්ලොක්)
            txtPrice.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }

                // තිතක් දැනටමත් තියෙනවා නම් තව තිතක් දාන්න දෙන්නේ නැහැ
                if ((e.KeyChar == '.') && (txtPrice.Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
            };

            // 💡 ප්‍රමාණය සඳහා: ඉලක්කම් සහ බැක්ස්පේස් විතරයි (තිත් හෝ සෘණ ලකුණු කිසිවක් බැහැ)
            txtQuantity.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
        }

        private void UC_Stock_Load(object sender, EventArgs e)
        {
            LoadStock();
            ClearForm();
        }

        // Category නම දුන්නම CategoryID එක හොයන, නැත්නම් අලුතින්ම හදලා දෙන සරල මැතඩ් එක
        private int GetOrCreateCategoryId(string categoryName)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                // 1. කලින් මේ Category එක ඩේටාබේස් එකේ තියෙනවද බලනවා
                string selectQuery = "SELECT CategoryID FROM StockCategories WHERE CategoryName = @category";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                selectCmd.Parameters.AddWithValue("@category", categoryName);

                object result = selectCmd.ExecuteScalar();
                if (result != null) return Convert.ToInt32(result);

                // 2. නැත්නම් අලුතින්ම Category එක ඇතුළත් කරනවා
                string insertQuery = "INSERT INTO StockCategories (CategoryName) VALUES (@category)";
                MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@category", categoryName);
                insertCmd.ExecuteNonQuery();

                return Convert.ToInt32(insertCmd.LastInsertedId);
            }
        }

        // 1. ඩේටාබේස් එකෙන් Stock විස්තර ටික Grid එකට ලෝඩ් කිරීම
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

                    // Grid Headers ලස්සන කිරීම
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

        // 💡 1st Year මට්ටමට ගැලපෙන සෘණ (Minus) අගයන් වළක්වන සරල Validation Logic එක
        private bool ValidateInputs(out decimal price, out int quantity)
        {
            price = 0;
            quantity = 0;

            // හිස්තැන් පිරික්සීම
            if (string.IsNullOrEmpty(txtItemName.Text.Trim()) || string.IsNullOrEmpty(txtCategory.Text.Trim()))
            {
                MessageBox.Show("Please enter item name and category.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // මිල පිරික්සීම (0 හෝ සෘණ අගයන් වැළැක්වීම)
            if (!decimal.TryParse(txtPrice.Text.Trim(), out price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price greater than 0.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }

            // ප්‍රමාණය පිරික්සීම (සෘණ අගයන් වැළැක්වීම - 0 විය හැක)
            if (!int.TryParse(txtQuantity.Text.Trim(), out quantity) || quantity < 0)
            {
                MessageBox.Show("Please enter a valid quantity (0 or greater).", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }

            return true;
        }

        // 2. අලුත් බඩුවක් ස්ටොක් එකට එකතු කිරීම (Add Item)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out decimal price, out int quantity)) return;

            try
            {
                int categoryId = GetOrCreateCategoryId(txtCategory.Text.Trim());

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

        // 3. තෝරාගත් බඩුවක විස්තර වෙනස් කිරීම (Update Item)
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
                int categoryId = GetOrCreateCategoryId(txtCategory.Text.Trim());

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

        // 4. බඩුවක් ස්ටොක් එකෙන් මුළුමනින්ම මකා දැමීම (Delete Item)
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
                MessageBox.Show("Cannot delete this item. It may already be used in service records.\n" + ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 5. බඩුවේ නමෙන් හෝ Category එකෙන් සෙවීම (Search)
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

        // 6. Grid එකෙන් Row එකක් ක්ලික් කරද්දී Form එකට දත්ත ලබා ගැනීම
        private void dgvStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvStock.Rows[e.RowIndex];
            selectedItemId = Convert.ToInt32(row.Cells["ItemID"].Value);

            lblItemID.Text = "ITM" + selectedItemId.ToString("000");
            txtItemName.Text = row.Cells["ItemName"].Value?.ToString();
            txtCategory.Text = row.Cells["CategoryName"].Value?.ToString();
            txtPrice.Text = row.Cells["Price"].Value?.ToString();
            txtQuantity.Text = row.Cells["Quantity"].Value?.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadStock();
        }

        // 8. Form එක Reset කිරීම
        private void ClearForm()
        {
            selectedItemId = 0;
            lblItemID.Text = "NEW";
            txtItemName.Clear();
            txtCategory.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            txtSearchItem.Clear();
            txtItemName.Focus();
        }
    }
}