using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKTX
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() => txtUserName.Focus()));
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "taind" && txtPassword.Text == "1")
            {
                DashBoard dashBoard = new DashBoard();
                dashBoard.Show();
                this.Hide(); //Ẩn form hiện tại là login
                //this.Close();
                //this.Dispose();
            }

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                errUserName.SetError(txtUserName, "Bạn chưa nhập user name");
                txtUserName.Focus();
            }
            else
            {
                errUserName.Clear();
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                errPassword.SetError(txtPassword, "Bạn chưa nhập password");
                txtPassword.Focus();
            }
            else
            {
                errPassword.Clear();
            }

        }
    }
}
