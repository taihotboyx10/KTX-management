using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.Common;
using System.Windows.Forms;
using System.Collections;

namespace QuanLyKTX
{
    class KTXManagementSQLConn
    {
        private SqlConnection setConnection()
        {
            string connectionString = "Server=localhost,1433;Database=hostel;UID=sa;PWD=Password123";
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }
        
        public DataSet GetData(string query)
        {
            SqlConnection conn = setConnection();
            conn.Open();
            var sqlCommand = new SqlCommand(query,conn);

            DataSet ds = new DataSet();
            //SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = sqlCommand; 
            dataAdapter.Fill(ds);

            conn.Close();
            return ds;

        }

        public List<string> GetRoomNoList(string query)
        {
            List<string> roomNos = new List<string>();
            SqlConnection conn = setConnection();
            conn.Open();

            var sqlCommand = new SqlCommand(query, conn);
            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())//Doc cho den het bang
            {
                string intValue = reader[0].ToString(); // Lấy giá trị số nguyên từ cột 0
                roomNos.Add(intValue);
            }

            conn.Close();

            return roomNos;
        }

        public List<string> GetStdNameInFees(string query)
        {
            List<string> sdtnames = new List<string>();
            SqlConnection conn = setConnection();
            conn.Open();

            var sqlCommand = new SqlCommand(query, conn);
            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())//Doc cho den het bang
            {
                string intValue = reader[0].ToString(); // Lấy giá trị số nguyên từ cột 0
                sdtnames.Add(intValue);
            }

            conn.Close();

            return sdtnames;
        }

        #region SetData
        public void SetData(string query, string msg)
        {
            SqlConnection conn = setConnection();
            conn.Open();
 
            SqlCommand cmd = new SqlCommand(query,conn);
            
            cmd.ExecuteNonQuery();// tra ve so luong dong bi anh huong
            
            conn.Close();

            MessageBox.Show(msg,"Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);

        }

        public void SetDataNoMessageBox(string query)
        {
            SqlConnection conn = setConnection();
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.ExecuteNonQuery();// tra ve so luong dong bi anh huong

            conn.Close();

        }
        #endregion
    }
}
