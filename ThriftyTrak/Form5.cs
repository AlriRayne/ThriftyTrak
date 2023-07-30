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
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ThriftyTrak
{
    public partial class Form5 : Form
    {
        private BindingSource bindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private bool inventoryView = true;
        private bool dashBoardView = true;
        private string userName, password, connStr;


        public Form5(string userName, string password, string connStr)
        {
            InitializeComponent();

            this.userName = userName;
            this.password = password;
            this.connStr = connStr;

            lblGreeting.Text = "Hello, " + userName + "!";
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);

            dataGridView2.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom);  
            
            dataGridView1.AllowUserToResizeColumns = true;

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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //adding GetData for Sales in DGV2
        private void GetSales()
        {
            dataGridView2.DataSource = null;

            try
            {
                SqlCommand command = new SqlCommand();
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                command.Connection = conn;
                command.CommandText = "SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                "ITEM_SELLING_PRICE AS 'Selling Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                "ITEM_TIMESTAMP AS Date FROM Sold";
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt;
                conn.Close();
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
            this.ResizeEnd += new EventHandler(Form5_ResizeEnd);

            GetSales();
        }

        private void Form5_ResizeEnd(object sender, EventArgs e)
        {
            if (inventoryView)
            {
                DisplayInventory();
            }
            else
            {
                DisplaySales();
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            myBtnSetting(btnAdd, null);
            Form1 add = new Form1(userName, password, connStr);
            add.Show();
        }

        private void btnDonate_Click_1(object sender, EventArgs e)
        {
            dashBoardView = false;
            myBtnSetting(btnDonate, null);
            dataGridView2.Visible = false;
            lblSalesTable.Visible = false;
            dataGridView1.Height = 500;
            DisplayInventory();
        }

        private void DisplayInventory()
        {
            inventoryView = true;
            tableLabel.Text = "Current Inventory:";
            GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                "ITEM_ASKING_PRICE AS 'Asking Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                "ITEM_TIMESTAMP AS Date FROM Inventory");
        }

        private void DisplaySales()
        {
            inventoryView = false;
            tableLabel.Text = "Sales History:";
            GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                "ITEM_SELLING_PRICE AS 'Selling Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                "ITEM_TIMESTAMP AS Date FROM Sold");
        }

        private void btnSales_Click_1(object sender, EventArgs e)
        {
            dashBoardView = false;
            myBtnSetting(btnSales, null);
            dataGridView2.Visible = false;
            lblSalesTable.Visible = false;
            dataGridView1.Height = 500;
            DisplaySales();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            myBtnSetting(btnAdd, null);
            Form1 add = new Form1(userName, password, connStr);
            add.FormClosed += new FormClosedEventHandler(addFormClosed);
            add.Show();
        }

        private void addFormClosed(object sender, FormClosedEventArgs e)
        {
            DisplayInventory();
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            myBtnSetting(btnEdit, null);
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
                Form2 edit = new Form2(table, itemId, name, category, type, description, condition, asking, purchased, userName, password, connStr);
                edit.FormClosed += new FormClosedEventHandler(editInventoryClosed);
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
                Form3 edit = new Form3("Sold", itemId, name, category, type, description, condition, selling, purchased, userName, password, connStr);
                edit.FormClosed += new FormClosedEventHandler(editSalesClosed);
                edit.Show();
            }
        }

        private void editInventoryClosed(object sender, FormClosedEventArgs e)
        {
            DisplayInventory();
        }

        private void editSalesClosed(object sender, FormClosedEventArgs e)
        {
            DisplaySales();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {

            // testing return item function with this button
            //ReturnItem();
            //return;

            myBtnSetting(btnDelete, null);
            int itemId;

            try
            {
                itemId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

            }
            catch (Exception ex)
            {
                MessageBox.Show("No item selected for deletion");
                return;
            }

            String name = (String)dataGridView1.SelectedRows[0].Cells[1].Value;

            var confirmation = MessageBox.Show("Confirm deletion of " + name + "?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (!(confirmation == DialogResult.Yes))
            {
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
            myBtnSetting(btnSell, null);
            if (!inventoryView || dataGridView1.SelectedRows == null)
            {
                MessageBox.Show("Please select an item to sell from current inventory");
                return;
            }


            int itemId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
            String id = "";
            String name = (String)dataGridView1.SelectedRows[0].Cells[1].Value;
            String category = "";
            String type = "";
            String description = "";
            String condition = "";
            String asking = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            String purchased = "";


            var confirmation = MessageBox.Show("Confirm sale of " + name + " for $" + asking + "?", "Confirm Sale", MessageBoxButtons.YesNo);
            if (!(confirmation == DialogResult.Yes))
            {
                return;
            }

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
            string DateNow = todayDate.ToString("yyyy-MM-dd HH:mm:ss");

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
            // refresh sales in dashboard view
            if (dashBoardView)
            {
                GetSales();
            }
        }

        private void ReturnItem()
        {
            try
            {
                DataGridView table = dashBoardView ? dataGridView2 : dataGridView1;
                int itemId = (int)table.SelectedRows[0].Cells[0].Value;
                String id = "";
                String name = (String)table.SelectedRows[0].Cells[1].Value;
                String category = "";
                String type = "";
                String description = "";
                String condition = "";
                String asking = table.SelectedRows[0].Cells[6].Value.ToString();
                String purchased = "";
                String selling = "";

                var confirmation = MessageBox.Show("Confirm return of " + name + " for $" + asking + "?", "Confirm Return", MessageBoxButtons.YesNo);
                if (!(confirmation == DialogResult.Yes))
                {
                    return;
                }


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
                        id = reader["ITEM_ID"].ToString();

                        // make sure to add an escape character to any 's
                        name = reader["ITEM_NAME"].ToString().Replace("'", "''");
                        category = reader["ITEM_CATEGORY"].ToString().Replace("'", "''");
                        type = reader["ITEM_TYPE"].ToString().Replace("'", "''");
                        description = reader["ITEM_DESCRIPTION"].ToString().Replace("'", "''");
                        condition = reader["ITEM_Condition"].ToString().Replace("'", "''");
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

                // add the selected item back to inventory

                DateTime todayDate = DateTime.Now;
                string DateNow = todayDate.ToString("yyyy-MM-dd HH:mm:ss");

                try
                {
                    String query = "INSERT INTO Inventory VALUES('" +
                        name + "', '" + category + "', '" + type + "', '" + description +
                        "', '" + condition + "', " + selling + ", " + purchased + ", '" + DateNow + "' );";

                    SqlConnection conn = new SqlConnection(connStr);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();

                    // delete from sales history
                    query = "DELETE FROM Sold WHERE ITEM_ID = " + int.Parse(id);
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
                if (dashBoardView)
                {
                    MessageBox.Show("GetSales()");
                    GetSales();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            myBtnSetting(btnSearch, null);
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
                        break;

                    // id search
                    case 1:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_ID) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Id";
                        break;

                    // name search
                    case 2:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_NAME) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Name";
                        break;

                    // category search
                    case 3:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_CATEGORY) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Category";
                        break;

                    // type search
                    case 4:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_TYPE) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Type";
                        break;

                    // description search
                    case 5:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_DESCRIPTION) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Description";
                        break;

                    // condition search
                    case 6:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_CONDITION) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Condition";
                        break;

                    // price search
                    case 7:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(" + price + ") LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = priceDisplay;
                        break;

                    // date search
                    case 8:
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                    "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition, " +
                    price + " AS '" + priceDisplay + "', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                    "ITEM_TIMESTAMP AS Date FROM " + table + " WHERE LOWER(ITEM_TIMESTAMP) LIKE LOWER('%" + searchTerm + "%')");
                        searchColumns = "Date";
                        break;
                }

                // show error message for no results
                if (dataGridView1.RowCount == 0)
                {
                    MessageBox.Show("No matches found for \"" + searchTerm + "\" in " + searchColumns);
                }

                // update tag for current table, search terms, and search columns
                // replace any escaped 's with single '
                tableLabel.Text = "Results for \"" + searchTerm.Replace("''", "'") + "\" in " + searchColumns + " in " + tableDisplay + ":";
            }
        }

        private void btnDash_Click(object sender, EventArgs e)
        {
            dashBoardView = true;
            myBtnSetting(btnDash, null);
            dataGridView2.Visible = true;
            lblSalesTable.Visible = true;
            dataGridView1.Height = 250;
            DisplayInventory();
            GetSales();
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

        private void btnReturn_Click(object sender, EventArgs e)
        {
            myBtnSetting(btnReturn, null);
        }

        //Sign Out button
        private void button2_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to sign out?", "Sign Out Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            LoginForm login = new LoginForm();
            this.Hide();
            login.ShowDialog();
            this.Close();
        }

        //Change button colors in navPanel
        private void myBtnSetting (object sender, EventArgs e)
        {
            foreach(Control c in navPanel.Controls)
            {
                c.BackColor = Color.FromArgb(255,102,102,153);
            }
            Control click = (Control)sender;
            click.BackColor = Color.FromArgb(255,0,176,80);
        }
    }
}
