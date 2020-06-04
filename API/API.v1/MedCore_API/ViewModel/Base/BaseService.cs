using System;
using AutoMapper;
using MedCore_DataAccess.DTO.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace MedCoreAPI.ViewModel.Base
{
    public abstract class BaseService : ControllerBase
    {
        protected IMapper Mapper { get; private set; } 
        protected BaseService(IMapper mapper)
        {
            Mapper = mapper;
        }

        protected void SetStateHeadersFromRequest()
        {
            Request.Headers.TryGetValue("appVersion", out StringValues appVersion);
            Request.Headers.TryGetValue("tokenLogin", out StringValues tokenLogin);
            Request.Headers.TryGetValue("hash", out StringValues hash);
            Request.Headers.TryGetValue("matricula", out StringValues matricula);
            Request.Headers.TryGetValue("idAplicacao", out StringValues idAplicacao);
            Request.Headers.TryGetValue("idDevice", out StringValues idDevice);
            Request.Headers.TryGetValue("employeeID", out StringValues employeeID);


            VersaoAplicacao = appVersion;
            TokenLogin = tokenLogin;
            Hash = hash;
            Matricula = (!string.IsNullOrEmpty(matricula))? Convert.ToInt32(matricula): 0;
            IdAplicacao = (!string.IsNullOrEmpty(idAplicacao))? Convert.ToInt32(idAplicacao): 0;
            IdDevice = (!string.IsNullOrEmpty(idDevice))? Convert.ToInt32(idDevice) : 0;
            EmployeeID = (!string.IsNullOrEmpty(employeeID))? Convert.ToInt32(employeeID) : 0;
        }

        protected int Matricula { get; set; }
        protected string VersaoAplicacao { get; set; }
        protected int IdAplicacao { get; set; }
        protected string TokenLogin { get; set; }
        protected int IdDevice { get; set; }
        public int EmployeeID { get; set; }
        protected string Hash { get; set; }
        private string Mensagem { get; set; }
        private bool Sucesso { get; set; }
        private string TipoException { get; set; }
        private string InnerException { get; set; }
        public const string MensagemSucesso = "OK";

        protected ResultViewModel<T> GetResultViewModel<T, K>(K obj, BaseResponse baseResponse = null)
        {
            var viewModel = new ResultViewModel<T>();

            viewModel.ETipoErro = baseResponse != null ? baseResponse.ETipoErro : null;
            viewModel.TituloMensagem = baseResponse != null ? baseResponse.TituloMensagem : null;
            viewModel.Sucesso = baseResponse != null ? baseResponse.Sucesso : Sucesso;
            viewModel.Mensagem = baseResponse != null ? baseResponse.Mensagem : Mensagem;
            viewModel.TipoErro = baseResponse != null ? baseResponse.TipoErro : TipoException;
            viewModel.Retorno = Sucesso ? Mapper.Map<T>(obj): default(T);
      
            return viewModel;
        }

        protected ResultViewModel<T> GetResultViewModel<T>(T obj)
        {
            var viewModel = new ResultViewModel<T>()
            {
                Sucesso = Sucesso,
                Mensagem = Mensagem,
                TipoErro = TipoException,
                Retorno = Sucesso ? obj : default(T)
            };
            return viewModel;
        }

        protected void Execute(Action metodo, bool throwException = false)
        {
            try
            {
                metodo();
                Mensagem = MensagemSucesso;
                Sucesso = true;
                InnerException = null;
                TipoException = null;
            }
            catch (Exception ex)
            {
                if (throwException) throw;
                Mensagem = GetExceptionMessage(ex);
                Sucesso = false;
                InnerException = ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message.ToString() : ex.InnerException.Message : null;
                TipoException = ex.GetType().ToString();
            }
        }

        private string GetExceptionMessage(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return ex.InnerException.Message;
            }
            else
            {
                return ex.Message;
            }
        }

        protected T Execute<T>(Func<T> metodo, bool throwException = true) 
        {
            try
            {
                var result = metodo();
                Mensagem = MensagemSucesso;
                Sucesso = true;
                TipoException = null;
                InnerException = null;
                return result;
            }
            catch (Exception ex)
            {
                if(throwException)
                {
                    throw;
                }
                Mensagem = ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message.ToString() : ex.InnerException.Message : ex.Message;
                Sucesso = false;
                InnerException = ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message.ToString() : ex.InnerException.Message : null;
                TipoException = ex.GetType().ToString();
                return default(T);
            }
        }
    }
}