using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Repository;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedCore_DataAccess.Business
{
    public class ContribuicaoBusiness : IContribuicaoBusiness
    {
        private readonly IContribuicaoData _rep;
        private readonly IContribuicaoArquivoData _repArquivo;

        public ContribuicaoBusiness(IContribuicaoData rep, IContribuicaoArquivoData repArquivo)
        {
            _rep = rep;
            _repArquivo = repArquivo;
        }

        public IList<ContribuicaoDTO> GetContribuicoes(ContribuicaoFiltroDTO e)
        {
            var contribuicoes = _rep.GetContribuicoes(e);
            var dataAtual = DateTime.Now;
            using(MiniProfiler.Current.Step("Setando tempo de publicação da CA e Buscando arquivos de cada uma"))
            {
                foreach (var cont in contribuicoes)
                {
                    cont.Data = Utilidades.GetTempoDecorrido(cont.DataCriacao);
                    cont.Arquivos = _repArquivo.ListarArquivosContribuicao(cont.ContribuicaoId);
                }
            }
            return contribuicoes;
        }

        public ContribuicaoDTO GetContribuicao(int id)
        {
            using(MiniProfiler.Current.Step("[SELECT] - Obtendo o estado do aluno"))
            {
                return _rep.GetContribuicao(id);
            }
        }

        public int AprovarContribuicao(Contribuicao e)
        {
            using(MiniProfiler.Current.Step("[SELECT] - Obtendo o estado do aluno"))
            {
                return _rep.AprovarContribuicao(e);
            }
            
        }

        public int InserirContribuicao(Contribuicao e)
        {
            if (e.ContribuicaoId == 0)
            {
                using(MiniProfiler.Current.Step("[SELECT] - Obtendo o estado do aluno"))
                {
                    e.Estado = Utilidades.GetEstadoCursoAluno(e.ClientId);
                }
                
                using(MiniProfiler.Current.Step("[INSERT] - Inserindo contribuição no BD"))
                {
                    e.ContribuicaoId = _rep.InserirContribuicao(e);
                }
                
                using(MiniProfiler.Current.Step("[INSERT] - Inserindo arquivos da CA no BD"))
                {
                    foreach (var item in e.Arquivos)
                    {
                        item.ContribuicaoID = e.ContribuicaoId;
                        var result = _repArquivo.InserirContribuicaoArquivo(item);
                        if (result == 0)
                        {
                            return result;
                        }
                    }
                }

                return e.ContribuicaoId;
            }
            else
            {
                var ret = 0;
                using(MiniProfiler.Current.Step("[UPDATE] - Atualizando CA"))
                {
                    ret = _rep.UpdateContribuicao(e);
                }
                
                using(MiniProfiler.Current.Step("[INSERT] Inserindo ou atualizando arquivos da CA no BD"))
                {
                    foreach (var item in e.Arquivos)
                    {
                        if (item.Id == 0)
                        {
                            item.ContribuicaoID = e.ContribuicaoId;
                            _repArquivo.InserirContribuicaoArquivo(item);
                        }
                        else
                        {
                            _repArquivo.UpdateContribuicaoArquivo(item);
                        }
                    }
                }

                return ret;
            }
        }

        public int DeletarContribuicao(int id)
        {
            var contribuicao = new ContribuicaoDTO();
            using(MiniProfiler.Current.Step("[SELECT] - Obtendo CA"))
            {
                contribuicao = GetContribuicao(id);
            }
            
            if (contribuicao != null && contribuicao.ContribuicaoId > 0)
            {

                if (contribuicao.CodigoMarcacao != null)
                {
                    using(MiniProfiler.Current.Step("[INSERT] - Manipulando e gerando nova versão de apostila"))
                    {
                         var _materialApostila = new MaterialApostilaEntity();
                        var materialApostilaAlunoManager = new MaterialApostilaAlunoManager();
                        var materialApostilaAluno = _materialApostila.GetMaterialApostilaAluno(contribuicao.ClientId, contribuicao.ApostilaId.Value);
                        var apostilaVersao = Utilidades.GetDetalhesApostila(materialApostilaAluno);
                        string chave = Utilidades.CriarNomeApostila(contribuicao.ClientId, contribuicao.ApostilaId.Value, apostilaVersao.Versao);
                        var conteudo = materialApostilaAlunoManager.ObterArquivo(chave);

                        conteudo = Utilidades.RemoveMarcacaoApostila(Constants.COMP_CONTRIBUICAO_APOSTILA, conteudo, contribuicao.CodigoMarcacao);
                        _materialApostila.PostModificacaoApostila(contribuicao.ClientId, contribuicao.ApostilaId.Value, conteudo);                   
                    }
                }

                using(MiniProfiler.Current.Step("[SELECT] - Listando arquivos da CA"))
                {
                    contribuicao.Arquivos = _repArquivo.ListarArquivosContribuicao(contribuicao.ContribuicaoId);
                }
                
                using(MiniProfiler.Current.Step("[DELETE] - Deletando arquivos da CA"))
                {
                    if (contribuicao.Arquivos.Count > 0)
                    {
                        var result = _repArquivo.DeletarContribuicaoArquivo(contribuicao.Arquivos.Select(s => s.Id).ToList());
                        if (result == 0)
                        {
                            return result;
                        }
                    }
                }

                using(MiniProfiler.Current.Step("[DELETE] - Deletando CA"))
                {
                    return _rep.DeletarContribuicao(id);
                }
                
            }
            return 0;
        }

        public int ArquivarContribuicao(Contribuicao e)
        {
            var contribuicaoArquivada = true;

            using(MiniProfiler.Current.Step("[SELECT] - Verificando se a CA já é arquivada"))
            {
                contribuicaoArquivada = _rep.HasContribuicaoArquivada(e);
            }

            using(MiniProfiler.Current.Step("[UPDATE] - Atualizando status de arquivada"))
            {
                if (!contribuicaoArquivada)
                {
                    return _rep.ArquivarContribuicao(e);
                }
                else
                {
                    return _rep.DeleteContribuicaoArquivada(e.ContribuicaoId);
                }
            }

        }

        public int EncaminharContribuicao(Contribuicao e)
        {
            using(MiniProfiler.Current.Step("[INSERT] - Encaminhar CA"))
            {
                return _rep.EncaminharContribuicao(e);
            }
            
        }

        public int InsertInteracao(ContribuicaoInteracao e)
        {
            using(MiniProfiler.Current.Step("[SELECT] - Obter interação"))
            {
                e = GetInteracao(e);
            }
            
            
            if (e.ContribuicaoInteracaoId > 0)
            {
                using(MiniProfiler.Current.Step("[DELETE] - Deleta interação")) 
                {
                    return DeleteContribuicaoInteracao(e.ContribuicaoInteracaoId);
                }
                
            }
            using(MiniProfiler.Current.Step("[INSERT] - Insere interação")) 
            {
                return _rep.InsertInteracao(e); 
            }
            
        }

        public ContribuicaoInteracao GetInteracao(ContribuicaoInteracao e)
        {
            using(MiniProfiler.Current.Step("[SELECT] - Obtendo a interação")) 
            {
                return _rep.GetInteracao(e);
            }
        }

        public int DeleteContribuicaoInteracao(int id)
        {
            using(MiniProfiler.Current.Step("[DELETE] - Deletar a interação")) 
            {
                return _rep.DeleteContribuicaoInteracao(id);
            }
            
        }

        public ContribuicaoBucketDTO GetContribuicaoBucket()
        {
            return ContribuicaoBucketManager.GetConfig();
        }        
    }
}