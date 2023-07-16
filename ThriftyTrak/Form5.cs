using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ThriftyTrak
{
    public partial class Form5 : Form
    {
        private BindingSource bindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private bool inventoryView = true;
        private string userName, password, connStr;


        public Form5(string userName, string password)
        {
            InitializeComponent();
            //experimenting with Windows Auth only. Comment out lines 28, 29, and 32 if needed and reinstate line 33 for the mixed mode version
            var datasource = @"(local)";
            var database = "ThriftyTrak";
            this.userName = userName;
            this.password = password;
            connStr = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Persist Security Info=True;User ID=" + userName + ";Password=" + password;
           // connStr = "Server=localhost; Database=ThriftyTrak; User Id=" + userName + "; Password=" + password;
            lblGreeting.Text = "Hello, " + userName + "!";
        }

        private void GetData(string query)
        {
            try
            {
                dataAdapter = new SqlDataAdapter(query, connStr);
                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(dataAdapter);

                DataTable table = new DataTable
                {
                    Locale = CultureInfo.InvariantCulture
                };

                dataAdapter.Fill(table);
                bindingSource.DataSource = table;

                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = bindingSource;
            GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                "ITEM_ASKING_PRICE AS 'Asking Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                "ITEM_TIMESTAMP AS Date FROM Inventory");
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            Form1 add = new Form1(userName, password);
            add.Show();
        }

       
        private void btnDonate_Click(object sender, EventArgs e)
        {
            inventoryView = true;
            tableLabel.Text = "Current Inventory:";
            GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                "ITEM_ASKING_PRICE AS 'Asking Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                "ITEM_TIMESTAMP AS Date FROM Inventory");
        
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            inventoryView = false;
            tableLabel.Text = "Sales History:";
            GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                "ITEM_SELLING_PRICE AS 'Selling Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                "ITEM_TIMESTAMP AS Date FROM Sold");

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void btnDonate_Click_1(object sender, EventArgs e)
        {
            inventoryView = true;
            tableLabel.Text = "Current Inventory:";
            GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                "ITEM_ASKING_PRICE AS 'Asking Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                "ITEM_TIMESTAMP AS Date FROM Inventory");
        }

        private void btnSales_Click_1(object sender, EventArgs e)
        {
            inventoryView = false;
            tableLabel.Text = "Sales History:";
            GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                "ITEM_SELLING_PRICE AS 'Selling Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                "ITEM_TIMESTAMP AS Date FROM Sold");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Form1 add = new Form1(userName, password);
            add.Show();
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows == null)
            {
                MessageBox.Show("Please select an item to edit");
                return;
            }

            int itemId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

            string name = "";
            string category = "";
            string type = "";
            string description = "";
            string condition = "";
            string asking = "";
            string purchased = "";

            string table = "Sold";
            if (inventoryView)
            {
                table = "Inventory";

                // get the selected item from inventory
                try
                {
                    string query = "SELECT * FROM " + table + " WHERE ITEM_ID = " + itemId;
                    SqlConnection conn = new SqlConnection(connStr);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        name = reader["ITEM_NAME"].ToString();
                        category = reader["ITEM_CATEGORY"].ToString();
                        type = reader["ITEM_TYPE"].ToString();
                        description = reader["ITEM_DESCRIPTION"].ToString();
                        condition = reader["ITEM_Condition"].ToString();
                        asking = reader["ITEM_ASKING_PRICE"].ToString();
                        purchased = reader["ITEM_PURCHASE_PRICE"].ToString();
                        break;
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Form2 edit = new Form2(table, itemId, name, category, type, description, condition, asking, purchased, userName, password);
                edit.Show();
            }
            else
            {
                string selling = "";

                // get the selected item from sales history
                try
                {
                    string query = "SELECT * FROM Sold WHERE ITEM_ID = " + itemId;
                    SqlConnection conn = new SqlConnection(connStr);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        name = reader["ITEM_NAME"].ToString();
                        category = reader["ITEM_CATEGORY"].ToString();
                        type = reader["ITEM_TYPE"].ToString();
                        description = reader["ITEM_DESCRIPTION"].ToString();
                        condition = reader["ITEM_CONDITION"].ToString();
                        selling = reader["ITEM_SELLING_PRICE"].ToString();
                        purchased = reader["ITEM_PURCHASE_PRICE"].ToString();

                        break;
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Form3 edit = new Form3("Sold", itemId, name, category, type, description, condition, selling, purchased, userName, password);
                edit.Show();
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            int itemId;
            try
            {
                itemId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

            }
            catch (Exception ex)
            {
                MessageBox.Show("No item selected");
                return;
            }

            string table = "Sold";
            if (inventoryView)
            {
                table = "Inventory";
            }

            // delete the selected item from appropriate table
            try
            {
                string query = "DELETE FROM " + table + " WHERE ITEM_ID = " + itemId;
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (inventoryView)
            {
                GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                "ITEM_ASKING_PRICE AS 'Asking Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                "ITEM_TIMESTAMP AS Date FROM Inventory");
            }
            else
            {
                GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                "ITEM_SELLING_PRICE AS 'Selling Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                "ITEM_TIMESTAMP AS Date FROM Sold");
            }
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            if (!inventoryView || dataGridView1.SelectedRows == null)
            {
                MessageBox.Show("Please select an item to sell from current inventory");
                return;
            }

            int itemId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
            String id = "";
            String name = "";
            String category = "";
            String type = "";
            String description = "";
            String condition = "";
            String asking = "";
            String purchased = "";

            // get the selected item from inventory
            try
            {
                string query = "SELECT * FROM Inventory WHERE ITEM_ID = " + itemId;
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id = reader["ITEM_ID"].ToString();

                    // make sure to add an escape character to any 's
                    name = reader["ITEM_NAME"].ToString().Replace("'", "''");
                    category = reader["ITEM_CATEGORY"].ToString().Replace("'", "''");
                    type = reader["ITEM_TYPE"].ToString().Replace("'", "''");
                    description = reader["ITEM_DESCRIPTION"].ToString().Replace("'", "''");
                    condition = reader["ITEM_Condition"].ToString().Replace("'", "''");
                    asking = reader["ITEM_ASKING_PRICE"].ToString();
                    purchased = reader["ITEM_PURCHASE_PRICE"].ToString();

                    break;
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // add the selected item to sold

            DateTime todayDate = DateTime.Now;
            string DateNow = todayDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

            try
            {
                string query = "INSERT INTO Sold VALUES(" + int.Parse(id) + ", '" +
                    name + "', '" + category + "', '" + type + "', '" + description +
                    "', '" + condition + "', '" + double.Parse(purchased) + "', '" + double.Parse(asking) +
                    "', '" + DateNow + "');";

                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                // delete from inventory
                query = "DELETE FROM Inventory WHERE ITEM_ID = " + int.Parse(id);
                cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // refresh inventory
            GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                "ITEM_ASKING_PRICE AS 'Asking Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                "ITEM_TIMESTAMP AS Date FROM Inventory");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String searchTerm = "";
            int columnSelection = 0;

            if (SearchDialog("Search by keyword or phrase", "Keyword/Phrase:", ref searchTerm, ref columnSelection) == DialogResult.OK)
            {
                string price = "ITEM_ASKING_PRICE";
                string priceDisplay = "Asking Price";
                string table = "Inventory";
                string tableDisplay = "current inventory";
                string searchColumns = "all columns";

                if (!inventoryView)
                {
                    price = "ITEM_SELLING_PRICE";
                    priceDisplay = "Selling Price";
                    table = "Sold";
                    tableDisplay = "sales history";
                }

                // all-column and column-specific queries
                switch (columnSelection)
                {

                    // all-column search
                    case 0:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_ID) LIKE LOWER('%" + searchTerm +
                    "%') OR LOWER(ITEM_NAME) LIKE LOWER('%" + searchTerm + "%') OR LOWER(ITEM_CATEGORY) LIKE LOWER('%" +
                    searchTerm + "%') OR LOWER(ITEM_TYPE)" + "LIKE LOWER('%" + searchTerm + "%') OR LOWER(ITEM_DESCRIPTION) " +
                    "LIKE LOWER('%" + searchTerm + "%')" + "OR LOWER(" + price + ") LIKE LOWER('%" + searchTerm + "%')" +
                    " OR LOWER(ITEM_TIMESTAMP) LIKE LOWER('%" + searchTerm + "%')");
                        if (dataGridView1.RowCount == 0)
                        {
                            MessageBox.Show("No matches found for \"" + searchTerm + "\" in " + searchColumns);
                        }
                        break;

                    // id search
                    case 1:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_ID) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Id";
                        if (dataGridView1.RowCount == 0)
                        {
                            MessageBox.Show("No matches found for \"" + searchTerm + "\" in " + searchColumns);
                        }
                        break;

                    // name search
                    case 2:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_NAME) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Name";
                        if (dataGridView1.RowCount == 0)
                        {
                            MessageBox.Show("No matches found for \"" + searchTerm + "\" in " + searchColumns);
                        }
                        break;

                    // category search
                    case 3:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_CATEGORY) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Category";
                        if (dataGridView1.RowCount == 0)
                        {
                            MessageBox.Show("No matches found for \"" + searchTerm + "\" in " + searchColumns);
                        }
                        break;

                    // type search
                    case 4:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_TYPE) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Type";
                        if (dataGridView1.RowCount == 0)
                        {
                            MessageBox.Show("No matches found for \"" + searchTerm + "\" in " + searchColumns);
                        }
                        break;

                    // description search
                    case 5:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_DESCRIPTION) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Description";
                        if (dataGridView1.RowCount == 0)
                        {
                            MessageBox.Show("No matches found for \"" + searchTerm + "\" in " + searchColumns);
                        }
                        break;

                    // condition search
                    case 6:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_CONDITION) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Condition";
                        if (dataGridView1.RowCount == 0)
                        {
                            MessageBox.Show("No matches found for \"" + searchTerm + "\" in " + searchColumns);
                        }
                        break;

                    // price search
                    case 7:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(" + price + ") LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = priceDisplay;
                        if (dataGridView1.RowCount == 0)
                        {
                            MessageBox.Show("No matches found for \"" + searchTerm + "\" in " + searchColumns);
                        }
                        break;

                    // date search
                    case 8:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_TIMESTAMP) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Date";
                        if (dataGridView1.RowCount == 0)
                        {
                            MessageBox.Show("No matches found for \"" + searchTerm + "\" in " + searchColumns);
                        }
                        break;
                }

                // update tag for current table, search terms, and search columns
                // replace any escaped 's with single '
                tableLabel.Text = "Results for \"" + searchTerm.Replace("''", "'") + "\" in " + searchColumns + " in " + tableDisplay + ":";
            }
        }
        

        public static DialogResult SearchDialog(string title, string prompt, ref String value, ref int columnSelection)
        {
            Form search = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonSearch = new Button();
            Button buttonCancel = new Button();
            ComboBox columnSelect = new ComboBox();
            columnSelect.Items.AddRange(new object[] { "All", "Id", "Item Name", "Category", "Type", "Description", "Condition", "Price", "Date" });
            columnSelect.SelectedIndex = 0;
            columnSelect.DropDownStyle = ComboBoxStyle.DropDownList;

            search.Text = title;
            label.Text = prompt;
            buttonSearch.Text = "Search";
            buttonCancel.Text = "Cancel";

            buttonSearch.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(36, 45, 372, 13);
            textBox.SetBounds(36, 75, 700, 20);
            columnSelect.SetBounds(36, 110, 150, 20);

            buttonSearch.SetBounds(180, 160, 160, 60);
            buttonCancel.SetBounds(400, 160, 160, 60);
            buttonSearch.Size = new Size(200, 30);
            buttonCancel.Size = new Size(200, 30);

            label.AutoSize = true;
            search.ClientSize = new Size(796, 250);
            search.FormBorderStyle = FormBorderStyle.FixedDialog;
            search.StartPosition = FormStartPosition.CenterScreen;
            search.MinimizeBox = false;
            search.MaximizeBox = false;

            search.Controls.AddRange(new Control[] { label, textBox, columnSelect, buttonSearch, buttonCancel });
            search.AcceptButton = buttonSearch;
            search.CancelButton = buttonCancel;

            DialogResult dialogResult = search.ShowDialog();

            // escape any 's
            value = textBox.Text.Replace("'", "''");

            // get the column selection
            columnSelection = columnSelect.SelectedIndex;

            return dialogResult;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
