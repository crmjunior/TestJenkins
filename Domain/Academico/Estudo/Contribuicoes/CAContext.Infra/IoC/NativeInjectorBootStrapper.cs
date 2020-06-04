using CAContext.Domain.Interfaces.Repositories;
using CAContext.Domain.Business;
using Microsoft.Extensions.DependencyInjection;
using CAContext.Domain.Interfaces.Business;
using Shared.Interfaces;
using CAContext.Application.Interfaces;
using CAContext.Application.Services;
using CAContext.Infra.Data.Repository;
using CAContext.Infra.Data.UoW;
using CAContext.Infra.Data.Context;

namespace CAContext.Data.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IContribuicaoRepository, ContribuicaoRepository>();
            services.AddScoped<IContribuicaoArquivoRepository, ContribuicaoArquivoRepository>();
            services.AddScoped<IContribuicaoAppService, ContribuicaoAppService>();
            services.AddScoped<IContribuicaoBusiness, ContribuicaoBusiness>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ContribuicoesContext>();
        }

    }
}