using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Business
{
    public interface IConfigBusiness
    {
        float GetTempoInadimplenciaTimeoutParametro();

        List<CacheConfig> GetCacheConfig();

        List<OfflineConfig> GetOfflineConfig();
    }
}