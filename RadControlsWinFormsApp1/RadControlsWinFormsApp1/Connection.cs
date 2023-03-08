using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ALCHANYSCHOOL.Properties;
namespace ALCHANYSCHOOL
{
    class Connection
    {
        static string ConStr = "";
        public SqlConnection  Con = null;

        public Connection(String ServerName, String DatabaseName, String UserName, String Password)
        {        
            try
            {
                if (DatabaseName.Equals(""))
                {
                    ConStr = "Data Source=" + ServerName + ";User ID=" + UserName + ";Password=" + Password;
                }
                else
                {
                    ConStr = "Data Source=" + ServerName + ";Initial Catalog=" + DatabaseName + ";User ID=" + UserName + ";Password=" + Password + ";MultipleActiveResultSets=true";
                }
                
                if (!ServerName.Equals(""))
                    Con = new SqlConnection(ConStr);
                Con.Open();
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
        }
        public Connection()
        {
            if (ConStr == null)
            {
                throw new Exception("Aucune connexion ouverte !");
            }
            else
            {
                try
                {
                    Con = new SqlConnection(ConStr);
                    Con.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public SqlCommand GetCommand(string query)
        {
            if (Con.State == ConnectionState.Closed)
                Con.Open();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(query, Con);
                return cmd;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public SqlDataReader GetReader(string query)
        {
            SqlCommand cmd = null;
            try
            {
                if (Con.State == ConnectionState.Closed)
                    Con.Open();
                cmd = GetCommand(query);
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Boolean  IsRecord(string query)
        {
            SqlCommand cmd = null;
            try
            {
                if (Con.State == ConnectionState.Closed)
                    Con.Open();
                cmd = GetCommand(query);
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {                  
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }
        public DataSet GetDataSet(string query, string NomTable)
        {
            DataSet ds = null;
            try
            {
                if (Con.State == ConnectionState.Closed)
                    Con.Open();
                ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(GetCommand(query));
                da.Fill(ds, NomTable);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }
        public void Execute(string query)
        {
            try
            {
                if (Con.State == ConnectionState.Closed)
                    Con.Open();
                GetCommand(query).ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }
        public DataTable GetDataTable(DataSet ds, string NomTable)
        {
            try
            {
                if (ds != null)
                {
                    return ds.Tables[NomTable];
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }
    }
}
