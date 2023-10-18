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
    public partial class StudentUpdate : Form
    {
        KTXManagementSQLConn managementSQLConn = new KTXManagementSQLConn();
        Validate validate = new Validate();
        string query;

        //Biến cờ đánh dấu là đã chọn sinh viên trước khi update, xóa
        bool searchedWG = false;

        //Biến toàn cục lưu giá trị ban đầu của cboRoomNo
        string roomNoWG = "";

        int id;
        public StudentUpdate()
        {
            InitializeComponent();
        }

        private void StudentUpdate_Load(object sender, EventArgs e)
        {
        }

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

        #region Validate
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
            string input = txtStudentName.Text.Trim(new char[] {' ', '\t', '\n'});
            if(input != "")
            {
                // trc khi search thì xóa các item trong cbo(k xóa sẽ bị duplicate)
                cboRoomNo.Items.Clear();

                //query = $"SELECT * FROM newStudent WHERE name LIKE '%{txtStudentName.Text}%'";
                query = $"SELECT * FROM newStudent WHERE name LIKE '%{input}%'";
                DataSet ds = managementSQLConn.GetData(query);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    lblText1.Text = "Sinh viên không tồn tại";
                    lblText1.Visible = true;
                }
                else
                {
                    lblText1.Visible = false;
                    searchedWG = true;
                    id = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);

                    txtStudentName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtCCCD.Text = ds.Tables[0].Rows[0]["idproof"].ToString();
                    txtDadName.Text = ds.Tables[0].Rows[0]["fname"].ToString();
                    txtMotherName.Text = ds.Tables[0].Rows[0]["mname"].ToString();
                    txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                    txtAddress.Text = ds.Tables[0].Rows[0]["paddress"].ToString();
                    txtSchool.Text = ds.Tables[0].Rows[0]["college"].ToString();
                    txtPhone.Text = ds.Tables[0].Rows[0]["mobile"].ToString();
                    // Gán giá trị của cbo cho roomNoWG
                    roomNoWG = ds.Tables[0].Rows[0]["roomNo"].ToString();
                    cboRoomNo.Items.Add(roomNoWG);
                    cboRoomNo.SelectedItem = roomNoWG;
                    cboStatus.Text = ds.Tables[0].Rows[0]["living"].ToString();
                }

                // Add các phòng còn trống vào cbo để đổi thông tin phòng
                query = "select roomNo from rooms where roomStatus = 1 and Booked = 'No';";
                List<string> roomNoList = managementSQLConn.GetRoomNoList(query);
                foreach (var item in roomNoList)
                {
                    cboRoomNo.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập tên sinh viên","Nhắc nhở",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(searchedWG == false)
            {
                DialogResult result = MessageBox.Show("Bạn chưa chọn sinh viên", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStudentName.Focus();
                return;
            }
            else
            {
                if (!CheckNull())
                {
                    return;
                }
                if (!IsValidate())
                {
                    return;
                }

                // Biến temp để lưu roomNo target
                string temp = "";
                string query = $"UPDATE newStudent " +
                   $"SET mobile = '{txtPhone.Text}', name = '{txtStudentName.Text}', " +
                   $"fname = '{txtDadName.Text}', mname = '{txtMotherName.Text}', " +
                   $"email = '{txtEmail.Text}', paddress = '{txtAddress.Text}', " +
                   $"college = '{txtSchool.Text}', idproof = '{txtCCCD.Text}', " +
                   $"roomNo = {cboRoomNo.Text}, living = '{cboStatus.Text}' " +    // Chuyển roomNo target sang status booked yes(trạng thái k thể cho thuê)                      
                   $"WHERE id = {id} update rooms set Booked = 'Yes' where roomNo = '{cboRoomNo.Text}';";

                temp = cboRoomNo.Text;
                managementSQLConn.SetData(query, "Cập nhật sinh viên thành công");


                // chuyển roomNo hiện tại sang status booked no(trạng thái có thể cho thuê)
                query = $"update rooms set Booked = 'No' where roomNo = '{roomNoWG}'";
                managementSQLConn.SetDataNoMessageBox(query);

                // (*)Xóa hết các item trong cbo
                cboRoomNo.Items.Clear();

                // Select lại toàn bộ các roomNo có status booked no(trạng thái có thể cho thuê)
                query = "select roomNo from rooms where roomStatus = 1 and Booked = 'No';";
                List<string> roomNoList = managementSQLConn.GetRoomNoList(query);
                foreach (var item in roomNoList)
                {
                    cboRoomNo.Items.Add(item);
                }

                // Thêm lại roomNo hiện tại(vì đã bị xóa hết bởi (*)) 
                cboRoomNo.Items.Add(temp);
                cboRoomNo.SelectedItem = temp;
                roomNoWG = temp;
            }

            //searchedWG = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (searchedWG == false)
            {
                MessageBox.Show("Bạn chưa chọn sinh viên", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStudentName.Focus();
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Bạn có muốn xóa sinh viên này không?", "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    string query = $"delete from newStudent where id = '{id}'";
                    managementSQLConn.SetData(query, "Xóa sinh viên thành công");

                    searchedWG = false;
                    ClearStudentInfo();
                }
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            searchedWG = false;
            ClearStudentInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
