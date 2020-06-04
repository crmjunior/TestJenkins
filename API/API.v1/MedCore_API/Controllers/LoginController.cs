using System;
using AutoMapper;
using MedCore_API.ViewModel.Base;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO.Base;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCoreAPI.ViewModel.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace MedCore_API.Controllers
{
    [ApiVersion("2")]
    [ApiVersionNeutral]
    [ApiController]
    public class LoginController : BaseService
    {
        public LoginController(IMapper mapper) 
            : base(mapper) {

        }

        [HttpGet("Login/Acesso/Registro/{register}/")]
        public AlunoMedsoft GetAluno(string register, string idaplicacao)
        {
            return GetAluno2(register, idaplicacao).Retorno;
        }

        [HttpGet("Login2/Acesso/Registro/{register}/")]
        public ResponseDTO<AlunoMedsoft> GetAluno2(string register, string idaplicacao)
        {
            return new AlunoEntity().GetAlunoMsCross(register, Convert.ToInt32(idaplicacao));
        }

        [HttpGet("Login/Acesso/Mensagens/Registro/{register}/")]
        public ResponseDTO<AlunoMedsoft> GetAlunoComMensagens(string register, string idaplicacao, string appversion)
        {
            return new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), new ConfigEntity(), new VersaoAppPermissaoEntity()).GetAcessoAluno(register, Convert.ToInt32(idaplicacao), appversion);
        }

        [HttpGet("Login/Acesso/")]
        [MapToApiVersion("2")]
        public ResultViewModel<LoginViewModel> GetAcessoAluno()
        {
            Request.Headers.TryGetValue("appVersion", out StringValues appVersion);
            Request.Headers.TryGetValue("idAplicacao", out StringValues idAplicacao);
            Request.Headers.TryGetValue("register", out StringValues register);
            Request.Headers.TryGetValue("senha", out StringValues senha);
            Request.Headers.TryGetValue("tokenDevice", out StringValues tokenDevice);
            Request.Headers.TryGetValue("idDevice", out StringValues idDevice);

            var alunoBusiness = new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), new ConfigEntity(), new VersaoAppPermissaoEntity());

            var result = Execute(() =>
            {
                var acessoAluno = alunoBusiness.GetAcessoAluno(register, senha, Convert.ToInt32(idAplicacao), appVersion, tokenDevice, Convert.ToInt32(idDevice));
                return acessoAluno;
            }, true);

            return GetResultViewModel<LoginViewModel, AlunoMedsoft>(result, alunoBusiness.GetResponse());
        }

        [HttpGet("Login/PreAcesso/")]
        [MapToApiVersion("2")]
        public ResultViewModel<PreLoginViewModel> GetPreAcessoAluno()
        {
            Request.Headers.TryGetValue("idAplicacao", out StringValues idAplicacao);
            Request.Headers.TryGetValue("register", out StringValues register);

            var alunoBusiness = new AlunoBusiness(new AlunoEntity(), new ClienteEntity(), new ConfigEntity(), new VersaoAppPermissaoEntity());

            var result = Execute(() =>
            {
                var acessoAluno = alunoBusiness.GetPreAcessoAluno(register, Convert.ToInt32(idAplicacao));
                return acessoAluno;
            }, true);

            return GetResultViewModel<PreLoginViewModel, AlunoMedsoft>(result, alunoBusiness.GetResponse());
        }

        [HttpGet("Login/Mensagem/SenhaIncorreta/AplicacaoId/{idAplicacao}/")]
        public MensagemRecurso GetMensagemSenhaIncorreta(string idAplicacao)
        {
            return new ClienteEntity().GetMensagemLogin(Convert.ToInt32(idAplicacao), Convert.ToInt32(MensagemRecurso.TipoMensagemLogin.SenhaIncorreta));
        }

        [HttpGet("Login/Mensagem/SenhaInexistente/AplicacaoId/{idAplicacao}/")]
        public MensagemRecurso GetMensagemSenhaInexistente(string idAplicacao)
        {
            return new ClienteEntity().GetMensagemLogin(Convert.ToInt32(idAplicacao), Convert.ToInt32(MensagemRecurso.TipoMensagemLogin.SenhaInexistente));
        }

        [HttpGet("Login/Mensagem/SenhaRecadastro/AplicacaoId/{idAplicacao}/")]
        public MensagemRecurso GetMensagemSenhaRecadastro(string idAplicacao)
        {
            return new ClienteEntity().GetMensagemLogin(Convert.ToInt32(idAplicacao), Convert.ToInt32(MensagemRecurso.TipoMensagemLogin.SenhaRecadastro));
        }

        [HttpGet("Aluno/PermissaoInadimplente/Registro/{registro}/AplicacaoId/{aplicativo}/")]
        public PermissaoInadimplencia GetPermissaoInadimplencia(string registro, string aplicativo)
        {
            return new AlunoBusiness(new AlunoEntity(), new ClienteEntity()).GetPermissaoInadimplencia(registro, Convert.ToInt32(aplicativo), VersaoAplicacao);
        }

        [HttpPost("Clientes/Senha/EsqueciSenha/")]
        public Cliente SetEsqueciSenha(Cliente cliente, string idaplicacao)
        {
            return new ClienteEntity().UpdateEsqueciSenha(cliente, aplicacao: (Aplicacoes)Convert.ToInt32(idaplicacao));
        }

        [HttpPost("Clientes/Senha/Inserir/")]
        public int SetSenha(Pessoa pessoa)
        {
            return new ClienteEntity().CadastrarSenha(pessoa.Register, pessoa.Senha, id: pessoa.ID, aplicacao: Aplicacoes.MEDSOFT);
        }

        [MapToApiVersion("2")]
        [HttpPost("Clientes/Senha/Inserir/")]
        public int SetCadastroSenha(CadastroSenha cadastro)
        {
            var clienteBusiness = new ClienteBusiness(new ClienteEntity());

            var result = Execute(() =>
            {
                var cadastroSenha = clienteBusiness.CadastrarSenha(cadastro.register, cadastro.senha, (Aplicacoes)Convert.ToInt32(cadastro.idAplicacao), cadastro.senhaAnterior);

                return cadastroSenha;
            }, true);

            return result;
        }

        [HttpPost("Aluno/PermissaoInadimplente/AceiteTermos/Insert/")]
        public int SetAceiteTermosPermissaoInadimplencia(PermissaoInadimplencia aceiteTermo)
        {
            return new AlunoEntity().SetAceiteTermosPermissaoInadimplencia(aceiteTermo);
        }

        [HttpPost("Login/Acesso/DeviceToken")]
        public int SetDeviceToken(DeviceToken deviceToken)
        {
            return new AlunoEntity().SetDeviceToken(deviceToken);
        }

        [HttpPost("Aluno/LogAcesso/Insert/")]
        public int SetLogAcesso(LogLogin log)
        {
             return new LogEntity().InsertAcessoLogin(log);
        }

        [HttpGet("TimeoutInadimplencia/")]
        public float GetTempoInadimplenciaTimeoutParametro()
        {
            return new ConfigBusiness(new ConfigEntity()).GetTempoInadimplenciaTimeoutParametro();
        }

        [HttpGet("Login/Link/EsqueciEmail/AplicacaoId/{aplicacaoId}/")]
        public string GetLinkEsqueciEmail(string idAplicacao)
        {
            return new ClienteEntity().GetLinkEsqueciEmail(Convert.ToInt32(idAplicacao));
        }

    }
}