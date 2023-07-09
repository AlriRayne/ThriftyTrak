using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class Form3 : Form
    {
        int id;
        string table;

        public Form3(string table, int id, string name, string category, string type, string description, string condition, string selling, string purchased)
        {
            InitializeComponent();

            // set item id for reference and choose table

            this.id = id;
            this.table = table;

            // fill in existing stats (on both sides)

            txtBoxName.Text = name;
            txtBoxCategory.Text = category;
            txtBoxType.Text = type;
            txtBoxDescription.Text = description;
            txtBoxCondition.Text = condition;
            txtBoxSellingPrice.Text = selling;
            txtBoxPurchasePrice.Text = purchased;

            txtBoxNewName.Text = name;
            txtBoxNewCategory.Text = category;
            txtBoxNewType.Text = type;
            txtBoxNewDescription.Text = description;
            txtBoxNewCondition.Text = condition;
            txtBoxNewSellingPrice.Text = selling;
            txtBoxNewPurchasePrice.Text = purchased;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string connStr = "Server=localhost; Database=ThriftyTrak; Integrated Security=True";
                DateTime todayDate = DateTime.Now;
                string DateNow = todayDate.ToString("yyyy-MM-dd HH:mm:ss");
                string name = txtBoxNewName.Text.ToString();
                string category = txtBoxNewCategory.Text.ToString();
                string type = txtBoxNewType.Text.ToString();
                string description = txtBoxNewDescription.Text.ToString();
                string condition = txtBoxNewCondition.Text.ToString();
                double sellingPrice = Double.Parse(txtBoxNewSellingPrice.Text);
                double purchasePrice = Double.Parse(txtBoxNewPurchasePrice.Text);

                string update = "UPDATE " + table + " SET ITEM_NAME = '" + name + "', ITEM_CATEGORY = '" +
                    category + "', ITEM_TYPE = '" + type + "', ITEM_DESCRIPTION = '" + description +
                    "', ITEM_CONDITION = '" + condition + "', ITEM_SELLING_PRICE = " + sellingPrice +
                    ", ITEM_PURCHASE_PRICE = " + purchasePrice + ", ITEM_TIMESTAMP = '" + DateNow + "'" +
                    "WHERE ITEM_ID = " + id;

                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(update, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Close();
        }

        private void txtboxNewAskingPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
