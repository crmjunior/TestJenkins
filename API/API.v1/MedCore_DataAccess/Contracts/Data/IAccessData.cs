using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IAccessData
    {
        List<AccessObject> GetAll(int applicationId, int objectTypeId);

        List<PermissaoRegra> GetRegras(List<AccessObject> menu, int idAplicacao);

        List<PermissaoRegra> GetRegrasNotificacoes(List<AccessObject> notificacaoObjects);

        List<RegraCondicao> GetRegraCondicoes(int? idAplicacao, int regraId = 0);

        List<RegraCondicao> GetCondicoesPreenchidasPeloAluno(int matricula, int idAplicacao);

        PermissaoRegra GetPermissoes(List<RegraCondicao> condicoesPreenchidasPeloAluno, List<PermissaoRegra> permissoes, List<RegraCondicao> condicoesRegras);

        List<Aluno> GetAlunosPorRegra(List<RegraCondicao> listRegraCondicao, Aplicacoes aplicacao);

        List<PermissaoRegra> GetAlunoPermissoes(List<AccessObject> lstObj, int idClient, int applicationId);

    }
}