using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class RecursoQuestaoConcursoDTO
    {
        public RecursoQuestaoConcursoDTO()
        {
            this.Alternativas = new List<AlternativaQuestaoConcursoDTO>();
        }
        
        public QuestaoConcursoRecursoDTO Questao { get; set; }
        public IEnumerable<AlternativaQuestaoConcursoDTO> Alternativas { get; set; }
        public ConcursoDTO Concurso { get; set; }
        public ForumRecursoDTO ForumRecurso { get; set; }
        public ProvaConcursoDTO Prova { get; set; }
    }
}