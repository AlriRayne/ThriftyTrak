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
            this.userName = userName;
            this.password = password;
            connStr = "Server=localhost; Database=ThriftyTrak; User Id=" + userName + "; Password=" + password;
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

            /* 
             * Ignore this for now, I was experimenting with how to implement a keyword
             * search. This is for next sprint :)
             * 
            String searchTerm = "";
            bool wholeWordMatch = false;
            if (SearchDialog("Search by keyword", "Keyword:", ref searchTerm, ref wholeWordMatch) == DialogResult.OK)
            {
                MessageBox.Show("Whole word: " + wholeWordMatch.ToString());
                if (inventoryView)
                {
                    // replace any escaped 's with single '
                    tableLabel.Text = "Results for \"" + searchTerm.Replace("''", "'") + "\" in current inventory:";

                    if (wholeWordMatch)
                    {
                        // query for whole word match only
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                            "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                            "ITEM_ASKING_PRICE AS 'Asking Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                            "ITEM_TIMESTAMP AS Date FROM Inventory WHERE LOWER(ITEM_NAME) LIKE LOWER('%[^a-z]" + searchTerm +
                            "[^a-z]%') OR LOWER(ITEM_CATEGORY) LIKE LOWER('%[^a-z]" + searchTerm + "[^a-z]%') OR LOWER(ITEM_TYPE)" +
                            "LIKE LOWER('%[^a-z]" + searchTerm + "[^a-z]%') OR LOWER(ITEM_DESCRIPTION) LIKE LOWER('%[^a-z]" + searchTerm + "[^a-z]%')");
                    }
                    else
                    {
                        // query for any match
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                            "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                            "ITEM_ASKING_PRICE AS 'Asking Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                            "ITEM_TIMESTAMP AS Date FROM Inventory WHERE LOWER(ITEM_NAME) LIKE LOWER('%" + searchTerm +
                            "%') OR LOWER(ITEM_CATEGORY) LIKE LOWER('%" + searchTerm + "%') OR LOWER(ITEM_TYPE)" +
                            "LIKE LOWER('%" + searchTerm + "%') OR LOWER(ITEM_DESCRIPTION) LIKE LOWER('%" + searchTerm + "%')");
                    }
                }
                else
                {
                    // replace any escaped 's with single '
                    tableLabel.Text = "Results for \"" + searchTerm.Replace("''", "'") + "\" in sales history:";

                    if (wholeWordMatch)
                    {
                        // query for whole word match only
                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                            "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                            "ITEM_SELLING_PRICE AS 'Selling Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                            "ITEM_TIMESTAMP AS Date FROM Sold WHERE LOWER(ITEM_NAME) LIKE LOWER('%[^a-z]" + searchTerm +
                            "[^a-z]%') OR LOWER(ITEM_CATEGORY) LIKE LOWER('%[^a-z]" + searchTerm + "[^a-z]%') OR LOWER(ITEM_TYPE)" +
                            "LIKE LOWER('%[^a-z]" + searchTerm + "[^a-z]%') OR LOWER(ITEM_DESCRIPTION) LIKE LOWER('%[^a-z]" + searchTerm + "[^a-z]%')");
                    }
                    else
                    {
                        // query for whole word match only

                        GetData("SELECT ITEM_ID AS Id, ITEM_NAME AS Name, ITEM_CATEGORY AS Category," +
                            "ITEM_TYPE AS Type, ITEM_DESCRIPTION AS Description, ITEM_CONDITION AS Condition," +
                            "ITEM_SELLING_PRICE AS 'Selling Price', ITEM_PURCHASE_PRICE AS 'Purchase Price'," +
                            "ITEM_TIMESTAMP AS Date FROM Sold WHERE LOWER(ITEM_NAME) LIKE LOWER('%" + searchTerm +
                            "%') OR LOWER(ITEM_CATEGORY) LIKE LOWER('%" + searchTerm + "%') OR LOWER(ITEM_TYPE)" +
                            "LIKE LOWER('%" + searchTerm + "%') OR LOWER(ITEM_DESCRIPTION) LIKE LOWER('%" + searchTerm + "%')");
                    }
                }
            }
            */
        }

        public static DialogResult SearchDialog(string title, string prompt, ref String value, ref bool wholeWordMatch)
        {
            Form search = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            CheckBox wholeWord = new CheckBox();
            Button buttonSearch = new Button();
            Button buttonCancel = new Button();

            search.Text = title;
            label.Text = prompt;
            buttonSearch.Text = "Search";
            buttonCancel.Text = "Cancel";
            wholeWord.Text = "Match whole word only";

            buttonSearch.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(36, 45, 372, 13);
            textBox.SetBounds(36, 75, 700, 20);
            wholeWord.SetBounds(36, 110, 372, 20);
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

            search.Controls.AddRange(new Control[] { label, textBox, wholeWord, buttonSearch, buttonCancel });
            search.AcceptButton = buttonSearch;
            search.CancelButton = buttonCancel;

            DialogResult dialogResult = search.ShowDialog();

            // escape any 's
            value = textBox.Text.Replace("'", "''");

            wholeWordMatch = wholeWord.Checked;

            return dialogResult;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
