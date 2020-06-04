using Dapper.FluentMap;
using CAContext.Infra.Data.DapperConfig;

namespace CAContext.Infra.Data.Mappings
{
    public class DapperMapping
    {
        public static void RegisterDapperMappings()
        {
            FluentMapper.Initialize(config => { config.AddMap(new ContribuicaoMap()); });
        }
    }
}