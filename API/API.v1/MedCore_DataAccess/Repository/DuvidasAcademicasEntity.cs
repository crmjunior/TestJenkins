using System;
using System.Linq;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Contracts.Repository;
using MedCore_DataAccess.DTO.DuvidaAcademica;
using MedCore_DataAccess.Entidades;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_API.Academico;
using System.Data.SqlClient;
using MedCore_DataAccess.Util;
using Microsoft.EntityFrameworkCore;

namespace MedCore_DataAccess.Repository
{
    public class DuvidasAcademicasEntity : IDuvidasAcademicasData
    {
        public const int QuantidadeDuvidasPadrao = 50;
        public const int RoleCoordenador = 6;
        public const int AnoSimuladoAcademico = 2019;

        public enum TipoQuestao
        {
            Simulado = 1,
            Concurso = 2,
        }
        public bool DeleteDenuncia(DenunciaDuvidasAcademicasDTO obj)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var entity = ctx.tblDuvidasAcademicas_Denuncia.FirstOrDefault(x => (x.intDuvidaID == obj.DuvidaId || x.intRespostaID == obj.RespostaId) && x.intContactID == obj.ClientId);
                    if (entity != null)
                    {
                        ctx.tblDuvidasAcademicas_Denuncia.Remove(entity);
                        var result = ctx.SaveChanges();
                        return result > 0;
                    }
                    return false;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int DeleteDuvida(DuvidaAcademicaInteracao interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    int duvidaId = Convert.ToInt32(interacao.DuvidaId);
                    var entity = ctx.tblDuvidasAcademicas_Duvidas.FirstOrDefault(x => x.intDuvidaID == duvidaId);
                    if (entity != null)
                    {
                        entity.bitAtiva = false;
                    }
                    return ctx.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        public int DeleteDuvidaApostilaPorMarcacao(DuvidaAcademicaInteracao duvidaAcademicaInteracao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var entity = ctx.tblDuvidasAcademicas_DuvidaApostila
                    .Include(a => a.tblDuvidasAcademicas_Duvidas)
                    .FirstOrDefault(x => x.txtCodigoMarcacao == duvidaAcademicaInteracao.CodigoMarcacao);
                    entity.tblDuvidasAcademicas_Duvidas.bitAtiva = false;

                    return ctx.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        public int DeleteDuvidaArquivada(tblDuvidasAcademicas_DuvidasArquivadas duvida)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    ctx.Entry(duvida).State = EntityState.Deleted;
                    return ctx.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        public int DeleteInteracao(tblDuvidasAcademicas_Interacoes interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    ctx.Entry(interacao).State = EntityState.Deleted;
                    return ctx.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        public int DeleteRespostaReplica(DuvidaAcademicaInteracao interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var entity = ctx.tblDuvidasAcademicas_Resposta.FirstOrDefault(x => x.intRespostaID == interacao.RespostaId);
                    entity.bitAtiva = false;
                    return ctx.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        public int EnviarEmailDuvidaAcademica(string mailTo, string mailBody, string mailSubject, string mailProfile)
        {
            return Utilidades.SendMailDirect(mailTo, mailBody, mailSubject, mailProfile);
        }

        public int UpdateDuvidaNotificacao(tblDuvidasAcademicas_Notificacao notificacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var entity = ctx.tblDuvidasAcademicas_Notificacao.FirstOrDefault(x => x.intDuvidaAcademicaNotificacaoId == notificacao.intDuvidaAcademicaNotificacaoId);

                    entity.bitAtiva = false;

                    return ctx.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        public IList<string> GetBlackWords()
        {
            using (var ctx = new DesenvContext())
            {
                var blackWords = ctx.tblBlackWords_DuvidasAcademicas.Select(x => x.txtBlackWordDA.Trim().ToLower()).ToList();
                return blackWords;
            }
        }

        public List<AcademicoDADTO> GetCoordenadores()
        {
            using (var ctx = new DesenvContext())
            {
                var coordenadores = (from p in ctx.tblEmailNotificacaoDuvidasAcademicas
                                     join pe in ctx.tblPersons on p.intContactID equals pe.intContactID into _pe
                                     from pers in _pe.DefaultIfEmpty()
                                     select new AcademicoDADTO
                                     {
                                         Id = p.intContactID,
                                         Email = p.txtEmail != null ? p.txtEmail : pers.txtEmail1 != null ? pers.txtEmail1 : string.Empty,
                                         Nome = p.intContactID != null ? pers.txtName : p.txtName != null ? p.txtName : "Acadêmico"
                                     })
                                     .ToList()
                                     .GroupBy(x => x.Id)
                                     .Select(x => x.FirstOrDefault())
                                     .ToList();
                return coordenadores;
            }
        }

        public DuvidaAcademicaDTO GetDuvida(int idDuvida)
        {
            using (var ctx = new DesenvContext())
            {
                var obj = ctx.tblDuvidasAcademicas_Duvidas.FirstOrDefault(x => x.intDuvidaID == idDuvida);
                var duvida = new DuvidaAcademicaDTO()
                {
                    DuvidaId = obj.intDuvidaID,
                    ClientId = obj.intClientID,
                    Descricao = obj.txtDescricao,
                    NomeFake = obj.txtNomeFake,
                    EstadoFake = obj.txtEstadoFake,
                    Origem = obj.txtOrigem,
                    OrigemSubnivel = obj.txtOrigemSubnivel,
                    BitAtiva = obj.bitAtiva,
                    BitEditada = obj.bitEditado,
                    BitAtivaAcademico = obj.bitAtivaDesenv
                };
                return duvida;
            }
        }

        public tblDuvidasAcademicas_DuvidasArquivadas GetDuvidaArquivada(DuvidaAcademicaInteracao interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    int duvidaId = Convert.ToInt32(interacao.DuvidaId);
                    return ctx.tblDuvidasAcademicas_DuvidasArquivadas
                        .FirstOrDefault(x => x.intClientID == interacao.ClientId && x.intDuvidaID == duvidaId && x.bitRespMaisTarde == interacao.BitResponderMaisTarde);
                }
                catch
                {
                    throw;
                }
            }
        }

        public IList<DuvidaAcademicaContract> GetDuvidas(DuvidaAcademicaFiltro filtro)
        {
            using (var ctx = new DesenvContext())
            {
                var dataAtual = DateTime.Now;
                var list = (from a in ctx.tblDuvidasAcademicas_Duvidas
                            join b in ctx.tblDuvidasAcademicas_DuvidaQuestao on a.intDuvidaID equals b.intDuvidaID into _ab
                            from ab in _ab.DefaultIfEmpty()
                            join c in ctx.tblDuvidasAcademicas_DuvidaApostila on a.intDuvidaID equals c.intDuvidaId into _ac
                            from ac in _ac.DefaultIfEmpty()
                            join d in ctx.tblMaterialApostila on ac.intMaterialApostilaId equals d.intID into _cd
                            from cd in _cd.DefaultIfEmpty()
                            join e in ctx.tblProducts on cd.intProductId equals e.intProductID into _ed
                            from ed in _ed.DefaultIfEmpty()
                            join f in ctx.tblPersons on a.intClientID equals f.intContactID
                            join g in ctx.tblClients on f.intContactID equals g.intClientID

                            let TemRascunhoLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                  where
                                                  resp.intDuvidaID == a.intDuvidaID && resp.bitAtiva && resp.txtObservacao != null && (resp.bitAprovacaoMedgrupo == 0 || resp.bitAprovacaoMedgrupo == null)
                                                  select resp).Any()
                            let MinhasRespostasLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                      where resp.intClientID == filtro.ClientId && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID && resp.intParentRespostaID == null
                                                      select resp).Any()
                            let AprovacaoMedGrupoLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                        where resp.bitAprovacaoMedgrupo == (int)TipoAprovacaoMedGrupo.Aprovado && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID
                                                        select resp).Any()
                            let RespostaMedGrupoLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                       where resp.bitRespostaMed == true && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID && resp.txtNomeFake == null && resp.txtEstadoFake == null
                                                       select resp).Any()
                            let NumeroRespostasLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                      where
                                                      resp.intDuvidaID == a.intDuvidaID && resp.bitAtiva && resp.intParentRespostaID == null && resp.bitAtivaDesenv == true
                                                      select resp).Count()
                            let Mais7DiasLet = (ctx.tblDuvidasAcademicas_Resposta.Where(resp => resp.intDuvidaID == a.intDuvidaID && resp.bitAtiva == true)
                                              .All(x => x.bitAprovacaoMedgrupo != 1 && x.bitRespostaMed != true) || !ctx.tblDuvidasAcademicas_Resposta.Any(resp => resp.intDuvidaID == a.intDuvidaID)) &&
                                              a.dteDataCriacao < DateTime.Now.AddDays(-7)
                            let MinhaDuvidaApostila = (from x in ctx.tblBooksEntitiesProfessor
                                                       join y in ctx.tblBooks on x.intBookEntitiesId equals y.intBookEntityID
                                                       where y.intBookID == ed.intProductID && x.intContactId == filtro.ClientId
                                                       select x).Any()
                            let MinhaDuvidaQuestaoConcurso = (from dq in ctx.tblDuvidasAcademicas_DuvidaQuestao
                                                              join dqc in ctx.tblConcursoQuestao_Classificacao on dq.intQuestaoId equals dqc.intQuestaoID
                                                              join dqp in ctx.tblProducts on dqc.intClassificacaoID equals dqp.intProductID
                                                              join dqb in ctx.tblBooks on dqp.intProductID equals dqb.intBookID
                                                              join dqpr in ctx.tblBooksEntitiesProfessor on dqb.intBookEntityID equals dqpr.intBookEntitiesId
                                                              where
                                                                dqc.intQuestaoID == ab.intQuestaoId &&
                                                                dq.intQuestaoId == ab.intQuestaoId &&
                                                              (ab.intTipoExercicioID == (int)Exercicio.tipoExercicio.CONCURSO || ab.intTipoExercicioID == (int)Exercicio.tipoExercicio.APOSTILA) &&
                                                                dqpr.intContactId == filtro.ClientId
                                                              select dq.intDuvidaID
                                                              ).Any()
                            let MinhaDuvidaQuestaoConcursoEspecialidade = (from dq in ctx.tblDuvidasAcademicas_DuvidaQuestao
                                                                           join dqc in ctx.tblConcursoQuestao_Classificacao on dq.intQuestaoId equals dqc.intQuestaoID
                                                                           join dqp in ctx.tblMedsoft_Especialidade_Classificacao on dqc.intClassificacaoID equals dqp.intClassificacaoID
                                                                           join dqpr in ctx.tblEspecialidadeProfessor on dqp.intEspecialidadeID equals dqpr.intEspecialidadeID
                                                                           where
                                                                           dqc.intQuestaoID == ab.intQuestaoId &&
                                                                           dq.intQuestaoId == ab.intQuestaoId &&
                                                                           dqpr.intContactID == filtro.ClientId
                                                                           select dq.intDuvidaID
                                                                            ).Any()

                            let MinhaDuvidaSimulado = (from dq in ctx.tblDuvidasAcademicas_DuvidaQuestao
                                                       join dscc in ctx.tblConcursoQuestao_Classificacao on dq.intQuestaoConcursoID equals dscc.intQuestaoID
                                                       join dqp in ctx.tblProducts on dscc.intClassificacaoID equals dqp.intProductID
                                                       join dsb in ctx.tblBooks on dqp.intProductID equals dsb.intBookID
                                                       join dspr in ctx.tblBooksEntitiesProfessor on dsb.intBookEntityID equals dspr.intBookEntitiesId
                                                       where
                                                            dscc.intQuestaoID == ab.intQuestaoId &&
                                                            dq.intQuestaoId == ab.intQuestaoId &&
                                                            ab.intTipoExercicioID == (int)Exercicio.tipoExercicio.SIMULADO &&
                                                            dspr.intContactId == filtro.ClientId
                                                       select ab.intDuvidaID).Any()

                            let MinhaDuvidaSimuladoAnteriores = (from dq in ctx.tblDuvidasAcademicas_DuvidaQuestao
                                                                 join dspr in ctx.tblEspecialidadeProfessor on dq.intEspecialidadeID equals dspr.intEspecialidadeID
                                                                 where
                                                                      dq.intQuestaoId == ab.intQuestaoId &&
                                                                      ab.intTipoExercicioID == (int)Exercicio.tipoExercicio.SIMULADO &&
                                                                      dspr.intContactID == filtro.ClientId
                                                                 select dspr.txtEspecialidade).Any()
                            let Favoritada = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intClientID == filtro.ClientId && x.bitFavorita == true && x.intDuvidaId == a.intDuvidaID)
                            let UpVotes = ctx.tblDuvidasAcademicas_Interacoes.Count(x => x.intDuvidaId == a.intDuvidaID && x.intVote == (int)TipoVoto.Upvote)
                            let VotadoUpvote = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intDuvidaId == a.intDuvidaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Upvote && x.intVote == (int)TipoVoto.Upvote && x.intClientID == filtro.ClientId)
                            let Denuncia = ctx.tblDuvidasAcademicas_Denuncia.Any(x => x.intContactID == filtro.ClientId && x.intDuvidaID == a.intDuvidaID)
                            let DenunciaAluno = ctx.tblDuvidasAcademicas_Denuncia.Any(x => x.intDuvidaID == a.intDuvidaID)
                            let NomeGestor = (from x in ctx.tblDuvidasAcademicas_DuvidasEncaminhadas
                                              join z in ctx.tblPersons on x.intGestorID equals z.intContactID
                                              where x.intDuvidaID == a.intDuvidaID
                                              select z.txtName).FirstOrDefault().Trim()
                            let PrimeirasDuvidas = filtro.IsAcademico ? ctx.tblDuvidasAcademicas_Duvidas.Count(x => x.intClientID == a.intClientID && x.bitAtiva && x.bitAtivaDesenv == true && a.dteDataCriacao > x.dteDataCriacao) < 5 : false
                            where
                            a.bitAtiva
                            select new DuvidaAcademicaContract
                            {
                                DuvidaId = a.intDuvidaID,
                                ExercicioId = ab.intExercicioId,
                                Privada = !a.bitAtivaDesenv.Value,
                                ProductId = ed.intProductID,
                                TipoCategoriaApostila = ac.intTipoCategoria,
                                NumeroCategoriaApostila = ac.intNumCapitulo.ToString(),
                                Favorita = Favoritada,
                                UpVotes = UpVotes,
                                Denuncia = Denuncia,
                                DenunciaAluno = DenunciaAluno,
                                Descricao = a.txtDescricao,
                                TrechoSelecionado = ac.txtTrecho,
                                EstadoAluno = a.txtEstadoFilial,
                                MaisDe7Dias = Mais7DiasLet,
                                PrimeirasDuvidas = PrimeirasDuvidas,
                                TemRascunho = TemRascunhoLet,
                                ClientId = a.intClientID,
                                BitAtiva = a.bitAtiva,
                                DataCriacao = a.dteDataCriacao,
                                BitResponderMaisTarde = false,
                                Editada = a.dteAtualizacao > a.dteDataCriacao,
                                ProfessoresEncaminhados = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Where(x => x.intGestorID == filtro.ClientId && a.intDuvidaID == x.intDuvidaID),
                                BitVisualizada = false,
                                BitEncaminhada = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Any(x => x.intDuvidaID == a.intDuvidaID && filtro.ClientId == x.intEmployeeID),
                                BitEnviada = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Any(x => x.intDuvidaID == a.intDuvidaID && filtro.ClientId == x.intGestorID),
                                NomeAluno = a.txtNomeFake == null ? f.txtName.Substring(0, 1) : a.txtNomeFake.Substring(0, 1),
                                NomeAlunoCompleto = f.txtName.Trim(),
                                NomeFake = a.txtNomeFake,
                                EstadoFake = a.txtEstadoFake,
                                Dono = a.intClientID == filtro.ClientId && a.txtNomeFake == null && a.txtEstadoFake == null,
                                QuestaoId = ab.intQuestaoId,
                                MinhaDuvidaApostila = filtro.MinhasApostilas ? MinhaDuvidaApostila : false,
                                MinhaDuvidaQuestaoConcurso = filtro.MinhasApostilas ? MinhaDuvidaQuestaoConcurso : false,
                                MinhaDuvidaSimulado = filtro.MinhasApostilas ? MinhaDuvidaSimulado : false,
                                MinhaDuvidaQuestaoConcursoEspecialidade = filtro.MinhasApostilas ? MinhaDuvidaQuestaoConcursoEspecialidade : false,
                                MinhaDuvidaSimuladoAnteriores = filtro.MinhasApostilas ? MinhaDuvidaSimuladoAnteriores : false,
                                NomeGestor = NomeGestor,
                                Arquivada = false,
                                NRespostas = NumeroRespostasLet,
                                VotadoUpvote = VotadoUpvote,
                                NumeroQuestao = ab.intNumQuestao,
                                CursoAluno = a.txtCurso,
                                MinhasRespostas = MinhasRespostasLet,
                                RespostaMedGrupo = RespostaMedGrupoLet,
                                AprovacaoMedGrupo = AprovacaoMedGrupoLet,
                                TipoQuestaoId = ab.intTipoQuestao,
                                ApostilaId = ac.intMaterialApostilaId,
                                NumeroCapitulo = ac.intNumCapitulo,
                                TipoCategoria = ac.intTipoCategoria.Value,
                                CodigoMarcacao = ac.txtCodigoMarcacao,
                                Origem = a.txtOrigem,
                                OrigemCompleta = null, 
                                OrigemSubnivel = (ab.txtOrigemQuestaoConcurso == null || ab.txtOrigemQuestaoConcurso.Trim() == "") ? a.txtOrigemSubnivel : ab.txtOrigemQuestaoConcurso,
                                OrigemProduto = a.txtOrigemProduto,
                                TipoExercicioId = ab.intTipoExercicioID,
                                Genero = f.intSex.Value
                            });

                list = AplicarFiltros(list, filtro);

                if (filtro.IsAcademico)
                {
                    list = list
                        .OrderByDescending(x => !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value)
                        .ThenByDescending(x => x.PrimeirasDuvidas)
                        .ThenByDescending(x => x.UpVotes)
                        .ThenBy(x => x.DuvidaId);

                }
                else if (filtro.ApostilaId > 0)
                {
                    list = list
                        .OrderBy(x => x.TipoCategoria)
                        .ThenBy(x => x.NumeroCapitulo)
                        .ThenByDescending(x => x.UpVotes)
                        .ThenByDescending(x => x.BitVisualizada)
                        .ThenByDescending(x => x.DuvidaId);
                }
                else
                {
                    list = list
                        .OrderByDescending(x => x.UpVotes)
                        .ThenByDescending(x => x.BitVisualizada)
                        .ThenByDescending(x => x.DuvidaId);
                }

                if (filtro.Page > 0)
                {
                    list = list.Skip((filtro.Page - 1) * QuantidadeDuvidasPadrao).Take(QuantidadeDuvidasPadrao);
                }

                return list.ToList();
            }
        }

        private IQueryable<DuvidaAcademicaContract> AplicarFiltros(IQueryable<DuvidaAcademicaContract> list, DuvidaAcademicaFiltro filtro)
        {
            var provas = new List<int>();

            if (filtro.SiglasConcurso != null)
            {
                foreach (var sigla in filtro.SiglasConcurso)
                {
                    provas.Add(Convert.ToInt32(sigla));
                }
            }

            if (filtro.IdsApostilas == null)
                filtro.IdsApostilas = new List<int>();

            if (filtro.IdsSimulados == null)
                filtro.IdsSimulados = new List<int>();

            if (filtro.IdsMateriais == null)
                filtro.IdsMateriais = new List<int>();

            if (filtro.IdsProfessores == null)
                filtro.IdsProfessores = new List<int>();

            int duvidaId = Convert.ToInt32(filtro.DuvidaId);
            if (Convert.ToInt32(filtro.DuvidaId) > 0)
                list = list.Where(x => x.DuvidaId == duvidaId);

            if (filtro.ApostilaId > 0)
                list = list.Where(x => x.ApostilaId == filtro.ApostilaId);

            if (Convert.ToInt32(filtro.QuestaoId) > 0)
            {
                int _questaoID = Convert.ToInt32(filtro.QuestaoId);
                list = list.Where(x => x.QuestaoId == _questaoID);
            }

            if (filtro.BitMinhas)
                list = list.Where(x => x.ClientId == filtro.ClientId || (x.BitEncaminhada && (filtro.IsAcademico || filtro.IsCoordenador)));

            if (!filtro.BitDenunciadas)
                list = list.Where(x => x.Privada ? x.ClientId == filtro.ClientId : true);

            if (!filtro.BitArquivadas)
                list = list.Where(x => !x.Arquivada);

            if (!filtro.BitResponderMaisTarde)
                list = list.Where(x => !x.BitResponderMaisTarde);

            if (filtro.IdsProfessores.Any() && filtro.BitTodosProfessores)
                list = list.Where(x => !x.BitEncaminhada);

            if ((filtro.NumeroCategoriaApostila != null && Convert.ToInt32(filtro.NumeroCategoriaApostila) > 0) && filtro.TipoCategoriaApostila > 0)
                list = list.Where(x => x.ApostilaId == filtro.ApostilaId && x.NumeroCategoriaApostila == filtro.NumeroCategoriaApostila && x.TipoCategoriaApostila == filtro.TipoCategoriaApostila);

            if (filtro.MaisDe7Dias || filtro.TemRascunho || filtro.BitMinhasRespostas || filtro.BitRespostaHomologadasMed || filtro.BitRespostaMed || filtro.BitEnviadas || filtro.BitArquivadas || filtro.BitFavoritas || filtro.BitResponderMaisTarde || filtro.BitEncaminhadas || filtro.BitTodosProfessores || filtro.IdsProfessores.Any() || filtro.BitDenunciadas || filtro.MinhasApostilas
                || filtro.BitSemInteracao || filtro.BitSemVinculo)
                list = list.Where(x =>
                    (x.MaisDe7Dias && filtro.MaisDe7Dias) ||
                    (x.TemRascunho && filtro.TemRascunho) ||
                    (x.AprovacaoMedGrupo.Value && filtro.BitRespostaHomologadasMed) ||
                    (x.RespostaMedGrupo.Value && filtro.BitRespostaMed) ||
                    (x.MinhasRespostas && filtro.BitMinhasRespostas) ||
                    (x.BitEnviada && filtro.BitEnviadas) ||
                    (x.BitEncaminhada && filtro.BitEncaminhadas) ||
                    (x.Favorita && filtro.BitFavoritas) ||
                    (
                        ((x.MinhaDuvidaApostila && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value) ||
                        (x.ApostilaId == null && x.QuestaoId == null && filtro.IsCoordenador && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value && !x.BitEnviada) ||
                        (x.MinhaDuvidaQuestaoConcurso && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value) ||
                        (x.MinhaDuvidaSimulado && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value) ||
                        (x.MinhaDuvidaSimuladoAnteriores && !x.MinhaDuvidaSimulado && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value) ||
                        (x.MinhaDuvidaQuestaoConcursoEspecialidade && x.TipoExercicioId == 2 && !x.MinhaDuvidaQuestaoConcurso && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value) ||
                        (x.BitEncaminhada && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value)) && filtro.MinhasApostilas && !x.BitEnviada) ||
                    ((x.ApostilaId == null && x.QuestaoId == null) && filtro.BitSemVinculo) ||
                    ((!x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value) && filtro.BitSemInteracao) ||
                    (x.Arquivada && filtro.BitArquivadas) ||
                    (x.BitResponderMaisTarde && filtro.BitResponderMaisTarde) ||
                    ((x.ProfessoresEncaminhados.Any(y => filtro.IdsProfessores.Any(z => z == y.intEmployeeID.Value))) || (x.BitEnviada && filtro.BitTodosProfessores)) ||
                    ((x.DenunciaAluno || x.Privada) && filtro.BitDenunciadas));

            if (filtro.IdsApostilas.Any() || filtro.BitTodasApostilas || provas.Any() || filtro.BitTodosConcursos || filtro.IdsSimulados.Any() || filtro.BitTodosSimulados)
                list = list.Where(x =>
                    ((filtro.IdsApostilas.Any(z => z == x.ExercicioId.Value) && x.TipoExercicioId == (int)Exercicio.tipoExercicio.APOSTILA) || (filtro.BitTodasApostilas && x.TipoExercicioId == (int)Exercicio.tipoExercicio.APOSTILA)) ||
                    ((provas.Any(z => z == x.ExercicioId.Value) && x.TipoExercicioId == (int)Exercicio.tipoExercicio.CONCURSO) || (filtro.BitTodosConcursos && x.TipoExercicioId == (int)Exercicio.tipoExercicio.CONCURSO)) ||
                    ((filtro.IdsSimulados.Any(z => z == x.ExercicioId.Value) && x.TipoExercicioId == (int)Exercicio.tipoExercicio.SIMULADO) || (filtro.BitTodosSimulados && x.TipoExercicioId == (int)Exercicio.tipoExercicio.SIMULADO)));

            if (filtro.IdsMateriais.Any() || filtro.BitTodosMateriais)
                list = list.Where(x => filtro.IdsMateriais.Any(y => y == x.ProductId.Value) || filtro.BitTodosMateriais && x.ProductId != null);

            return list;
        }

        public IList<DuvidaAcademicaContract> GetDuvidasConcurso()
        {
            using (var ctx = new DesenvContext())
            {
                var duvidas = ctx.tblDuvidasAcademicas_DuvidaQuestao
                                            .Where(w => w.intTipoExercicioID == (int)Exercicio.tipoExercicio.CONCURSO && w.tblDuvidasAcademicas_Duvidas.bitAtiva)
                                            .Select(s => new DuvidaAcademicaContract()
                                            {

                                                DuvidaId = s.intDuvidaID,
                                                QuestaoId = s.intDuvidaQuestaoId,
                                                ExercicioId = s.intExercicioId,
                                                TipoQuestaoId = s.intTipoQuestao,
                                                TipoExercicioId = s.intTipoExercicioID,
                                                NumeroQuestao = s.intNumQuestao
                                            }).ToList();
                return duvidas;
            }
        }

        public IList<DuvidasAcademicasProfessorDTO> GetDuvidasProfessor(DuvidaAcademicaFiltro filtro)
        {
            using (var ctx = new DesenvContext())
            {
                var dataAtual = DateTime.Now;

                var list = (from a in ctx.tblDuvidasAcademicas_Duvidas
                            join b in ctx.tblDuvidasAcademicas_DuvidaQuestao on a.intDuvidaID equals b.intDuvidaID into _ab
                            from ab in _ab.DefaultIfEmpty()
                            join c in ctx.tblDuvidasAcademicas_DuvidaApostila on a.intDuvidaID equals c.intDuvidaId into _ac
                            from ac in _ac.DefaultIfEmpty()
                            join d in ctx.tblMaterialApostila on ac.intMaterialApostilaId equals d.intID into _cd
                            from cd in _cd.DefaultIfEmpty()
                            join e in ctx.tblProducts on cd.intProductId equals e.intProductID into _ed
                            from ed in _ed.DefaultIfEmpty()
                            join bked in ctx.tblBooks on ed.intProductID equals bked.intBookID into _bed
                            from bed in _bed.DefaultIfEmpty()
                            join bken in ctx.tblBooks_Entities on bed.intBookEntityID equals bken.intID into _bken
                            from ben in _bken.DefaultIfEmpty()
                            join f in ctx.tblPersons on a.intClientID equals f.intContactID
                            join g in ctx.tblClients on f.intContactID equals g.intClientID

                            let AprovacaoMedGrupoLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                        where resp.bitAprovacaoMedgrupo == (int)TipoAprovacaoMedGrupo.Aprovado && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID
                                                        select resp).Any()
                            let RespostaMedGrupoLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                       where resp.bitRespostaMed == true && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID && resp.txtNomeFake == null && resp.txtEstadoFake == null
                                                       select resp).Any()
                            let MinhaDuvidaApostila = (from x in ctx.tblBooksEntitiesProfessor
                                                       join y in ctx.tblBooks on x.intBookEntitiesId equals y.intBookEntityID
                                                       where y.intBookID == ed.intProductID && x.intContactId == filtro.ClientId
                                                       select x).Any()
                            let MinhaDuvidaQuestaoConcurso = (
                                                              from dq in ctx.tblDuvidasAcademicas_DuvidaQuestao
                                                              join dqc in ctx.tblConcursoQuestao_Classificacao on dq.intQuestaoId equals dqc.intQuestaoID
                                                              join dqp in ctx.tblProducts on dqc.intClassificacaoID equals dqp.intProductID
                                                              join dqb in ctx.tblBooks on dqp.intProductID equals dqb.intBookID
                                                              join be in ctx.tblBooks_Entities on dqb.intBookEntityID equals be.intID
                                                              join dqpr in ctx.tblBooksEntitiesProfessor on dqb.intBookEntityID equals dqpr.intBookEntitiesId
                                                              where
                                                                dqc.intQuestaoID == ab.intQuestaoId &&
                                                                dq.intQuestaoId == ab.intQuestaoId &&
                                                              (ab.intTipoExercicioID == (int)Exercicio.tipoExercicio.CONCURSO || ab.intTipoExercicioID == (int)Exercicio.tipoExercicio.APOSTILA) &&
                                                                dqpr.intContactId == filtro.ClientId
                                                              select be.txtName
                                                              ).FirstOrDefault()
                            let MinhaDuvidaQuestaoConcursoEspecialidade = (from dq in ctx.tblDuvidasAcademicas_DuvidaQuestao
                                                                           join dqc in ctx.tblConcursoQuestao_Classificacao on dq.intQuestaoId equals dqc.intQuestaoID
                                                                           join dqp in ctx.tblMedsoft_Especialidade_Classificacao on dqc.intClassificacaoID equals dqp.intClassificacaoID
                                                                           join dqpr in ctx.tblEspecialidadeProfessor on dqp.intEspecialidadeID equals dqpr.intEspecialidadeID
                                                                           where
                                                                           dqc.intQuestaoID == ab.intQuestaoId &&
                                                                           dq.intQuestaoId == ab.intQuestaoId &&
                                                                           (ab.intTipoExercicioID == (int)Exercicio.tipoExercicio.CONCURSO || ab.intTipoExercicioID == (int)Exercicio.tipoExercicio.APOSTILA) &&
                                                                           dqpr.intContactID == filtro.ClientId
                                                                           select dqpr.txtEspecialidade
                                                                            ).FirstOrDefault()

                            let MinhaDuvidaSimulado = (from dq in ctx.tblDuvidasAcademicas_DuvidaQuestao
                                                       join dscc in ctx.tblConcursoQuestao_Classificacao on dq.intQuestaoConcursoID equals dscc.intQuestaoID
                                                       where dscc.intQuestaoID == ab.intQuestaoId
                                                       join dqp in ctx.tblProducts on dscc.intClassificacaoID equals dqp.intProductID
                                                       join dsb in ctx.tblBooks on dqp.intProductID equals dsb.intBookID
                                                       join be in ctx.tblBooks_Entities on dsb.intBookEntityID equals be.intID
                                                       join dspr in ctx.tblBooksEntitiesProfessor on dsb.intBookEntityID equals dspr.intBookEntitiesId
                                                       where
                                                         dscc.intQuestaoID == ab.intQuestaoId &&
                                                            dq.intQuestaoId == ab.intQuestaoId &&
                                                            ab.intTipoExercicioID == (int)Exercicio.tipoExercicio.SIMULADO &&
                                                            dspr.intContactId == filtro.ClientId
                                                       select be.txtName).FirstOrDefault()

                            let MinhaDuvidaSimuladoAnteriores = (from dq in ctx.tblDuvidasAcademicas_DuvidaQuestao
                                                                 join dspr in ctx.tblEspecialidadeProfessor on dq.intEspecialidadeID equals dspr.intEspecialidadeID
                                                                 where
                                                                      dq.intQuestaoId == ab.intQuestaoId &&
                                                                      ab.intTipoExercicioID == (int)Exercicio.tipoExercicio.SIMULADO &&
                                                                      dspr.intContactID == filtro.ClientId
                                                                 select dspr.txtEspecialidade).FirstOrDefault()
                            let Nome = (from p in ctx.tblPersons
                                        where p.intContactID == filtro.ClientId
                                        select p.txtName).FirstOrDefault()

                            let PrimeirasDuvidas = filtro.IsAcademico ? ctx.tblDuvidasAcademicas_Duvidas.Count(x => x.intClientID == a.intClientID && x.bitAtiva && x.bitAtivaDesenv == true && a.dteDataCriacao > x.dteDataCriacao) < 5 : false
                            let Encaminhada = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Any(x => x.intDuvidaID == a.intDuvidaID && filtro.ClientId == x.intEmployeeID)
                            let Enviada = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Any(x => x.intDuvidaID == a.intDuvidaID && filtro.ClientId == x.intGestorID)
                            let EntidadeApostila = ac != null ? ac.intMaterialApostilaId : 0
                            let TipoExercicio = ab != null ? ab.intTipoExercicioID : 0
                            let Data = a.dteDataCriacao.Day.ToString() + "/" + a.dteDataCriacao.Month.ToString() + "/" + a.dteDataCriacao.Year.ToString()
                            where
                            a.bitAtivaDesenv != true ? a.intClientID == filtro.ClientId && a.bitAtiva : a.bitAtiva
                            select new DuvidasAcademicasProfessorDTO
                            {
                                DuvidaId = a.intDuvidaID,
                                QuestaoId = ab.intQuestaoId,
                                DataOriginal = a.dteDataCriacao,
                                Data = Data,
                                ApostilaId = ac.intMaterialApostilaId,
                                EntidadeConcurso = MinhaDuvidaQuestaoConcurso.Trim(),
                                EntidadeSimulado = MinhaDuvidaSimulado.Trim(),
                                EspecialidadeConcurso = "ORIGINAIS " + MinhaDuvidaQuestaoConcursoEspecialidade.Trim(),
                                EspecialidadeSimulado = "ORIGINAIS " + MinhaDuvidaSimuladoAnteriores.Trim(),
                                Professor = Nome.Trim(),
                                IdProfessor = filtro.ClientId,
                                TextoDuvida = a.txtDescricao,
                                TextoQuestao = a.txtDescricao,
                                TipoExercicioId = ab.intTipoExercicioID,
                                PrimeirasDuvidas = PrimeirasDuvidas,
                                MinhaDuvidaApostila = MinhaDuvidaApostila,
                                MinhaDuvidaQuestaoConcurso = MinhaDuvidaQuestaoConcurso != null,
                                MinhaDuvidaSimulado = MinhaDuvidaSimulado != null,
                                MinhaDuvidaQuestaoConcursoEspecialidade = MinhaDuvidaQuestaoConcursoEspecialidade != null,
                                MinhaDuvidaSimuladoAnteriores = MinhaDuvidaSimuladoAnteriores != null,
                                EntidadeApostilaDescricao = ben.txtName.Trim(),
                                AprovacaoMedGrupo = AprovacaoMedGrupoLet,
                                RespostaMedGrupo = RespostaMedGrupoLet,
                                BitEncaminhada = Encaminhada,
                                BitEnviada = Enviada,
                                EntidadeApostila = EntidadeApostila,
                            });

                list = AplicarFiltroProfessor(list, filtro);

                return list.ToList();
            }
        }

        private IQueryable<DuvidasAcademicasProfessorDTO> AplicarFiltroProfessor(IQueryable<DuvidasAcademicasProfessorDTO> list, DuvidaAcademicaFiltro filtro)
        {
            list = list.Where(x =>
                (
                    (x.MinhaDuvidaApostila && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value) ||
                    (x.ApostilaId == 0 && x.QuestaoId == 0 && filtro.IsCoordenador && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value && !x.BitEnviada) ||
                    (x.MinhaDuvidaQuestaoConcurso && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value) ||
                    (x.MinhaDuvidaSimulado && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value) ||
                    (x.ApostilaId == null && x.QuestaoId == null && filtro.IsCoordenador && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value && !x.BitEnviada) ||
                    (x.MinhaDuvidaSimuladoAnteriores && !x.MinhaDuvidaSimulado && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value) ||
                    (x.MinhaDuvidaQuestaoConcursoEspecialidade && x.TipoExercicioId == (int)Exercicio.tipoExercicio.CONCURSO && !x.MinhaDuvidaQuestaoConcurso && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value) ||
                    (x.BitEncaminhada && !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value) && filtro.MinhasApostilas));

            return list;
        }

        private List<string> GetEspecialidadeSimulado(int QuestaoId, int TipoExercicio, int ClientId)
        {
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    List<int?> listaSimuladoEspecialidade = (from dq in ctxMatDir.tblDuvidasAcademicas_DuvidaQuestao
                                                             join dspr in ctxMatDir.tblEspecialidadeProfessor on dq.intEspecialidadeID equals dspr.intEspecialidadeID
                                                             where dq.intQuestaoId == QuestaoId &&
                                                                  TipoExercicio == (int)Exercicio.tipoExercicio.SIMULADO &&
                                                                  dspr.intContactID == ClientId
                                                             select dq.intEspecialidadeID).ToList();

                    List<string> listaEspecialidade = (from e in ctx.tblEspecialidades
                                                       where listaSimuladoEspecialidade.Any(f => f == e.intEspecialidadeID)
                                                       select e.DE_ESPECIALIDADE).ToList();

                    return new List<string>();
                }
            }
        }

        private List<string> GetEspecialidadeConcurso(int QuestaoId, int ContactId)
        {
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    List<int> listaconcursoEspecialidade = (from dq in ctxMatDir.tblDuvidasAcademicas_DuvidaQuestao
                                                            join dqc in ctxMatDir.tblConcursoQuestao_Classificacao on dq.intQuestaoId equals dqc.intQuestaoID
                                                            join dqp in ctxMatDir.tblMedsoft_Especialidade_Classificacao on dqc.intClassificacaoID equals dqp.intClassificacaoID
                                                            join dqpr in ctxMatDir.tblEspecialidadeProfessor on dqp.intEspecialidadeID equals dqpr.intEspecialidadeID
                                                            where
                                                            dq.intQuestaoId == QuestaoId &&
                                                            dqpr.intContactID == ContactId
                                                            select dqp.intEspecialidadeID).ToList();

                    List<string> listaEspecialidade = (from e in ctx.tblEspecialidades
                                                       where listaconcursoEspecialidade.Any(f => f == e.intEspecialidadeID)
                                                       select e.DE_ESPECIALIDADE).ToList();

                    return listaEspecialidade;
                }
            }
        }

        public List<CronogramaSimplificadoDTO> GetExerciciosDuvidasQuestao()
        {
            using (var ctx = new DesenvContext())
            {

                var materiais = (from a in ctx.tblDuvidasAcademicas_Duvidas
                                 join b in ctx.tblDuvidasAcademicas_DuvidaQuestao on a.intDuvidaID equals b.intDuvidaID
                                 join c in ctx.tblBooks_Entities on b.intExercicioId.Value equals c.intID
                                 where a.bitAtiva
                                 group a by new { c.txtName, c.intID } into g
                                 select new CronogramaSimplificadoDTO
                                 {
                                     Nome = g.Key.txtName,
                                     IdEntidade = (int)g.Key.intID
                                 }).ToList();
                return materiais;
            }
        }

        public tblDuvidasAcademicas_Interacoes GetInteracao(DuvidaAcademicaInteracao interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    int duvidaId = Convert.ToInt32(interacao.DuvidaId);

                    return ctx.tblDuvidasAcademicas_Interacoes
                                .FirstOrDefault(x => x.intClientID == interacao.ClientId
                                          && (x.intDuvidaId == duvidaId || x.intRespostaId == interacao.RespostaId)
                                          && ((interacao.RespostaId != null && (interacao.TipoInteracao == (int)TipoInteracaoDuvida.Upvote || interacao.TipoInteracao == (int)TipoInteracaoDuvida.Downvote)) ? x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Upvote || x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Downvote : x.intTipoInteracaoId == interacao.TipoInteracao));
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public tblDuvidasAcademicas_Interacoes GetInteracaoResposta(DuvidaAcademicaInteracao interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    return ctx.tblDuvidasAcademicas_Interacoes
                                .FirstOrDefault(x => x.intClientID == interacao.ClientId && x.intRespostaId == interacao.RespostaId);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public tblDuvidasAcademicas_Interacoes GetInteracaoUpVoteDuvida(DuvidaAcademicaInteracao interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    int duvidaId = Convert.ToInt32(interacao.DuvidaId);
                    return ctx.tblDuvidasAcademicas_Interacoes
                                .FirstOrDefault(x => x.intClientID == interacao.ClientId && x.intDuvidaId == duvidaId && x.intVote == (int)TipoVoto.Upvote);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public List<CronogramaSimplificadoDTO> GetProdutoIdDuvidasApostila()
        {
            using (var ctx = new DesenvContext())
            {
                var materiais = (from a in ctx.tblDuvidasAcademicas_Duvidas
                                 join b in ctx.tblDuvidasAcademicas_DuvidaApostila on a.intDuvidaID equals b.intDuvidaId
                                 join c in ctx.tblMaterialApostila on b.intMaterialApostilaId equals c.intID
                                 join d in ctx.tblProducts on c.intProductId equals d.intProductID
                                 join e in ctx.tblBooks on d.intProductID equals e.intBookID
                                 join f in ctx.tblBooks_Entities on e.intBookEntityID equals f.intID
                                 join g in ctx.tblBooksEntitiesProfessor on f.intID equals g.intBookEntitiesId
                                 where a.bitAtiva && e.intBookID == d.intProductID && g.intBookEntitiesId == e.intBookEntityID
                                 group a by new { f.txtName, c.intProductId, e.intBookEntityID } into g
                                 select new CronogramaSimplificadoDTO
                                 {
                                     Nome = g.Key.txtName,
                                     IdEntidade = (int)g.Key.intBookEntityID,
                                     MaterialId = g.Key.intProductId.Value
                                 }).ToList();
                return materiais;
            }
        }

        public List<AcademicoDADTO> GetProfessores()
        {
            using (var ctx = new DesenvContext())
            {
                var professores = (from p in ctx.tblPersons
                                   join g in ctx.tblPessoaGrupo on p.intContactID equals g.intContactID
                                   select new AcademicoDADTO
                                   {
                                       Id = p.intContactID,
                                       Nome = p.txtName.Trim(),
                                       Register = p.txtRegister,
                                       Email = p.txtEmail1,
                                       Perfil = EnumTipoPerfil.Professor
                                   })
                                     .ToList()
                                     .GroupBy(x => x.Id)
                                     .Select(x => x.FirstOrDefault())
                                     .ToList();
                return professores;
            }
        }

        public List<PessoaGrupoDTO> GetProfessoresDuvidasEncaminhadas(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                var professores = (from a in ctx.tblDuvidasAcademicas_DuvidasEncaminhadas
                                   join b in ctx.tblEmployees on a.intEmployeeID equals b.intEmployeeID
                                   join c in ctx.tblPersons on b.intEmployeeID equals c.intContactID
                                   join d in ctx.tblDuvidasAcademicas_Duvidas on a.intDuvidaID equals d.intDuvidaID
                                   where a.intEmployeeID != matricula && d.bitAtiva
                                   select new PessoaGrupoDTO
                                   {
                                       ContactID = b.intEmployeeID,
                                       Nome = c.txtName,
                                       Register = c.txtRegister.Trim()
                                   }).ToList();

                return professores;
            }
        }

        public QuestaoAcademicoDTO GetQuestaoConcurso(int questaoId)
        {
            using (var ctx = new AcademicoContext())
            {
                var questaoAcademico = ctx.tblQuestoes.Where(x => x.intQuestaoID == questaoId).FirstOrDefault();
                if (questaoAcademico != null)
                {
                    var questao = new QuestaoAcademicoDTO()
                    {
                        QuestaoId = questaoAcademico.intQuestaoID,
                        EspecialidadeId = questaoAcademico.intEspecialidadeID,
                        QuestaoConcursoId = questaoAcademico.intQuestaoConcursoID
                    };
                    return questao;
                }
                return new QuestaoAcademicoDTO();
            }
        }

        public DuvidaAcademicaContract GetReplica(DuvidaAcademicaFiltro filtro)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var date = DateTime.Now;
                    var list = (from a in ctx.tblDuvidasAcademicas_Resposta
                                join b in ctx.tblPersons on a.intClientID equals b.intContactID
                                join c in ctx.tblClients on b.intContactID equals c.intClientID
                                where
                                    a.bitAtiva &&
                                    a.intParentRespostaID == filtro.RespostaId
                                select new DuvidaAcademicaContract
                                {
                                    NomeAluno = b.txtName,
                                    ClientId = a.intClientID,
                                    CursoAluno = a.txtCurso,
                                    Descricao = a.txtDescricao,
                                    EstadoAluno = a.txtEstadoFilial,
                                    UpVotes = ctx.tblDuvidasAcademicas_Interacoes.Count(x => x.intRespostaId == a.intRespostaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Upvote && x.intVote == (int)TipoVoto.Upvote),
                                    VotadoUpvote = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intRespostaId == a.intRespostaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Upvote && x.intVote == (int)TipoVoto.Upvote && x.intClientID == filtro.ClientId),
                                    DownVotes = ctx.tblDuvidasAcademicas_Interacoes.Count(x => x.intRespostaId == a.intRespostaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Downvote && x.intVote == (int)TipoVoto.Downvote),
                                    VotadoDownvote = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intRespostaId == a.intRespostaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Downvote && x.intVote == (int)TipoVoto.Downvote && x.intClientID == filtro.ClientId),
                                    Dono = a.intClientID == filtro.ClientId,
                                    Editada = a.dteAtualizacao > a.dteDataCriacao,
                                    Denuncia = ctx.tblDuvidasAcademicas_Denuncia.Any(x => x.intContactID == filtro.ClientId && x.intRespostaID == a.intRespostaID) ||
                                            ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.bitDenuncia == true && filtro.ClientId == x.intClientID && x.intRespostaId == a.intRespostaID),
                                    AprovacaoMedGrupo = a.bitAprovacaoMedgrupo == (int)TipoAprovacaoMedGrupo.Aprovado,
                                    RespostaMedGrupo = a.bitRespostaMed == true,
                                    NomeAlunoCompleto = b.txtName,
                                    EstadoFake = a.txtEstadoFake,
                                    NomeFake = a.txtNomeFake,
                                    RespostaId = a.intRespostaID,
                                    DataCriacao = a.dteDataCriacao,
                                    MedGrupoId = a.intMedGrupoID,
                                    ObservacaoMedGrupo = a.txtObservacao
                                });

                    list = list.OrderByDescending(x => x.RespostaId);
                    var replica = list.FirstOrDefault();


                    return replica;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DuvidaAcademicaReplicaResponse GetReplicasResposta(DuvidaAcademicaFiltro filtro)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var date = DateTime.Now;
                    var list = (from a in ctx.tblDuvidasAcademicas_Resposta
                                join b in ctx.tblPersons on a.intClientID equals b.intContactID
                                join c in ctx.tblClients on b.intContactID equals c.intClientID
                                where
                                    (a.bitAtivaDesenv != true ? a.intClientID == filtro.ClientId && a.bitAtiva : a.bitAtiva) && a.intParentRespostaID == filtro.RespostaId &&
                                    a.intParentRespostaID == filtro.RespostaId
                                select new DuvidaAcademicaContract
                                {
                                    NomeAluno = b.txtName,
                                    ClientId = a.intClientID,
                                    CursoAluno = a.txtCurso,
                                    EstadoAluno = a.txtEstadoFilial,
                                    Descricao = a.txtDescricao,
                                    Privada = !a.bitAtivaDesenv.Value,
                                    UpVotes = ctx.tblDuvidasAcademicas_Interacoes.Count(x => x.intRespostaId == a.intRespostaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Upvote && x.intVote == (int)TipoVoto.Upvote),
                                    VotadoUpvote = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intRespostaId == a.intRespostaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Upvote && x.intVote == (int)TipoVoto.Upvote && x.intClientID == filtro.ClientId),
                                    DownVotes = ctx.tblDuvidasAcademicas_Interacoes.Count(x => x.intRespostaId == a.intRespostaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Downvote && x.intVote == (int)TipoVoto.Downvote),
                                    VotadoDownvote = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intRespostaId == a.intRespostaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Downvote && x.intVote == (int)TipoVoto.Downvote && x.intClientID == filtro.ClientId),
                                    Dono = a.intClientID == filtro.ClientId,
                                    Editada = a.dteAtualizacao > a.dteDataCriacao,
                                    Denuncia = ctx.tblDuvidasAcademicas_Denuncia.Any(x => x.intContactID == filtro.ClientId && x.intRespostaID == a.intRespostaID) ||
                                                ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.bitDenuncia == true && filtro.ClientId == x.intClientID && x.intRespostaId == a.intRespostaID),
                                    AprovacaoMedGrupo = a.bitAprovacaoMedgrupo == (int)TipoAprovacaoMedGrupo.Aprovado && a.txtNomeFake == null && a.txtEstadoFake == null,
                                    RespostaMedGrupo = a.bitRespostaMed == true && a.txtNomeFake == null && a.txtEstadoFake == null,
                                    NomeAlunoCompleto = b.txtName,
                                    RespostaId = a.intRespostaID,
                                    DataCriacao = a.dteDataCriacao,
                                    MedGrupoId = a.intMedGrupoID,
                                    EstadoFake = a.txtEstadoFake,
                                    NomeFake = a.txtNomeFake,
                                    ObservacaoMedGrupo = a.txtObservacao
                                });

                    list = list
                        .OrderBy(x => x.DataCriacao)
                        .ThenByDescending(x => x.VotadoUpvote)
                        .ThenByDescending(x => x.RespostaId);

                    if (filtro.Page > 0)
                    {
                        list = list.Skip((filtro.Page - 1) * filtro.QuantidadeReplicas).Take(filtro.QuantidadeReplicas);
                    }

                    var result = new DuvidaAcademicaReplicaResponse
                    {
                        QuantidadeReplicas = ctx.tblDuvidasAcademicas_Resposta.Where(x => x.intParentRespostaID == filtro.RespostaId && x.bitAtiva).Count() - (filtro.Page * filtro.QuantidadeReplicas),
                        Replicas = list.ToList()
                    };

                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IList<DuvidaAcademicaDTO> GetResolvidosProfessor(int idProfessor)
        {
            using (var ctx = new DesenvContext())
            {
                var duvidasResolvidas = ctx.tblDuvidasAcademicas_Resposta
                    .Where(x => x.bitAtiva && x.bitRespostaMed == true && x.intClientID == idProfessor)
                    .GroupBy(x => new { x.intDuvidaID, x.dteDataCriacao })
                    .Select(y => new DuvidaAcademicaDTO()
                    {
                        DuvidaId = y.Key.intDuvidaID,
                        DataCriacao = y.Key.dteDataCriacao
                    }).ToList();
                return duvidasResolvidas;
            }
        }

        public tblDuvidasAcademicas_Resposta GetResposta(int idResposta)
        {
            using (var ctx = new DesenvContext())
            {
                var resposta = ctx.tblDuvidasAcademicas_Resposta.FirstOrDefault(x => x.intRespostaID == idResposta);
                if (resposta != null)
                {
                    return resposta;
                }
                return null;
            }
        }

        public IList<DuvidaAcademicaContract> GetRespostasPorDuvida(DuvidaAcademicaFiltro filtro)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    int duvidaId = Convert.ToInt32(filtro.DuvidaId);
                    var list = (from a in ctx.tblDuvidasAcademicas_Resposta
                                join b in ctx.tblPersons on a.intClientID equals b.intContactID
                                join c in ctx.tblClients on b.intContactID equals c.intClientID
                                where
                                    (a.bitAtivaDesenv != true ? a.intClientID == filtro.ClientId && a.bitAtiva : a.bitAtiva) &&
                                    a.intDuvidaID == duvidaId &&
                                    a.intParentRespostaID == null
                                select new DuvidaAcademicaContract
                                {
                                    NomeAluno = b.txtName.Substring(0, 1),
                                    ClientId = a.intClientID,
                                    DuvidaId = a.intDuvidaID,
                                    CursoAluno = a.txtCurso,
                                    Privada = !a.bitAtivaDesenv.Value,
                                    EstadoAluno = a.txtEstadoFilial,
                                    NomeFake = a.txtNomeFake,
                                    EstadoFake = a.txtEstadoFake,
                                    Descricao = a.txtDescricao,
                                    UpVotes = ctx.tblDuvidasAcademicas_Interacoes.Count(x => x.intRespostaId == a.intRespostaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Upvote && x.intVote == (int)TipoVoto.Upvote),
                                    VotadoUpvote = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intRespostaId == a.intRespostaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Upvote && x.intVote == (int)TipoVoto.Upvote && x.intClientID == filtro.ClientId),
                                    DownVotes = ctx.tblDuvidasAcademicas_Interacoes.Count(x => x.intRespostaId == a.intRespostaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Downvote && x.intVote == (int)TipoVoto.Downvote),
                                    VotadoDownvote = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intRespostaId == a.intRespostaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Downvote && x.intVote == (int)TipoVoto.Downvote && x.intClientID == filtro.ClientId),
                                    Dono = a.intClientID == filtro.ClientId && a.txtNomeFake == null && a.txtEstadoFake == null,
                                    Editada = a.dteAtualizacao > a.dteDataCriacao,
                                    Denuncia = ctx.tblDuvidasAcademicas_Denuncia.Any(x => x.intContactID == filtro.ClientId && x.intRespostaID == a.intRespostaID) ||
                                                ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.bitDenuncia == true && filtro.ClientId == x.intClientID && x.intRespostaId == a.intRespostaID),
                                    AprovacaoMedGrupo = a.bitAprovacaoMedgrupo == (int)TipoAprovacaoMedGrupo.Aprovado,
                                    RespostaMedGrupo = a.bitRespostaMed == true && a.txtNomeFake == null && a.txtEstadoFake == null,
                                    NomeAlunoCompleto = b.txtName,
                                    RespostaId = a.intRespostaID,
                                    DataCriacao = a.dteDataCriacao,
                                    MedGrupoId = a.intMedGrupoID,
                                    ObservacaoMedGrupo = a.txtObservacao
                                });

                    list = list
                            .OrderByDescending(x => x.AprovacaoMedGrupo)
                            .ThenByDescending(x => x.RespostaMedGrupo)
                            .ThenByDescending(x => x.UpVotes)
                            .ThenBy(x => x.RespostaId);

                    return list.ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetTrechoApostilaSelecionado(int duvidaId)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var duvida = ctx.tblDuvidasAcademicas_DuvidaApostila
                        .FirstOrDefault(x => x.intDuvidaId == duvidaId);

                    return duvida.txtTrecho;
                }
                catch
                {
                    throw;
                }
            }
        }

        public bool HasRespostaHomologada(int duvidaID)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var hasHomologada = ctx.tblDuvidasAcademicas_Resposta.Any(x => x.intDuvidaID == duvidaID && x.bitAprovacaoMedgrupo == 1 && x.bitAtiva);
                    return hasHomologada;

                }
                catch
                {
                    throw;
                }
            }
        }

        public bool InsertDenuncia(DenunciaDuvidasAcademicasDTO obj)
        {
            using (var ctx = new DesenvContext())
            {
                var denuncia = new tblDuvidasAcademicas_Denuncia
                {
                    intContactID = obj.ClientId,
                    intDuvidaID = obj.DuvidaId,
                    intRespostaID = obj.RespostaId,
                    intTipoDenuncia = (int)obj.TipoDenuncia,
                    txtComplemento = obj.Comentario,
                    dteDataCriacao = DateTime.Now
                };
                ctx.tblDuvidasAcademicas_Denuncia.Add(denuncia);
                var result = ctx.SaveChanges();
                return result > 0;
            }
        }

        public int InsertDuvida(DuvidaAcademicaInteracao interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var duvida = new tblDuvidasAcademicas_Duvidas
                    {
                        txtCurso = interacao.CursoAluno,
                        intClientID = interacao.ClientId,
                        txtDescricao = interacao.Descricao,
                        dteDataCriacao = DateTime.Now,
                        bitAtiva = interacao.BitAtiva != null ? (bool)interacao.BitAtiva : true,
                        bitAtivaDesenv = true,
                        dteAtualizacao = DateTime.Now,
                        txtEstadoFilial = interacao.EstadoAluno,
                        txtNomeFake = interacao.NomeFake,
                        txtEstadoFake = interacao.EstadoFake
                    };

                    ctx.tblDuvidasAcademicas_Duvidas.Add(duvida);
                    ctx.SaveChanges();
                    return duvida.intDuvidaID;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int InsertDuvidaApostila(DuvidaAcademicaInteracao interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var duvida = new tblDuvidasAcademicas_Duvidas
                    {
                        txtCurso = interacao.CursoAluno,
                        intClientID = interacao.ClientId,
                        txtDescricao = interacao.Descricao,
                        dteDataCriacao = DateTime.Now,
                        bitAtiva = interacao.BitAtiva != null ? (bool)interacao.BitAtiva : true,
                        dteAtualizacao = DateTime.Now,
                        txtOrigem = interacao.Origem,
                        bitAtivaDesenv = true,
                        txtOrigemSubnivel = interacao.OrigemSubnivel,
                        txtEstadoFilial = interacao.EstadoAluno,
                        txtNomeFake = interacao.NomeFake,
                        txtEstadoFake = interacao.EstadoFake,
                        txtOrigemProduto = (interacao.OrigemProduto == null || interacao.OrigemProduto.Trim() == "") ? null : interacao.OrigemProduto
                    };

                    ctx.tblDuvidasAcademicas_DuvidaApostila.Add(new tblDuvidasAcademicas_DuvidaApostila
                    {
                        intMaterialApostilaId = interacao.ApostilaId.Value,
                        intNumCapitulo = Convert.ToInt32(interacao.NumeroCapitulo),
                        txtTrecho = interacao.TrechoApostila,
                        txtCodigoMarcacao = interacao.CodigoMarcacao,
                        intTipoCategoria = (int)interacao.TipoCategoria,
                        tblDuvidasAcademicas_Duvidas = duvida
                    });

                    ctx.SaveChanges();
                    return duvida.intDuvidaID;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public int InsertDuvidaLida(tblDuvidasAcademicas_Lidas entity)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var duvida = ctx.tblDuvidasAcademicas_Lidas.FirstOrDefault(x => x.intDuvidaID == entity.intDuvidaID && x.intClientID == entity.intClientID);
                    if (duvida == null)
                        ctx.tblDuvidasAcademicas_Lidas.Add(entity);

                    return ctx.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        public int InsertDuvidaQuestao(DuvidaAcademicaInteracao interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var duvida = new tblDuvidasAcademicas_Duvidas
                    {
                        txtCurso = interacao.CursoAluno,
                        intClientID = interacao.ClientId,
                        txtDescricao = interacao.Descricao,
                        dteDataCriacao = DateTime.Now,
                        bitAtiva = interacao.BitAtiva != null ? (bool)interacao.BitAtiva : true,
                        bitAtivaDesenv = true,
                        dteAtualizacao = DateTime.Now,
                        txtOrigem = interacao.Origem,
                        txtOrigemSubnivel = interacao.OrigemSubnivel,
                        txtEstadoFilial = interacao.EstadoAluno,
                        txtNomeFake = interacao.NomeFake,
                        txtEstadoFake = interacao.EstadoFake,
                        txtOrigemProduto = (interacao.OrigemProduto == null || interacao.OrigemProduto.Trim() == "") ? null : interacao.OrigemProduto
                    };
                    var numquestao = interacao.OrigemSubnivel.Split()[1];

                    var duvidaQuestao = new tblDuvidasAcademicas_DuvidaQuestao
                    {
                        intQuestaoId = Convert.ToInt32(interacao.QuestaoId),
                        intExercicioId = interacao.ExercicioId,
                        intTipoExercicioID = interacao.TipoExercicioId,
                        intTipoQuestao = interacao.TipoQuestaoId,
                        intNumQuestao = interacao.NumeroQuestao,
                        intQuestaoConcursoID = interacao.ConcursoQuestaoId,
                        intEspecialidadeID = interacao.EspecialidadeId,
                        txtOrigemQuestaoConcurso = (interacao.OrigemQuestaoConcurso == null || interacao.OrigemQuestaoConcurso.Trim() == "") ? null : interacao.OrigemQuestaoConcurso,
                        tblDuvidasAcademicas_Duvidas = duvida
                    };

                    ctx.tblDuvidasAcademicas_DuvidaQuestao.Add(duvidaQuestao);
                    ctx.SaveChanges();
                    return duvida.intDuvidaID;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int InsertDuvidasEncaminhadas(DuvidaAcademicaInteracao duvidaInteracao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    foreach (var idProfessor in duvidaInteracao.ProfessoresSelecionados)
                    {
                        var obj = new tblDuvidasAcademicas_DuvidasEncaminhadas
                        {
                            intDuvidaID = Convert.ToInt32(duvidaInteracao.DuvidaId),
                            intGestorID = duvidaInteracao.ClientId,
                            intEmployeeID = idProfessor,
                            dteDataEncaminhamento = DateTime.Now
                        };
                        ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Add(obj);
                    }
                    return ctx.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        public int InsertInteracao(DuvidaAcademicaInteracao interacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {

                    var obj = new tblDuvidasAcademicas_Interacoes
                    {
                        bitDenuncia = interacao.TipoInteracao == (int)TipoInteracaoDuvida.Denuncia,
                        bitFavorita = interacao.TipoInteracao == (int)TipoInteracaoDuvida.Favorita,
                        intVote = interacao.TipoInteracao == (int)TipoInteracaoDuvida.Upvote ? (int)TipoVoto.Upvote : (int)TipoVoto.Downvote,
                        intClientID = interacao.ClientId,
                        dteCriacao = DateTime.Now,
                        intDuvidaId = Convert.ToInt32(interacao.DuvidaId),
                        intTipoInteracaoId = interacao.TipoInteracao.Value
                    };

                    if (interacao.RespostaId != null)
                    {
                        obj.intRespostaId = interacao.RespostaId;
                        if (interacao.TipoInteracao == (int)TipoInteracaoDuvida.Upvote)
                        {
                            obj.intVote = (int)TipoVoto.Upvote;
                        }

                        if (interacao.TipoInteracao == (int)TipoInteracaoDuvida.Downvote)
                        {
                            obj.intVote = (int)TipoVoto.Downvote;
                        }
                    }

                    ctx.tblDuvidasAcademicas_Interacoes.Add(obj);
                    return ctx.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int InsertRespostaReplica(DuvidaAcademicaInteracao interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var obj = new tblDuvidasAcademicas_Resposta
                    {
                        txtCurso = interacao.CursoAluno,
                        intDuvidaID = Convert.ToInt32(interacao.DuvidaId),
                        intClientID = interacao.ClientId,
                        txtDescricao = interacao.Descricao,
                        dteDataCriacao = DateTime.Now,
                        bitRespostaMed = interacao.RespostaMedGrupo,
                        intParentRespostaID = interacao.RespostaParentId,
                        dteAtualizacao = DateTime.Now,
                        bitAtiva = true,
                        bitAtivaDesenv = true,
                        txtEstadoFilial = interacao.EstadoAluno,
                        txtNomeFake = interacao.NomeFake,
                        txtEstadoFake = interacao.EstadoFake
                    };

                    ctx.tblDuvidasAcademicas_Resposta.Add(obj);
                    ctx.SaveChanges();
                    return obj.intRespostaID;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<int> ListarUsuariosFavoritaramDuvida(int idDuvida, int donoDuvida, int clientId)
        {
            try
            {
                var lstClientId = new List<int>();

                using (var ctx = new DesenvContext())
                {
                    var interacoes = ctx.tblDuvidasAcademicas_Interacoes.Where(x => x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Favorita && x.intDuvidaId == idDuvida && x.intClientID != clientId && x.intClientID != donoDuvida).ToList();

                    if (interacoes.Any())
                        lstClientId = interacoes.Select(x => x.intClientID).ToList();
                    return lstClientId;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<int> ListarUsuariosResponderamDuvida(int idDuvida, int clientId)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var lstClientId = ctx.tblDuvidasAcademicas_Resposta.Where(x => x.intDuvidaID == idDuvida && x.intClientID != clientId)
                        .GroupBy(x => new { x.intClientID }).Select(y => y.Key.intClientID).ToList();

                    return lstClientId;
                }
            }
            catch
            {
                throw;
            }
        }

        public bool SetDuvidaAcademicaPrivada(DuvidasRespostaPrivadaDTO obj)
        {
            using (var ctx = new DesenvContext())
            {
                var duvida = ctx.tblDuvidasAcademicas_Duvidas.FirstOrDefault(x => x.intDuvidaID == obj.DuvidaId);
                duvida.bitAtivaDesenv = duvida.bitAtivaDesenv == null ? true : !duvida.bitAtivaDesenv;
                duvida.intAcademicoID = obj.ClientId;
                return ctx.SaveChanges() > 0;
            }
        }

        public int SetDuvidaArquivada(DuvidaAcademicaInteracao duvidaInteracao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    int duvidaId = Convert.ToInt32(duvidaInteracao.DuvidaId);
                    var entity = ctx.tblDuvidasAcademicas_Duvidas.FirstOrDefault(x => x.intDuvidaID == duvidaId);
                    if (entity != null)
                    {
                        var duvidaArquivada = new tblDuvidasAcademicas_DuvidasArquivadas()
                        {
                            intDuvidaID = entity.intDuvidaID,
                            intClientID = duvidaInteracao.ClientId,
                            dteDataCriacao = DateTime.Now,
                            bitRespMaisTarde = duvidaInteracao.BitResponderMaisTarde
                        };

                        ctx.tblDuvidasAcademicas_DuvidasArquivadas.Add(duvidaArquivada);
                    }
                    return ctx.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        public int SetRespostaHomologada(DuvidaAcademicaInteracao interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var entity = ctx.tblDuvidasAcademicas_Resposta.FirstOrDefault(x => x.intRespostaID == interacao.RespostaId);
                    if (entity != null && entity.bitAprovacaoMedgrupo != (int)TipoAprovacaoMedGrupo.Aprovado)
                    {
                        entity.bitAprovacaoMedgrupo = (int)TipoAprovacaoMedGrupo.Aprovado;
                        entity.intMedGrupoID = interacao.ClientId.ToString();
                    }
                    else
                    {
                        entity.bitAprovacaoMedgrupo = (int)TipoAprovacaoMedGrupo.Indefinido;
                        entity.intMedGrupoID = null;
                    }
                    return ctx.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        public bool SetRespostaReplicaPrivada(DuvidasRespostaPrivadaDTO obj)
        {
            using (var ctx = new DesenvContext())
            {
                var resp = ctx.tblDuvidasAcademicas_Resposta.FirstOrDefault(x => x.intRespostaID == obj.RespostaId);
                resp.bitAtivaDesenv = resp.bitAtivaDesenv == null ? true : !resp.bitAtivaDesenv;
                resp.intAcademicoID = obj.ClientId;
                return ctx.SaveChanges() > 0;
            }
        }

        public int UpdateDuvida(DuvidaAcademicaInteracao interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    int duvidaId = Convert.ToInt32(interacao.DuvidaId);
                    var entity = ctx.tblDuvidasAcademicas_Duvidas.FirstOrDefault(x => x.intDuvidaID == duvidaId);

                    ctx.tblDuvidasAcademicas_DuvidasHistorico.Add(new tblDuvidasAcademicas_DuvidasHistorico
                    {
                        intDuvidaID = entity.intDuvidaID,
                        txtDescricao = entity.txtDescricao,
                        dteAtualizacao = DateTime.Now
                    });

                    entity.txtDescricao = interacao.Descricao;
                    entity.dteAtualizacao = DateTime.Now;
                    entity.bitEditado = true;
                    entity.bitAtivaDesenv = interacao.BitAtivaDesenv;

                    ctx.SaveChanges();
                    return entity.intDuvidaID;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int UpdateObservacaoMedGrupo(DuvidaAcademicaInteracao interacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var entity = ctx.tblDuvidasAcademicas_Resposta.FirstOrDefault(x => x.intRespostaID == interacao.RespostaId);
                    if (entity != null)
                    {
                        entity.txtObservacao = interacao.ObservacaoMedgrupo;
                        entity.intMedGrupoID = interacao.ClientId.ToString();
                    }

                    return ctx.SaveChanges();
                }

            }
            catch
            {
                throw;
            }
        }

        public int UpdateRespostaReplica(DuvidaAcademicaInteracao interacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var entity = ctx.tblDuvidasAcademicas_Resposta.FirstOrDefault(x => x.intRespostaID == interacao.RespostaId);

                    ctx.tblDuvidasAcademicas_RespostaHistorico.Add(new tblDuvidasAcademicas_RespostaHistorico
                    {
                        intRespostaID = entity.intRespostaID,
                        txtDescricao = entity.txtDescricao,
                        dteAtualizacao = DateTime.Now
                    });

                    entity.bitAtivaDesenv = interacao.BitAtivaDesenv;
                    entity.txtDescricao = interacao.Descricao;
                    entity.dteAtualizacao = DateTime.Now;
                    entity.bitEditado = true;

                    ctx.SaveChanges();
                    return entity.intRespostaID;
                }
                catch
                {
                    throw;
                }
            }
        }

        public IList<DuvidaAcademicaContract> GetDuvidasAlunoQuestoes(DuvidaAcademicaFiltro filtro)
        {
            using (var ctx = new DesenvContext())
            {
                var dataAtual = DateTime.Now;
                var list = (from a in ctx.tblDuvidasAcademicas_Duvidas
                            join b in ctx.tblDuvidasAcademicas_DuvidaQuestao on a.intDuvidaID equals b.intDuvidaID
                            join f in ctx.tblPersons on a.intClientID equals f.intContactID
                            let TemRascunhoLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                  where
                                                  resp.intDuvidaID == a.intDuvidaID && resp.bitAtiva && resp.txtObservacao != null && (resp.bitAprovacaoMedgrupo == 0 || resp.bitAprovacaoMedgrupo == null)
                                                  select resp).Any()
                            let MinhasRespostasLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                      where resp.intClientID == filtro.ClientId && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID && resp.intParentRespostaID == null
                                                      select resp).Any()
                            let AprovacaoMedGrupoLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                        where resp.bitAprovacaoMedgrupo == (int)TipoAprovacaoMedGrupo.Aprovado && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID
                                                        select resp).Any()
                            let RespostaMedGrupoLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                       where resp.bitRespostaMed == true && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID && resp.txtNomeFake == null && resp.txtEstadoFake == null
                                                       select resp).Any()
                            let NumeroRespostasLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                      where
                                                      resp.intDuvidaID == a.intDuvidaID && resp.bitAtiva && resp.intParentRespostaID == null && resp.bitAtivaDesenv == true
                                                      select resp).Count()

                            let Favoritada = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intClientID == filtro.ClientId && x.bitFavorita == true && x.intDuvidaId == a.intDuvidaID)
                            let UpVotes = ctx.tblDuvidasAcademicas_Interacoes.Count(x => x.intDuvidaId == a.intDuvidaID && x.intVote == (int)TipoVoto.Upvote)
                            let VotadoUpvote = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intDuvidaId == a.intDuvidaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Upvote && x.intVote == (int)TipoVoto.Upvote && x.intClientID == filtro.ClientId)
                            let Denuncia = ctx.tblDuvidasAcademicas_Denuncia.Any(x => x.intContactID == filtro.ClientId && x.intDuvidaID == a.intDuvidaID)
                            let DenunciaAluno = ctx.tblDuvidasAcademicas_Denuncia.Any(x => x.intDuvidaID == a.intDuvidaID)

                            where
                            a.bitAtiva
                            select new DuvidaAcademicaContract
                            {
                                DuvidaId = a.intDuvidaID,
                                ExercicioId = b.intExercicioId,
                                Privada = !a.bitAtivaDesenv.Value,
                                Favorita = Favoritada,
                                UpVotes = UpVotes,
                                Denuncia = Denuncia,
                                DenunciaAluno = DenunciaAluno,
                                Descricao = a.txtDescricao,
                                EstadoAluno = a.txtEstadoFilial,
                                MaisDe7Dias = false,
                                PrimeirasDuvidas = false,
                                TemRascunho = TemRascunhoLet,
                                ClientId = a.intClientID,
                                BitAtiva = a.bitAtiva,
                                DataCriacao = a.dteDataCriacao,
                                BitResponderMaisTarde = false,
                                Editada = a.dteAtualizacao > a.dteDataCriacao,
                                ProfessoresEncaminhados = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Where(x => x.intGestorID == filtro.ClientId && a.intDuvidaID == x.intDuvidaID),
                                BitVisualizada = false,
                                BitEncaminhada = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Any(x => x.intDuvidaID == a.intDuvidaID && filtro.ClientId == x.intEmployeeID),
                                BitEnviada = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Any(x => x.intDuvidaID == a.intDuvidaID && filtro.ClientId == x.intGestorID),
                                NomeAluno = a.txtNomeFake == null ? f.txtName.Substring(0, 1) : a.txtNomeFake.Substring(0, 1),
                                NomeAlunoCompleto = f.txtName.Trim(),
                                NomeFake = a.txtNomeFake,
                                EstadoFake = a.txtEstadoFake,
                                Dono = a.intClientID == filtro.ClientId && a.txtNomeFake == null && a.txtEstadoFake == null,
                                QuestaoId = b.intQuestaoId,
                                MinhaDuvidaQuestaoConcurso = false,
                                MinhaDuvidaSimulado = false,
                                MinhaDuvidaQuestaoConcursoEspecialidade = false,
                                MinhaDuvidaSimuladoAnteriores = false,
                                NomeGestor = string.Empty,
                                Arquivada = false,
                                NRespostas = NumeroRespostasLet,
                                VotadoUpvote = VotadoUpvote,
                                NumeroQuestao = b.intNumQuestao,
                                CursoAluno = null,
                                MinhasRespostas = MinhasRespostasLet,
                                RespostaMedGrupo = RespostaMedGrupoLet,
                                AprovacaoMedGrupo = AprovacaoMedGrupoLet,
                                TipoQuestaoId = b.intTipoQuestao,
                                Origem = a.txtOrigem,
                                OrigemSubnivel = a.txtOrigemSubnivel,
                                TipoExercicioId = b.intTipoExercicioID,
                                Genero = f.intSex.Value
                            });

                list = AplicarFiltros(list, filtro);

                if (filtro.IsAcademico)
                {
                    list = list
                        .OrderByDescending(x => !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value)
                        .ThenByDescending(x => x.PrimeirasDuvidas)
                        .ThenByDescending(x => x.UpVotes)
                        .ThenBy(x => x.DuvidaId);

                }
                else if (filtro.ApostilaId > 0)
                {
                    list = list
                        .OrderBy(x => x.TipoCategoria)
                        .ThenBy(x => x.NumeroCapitulo)
                        .ThenByDescending(x => x.UpVotes)
                        .ThenByDescending(x => x.BitVisualizada)
                        .ThenByDescending(x => x.DuvidaId);
                }
                else
                {
                    list = list
                        .OrderByDescending(x => x.UpVotes)
                        .ThenByDescending(x => x.BitVisualizada)
                        .ThenByDescending(x => x.DuvidaId);
                }

                if (filtro.Page > 0)
                {
                    list = list.Skip((filtro.Page - 1) * QuantidadeDuvidasPadrao).Take(QuantidadeDuvidasPadrao);
                }

                return list.ToList();
            }
        }

        public IList<DuvidaAcademicaContract> GetDuvidasAlunoApostila(DuvidaAcademicaFiltro filtro)
        {
            using (var ctx = new DesenvContext())
            {
                var dataAtual = DateTime.Now;
                var list = (from a in ctx.tblDuvidasAcademicas_Duvidas
                            join c in ctx.tblDuvidasAcademicas_DuvidaApostila on a.intDuvidaID equals c.intDuvidaId
                            join d in ctx.tblMaterialApostila on c.intMaterialApostilaId equals d.intID
                            join f in ctx.tblPersons on a.intClientID equals f.intContactID

                            let MinhasRespostasLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                      where resp.intClientID == filtro.ClientId && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID && resp.intParentRespostaID == null
                                                      select resp).Any()
                            let AprovacaoMedGrupoLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                        where resp.bitAprovacaoMedgrupo == (int)TipoAprovacaoMedGrupo.Aprovado && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID
                                                        select resp).Any()
                            let RespostaMedGrupoLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                       where resp.bitRespostaMed == true && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID && resp.txtNomeFake == null && resp.txtEstadoFake == null
                                                       select resp).Any()
                            let NumeroRespostasLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                      where
                                                      resp.intDuvidaID == a.intDuvidaID && resp.bitAtiva && resp.intParentRespostaID == null && resp.bitAtivaDesenv == true
                                                      select resp).Count()

                            let TemRascunhoLet = false
                            let Mais7DiasLet = false
                            let MinhaDuvidaApostila = false
                            let MinhaDuvidaQuestaoConcurso = false
                            let MinhaDuvidaQuestaoConcursoEspecialidade = false
                            let MinhaDuvidaSimulado = false
                            let MinhaDuvidaSimuladoAnteriores = false
                            let NomeGestor = ""
                            let PrimeirasDuvidas = false

                            let Favoritada = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intClientID == filtro.ClientId && x.bitFavorita == true && x.intDuvidaId == a.intDuvidaID)
                            let UpVotes = ctx.tblDuvidasAcademicas_Interacoes.Count(x => x.intDuvidaId == a.intDuvidaID && x.intVote == (int)TipoVoto.Upvote)
                            let VotadoUpvote = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intDuvidaId == a.intDuvidaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Upvote && x.intVote == (int)TipoVoto.Upvote && x.intClientID == filtro.ClientId)
                            let Denuncia = ctx.tblDuvidasAcademicas_Denuncia.Any(x => x.intContactID == filtro.ClientId && x.intDuvidaID == a.intDuvidaID)
                            let DenunciaAluno = ctx.tblDuvidasAcademicas_Denuncia.Any(x => x.intDuvidaID == a.intDuvidaID)

                            where
                            a.bitAtiva
                            select new DuvidaAcademicaContract
                            {
                                DuvidaId = a.intDuvidaID,
                                ExercicioId = 0,
                                Privada = !a.bitAtivaDesenv.Value,
                                ProductId = d.intProductId,
                                TipoCategoriaApostila = c.intTipoCategoria,
                                NumeroCategoriaApostila = c.intNumCapitulo.ToString(),
                                Favorita = Favoritada,
                                UpVotes = UpVotes,
                                Denuncia = Denuncia,
                                DenunciaAluno = DenunciaAluno,
                                Descricao = a.txtDescricao,
                                TrechoSelecionado = c.txtTrecho,
                                EstadoAluno = a.txtEstadoFilial,
                                MaisDe7Dias = Mais7DiasLet,
                                PrimeirasDuvidas = PrimeirasDuvidas,
                                TemRascunho = TemRascunhoLet,
                                ClientId = a.intClientID,
                                BitAtiva = a.bitAtiva,
                                DataCriacao = a.dteDataCriacao,
                                BitResponderMaisTarde = false,
                                Editada = a.dteAtualizacao > a.dteDataCriacao,
                                ProfessoresEncaminhados = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Where(x => x.intGestorID == filtro.ClientId && a.intDuvidaID == x.intDuvidaID),
                                BitVisualizada = false,
                                BitEncaminhada = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Any(x => x.intDuvidaID == a.intDuvidaID && filtro.ClientId == x.intEmployeeID),
                                BitEnviada = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Any(x => x.intDuvidaID == a.intDuvidaID && filtro.ClientId == x.intGestorID),
                                NomeAluno = a.txtNomeFake == null ? f.txtName.Substring(0, 1) : a.txtNomeFake.Substring(0, 1),
                                NomeAlunoCompleto = f.txtName.Trim(),
                                NomeFake = a.txtNomeFake,
                                EstadoFake = a.txtEstadoFake,
                                Dono = a.intClientID == filtro.ClientId && a.txtNomeFake == null && a.txtEstadoFake == null,
                                QuestaoId = 0,
                                MinhaDuvidaApostila = filtro.MinhasApostilas ? MinhaDuvidaApostila : false,
                                MinhaDuvidaQuestaoConcurso = filtro.MinhasApostilas ? MinhaDuvidaQuestaoConcurso : false,
                                MinhaDuvidaSimulado = filtro.MinhasApostilas ? MinhaDuvidaSimulado : false,
                                MinhaDuvidaQuestaoConcursoEspecialidade = filtro.MinhasApostilas ? MinhaDuvidaQuestaoConcursoEspecialidade : false,
                                MinhaDuvidaSimuladoAnteriores = filtro.MinhasApostilas ? MinhaDuvidaSimuladoAnteriores : false,
                                NomeGestor = NomeGestor,
                                Arquivada = false,
                                NRespostas = NumeroRespostasLet,
                                VotadoUpvote = VotadoUpvote,
                                NumeroQuestao = 0,
                                CursoAluno = null,
                                MinhasRespostas = MinhasRespostasLet,
                                RespostaMedGrupo = RespostaMedGrupoLet,
                                AprovacaoMedGrupo = AprovacaoMedGrupoLet,
                                TipoQuestaoId = 0,
                                ApostilaId = c.intMaterialApostilaId,
                                NumeroCapitulo = c.intNumCapitulo,
                                TipoCategoria = c.intTipoCategoria.Value,
                                CodigoMarcacao = c.txtCodigoMarcacao,
                                Origem = a.txtOrigem,
                                OrigemSubnivel = a.txtOrigemSubnivel,
                                TipoExercicioId = 0,
                                Genero = f.intSex.Value
                            });

                list = AplicarFiltros(list, filtro);

                if (filtro.IsAcademico)
                {
                    list = list
                        .OrderByDescending(x => !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value)
                        .ThenByDescending(x => x.PrimeirasDuvidas)
                        .ThenByDescending(x => x.UpVotes)
                        .ThenBy(x => x.DuvidaId);

                }
                else if (filtro.ApostilaId > 0)
                {
                    list = list
                        .OrderBy(x => x.TipoCategoria)
                        .ThenBy(x => x.NumeroCapitulo)
                        .ThenByDescending(x => x.UpVotes)
                        .ThenByDescending(x => x.BitVisualizada)
                        .ThenByDescending(x => x.DuvidaId);
                }
                else
                {
                    list = list
                        .OrderByDescending(x => x.UpVotes)
                        .ThenByDescending(x => x.BitVisualizada)
                        .ThenByDescending(x => x.DuvidaId);
                }

                if (filtro.Page > 0)
                {
                    list = list.Skip((filtro.Page - 1) * QuantidadeDuvidasPadrao).Take(QuantidadeDuvidasPadrao);
                }

                return list.ToList();
            }
        }

        public IList<DuvidaAcademicaContract> GetDuvidasAluno(DuvidaAcademicaFiltro filtro)
        {
            using (var ctx = new DesenvContext())
            {
                var dataAtual = DateTime.Now;
                var list = (from a in ctx.tblDuvidasAcademicas_Duvidas
                            join b in ctx.tblDuvidasAcademicas_DuvidaQuestao on a.intDuvidaID equals b.intDuvidaID into _ab
                            from ab in _ab.DefaultIfEmpty()
                            join c in ctx.tblDuvidasAcademicas_DuvidaApostila on a.intDuvidaID equals c.intDuvidaId into _ac
                            from ac in _ac.DefaultIfEmpty()
                            join d in ctx.tblMaterialApostila on ac.intMaterialApostilaId equals d.intID into _cd
                            from cd in _cd.DefaultIfEmpty()
                            join e in ctx.tblProducts on cd.intProductId equals e.intProductID into _ed
                            from ed in _ed.DefaultIfEmpty()
                            join f in ctx.tblPersons on a.intClientID equals f.intContactID
                            join g in ctx.tblClients on f.intContactID equals g.intClientID

                            let MinhasRespostasLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                      where resp.intClientID == filtro.ClientId && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID && resp.intParentRespostaID == null
                                                      select resp).Any()
                            let AprovacaoMedGrupoLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                        where resp.bitAprovacaoMedgrupo == (int)TipoAprovacaoMedGrupo.Aprovado && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID
                                                        select resp).Any()
                            let RespostaMedGrupoLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                       where resp.bitRespostaMed == true && resp.bitAtiva && resp.intDuvidaID == a.intDuvidaID && resp.txtNomeFake == null && resp.txtEstadoFake == null
                                                       select resp).Any()
                            let NumeroRespostasLet = (from resp in ctx.tblDuvidasAcademicas_Resposta
                                                      where
                                                      resp.intDuvidaID == a.intDuvidaID && resp.bitAtiva && resp.intParentRespostaID == null && resp.bitAtivaDesenv == true
                                                      select resp).Count()

                            let TemRascunhoLet = false
                            let Mais7DiasLet = false
                            let MinhaDuvidaApostila = false
                            let MinhaDuvidaQuestaoConcurso = false
                            let MinhaDuvidaQuestaoConcursoEspecialidade = false
                            let MinhaDuvidaSimulado = false
                            let MinhaDuvidaSimuladoAnteriores = false
                            let NomeGestor = ""
                            let PrimeirasDuvidas = false

                            let Favoritada = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intClientID == filtro.ClientId && x.bitFavorita == true && x.intDuvidaId == a.intDuvidaID)
                            let UpVotes = ctx.tblDuvidasAcademicas_Interacoes.Count(x => x.intDuvidaId == a.intDuvidaID && x.intVote == (int)TipoVoto.Upvote)
                            let VotadoUpvote = ctx.tblDuvidasAcademicas_Interacoes.Any(x => x.intDuvidaId == a.intDuvidaID && x.intTipoInteracaoId == (int)TipoInteracaoDuvida.Upvote && x.intVote == (int)TipoVoto.Upvote && x.intClientID == filtro.ClientId)
                            let Denuncia = ctx.tblDuvidasAcademicas_Denuncia.Any(x => x.intContactID == filtro.ClientId && x.intDuvidaID == a.intDuvidaID)
                            let DenunciaAluno = ctx.tblDuvidasAcademicas_Denuncia.Any(x => x.intDuvidaID == a.intDuvidaID)


                            where
                            a.bitAtiva
                            select new DuvidaAcademicaContract
                            {
                                DuvidaId = a.intDuvidaID,
                                ExercicioId = ab.intExercicioId,
                                Privada = !a.bitAtivaDesenv.Value,
                                ProductId = cd.intProductId,
                                TipoCategoriaApostila = ac.intTipoCategoria,
                                NumeroCategoriaApostila = ac.intNumCapitulo.ToString(),
                                Favorita = Favoritada,
                                UpVotes = UpVotes,
                                Denuncia = Denuncia,
                                DenunciaAluno = DenunciaAluno,
                                Descricao = a.txtDescricao,
                                TrechoSelecionado = ac.txtTrecho,
                                EstadoAluno = a.txtEstadoFilial,
                                MaisDe7Dias = Mais7DiasLet,
                                PrimeirasDuvidas = PrimeirasDuvidas,
                                TemRascunho = TemRascunhoLet,
                                ClientId = a.intClientID,
                                BitAtiva = a.bitAtiva,
                                DataCriacao = a.dteDataCriacao,
                                BitResponderMaisTarde = false,
                                Editada = a.dteAtualizacao > a.dteDataCriacao,
                                ProfessoresEncaminhados = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Where(x => x.intGestorID == filtro.ClientId && a.intDuvidaID == x.intDuvidaID),
                                BitVisualizada = false,
                                BitEncaminhada = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Any(x => x.intDuvidaID == a.intDuvidaID && filtro.ClientId == x.intEmployeeID),
                                BitEnviada = ctx.tblDuvidasAcademicas_DuvidasEncaminhadas.Any(x => x.intDuvidaID == a.intDuvidaID && filtro.ClientId == x.intGestorID),
                                NomeAluno = a.txtNomeFake == null ? f.txtName.Substring(0, 1) : a.txtNomeFake.Substring(0, 1),
                                NomeAlunoCompleto = f.txtName.Trim(),
                                NomeFake = a.txtNomeFake,
                                EstadoFake = a.txtEstadoFake,
                                Dono = a.intClientID == filtro.ClientId && a.txtNomeFake == null && a.txtEstadoFake == null,
                                QuestaoId = ab.intQuestaoId,
                                MinhaDuvidaApostila = filtro.MinhasApostilas ? MinhaDuvidaApostila : false,
                                MinhaDuvidaQuestaoConcurso = filtro.MinhasApostilas ? MinhaDuvidaQuestaoConcurso : false,
                                MinhaDuvidaSimulado = filtro.MinhasApostilas ? MinhaDuvidaSimulado : false,
                                MinhaDuvidaQuestaoConcursoEspecialidade = filtro.MinhasApostilas ? MinhaDuvidaQuestaoConcursoEspecialidade : false,
                                MinhaDuvidaSimuladoAnteriores = filtro.MinhasApostilas ? MinhaDuvidaSimuladoAnteriores : false,
                                NomeGestor = NomeGestor,
                                Arquivada = false,
                                NRespostas = NumeroRespostasLet,
                                VotadoUpvote = VotadoUpvote,
                                NumeroQuestao = ab.intNumQuestao,
                                CursoAluno = null,
                                MinhasRespostas = MinhasRespostasLet,
                                RespostaMedGrupo = RespostaMedGrupoLet,
                                AprovacaoMedGrupo = AprovacaoMedGrupoLet,
                                TipoQuestaoId = ab.intTipoQuestao,
                                ApostilaId = ac.intMaterialApostilaId,
                                NumeroCapitulo = ac.intNumCapitulo,
                                TipoCategoria = ac.intTipoCategoria.Value,
                                CodigoMarcacao = ac.txtCodigoMarcacao,
                                Origem = a.txtOrigem,
                                OrigemSubnivel = a.txtOrigemSubnivel,
                                TipoExercicioId = ab.intTipoExercicioID,
                                Genero = f.intSex.Value
                            });

                list = AplicarFiltros(list, filtro);

                if (filtro.IsAcademico)
                {
                    list = list
                        .OrderByDescending(x => !x.AprovacaoMedGrupo.Value && !x.RespostaMedGrupo.Value)
                        .ThenByDescending(x => x.PrimeirasDuvidas)
                        .ThenByDescending(x => x.UpVotes)
                        .ThenBy(x => x.DuvidaId);

                }
                else if (filtro.ApostilaId > 0)
                {
                    list = list
                        .OrderBy(x => x.TipoCategoria)
                        .ThenBy(x => x.NumeroCapitulo)
                        .ThenByDescending(x => x.UpVotes)
                        .ThenByDescending(x => x.BitVisualizada)
                        .ThenByDescending(x => x.DuvidaId);
                }
                else
                {
                    list = list
                        .OrderByDescending(x => x.UpVotes)
                        .ThenByDescending(x => x.BitVisualizada)
                        .ThenByDescending(x => x.DuvidaId);
                }

                if (filtro.Page > 0)
                {
                    list = list.Skip((filtro.Page - 1) * QuantidadeDuvidasPadrao).Take(QuantidadeDuvidasPadrao);
                }

                return list.ToList();
            }
        }

        
    }
}