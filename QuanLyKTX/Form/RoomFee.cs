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
    public partial class RoomFee : Form
    {
        KTXManagementSQLConn managementSQLConn = new KTXManagementSQLConn();
        string query;
        //Biến cờ đánh dấu là đã chọn sinh viên trước khi thanh toán
        bool searchedWG = false;
        public RoomFee()
        {
            InitializeComponent();
        }

        private void RoomFee_Load(object sender, EventArgs e)
        {

        }

        private void txtMoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem ký tự vừa nhập có phải là số không
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) // 8 là phím Backspace
            {
                e.Handled = true; // Chặn ký tự không phải số
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string input = txtStudentName.Text.Trim(new char[] { ' ', '\t', '\n' });
            if (input != "")
            {

                query = $"SELECT name,email,mobile,roomNo FROM newStudent WHERE name LIKE '%{input}%'";
                DataSet dsStd = managementSQLConn.GetData(query);
                if (dsStd.Tables[0].Rows.Count == 0)
                {
                    lblText1.Text = "Sinh viên không tồn tại";
                    lblText1.Visible = true;
                }
                else
                {
                    lblText1.Visible = false;

                    txtStudentName.Text = dsStd.Tables[0].Rows[0]["name"].ToString();
                    txtEmail.Text = dsStd.Tables[0].Rows[0]["email"].ToString();
                    txtPhone.Text = dsStd.Tables[0].Rows[0]["mobile"].ToString();
                    txtRoomNo.Text = dsStd.Tables[0].Rows[0]["roomNo"].ToString();

                    //Khi chọn sv xong thì gán biến cờ về true
                    searchedWG = true;
                }

                //Nếu sv đã thanh toán thì thông tin thanh toán sẽ được hiển thị tại dgv
                 //Tạo lớp giả DTO để set lại header cho dgv
                List<RoomFeeDTO> roomFeeDTOs = new List<RoomFeeDTO>();
                dgvFee.DataSource = roomFeeDTOs;
                query = $"select * from fees where stdName = '{txtStudentName.Text}'";
                DataSet dsFee = managementSQLConn.GetData(query);
                if (dsFee.Tables[0].Rows.Count > 0)
                {
                    dgvFee.DataSource = dsFee.Tables[0];
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập tên sinh viên", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            query = "select stdName from fees";
            List<string> sdtNames = managementSQLConn.GetStdNameInFees(query);

            //nếu searchedWG == false tức là chưa chọn sv
            if (searchedWG == false)
            {
                MessageBox.Show("Bạn chưa chọn sinh viên", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning );
            }
            else if (sdtNames.Contains(txtStudentName.Text))
            {
                MessageBox.Show("Sinh viên này đã thanh toán phí phòng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string inputFee = txtMoney.Text.Trim(new char[] { ' ', '\t', '\n' });
                if (inputFee != "")
                {
                    //Thêm sv đó vào danh sách sv đã thanh toán phí phòng
                    string formattedDate = dtpFee.Value.ToString("MM/yyyy");
                    query = $"INSERT INTO fees (stdName, fmonth, amount) VALUES ('{txtStudentName.Text}', '{formattedDate}', {txtMoney.Text})";
                    managementSQLConn.SetData(query, "Sinh viên thanh toán phí phòng thành công");
                    //trả lại giá trị của searchedWG về false để tiếp tục chọn sv
                    searchedWG = false;

                    ClearData();

                }
                else
                {
                    MessageBox.Show("Bạn chưa nhập phí phòng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            
        }

        private void ClearData()
        {
            txtStudentName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtRoomNo.Text = "";
            txtMoney.Text = "";
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
            searchedWG = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
