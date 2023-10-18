using QuanLyKTX.DTO;
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
    public partial class StaffSalary : Form
    {
        KTXManagementSQLConn managementSQLConn = new KTXManagementSQLConn();
        string query;

        //Biến cờ xác nhận đã chọn nhân viên
        bool searchedWG;

        public StaffSalary()
        {
            InitializeComponent();
        }

        private void StaffSalary_Load(object sender, EventArgs e)
        {

        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string input = txtStaffName.Text.Trim();
            if(string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Bạn chưa nhập tên nhân viên", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                query = $"select ename, eemail, emobile, edesignation from newEmployee where ename like '%{input}%'";
                DataSet dsStaff = managementSQLConn.GetData(query);
                if (dsStaff.Tables[0].Rows.Count > 0)
                {
                    lblText1.Visible = false;

                    txtStaffName.Text = dsStaff.Tables[0].Rows[0]["ename"].ToString();
                    txtEmail.Text = dsStaff.Tables[0].Rows[0]["eemail"].ToString();
                    txtPhone.Text = dsStaff.Tables[0].Rows[0]["emobile"].ToString();
                    txtPosition.Text = dsStaff.Tables[0].Rows[0]["edesignation"].ToString();

                    //Khi chọn nv xong thì gán biến cờ về true
                    searchedWG = true;

                    //Nếu nhân viên đã thanh toán thì hiển thị thông tin tại dgv
                    //Tạo lớp giả DTO để set lại header cho dgv
                    StaffSalaryDTO staffSalaryDTO = new StaffSalaryDTO();
                    dgvSalary.DataSource = staffSalaryDTO;

                    query = $"select * from employeeSalary where staffName = '{txtStaffName.Text}'";
                    DataSet dsSalary = managementSQLConn.GetData(query);
                    if (dsSalary.Tables[0].Rows.Count > 0)
                    {
                        dgvSalary.DataSource = dsSalary.Tables[0];
                    }
                }
                else
                {
                    lblText1.Visible = true;
                    lblText1.Text = "Nhân viên không tồn tại";
                }
            }
        }

        private void btnSalary_Click(object sender, EventArgs e)
        {
            query = "select staffdName from employeeSalary";
            List<string> staffNames = managementSQLConn.GetNameList(query);

            if(searchedWG == false)
            {
                MessageBox.Show("Bạn chưa chọn nhân viên", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(staffNames.Contains(txtStaffName.Text))
            {
                MessageBox.Show("Đã trả lương cho nhân viên này", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string inputSalary = txtMoney.Text.Trim();
                if(inputSalary == "")
                {
                    MessageBox.Show("Bạn chưa nhập lương", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string formattedDate = dtpSalary.Value.ToString("MM/yyyy");
                    query = $"insert into employeeSalary (staffName, fmonth, amount) values ('{txtStaffName.Text}', '{formattedDate}', '{txtMoney.Text}')";
                    managementSQLConn.SetData(query, "Trả lương nhân viên thành công");

                    //trả lại giá trị của searchedWG về false để tiếp tục chọn sv
                    searchedWG = false;

                    ClearInfo();
                }
            }

        }

        // Chi cho phép nhập ký tự số
        private void txtMoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void ClearInfo() 
        {
            txtStaffName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtPosition.Text = "";
            txtMoney.Text = "";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInfo();
            searchedWG = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
