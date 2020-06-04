using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class SimuladoDTO
    {
		#region Ctor
		public SimuladoDTO()
		{
			PodeAlterarEspecialidades = true;
			Especialidades = new List<SimuladoEspecialidadeDTO>();
		}
		#endregion

		#region Properties Members
		public int ID { get; set; }

		public int? TemaID { get; set; }

		public int? LivroID { get; set; }

		public string Origem { get; set; }

		public string Nome { get; set; }

		public string Descricao { get; set; }

		public int? Ordem { get; set; }

		public int Duracao { get; set; }

		public int? ConcursoID { get; set; }

		public int? Ano { get; set; }

		public bool? ParaWeb { get; set; }

		public DateTime? DataInicioWeb { get; set; }

		public DateTime? DataTerminoWeb { get; set; }

		public DateTime? DataLiberacaoWeb { get; set; }

		public DateTime? DataLiberacaoGabarito { get; set; }

		public DateTime? DataLiberacaoComentario { get; set; }

		public DateTime? DataInicioConsultaRanking { get; set; }

		public DateTime? DataLimiteParaRanking { get; set; }

		public bool? EhDemonstracao { get; set; }

		public string CodigoEspecialidade { get; set; }

		public int? InstituicaoID { get; set; }

		public string CaminhoGabarito { get; set; }

		public int? QuantidadeQuestoes { get; set; }

		public bool? RankingWeb { get; set; }

		public bool? GabaritoWeb { get; set; }

		public bool? RankingFinalWeb { get; set; }

		public string CodigoQuestoes { get; set; }

		public bool? VideoComentariosWeb { get; set; }

		public int? QuantidadeQuestoesCasoClinico { get; set; }

		public Guid Identificador { get; set; }

		public DateTime DataUltimaAtualizacao { get; set; }

		public bool CronogramaAprovado { get; set; }

		public int TipoId { get; set; }

		public bool Geral { get; set; }

		public bool Online { get; set; }

		public int? PesoProvaObjetiva { get; set; }

		public DateTime? DataInicio { get; set; }

		public DateTime? DataFim { get; set; }

		public string ExercicioName { get; set; }

		public DateTime? DtLiberacaoRanking { get; set; }
		#endregion

		#region Navigation Members
		public List<SimuladoEspecialidadeDTO> Especialidades { get; set; }
		#endregion

		#region Business Members
		public bool PodeAlterarEspecialidades { get; set; }
		#endregion
	}
}