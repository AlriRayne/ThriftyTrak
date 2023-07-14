using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ThriftyTrak
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnDash_Click(object sender, EventArgs e)
        {

        }

        private void btnDonate_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                var datasource = @"(local)\SQLEXPRESS";
                var database = "ThriftyTrak";
                string connStr = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Integrated Security=True";

                string query = "SELECT * FROM Inventory";
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    string entry = reader["ITEM_ID"].ToString();
                    entry += "  |  Name: " + reader["ITEM_NAME"].ToString();
                    entry += "  |  Category: " + reader["ITEM_CATEGORY"].ToString();
                    entry += "  |  Type: " + reader["ITEM_TYPE"].ToString();
                    entry += "  |  Description: " + reader["ITEM_DESCRIPTION"].ToString();
                    entry += "  |  Condition: " + reader["ITEM_CONDITION"].ToString();
                    entry += "  |  Purchased for: $" + reader["ITEM_PURCHASE_PRICE"].ToString();
                    entry += "  |  Asking: $" + reader["ITEM_ASKING_PRICE"].ToString();
                    entry += "  |  Timestamp: " + reader["ITEM_TIMESTAMP"].ToString();


                    listBox1.Items.Add(entry);


                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.Clear();
                var datasource = @"(local)\SQLEXPRESS";
                var database = "ThriftyTrak";
                string connStr = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Integrated Security=True";

                string query = "SELECT * FROM Sold";
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    string entry = reader["ITEM_ID"].ToString();
                    entry += "  |  Name: " + reader["ITEM_NAME"].ToString();
                    entry += "  |  Category: " + reader["ITEM_CATEGORY"].ToString();
                    entry += "  |  Type: " + reader["ITEM_TYPE"].ToString();
                    entry += "  |  Description: " + reader["ITEM_DESCRIPTION"].ToString();
                    entry += "  |  Condition: " + reader["ITEM_CONDITION"].ToString();
                    entry += "  |  Purchased for: $" + reader["ITEM_PURCHASE_PRICE"].ToString();
                    entry += "  |  Sold for: $" + reader["ITEM_SELLING_PRICE"].ToString();
                    entry += "  |  Timestamp: " + reader["ITEM_TIMESTAMP"].ToString();


                    listBox2.Items.Add(entry);


                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Form1 add = new Form1("", "");
            add.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select an item to edit from the inventory");
                return;
            }

            string item = listBox1.SelectedItem.ToString();
            string digits = new string(item.TakeWhile(Char.IsDigit).ToArray());
            int itemId = Convert.ToInt32(digits);
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
                var datasource = @"(local)\SQLEXPRESS";
                var database = "ThriftyTrak";
                string connStr = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Integrated Security=True";
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
            Form2 edit = new Form2("Inventory", int.Parse(id), name, category, type, description, condition, asking, purchased, "", "");
            edit.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                if (listBox1.SelectedItem ==  null)
                {
                    MessageBox.Show("Please select an item to sell from the inventory");
                    return;
                }
                string item = listBox1.SelectedItem.ToString();
                string digits = new string(item.TakeWhile(Char.IsDigit).ToArray());
                int itemId = Convert.ToInt32(digits);
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
                    var datasource = @"(local)\SQLEXPRESS";
                    var database = "ThriftyTrak";
                    string connStr = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Integrated Security=True";

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
                    var datasource = @"(local)\SQLEXPRESS";
                    var database = "ThriftyTrak";
                    string connStr = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Integrated Security=True";

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
                try
                {
                    listBox1.Items.Clear();
                    var datasource = @"(local)\SQLEXPRESS";
                    var database = "ThriftyTrak";
                    string connStr = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Integrated Security=True";

                    string query = "SELECT * FROM Inventory";
                    SqlConnection conn = new SqlConnection(connStr);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        string entry = reader["ITEM_ID"].ToString();
                        entry += "  |  Name: " + reader["ITEM_NAME"].ToString();
                        entry += "  |  Category: " + reader["ITEM_CATEGORY"].ToString();
                        entry += "  |  Type: " + reader["ITEM_TYPE"].ToString();
                        entry += "  |  Description: " + reader["ITEM_DESCRIPTION"].ToString();
                        entry += "  |  Condition: " + reader["ITEM_Condition"].ToString();
                        entry += "  |  Asking: $" + reader["ITEM_ASKING_PRICE"].ToString();
                        entry += "  |  Purchased for: $" + reader["ITEM_PURCHASE_PRICE"].ToString();
                        entry += "  |  Timestamp: " + reader["ITEM_TIMESTAMP"].ToString();

                        listBox1.Items.Add(entry);
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                // refresh sold

                try
                {
                    listBox2.Items.Clear();
                    var datasource = @"(local)\SQLEXPRESS";
                    var database = "ThriftyTrak";
                    string connStr = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Integrated Security=True";

                    string query = "SELECT * FROM Sold";
                    SqlConnection conn = new SqlConnection(connStr);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string entry = reader["ITEM_ID"].ToString();
                        entry += "  |  Name: " + reader["ITEM_NAME"].ToString();
                        entry += "  |  Category: " + reader["ITEM_CATEGORY"].ToString();
                        entry += "  |  Type: " + reader["ITEM_TYPE"].ToString();
                        entry += "  |  Description: " + reader["ITEM_DESCRIPTION"].ToString();
                        entry += "  |  Condition: " + reader["ITEM_CONDITION"].ToString();
                        entry += "  |  Purchased for: $" + reader["ITEM_PURCHASE_PRICE"].ToString();
                        entry += "  |  Sold for: $" + reader["ITEM_SELLING_PRICE"].ToString();
                        entry += "  |  Timestamp: " + reader["ITEM_TIMESTAMP"].ToString();

                        listBox2.Items.Add(entry);
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnEditSale_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Please select an item to edit from the sales history");
                return;
            }

            string item = listBox2.SelectedItem.ToString();
            string digits = new string(item.TakeWhile(Char.IsDigit).ToArray());
            int itemId = Convert.ToInt32(digits);
            string id = "";
            string name = "";
            string category = "";
            string type = "";
            string description = "";
            string condition = "";
            string selling = "";
            string purchased = "";

            // get the selected item from sales history
            try
            {
                var datasource = @"(local)\SQLEXPRESS";
                var database = "ThriftyTrak";
                string connStr = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Integrated Security=True";
                string query = "SELECT * FROM Sold WHERE ITEM_ID = " + itemId;
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
            Form3 edit = new Form3("Sold", int.Parse(id), name, category, type, description, condition, selling, purchased, "", "");
            edit.Show();
        }
    }
}
