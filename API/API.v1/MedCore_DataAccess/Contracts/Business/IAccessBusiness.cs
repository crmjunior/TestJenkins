using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Business
{
    public interface IAccessBusiness
    {
         List<AccessObject> GetAll(int applicationId, int objectTypeId);

        List<PermissaoRegra> GetAlunoPermissoes(List<AccessObject> lstObj, int idClient, int applicationId);

        string LoginJWT(string register, string senha, int idAplicacao, int exp = 0);

    }
}