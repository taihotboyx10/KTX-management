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
    public partial class StaffLeave : Form
    {
        KTXManagementSQLConn managementSQLConn = new KTXManagementSQLConn();

        public StaffLeave()
        {
            InitializeComponent();
        }
        
        private void StaffLeave_Load(object sender, EventArgs e)
        {
            string query = "select * from newEmployee where working = 'No'";
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
