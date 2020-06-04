using MedCore_DataAccess.Contracts.Enums;

namespace MedCore_DataAccess.DTO
{
    public class ImagemDTO
    {
        public int IdImagem { get; set; }
        public string Descricao { get; set; }
        public string NomeArquivo { get; set; }
        public string UrlImagem { get; set; }
        public string UrlImagemOtimizada { get; set; }
        public byte[] ByteArrayImagem { get; set; }
        public AndamentoCadastroQuestao AndamentoCadastro { get; set; }
    }
}