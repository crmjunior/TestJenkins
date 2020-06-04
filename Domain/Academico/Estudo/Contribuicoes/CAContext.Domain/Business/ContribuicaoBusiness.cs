using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAContext.Domain.Entities;
using CAContext.Domain.Interfaces.Repositories;
using CAContext.Domain.Interfaces.Business;
using CAContext.Domain.ValueObjects;

namespace CAContext.Domain.Business
{
    public class ContribuicaoBusiness : IContribuicaoBusiness
    {
        private readonly IContribuicaoRepository _contribuicaoRep;
        private readonly IContribuicaoArquivoRepository _contribuicaoArquivosRep;

        public ContribuicaoBusiness(
            IContribuicaoRepository contribuicaoRep,
            IContribuicaoArquivoRepository contribuicaoArquivoRep)
        {
            _contribuicaoRep = contribuicaoRep;
            _contribuicaoArquivosRep = contribuicaoArquivoRep;
        }

        public void Dispose()
        {
            _contribuicaoRep.Dispose();
            _contribuicaoArquivosRep.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task DeletarContribuicao(int id)
        {
            var contribuicao = await GetContribuicao(id);


            if(contribuicao.Arquivos != null) 
            {
                foreach(var arquivo in contribuicao.Arquivos)
                {
                    await _contribuicaoArquivosRep.Deletar(arquivo.Id);
                }
            }

            await _contribuicaoRep.Deletar(id);
        }

        public async Task<Contribuicao> GetContribuicao(int id)
        {
            return await _contribuicaoRep.ObterPorId(id);
        }

        public async Task<IEnumerable<Contribuicao>> GetContribuicoes(Filtro e)
        {
            return await _contribuicaoRep.Listar(x => x.Ativa);
        }

        public async Task<Contribuicao> InserirContribuicao(Contribuicao e)
        {
            if(!e.Valido(_contribuicaoRep))
                return e;

            await _contribuicaoRep.Adicionar(e);
            
            foreach(var arquivo in e.Arquivos)
            {
                arquivo.ContribuicaoId = e.Id;
                await _contribuicaoArquivosRep.Adicionar(arquivo);
            }

            return e;
        }
    }
}