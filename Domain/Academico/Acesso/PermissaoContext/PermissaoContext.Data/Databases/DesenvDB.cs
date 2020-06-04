using System;
using System.Data;
using System.Data.SqlClient;

namespace PermissaoContext.Data.Databases
{
    public class DesenvDB : IDisposable
    {
        public SqlConnection Connection { get; set; }

        public DesenvDB()
        {
            Connection = new SqlConnection(@"Data Source=mbd.ordomederi.com;Initial Catalog=homologacaoCtrlPanel2012;Application Name=API;Persist Security Info=True;User ID=MateriaisDireito;Password=200t@bl7sL1m1t1;MultipleActiveResultSets=True");
            Connection.Open();
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }
    }
}