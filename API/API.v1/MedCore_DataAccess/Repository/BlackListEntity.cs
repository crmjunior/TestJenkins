using System;
using System.Linq;
using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Contracts.Data;

namespace MedCore_DataAccess.Repository
{
    public class BlackListEntity : IBlackListData
    {
        public List<Pessoa> GetAll()
        {
            try
            {
                var alunoLista = new List<Pessoa>();

                var listaEmails = GetEmails();
                var anexosDossie = GetAnexoDossie();
                var listaMotivoHistorico = GetMotivoHitorico();
                var listaBlacklistLog = GetBlacklistLog();
                var bloqueadosPorRecurso = GetBloqueadosPorRecurso();
                var bloqueadosPorAplicativo = GetBloqueadosTodosAplicativos();
                var bloqueadosPorInscricao = GetBloqueadosPorInscricao();
                var bloqueadosPorAplicacao = GetBloqueadosPorAplicacao();
                var bloqueados = bloqueadosPorRecurso.Concat(bloqueadosPorAplicativo.Concat(bloqueadosPorInscricao.Concat(bloqueadosPorAplicacao))).ToList();



                var retorno = (from bl in bloqueados
                               select new Pessoa
                               {
                                   ID = bl.ID,
                                   Nome = bl.Nome,
                                   Register = bl.Register,
                                   Bloqueios = bl.Bloqueios,
                                   Email = bl.Email
                               }).GroupBy(x => x.Register).ToList();

                var blackListEmAprovacao = GetAllEmAprovacao();

                foreach (var item in retorno)
                {
                    if (AindaEmAprovacao(blackListEmAprovacao, item))
                        continue;

                    var aluno = new Pessoa();
                    aluno.Nome = item.First().Nome;
                    aluno.ID = item.First().ID;
                    aluno.Register = item.First().Register;
                    aluno.Bloqueios = new List<Bloqueio>();
                    aluno.Email = listaEmails.Any(e => e.Register == item.First().Register) ? listaEmails.Where(e => e.Register == item.First().Register).Select(e => e.Email).FirstOrDefault() : item.First().Email;
                    aluno.AnexoDossie = aluno.ID == 0 ? anexosDossie.Where(c => c.Register == aluno.Register).ToList()
                                                                   : anexosDossie.Where(c => c.ContactID == aluno.ID).ToList();
                    foreach (var obj in item)
                        aluno.Bloqueios.Add(obj.Bloqueios.First());

                    alunoLista.Add(aluno);
                }

                alunoLista.ForEach(a => a.Motivos = listaMotivoHistorico.Where(l => l.Register == a.Register).ToList());

                alunoLista.ForEach(b => b.BlacklistLog = listaBlacklistLog.Where(bl => bl.txtRegister == b.Register).ToList());

                return alunoLista.OrderBy(o => o.Register).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<Pessoa> GetEmails()
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    return (from m in ctx.tblClients_BlackList
                            select new Pessoa
                            {
                                Email = m.txtEmail,
                                Register = m.txtRegister
                            }).ToList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private List<AnexoDossie> GetAnexoDossie()
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    return (from a in ctx.tblBlackList_Anexo
                            where a.bitAtivo
                            select new AnexoDossie
                            {
                                ID = a.intBlackListAnexoID,
                                ContactID = a.intContactID,
                                Anexo = a.txtAnexoDossie,
                                Register = a.txtRegister
                            }).ToList();
                }
                catch
                {
                    throw;
                }
            }
        }

        public List<MotivoHistorico> GetMotivoHitorico()
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    return (from m in ctx.tblClients_BlackListMotivos
                            orderby m.dteDataMotivo descending
                            select new MotivoHistorico
                            {
                                Id = m.intClientBlackListID,
                                Data = m.dteDataMotivo,
                                Motivo = m.txtMessage,
                                Register = m.txtRegister
                            }).ToList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public List<BlacklistLog> GetBlacklistLog()
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    return (from bl in ctx.tblBlacklist_Log
                            orderby bl.dteData descending
                            select new BlacklistLog
                            {
                                intId = bl.intId,
                                intClientId = bl.intClientId ?? 0,
                                txtRegister = bl.txtRegister,
                                intTipoBloqueio = bl.intTipoBloqueio ?? 0,
                                txtMotivo = bl.txtMotivo,
                                bitBloqueio = bl.bitBloqueio ?? false,
                                dteData = bl.dteData ?? DateTime.MinValue,
                                EmployeeId = bl.intEmployeeId ?? 0,
                                EmployeeNome = ctx.tblPersons.Where(a => a.intContactID == bl.intEmployeeId).Select(a => a.txtName).FirstOrDefault(),
                                Nome = bl.txtNome,
                                Email = bl.txtEmail,
                                Faculdade = bl.txtFaculdade,
                                Tipo = (BlacklistLog.EnumTipo)bl.intTipo

                            }).ToList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private List<Pessoa> GetBloqueadosPorRecurso()
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var bloqueios = (
                                from ad in ctx.tblConcurso_Recurso_AccessDenied
                                group ad by ad.intClientID into g
                                select new
                                {
                                    intClientID = g.Key,
                                    MaxId = g.Max(ad => ad.intConcursoRecursoId)
                                });

                    return (from p in ctx.tblPersons
                            join ad in ctx.tblConcurso_Recurso_AccessDenied on p.intContactID equals ad.intClientID
                            from c in ctx.tblClients_BlackList.Where(client => client.txtRegister == p.txtRegister).DefaultIfEmpty()
                            join b in bloqueios on ad.intConcursoRecursoId equals b.MaxId
                            where ad.intClientID == b.intClientID &&
                                  (!string.IsNullOrEmpty(p.txtRegister)) &&
                                  ad.bitActive.Value
                            select new Pessoa
                            {
                                Register = p.txtRegister.TrimEnd(),
                                ID = p.intContactID,
                                Nome = p.txtName.TrimEnd(),
                                Email = c.txtEmail == null ? p.txtEmail1 : c.txtEmail,
                                Bloqueios = new List<Bloqueio>()
                             {
                                         new Bloqueio()
                                         {
                                           dteDateTimeStart = ad.dteDateTimeStart.HasValue ? ad.dteDateTimeStart.Value : default(DateTime),
                                           MotivoBloqueio = ad.txtReason.TrimEnd(),
                                           TabelaBloqueio = Bloqueio.TipoBloqueio.Recursos,
                                           Categoria = (from bc in ctx.tblBlackList_Categoria
                                                        where bc.intCategoriaID == ad.intBlackListCategoriaID
                                                        select new BlackListCategoria()
                                                        {
                                                          CategoriaID = bc.intCategoriaID,
                                                          Descricao = bc.txtDescricao
                                                        }).FirstOrDefault()
                                         }
                                       }
                            }).ToList();
                }
                catch
                {
                    throw;
                }
            }
        }

        private List<Pessoa> GetBloqueadosTodosAplicativos()
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    return (from p in ctx.tblPersons
                            join ad in ctx.tblEmed_AccessDenied on p.intContactID equals ad.intClientID
                            from c in ctx.tblClients_BlackList.Where(client => client.txtRegister == p.txtRegister).DefaultIfEmpty()
                            join app in ctx.tblApplication_AcessDenied on ad.intEmedId equals app.intEmedID
                            where ad.intEmedId == app.intEmedID &&
                                  (!string.IsNullOrEmpty(p.txtRegister)) && app.intApplicationID == 0
                            select new Pessoa
                            {
                                Register = p.txtRegister.TrimEnd(),
                                ID = p.intContactID,
                                Nome = p.txtName.TrimEnd(),
                                Email = c.txtEmail == null ? p.txtEmail1 : c.txtEmail,
                                Bloqueios = new List<Bloqueio>()
                             {
                                         new Bloqueio()
                                         {
                                           dteDateTimeStart = ad.dteDateTimeStart.HasValue ? ad.dteDateTimeStart.Value : default(DateTime),
                                           MotivoBloqueio = app.txtReason.TrimEnd(),
                                           TabelaBloqueio = Bloqueio.TipoBloqueio.Aplicativos,
                                           Categoria = (from bc in ctx.tblBlackList_Categoria
                                                        where bc.intCategoriaID == ad.intBlackListCategoriaID
                                                        select new BlackListCategoria()
                                                        {
                                                          CategoriaID = bc.intCategoriaID,
                                                          Descricao = bc.txtDescricao
                                                        }).FirstOrDefault()
                                         }
                                       }
                            }).ToList();
                }
                catch
                {
                    throw;
                }
            }
        }

        private List<Pessoa> GetBloqueadosPorInscricao()
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var bloqueios = (
                                from ib in ctx.tblInscricoesBloqueios
                                group ib by ib.txtRegister into g
                                select new
                                {
                                    txtRegister = g.Key,
                                    MaxId = g.Max(ib => ib.intID)
                                });

                    return (from ib in ctx.tblInscricoesBloqueios
                            join b in bloqueios on ib.intID equals b.MaxId
                            join p1 in ctx.tblPersons on ib.txtRegister equals p1.txtRegister into p2
                            from p in p2.DefaultIfEmpty()
                            from c in ctx.tblClients_BlackList.Where(client => client.txtRegister == p.txtRegister).DefaultIfEmpty()
                            where ib.txtRegister == b.txtRegister &&
                                  (!string.IsNullOrEmpty(ib.txtRegister))
                            select new Pessoa
                            {
                                Register = ib.txtRegister.TrimEnd(),
                                ID = p.intContactID,
                                Nome = p.txtName.TrimEnd(),
                                Email = c.txtEmail == null ? p.txtEmail1 : c.txtEmail,
                                Bloqueios = new List<Bloqueio>()
                             {
                                         new Bloqueio()
                                         {
                                           dteDateTimeStart = ib.dteInclusaoBloqueio.Value,
                                           MotivoBloqueio = ib.txtMotivo == null ? "Bloqueio Inscrições": ib.txtMotivo.TrimEnd(),
                                           TabelaBloqueio = Bloqueio.TipoBloqueio.Inscricoes,
                                           Categoria = (from bc in ctx.tblBlackList_Categoria
                                                        where bc.intCategoriaID == ib.intBlackListCategoriaID
                                                        select new BlackListCategoria()
                                                        {
                                                          CategoriaID = bc.intCategoriaID,
                                                          Descricao = bc.txtDescricao
                                                        }).FirstOrDefault()
                                         }
                                       }
                            }).ToList();
                }
                catch
                {
                    throw;
                }
            }
        }

        private List<Pessoa> GetBloqueadosPorAplicacao()
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var query = (from p in ctx.tblPersons
                                 join e in ctx.tblEmed_AccessDenied on p.intContactID equals e.intClientID
                                 from c in ctx.tblClients_BlackList.Where(client => client.txtRegister == p.txtRegister).DefaultIfEmpty()
                                 join ad in ctx.tblApplication_AcessDenied on e.intEmedId equals ad.intEmedID
                                 where ad.intEmedID == e.intEmedId &&
                                                 (!string.IsNullOrEmpty(p.txtRegister)) && ad.intApplicationID == (int)Aplicacoes.LeitordeApostilas
                                 select new Pessoa
                                 {
                                     Register = p.txtRegister.TrimEnd(),
                                     ID = p.intContactID,
                                     Nome = p.txtName.TrimEnd(),
                                     Email = c.txtEmail == null ? p.txtEmail1 : c.txtEmail,
                                     Bloqueios = new List<Bloqueio>()
                                           {
                                             new Bloqueio()
                                             {
                                               dteDateTimeStart = e.dteDateTimeStart.HasValue ? e.dteDateTimeStart.Value : default(DateTime),
                                               MotivoBloqueio = ad.txtReason.TrimEnd(),
                                               TabelaBloqueio = Bloqueio.TipoBloqueio.Aplicacao,
                                               AplicacaoId = (int)ad.intApplicationID,
                                               Categoria = (from bc in ctx.tblBlackList_Categoria
                                                            where bc.intCategoriaID == e.intBlackListCategoriaID
                                                            select new BlackListCategoria()
                                                            {
                                                              CategoriaID = bc.intCategoriaID,
                                                              Descricao = bc.txtDescricao
                                                            }).FirstOrDefault()
                                             }
                                           }
                                 }).ToList();

                    return query;
                }
                catch
                {
                    throw;
                }
            }
        }   

        private bool AindaEmAprovacao(List<Pessoa> blackListEmAprovacao, IGrouping<string, Pessoa> item)
        {
            return blackListEmAprovacao.Any(a => a.Register == item.Key);
        }

        private List<Pessoa> GetAllEmAprovacao()
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    return (from a in ctx.tblClients_BlackListAprovacoes
                            from person in ctx.tblPersons.Where(p => a.intClientID == p.intContactID).DefaultIfEmpty()
                            select new Pessoa
                            {
                                ID = a.intClientID,
                                Nome = person.txtName.Trim(),
                                Register = a.txtRegister.Trim(),
                                Email = a.txtEmail == null ? person.txtEmail1 : a.txtEmail
                            }).ToList();

                }
                catch
                {
                    throw;
                }
            }
        }

        public bool isBloqueado(Pessoa pessoa)
        {
            var bloqueados = GetAll();
            return (bloqueados.Where(b => b.Register.Trim() == pessoa.Register.Trim()).Count() > 0);
        }

        public bool IsBloqueado(Pessoa pessoa, Bloqueio.TipoBloqueio tipoBloqueio)
        {
            var bloqueados = GetAll();
            return (bloqueados.Where(b => b.Register.Trim() == pessoa.Register.Trim() && b.Bloqueios.Any(x => x.TabelaBloqueio == tipoBloqueio)).Count() > 0);
        }

        public bool IsAcessoBloqueado(int clientId, Aplicacoes aplicacao)
        {
            bool bloqueado = false;

            var isRecursos = (aplicacao == Aplicacoes.Recursos || aplicacao == Aplicacoes.Recursos_iPad || aplicacao == Aplicacoes.Recursos_iPhone || aplicacao == Aplicacoes.Recursos_Android);

            bool isOnlyMedReaderDenied = false;
            bool isBloqueadoPorAplicacoesOuAplicativos = false;

            if (clientId > 0)
            {
                var _blacklist = GetAll().Where(x => x.ID == clientId);
                var blacklistMember = _blacklist.FirstOrDefault();

                if (blacklistMember != null)
                {
                    isBloqueadoPorAplicacoesOuAplicativos = _blacklist.Where(x => x.Bloqueios.Any(y => (y.TabelaBloqueio == Bloqueio.TipoBloqueio.Aplicacao || y.TabelaBloqueio == Bloqueio.TipoBloqueio.Aplicativos))).Any();

                    isOnlyMedReaderDenied = blacklistMember.Bloqueios.Count == 1 && (blacklistMember.Bloqueios[0].AplicacaoId == (int)Aplicacoes.LeitordeApostilas);

                    if (isOnlyMedReaderDenied && aplicacao != Aplicacoes.LeitordeApostilas)
                        isBloqueadoPorAplicacoesOuAplicativos = false;

                    if (isRecursos || aplicacao == Aplicacoes.MEDSOFT)
                    {
                        bloqueado = IsBloqueadoPorRecurso(clientId);
                    }
                    else
                    {
                        bloqueado = !(!isBloqueadoPorAplicacoesOuAplicativos || (isOnlyMedReaderDenied && aplicacao == Aplicacoes.MsProMobile));
                    }
                }
            }

            return bloqueado;
        }

        private bool IsBloqueadoPorRecurso(int clientId)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblConcurso_Recurso_AccessDenied.Where(b => b.bitActive.Value).Select(b => b.intClientID).Contains(clientId);
            }
        }

    }
}