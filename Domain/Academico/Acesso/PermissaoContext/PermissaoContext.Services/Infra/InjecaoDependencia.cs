using PermissaoContext.Domain.Repositories;
using PermissaoContext.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using PermissaoContext.Data.Databases;

namespace PermissaoContext.Services.Infra
{
    public static class InjecaoDependencia
    {
        public static void Start(IServiceCollection services)
        {
            AddPermissaoDependencies(services);
        }

        private static IServiceCollection AddPermissaoDependencies(IServiceCollection services)
        {
            services.AddScoped<PermissoesDB, PermissoesDB>();
            services.AddScoped<DesenvDB, DesenvDB>();
            services.AddScoped<IAcessoConteudoRepository, AcessoConteudoRepository>();
            services.AddScoped<IAcessoAplicacaoRepository, AcessoAplicacaoRepository>();

            return services;
        }

    }
}