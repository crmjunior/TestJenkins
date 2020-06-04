using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IConfigData
    {
        List<CacheConfig> GetCacheConfig();

        float GetTempoInadimplenciaTimeoutParametro();

        List<OfflineConfig> GetOfflineConfig();

        bool GetDeveBloquearAppVersaoNula();

        bool GetDeveBloquearAppVersaoNulaCache();
    }
}