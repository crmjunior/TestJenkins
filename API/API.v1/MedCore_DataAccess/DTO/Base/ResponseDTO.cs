using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.DTO.Base
{
    public class ResponseDTO<T> : BaseResponse
    {   
        public T Retorno { get; set; }
        public List<PermissaoAcessoItem> LstOrdemVendaMsg { get; set; }
    }
}