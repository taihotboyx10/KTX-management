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
    public partial class StaffList : Form
    {
        KTXManagementSQLConn managementSQLConn = new KTXManagementSQLConn();

        public StaffList()
        {
            InitializeComponent();
        }

        private void StaffList_Load(object sender, EventArgs e)
        {
            string query = "select * from newEmployee where working = 'Yes'";
            DataSet dataSet = managementSQLConn.GetData(query);
            dgvStaff.AutoGenerateColumns = false;
            dgvStaff.DataSource = dataSet.Tables[0];
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
