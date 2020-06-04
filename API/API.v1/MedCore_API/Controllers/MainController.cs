using System;
using System.Collections.Generic;
using AutoMapper;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO.Base;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCoreAPI.ViewModel.Base;
using Microsoft.AspNetCore.Mvc;

namespace MedCore_API.Controllers
{
    [ApiVersion("2")]
    [ApiVersionNeutral]
    [ApiController]
    public class MainController : BaseService
    {
        public MainController(IMapper mapper) : base(mapper)
        {}

        [HttpGet("Home/Progressos/Matricula/{matricula}/Produto/{idProduto}")]
        public ResponseDTO<List<SemanaProgressoPermissao>> GetProgressosProduto(string idProduto, string matricula, string ano = "0")
        {
            var business = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            return business.GetProgressos(Convert.ToInt32(idProduto), Convert.ToInt32(matricula), Convert.ToInt32(ano));
        }

        [HttpGet("Home/Permissoes/Matricula/{matricula}/Produto/{idProduto}/")]
        public ResponseDTO<List<SemanaProgressoPermissao>> GetPermissoesProduto(string idProduto, string matricula, string ano = "0")
        {
            var business = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            return business.GetPermissoes(Convert.ToInt32(idProduto), Convert.ToInt32(matricula), Convert.ToInt32(ano));
        }

        [HttpPost("Aluno/NickName/Insert/")]

        public int SetNickName(Aluno aluno)
        {
            return new AlunoEntity().SetNickName(aluno);
        }

        [HttpGet("RealizaProva/Simulado/Impresso/Ativo/{idSimulado}/Matricula/{matricula}/IdAplicacao/{ApplicationID}/")]

        public int IsSimuladoImpressoAtivo(string idSimulado = "0", string matricula = "0", string ApplicationID = "16")
        {
            return 1;

        }
        
        [HttpGet("Aluno/PermiteAcesso/Mobile/Matricula/{matricula}/AplicacaoId/{idAplicacao}/Token/{token}/IdDevice/{idDevice}/")]
        public PermissaoDevice GetPermissaoAcessoMovel(string idAplicacao, string matricula, string token, string idDevice)
        {
            //if (Convert.ToInt32(idAplicacao) == 16)
            return new AlunoEntity().GetPermissaoAcesso(Convert.ToInt32(idAplicacao), Convert.ToInt32(matricula), token, (Utilidades.TipoDevice)Convert.ToInt32(idDevice));
            //else
            //return new AlunoEntity().GetPermissaoAcessoMovel(Convert.ToInt32(idAplicacao), Convert.ToInt32(matricula), token, (Utilidades.TipoDevice)Convert.ToInt32(idDevice));
        }

        [HttpPost("Aluno/AutorizaDevice/Insert/")]
        public int SetAutorizacaoTrocaDispositivo(SegurancaDevice device)
        {
            return new AlunoEntity().SetAutorizacaoTrocaDispositivo(device);
        }

        [HttpGet("Menu/idAplicacao/{idAplicacao}/Matricula/{matricula}/")]
        public List<Menu> GetMenuJson(string idAplicacao, string matricula, string completo = "", string produto = "", string versao = "")
        {
            return new MenuBusiness(new MenuEntity(), new PessoaEntity(), new BlackListEntity()).GetPermitidos(Convert.ToInt32(idAplicacao), Convert.ToInt32(matricula), Convert.ToInt32(completo), Convert.ToInt32(produto), versao);
        }

        [HttpPost("Aluno/ScreenshotReport/Insert/")]
        public bool SetMedsoftScreenshotReport(SegurancaDevice seguranca)
        {
            return new AlunoEntity().SetMedsoftScreenshotReport(seguranca);
        }
        

        [HttpGet("Features/Desktop/CadastroNickname/Ativa/")]
        public bool IsFeatureDesktopCadastroNicknameAtiva()
        {
            return new PermissaoRegraItemEntity().IsFeatureAtiva((int)Aplicacoes.MsProDesktop, Constants.IdObjetoDesktopCadastroNickname);
        }

        [HttpGet("Aulas/Ano/{ano}/IdProduto/{idProduto}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]

        public List<Semana> GetAulaSemanas(string ano, string idProduto, string matricula, string idAplicacao)
        {
            return new AulaEntity().GetSemanas(Convert.ToInt32(ano), Convert.ToInt32(idProduto), Convert.ToInt32(matricula), Semana.TipoAba.Aulas);
        }

        [HttpGet("Questoes/Ano/{ano}/IdProduto/{idProduto}/Matricula/{matricula}/IdAplicacao/{idAplicacao}/")]

         public List<Semana> GetQuestoesSemanas(string ano, string idProduto, string matricula, string idAplicacao)
        {
            return new AulaEntity().GetSemanas(Convert.ToInt32(ano), Convert.ToInt32(idProduto), Convert.ToInt32(matricula), Semana.TipoAba.Questoes);
        }

        [HttpGet("Aulas/Ano/{ano}/Matricula/{matricula}/Produto/{produto}/Progresso")]
        public List<ProgressoSemana> GetProgressoAulas(string ano, string matricula, string produto)
        {
            return new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity()).GetPercentSemanas(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(produto), Semana.TipoAba.Aulas);
        }

        [HttpGet("Revalida/Ano/{ano}/Matricula/{matricula}/Produto/{produto}/Progresso")]
        public List<ProgressoSemana> GetProgressoRevalida(string ano, string matricula, string produto)
        {
            return new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity()).GetPercentSemanas(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(produto), Semana.TipoAba.Revalida);
        }

        [HttpGet("Cronograma/Produto/{produto}/Ano/{ano}/Menu/{id}/")]
        public CronogramaSemana GetCronograma(string produto, string ano, string id, string matricula)
        {
            var business = new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity());
            return business.GetCronogramaAluno(Convert.ToInt32(produto), Convert.ToInt32(ano), Convert.ToInt32(id), Convert.ToInt32(matricula), VersaoAplicacao);
        }

        [HttpGet("Questoes/Ano/{ano}/Matricula/{matricula}/Produto/{produto}/Progresso")]
        public List<ProgressoSemana> GetProgressoQuestoes(string ano, string matricula, string produto)
        {
            return new CronogramaBusiness(new AulaEntity(), new MednetEntity(), new MaterialApostilaEntity(), new RevalidaEntity(), new CronogramaEntity()).GetPercentSemanas(Convert.ToInt32(ano), Convert.ToInt32(matricula), Convert.ToInt32(produto), Semana.TipoAba.Questoes);
        }

        [HttpGet("Aluno/Produtos/Matricula/{matricula}/IdAplicacao/{idaplicacao}/")]

        public List<Combo> GetProdutos(string matricula, string idaplicacao)
        {
            base.SetStateHeadersFromRequest();
           return new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsProCache(Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao), VersaoAplicacao);
        }

        [HttpPost("AvaliacaoAluno/")]
        public int SetAvaliacaoAluno(AvaliacaoAluno avaliacao)
        {
            return new AvaliacaoAlunoEntity().SetAvaliacao(avaliacao);
        }

        [HttpGet("AvaliacaoAluno/Matricula/{matricula}")]
         public int GetAvaliar(string matricula)
        {
            return new AvaliacaoAlunoEntity().GetAvaliar(Convert.ToInt32(matricula));
        }


        [HttpGet("VerificaAppAtualizado/VersaoApp/{versaoApp}/Aplicacao/{idAplicacao}")]
        public Aplicacao GetVerificacaoAppAtualizado(string versaoApp, string idAplicacao)
        {
            return new AplicativoEntity().VerificaAppAtualizado(versaoApp, Convert.ToInt32(idAplicacao));
        }

        [HttpPost("Aluno/ClipboardReport/Insert/")]
        public bool SetMedsoftClipboardReport(SegurancaDevice seguranca)
        {
            return new AlunoEntity().SetMedsoftClipboardReport(seguranca);
        }

        [HttpGet("Utilidades/Config/Cache")]
        public List<CacheConfig> GetCacheConfig()
        {
            return new ConfigBusiness(new ConfigEntity()).GetCacheConfig();
        }

        [HttpPost("utilidades/email/enviar/")]
        public int SendMail(Email e)
        {
            return Utilidades.SendMailProfile("MSCross", e.mailTo, e.mailSubject, e.mailBody, e.copyRecipients, e.BlindCopyRecipients);
        }

        [HttpGet("Login/Acesso/VersaoApp/{versaoApp}/")]
        public int IsVersaoAppValida(string versaoApp, string produtoId = "")
        {
            return new VersaoAppPermissaoBusiness(new VersaoAppPermissaoEntity()).IsVersaoValida(versaoApp, Convert.ToInt32(produtoId));
        }
        
    }
}