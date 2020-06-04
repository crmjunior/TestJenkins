using System;
using MedCore_DataAccess.Business.Enums;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Business
{
    public class ClienteBusiness : BaseBusiness, IClienteBusiness
    {

        private readonly IClienteEntity _clienteDataRepository;

        public ClienteBusiness(IClienteEntity clienteDataRepository)
        {
            _clienteDataRepository = clienteDataRepository;
        }

        public int CadastrarSenha(string register, string senha, Aplicacoes aplicacao, string senhaAnterior = "", int id = 0)
        {
            var isMedSoftPro = (aplicacao == Aplicacoes.MsProMobile);
            var isMedSoftProDesktop = ((int)aplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON);
            var isMedEletro = (aplicacao == Aplicacoes.MEDELETRO);

            var idAplicacao = (int)aplicacao;

            var filtro = new AlunoDTO();

            if ((isMedSoftPro || isMedSoftProDesktop || isMedEletro) && Utilidades.IsValidEmail(register))
            {
                filtro.Email = register.ToLower().Trim();
            }
            else
            {
                filtro.Register = register;
                filtro.Id = id;
            }

            var aluno = _clienteDataRepository.GetAlunoPorFiltros(filtro);

            if(aluno != null && aluno.Id != 0)
            {
                var novaSenha = new AlunoSenhaDTO
                {
                    ClientId = aluno.Id,
                    AplicacaoId = idAplicacao,
                    Senha = senha,
                    Data = DateTime.Now
                };

                var alunoSenha = _clienteDataRepository.GetAlunoSenha(aluno.Id);

                if (alunoSenha == null)
                {
                    _clienteDataRepository.InserirAlunoSenha(novaSenha);
                }
                else if (alunoSenha.Senha != senhaAnterior)
                {
                    return 0;
                }
                else
                {
                    novaSenha.Id = alunoSenha.Id;
                    _clienteDataRepository.AlterarAlunoSenha(novaSenha);
                }
                return 1;
            }
            else
            {
                return 0;
            }
           

        }

        public ValidacaoDTO CadastrarSenhaAluno(string identificador, string senhaAtual, string novaSenha, Aplicacoes aplicacao)
        {
            var filtro = new AlunoDTO();
            var validacao = new ValidacaoDTO
            {
                Status = StatusRetorno.Falha,
                Mensagem = Constants.Messages.Acesso.AlunoInexistente.GetDescription()
            };
            
            if (Utilidades.IsValidEmail(identificador))
            {
                filtro.Email = identificador;
            }
            else
            {
                filtro.Register = identificador;
            }

            var aluno = _clienteDataRepository.GetAlunoPorFiltros(filtro);

            if(aluno != null && aluno.Id != default(int))
            {
                var result = CadastrarSenha(aluno.Register, novaSenha, aplicacao, senhaAtual, aluno.Id);
                
                if((result == default(int)))
                {
                    validacao.Status = StatusRetorno.Falha;
                    validacao.Mensagem = Constants.Messages.Acesso.SenhaIncorreta.GetDescription();
                }
                else
                {
                    validacao.Status = StatusRetorno.Sucesso;
                    validacao.Mensagem = string.Empty;
                }
            }
            return validacao;
        }

        public RecuperaSenhaDTO EnviarEmailEsqueciSenha(string identificador, Aplicacoes aplicacao)
        {
            Cliente cliente = new Cliente();
            var filtro = new AlunoDTO();
            var recupera = new RecuperaSenhaDTO();
            recupera.Validacao = ValidaRecuperaSenha.Inexistente;

            if (Utilidades.IsValidEmail(identificador))
            {
                filtro.Email = identificador;
            }
            else
            {
                filtro.Register = identificador;
            }

            var aluno = _clienteDataRepository.GetAlunoPorFiltros(filtro);

            if (aluno != null && aluno.Id != default(int))
            {
                cliente.Register = aluno.Register;
                var result = _clienteDataRepository.UpdateEsqueciSenha(cliente, aplicacao);
                recupera.Validacao = result.Key > default(int) ? ValidaRecuperaSenha.EmailEnviado : ValidaRecuperaSenha.Inexistente;
            }

            return recupera;
        }
    }
}