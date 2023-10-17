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
    public partial class StudentInfo : Form
    {
        KTXManagementSQLConn managementSQLConn = new KTXManagementSQLConn();
        Validate validate = new Validate();
        string query;
        public StudentInfo()
        {
            InitializeComponent();
        }

        private void StudentInfo_Load(object sender, EventArgs e)
        {
            query = "select roomNo from rooms where roomStatus = 1 and Booked = 'No';";
            List<string> roomNoList = managementSQLConn.GetRoomNoList(query);
            foreach (var item in roomNoList)
            {
                cboRoomNo.Items.Add(item);
            }
        }

        #region"Validate"

        private bool CheckNull() 
        {
            if(!(validate.CheckNullInTextBox(errStudentName, txtStudentName, txtStudentName.Text, "Bạn chưa nhập tên sinh viên"))
                || !(validate.CheckNullInTextBox(errCCCD, txtCCCD, txtCCCD.Text, "Bạn chưa nhập CCCD"))
                || !(validate.CheckNullInComboBox(errRoomNo, cboRoomNo, txtStudentName.Text, "Bạn chưa chọn phòng")))
            {
                return false;
            }

            return true;
        }
        //Chi cho phep nhap so vao CCCD

        private void txtCCCD_KeyPress_1(object sender, KeyPressEventArgs e)
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

        private bool IsValidate() 
        { 
            if(!string.IsNullOrEmpty(txtPhone.Text) && !validate.PhoneValid(txtPhone.Text)) 
            {
                MessageBox.Show("Số điện thoại không đúng định dạng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhone.Focus();
                return false;
            }

            if(!string.IsNullOrEmpty(txtEmail.Text) && !validate.EmailValid(txtEmail.Text)) 
            {
                MessageBox.Show("Mail không đúng định dạng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Focus();
                return false;
            }
            return true;
        }
        #endregion

        public void ClearStudentInfo()
        {
            txtStudentName.Clear();
            txtCCCD.Clear();
            txtDadName.Clear();
            txtMotherName.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtSchool.Clear();
            txtPhone.Clear();
            cboRoomNo.SelectedIndex = -1;
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

            query = $"insert into newStudent (mobile, name, fname, mname, email, paddress, college, idproof,roomNo)" +
            $"values ('{txtPhone.Text}', '{txtStudentName.Text}', '{txtDadName.Text}', '{txtMotherName.Text}', '{txtEmail.Text}', '{txtAddress.Text}', '{txtSchool.Text}', '{txtCCCD.Text}', {cboRoomNo.Text})" +
            $"update rooms set Booked = 'Yes' where roomNo = {cboRoomNo.Text}";

            managementSQLConn.SetData(query, "Thêm sinh viên thành công");
            cboRoomNo.Items.Clear();
            StudentInfo_Load(this, null);

            ClearStudentInfo();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult rerult = MessageBox.Show("Bạn có muốn xóa thông tin đang nhập dở không?","Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(rerult == DialogResult.OK)
            {
                ClearStudentInfo();
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
