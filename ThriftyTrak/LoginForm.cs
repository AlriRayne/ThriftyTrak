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
        public static string userName = "";
        public static string password = "";

        public LoginForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            txtPassword.KeyUp += HandleEnter;
        }

        private void HandleEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
                e.Handled = true;
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            userName = txtUsername.Text;
            password = txtPassword.Text;

            if (userName == "" || password == "")
            {
                MessageBox.Show("Text fields are required. Please enter a valid username and password.");
            }
            else
            {
                //Use SQLEXPRESS for first version of database. Use just (local) for second version.
                var datasource = @"(local)\SQLEXPRESS";
                var database = "ThriftyTrak";
                var thisUsername = userName;
                var thisPassword = password;
                string connString = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Persist Security Info=True;User ID=" + userName + ";Password=" + password;
                string connStr = "Server=localhost; Database=ThriftyTrak; User Id=" + userName + "; Password=" + password;
                SqlConnection conn = new SqlConnection(connString);

                // try the connection all three ways
                // and you might want to try moving these around to see which can work. I put the named SQLSERVER
                // instance at the top.
                try
                {
                    conn.Open();
                    Form5 dashboard = new Form5(userName, password, connStr);
                    this.Hide();
                    dashboard.ShowDialog();
                    this.Close();
                }
                catch (Exception firstE)
                {
                    try
                    {
                        datasource = @"(local)";
                        connString = @"Data Source=" + datasource + ";Initial Catalog=" + database + ";Persist Security Info=True;User ID=" + userName + ";Password=" + password;
                        conn = new SqlConnection(connString);
                        conn.Open();
                        Form5 dashboard = new Form5(userName, password, connString);
                        this.Hide();
                        dashboard.ShowDialog();
                        this.Close();
                    }
                    catch (Exception secondE)
                    { 
                        // let the outer catch block produce one error message
                    }
                    try
                    {
                        conn = new SqlConnection(connStr);
                        conn.Open();
                        Form5 dashboard = new Form5(userName, password, connStr);
                        this.Hide();
                        dashboard.ShowDialog();
                        this.Close();
                    }
                    catch (Exception exce)
                    {
                        // let the outer catch block produce one error message
                    }
                    MessageBox.Show("Error: " + firstE.Message);
                }
            }
        }
    }
}
