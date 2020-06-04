using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PermissaoContext.Domain.Entities;
using PermissaoContext.Domain.Repositories;
using PermissaoContext.Services.Infra;

namespace PermissaoContext.Tests
{
    [TestClass]
    public class AcessoAlunoTests
    {
        IAcessoAplicacaoRepository acessoAlunoRepository;
        static IServiceCollection services;
        static IServiceProvider provider;
        [AssemblyInitialize]
        public static void StartUp(TestContext testContext)
        {
            services = new ServiceCollection();
            InjecaoDependencia.Start(services);
            provider = services.BuildServiceProvider();

        }



        [TestMethod]
        public void Deve_ObterMateriaisPermitidos_RetornarListaComItens()
        {
            var matricula = 96409;
            acessoAlunoRepository = provider.GetRequiredService<IAcessoAplicacaoRepository>();

            var materiaisDireito = acessoAlunoRepository.GetMateriaisDireito(matricula);

            var acessoAluno = new AcessoAplicacao(119181, acessoAlunoRepository);

            Assert.IsNotNull(materiaisDireito);
            Assert.AreNotEqual(0, materiaisDireito.Count);

        }



    }
}