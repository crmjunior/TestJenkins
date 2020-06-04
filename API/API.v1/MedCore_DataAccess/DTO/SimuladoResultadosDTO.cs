namespace MedCore_DataAccess.DTO
{
    public class SimuladoResultadosDTO
    {
        public int intClientID { get; set; }
        public int intSimuladoID { get; set; }
        public int intVersaoID { get; set; }
        public int? intAcertos { get; set; }
        public int? intArquivoID { get; set; }
    }
}