using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class ConcursoDTO
    {      
        public string Descricao { get; set; }

        public string Sigla { get; set; }

        public string SiglaEstado { get; set; }

        public IEnumerable<int> IdsProva { get; set; }
    }
}