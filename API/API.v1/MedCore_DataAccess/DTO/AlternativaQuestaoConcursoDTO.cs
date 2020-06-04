namespace MedCore_DataAccess.DTO
{
    public class AlternativaQuestaoConcursoDTO
    {
        public int IdAlternativa { get; set; }
        public string Descricao { get; set; }
        public string Letra { get; set; }
        public bool CorretaOficial { get; set; }
        public bool CorretaPreliminar { get; set; }
        public int QtdResponderam { get; set; }
        public string UrlImagem { get; set; }
    }
}