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
                MessageBox.Show("Chọn trạng thái có thể cho thuê trước khi thêm phòng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            query = "select * from rooms where roomNo = " + txtRoomNum2.Text + ";";
            DataSet dataSet = new DataSet();
            dataSet = managementSQLConn.GetData(query);

            if (dataSet.Tables[0].Rows.Count > 0)
            {
                dgvListRoom.DataSource = dataSet.Tables[0];
            }
            else
            {
                lblText2.Text = "Phòng không tồn tại";
                lblText2.Visible = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //if(dgvListRoom.SelectedRows.Count == 0)
            //{
            //    MessageBox.Show("Chon phong ban muon xoa", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            var result = MessageBox.Show("Bạn có muốn xóa phòng này không?","Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(result == DialogResult.OK)
            {
                string roomNoToDelete = dgvListRoom.SelectedRows[0].Cells["roomNo"].Value.ToString();
                string query = "DELETE FROM rooms WHERE roomNo = " + roomNoToDelete;

                managementSQLConn.SetData(query, "Xóa phòng thành công");
                //RoomManagement_Load(this, null);
                dgvListRoom.Rows.RemoveAt(dgvListRoom.SelectedRows[0].Index);
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

        private void btnUpdate_Click(object sender, EventArgs e)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
