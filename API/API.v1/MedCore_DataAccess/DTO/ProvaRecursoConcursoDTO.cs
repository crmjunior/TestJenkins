using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.DTO
{
    public class ProvaRecursoConcursoDTO
    {
        public int Id { get; set; }
        public int IdConcurso { get; set; }
        public int? Ano { get; set; }
        public string Nome { get; set; }
        public string NomeCompleto { get; set; }
        public ProvaRecurso.TipoProva Tipo { get; set; }
        public ProvaRecurso.GrupoConcurso Grupo { get; set; }
        public int IdStatus { get; set; }
        public bool Liberado { get; set; }
        public bool Favorito { get; set; }
        public bool AlteradaBanca { get; set; }
        public bool Discursiva { get; set; }
        public bool UploadLiberado { get; set; }
        public int TipoOrder { get; set; }
        public string[] Subespecialidades { get; set; }
        public List<int> GrandeAreas { get; set; }
    }
}