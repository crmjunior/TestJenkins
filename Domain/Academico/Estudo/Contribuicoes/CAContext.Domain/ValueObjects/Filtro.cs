using System.Collections.Generic;

namespace CAContext.Domain.ValueObjects
{
    public class Filtro
    {
        public int Matricula { get; set; }
        public int ContribuicaoId { get; set; }
        public int ApostilaId { get; set; }
        public int MedGrupoID { get; set; }
        public bool IsProfessor { get; set; }
        public bool IsPendente { get; set; }
        public bool IsAprovado { get; set; }
        public string CodigoMarcacao { get; set; }
        public bool IsArquivada { get; set; }
        public bool IsAprovarMaisTarde { get; set; }
        public bool IsEncaminhado { get; set; }
        public bool IsPublicadoMedGrupo { get; set; }
        public bool ByText { get; set; }
        public bool ByImage { get; set; }
        public bool ByVideo { get; set; }
        public bool ByAudio { get; set; }
        public bool JustMyAid { get; set; }
        public IEnumerable<int> IdsProfessores { get; set; }
        public IEnumerable<int> TiposInteracoes { get; set; }
        public bool IsPublicadasPorMim { get; set; }
        public int Page { get; set; }        
    }
}