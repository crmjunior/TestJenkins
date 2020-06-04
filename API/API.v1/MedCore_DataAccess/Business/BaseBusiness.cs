using MedCore_DataAccess.Business.Enums;
using MedCore_DataAccess.DTO.Base;

namespace MedCore_DataAccess.Business
{
    public class BaseBusiness
    {
        public BaseBusiness()
        {
            Response = new BaseResponse();
        }

        private BaseResponse Response { get; set; }

        public void SetResponse(bool sucesso)
        {
            Response.Sucesso = sucesso;
        }

        public void SetResponse(bool sucesso, string tituloMensagem)
        {
            SetResponse(sucesso);
            Response.TituloMensagem = tituloMensagem;
        }

        public void SetResponse(bool sucesso, string tituloMensagem, string mensagem)
        {
            SetResponse(sucesso, tituloMensagem);
            Response.Mensagem = mensagem;
        }

        public void SetResponse(bool sucesso, string tituloMensagem, string mensagem, TipoErroAcesso eTipoErro)
        {
            SetResponse(sucesso, tituloMensagem, mensagem);
            Response.ETipoErro = eTipoErro;
        }

        public void SetResponse(bool sucesso, string tituloMensagem, string mensagem, TipoErroAcesso eTipoErro, string tipoErro)
        {
            SetResponse(sucesso, tituloMensagem, mensagem, eTipoErro);
            Response.TipoErro = tipoErro;
        }

        public BaseResponse GetResponse()
        {
            return this.Response;
        }        
    }
}