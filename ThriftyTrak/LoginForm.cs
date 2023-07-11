using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace ThriftyTrak
{
    public partial class LoginForm : Form
    {
        private int attemptsRemaining = 4;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUsername.Text;
            string password = txtPassword.Text;
            bool valid = false;

            try
            {
                string connStr = "Server=localhost; Database=ThriftyTrak; User Id=" + userName + "; Password=" + password; 
                string query = "SELECT * FROM Inventory";
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                conn.Close();
                valid = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Incorrect Username or Password. " + --attemptsRemaining + " login attempts remaining.");
            }
            if (valid)
            {
                Form5 dashboard = new Form5(userName, password);
                this.Hide();
                dashboard.FormClosed += new FormClosedEventHandler(dashboard_FormClosed);
                dashboard.Show();
            }
            else
            {
                if (attemptsRemaining <= 0)
                {
                    MessageBox.Show("Login attempt limit exceeded.");
                    Close();
                }
            }
        }

        private void dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
            attemptsRemaining = 4;
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtUsername.Focus();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
