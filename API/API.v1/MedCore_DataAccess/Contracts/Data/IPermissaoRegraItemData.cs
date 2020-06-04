using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IPermissaoRegraItemData
    {
        AccessObject GetAcessoObjetoAtivoComDatasDisponibilidadeAtivas(int idObjeto);
    }
}