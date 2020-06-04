using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Entidades;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Business
{
    public class ConfigBusiness : IConfigBusiness
    {
        private IConfigData _configRepository;

        public ConfigBusiness(IConfigData repository)
        {
            _configRepository = repository;
        }

        public float GetTempoInadimplenciaTimeoutParametro()
        {
            using(MiniProfiler.Current.Step("Obtendo tempo de inadimplencia"))
            {
                return _configRepository.GetTempoInadimplenciaTimeoutParametro();
            }
        }

        public List<CacheConfig> GetCacheConfig()
        {
            return _configRepository.GetCacheConfig();
        }

        public List<OfflineConfig> GetOfflineConfig()
        {
            return _configRepository.GetOfflineConfig();
        }
    }
}