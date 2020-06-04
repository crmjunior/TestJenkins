namespace MedCore_DataAccess.Util
{
    public class CriterioOrdenacaoBuscaTexto
    {
        public int Ordem { get; set; }

        public string Texto { get; set; }
    }

    public enum TipoOrdenacao
    {
        Concurso = 1,
        UF = 2
    }
}