using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class ProvaAlunosFavoritoDTO
    {
        public ProvaConcursoDTO Prova { get; set; }
        public List<int> MatriculasFavoritaram { get; set; }
    }
}