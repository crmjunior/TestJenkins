using MedCore_DataAccess.Business.Enums;

namespace MedCore_DataAccess.DTO
{
    public class ValidacaoDTO
    {
        public StatusRetorno Status { get; set; }
        public string Mensagem { get; set; }
    }
}