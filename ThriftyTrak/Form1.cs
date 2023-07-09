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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ThriftyTrak
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // add item to inventory

            try
            {
                String connStr = "Server=localhost; Database=ThriftyTrak; Integrated Security=True";
                DateTime todayDate = DateTime.Now;
                String DateNow = todayDate.ToString("yyyy-MM-dd HH:mm:ss.ffff");
                String name = txtBoxName.Text.ToString();
                String category = txtBoxCategory.Text.ToString();
                String type = txtBoxType.Text.ToString();
                string description = txtBoxDescription.Text.ToString();
                string condition = txtBoxCondition.Text.ToString();
                double askingPrice = (float)Convert.ToDouble(txtBoxAskingPrice.Text);
                double purchasePrice = (float)Convert.ToDouble(txtBoxPurchasePrice.Text);

                String insert = "INSERT INTO Inventory VALUES('" +
                    name + "', '" + category + "', '" + type + "', '" + description +
                    "', '" + condition + "', " + askingPrice + ", " + purchasePrice + ", '" + DateNow + "' );";
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(insert, conn);
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
    }
}
