using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Contracts.Repository;
using Microsoft.EntityFrameworkCore;
using MedCore_DataAccess.Model;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Repository
{
    public class ContribuicaoEntity : IContribuicaoData
    {
        private const int PageSize = 50;

        public IList<ContribuicaoDTO> GetContribuicoes(ContribuicaoFiltroDTO filtro)
        {
            using(MiniProfiler.Current.Step("GET - Contribuições paginadas"))
            {
                using (var ctx = new DesenvContext())
                {
                    var list = (from contribuicao in ctx.tblContribuicao
                                join person in ctx.tblPersons on contribuicao.intClientID equals person.intContactID
                                join client in ctx.tblClients on person.intContactID equals client.intClientID
                                where contribuicao.bitAtiva == true && (contribuicao.intClientID == filtro.ClientId || contribuicao.intOpcaoPrivacidade == (int)TipoOpcaoPrivacidade.Publica)
                                let IsVideo = ctx.tblContribuicaoArquivo.Any(arquivo => arquivo.intContribuicaoID == contribuicao.intContribuicaoID && arquivo.intTipoArquivo.Value == (int)EnumTipoArquivoContribuicao.Video)
                                let IsAudio = ctx.tblContribuicaoArquivo.Any(arquivo => arquivo.intContribuicaoID == contribuicao.intContribuicaoID && arquivo.intTipoArquivo.Value == (int)EnumTipoArquivoContribuicao.Audio)
                                let IsImagem = ctx.tblContribuicaoArquivo.Any(arquivo => arquivo.intContribuicaoID == contribuicao.intContribuicaoID && arquivo.intTipoArquivo.Value == (int)EnumTipoArquivoContribuicao.Imagem)
                                select new ContribuicaoDTO
                                {
                                    ContribuicaoId = contribuicao.intContribuicaoID,
                                    Descricao = contribuicao.txtDescricao,
                                    ClientId = contribuicao.intClientID.Value,
                                    MedGrupoID = contribuicao.intMedGrupoID.Value,
                                    DataCriacao = contribuicao.dteDataCriacao.Value,
                                    Editada = contribuicao.bitEditado == true,
                                    NomeAluno = person.txtName,
                                    Estado = contribuicao.txtEstado,
                                    SiglaAluno = person.txtName.Substring(0, 1),
                                    Dono = (contribuicao.intClientID == filtro.ClientId),
                                    ApostilaId = contribuicao.intApostilaID,
                                    CodigoMarcacao = contribuicao.txtCodigoMarcacao,
                                    Origem = contribuicao.txtOrigem,
                                    BitAprovacaoMedgrupo = contribuicao.bitAprovacaoMedgrupo,
                                    OrigemSubnivel = contribuicao.txtOrigemSubnivel,
                                    TrechoSelecionado = contribuicao.txtTrechoSelecionado,
                                    IsVideo = IsVideo,
                                    IsAudio = IsAudio,
                                    IsImagem = IsImagem,
                                    Arquivada = ctx.tblContribuicoes_Arquivadas.Any(arquivada => arquivada.intContribuicaoID == contribuicao.intContribuicaoID && arquivada.intClientID == filtro.ClientId && !arquivada.bitAprovarMaisTarde),
                                    AprovarMaisTarde = ctx.tblContribuicoes_Arquivadas.Any(arquivada => arquivada.intContribuicaoID == contribuicao.intContribuicaoID && arquivada.intClientID == filtro.ClientId && arquivada.bitAprovarMaisTarde),
                                    Encaminhada = ctx.tblContribuicao_Encaminhadas.Any(encaminhada => encaminhada.intContribuicaoID == contribuicao.intContribuicaoID && encaminhada.intClientID == filtro.ClientId),
                                    ProfessoresEncaminhados = ctx.tblContribuicao_Encaminhadas.Where(encaminhada => encaminhada.intClientID == filtro.ClientId && contribuicao.intContribuicaoID == encaminhada.intContribuicaoID).Select(y => y.intEmployeeID),
                                    Interacoes = ctx.tblContribuicoes_Interacao.Where(x => x.intClientID == filtro.ClientId && x.intContribuicaoID == contribuicao.intContribuicaoID).Select(y => y.intContribuicaoTipo),
                                    TipoCategoria = contribuicao.intTipoCategoria,
                                    TipoContribuicao = contribuicao.intTipoContribuicao,
                                    NumeroCapitulo = contribuicao.intNumCapitulo,
                                    OpcaoPrivacidade = (TipoOpcaoPrivacidade)contribuicao.intOpcaoPrivacidade
                                });

                    list = AplicarFiltros(list, filtro);

                    if (filtro.ApostilaId > 0)
                    {
                        list = list.OrderBy(x => x.TipoCategoria).ThenBy(x => x.NumeroCapitulo).ThenByDescending(x => x.DataCriacao);
                    }
                    else
                    {
                        list = list.OrderByDescending(x => x.DataCriacao);
                    }

                    if (filtro.Page > 0)
                        list = list.Skip((filtro.Page - 1) * PageSize).Take(PageSize);

                    return list.ToList();
                }
            }
        }

        public ContribuicaoDTO GetContribuicao(int id)
        {
            using (var ctx = new DesenvContext())
            {
                var contribuicao = ctx.tblContribuicao.FirstOrDefault(x => x.intContribuicaoID == id);
                var entity = new ContribuicaoDTO()
                {
                    ContribuicaoId = contribuicao.intContribuicaoID,
                    ClientId = contribuicao.intClientID.Value,
                    ApostilaId = contribuicao.intApostilaID,
                    BitAprovacaoMedgrupo = contribuicao.bitAprovacaoMedgrupo,
                    CodigoMarcacao = contribuicao.txtCodigoMarcacao,
                    Estado = contribuicao.txtEstado,
                    DataCriacao = contribuicao.dteDataCriacao.Value,
                    TrechoSelecionado = contribuicao.txtTrechoSelecionado,
                    Descricao = contribuicao.txtDescricao,
                    Origem = contribuicao.txtOrigem,
                    OrigemSubnivel = contribuicao.txtOrigemSubnivel,
                    NumeroCapitulo = contribuicao.intNumCapitulo,
                    Editada = contribuicao.bitEditado
                };
                return entity;
            }
        }

        public bool HasContribuicaoArquivada(Contribuicao e)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblContribuicoes_Arquivadas
                    .Any(x => x.intClientID == e.ClientId && x.intContribuicaoID == e.ContribuicaoId && x.bitAprovarMaisTarde == e.BitAprovarMaisTarde);
            }
        }

        public int DeleteContribuicaoArquivada(int id)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var contribuicao = ctx.tblContribuicoes_Arquivadas.FirstOrDefault(x => x.intContribuicaoID == id);
                    ctx.Entry(contribuicao).State = EntityState.Deleted;
                    return ctx.SaveChanges();
                }
                catch
                {
                    return -1;
                }
            }
        }

        public int InserirContribuicao(Contribuicao e)
        {
            using (var ctx = new DesenvContext())
            {
                var entity = new tblContribuicao()
                {
                    intClientID = e.ClientId,
                    intApostilaID = e.ApostilaId,
                    dteDataCriacao = DateTime.Now,
                    txtDescricao = e.Descricao,
                    bitAtiva = true,
                    txtOrigem = e.Origem,
                    bitEditado = false,
                    txtEstado = e.Estado,
                    intNumCapitulo = Convert.ToInt32(e.NumeroCapitulo),
                    txtTrechoSelecionado = e.TrechoApostila,
                    txtCodigoMarcacao = e.CodigoMarcacao,
                    txtOrigemSubnivel = e.OrigemSubnivel,
                    intOpcaoPrivacidade = (int)e.OpcaoPrivacidade,
                    intTipoCategoria = (int)e.TipoCategoria,
                    intTipoContribuicao = (int)e.TipoContribuicao,
                };


                ctx.tblContribuicao.Add(entity);
                ctx.SaveChanges();
                return entity.intContribuicaoID;
            }
        }

        public int UpdateContribuicao(Contribuicao e)
        {
            using (var ctx = new DesenvContext())
            {
                var entity = ctx.tblContribuicao.FirstOrDefault(x => x.intContribuicaoID == e.ContribuicaoId);

                entity.txtDescricao = e.Descricao;
                entity.bitEditado = true;
                return ctx.SaveChanges();
            }
        }

        public int AprovarContribuicao(Contribuicao e)
        {
            using(var ctx = new DesenvContext())
            {
                var entity = ctx.tblContribuicao.FirstOrDefault(x => x.intContribuicaoID == e.ContribuicaoId);
                entity.bitAprovacaoMedgrupo = true;
                entity.intMedGrupoID = e.MedGrupoID;

                return ctx.SaveChanges();
            }
        }

        public int DeletarContribuicao(int id)
        {
            using (var ctx = new DesenvContext())
            {
                var contribuicao = ctx.tblContribuicao.FirstOrDefault(x => x.intContribuicaoID == id);
                if(contribuicao != null)
                {
                    contribuicao.bitAtiva = false;
                }
                return ctx.SaveChanges();
            }
        }

        public int AprovarContribuicao(int id)
        {
            using (var ctx = new DesenvContext())
            {
                var contribuicao = ctx.tblContribuicao.FirstOrDefault(x => x.intContribuicaoID == id);
                if (contribuicao != null)
                {
                    contribuicao.bitAprovacaoMedgrupo = true;
                }
                return ctx.SaveChanges();
            }
        }

        public int ArquivarContribuicao(Contribuicao e)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var entity = ctx.tblContribuicao.FirstOrDefault(x => x.intContribuicaoID == e.ContribuicaoId);
                    if (entity != null)
                    {
                        var contribuicaoArquivada = new tblContribuicoes_Arquivadas()
                        {
                            intContribuicaoID = entity.intContribuicaoID,
                            intClientID = e.ClientId,
                            dteDataCriacao = DateTime.Now,
                            bitAprovarMaisTarde = e.BitAprovarMaisTarde
                        };

                        ctx.tblContribuicoes_Arquivadas.Add(contribuicaoArquivada);
                    }
                    return ctx.SaveChanges();
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int EncaminharContribuicao(Contribuicao e)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    foreach(var idProfessor in e.ProfessoresSelecionados)
                    {
                        var obj = new tblContribuicao_Encaminhadas
                        {
                            intContribuicaoID = e.ContribuicaoId,
                            intClientID = e.ClientId,
                            intEmployeeID = idProfessor,
                            dteDataEncaminhamento = DateTime.Now
                        };
                        ctx.tblContribuicao_Encaminhadas.Add(obj);
                    }
                    return ctx.SaveChanges();
                }
                catch
                {
                    return 0;
                }
            }
        }

        private IQueryable<ContribuicaoDTO> AplicarFiltros(IQueryable<ContribuicaoDTO> list, ContribuicaoFiltroDTO filtro)
        {
            if (filtro.ByAudio || filtro.ByImage || filtro.ByText || filtro.ByVideo)
            {
                list = list.Where(x =>
                    (x.IsAudio == true && filtro.ByAudio) ||
                    (x.IsImagem == true && filtro.ByImage) ||
                    (!string.IsNullOrEmpty(x.Descricao) && filtro.ByText) ||
                    (x.IsVideo == true && filtro.ByVideo)
                );
            }

            if (filtro.JustMyAid) list = list.Where(x => x.Dono == true);

            if (filtro.ApostilaId > 0) list = list.Where(x => x.ApostilaId == filtro.ApostilaId);

            if (filtro.ContribuicaoId > 0) list = list.Where(x => x.ContribuicaoId == filtro.ContribuicaoId);

            if (filtro.IsPendente) list = list.Where(x => x.BitAprovacaoMedgrupo == false);

            if (!string.IsNullOrEmpty(filtro.CodigoMarcacao)) list = list.Where(x => x.CodigoMarcacao == filtro.CodigoMarcacao);

            if (filtro.IsAprovado) list = list.Where(x => x.BitAprovacaoMedgrupo == true);

            if (filtro.TiposInteracoes != null && filtro.TiposInteracoes.Count() > 0) list = list.Where(x => x.Interacoes.Any(y => filtro.TiposInteracoes.Any(z => z == y)));

            if (filtro.IsPublicadasPorMim) list = list.Where(x => x.MedGrupoID.Value == filtro.ClientId);

            if (filtro.IsProfessor)
            {
                if (filtro.IsArquivada || filtro.IsAprovarMaisTarde || filtro.IsEncaminhado)
                {
                    if (filtro.IdsProfessores != null && filtro.IdsProfessores.Count() > 0)
                    {
                        list = list.Where(x =>
                            (x.Arquivada == filtro.IsArquivada && filtro.IsArquivada) ||
                            (x.AprovarMaisTarde == filtro.IsAprovarMaisTarde && filtro.IsAprovarMaisTarde) ||
                            ((x.ProfessoresEncaminhados.Any(y => filtro.IdsProfessores.Any(z => z == y))) && filtro.IsEncaminhado)
                        );
                    }
                    else
                    {
                        list = list.Where(x =>
                            (x.Arquivada == filtro.IsArquivada && filtro.IsArquivada) ||
                            (x.AprovarMaisTarde == filtro.IsAprovarMaisTarde && filtro.IsAprovarMaisTarde) ||
                            (x.Encaminhada == filtro.IsEncaminhado && filtro.IsEncaminhado)
                        );
                    }
                }
                else
                {
                    list = list.Where(x =>
                        x.Arquivada == false &&
                        x.AprovarMaisTarde == false &&
                        x.Encaminhada == false
                    );
                }
            }

            return list;
        }

        public int InsertInteracao(ContribuicaoInteracao e)
        {
            using (var ctx = new DesenvContext())
            {
                var entity = new tblContribuicoes_Interacao()
                {
                    intClientID = e.ClientId,
                    intContribuicaoID = e.ContribuicaoId,
                    intContribuicaoTipo = (int)e.TipoInteracao,
                    dteDataCriacao = DateTime.Now
                };
                ctx.tblContribuicoes_Interacao.Add(entity);
                return ctx.SaveChanges();
            }
        }

        public ContribuicaoInteracao GetInteracao(ContribuicaoInteracao e)
        {
            using (var ctx = new DesenvContext())
            {
                var result = ctx.tblContribuicoes_Interacao.FirstOrDefault(x => x.intContribuicaoID == e.ContribuicaoId && x.intContribuicaoTipo == (int) e.TipoInteracao);
                if (result != null)
                {
                    e.ContribuicaoInteracaoId = result.intContribuicaoInteracaoID;
                }
                return e;
            }
        }

        public int DeleteContribuicaoInteracao(int id)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var entity = ctx.tblContribuicoes_Interacao.FirstOrDefault(x => x.intContribuicaoInteracaoID == id);
                    if (entity != null)
                    {
                        ctx.tblContribuicoes_Interacao.Remove(entity);
                    }
                    return ctx.SaveChanges();
                }
                catch
                {
                    return -1;
                }
            }
        }
    }
}