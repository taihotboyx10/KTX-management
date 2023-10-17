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
    public partial class RoomStatus : Form
    {
        KTXManagementSQLConn managementSQLConn = new KTXManagementSQLConn();
        string query;
        public RoomStatus()
        {
            InitializeComponent();
        }

        private void RoomStatus_Load(object sender, EventArgs e)
        {

            query = "select * from newStudent where living = 'Yes'";
            DataSet dataSet = managementSQLConn.GetData(query);
            dgvStudent.AutoGenerateColumns = false;
            dgvStudent.DataSource = dataSet.Tables[0];
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
