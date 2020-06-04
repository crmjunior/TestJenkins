using System;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;

namespace MedCore_DataAccess.Business
{
    public class ConcursoBusiness
    {
        private readonly IConcursoData _concursoRepository;

		public ConcursoBusiness(ConcursoEntity concursoEntity)
		{
			this._concursoRepository = concursoEntity;
		}

		public AlunoConcursoEstatistica GetConcursoStatsAluno(int idExercicio, int idMatricula, bool anulada = false)
		{

			int[] tipoExercicio = new int[] { (int)Exercicio.tipoExercicio.CONCURSO, (int)Exercicio.tipoExercicio.MONTAPROVA };

			try
			{

				var questoes = _concursoRepository.GetQuestoesbyIdExercicio(idExercicio);


				var questoesFiltradas = questoes
					.Where(x => x.Anulada == false && x.AlternativaCorreta == true)
					.GroupBy(x => x.QuestaoId)
					.Select(x => new { QuestaoId = x.Key, AlternativasCorretas = x.Select(y => y.Alternativa).ToList() })
					.ToList();


				var respostasAluno = _concursoRepository.GetRespostabyIdExercicioIDMatricula(idExercicio, idMatricula, tipoExercicio);



				var respostasAlunoNaoAnuladas = (from ra in respostasAluno
												 join q in questoesFiltradas
												 on ra.QuestaoId equals q.QuestaoId
												 select new { QuestaoId = ra.QuestaoId, Alternativa = ra.Alternativa, ra.Ultimoregistro }
												  ).Distinct().ToList();

				var respostasAlunosemduplicada = (from c in respostasAlunoNaoAnuladas
												  group c by c.QuestaoId into grp
												  where grp.Count() > 1
												  select grp.Key
											 ).ToList();

				if (respostasAlunosemduplicada.Count > 0)
				{
					var itemComDuplicata = respostasAlunoNaoAnuladas.Where(a => respostasAlunosemduplicada.Contains(a.QuestaoId)).OrderBy(b => b.Ultimoregistro).ToList();
					if (itemComDuplicata.Count > 0)
					{
						foreach (var item in itemComDuplicata)
						{
							var indice = respostasAlunoNaoAnuladas.Where(a => a.QuestaoId == item.QuestaoId && a.Ultimoregistro > item.Ultimoregistro).ToList();
							if (indice.Count > 0)
								respostasAlunoNaoAnuladas.Remove(item);
						}

					}
				}

				var estatistica = new AlunoConcursoEstatistica();

				estatistica.TotalQuestoes = questoes.GroupBy(x => x.QuestaoId).Count();
				estatistica.NaoRealizadas = estatistica.TotalQuestoes - respostasAlunoNaoAnuladas.Select(a => a.QuestaoId).Distinct().Count();

				estatistica.Acertos = (from q in questoesFiltradas
									   join r in respostasAlunoNaoAnuladas
									   on q.QuestaoId equals r.QuestaoId
									   where q.AlternativasCorretas.Contains(r.Alternativa)
									   select q.QuestaoId
										).Distinct().Count();

				estatistica.Erros = respostasAlunoNaoAnuladas.Count() - estatistica.Acertos;

				if (anulada && respostasAlunoNaoAnuladas.Count() > 0)
				{
					var anuladas = estatistica.TotalQuestoes - questoesFiltradas.Count();
					estatistica.NaoRealizadas -= anuladas;
					estatistica.Acertos += anuladas;
				}

				estatistica.Nota = estatistica.Acertos;

				return estatistica;


			}
			catch
			{

				throw;
			}
		}
    }
}