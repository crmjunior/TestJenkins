using System;
using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;
using static MedCore_DataAccess.Repository.CronogramaEntity;

namespace MedCore_DataAccessTests.EntitiesMockData
{
    public class MenuEntityTestData
    {
        public List<Menu> ObterListaDeTodosMenusAplicacao0()
        {
            return new System.Collections.Generic.List<Menu>
                {
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = -1,
		                Novo = 1,
		                Ordem = 1,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapMain",
		                VersaoMinima = "0.0.0",
                        Id = 84,
                        Nome= "MAIN",
                        PermiteOffline= 0 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = -1,
		                Novo = 1,
		                Ordem = 2,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapAreaDeTreinamento",
		                VersaoMinima = "0.0.0",
                        Id = 85,
                        Nome= "ÁREA DE TREINAMENTO",
                        PermiteOffline= 0 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = -1,
		                Novo = 1,
		                Ordem = 3,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapMedcode",
		                VersaoMinima = "1.5.7",
                        Id = 86,
                        Nome= "MEDCODE",
                        PermiteOffline= 0 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = -1,
		                Novo = 1,
		                Ordem = 4,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapContribuir",
		                VersaoMinima = "0.0.0",
                        Id = 87,
                        Nome= "CONTRIBUIR",
                        PermiteOffline= 0 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = -1,
		                Novo = 1,
		                Ordem = 5,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapDuvidasAcademicas",
		                VersaoMinima = "0.0.0",
                        Id = 88,
                        Nome= "DÚVIDAS ACADÊMICAS",
                        PermiteOffline= 0 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = -1,
		                Novo = 1,
		                Ordem = 6,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapCronograma",
		                VersaoMinima = "0.0.0",
                        Id=89,
                        Nome=  "CRONOGRAMA",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = -1,
		                Novo = 1,
		                Ordem = 7,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapRecursos",
		                VersaoMinima = "0.0.0",
                        Id=90,
                        Nome=  "RECURSOS",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = -1,
		                Novo = 1,
		                Ordem = 8,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapFaleConosco",
		                VersaoMinima = "0.0.0",
                        Id=91,
                        Nome=  "FALE CONOSCO",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 84,
		                Novo = 1,
		                Ordem = 1,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "_selectAulas",
		                VersaoMinima = "0.0.0",
                        Id=92,
                        Nome=  "AULAS",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 84,
		                Novo = 1,
		                Ordem = 2,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "_selectMaterial",
		                VersaoMinima = "2.0.0",
                        Id=93,
                        Nome=  "MATERIAL",
                        PermiteOffline= 1 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 84,
		                Novo = 1,
		                Ordem = 3,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "_selectQuestoes",
		                VersaoMinima = "0.0.0",
                        Id=94,
                        Nome=  "QUESTÕES",
                        PermiteOffline= 0 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 85,
		                Novo = 1,
		                Ordem = 1,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapEstatistica",
		                VersaoMinima = "0.0.0",
                        Id=95,
                        Nome=  "ESTATÍSTICAS",
                        PermiteOffline= 0 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 85,
		                Novo = 1,
		                Ordem = 2,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapSimulado",
		                VersaoMinima = "0.0.0",
                        Id=96,
                        Nome=  "SIMULADOS",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 85,
		                Novo = 1,
		                Ordem = 3,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapConcursosNaIntegra",
		                VersaoMinima = "0.0.0",
                        Id=97,
                        Nome=  "CONCURSOS NA ÍNTEGRA",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 85,
		                Novo = 1,
		                Ordem = 4,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapQuestoesDeApostila",
		                VersaoMinima = "0.0.0",
                        Id=98,
                        Nome=  "QUESTÕES DE APOSTILA",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 85,
		                Novo = 1,
		                Ordem = 5,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapMontaProva",
		                VersaoMinima = "0.0.0",
                        Id=99,
                        Nome=  "MONTA PROVA",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 84,
		                Novo = 1,
		                Ordem = 4,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "_selectRevalida",
		                VersaoMinima = "0.0.0",
                        Id=119,
                        Nome=  "REVALIDA",
                        PermiteOffline= 0 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = -1,
		                Novo = 1,
		                Ordem = 9,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "vm.onTapSlidesDeAula",
		                VersaoMinima = "1.5.7",
                        Id=129,
                        Nome=  "SLIDES DE AULA",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 96,
		                Novo = 1,
		                Ordem = 1,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=160,
                        Nome=  "QUESTÃO",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 96,
		                Novo = 1,
		                Ordem = 2,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=161,
                        Nome=  "COMENTÁRIOS DA QUESTÃO",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 96,
		                Novo = 1,
		                Ordem = 3,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=162,
                        Nome=  "DÚVIDAS DA QUESTÃO",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 96,
		                Novo = 1,
		                Ordem = 4,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=163,
                        Nome=  "CARTÃO RESPOSTA",
                        PermiteOffline= 0 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 97,
		                Novo = 1,
		                Ordem = 1,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=164,
                        Nome=  "QUESTÃO",
                        PermiteOffline= 0  
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 97,
		                Novo = 1,
		                Ordem = 2,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=165,
                        Nome=  "COMENTÁRIOS DA QUESTÃO",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 97,
		                Novo = 1,
		                Ordem = 3,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=166,
                        Nome=  "DÚVIDAS DA QUESTÃO",
                        PermiteOffline= 0  
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 97,
		                Novo = 1,
		                Ordem = 4,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=167,
                        Nome=  "CARTÃO RESPOSTA",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 99,
		                Novo = 1,
		                Ordem = 1,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=168,
                        Nome=  "QUESTÃO",
                        PermiteOffline= 0  
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 99,
		                Novo = 1,
		                Ordem = 2,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=169,
                        Nome=  "COMENTÁRIOS DA QUESTÃO",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 99,
		                Novo = 1,
		                Ordem = 3,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=170,
                        Nome=  "DÚVIDAS DA QUESTÃO",
                        PermiteOffline= 0 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 99,
		                Novo = 1,
		                Ordem = 4,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=171,
                        Nome=  "CARTÃO RESPOSTA",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 93,
		                Novo = 1,
		                Ordem = 1,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=172,
                        Nome=  "FEED DÚVIDAS",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 93,
		                Novo = 1,
		                Ordem = 2,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=173,
                        Nome=  "INSERIR DÚVIDA",
                        PermiteOffline= 0 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 93,
		                Novo = 1,
		                Ordem = 3,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=174,
                        Nome=  "HISTÓRICO DÚVIDAS",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 94,
		                Novo = 1,
		                Ordem = 1,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=175,
                        Nome=  "QUESTÃO",
                        PermiteOffline= 0  
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 94,
		                Novo = 1,
		                Ordem = 2,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=176,
                        Nome=  "COMENTÁRIOS DA QUESTÃO",
                        PermiteOffline= 0 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 94,
		                Novo = 1,
		                Ordem = 3,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=177,
                        Nome=  "RECURSOS",
                        PermiteOffline= 0
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 94,
		                Novo = 1,
		                Ordem = 4,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=178,
                        Nome=  "DÚVIDAS DA QUESTÃO",
                        PermiteOffline= 0 
	                },
	                new Menu
	                {
		                Autenticacao = 0,
		                IdAplicacao = 17,
		                IdMensagem = 0,
		                IdPai = 94,
		                Novo = 1,
		                Ordem = 5,
		                SubMenu = new System.Collections.Generic.List<Menu>
		                {
		                },
		                SubMenusIds = null,
		                Target = null,
		                Url = "",
		                VersaoMinima = "0.0.0",
                        Id=179,
                        Nome=  "CARTÃO RESPOSTA",
                        PermiteOffline= 0 
	                }
                };
        }

        public List<PermissaoRegra> ObterListaDePermissaoRegraParaAlunoSemPermissao()
        {
          return new System.Collections.Generic.List<PermissaoRegra>
        {
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 1,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 641,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = -1,
		        ObjetoId = 86,
		        Ordem = 10,
		        Regra = new Regra
		        {
			        Ativo = false,
			        DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			        Descricao = null,
			        EmployeeId = 0,
			        Id = 9,
			        RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			        {
			        }
		        }
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 1,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 641,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = -1,
		        ObjetoId = 93,
		        Ordem = -1,
		        Regra = new Regra
		        {
			        Ativo = false,
			        DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			        Descricao = null,
			        EmployeeId = 0,
			        Id = 9,
			        RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			        {
			        }
		        }
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 1,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 641,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = -1,
		        ObjetoId = 129,
		        Ordem = 10,
		        Regra = new Regra
		        {
			        Ativo = false,
			        DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			        Descricao = null,
			        EmployeeId = 0,
			        Id = 9,
			        RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			        {
			        }
		        }
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        },
	        new PermissaoRegra
	        {
		        AcessoId = 0,
		        Ativo = false,
		        DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		        Descricao = null,
		        DescricaoMensagem = null,
		        EmployeeId = 0,
		        Id = 0,
		        InterruptorId = 0,
		        IsDataLimite = false,
		        MensagemId = null,
		        ObjetoId = 0,
		        Ordem = 0,
		        Regra = null
	        }
        };

        }

        public List<PermissaoRegra> ObterListaDePermissaoRegraParaAlunoComPermissao()
        {
            return new System.Collections.Generic.List<PermissaoRegra>
{
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 427,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 84,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 6,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 427,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 85,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 6,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 427,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 86,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 6,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 427,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 88,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 6,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 427,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 92,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 6,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 642,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 93,
		Ordem = -2,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 724,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 427,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 94,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 6,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 427,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 96,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 6,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 427,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 97,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 6,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 427,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 99,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 6,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 0,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 0,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = null,
		ObjetoId = 0,
		Ordem = 0,
		Regra = null
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 427,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 129,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 6,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 160,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 161,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 162,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 163,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 164,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 165,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 166,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 167,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 168,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 169,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 170,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 171,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 172,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 173,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 174,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 175,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 176,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 177,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 178,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	},
	new PermissaoRegra
	{
		AcessoId = 3,
		Ativo = false,
		DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
		Descricao = null,
		DescricaoMensagem = null,
		EmployeeId = 0,
		Id = 428,
		InterruptorId = 0,
		IsDataLimite = false,
		MensagemId = -1,
		ObjetoId = 179,
		Ordem = 1,
		Regra = new Regra
		{
			Ativo = false,
			DataCriacao = new DateTime(1, 1, 1, 0, 0, 0),
			DataUltimaAlteracao = new DateTime(1, 1, 1, 0, 0, 0),
			Descricao = null,
			EmployeeId = 0,
			Id = 7,
			RegraDetalhes = new System.Collections.Generic.List<RegraCondicao>
			{
			}
		}
	}
};

        }

        private Pessoa ObterAluno()
        {
            return new Pessoa
            {
                AnexoDossie = new System.Collections.Generic.List<AnexoDossie>
                {
                },
                Bairro = null,
                BlacklistLog = new System.Collections.Generic.List<BlacklistLog>
                {
                },
                Bloqueios = new System.Collections.Generic.List<Bloqueio>
                {
                },
                Cep = null,
                Cidade = null,
                Complemento = null,
                Email = null,
                Email2 = null,
                Email3 = null,
                Endereco = null,
                EnderecoNumero = null,
                EnderecoReferencia = null,
                Estado = null,
                Faculdade = null,
                Foto = null,
                FotoPerfil = null,
                ID = 229009,
                IdEstado = 0,
                Motivos = new System.Collections.Generic.List<MotivoHistorico>
                {
                },
                NickName = null,
                Nome = "ALNO POIARES VIEIRA",
                PercentVisualizado = 0,
                Register = "00546237150",
                Senha = null,
                Sexo = 0,
                Telefone = null,
                TipoPessoa = Pessoa.EnumTipoPessoa.NaoExiste,
                TipoPessoaDescricao = null,
                UrlAvatar = null,
                UsuarioInclusaoID = 0
            };

        }

        public List<Pessoa> ObterListaDeAlunos()
        {
            var pessoasMocked = new List<Pessoa>();
            pessoasMocked.Add(ObterAluno());
            return pessoasMocked;
        }

        private Pessoa ObterAlunoBlackList()
        {
            return new Pessoa
            {
                AnexoDossie = new System.Collections.Generic.List<AnexoDossie>
                {
                },
                Bairro = null,
                BlacklistLog = new System.Collections.Generic.List<BlacklistLog>
	{
		new BlacklistLog
		{
			bitBloqueio = false,
			ClientNome = null,
			DataFormatada = "09/08/2018 13:56:26",
			dteData = new DateTime(2018, 8, 9, 13, 56, 26),
			Email = "alnopoiaresvieira@gmail.com",
			EmployeeId = 96096,
			EmployeeNome = "ROBERTA DA SILVA FONSECA                                                                            ",
			Faculdade = "",
			intClientId = 229009,
			intId = 137,
			intTipoBloqueio = 4,
			Nome = "ALNO POIARES VIEIRA",
			Tipo = BlacklistLog.EnumTipo.PendenteAprovacao,
			txtMotivo = "Aprovado para a lista de Alunos",
			txtRegister = "00546237150"
		},
		new BlacklistLog
		{
			bitBloqueio = true,
			ClientNome = null,
			DataFormatada = "09/08/2018 13:56:26",
			dteData = new DateTime(2018, 8, 9, 13, 56, 26),
			Email = "alnopoiaresvieira@gmail.com",
			EmployeeId = 96096,
			EmployeeNome = "ROBERTA DA SILVA FONSECA                                                                            ",
			Faculdade = "",
			intClientId = 229009,
			intId = 136,
			intTipoBloqueio = 4,
			Nome = "ALNO POIARES VIEIRA",
			Tipo = BlacklistLog.EnumTipo.Alunos,
			txtMotivo = "Bloqueado por prints excessivos no MED READER",
			txtRegister = "00546237150"
		},
		new BlacklistLog
		{
			bitBloqueio = true,
			ClientNome = null,
			DataFormatada = "09/08/2018 13:55:55",
			dteData = new DateTime(2018, 8, 9, 13, 55, 55),
			Email = "alnopoiaresvieira@gmail.com",
			EmployeeId = 208222,
			EmployeeNome = "BRUNA MONTEBELLO JACOBSEN                                                                           ",
			Faculdade = "",
			intClientId = 229009,
			intId = 135,
			intTipoBloqueio = 4,
			Nome = "ALNO POIARES VIEIRA",
			Tipo = BlacklistLog.EnumTipo.PendenteAprovacao,
			txtMotivo = "Bloqueado por prints excessivos no MED READER",
			txtRegister = "00546237150"
		},
		new BlacklistLog
		{
			bitBloqueio = false,
			ClientNome = null,
			DataFormatada = "09/08/2018 13:55:09",
			dteData = new DateTime(2018, 8, 9, 13, 55, 9),
			Email = "alnopoiaresvieira@gmail.com",
			EmployeeId = 208222,
			EmployeeNome = "BRUNA MONTEBELLO JACOBSEN                                                                           ",
			Faculdade = "",
			intClientId = 229009,
			intId = 134,
			intTipoBloqueio = 2,
			Nome = "ALNO POIARES VIEIRA",
			Tipo = BlacklistLog.EnumTipo.PendenteAprovacao,
			txtMotivo = ".",
			txtRegister = "00546237150"
		},
		new BlacklistLog
		{
			bitBloqueio = true,
			ClientNome = null,
			DataFormatada = "09/08/2018 13:54:36",
			dteData = new DateTime(2018, 8, 9, 13, 54, 36),
			Email = "alnopoiaresvieira@gmail.com",
			EmployeeId = 208222,
			EmployeeNome = "BRUNA MONTEBELLO JACOBSEN                                                                           ",
			Faculdade = "",
			intClientId = 229009,
			intId = 133,
			intTipoBloqueio = 2,
			Nome = "ALNO POIARES VIEIRA",
			Tipo = BlacklistLog.EnumTipo.PendenteAprovacao,
			txtMotivo = "PRINTS MED READER",
			txtRegister = "00546237150"
		}
	},
                Bloqueios = new System.Collections.Generic.List<Bloqueio>
	{
		new Bloqueio
		{
			AplicacaoId = 5,
			AutorizadorId = 0,
			Bloqueado = false,
			Categoria = new BlackListCategoria
			{
				CategoriaID = 5,
				Descricao = "Outros"
			},
			dteDateTimeEnd = new DateTime(1, 1, 1, 0, 0, 0),
			dteDateTimeStart = new DateTime(2018, 8, 9, 13, 56, 26),
			MotivoBloqueio = "Bloqueado por prints excessivos no MED READER",
			MotivoDesbloqueio = null,
			SolicitadorId = 0,
			TabelaBloqueio = Bloqueio.TipoBloqueio.Aplicacao
		}
	},
                Cep = null,
                Cidade = null,
                Complemento = null,
                Email = "alnopoiaresvieira@gmail.com",
                Email2 = null,
                Email3 = null,
                Endereco = null,
                EnderecoNumero = null,
                EnderecoReferencia = null,
                Estado = null,
                Faculdade = null,
                Foto = null,
                FotoPerfil = null,
                ID = 229009,
                IdEstado = 0,
                Motivos = new System.Collections.Generic.List<MotivoHistorico>
                {
                },
                NickName = null,
                Nome = "ALNO POIARES VIEIRA",
                PercentVisualizado = 0,
                Register = "00546237150",
                Senha = null,
                Sexo = 0,
                Telefone = null,
                TipoPessoa = Pessoa.EnumTipoPessoa.NaoExiste,
                TipoPessoaDescricao = null,
                UrlAvatar = null,
                UsuarioInclusaoID = 0
            };

        }

        public List<Pessoa> ObterListaDeAlunosBlackList()
        {
            var pessoasMocked = new List<Pessoa>();
            pessoasMocked.Add(ObterAlunoBlackList());
            return pessoasMocked;
        }

        public List<Pessoa> ObterListaDeAlunosBlackListVazia()
        {
            var pessoasMocked = new List<Pessoa>();
            return pessoasMocked;
        }

        public List<Menu> ObtemListaMenusMockados()
        {
            var listMenu = new List<Menu>();
            listMenu.Add(new Menu()
            {
                Id = (int)Utilidades.EMenuAccessObject.RecursosRUm,
                Nome = "R Um",
                PermiteOffline = 1,
                VersaoMinimaOffline = "1.0.0",
                IdMensagem = -1,
                IdPai = -1
            });

            listMenu.Add(new Menu()
            {
                Id = (int)ESubMenus.Aulas,
                Nome = "Menu Aulas",
                PermiteOffline = 0,
                IdMensagem = -1
            });

            return listMenu;
        }

        public List<PermissaoRegra> ObtemPermissaoRegraMockados()
        {
            var listPermissaoRegra = new List<PermissaoRegra>();

            listPermissaoRegra.Add(new PermissaoRegra()
            {
                Id = 1,
                ObjetoId = (int)Utilidades.EMenuAccessObject.RecursosRUm,
                Ativo = true,
                Descricao = "Materiais",
                MensagemId = -1

            });

            listPermissaoRegra.Add(new PermissaoRegra()
            {
                Id = 2,
                ObjetoId = (int)ESubMenus.Aulas,
                Ativo = true,
                Descricao = "Aulas",
                MensagemId = -1
            });

            return listPermissaoRegra;
        }
    }
}