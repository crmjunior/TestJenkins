using System;

namespace MedCore_DataAccess.DTO
{
    public class ConcursoQuestaoDTO
    {
        public string SiglaConcurso { get; set; }
        public int OrdemQuestao { get; set; }
        public int AnoConcurso { get; set; }
        public string NomeProva { get; set; }
        public int IdQuestao { get; set; }
        public int AnoQuestao { get; set; }
        public DateTime? DataQuestao { get; set; }
    }
}