using MongoDB.Driver;

namespace PermissaoContext.Data.Databases
{
    public class PermissoesDB
    {

        private const string CONN = "mongodb://localhost:27017";
        private const string DB = "permissoes";
        public IMongoClient Cliente { get; private set; }
        public IMongoDatabase Db { get; private set; }

        public PermissoesDB()
        {
            Cliente = new MongoClient(CONN);
            Db = Cliente.GetDatabase(DB);
        }
    }
}