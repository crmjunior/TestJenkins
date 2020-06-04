using System.ComponentModel;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "ConcursoProva", Namespace = "a")]
    public class ProvaRecurso : Exercicio
    {
        [DataMember(Name = "SiglaConcurso")]
        public string SiglaConcurso { get; set; }

        [DataMember(Name = "Acesso")]
        public string Acesso { get; set; }

        //[DataMember(Name = "Descricao", EmitDefaultValue = false)]
        //public string Descricao { get; set; }

        [DataMember(Name = "ConcursoId")]
        public int ConcursoId { get; set; }

        [DataMember(Name = "IdStatus")]
        public int IdStatus { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "IdGrupo")]
        public int IdGrupo { get; set; }

        [DataMember(Name = "Grupo")]
        public string Grupo { get; set; }

        [DataMember(Name = "Prazo")]
        public string Prazo { get; set; }

        [DataMember(Name = "TemVereditoBanca")]
        public bool TemVereditoBanca { get; set; }

        [DataMember(Name = "SemQuestoes")]
        public bool SemQuestoes { get; set; }

        [DataMember(Name = "TemGabaritoPos")]
        public bool TemGabaritoPos { get; set; }

        [DataMember(Name = "Comunicado")]
        public MensagemRecurso Comunicado { get; set; }

        [DataMember(Name = "StatusComunicado")]
        public bool StatusComunicado { get; set; }

        [DataMember(Name = "Favorita")]
        public bool Favorita { get; set; }

        public enum StatusProva
        {
            [Description("")]
            Inexistente = -1,
            [Description("Em análise")]
            RecursosEmAnalise = 1,
            Bloqueado = 2,
            [Description("Próximo")]
            RecursosPróximos = 6,
            AguardandoInteração = 0,
            RecursosExpirados = 9,
            BloqueadoParaNovosRecursos = 13,
            SobDemanda = 14
        }
        
        public enum GrupoConcurso
        {
            SemGrupo = 0,
            Analise = 1,
            Proximos = 2,
            Expirados = 3
        }

        public enum TipoProva
        {
            [Description("Objetiva")]
            Objetiva = 1,
            [Description("Discursiva")]
            Discursiva = 2
        }

        public enum GrandeAreaProva
        {
            R3PEDIATRIA = 34,
            R3CIRURGIA = 39,
            R3CLINICA = 41,
            R4GO = 161
        }
    }
}