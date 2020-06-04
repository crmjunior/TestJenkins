namespace MedCore_DataAccess.DTO
{
    public class AlternativaSimualdoAgendadoDTO
    {
        public int Id { get; set; }
        public string Letra { get; set; }
        public string Nome { get; set; }
        public bool Editar { get; set; }
        public bool Selecionada { get; set; }
    }
}