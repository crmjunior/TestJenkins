using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class QuestaoConcursoRecursoDTO
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public string Enunciado { get; set; }
        public string CasoClinico { get; set; }
        public string GabaritoLetra { get; set; }
        public List<string> GabaritoLetras { get; set; }
        public string GabaritoDiscursiva { get; set; }
        public string GabaritoTipo { get; set; }
        public string AlternativaSelecionada { get; set; }
        public string StatusQuestao { get; set; }
        public bool? AnuladaPosRecurso { get; set; }
        public bool Anulada { get; set; }
        public int TotalRespostas { get; set; }
        public int[] EnunciadoImagensId { get; set; }
        public bool Discursiva { get; set; }
    }
}