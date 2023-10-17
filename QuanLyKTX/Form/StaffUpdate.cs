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
using System.Xml.Linq;

namespace QuanLyKTX
{
    public partial class StaffUpdate : Form
    {
        KTXManagementSQLConn managementSQLConn = new KTXManagementSQLConn();
        string query;
        Validate validate = new Validate();

        //biến lưu giá trị id để update và xóa
        int id;

        //biến cờ đánh dấu đã chọn nhân viên
        bool searchedWG = false;

        public StaffUpdate()
        {
            InitializeComponent();
        }
        private void StaffUpdate_Load(object sender, EventArgs e)
        {

        }

        private void ClearStaffInfo()
        {
            txtStaffName.Clear();
            txtCCCD.Clear();
            txtDadName.Clear();
            txtMotherName.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            cboPosition.SelectedIndex = -1;
        }

        #region Validate
        private bool CheckNull()
        {

            if (!(validate.CheckNullInTextBox(errStaffName, txtStaffName, txtStaffName.Text, "Bạn chưa nhập tên nhân viên"))
                || !(validate.CheckNullInTextBox(errCCCD, txtCCCD, txtCCCD.Text, "Bạn chưa nhập CCCD"))
                || !(validate.CheckNullInComboBox(errPosition, cboPosition, cboPosition.Text, "Bạn chưa chọn chức vụ")))
            {
                return false;
            }

            return true;
        }

        //Chi cho phep nhap so vao CCCD
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

        private bool IsValidate()
        {
            if (!string.IsNullOrEmpty(txtPhone.Text) && !validate.PhoneValid(txtPhone.Text))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string input = txtStaffName.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Bạn chưa nhập tên nhân viên", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                query = $"select * from newEmployee where ename LIKE '%{input}%';";
                DataSet dataSet = managementSQLConn.GetData(query) ;

                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    searchedWG = true;

                    lblText1.Visible = false;

                    id = Convert.ToInt32(dataSet.Tables[0].Rows[0]["id"]);
                    txtStaffName.Text = dataSet.Tables[0].Rows[0]["ename"].ToString();
                    txtCCCD.Text = dataSet.Tables[0].Rows[0]["eidproof"].ToString();
                    txtDadName.Text = dataSet.Tables[0].Rows[0]["efname"].ToString();
                    txtMotherName.Text = dataSet.Tables[0].Rows[0]["emname"].ToString();
                    txtEmail.Text = dataSet.Tables[0].Rows[0]["eemail"].ToString();
                    txtAddress.Text = dataSet.Tables[0].Rows[0]["epaddress"].ToString();
                    txtPhone.Text = dataSet.Tables[0].Rows[0]["emobile"].ToString();
                    cboPosition.Text = dataSet.Tables[0].Rows[0]["edesignation"].ToString();
                    cboStatus.Text = dataSet.Tables[0].Rows[0]["working"].ToString();
                }
                else
                {
                    lblText1.Visible = true;
                    lblText1.Text = "Nhân viên không tồn tại";
                }
            }
                
            

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(searchedWG == true)
            {
                if (!CheckNull())
                {
                    return;
                }
                if (!Validate())
                {
                    return;
                }

                query = $"update newEmployee SET ename = '{txtStaffName.Text}', eidproof = '{txtCCCD.Text}', efname = '{txtDadName.Text}', emname = '{txtMotherName.Text}'," +
                $"eemail = '{txtEmail.Text}', epaddress = '{txtAddress.Text}', emobile = '{txtPhone.Text}', edesignation = '{cboPosition.Text}', working = '{cboStatus.Text}' " +
                $"WHERE id = {id}";

                managementSQLConn.SetData(query, "Cập nhật nhân viên thành công");
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn nhân viên", "Cảnh báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtStaffName.Focus();
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(searchedWG == true)
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có muốn xóa nhân viên này không?", "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.OK)
                {
                    query = $"delete from newEmployee where id = {id}";
                    managementSQLConn.SetData(query, "Xóa nhân viên thành công");

                    searchedWG = false;
                    ClearStaffInfo();
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn nhân viên", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStaffName.Focus();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            searchedWG = false;
            ClearStaffInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
