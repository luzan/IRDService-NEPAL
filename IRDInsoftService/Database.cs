using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRDInsoftService
{
    class Database
    {
        protected SqlCommand cmd;
        protected SqlConnection con;
        protected SqlDataReader reader;

        public Database()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["IRD_Base"].ConnectionString;

            cmd = new SqlCommand();
            cmd.Connection = con;

            reader = null;
        }

        public Database(String key)
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString.ToString();
            cmd = new SqlCommand();
            cmd.Connection = con;

            reader = null;
        }

        public void Connect()
        {
            if (con.State == ConnectionState.Closed)
            {
                try
                {
                    con.Open();
                }
                catch (Exception se)
                {
                    Console.WriteLine(se.ToString());
                }
            }
        }

        public void Disconnect()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
        }

        public int InsertUpdateDelete(String query)
        {
            int res = -1;
            cmd.CommandText = query;
            Connect();
            try
            {
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                res = -1;
            }
            return res;
        }
    }
}
