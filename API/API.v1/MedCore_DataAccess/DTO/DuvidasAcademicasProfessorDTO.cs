using System;

namespace MedCore_DataAccess.DTO
{
    public class DuvidasAcademicasProfessorDTO
    {
        public int DuvidaId { get; set; }

        public int? QuestaoId { get; set; }

        public string TextoDuvida { get; set; }

        public string TextoQuestao { get; set; }

        public string Data { get; set; }

        public DateTime DataOriginal { get; set; }

        public bool PrimeirasDuvidas { get; set; }

        public bool MinhaDuvidaApostila { get; set; }

        public bool MinhaDuvidaSimuladoAnteriores { get; set; }

        public bool MinhaDuvidaQuestaoConcursoEspecialidade { get; set; }

        public bool MinhaDuvidaQuestaoConcurso { get; set; }

        public bool MinhaDuvidaSimulado { get; set; }

        public string EntidadeEspecialidade { get; set; }

        public int EntidadeApostila { get; set; }

        public string EntidadeConcurso { get; set; }

        public string EntidadeSimulado { get; set; }

        public string EspecialidadeConcurso { get; set; }

        public string EspecialidadeSimulado { get; set; }

        public string EntidadeApostilaDescricao { get; set; }

        public bool? AprovacaoMedGrupo { get; set; }

        public bool? RespostaMedGrupo { get; set; }

        public int? TipoExercicioId { get; set; }

        public int? ApostilaId { get; set; }

        public bool BitEnviada { get; set; }

        public bool BitEncaminhada { get; set; }

        public string Professor { get; set; }

        public int IdProfessor { get; set; }
    }
}