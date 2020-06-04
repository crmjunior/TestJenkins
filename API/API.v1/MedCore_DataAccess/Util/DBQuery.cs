using System;
using System.Data;
using MedCore_DataAccess.Model;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MedCore_DataAccess.Util
{
    public class DBQuery
    {
        public System.Data.Common.DbDataReader reader { get; set; }
        public String erro = "";
        public DBQuery()
        { }

        public DBQuery(String Proc)
        {
            using(var ctx = new DesenvContext())
            {
                ctx.Database.OpenConnection();
                var cmd = ctx.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = Proc;
                cmd.CommandTimeout = 600;
                try
                {
                    reader = cmd.ExecuteReader();
                }
                catch (Exception e)
                {
                    erro = e.Message;
                }
            }
        }

        public DataSet ExecuteStoredProcedure(string storedProcedureName, SqlParameter[] parameters = null, bool isAcademicoRDS = false)
        {
            try
            {
                
                var connectionString = "";
                if (isAcademicoRDS)
                    connectionString = ConfigurationProvider.Get("ConnectionStrings:AcademicoRDS");
                else
                    connectionString = ConfigurationProvider.Get("ConnectionStrings:DesenvConnection");

                var ds = new DataSet();

                using (var conn = new SqlConnection(connectionString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = storedProcedureName;
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                            foreach (var parameter in parameters)
                            {
                                if (parameter.Value == null) continue;
                                cmd.Parameters.Add(parameter);
                            }

                        using (var adapter = new SqlDataAdapter(cmd))
                            adapter.Fill(ds);
                    }
                }
                return ds;
            }
            catch
            {
                throw;
            }
        }

        public DataSet ExecuteQuery(string query, bool isAcademicoRDS = false)
        {
            var connectionString = "";
            if(isAcademicoRDS)
                connectionString = ConfigurationProvider.Get("ConnectionStrings:AcademicoRDS");
            else
                connectionString = ConfigurationProvider.Get("ConnectionStrings:DesenvConnection");

            var ds = new DataSet();

            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;

                    using (var adapter = new SqlDataAdapter(cmd))
                        adapter.Fill(ds);
                }
            }

            return ds;
        }

        public bool ExecuteNonQuery(string query)
        {
            try
            {
                var connectionString = ConfigurationProvider.Get("ConnectionStrings:DesenvConnection");
                var ds = new DataSet();

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}