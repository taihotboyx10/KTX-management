using QuanLyKTX.Util;
using System;
using System.Collections;
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
    public partial class RoomManagement : Form
    {
        KTXManagementSQLConn managementSQLConn = new KTXManagementSQLConn();
        string query;
        int status;

        //Biến cờ xác nhận đã chọn nhân viên
        bool searchedWG;

        public RoomManagement()
        {
            InitializeComponent();
        }
        private void RoomManagement_Load(object sender, EventArgs e)
        {
            lblText1.Visible = false;
            lblText2.Visible = false;

            //select tất cả table rooms hiển thị lên dgv
            query = "select * from rooms";
            DataSet dataSet = managementSQLConn.GetData(query);

            //Doi so qua text
            ClnStatus.DataSource = Constan.ROOM_STATUS.ToList();
            ClnStatus.DisplayMember = "Value";
            ClnStatus.ValueMember = "Key";
            dgvListRoom.DataSource = dataSet.Tables[0];

        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtRoomNum1.Text))
            {
                MessageBox.Show("Vui lòng nhập số phòng trước khi thêm", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(!rbtAction1.Checked && !rbtNotAction1.Checked)
            {
                MessageBox.Show("Chọn trạng thái trước khi thêm phòng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                query = "select * from rooms where roomNo = " + txtRoomNum1.Text + ";";
                DataSet dataSet = managementSQLConn.GetData(query);

                if (dataSet.Tables[0].Rows.Count == 0)
                {
                    if (rbtAction1.Checked)
                    {
                        status = 1;
                    }
                    if (rbtNotAction1.Checked)
                    {
                        status = 0;
                    }
                    query = "insert into rooms (roomNo, roomStatus) values (" + txtRoomNum1.Text + ",'" + status + "')";

                    managementSQLConn.SetData(query, "Thêm phòng thành công");
                    RoomManagement_Load(this, null);
                }
                else
                {
                    lblText1.Text = "Phòng đã tồn tại";
                    lblText1.Visible = true;
                }
            }
            

        }

        #region"Validate"
        private void txtRoomNum2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem ký tự vừa nhập có phải là số không
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) // 8 là phím Backspace
            {
                e.Handled = true; // Chặn ký tự không phải số
            }
        }

        private void txtRoomNum1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra xem ký tự vừa nhập có phải là số không
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) // 8 là phím Backspace
            {
                e.Handled = true; // Chặn ký tự không phải số
            }
        }
        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRoomNum2.Text))
            {
                MessageBox.Show("Vui lòng nhập số phòng trước khi tìm kiếm", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoomNum2.Focus();
            }
            else
            {
                query = "select * from rooms where roomNo = " + txtRoomNum2.Text + ";";
                DataSet dataSet = managementSQLConn.GetData(query);

                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    searchedWG = true;
                    lblText2.Visible = false;

                    if (Convert.ToInt32(dataSet.Tables[0].Rows[0]["roomStatus"]) == 1)
                    {
                        rbtAction2.Checked = true;
                    }
                    else
                    {
                        rbtNotAction2.Checked = true;
                    }

                    dgvListRoom.DataSource = dataSet.Tables[0];
                }
                else
                {
                    lblText2.Text = "Phòng không tồn tại";
                    lblText2.Visible = true;
                }
            }
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(searchedWG == true)
            {
                if (rbtAction2.Checked)
                {
                    status = 1;
                }
                if (rbtNotAction2.Checked)
                {
                    status = 0;
                }
                query = "update rooms set roomStatus = '" + status + "' where roomNo = " + txtRoomNum2.Text + ";";
                managementSQLConn.SetData(query, "Cập nhật thành công");
                RoomManagement_Load(this, null);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phòng trước khi cập nhật", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoomNum2.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //if(dgvListRoom.SelectedRows.Count == 0)
            //{
            //    MessageBox.Show("Chon phong ban muon xoa", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            if(searchedWG == true)
            {
                var result = MessageBox.Show("Bạn có muốn xóa phòng này không?", "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    //Trước khi xóa kiểm tra xem phòng này đang có sinh viên ở hay không
                    List<string> roomExist = managementSQLConn.GetRoomNoList("select roomNo from newStudent;");
                    if (roomExist.Contains(txtRoomNum2.Text))
                    {
                        MessageBox.Show("Phòng có sinh viên đang ở nên không thể xóa", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        //string roomNoToDelete = dgvListRoom.SelectedRows[0].Cells["roomNo"].Value.ToString();
                        query = "DELETE FROM rooms WHERE roomNo = " + txtRoomNum2.Text;

                        managementSQLConn.SetData(query, "Xóa phòng thành công");
                        //RoomManagement_Load(this, null);
                        dgvListRoom.Rows.RemoveAt(dgvListRoom.SelectedRows[0].Index);

                        searchedWG = false;
                        txtRoomNum2.Text = "";
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phòng trước khi xóa", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoomNum2.Focus();
            }

        }

        private void dgvListRoom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string roomNo = dgvListRoom.SelectedRows[0].Cells[0].Value.ToString();
            txtRoomNum2.Text = roomNo;

            string status = dgvListRoom.SelectedRows[0].Cells[1].Value.ToString();
            if (status == "1")
            {
                rbtAction2.Checked = true;
            }
            else
            {
                rbtNotAction2.Checked = true;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
