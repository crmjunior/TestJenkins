namespace MedCore_DataAccess.DTO
{
    public class ApostilaDTO
    {
        public int ID { get; set; }
        public string Capa { get; set; }
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public int Ano { get; set; }
        public int IdProduto { get; set; }
        public int IdProdutoGrupo { get; set; }
        public int IdGrandeArea { get; set; }
        public int IdSubEspecialidade { get; set; }
        public int IdEntidade { get; set; }
        public string ProdutosAdicionais { get; set; }
        public string NomeCompleto { get; set; }
        public bool LiberadaRevisao { get; set; }
        public bool Status { get; set; }
    }
}