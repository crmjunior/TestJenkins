using System;
using System.Collections.Generic;
using AutoMapper;
using MedCore_API.ViewModel.Base;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCoreAPI.ViewModel.Base;
using Microsoft.AspNetCore.Mvc;

namespace MedCore_API.Controllers
{   
    [ApiVersion("2")]
    [ApiVersionNeutral]
    [ApiController]
    public class NotificacaoController : BaseService
    {
        public NotificacaoController(IMapper mapper) : base(mapper)
        { }
        
        [HttpGet("Aluno/GetNotificacoes/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]
        public List<Notificacao> GetNotificacoes(string matricula, string idAplicacao, string completo = "", string produto = "", string versao = "")
        {
           return new NotificacaoBusiness(new NotificacaoEntity(), new AccessEntity(), new AlunoEntity(),new NotificacaoDuvidasAcademicasEntity(), new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity())).GetNotificacoesPorPerfil(Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao), Convert.ToInt32(completo), Convert.ToInt32(produto), versao);
        }

        [HttpPost("Aluno/GetNotificacao/Inserir/")]

        public int SetNotificacao(Notificacao notificacao)
        {
            return new NotificacaoBusiness(new NotificacaoEntity(), new AccessEntity(), new AlunoEntity(),new NotificacaoDuvidasAcademicasEntity(), new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity())).SetNotificacaoLida(notificacao);
        }

        [MapToApiVersion("2")]
        [HttpGet("Notificacoes/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]

          public ResultViewModel<NotificacoesClassificacaoViewModel> GetNotificacoesClassificacaoViewModel(string matricula, string idAplicacao, string versaoAplicacao)
        {
            var result = Execute(() =>
            {
                var business = new NotificacaoBusiness(new NotificacaoEntity(), new AccessEntity(), new AlunoEntity(), new NotificacaoDuvidasAcademicasEntity(), new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity()));
                var notificacoes = business.GetNotificacoesClassificadas(Convert.ToInt32(matricula), Convert.ToInt32(idAplicacao), versaoAplicacao);
                return notificacoes;
            }, true);
            return GetResultViewModel<NotificacoesClassificacaoViewModel, NotificacoesClassificadasDTO>(result);

          
        }
    }
}