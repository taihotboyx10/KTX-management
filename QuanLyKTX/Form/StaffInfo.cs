using QuanLyKTX.Util;
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
    public partial class StaffInfo : Form
    {
        KTXManagementSQLConn managementSQLConn = new KTXManagementSQLConn();
        string query;
        Validate validate = new Validate();
        public StaffInfo()
        {
            InitializeComponent();
        }

        private void StaffInfo_Load(object sender, EventArgs e)
        {
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckNull())
            {
                return;
            }
            if (!IsValidate())
            {
                return;
            }
            query = $"insert into newEmployee (ename, eidproof, efname,emname, eemail, epaddress, emobile, edesignation) " +
                $"values ('{txtStaffName.Text}','{txtCCCD.Text}','{txtDadName.Text}','{txtMotherName.Text}','{txtEmail.Text}','{txtAddress.Text}','{txtPhone.Text}','{cboPosition.Text}')";
            managementSQLConn.SetData(query, "Lưu nhân viên thành công");

            cboPosition.Text = "";
            ClearInput();
        }

        #region Validate

        public bool CheckNull()
        {

            if (!(validate.CheckNullInTextBox(errStaffName, txtStaffName, txtStaffName.Text, "Bạn chưa nhập tên nhân viên"))
                || !(validate.CheckNullInTextBox(errCCCD, txtCCCD, txtCCCD.Text, "Bạn chưa nhập CCCD"))
                || !(validate.CheckNullInComboBox(errPosition, cboPosition, cboPosition.Text, "Bạn chưa chọn chức vụ cho nhân viên")))
            {
                return false;
            }

            return true;
        }

        public bool IsValidate()
        {
            if(!string.IsNullOrEmpty(txtPhone.Text) && !validate.PhoneValid(txtPhone.Text))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng", "Cảnh báo",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhone.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(txtEmail.Text) && !validate.EmailValid(txtEmail.Text))
            {
                MessageBox.Show("Mail không đúng định dạng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Focus();
                return false;
            }

            return true;
        }

        private void ClearInput()
        {
            txtStaffName.Text = "";
            txtCCCD.Text = "";
            txtDadName.Text = "";
            txtEmail.Text = "";
            txtMotherName.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            cboPosition.SelectedIndex = -1;
        }

        private void txtCCCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem ký tự vừa nhập có phải là số không
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) // 8 là phím Backspace
            {
                e.Handled = true; // Chặn ký tự không phải số
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem ký tự vừa nhập có phải là số không
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) // 8 là phím Backspace
            {
                e.Handled = true; // Chặn ký tự không phải số
            }
        }
        #endregion

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInput();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
