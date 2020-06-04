using MedCore_DataAccess.Business.Enums;

namespace MedCore_DataAccess.DTO.Base
{
    public class BaseResponse
    {
        public bool Sucesso { get; set; }
        public string TituloMensagem { get; set; }
        public string Mensagem { get; set; }
        public string TipoErro { get; set; }
        public TipoErroAcesso? ETipoErro { get; set; }
    }
}