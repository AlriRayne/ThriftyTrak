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

        public LoginForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public static string userName = "";
        public static string password = "";

        private void btnLogin_Click(object sender, EventArgs e)
        {
            userName = txtUsername.Text;
            password = txtPassword.Text;

            if(userName == "" || password == "")
            {
                MessageBox.Show("Text fields are required. Please enter a valid username and password.");
            }
            else
            {
                var datasource = @"(local)";
                var database = "ThriftyTrak";
                var thisUsername = userName;
                var thisPassword = password;
                string connString = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Persist Security Info=True;User ID=" + userName + ";Password=" + password;
                SqlConnection conn = new SqlConnection(connString);

                try
                {
                    conn.Open();
                    Form5 dashboard = new Form5(userName, password);
                    dashboard.Show();
              
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

        }
    }
}
