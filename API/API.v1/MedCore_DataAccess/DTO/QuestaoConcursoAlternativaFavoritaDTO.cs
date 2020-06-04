using System;

namespace MedCore_DataAccess.DTO
{
    public class QuestaoConcursoAlternativaFavoritaDTO
    {
        public int IdQuestao { get; set; }
        public string LetraAlternativaSelecionada { get; set; }
        public string LetraUltimaAlternativaSelecionada { get; set; }
        public DateTime Data { get; set; }
    }
}