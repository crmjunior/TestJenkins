using Shared.Enums;

namespace Shared.Entities
{
    public class BaseResponse
    {
        public bool Sucesso { get; set; }
        public string TituloMensagem { get; set; }
        public string Mensagem { get; set; }
    }
}