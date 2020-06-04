using System.Collections.Generic;

namespace MedCore_DataAccess.Entidades
{
    public class DuvidaAcademicaFiltro
    {
        public int ClientId { get; set; }

        public int? ExercicioId { get; set; }

        private string _DuvidaId;
        public dynamic DuvidaId { 
            get => _DuvidaId; 
            set => this._DuvidaId = value.ToString();            
        }

        private string _QuestaoId;
        public dynamic QuestaoId { 
            get => _QuestaoId; 
            set => this._QuestaoId = value.ToString();            
        }

        public bool IsAcademico { get; set; }

        public bool BitSemVinculo { get; set; }

        public bool BitSemInteracao { get; set; }

        public int? RespostaId { get; set; }

        public bool IsCoordenador { get; set; }

        public bool MinhasApostilas { get; set; }

        public int? ApostilaId { get; set; }

        public int? ConcursoId { get; set; }

        public int? SimuladoId { get; set; }

        public int? AplicacaoId { get; set; }

        public int? TipoCategoriaApostila { get; set; }

        public bool BitEncaminhadas { get; set; }

        public string NumeroCategoriaApostila { get; set; }

        public IList<int> IdsMateriais { get; set; }

        public IList<int> IdsApostilas { get; set; }

        public IList<int> SiglasConcurso { get; set; }

        public IList<int> IdsSimulados { get; set; }

        public IList<int> IdsProfessores { get; set; }

        public bool BitResponderMaisTarde { get; set; }

        public bool BitTodosConcursos { get; set; }

        public bool BitTodosSimulados { get; set; }

        public bool BitTodasApostilas { get; set; }

        public bool BitTodosMateriais { get; set; }

        public bool BitTodosProfessores { get; set; }

        public bool? BitAtiva { get; set; }

        public TipoDuvida TipoDuvida { get; set; }

        public bool BitTodas { get; set; }

        public bool BitMinhas { get; set; }

        public bool BitArquivadas { get; set; }

        public bool BitFavoritas { get; set; }

        public bool MaisDe7Dias { get; set; }

        public bool TemRascunho { get; set; }

        public bool BitDenunciadas { get; set; }

        public bool BitRespostaHomologadasMed { get; set; }

        public bool BitRespostaMed { get; set; }

        public bool BitMinhasRespostas { get; set; }

        public bool BitEnviadas { get; set; }

        public int UltimaDuvidaId { get; set; }

        public int QuantidadeDuvidas { get; set; }

        public int QuantidadeReplicas { get; set; }

        public int Page{ get; set; }
    }
}