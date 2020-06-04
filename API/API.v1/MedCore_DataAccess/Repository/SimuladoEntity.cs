using System;
using MedCore_API.Academico;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;
using System.Linq;
using MedCore_DataAccess.Entidades;
using System.Collections.Generic;
using MedCore_DataAccess.Util;
using StackExchange.Profiling;
using Microsoft.EntityFrameworkCore;
using MedCore_DataAccess.Model;

namespace MedCore_DataAccess.Repository
{
    public class SimuladoEntity : ISimuladoData
    {
        public List<Simulado> GetAll()
        {
            using (var ctx = new AcademicoContext())
            {
                var simulados = (from s in ctx.tblSimulado
                                 join es in ctx.tblEspecialidadesSimulado on s.intSimuladoID equals es.intSimuladoID
                                 join e in ctx.tblEspecialidades on es.intEspecialidadeID equals e.intEspecialidadeID
                                 select new Simulado()
                                 {
                                     ID = s.intSimuladoID,
                                     Nome = s.txtSimuladoName != null ? s.txtSimuladoName.Trim() : null,
                                     Descricao = s.txtSimuladoDescription != null ? s.txtSimuladoDescription.Trim() : null,
                                     Ano = (s.intAno ?? 0),
                                     CodigoQuestoes = s.txtCodQuestoes != null ? s.txtCodQuestoes.Trim() : null,
                                     Ordem = (s.intSimuladoOrdem ?? 0),
                                     QtdQuestoes = s.intQtdQuestoes ?? 50,
                                     QtdQuestoesDiscursivas = s.intQtdQuestoesCasoClinico,
                                     especialidadeId = e.intEspecialidadeID,
                                     descricaoEspecialidade = e.DE_ESPECIALIDADE,
                                     DtHoraInicio = s.dteDataHoraInicioWEB,
                                     DtHoraFim = s.dteDataHoraTerminoWEB
                                 }).ToList();

                simulados.ForEach(x =>
                {
                    if (x.Especialidades == null)
                        x.Especialidades = new List<Especialidade>();

                    if (x.Especialidades.Count() == 0)
                        x.Especialidades = simulados.Where(y => y.ID == x.ID && y.especialidadeId.HasValue).Select(y => new Especialidade() { Id = y.especialidadeId.Value, Descricao = y.descricaoEspecialidade }).ToList();

                    x.especialidadeId = null;
                    x.descricaoEspecialidade = null;
                });


                simulados = simulados.GroupBy(x => x.ID).Select(g => g.First()).OrderBy(o => o.Ano).ThenBy(o => o.Ordem).ToList(); 

                return simulados;
            }
        }

        public DateTime GetDtInicioUltimaRealizacao(int matricula, int idSimulado)
        {
            try
            {
                using (var ctx = new AcademicoContext())
                {
                    var data = ctx.tblExercicio_Historico
                        .Where(h => h.intClientID == matricula && h.intExercicioTipo == 1 && h.intExercicioID == idSimulado)
                        .Select(h => h.dteDateInicio)
                        .OrderByDescending(h => h)
                        .FirstOrDefault();

                    return data;
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public Especialidades GetEspecialidadesSimulado(List<Exercicio> simulados, ExercicioDTO simulado)
        {
            List<Especialidade> listaDeEspecialidades = simulados.Where(x => x.ID == simulado.ID).Select(d => d.Especialidade).ToList();
            Especialidades especialidades = new Especialidades();
            especialidades.AddRange(listaDeEspecialidades);
            return especialidades;
        }

        public int GetIdProximoSimulado(List<Exercicio> lista)
        {
            var proximo = lista.Where(x => x.DataFim > Utilidades.DateTimeToUnixTimestamp(DateTime.Now))
                        .OrderBy(y => y.DataFim)
                        .FirstOrDefault();

            if (proximo == null)
                return lista.OrderByDescending(x => x.DataFim).FirstOrDefault().ID;
            else
                return proximo.ID;
        }

        public SimuladoDTO GetSimulado(int idSimulado)
        {
            SimuladoDTO simulado = null;
            using (var ctx = new AcademicoContext())
            {
                simulado = (from s in ctx.tblSimulado
                            where s.intSimuladoID == idSimulado
                            select new SimuladoDTO
                            {
                                ID = s.intSimuladoID,
                                ExercicioName = s.txtSimuladoName,
                                Descricao = s.txtSimuladoDescription,
                                Ano = (int)s.intAno,
                                DataInicio = s.dteDataHoraInicioWEB,
                                DataFim = s.dteDataHoraTerminoWEB,
                                TipoId = s.intTipoSimuladoID,
                                Online = s.bitOnline,
                                Duracao = s.intDuracaoSimulado,
                                DtLiberacaoRanking = s.dteLimiteParaRanking
                            }).FirstOrDefault();

                return simulado;
            }
        }

        public Exercicio GetSimuladoAgendado(int matricula)
        {
            try
            {
                using(MiniProfiler.Current.Step("Obtendo simulado agendado"))
                {
                    using (var ctx = new AcademicoContext())
                    {
                        var sim = new Exercicio();
                        var isSimuladoRealizado = false;

                        var simAgendado = ctx.tblSimulado.FirstOrDefault(s =>
                                                                            s.bitOnline
                                                                            && DateTime.Now >= s.dteDataHoraInicioWEB
                                                                            && DateTime.Now <= s.dteDataHoraTerminoWEB);

                        if (simAgendado != null)

                            isSimuladoRealizado = ctx.tblExercicio_Historico.Any(e =>
                                                                                    e.intExercicioTipo == (int)Exercicio.tipoExercicio.SIMULADO
                                                                                    && e.intExercicioID == simAgendado.intSimuladoID
                                                                                    && e.intClientID == matricula);


                        if (!isSimuladoRealizado)
                        {
                            sim = new Exercicio
                            {
                                ID = simAgendado.intSimuladoID,
                                Descricao = simAgendado.txtSimuladoDescription,
                                DataInicio = Utilidades.DateTimeToUnixTimestamp(Convert.ToDateTime(simAgendado.dteDataHoraInicioWEB))
                            };
                        }
                        return sim;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public List<int> GetIdsRealizados(int matricula)
        {
            try
            {
                using (var ctx = new AcademicoContext())
                {
                    var dteDateRefazer = new DateTime(1900, 01, 01);
                    var ids = ctx.tblExercicio_Historico
                        .Where(h => h.intClientID == matricula && h.intExercicioTipo == 1 && !h.dteDateInicio.Equals(dteDateRefazer))
                        .Select(h => h.intExercicioID)
                        .Distinct()
                        .ToList();

                    return ids;
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public int isObjetivo(int simuladoId)
        {
            using (var ctx = new AcademicoContext())
            {
                var isSimuladoObjetivo = (from s in ctx.tblSimulado
                                          where s.intSimuladoID == simuladoId && s.intPesoProvaObjetiva == 100
                                          select s).Any();

                if (isSimuladoObjetivo)
                    return 1;
                else
                    return 0;
            }
        }

        public tblSimulado ObterSimuladoCorrente7DiasAntes()
        {
            tblSimulado simulado = null;
            var dataCorrente = Utilidades.GetServerDate();
            var anoCorrente = dataCorrente.Year;
            var dataFutura7Dias = dataCorrente.AddDays(7);


            using (var ctx = new AcademicoContext())
            {
                simulado = (from sim in ctx.tblSimulado
                            where sim.intAno == anoCorrente
                            && sim.dteDataHoraInicioWEB <= dataFutura7Dias
                            & sim.dteDataHoraTerminoWEB >= dataFutura7Dias
                            select sim).FirstOrDefault();
            }

            return simulado;
        }

        public int ObterQuantidadeDeQuestoesCadastradasNoSimulado(int simuladoID)
        {
            var quantidade = 0;

            using (var ctx = new AcademicoContext())
            {
                var simulado = (from sim in ctx.tblSimulado
                                where sim.intSimuladoID == simuladoID
                                select sim).First();

                var quantidadeObjetiva = simulado.intQtdQuestoes.HasValue ? simulado.intQtdQuestoes.Value : 0;
                var quantidadeDiscursiva = simulado.intQtdQuestoesCasoClinico.HasValue ? simulado.intQtdQuestoesCasoClinico.Value : 0;

                quantidade = quantidadeObjetiva + quantidadeDiscursiva;
            }

            return quantidade;
        }

        public int ObterQuantidadeDeQuestoesDeFatoCadastradasNoSimulado(int simuladoID)
        {
            var quantidade = 0;

            using (var ctx = new AcademicoContext())
            {
                quantidade = (from q in ctx.tblQuestoes
                              join qs in ctx.tblQuestao_Simulado on q.intQuestaoID equals qs.intQuestaoID
                              where qs.intSimuladoID == simuladoID
                              select q).Count();

            }

            return quantidade;
        }

        public List<tblQuestaoAlternativas> ObterAlternativasQuestaoSimulado(int questaoId)
        {
            var alternativas = new List<tblQuestaoAlternativas>();

            using (var ctx = new AcademicoContext())
            {
                alternativas = (from q in ctx.tblQuestaoAlternativas
                                where q.intQuestaoID == questaoId
                                select q).ToList();

            }

            return alternativas;
        }

		public SimuladoDTO GetSimuladoPorId(int id)
		{
			SimuladoDTO registro = null;

			using (var ctx = new AcademicoContext())
			{
				var entity =
				(
					from a in ctx.tblSimulado
					where a.intSimuladoID == id
					select a
				)
				.FirstOrDefault();

				if (entity != null)
				{
					registro = EntityToModel(entity);
				}
			}

			return registro;
		}

		public List<SimuladoDTO> GetSimuladosPorAno(int ano)
		{
			List<SimuladoDTO> registros = new List<SimuladoDTO>();

			using (var ctx = new AcademicoContext())
			{
				var entites = ctx.Set<tblSimulado>()
					.Where(c =>
					   c.intAno != null &&
					   c.intAno == ano
					)
					.ToList();

				if (entites != null && entites.Count > 0)
				{
					registros = entites
						.Select(x => EntityToModel(x))
						.ToList();
				}
			}

			return registros;
		}

		private SimuladoDTO EntityToModel(tblSimulado entity)
		{
			if (entity == null)
			{
				return null;
			}

			SimuladoDTO model = new SimuladoDTO();
			model.ID = entity.intSimuladoID;
			model.TemaID = entity.intLessonTitleID;
			model.LivroID = entity.intBookID;
			model.Origem = entity.txtOrigem;
			model.Nome = entity.txtSimuladoName;
			model.Descricao = entity.txtSimuladoDescription;
			model.Ordem = entity.intSimuladoOrdem;
			model.Duracao = entity.intDuracaoSimulado;
			model.ConcursoID = entity.intConcursoID;
			model.Ano = entity.intAno;
			model.ParaWeb = entity.bitParaWEB;
			model.DataInicioWeb = entity.dteDataHoraInicioWEB;
			model.DataTerminoWeb = entity.dteDataHoraTerminoWEB;
			model.DataLiberacaoWeb = entity.dteReleaseSimuladoWeb;
			model.DataLiberacaoGabarito = entity.dteReleaseGabarito;
			model.DataLiberacaoComentario = entity.dteReleaseComentario;
			model.DataInicioConsultaRanking = entity.dteInicioConsultaRanking;
			model.DataLimiteParaRanking = entity.dteLimiteParaRanking;
			model.EhDemonstracao = entity.bitIsDemo;
			model.CodigoEspecialidade = entity.CD_ESPECIALIDADE;
			model.InstituicaoID = entity.ID_INSTITUICAO;
			model.CaminhoGabarito = entity.txtPathGabarito;
			model.QuantidadeQuestoes = entity.intQtdQuestoes;
			model.RankingWeb = entity.bitRankingWeb;
			model.GabaritoWeb = entity.bitGabaritoWeb;
			model.RankingFinalWeb = entity.bitRankingFinalWeb;
			model.CodigoQuestoes = entity.txtCodQuestoes;
			model.VideoComentariosWeb = entity.bitVideoComentariosWeb;
			model.QuantidadeQuestoesCasoClinico = entity.intQtdQuestoesCasoClinico;
			model.Identificador = entity.guidSimuladoID;
			model.DataUltimaAtualizacao = entity.dteDateTimeLastUpdate;
			model.CronogramaAprovado = entity.bitCronogramaAprovado;
			model.TipoId = entity.intTipoSimuladoID;
			model.Geral = entity.bitSimuladoGeral;
			model.Online = entity.bitOnline;
			model.PesoProvaObjetiva = entity.intPesoProvaObjetiva;
			model.DataInicio = entity.dteDateInicio;
			model.DataFim = entity.dteDateFim;

			#region Especialidades
			model.Especialidades = new List<SimuladoEspecialidadeDTO>();

			using (var ctx = new AcademicoContext())
			{
				var especialidades =
				(
					from a in ctx.tblEspecialidadesSimulado
					where a.intSimuladoID == model.ID
					orderby a.intOrdem
					select a
				)
				.ToList();

				if (especialidades != null && especialidades.Count > 0)
				{
					foreach (var item in especialidades)
					{
						model.Especialidades.Add(new SimuladoEspecialidadeDTO()
						{
							SimuladoID = model.ID,
							EspecialidadeID = item.intEspecialidadeID,
							Ordem = item.intOrdem
						});
					}
				}
			}
			#endregion

			return model;
		}

        private tblSimulado ModelToEntity(SimuladoDTO model)
		{
			if (model == null)
			{
				return null;
			}

			tblSimulado entity = new tblSimulado();
			entity.intSimuladoID = model.ID;
			entity.intLessonTitleID = model.TemaID;
			entity.intBookID = model.LivroID;
			entity.txtOrigem = model.Origem;
			entity.txtSimuladoName = model.Nome;
			entity.txtSimuladoDescription = model.Descricao;
			entity.intSimuladoOrdem = model.Ordem;
			entity.intDuracaoSimulado = model.Duracao;
			entity.intConcursoID = model.ConcursoID;
			entity.intAno = model.Ano;
			entity.bitParaWEB = model.ParaWeb;
			entity.dteDataHoraInicioWEB = model.DataInicioWeb;
			entity.dteDataHoraTerminoWEB = model.DataTerminoWeb;
			entity.dteReleaseSimuladoWeb = model.DataLiberacaoWeb;
			entity.dteReleaseGabarito = model.DataLiberacaoGabarito;
			entity.dteReleaseComentario = model.DataLiberacaoComentario;
			entity.dteInicioConsultaRanking = model.DataInicioConsultaRanking;
			entity.dteLimiteParaRanking = model.DataLimiteParaRanking;
			entity.bitIsDemo = model.EhDemonstracao;
			entity.CD_ESPECIALIDADE = model.CodigoEspecialidade;
			entity.ID_INSTITUICAO = model.InstituicaoID;
			entity.txtPathGabarito = model.CaminhoGabarito;
			entity.intQtdQuestoes = model.QuantidadeQuestoes;
			entity.bitRankingWeb = model.RankingWeb;
			entity.bitGabaritoWeb = model.GabaritoWeb;
			entity.bitRankingFinalWeb = model.RankingFinalWeb;
			entity.txtCodQuestoes = model.CodigoQuestoes;
			entity.bitVideoComentariosWeb = model.VideoComentariosWeb;
			entity.intQtdQuestoesCasoClinico = model.QuantidadeQuestoesCasoClinico;
			entity.guidSimuladoID = model.Identificador;
			entity.dteDateTimeLastUpdate = model.DataUltimaAtualizacao;
			entity.bitCronogramaAprovado = model.CronogramaAprovado;
			entity.intTipoSimuladoID = model.TipoId;
			entity.bitSimuladoGeral = model.Geral;
			entity.bitOnline = model.Online;
			entity.intPesoProvaObjetiva = model.PesoProvaObjetiva;
			entity.dteDateInicio = model.DataInicio;
			entity.dteDateFim = model.DataFim;

			return entity;
		}

        public int Alterar(SimuladoDTO registro)
		{
			int retorno = 0;

			if (registro == null)
			{
				return retorno;
			}

			using (var ctx = new AcademicoContext())
			{
				var data = ctx.Set<tblSimulado>()
					.Include("tblEspecialidadesSimulado")
					.FirstOrDefault(c => c.intSimuladoID == registro.ID);

				if (data == null)
				{
					return 0;
				}

				var entity = ModelToEntity(registro);
				ctx.Entry(data).CurrentValues.SetValues(entity);

				#region Salvar Especialidades
				if (registro.Especialidades != null && registro.Especialidades.Count > 0)
				{
					foreach (var item in registro.Especialidades)
					{
						var especialidade = new tblEspecialidadesSimulado();
						especialidade.intSimuladoID = entity.intSimuladoID;
						especialidade.intEspecialidadeID = item.EspecialidadeID;
						especialidade.intOrdem = item.Ordem.Value;

						ctx.Set<tblEspecialidadesSimulado>().Add(especialidade);
					}
				}
				#endregion

				ctx.SaveChanges();
				retorno = 1;
			}

			return retorno;
		}

		public List<TemaSimuladoDTO> GetTemasSimuladoPorAno(int ano)
		{
			List<TemaSimuladoDTO> registros = new List<TemaSimuladoDTO>();

			using (var ctx = new DesenvContext())
			{
				registros =
				(
					from a in ctx.tblLessonTitles
					where
						a.intAno != null &&
						a.intAno == ano
					orderby a.intAno, a.txtLessonTitleName
					select new TemaSimuladoDTO()
					{
						ID = a.intLessonTitleID,
						Nome = a.txtLessonTitleName,
						Ano = a.intAno
					}
				)
				.ToList();
			}

			return registros;
		}

		public List<TipoSimuladoDTO> GetTiposSimulado()
		{
			List<TipoSimuladoDTO> registros = new List<TipoSimuladoDTO>();

			using (var ctx = new DesenvContext())
			{
				registros =
				(
					from a in ctx.tblSimuladoTipos
					orderby a.txtDescription
					select new TipoSimuladoDTO()
					{
						ID = a.intTipoSimuladoID,
						Nome = a.txtDescription
					}
				)
				.ToList();
			}

			return registros;
		}

        public Exercicio GetInformacoesBasicasSimulado(Banner bannerSimulado)
        {
            using (var ctx = new AcademicoContext())
            {
                var simuladoInfo = new Exercicio();


                var resultSimuladoAtual = bannerSimulado.ID > 0 ? ctx.tblSimulado.Where(s => s.intSimuladoID == bannerSimulado.IdSimulado).FirstOrDefault() : new tblSimulado();


                if (resultSimuladoAtual.intSimuladoID > 0)
                {
                    simuladoInfo = new Exercicio
                    {
                        Ano = resultSimuladoAtual.intAno ?? 0,
                        ExercicioName = resultSimuladoAtual.txtSimuladoName.Substring(5).Split('-')[0].Trim(),
                        Descricao = resultSimuladoAtual.txtSimuladoDescription.Substring(5).Split('-')[1].Trim(),
                        ID = resultSimuladoAtual.intSimuladoID,
                        DataInicio = resultSimuladoAtual.dteDataHoraInicioWEB == null ? 0 : Utilidades.DateTimeToUnixTimestamp(Convert.ToDateTime(resultSimuladoAtual.dteDataHoraInicioWEB)),
                        DataFim = resultSimuladoAtual.dteDataHoraInicioWEB == null ? 0 : Utilidades.DateTimeToUnixTimestamp(Convert.ToDateTime(resultSimuladoAtual.dteDataHoraTerminoWEB))
                    };
                }

                return simuladoInfo;
            }
        }
    }
}