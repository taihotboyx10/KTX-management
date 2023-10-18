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
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();

        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            
        }

        private void btnRoomManagement_Click(object sender, EventArgs e)
        {
            RoomManagement roomManagement = new RoomManagement();
            roomManagement.ShowDialog();
        }

        private void btnStudentAdd_Click(object sender, EventArgs e)
        {
            StudentInfo studentInfo = new StudentInfo();
            studentInfo.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void btnStdUpdateDelete_Click(object sender, EventArgs e)
        {
            StudentUpdate studentUpdate = new StudentUpdate();
            studentUpdate.ShowDialog();
        }

        private void btnRoomFee_Click(object sender, EventArgs e)
        {
            RoomFee roomFee = new RoomFee();
            roomFee.ShowDialog();
        }

        private void btnRoomState_Click(object sender, EventArgs e)
        {
            RoomStatus roomStatus = new RoomStatus();
            roomStatus.ShowDialog();
        }

        private void btnRoomLeaving_Click(object sender, EventArgs e)
        {
            StudentLeave roomLeave = new StudentLeave();
            roomLeave.ShowDialog();
        }

        private void btnAddStaff_Click(object sender, EventArgs e)
        {
            StaffInfo staffInfo = new StaffInfo();
            staffInfo.ShowDialog();
        }

        private void btnStaffUpdateDelete_Click(object sender, EventArgs e)
        {
            StaffUpdate staffUpdate = new StaffUpdate();
            staffUpdate.ShowDialog();
        }

        private void btnStaffSalary_Click(object sender, EventArgs e)
        {
            StaffSalary staffSalary = new StaffSalary();
            staffSalary.ShowDialog();
        }

        private void BtnStaffList_Click(object sender, EventArgs e)
        {
            StaffList staffList = new StaffList();
            staffList.ShowDialog();
        }

        private void btnStaffLeavingWork_Click(object sender, EventArgs e)
        {
            StaffLeave staffLeave = new StaffLeave();
            staffLeave.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát chương trình không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                this.Close();
                Login login = new Login();
                login.Show();
            }
        }
    }
}
