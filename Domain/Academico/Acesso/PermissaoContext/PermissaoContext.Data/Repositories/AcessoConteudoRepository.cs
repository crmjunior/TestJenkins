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
    public class AcessoConteudoRepository : IAcessoConteudoRepository
    {
        //private IConfiguration _configuration;

        private PermissoesDB _permissoesDB { get; set; }
        private static DesenvDB _desenvDB { get; set; }

        private static IAcessoAplicacaoRepository _acessoAlunoRepository { get; set; }

        public AcessoConteudoRepository(PermissoesDB permissoesDB, DesenvDB desenvDB)
        {
            _permissoesDB = permissoesDB;
            _desenvDB = desenvDB;


        }
        // public AcessoAlunoDados(IConfiguration config)
        // {
        //     _configuration = config;
        // }

        public async Task<List<Material>> GetApostilasPermitidas(int matricula)
        {

            return await GetMateriaisPermitidosFromPermissoes(matricula);
        }


        public List<Material> GetMateriaisPermitidosFromDesenv(int matricula)
        {
            throw new System.NotImplementedException();

        }


        public async Task<IList<MaterialDireito>> GetMateriaisDireito(int matricula)
        {
            var retorno = await _desenvDB
                .Connection
                .QueryAsync<MaterialDireito>(
                    @"select 
                        intMaterialID MaterialId,
                        replace(txtDate, '.','-') DataLiberacao, 
                        year(replace(txtDate, '.','-')) Ano
                        from dbo.medfn_EMED_MateriaisDeDireito_consultadata(@matricula, getdate());",
                    new { matricula = matricula }
                    );

            return retorno.ToList();
        }
        public async Task<List<Material>> GetMateriaisPermitidosFromPermissoes(int matricula)
        {
            var filter = Builders<List<Material>>.Filter.Eq("matricula", matricula);

            //var databases = _permissoesDB.Cliente.ListDatabaseNames().ToList();

            var material = await _permissoesDB.Db.GetCollection<List<Material>>("Material".ToLower())
                .Find(filter).Project<List<Material>>("{permissoes: 1}").FirstOrDefaultAsync();

            return material;
        }

        public List<Material> GetMateriaisPermitidosCache(int matricula)
        {
            //if se já passou 1h
            //--gravo o cache atual em variável
            //--async que  vai renovar os materiais do aluno
            //return do cache atual
            throw new System.NotImplementedException();

        }
    }

}