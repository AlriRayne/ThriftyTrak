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
    public partial class Form2 : Form
    {
        private int id;
        private string table, userName, password, connStr;

        public Form2(string table, int id, string name, string category, string type, 
            string description, string condition, string asking, string purchased, string userName, string password, string connStr)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.userName = userName;
            this.password = password;
            this.connStr = connStr;

            // set item id for reference and choose table

            this.id = id;
            this.table = table;

            // fill in existing stats (on both sides)

            txtBoxName.Text = name;
            txtBoxCategory.Text = category;
            txtBoxType.Text = type;
            txtBoxDescription.Text = description;
            txtBoxCondition.Text = condition;
            txtBoxAskingPrice.Text = asking;
            txtBoxPurchasePrice.Text = purchased;

            txtBoxNewName.Text = name;
            txtBoxNewCategory.Text = category;
            txtBoxNewType.Text = type;
            txtBoxNewDescription.Text = description;
            txtBoxNewCondition.Text = condition;
            txtBoxNewAskingPrice.Text = asking;
            txtBoxNewPurchasePrice.Text = purchased;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // input validation

            // allow for no purchase price
            if (txtBoxNewPurchasePrice.Text == "")
            {
                txtBoxNewPurchasePrice.Text = "0";
            }

            // check for empty text boxes
            if (txtBoxNewName.Text == "" || txtBoxNewCategory.Text == "" || txtBoxNewType.Text == "" ||
                txtBoxNewDescription.Text == "" || txtBoxNewCondition.Text == "" ||
                txtBoxNewAskingPrice.Text == "")
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // further input validation will be in the form of an SQL exception
            try
            {
                DateTime todayDate = DateTime.Now;
                string DateNow = todayDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

                // make sure to add an escape character to any 's
                String name = txtBoxNewName.Text.ToString().Replace("'", "''");
                String category = txtBoxNewCategory.Text.ToString().Replace("'", "''"); ;
                String type = txtBoxNewType.Text.ToString().Replace("'", "''"); ;
                String description = txtBoxNewDescription.Text.ToString().Replace("'", "''"); ;
                String condition = txtBoxNewCondition.Text.ToString().Replace("'", "''"); ;
                double askingPrice = Double.Parse(txtBoxNewAskingPrice.Text);
                double purchasePrice = Double.Parse(txtBoxNewPurchasePrice.Text);

                string update = "UPDATE " + table + " SET ITEM_NAME = '" + name + "', ITEM_CATEGORY = '" + 
                    category + "', ITEM_TYPE = '" + type + "', ITEM_DESCRIPTION = '" + description +
                    "', ITEM_CONDITION = '" + condition + "', ITEM_ASKING_PRICE = " + askingPrice + 
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


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void txtboxNewAskingPrice_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
