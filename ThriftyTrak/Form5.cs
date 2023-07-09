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

namespace ThriftyTrak
{
    public partial class Form5 : Form
    {
        private BindingSource bindingSource = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private bool inventoryView = true;

        public Form5()
        {
            InitializeComponent();
        }

        private void GetData(string query)
        {
            try
            {
                string connStr = "Server=localhost; Database=ThriftyTrak; Integrated Security=True";

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
            Form1 add = new Form1();
            add.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
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
                    string connStr = "Server=localhost; Database=ThriftyTrak; Integrated Security=True";
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
                Form2 edit = new Form2(table, itemId, name, category, type, description, condition, asking, purchased);
                edit.Show();
            }
            else
            {
                string selling = "";

                // get the selected item from sales history
                try
                {
                    string connStr = "Server=localhost; Database=ThriftyTrak; Integrated Security=True";
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
                Form3 edit = new Form3("Sold", itemId, name, category, type, description, condition, selling, purchased);
                edit.Show();
            }
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int itemId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

            string table = "Sold";
            if (inventoryView)
            {
                table = "Inventory";
            }

            // delete the selected item from appropriate table
            try
            {
                string connStr = "Server=localhost; Database=ThriftyTrak; Integrated Security=True";
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

        private void btnSellItem_Click(object sender, EventArgs e)
        {
            if (!inventoryView || dataGridView1.SelectedRows == null)
            {
                MessageBox.Show("Please select an item to sell from current inventory");
                return;
            }

            int itemId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
            string id = "";
            string name = "";
            string category = "";
            string type = "";
            string description = "";
            string condition = "";
            string asking = "";
            string purchased = "";

            // get the selected item from inventory
            try
            {
                string connStr = "Server=localhost; Database=ThriftyTrak; Integrated Security=True";

                string query = "SELECT * FROM Inventory WHERE ITEM_ID = " + itemId;
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id = reader["ITEM_ID"].ToString();
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

            // add the selected item to sold

            DateTime todayDate = DateTime.Now;
            string DateNow = todayDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

            try
            {
                string connStr = "Server=localhost; Database=ThriftyTrak; Integrated Security=True";

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
    }
}
