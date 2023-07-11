﻿using System;
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
        private string userName, password, connStr;
        public Form1(string userName, string password)
        {
            InitializeComponent();
            this.userName = userName;
            this.password = password;
            connStr = "Server=localhost; Database=ThriftyTrak; User Id=" + userName + "; Password=" + password;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // add item to inventory

            try
            {
                DateTime todayDate = DateTime.Now;
                String DateNow = todayDate.ToString("yyyy-MM-dd HH:mm:ss.ffff");

                // make sure to add an escape character to any 's
                String name = txtBoxName.Text.ToString().Replace("'", "''");
                String category = txtBoxCategory.Text.ToString().Replace("'", "''"); ;
                String type = txtBoxType.Text.ToString().Replace("'", "''"); ;
                String description = txtBoxDescription.Text.ToString().Replace("'", "''"); ;
                String condition = txtBoxCondition.Text.ToString().Replace("'", "''"); ;
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
