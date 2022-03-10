using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace SqlConnectionLibrary
{
    public class SqlConnections
    {
        //SqlConnection con = new SqlConnection("Data Source=DESKTOP-HT03RF5;Initial Catalog=Provincias;Integrated Security=True");
        //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["dataSource"]);
        private string connection = string.Empty;
        private SqlConnection connect;
        private SqlCommand command;
        private SqlDataAdapter da;
        private DataTable dt;
        private DataSet ds;
        private bool success = false;

        public SqlConnections()
        {
            connect = new SqlConnection();

            try
            {
                connection = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
            }
            catch
            {

                connection = ConfigurationManager.AppSettings.Get("connection");
            }
        }

        private SqlConnection OpenConnection()
        {
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.ConnectionString = connection;
                    connect.Open();
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Something goes wrong" + ex.Message);
                }
            }
            return connect;
        }

        private void CloseConnection()
        {
            if (connect.State != ConnectionState.Closed)
            {
                connect.Close();
            }
        }

        public string SelectString(string query)
        {
            string result = string.Empty;

            try
            {
                OpenConnection();
                command = new SqlCommand(query, connect);
                result = command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something goes wrong" + ex.Message);
                result = string.Empty;
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }

        public bool ExecuteCommand(string query)
        {
            try
            {
                OpenConnection();
                command = new SqlCommand(query, connect);
                command.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Console.WriteLine("Something goes wrong" + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return success;
        }

        public bool ExecuteProcedureSql(string procedure)
        {
            try
            {
                OpenConnection();
                command = new SqlCommand(procedure, connect);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Console.WriteLine("Something goes wrong " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return success;
        }

        public DataTable SelectDataTableFromStoreProcedure(string procedure)
        {
            dt = new DataTable();
            da = new SqlDataAdapter();

            try
            {
                OpenConnection();
                command = new SqlCommand(procedure, connect);
                command.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = command;
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something goes wrong " + ex.Message); ;
            }
            finally
            {
                CloseConnection();
            }
            return dt;
        }

        public DataTable SelectDataTableByQuery(string query)
        {
            dt = new DataTable();

            try
            {
                OpenConnection();
                da = new SqlDataAdapter(query, connect);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something goes wrong " + ex.Message);
            }
            finally 
            {
                CloseConnection();
            }
            return dt;
        }

        public DataSet SelectDataSet(string query, string table)
        {
            ds = new DataSet();
            try
            {
                OpenConnection();
                da = new SqlDataAdapter(query, connect);
                da.Fill(ds, table);
            }
            catch
            {
            }
            finally
            {
                CloseConnection();
            }
            return ds;
        }
    }
}
