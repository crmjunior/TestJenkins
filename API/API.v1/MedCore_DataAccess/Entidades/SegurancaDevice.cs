namespace MedCore_DataAccess.Entidades
{
    public class SegurancaDevice
    {
        public int IdSeguranca { get; set; }
        public int Matricula { get; set; }
        public int IdDevice { get; set; }
        public string Token { get; set; }
        public double DataCadastro { get; set; }
        public int IdAplicacao { get; set; }
        public int? ScreenshotCounter { get; set; }
    }
}