using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IAlunoEntity
    {
        Aluno GetAlunosDevice(int matricula, Aplicacoes aplicacao);
         int? GetAnoProva(int idProva);
         Generica GetEspecialidadeAluno(int matricula);

         int? GetConcursoIDByProvaId(int provaId);
         int GetTotalQuestoesByProvaId(int provaId);
        List<AlunoEspecialidadeDTO> GetEspecialiddesByConcursoIdAnoProva(int? concursoID, int? anoProva);
        double? GetNotadeCorteAnoPosteriorByNmConcursoAnoProvaEspecialideID(string txtDescription, int? anoProva, int? intEspecialidadeID);

        bool IsExAlunoTodosProdutos(int matricula);

        int SetAutorizacaoTrocaDispositivo(SegurancaDevice device);
        string GetMensagensLogin(int idAplicacao, int idTipoMensagem);

        PermissaoDevice GetPermissaoAcesso(int idAplicacao, int matricula, string token, Utilidades.TipoDevice idDevice);

        bool IsAlunoPendentePagamento(int matricula);
        List<Produto.Produtos> GetProdutosPermitidosLogin(int idAplicacao);
        PermissaoInadimplencia GetPermissao(string registro, int idAplicacao, bool geraChamado = true,
            int ClientId = 0);
        PermissaoInadimplencia RemoveOvInadimplenteBloqueado(PermissaoInadimplencia permissoes);
        string GetAlunoEstado(int matricula);
        int SetChamadoInadimplencia(PermissaoInadimplencia aceiteTermo);
        int SetDeviceToken(DeviceToken deviceToken);
        bool IsExAlunoTodosProdutosCache(int matricula);
    }
}