using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Model;
using System.Linq;
using System;
using System.Data.SqlClient;
using MedCore_API.Academico;
using System.Data;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Contracts.Enums;
using Newtonsoft.Json;
using Medgrupo.DataAccessEntity;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MedCore_DataAccess.Business;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Repository
{
    public class MontaProvaEntity : IMontaProvaData
    {
        public List<RespostaConcursoDTO> AcademicoObjecto;
        public List<RespostaConcursoQuestoesDTO> QuestoesObjecto;
        public List<RespostaSimuladoDTO> RespostaSimuladoObjecto;

        public int Insert(List<Questao> questoes, MontaProvaFiltroPost filtroPost, MontaProvaFiltro filtro)
        {
            try
            {
                var filtroAluno = new tblFiltroAluno_MontaProva()
                {
                    bitAtivo = true,
                    dteDataCriacao = DateTime.Now,
                    intClientId = filtroPost.Matricula,
                    txtAnos = GetSelecao(filtro, EModuloFiltro.UltimosAnos),
                    txtConcursos = GetSelecao(filtro, EModuloFiltro.Concursos),
                    txtJsonFiltro = JsonConvert.SerializeObject(filtroPost),
                    txtNome = filtroPost.Nome,
                    txtPalavraChave = filtroPost.FiltroTexto,
                    txtEspecialidades = GetSelecao(filtro, EModuloFiltro.Especialidades),
                    txtFiltrosEspeciais = GetSelecao(filtro, EModuloFiltro.FiltrosEspeciais),
                    intQtdQuestoes = filtro.TotalQuestoes
                };

                using (var ctx = new DesenvContext())
                {
                    ctx.tblFiltroAluno_MontaProva.Add(filtroAluno);
                    ctx.SaveChanges();
                }

                var prova = new tblExercicio_MontaProva()
                {
                    dteDataCriacao = DateTime.Now,
                    bitAtivo = true,
                    intFiltroId = filtroAluno.intID
                };

                using (var ctx = new DesenvContext())
                {
                    ctx.tblExercicio_MontaProva.Add(prova);
                    ctx.SaveChanges();
                }

                var dt = ToDataTable(questoes, filtroAluno.intID);
                var ret = BulkInsert(dt, "tblQuestao_MontaProva");
                ConfiguraProva(prova.intID, filtroPost.Matricula);

                RelacionarQuestoes(filtroAluno.intID, prova.intID);

                return prova.intID;
            }
            catch
            {
                throw;
            }
        }

        public ProvaAluno InsertProva(int idFiltro)
        {
            var novaProva = new tblExercicio_MontaProva()
            {
                bitAtivo = true,
                dteDataCriacao = DateTime.Now,
                intFiltroId = idFiltro
            };

            using (var ctx = new DesenvContext())
            {
                ctx.tblExercicio_MontaProva.Add(novaProva);
                ctx.SaveChanges();
            }

            int matricula;

            using (var ctx = new DesenvContext())
            {
                matricula = ctx.tblFiltroAluno_MontaProva.FirstOrDefault(x => x.intID == idFiltro).intClientId;
            }

            ConfiguraProva(novaProva.intID, matricula);

            var nQuestoes = RelacionarQuestoes(idFiltro, novaProva.intID);

            return new ProvaAluno()
            {
                ID = novaProva.intID,
                Acertos = 0,
                DataCriacao = Utilidades.DateTimeToUnixTimestamp(novaProva.dteDataCriacao),
                Erros = 0,
                NaoRealizadas = nQuestoes,
                QuantidadeQuestoes = nQuestoes
            };
        }

        public int AlterarQuestoesProva(int idFiltro, int idProva, int quantidade)
       {
            if (quantidade > 0)
            {
                AdicionarQuestoes(idFiltro, idProva, quantidade);
            }
            else
            {
                RemoverQuestoes(idFiltro, idProva, -quantidade);
            }

			ModificarQuestoesContador(ObterIdFiltroPorIdProva(idFiltro), quantidade);


			return 1;
       }

        public int AlterarQuestoesProvaNovo(int idFiltro, int idProva, int quantidade)
        {              
            if (quantidade > 0)
            {
                AdicionarQuestoesNovo(idFiltro, idProva, quantidade);
            }
            else
            {
                RemoverQuestoesNovo(idFiltro, idProva, -quantidade);
            }

			ModificarQuestoesContador(ObterIdFiltroPorIdProva(idFiltro), quantidade);

			return 1;
        }

		public int ObterIdFiltroPorIdProva(int idFiltro)
		{
			using (var ctx = new DesenvContext())
			{
				return ctx.tblExercicio_MontaProva.Where(x => x.intFiltroId == idFiltro).Select(x => x.intID).FirstOrDefault();
			}
		}

		public void ModificarQuestoesContador(int idFiltro, int qtdQuestoes, int acertos = 0, int erros = 0, int realizacao = 0)
		{
			using (var ctx = new DesenvContext())
			{
				var contador = ctx.tblContadorQuestoes_MontaProva.Where(x => x.intProvaId == idFiltro).FirstOrDefault();
				if (contador != null)
				{
					contador.intQuantidadeQuestoes += qtdQuestoes;
					contador.intNaoRealizadas += realizacao != 0 ? -realizacao : qtdQuestoes;
					contador.intAcertos += acertos;
					contador.intErros += erros;
				}

				ctx.SaveChanges();
			}
		}


        public void AdicionarQuestoes(int idFiltro, int idProva, int qtd)
        {
            using (var ctx = new DesenvContext())
            {
                var questoesProva = ctx.tblQuestao_MontaProva.Where(x => x.intFiltroId == idFiltro && x.intProvaId == null).Random(qtd).ToList();
                questoesProva.ForEach(x => x.intProvaId = idProva);

                ctx.SaveChanges();
            }
        }

        public void AdicionarQuestoesNovo(int idFiltro, int idProva, int qtd)
        {

            //Buscar Novas Questões do Mongo que não estejam na prova de modo randômico limitado a quantidade a adicionar
            
            //Recuperar o Filtro
            var filtro = this.GetFiltro(idFiltro);
            MontaProvaFiltroPost montaProvafiltroPost = Newtonsoft.Json.JsonConvert.DeserializeObject<MontaProvaFiltroPost>(filtro.txtJsonFiltro);

            //GetIdsQuestoes da Prova
            var notInList = GetQuestoesProva(idProva);

            montaProvafiltroPost.ExercicioJaExistenteProva = notInList;

            //Recuperar Questões do Filtro
            var questoesMongo = new MontaProvaManager().GetQuestoesFiltro(montaProvafiltroPost, true, qtd);

            //Adicionar estas questões no SQL tblQuestao_MontaProva de forma já associada a prova.

            var dt = ToDataTable(questoesMongo, idFiltro, idProva);
            var ret = BulkInsert(dt, "tblQuestao_MontaProva", true);

        }

        private List<Questao> GetQuestoesProva(int idProva)
        {
            List<Questao> questoes = new List<Questao>();

            using (var ctx = new DesenvContext())
            {
                var questoesProva = ctx.tblQuestao_MontaProva.Where(x => x.intProvaId == idProva).ToList();
                questoesProva.ForEach(x => questoes.Add(new Questao() { Id = x.intQuestaoId, Tipo = x.intTipoExercicioId.Value }));
            }

            return questoes;
        }

        protected void RemoverQuestoes(int idFiltro, int idProva, int qtd)
        {
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    var intClientId = ctxMatDir.tblFiltroAluno_MontaProva.FirstOrDefault(x => x.intID == idFiltro).intClientId;


                    var questoes = (from q in ctxMatDir.tblQuestao_MontaProva
                                    where q.intProvaId == idProva
                                    select new { id = q.intID, Questaoid = q.intQuestaoId, tipoExercicio = q.intTipoExercicioId }
                                    ).ToList();

                    var questoesConcurso = questoes.Where(x => x.tipoExercicio == (int)Exercicio.tipoExercicio.CONCURSO).Select(x => x.Questaoid).ToArray();
                    var questoesSimulado = questoes.Where(x => x.tipoExercicio == (int)Exercicio.tipoExercicio.SIMULADO).Select(x => x.Questaoid).ToArray();

                    int[] simuladoTipoId = new int[] { (int)Exercicio.tipoExercicio.SIMULADO, (int)Exercicio.tipoExercicio.MONTAPROVA };
                    int[] concursoTipoId = new int[] { (int)Exercicio.tipoExercicio.CONCURSO, (int)Exercicio.tipoExercicio.MONTAPROVA };

                    var respostasSimulado = (from cro in ctx.tblCartaoResposta_objetiva
                                             join qa in ctx.tblQuestaoAlternativas
                                             on cro.intQuestaoID equals qa.intQuestaoID
                                             join q in ctx.tblQuestoes
                                             on qa.intQuestaoID equals q.intQuestaoID
                                             join eh in ctx.tblExercicio_Historico
                                             on cro.intHistoricoExercicioID equals eh.intHistoricoExercicioID
                                             where questoesSimulado.Contains(qa.intQuestaoID)
                                             && simuladoTipoId.Contains(eh.intExercicioTipo)
                                             && qa.txtLetraAlternativa == cro.txtLetraAlternativa
                                             && eh.intClientID == intClientId
                                             && (q.bitCasoClinico == null || q.bitCasoClinico != "1")
                                             select new
                                             {
                                                 questaoId = qa.intQuestaoID,
                                                 tipo = (int)Exercicio.tipoExercicio.SIMULADO
                                             }).ToList();

                    var respostasObjetiva = (from cro in ctx.tblCartaoResposta_objetiva
                                             join eh in ctx.tblExercicio_Historico
                                             on cro.intHistoricoExercicioID equals eh.intHistoricoExercicioID
                                             where questoesConcurso.Contains(cro.intQuestaoID)
                                             && concursoTipoId.Contains(eh.intExercicioTipo)
                                             && eh.intClientID == intClientId
                                             select new { cro.intQuestaoID, cro.txtLetraAlternativa }
                                             ).ToList();

                    var questoesAlternativas = (from cq in ctxMatDir.tblConcursoQuestoes
                                                join qa in ctxMatDir.tblConcursoQuestoes_Alternativas on cq.intQuestaoID equals qa.intQuestaoID
                                                where questoesConcurso.Contains(qa.intQuestaoID)
                                                select new { qa.intQuestaoID, qa.txtLetraAlternativa }
                                                ).ToList();

                    var respostasConcurso = (from qa in questoesAlternativas
                                             join cro in respostasObjetiva on qa.intQuestaoID equals cro.intQuestaoID
                                             where qa.txtLetraAlternativa == cro.txtLetraAlternativa
                                             select new
                                             {
                                                 questaoId = qa.intQuestaoID,
                                                 tipo = (int)Exercicio.tipoExercicio.CONCURSO
                                             }).ToList();

                    var questoesMarcadasSimulado = (from q in questoes
                                                    join c in respostasSimulado
                                                    on q.Questaoid equals c.questaoId
                                                    where q.tipoExercicio == (int)Exercicio.tipoExercicio.SIMULADO
                                                    select q.id
                                            ).ToArray();

                    var questoesMarcadasConcurso = (from q in questoes
                                                    join c in respostasConcurso
                                                    on q.Questaoid equals c.questaoId
                                                    where q.tipoExercicio == (int)Exercicio.tipoExercicio.CONCURSO
                                                    select q.id
                                            ).ToArray();

                    var questoesMarcadas = questoesMarcadasConcurso.Concat(questoesMarcadasSimulado).ToArray();

                    var questoesNaoRealizadas = ctxMatDir.tblQuestao_MontaProva.Where(x => x.intProvaId == idProva && !questoesMarcadas.Contains(x.intID)).ToList();

                    var randomQuestions = questoesNaoRealizadas.Random(qtd);

                    randomQuestions.ForEach(x => x.intProvaId = null);

                    ctxMatDir.SaveChanges();
                }
            }
        }

        protected void RemoverQuestoesNovo(int idFiltro, int idProva, int qtd)
        {
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    var intClientId = ctxMatDir.tblFiltroAluno_MontaProva.FirstOrDefault(x => x.intID == idFiltro).intClientId;


                    var questoes = (from q in ctxMatDir.tblQuestao_MontaProva
                                    where q.intProvaId == idProva
                                    select new { id = q.intID, Questaoid = q.intQuestaoId, tipoExercicio = q.intTipoExercicioId }
                                    ).ToList();

                    var questoesConcurso = questoes.Where(x => x.tipoExercicio == (int)Exercicio.tipoExercicio.CONCURSO).Select(x => x.Questaoid).ToArray();
                    var questoesSimulado = questoes.Where(x => x.tipoExercicio == (int)Exercicio.tipoExercicio.SIMULADO).Select(x => x.Questaoid).ToArray();

                    int[] simuladoTipoId = new int[] { (int)Exercicio.tipoExercicio.SIMULADO, (int)Exercicio.tipoExercicio.MONTAPROVA };
                    int[] concursoTipoId = new int[] { (int)Exercicio.tipoExercicio.CONCURSO, (int)Exercicio.tipoExercicio.MONTAPROVA };

                    var respostasSimulado = (from cro in ctx.tblCartaoResposta_objetiva
                                             join qa in ctx.tblQuestaoAlternativas
                                             on cro.intQuestaoID equals qa.intQuestaoID
                                             join q in ctx.tblQuestoes
                                             on qa.intQuestaoID equals q.intQuestaoID
                                             join eh in ctx.tblExercicio_Historico
                                             on cro.intHistoricoExercicioID equals eh.intHistoricoExercicioID
                                             where questoesSimulado.Contains(qa.intQuestaoID)
                                             && simuladoTipoId.Contains(eh.intExercicioTipo)
                                             && qa.txtLetraAlternativa == cro.txtLetraAlternativa
                                             && eh.intClientID == intClientId
                                             && (q.bitCasoClinico == null || q.bitCasoClinico != "1")
                                             select new
                                             {
                                                 questaoId = qa.intQuestaoID,
                                                 tipo = (int)Exercicio.tipoExercicio.SIMULADO
                                             }).ToList();

                    var retorno = (from cro in ctx.tblCartaoResposta_objetiva
                                   join eh in ctx.tblExercicio_Historico
                                   on cro.intHistoricoExercicioID equals eh.intHistoricoExercicioID
                                   where concursoTipoId.Contains(eh.intExercicioTipo)
                                   && eh.intClientID == intClientId
                                   select new { cro.intQuestaoID, cro.txtLetraAlternativa }
                                             ).ToList();

                    var respostasObjetiva = (from a in retorno
                                             where questoesConcurso.Contains(a.intQuestaoID)
                                             select a).ToList();
                    var retornoAlternativas = (from cq in ctxMatDir.tblConcursoQuestoes
                                               join qa in ctxMatDir.tblConcursoQuestoes_Alternativas on cq.intQuestaoID equals qa.intQuestaoID
                                               select new { qa.intQuestaoID, qa.txtLetraAlternativa }
                                                ).ToList();
                    var questoesAlternativas = (from a in retornoAlternativas
                                                where questoesConcurso.Contains(a.intQuestaoID)
                                                select a).ToList();
                    var respostasConcurso = (from qa in questoesAlternativas
                                             join cro in respostasObjetiva on qa.intQuestaoID equals cro.intQuestaoID
                                             where qa.txtLetraAlternativa == cro.txtLetraAlternativa
                                             select new
                                             {
                                                 questaoId = qa.intQuestaoID,
                                                 tipo = (int)Exercicio.tipoExercicio.CONCURSO
                                             }).ToList();

                    var questoesMarcadasSimulado = (from q in questoes
                                                    join c in respostasSimulado
                                                    on q.Questaoid equals c.questaoId
                                                    where q.tipoExercicio == (int)Exercicio.tipoExercicio.SIMULADO
                                                    select q.id
                                            ).ToArray();

                    var questoesMarcadasConcurso = (from q in questoes
                                                    join c in respostasConcurso
                                                    on q.Questaoid equals c.questaoId
                                                    where q.tipoExercicio == (int)Exercicio.tipoExercicio.CONCURSO
                                                    select q.id
                                            ).ToArray();

                    var questoesMarcadas = questoesMarcadasConcurso.Concat(questoesMarcadasSimulado).ToArray();

                    var retornoQuestoes = ctxMatDir.tblQuestao_MontaProva.Where(x => x.intProvaId == idProva).Select(y => new { intID = y.intID }).ToList();

                    var questoesNaoRealizadas = retornoQuestoes.Where(x => !questoesMarcadas.Contains(x.intID)).ToList();

                    var randomQuestionsIds = questoesNaoRealizadas.Random(qtd).Select(x => x.intID).ToList();

                    DeleteQuestoes(randomQuestionsIds);
                    ctxMatDir.SaveChanges();
                }
            }
        }

        protected int DeleteQuestoes(List<int> questoesIds)
        {
            if (questoesIds.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                string combindedString = string.Join(",", questoesIds.ToArray());
                sb.Append(combindedString);
                sb.Append(")");
                var stringbuilderresult = sb.ToString();

                using (var ctx = new DesenvContext())
                {
                    ctx.SetCommandTimeOut(180);
                    ctx.Database.ExecuteSqlRaw(String.Format("DELETE from tblQuestao_MontaProva WHERE intID in {0}", sb.ToString()));
                }
            }
            return 1;
        }
        protected int DeleteQuestoesByProvaId(int ProvaId)
        {

            var sb = ProvaId.ToString();
            

            using (var ctx = new DesenvContext())
            {
                ctx.SetCommandTimeOut(180);
                ctx.Database.ExecuteSqlRaw(String.Format("DELETE from tblQuestao_MontaProva WHERE intProvaId = {0}", sb));
            }

            return 1;
        }

        private int RelacionarQuestoes(int idFiltro, int idProva)
        {
            var nQuestoes = Convert.ToInt32(ConfigurationProvider.Get("Settings:quantidadeQuestaoProva"));

            using (var ctx = new DesenvContext())
            {
                var questoesProva = ctx.tblQuestao_MontaProva.Where(x => x.intFiltroId == idFiltro &&  x.intProvaId == null).Random(nQuestoes).ToList();
                questoesProva.ForEach(x => x.intProvaId = idProva);

                var nQuestoesProva = questoesProva.Count(x => x.intProvaId == idProva);

                ctx.SaveChanges();

                return nQuestoesProva;
            }
        }

        private string GetSelecao(MontaProvaFiltro filtro, EModuloFiltro modulo)
        {
            return filtro.Filtros.FirstOrDefault(x => x.Modulo == modulo).Selecao;
        }

        private DataTable ToDataTable(List<Questao> questoes, int filtroId, int? provaId = null)
        {
            var dt = new DataTable();

            dt.Columns.Add(("intQuestaoId"));
            dt.Columns.Add(("intTipoExercicioId"));
            if (provaId.HasValue)
                dt.Columns.Add(("intProvaId"));
            dt.Columns.Add(("intFiltroId"));

            foreach (var questao in questoes)
            {
                DataRow row = dt.NewRow();

                row["intQuestaoId"] = questao.Id;
                row["intTipoExercicioId"] = questao.ExercicioTipoID;
                if (provaId.HasValue)
                    row["intProvaId"] = provaId;
                row["intFiltroId"] = filtroId;

                dt.Rows.Add(row);
            }

            return dt;
        }

        private int BulkInsert(DataTable dataTable, string tableName, bool relacionaProva = false)
        {

            var connectionString = ConfigurationProvider.Get("ConnectionStrings:DesenvConnection");;

            using (SqlBulkCopy sqlbc = new SqlBulkCopy(connectionString))
            {
                sqlbc.BulkCopyTimeout = 120;

                sqlbc.DestinationTableName = tableName;
                sqlbc.ColumnMappings.Add("intQuestaoId", "intQuestaoId");
                sqlbc.ColumnMappings.Add("intTipoExercicioId", "intTipoExercicioId");
                sqlbc.ColumnMappings.Add("intFiltroId", "intFiltroId");
                if (relacionaProva)
                    sqlbc.ColumnMappings.Add("intProvaId", "intProvaId");
                sqlbc.WriteToServer(dataTable);
            }

            return 1;
        }

        public int Edit(ProvaAluno provaAluno)
        {
            try
            {
                using(MiniProfiler.Current.Step("Editando prova"))
                {
                    using (var ctx = new DesenvContext())
                    {
                        var filtro = ctx.tblFiltroAluno_MontaProva.FirstOrDefault(x => x.intID == provaAluno.ID);
                        if (filtro == null) return 0;

                        filtro.txtNome = provaAluno.Nome;

                        ctx.SaveChanges();

                        return 1;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int Delete(ProvaAluno provaAluno)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var item = ctx.tblExercicio_MontaProva.FirstOrDefault(p => p.intID == provaAluno.ID);
                    if (item == null) return 0;

                    item.bitAtivo = false;

                    var questoes = ctx.tblQuestao_MontaProva.Where(x => x.intProvaId == provaAluno.ID).ToList();
                    questoes.ForEach(x => x.intProvaId = null);

                    if (!ctx.tblExercicio_MontaProva.ToList().Any(x => x.intFiltroId == item.intFiltroId && (bool)x.bitAtivo))
                    {
                        ctx.tblFiltroAluno_MontaProva.FirstOrDefault(x => x.intID == item.intFiltroId).bitAtivo = false;
                    }

                    ctx.SaveChanges();

                    return 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int DeleteNovo(ProvaAluno provaAluno)
        {
            try
            {
                tblExercicio_MontaProva item = null;
                using (var ctx = new DesenvContext())
                {
                    item = ctx.tblExercicio_MontaProva.FirstOrDefault(p => p.intID == provaAluno.ID);
                }

                if (item == null)
                        return 0;
                else
                    {

                        //Deleta Todas As Questões Relacionadas a Prova
                        DeleteQuestoesByProvaId(provaAluno.ID);

                        int? filtroId;
                        using (var ctx = new DesenvContext())
                        {
                            item = ctx.tblExercicio_MontaProva.FirstOrDefault(p => p.intID == provaAluno.ID);

                            filtroId = item.intFiltroId;
                            ctx.tblExercicio_MontaProva.Remove(item);
                            ctx.SaveChanges();
                        }

                        using (var ctx = new DesenvContext())
                        {
                            if (!ctx.tblExercicio_MontaProva.ToList().Any(x => x.intFiltroId == filtroId && (bool)x.bitAtivo))
                            {
                                DeleteQuestoesNaoAssociadas(filtroId.Value);
                                var filtroADeletar = ctx.tblFiltroAluno_MontaProva.FirstOrDefault(x => x.intID == filtroId);
                                ctx.tblFiltroAluno_MontaProva.Remove(filtroADeletar);
                            }
                            ctx.SaveChanges();
                        }


                        return 1;
                    }
                }

            catch (Exception)
            {
                throw;
            }
        }

        public int DeleteFiltro(int idFiltroAluno)
        {
            try
            {
                using(MiniProfiler.Current.Step("Deletando filtro - Monta Prova"))
                {
                    using (var ctx = new DesenvContext())
                    {
                        var item = ctx.tblFiltroAluno_MontaProva.FirstOrDefault(x => x.intID == idFiltroAluno);
                        if (item == null) return 0;

                        item.bitAtivo = false;

                        var provasFiltro = ctx.tblExercicio_MontaProva.Where(x => x.intFiltroId == idFiltroAluno).ToList();

                        provasFiltro.ForEach(prova =>
                        {
                            prova.bitAtivo = false;

                            var questoes = ctx.tblQuestao_MontaProva.Where(questao => questao.intProvaId == prova.intID).ToList();
                            questoes.ForEach(questao => questao.intProvaId = null);
                        });
                        
                        ctx.SaveChanges();

                        return 1;
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
        
        //public FiltrosAluno GetProvasAluno(int matricula, int idAplicacao)
        //{
          
        //    // ======================== LOG
        //    new Util.Log().SetLog(new LogMsPro
        //    {
        //        Matricula = matricula,
        //        IdApp = (Aplicacoes)idAplicacao,
        //        Tela = Util.Log.MsProLog_Tela.MinhasProvas,
        //        Acao = Util.Log.MsProLog_Acao.Abriu
        //    });
        //    // ======================== 

        //    List<FiltroAluno> listaFiltros;

        //    using (var ctx = new DesenvContext())
        //    {
        //        listaFiltros = ctx.tblFiltroAluno_MontaProva.Where(x => x.intClientId == matricula && (bool)x.bitAtivo).Select(x => new FiltroAluno()
        //        {
        //            Anos = x.txtAnos,
        //            Concursos = x.txtConcursos,
        //            Criacao = x.dteDataCriacao,
        //            FiltrosEspeciais = x.txtFiltrosEspeciais,
        //            Especialidades = x.txtEspecialidades,
        //            Matricula = x.intClientId,
        //            PalavraChave = x.txtPalavraChave,
        //            Id = x.intID,
        //            Nome = x.txtNome
        //        }).OrderByDescending(x => x.Id).ToList();
        //    }

        //    var filtrosAluno = new FiltrosAluno();
        //    filtrosAluno.AddRange(listaFiltros);

        //    filtrosAluno.ForEach(x =>
        //    {
        //        x.ProvasAluno = GetProvasFiltro(matricula, x.Id);
        //        x.QuantidadeQuestoesNaoAssociadas = GetQuantidadeQuestoesNaoAssociadas(x.Id);
        //        x.QuantidadeQuestoesAssociadas = GetQuantidadeQuestoesAssociadas(x.Id);
        //    });

        //    return filtrosAluno;
        //}

        public void ConfiguraProva(int provaId, int matricula)
        {
            using (var ctx = new AcademicoContext())
            {

                var historico = ctx.tblExercicio_Historico.Add(new tblExercicio_Historico
                {
                    intApplicationID = (int)Aplicacoes.MsProMobile,// 17,
                    intClientID = matricula,
                    intExercicioID = provaId,
                    intExercicioTipo = (int)Exercicio.tipoExercicio.MONTAPROVA, // 4,
                    dteDateInicio = DateTime.Now
                });
                ctx.SaveChanges();
            }
        }

        //public ProvasAluno GetProvasFiltro(int matricula, int idFiltro)
        //{
        //    try
        //    {
        //        var prvsAluno = new ProvasAluno();

        //        var provasAluno = ObterProvasAluno(idFiltro);

        //        foreach (var prova in provasAluno)
        //        {
        //            prova.DataCriacao = Utilidades.DateTimeToUnixTimestamp(prova.Criacao);

        //            var questoes = ObterQuestoesMontaProva(prova);
                        
        //            prova.QuantidadeQuestoes = questoes.Count();

        //            var questoesSimulado = ObterProvaSimulado(matricula, questoes);
        //            var questoesConcurso = ObterProvaConcurso(matricula, questoes);

        //            prova.NaoRealizadas = questoesSimulado.NaoRealizadas + questoesConcurso.NaoRealizadas;
        //            prova.Acertos = questoesSimulado.Acertos + questoesConcurso.Acertos;
        //            prova.Erros = questoesSimulado.Erros + questoesConcurso.Erros;
        //        }

        //        prvsAluno.AddRange(provasAluno);

        //        return prvsAluno;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
            
        //}
        public List<RespostaConcursoQuestoesDTO> GetAllQuestoesAlternativa()
        {
            using (var ctxMatDir = new DesenvContext())
            {
                var desenv = (from cq in ctxMatDir.tblConcursoQuestoes
                              join x in ctxMatDir.tblConcursoQuestoes_Alternativas
                             on cq.intQuestaoID equals x.intQuestaoID
                              where
                               (cq.bitCasoClinico == null || cq.bitCasoClinico != "1")

                              select new RespostaConcursoQuestoesDTO
                              {
                                  intQuestaoID = x.intQuestaoID
                                  ,
                                  txtLetraAlternativa = x.txtLetraAlternativa
                                  ,
                                  bitCorreta = x.bitCorreta
                                  ,
                                  bitCorretaPreliminar = x.bitCorretaPreliminar
                                  ,
                                  bitAnulada = cq.bitAnulada
                              }).ToList();
                return desenv;
            }
        }

        public List<RespostaConcursoQuestoesDTO> ObterQuestoesAlternativaCache()
        {
            try
            {
                if (RedisCacheManager.CannotCache(RedisCacheConstants.Questao.KeyGetQuestaoMontaProva))
                    return GetAllQuestoesAlternativa();

                var key = String.Format("{0}:{1}", RedisCacheConstants.Questao.KeyGetQuestaoMontaProva, DateTime.Today.ToString("d"));
                var questoesMontaProva = RedisCacheManager.GetItemObject<List<RespostaConcursoQuestoesDTO>>(key);

                if (questoesMontaProva == null)
                {
                    questoesMontaProva = GetAllQuestoesAlternativa();
                    if (questoesMontaProva != null)
                    {
                        RedisCacheManager.DeleteGroup(RedisCacheConstants.Questao.KeyGetQuestaoMontaProva);
                        RedisCacheManager.SetItemObject(key, questoesMontaProva);

                    }
                }

                return questoesMontaProva;
            }
            catch
            {
                return GetAllQuestoesAlternativa();
            }

        }

        public List<RespostaConcursoDTO> ObterRespostasConcurso(int matricula, int[] questoesConcurso)
        {
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    if (QuestoesObjecto == null)
                    {
                        QuestoesObjecto = ObterQuestoesAlternativaCache();
                    }
                    var desenv = QuestoesObjecto.ToList();
                    var respostasConcurso = new List<RespostaConcursoDTO>();
                    if (AcademicoObjecto == null)
                    {

                        var filtroQuestoes = "(" + Convert.ToString((int)Exercicio.tipoExercicio.CONCURSO) + "," + Convert.ToString((int)Exercicio.tipoExercicio.MONTAPROVA) + ")";


                        var CartaoRespostaObejtiva = new DBQuery().ExecuteQuery(@"select
	                                                cro.txtLetraAlternativa,
	                                                cro.dteCadastro,
	                                                cro.intQuestaoID
	                                                from tblCartaoResposta_objetiva cro
	                                                where cro.intClientID = " + matricula + " and cro.intExercicioTipoId in " + filtroQuestoes + "", true);


                        if (CartaoRespostaObejtiva.Tables[0].Rows.Count > 0)
                            foreach (DataRow dRow in CartaoRespostaObejtiva.Tables[0].Rows)
                                respostasConcurso.Add(new RespostaConcursoDTO()
                                {
                                    QuestaoId = Convert.ToInt32(dRow["intQuestaoID"]),
                                    Alternativa = dRow["txtLetraAlternativa"].ToString(),
                                    DteCadastro = Convert.ToDateTime(dRow["dteCadastro"].ToString()),

                                });
                        AcademicoObjecto = respostasConcurso;
                    }



                    var Academico = AcademicoObjecto.ToList();


                    var lista = (from d in desenv
                                 join a in Academico
                                   on new
                                   {
                                       QuestaoID = d.intQuestaoID
                                     ,
                                       Alternativa = d.txtLetraAlternativa
                                   } equals
                                      new
                                      {
                                          QuestaoID = a.QuestaoId
                                        ,
                                          Alternativa = a.Alternativa
                                      }
                                 join c in questoesConcurso on d.intQuestaoID equals c
                                 select new RespostaConcursoDTO()
                                 {
                                     QuestaoId = d.intQuestaoID,
                                     Alternativa = d.txtLetraAlternativa,
                                     Correta = (bool)((d.bitCorreta ?? false) || (d.bitCorretaPreliminar ?? false)), // Convert.ToBoolean(dRow["bitCorreta"]) || Convert.ToBoolean(dRow["bitCorretaPreliminar"]),
                                     AlternativaRespondida = a.Alternativa, //dRow["AlternativaRespondida"].ToString(),
                                     Anulada = d.bitAnulada, //Convert.ToBoolean(dRow["bitAnulada"]),
                                     DteCadastro = a.DteCadastro,//Convert.ToDateTime(dRow["dteCadastro"])
                                 }).ToList();


                    return lista;
                }
            }//            
        }

        public List<RespostaSimuladoDTO> ObterRespostasSimulado(int matricula, int[] questoesSimulado)
        {
            var respostasSimulado = new List<RespostaSimuladoDTO>();
            if (RespostaSimuladoObjecto == null)
            {


                var filtroQuestoes = "(" + Convert.ToString((int)Exercicio.tipoExercicio.SIMULADO) + "," + Convert.ToString((int)Exercicio.tipoExercicio.MONTAPROVA) + ")";

                var lista = new DBQuery().ExecuteQuery(@"select
	                                                qa.intQuestaoID,
                                                    qa.txtLetraAlternativa,
                                                    cro.txtLetraAlternativa as AlternativaRespondida,
                                                    qa.bitCorreta,
                                                    q.bitAnulada,
                                                    cro.dteCadastro
	                                                from tblCartaoResposta_objetiva cro
	                                                inner join tblQuestaoAlternativas qa on cro.intQuestaoID = qa.intQuestaoID and qa.txtLetraAlternativa = cro.txtLetraAlternativa and cro.intExercicioTipoId in " + filtroQuestoes + @"
	                                                inner join tblQuestoes q on qa.intQuestaoID = q.intQuestaoID  and (q.bitCasoClinico is null or q.bitCasoClinico != '1')
	                                                where cro.intClientID = " + matricula + @"
	                                                ", true);

                if (lista.Tables[0].Rows.Count > 0)
                    foreach (DataRow dRow in lista.Tables[0].Rows)
                        respostasSimulado.Add(new RespostaSimuladoDTO()
                        {
                            QuestaoId = Convert.ToInt32(dRow["intQuestaoID"]),
                            Alternativa = dRow["txtLetraAlternativa"].ToString(),
                            Correta = Convert.ToBoolean(dRow["bitCorreta"]),
                            AlternativaRespondida = dRow["AlternativaRespondida"].ToString(),
                            Anulada = Convert.ToBoolean(dRow["bitAnulada"])
                        });
                RespostaSimuladoObjecto = respostasSimulado;
            }
            return (from x in RespostaSimuladoObjecto
                    join a in questoesSimulado on x.QuestaoId equals a
                    select x
                    ).ToList();
        }

        public List<ProvaAluno> ObterProvasAluno(int idFiltro)
        {
            var provasAluno = new List<ProvaAluno>();
            
            using (var ctx = new DesenvContext())
            {
                provasAluno = (from p in ctx.tblExercicio_MontaProva
                                   where p.intFiltroId == idFiltro
                                   && p.bitAtivo == true
                                   select new ProvaAluno
                                   {
                                       ID = p.intID,
                                       Criacao = p.dteDataCriacao
                                   }
                             ).ToList().OrderByDescending(x => x.ID).ToList();
            }

            return provasAluno;
        }

        public List<KeyValuePair<int, int?>> ObterQuestoesMontaProva(ProvaAluno prova)
        {
            var questoes = new List<KeyValuePair<int, int?>>();

            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    var Questao_MontaProva = (from q in ctxMatDir.tblQuestao_MontaProva
                                              where q.intProvaId == prova.ID
                                              select new { id = q.intQuestaoId, tipoExercicio = q.intTipoExercicioId, intProvaId = q.intProvaId }
                                              ).ToList();

                    questoes = (from q in Questao_MontaProva
                                join eh in ctx.tblExercicio_Historico
                                on q.intProvaId equals eh.intExercicioID
                                where q.intProvaId == prova.ID && eh.intExercicioTipo == (int)Exercicio.tipoExercicio.MONTAPROVA
                                select new { id = q.id, tipoExercicio = q.tipoExercicio, historicoId = eh.intHistoricoExercicioID }
                                    )
                                    .OrderByDescending(x => x.historicoId)
                                    .GroupBy(x => x.id)
                                    .AsEnumerable()
                                    .Select(y => new KeyValuePair<int, int?>(y.FirstOrDefault().id, y.FirstOrDefault().tipoExercicio))
                                    .ToList();
                }
            }
            return questoes;
        }

        public int GetQuantidadeQuestoesNaoAssociadas(int idFiltro)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblQuestao_MontaProva.Count(x => x.intFiltroId == idFiltro && x.intProvaId == null);
            }
        }

        public int GetQuantidadeQuestoesAssociadas(int idFiltro)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblQuestao_MontaProva.Count(x => x.intFiltroId == idFiltro && x.intProvaId != null);
            }
        }

        public int GetQuantidadeQuestoesFiltro(int idFiltro)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblQuestao_MontaProva.Count(x => x.intFiltroId == idFiltro);
            }
        }

        public int GetQuantidadeQuestoesAssociadasProva(int idFiltro, int idProva)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblQuestao_MontaProva.Count(x => x.intFiltroId == idFiltro && x.intProvaId == idProva);
            }
        }

        public tblFiltroAluno_MontaProva GetFiltro(int idFiltro)
        {
            using (var ctx = new DesenvContext())
            {
               return ctx.tblFiltroAluno_MontaProva.FirstOrDefault(x => x.intID == idFiltro);
            }
        }

        public int? GetFiltroQuantidadeQuestoes(int idFiltro)
        {
            using (var ctx = new DesenvContext())
            {
                var questoes = (from q in ctx.tblFiltroAluno_MontaProva
                                where q.intID == idFiltro
                                select q.intQtdQuestoes
                                ).ToArray().FirstOrDefault();
                return questoes;
            }
        }

        public void SetFiltroQuantityCounter(int idFiltro, int questoesCount)
        {
            using (var ctx = new DesenvContext())
            {
                ctx.tblFiltroAluno_MontaProva.FirstOrDefault(x => x.intID == idFiltro).intQtdQuestoes = questoesCount;
                ctx.SaveChanges();
            }
        }

        public void DeleteQuestoesNaoAssociadas(int idFiltro)
        {
            using (var ctx = new DesenvContext())
            {
                ctx.SetCommandTimeOut(180);
                SqlCommand sql = new SqlCommand(String.Format("DELETE from tblQuestao_MontaProva WHERE intFiltroId = {0} and intProvaId IS NULL", idFiltro.ToString()));
                ctx.Database.ExecuteSqlRaw(sql.CommandText);
            }
        }

        public List<KeyValuePair<int, int?>> GetQuestoesProva(ProvaAluno prova)
        {
            var questoes = new List<KeyValuePair<int, int?>>();

            using (var ctx = new DesenvContext())
            {
                questoes = (from q in ctx.tblQuestao_MontaProva
                            where q.intProvaId == prova.ID
                            select new { id = q.intQuestaoId, tipoExercicio = q.intTipoExercicioId }
                                )
                                .AsEnumerable()
                                .Select(y => new KeyValuePair<int, int?>(y.id, y.tipoExercicio))
                                .ToList();
            }

            return questoes;
        }

        public List<FiltroAluno> GetFiltrosAluno(int matricula, int page, int limit)
        {
            List<FiltroAluno> listaFiltros;
            using (var ctx = new DesenvContext())
            {
                var retorno = ctx.tblFiltroAluno_MontaProva.Where(x => x.intClientId == matricula && (bool)x.bitAtivo).Select(x => new FiltroAluno()
                {
                    Anos = x.txtAnos,
                    Concursos = x.txtConcursos,
                    Criacao = x.dteDataCriacao,
                    FiltrosEspeciais = x.txtFiltrosEspeciais,
                    Especialidades = x.txtEspecialidades,
                    Matricula = x.intClientId,
                    PalavraChave = x.txtPalavraChave,
                    Id = x.intID,
                    Nome = x.txtNome,
                    QuantidadeQuestoesOrNull = x.intQtdQuestoes
                }).OrderByDescending(x => x.Id);

                if (page > 0 && limit > 0)
                {
                    listaFiltros = retorno.Skip(((page == 0 ? 1 : page) - 1) * limit).Take(limit).ToList();
                }
                else
                {
                    listaFiltros = retorno.ToList();
                }
            }
            return listaFiltros;
        }

		public List<ProvaAluno> ObterContadorDeQuestoes(int matricula)
		{
			using (var ctx = new DesenvContext())
			{
				List<ProvaAluno> provas = new List<ProvaAluno>();
				ctx.tblContadorQuestoes_MontaProva.Where(x => x.intClientId == matricula)
					.ToList().ForEach(x => provas.Add(new ProvaAluno()
					{
						QuantidadeQuestoes = x.intQuantidadeQuestoes,
						Acertos = x.intAcertos,
						Erros = x.intErros,
						NaoRealizadas = x.intNaoRealizadas,
						ID = x.intProvaId
					}));

				return provas;
			}
		}

		public void InserirContadorDeQuestoes(ProvaAluno prova, int matricula)
		{
			using (var ctx = new DesenvContext())
			{
				var existe = ctx.tblContadorQuestoes_MontaProva.Where(x => x.intProvaId == prova.ID).Any();

				if (!existe)
				{
					ctx.tblContadorQuestoes_MontaProva.Add(new tblContadorQuestoes_MontaProva()
					{
						intQuantidadeQuestoes = prova.QuantidadeQuestoes,
						intAcertos = prova.Acertos,
						intErros = prova.Erros,
						intNaoRealizadas = prova.NaoRealizadas,
						intProvaId = prova.ID,
						intClientId = matricula,
						dteDataCriacao = DateTime.Now
					});

					ctx.SaveChanges();
				}
			}
		}
		public void AtualizarContadorDeQuestoes(ProvaAluno prova)
		{
			using (var ctx = new DesenvContext())
			{
				var provaBanco = ctx.tblContadorQuestoes_MontaProva.Where(x => x.intProvaId == prova.ID).FirstOrDefault();
				provaBanco.intAcertos = prova.Acertos;
				provaBanco.intErros = prova.Erros;
				provaBanco.intNaoRealizadas = prova.NaoRealizadas;

				ctx.SaveChanges();
			}
		}
		public List<int> ObterIdProvasPorQuestao(int questao)
		{
			using (var ctx = new DesenvContext())
			{
				var idsProvas = ctx.tblQuestao_MontaProva
					.Where(m => m.intQuestaoId == questao)
					.Select(m => m.intFiltroId)
					.ToList();
				return ctx.tblExercicio_MontaProva
					.Where(m => idsProvas.Contains(m.intFiltroId))
					.Select(m => m.intID)
                    .ToList();
			}
		}

        /// <summary>
        /// Metodo para validação de teste de integração
        /// </summary>
        public tblContadorQuestoes_MontaProva GetContadorQuestoes_MontaProva_TestData(int clientId)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblContadorQuestoes_MontaProva.FirstOrDefault(x => x.intClientId == clientId);
            }
        }
    }

}