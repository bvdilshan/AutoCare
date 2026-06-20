using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using VehicleServiceSystem.Data;

namespace VehicleServiceSystem.UserControls
{
    public partial class UC_Stock : UserControl
    {
        //Variable store the ud of the item currently selected in the table 
        private int selectedItemId = 0;

        public UC_Stock()
        {
            InitializeComponent();
            
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            btnSearch.Click += btnSearch_Click;
            dgvStock.CellClick += dgvStock_CellClick;
        }
        //runs when thw user control first loads
        private void UC_Stock_Load(object sender, EventArgs e)
        {
            LoadStock();
            ClearForm();
        }
        //helper method to find an existion catogary id or create a new one if it doesnt exist
        private int GetOrCreateCategoryId(string categoryName)
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string selectQuery = "SELECT CategoryID FROM StockCategories WHERE CategoryName = @category";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                selectCmd.Parameters.AddWithValue("@category", categoryName);

                object result = selectCmd.ExecuteScalar();

                if (result != null)
                    return Convert.ToInt32(result);

                string insertQuery = "INSERT INTO StockCategories (CategoryName) VALUES (@category)";
                MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@category", categoryName);
                insertCmd.ExecuteNonQuery();

                return Convert.ToInt32(insertCmd.LastInsertedId);
            }
        }
        //fetches data from the database and display it in the datagridview
        private void LoadStock()
        {
            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        si.ItemID,
                        si.ItemName,
                        sc.CategoryName,
                        si.Price,
                        si.Quantity,
                        si.ReorderLevel,
                        si.CreatedAt
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
        //Adds a new stock item to the database after validating the input fields
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string itemName = txtItemName.Text.Trim();
            string category = txtCategory.Text.Trim();

            if (itemName == "" || category == "")
            {
                MessageBox.Show("Please enter item name and category.");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price))
            {
                MessageBox.Show("Please enter valid price.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity))
            {
                MessageBox.Show("Please enter valid quantity.");
                return;
            }

            try
            {
                int categoryId = GetOrCreateCategoryId(category);

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO StockItems
                        (ItemName, CategoryID, Price, Quantity, ReorderLevel)
                        VALUES
                        (@itemName, @categoryId, @price, @quantity, 5)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@itemName", itemName);
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@quantity", quantity);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Stock item added successfully.");
                LoadStock();
                ClearForm();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }
        //Updates the selected stock item in the database after validating the input fields
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedItemId == 0)
            {
                MessageBox.Show("Please select an item from the table.");
                return;
            }

            string itemName = txtItemName.Text.Trim();
            string category = txtCategory.Text.Trim();

            if (itemName == "" || category == "")
            {
                MessageBox.Show("Please enter item name and category.");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price))
            {
                MessageBox.Show("Please enter valid price.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity))
            {
                MessageBox.Show("Please enter valid quantity.");
                return;
            }

            try
            {
                int categoryId = GetOrCreateCategoryId(category);

                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        UPDATE StockItems
                        SET ItemName = @itemName,
                            CategoryID = @categoryId,
                            Price = @price,
                            Quantity = @quantity
                        WHERE ItemID = @itemId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@itemName", itemName);
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@itemId", selectedItemId);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Stock item updated successfully.");
                LoadStock();
                ClearForm();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }
        //Deletes the selected stock item from the database after confirming with the user
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedItemId == 0)
            {
                MessageBox.Show("Please select an item from the table.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this stock item?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = "DELETE FROM StockItems WHERE ItemID = @itemId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@itemId", selectedItemId);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Stock item deleted successfully.");
                LoadStock();
                ClearForm();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Cannot delete this item. It may already be used in jobs.\n" + ex.Message);
            }
        }
        //Searches for stock items based on the item name or category name entered in the search box and displays the results in the datagridview
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearchItem.Text.Trim();

            using (MySqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        si.ItemID,
                        si.ItemName,
                        sc.CategoryName,
                        si.Price,
                        si.Quantity,
                        si.ReorderLevel,
                        si.CreatedAt
                    FROM StockItems si
                    INNER JOIN StockCategories sc ON si.CategoryID = sc.CategoryID
                    WHERE si.ItemName LIKE @search
                       OR sc.CategoryName LIKE @search
                    ORDER BY si.ItemID DESC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvStock.DataSource = dt;
            }
        }
        //When the user clicks on a row in the datagridview, this event handler will populate the form fields with the data from that row and store the item ID in the selectedItemId variable for later use in update and delete operations
        private void dgvStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvStock.Rows[e.RowIndex];

            selectedItemId = Convert.ToInt32(row.Cells["ItemID"].Value);

            lblItemID.Text = "ITM" + selectedItemId.ToString("000");
            txtItemName.Text = row.Cells["ItemName"].Value?.ToString();
            txtCategory.Text = row.Cells["CategoryName"].Value?.ToString();
            txtPrice.Text = row.Cells["Price"].Value?.ToString();
            txtQuantity.Text = row.Cells["Quantity"].Value?.ToString();
        }
        //Clears the form fields and resets the selectedItemId variable to 0, indicating that no item is currently selected. It also reloads the stock data to reflect any changes made.
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadStock();
        }
        //This method is called to reset the form to its default state, allowing the user to enter new data for adding a new stock item or clearing the search results.
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