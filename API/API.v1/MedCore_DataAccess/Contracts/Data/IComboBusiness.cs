using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IComboBusiness
    {
        List<Combo> GetCombo(int idClient, int applicationId, string versaoApp);
        List<Combo> GetComboAposLancamentoMsPro(int idClient, int applicationId, string versaoApp);
    }
}