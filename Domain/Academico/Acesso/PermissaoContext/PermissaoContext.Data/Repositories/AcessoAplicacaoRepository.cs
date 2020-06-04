using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PermissaoContext.Data.Databases;
using PermissaoContext.Domain.Repositories;
using PermissaoContext.Domain.ValueObjects;
using Dapper;
using System.Linq;
using System.Data;
using System.Data.SqlClient;


namespace PermissaoContext.Data.Repositories
{
    public class AcessoAplicacaoRepository : IAcessoAplicacaoRepository
    {
        //private IConfiguration _configuration;



        private PermissoesDB _permissoesDB { get; set; }
        private static DesenvDB _desenvDB { get; set; }

        private static IAcessoAplicacaoRepository _acessoAlunoRepository { get; set; }

        public AcessoAplicacaoRepository(PermissoesDB permissoesDB, DesenvDB desenvDB)
        {
            _permissoesDB = permissoesDB;
            _desenvDB = desenvDB;


        }
        // public AcessoAlunoDados(IConfiguration config)
        // {
        //     _configuration = config;
        // }

        public T ObterItem<T>(string codigo)
        {

            var filter = Builders<T>.Filter.Eq("Codigo", codigo);

            return _permissoesDB.Db.GetCollection<T>("Catalogo")
                .Find(filter).FirstOrDefault();
        }

        public async Task<List<Material>> GetMateriaisPermitidos(int matricula)
        {

            return await GetMateriaisPermitidosFromPermissoes(matricula);
        }

        public bool IsBlackList(int matricula)
        {
            throw new System.NotImplementedException();
        }

        public List<Material> GetMateriaisPermitidosFromDesenv(int matricula)
        {
            throw new System.NotImplementedException();

        }


        public List<MaterialDireito> GetMateriaisDireito(int matricula)
        {
            var retorno = _desenvDB
                .Connection
                .Query<MaterialDireito>(
                    @"select 
                        intMaterialID MaterialId,
                        replace(txtDate, '.','-') DataLiberacao, 
                        year(replace(txtDate, '.','-')) Ano
                        from dbo.medfn_EMED_MateriaisDeDireito_consultadata(@matricula, getdate());",
                    new { matricula = matricula }
                    )
                .ToList();
            return retorno;
        }
        public async Task<List<Material>> GetMateriaisPermitidosFromPermissoes(int matricula)
        {
            var filter = Builders<List<Material>>.Filter.Eq("matricula", matricula);

            var databases = _permissoesDB.Cliente.ListDatabaseNames().ToList();

            var material = await _permissoesDB.Db.GetCollection<List<Material>>("Material".ToLower())
                .Find(filter).Project<List<Material>>("{permissoes: 1}").FirstOrDefaultAsync();

            return material;
        }

        public List<Material> GetMateriaisPermitidosCache(int matricula)
        {
            throw new System.NotImplementedException();

        }
    }

}