using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class ProvaQuestoesConcursoDTO
    {
        public ProvaConcursoDTO Prova { get; set; }
        public IEnumerable<QuestaoConcursoRecursoDTO> Questoes { get; set; }
    }
}