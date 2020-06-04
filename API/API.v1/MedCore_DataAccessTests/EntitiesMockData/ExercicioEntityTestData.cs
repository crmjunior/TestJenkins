using System;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccessTests.EntitiesMockData
{
    public class ExercicioEntityTestData
    {
        public static string HASH_RMAIS = "xO20dCIM88lV6JiHO2QQGjOlsFCDdpwbV2fj6hwuRc7y9d+TdWfNIFskkQPW4ndSJcpsOPDK8hmglKArqjHeC7skd+m3E9dNR2AE12keUH74JkZYV6HkECcacLGxquTc5pcCuTaPpO3Oy6SnVt9zuA2Fpx6GcwLP6lVpzs0m6C3R967TvQxNAnNWTKDNDQ8+";
        
        public static Dictionary<bool, List<int>> GetIdsExerciciosRealizadosAluno()
        {
            var dic = new Dictionary<bool, List<int>>();
            List<int> listaIds = new List<int>();
            listaIds.Add(0);
            listaIds.Add(1);
            listaIds.Add(2);
            listaIds.Add(3);
            listaIds.Add(4);

            dic.Add(true, listaIds);

            return dic;
        }

        public static string GetHashPermissaoRMais(int matricula, bool permissaoCirurgia = true)
        {
            var hash = new HashPermissaoDTO();
            var result = new Dictionary<Utilidades.EMenuAccessObject, bool>();
            result.Add(Utilidades.EMenuAccessObject.RecursosRMaisCirurgia, permissaoCirurgia);
            result.Add(Utilidades.EMenuAccessObject.RecursosRMaisClinica, false);
            result.Add(Utilidades.EMenuAccessObject.RecursosRMaisPediatria, false);
            result.Add(Utilidades.EMenuAccessObject.RecursosRMaisGO, false);

            hash.Permissoes = new Dictionary<string, object>();
            hash.Permissoes.Add("RMais", result);
            hash.Permissoes.Add("matricula", matricula);
            return Criptografia.CryptAES(hash);
        }

        public static Concurso GetConcursoConfiguracaoR1() {
            var Concurso = new Concurso
            {

                Ano = 2019,
                CartoesResposta = new CartoesResposta
                {
                    HistoricoId = 16761059,
                    Questoes = { }
                },
                IdExercicio = 924913,
                Nome = "UFF|HOSPITAL UNIVERSITÁRIO ANTÔNIO PEDRO",
                TipoProva = "ACESSO DIRETO 1",
                PermissaoProva = new PermissaoProva
                {
                    ComentarioTexto = 1,
                    ComentarioVideo = 1,
                    Cronometro = 0,
                    Estatística = 1,
                    Favorito = 1,
                    Gabarito = 1,
                    Recursos = 1
                },
                QtdQuestoes = 100
                

            };
            Concurso.PermissaoProva.Impressao = ValidarPermissaoImpressao(Concurso.TipoProva);
            return Concurso;

        }
        public static int ValidarPermissaoImpressao(string prova) {
            return prova.ToUpper().Contains("R3") || prova.ToUpper().Contains("ACESSO DIRETO") ? 1 : 0;
        }
        public static ExercicioDTO GetExercicioPeloAlunoByFilters2019CPMED()
        {
            

           return new ExercicioDTO()
            {
                Ano = 2019,
                Descricao = "2019 CPMED - Geral 1",
                ID = 0,
                ExercicioName = "Teste",
                DataInicio = 49852255487,
                DataFim = 49852255489,
                IdTipoRealizacao = 1,
                Online = 0,
                Ativo = true,
                Duracao = 50,
                DtLiberacaoRanking = DateTime.Now.AddDays(10),
                TipoId = 2,
                bitAndamento = false
            };
          
        }
        public static Especialidades GetEspacialidadeByFilters2019CPMED()
        {
            return new Especialidades()
            {
                new Especialidade()
                {
                    Id = 0,
                    IdAreaAcademica = 1,
                    IntEmployeeID = 96409,
                    Editavel = true,
                    DataClassificacao = DateTime.Now.Date
                }
            };
        }

        public static List<Exercicio> GetSimuladosRealizadosPeloAlunoByFilters2019CPMED()
        {
            List<Exercicio> exercicios = new List<Exercicio>();

            var exercicio = new Exercicio()
            {
                Ano = 2019,
                Descricao = "2019 CPMED - Geral 1",
                ID = 0,
                ExercicioName = "Teste",
                DataInicio = 49852255487,
                DataFim = 49852255489,
                IdTipoRealizacao = 1,
                Online = 0,
                Ativo = true,
                Duracao = 50,
                DtLiberacaoRanking = DateTime.Now.Date.AddDays(10),
                TipoId = 2,
                Especialidades = new Especialidades()
                {
                    new Especialidade()
                    {
                        Id = 0,
                        IdAreaAcademica = 1,
                        IntEmployeeID = 96409,
                        Editavel = true,
                        DataClassificacao = DateTime.Now.Date
                    }
                }
            };
            exercicios.Add(exercicio);
            return exercicios;
        }
        public static List<Exercicio> GetSimuladosRealizadosPeloAlunoBy2019CPMED()
        {
            List<Exercicio> exercicios = new List<Exercicio>();

            var exercicio = new Exercicio()
            {
                ID = 0
                , ExercicioName = "Teste"
                , TipoId = 2
                , EstadoID = 0
                , RegiaoID = 0
                , Descricao = "2019 CPMED - Geral 1"
                , Ordem = 0
                , Ano = 2019
                , Duracao = 50
                , QtdQuestoes = 0
                , TempoTolerancia = 0
                , TipoApostilaId = 0
                , StatusId = 0
                , Ativo = true
                , EntidadeApostilaID =0
                , Acertos = 0
                , IsPremium = false
                , IdConcurso = 0
                , TempoExcedido = 0
                , Ranqueado = 0
                , HistoricoId = 0
                , DataInicio = 49852255487
                , DataFim = 49852255489
                , IdTipoRealizacao = 1
                , Realizado = 0
                , Online = 0
                , DtLiberacaoRanking = DateTime.Now.Date.AddDays(10)
                , DtUnixLiberacaoRanking = 0
                , Especialidade =
                    new Especialidade()
                    {
                        Id = 0,
                        IdAreaAcademica = 1,
                        IntEmployeeID = 96409,
                        Editavel = true,
                        DataClassificacao = DateTime.Now.Date,
                        Descricao = "2019 CPMED - Geral 1"
                    }
                ,
                Especialidades = new Especialidades()
                {
                    new Especialidade()
                    {
                        Id = 0,
                        IdAreaAcademica = 1,
                        IntEmployeeID = 96409,
                        Editavel = true,
                        DataClassificacao = DateTime.Now.Date,
                        Descricao = "2019 CPMED - Geral 1"
                    }
                }
                ,  Atual = 0
                ,  BitAndamento = false
            };
            exercicios.Add(exercicio);
            return exercicios;
        }
        public static List<Produto> GetListaProdutos()
        {
            return new List<Produto>()
            {
                new Produto()
                {
                    ID = 0,
                    IDProduto = 91,
                    Ano =2019,
                    txtComment = "2019 Módulo \"online\" RAC iMED MEDREADER"

                }
            };
        }

        public static List<Exercicio> GetSimuladosRealizadosPeloAlunoByFilters2017MED()
        {
            List<Exercicio> exercicios = new List<Exercicio>();

            var exercicio = new Exercicio()
            {
                Ano = 2017,
                Descricao = "2017 SIM01 - Geral 1",
                ID = 1,
                ExercicioName = "Teste",
                DataInicio = 49852255487,
                DataFim = 49852255489,
                IdTipoRealizacao = 1,
                Online = 0,
                Ativo = true,
                Duracao = 50,
                DtLiberacaoRanking = new DateTime()
            };

            exercicios.Add(exercicio);

            exercicios.Add(new Exercicio()
            {
                Ano = 2017,
                Descricao = "2017 SIM01 - Geral 2",
                ID = 2,
                ExercicioName = "Teste",
                DataInicio = 49852255487,
                DataFim = 49852255489,
                IdTipoRealizacao = 1,
                Online = 0,
                Ativo = true,
                Duracao = 50,
                DtLiberacaoRanking = new DateTime()
            });

            exercicios.Add(exercicio);

            exercicios.Add(exercicio);

            exercicios.Add(new Exercicio()
            {
                Ano = 2017,
                Descricao = "2017 SIM01 - Geral 3",
                ID = 3,
                ExercicioName = "Teste",
                DataInicio = 49852255487,
                DataFim = 49852255489,
                IdTipoRealizacao = 1,
                Online = 0,
                Ativo = true,
                Duracao = 50,
                DtLiberacaoRanking = new DateTime()
            });

            exercicios.Add(new Exercicio()
            {
                Ano = 2017,
                Descricao = "2017 SIM01 - Geral 4",
                ID = 4,
                ExercicioName = "Teste",
                DataInicio = 49852255487,
                DataFim = 49852255489,
                IdTipoRealizacao = 1,
                Online = 1,
                Ativo = true,
                Duracao = 50,
                DtLiberacaoRanking = new DateTime()
            });

            exercicios.Add(exercicio);

            exercicios.Add(exercicio);
            return exercicios;
        }

        public static AlunoConcursoEstatistica ObterEstatisticaAlunoSimulado(int Acertos, int Erros, int NaoRealizadas)
        {
            return new AlunoConcursoEstatistica
            {
                    Acertos = Acertos,
                    Erros = Erros,
                    NaoRealizadas = NaoRealizadas,
                    Nota = Acertos,
                    TotalQuestoes = Acertos + Erros + NaoRealizadas
            };
        }

        public static List<PosicaoRankingDTO> ObterRankingPorSimuladoVazio()
        {
            return new List<PosicaoRankingDTO>
            {
            };

        }

        public static List<PosicaoRankingDTO> ObterRankingPorSimulado10AlunosBemColocados()
        {
            return new List<PosicaoRankingDTO>
                {
	                new  PosicaoRankingDTO
	                {
		                Acertos = 95,
		                Nota = 9.5,
		                Posicao = "1º"
	                },
	                new  PosicaoRankingDTO
	                {
		                Acertos = 94,
		                Nota = 9.3999999999999986,
		                Posicao = "2º"
	                },
	                new  PosicaoRankingDTO
	                {
		                Acertos = 94,
		                Nota = 9.3999999999999986,
		                Posicao = "2º"
	                },
	                new  PosicaoRankingDTO
	                {
		                Acertos = 93,
		                Nota = 9.3,
		                Posicao = "4º"
	                },
	                new  PosicaoRankingDTO
	                {
		                Acertos = 93,
		                Nota = 9.3,
		                Posicao = "4º"
	                },
	                new  PosicaoRankingDTO
	                {
		                Acertos = 93,
		                Nota = 9.3,
		                Posicao = "4º"
	                },
	                new  PosicaoRankingDTO
	                {
		                Acertos = 92,
		                Nota = 9.2000000000000011,
		                Posicao = "7º"
	                },
	                new  PosicaoRankingDTO
	                {
		                Acertos = 92,
		                Nota = 9.2000000000000011,
		                Posicao = "7º"
	                },
	                new  PosicaoRankingDTO
	                {
		                Acertos = 92,
		                Nota = 9.2000000000000011,
		                Posicao = "7º"
	                },
	                new  PosicaoRankingDTO
	                {
		                Acertos = 92,
		                Nota = 9.2000000000000011,
		                Posicao = "7º"
	                }
                };
        }

        public static ExercicioHistoricoDTO ObterExercicioHistoricoRealizado()
        {
            return new ExercicioHistoricoDTO
            {
                bitPresencial = false,
                bitRanqueado = false,
                bitRealizadoOnline = null,
                dteDateFim = null,
                dteDateInicio = new DateTime(2014, 4, 13, 16, 6, 25),
                intApplicationID = 1,
                intClientID = 167144,
                intExercicioID = 569,
                intExercicioTipo = 1,
                intHistoricoExercicioID = 614,
                intTempoExcedido = 0,
                intTipoProva = 1,
                intVersaoID = 1
            };
        }

        public static Exercicios ObterExerciciosPermitidos()
        {
            return new Exercicios
            {
                new Exercicio{ ID = 1, TipoId = 2 },
                new Exercicio{ ID = 2, TipoId = 2 },
                new Exercicio{ ID = 3, TipoId = 8 },
                new Exercicio{ ID = 4, TipoId = 4 },
                new Exercicio{ ID = 5, TipoId = 4 },
                new Exercicio{ ID = 6, TipoId = 5 },
                new Exercicio{ ID = 10, TipoId = 6 },
                new Exercicio{ ID = 9, TipoId = 2 },
                new Exercicio{ ID = 8, TipoId = 8 },
                new Exercicio{ ID = 7, TipoId = 8 }
            };
        }

        public static List<ForumProva.Acerto> GetAcertosEspecialidadeProva()
        {
            return new List<ForumProva.Acerto>
            {
                new ForumProva.Acerto
                {
                    Acertos = 100,
                    Especialidade = new Especialidade{ Descricao = "Desenvolvedor" },
                    Nome = "Tiltano",
                    UF = "SP",
                    Matricula = 1
                },
                new ForumProva.Acerto
                {
                    Acertos = 50,
                    Especialidade = new Especialidade{ Descricao = "Diagramador" },
                    Nome = "Fulano",
                    UF = "SP",
                    Matricula = 2
                },
                new ForumProva.Acerto
                {
                    Acertos = 50,
                    Especialidade = new Especialidade{ Descricao = "Diagramador" },
                    Nome = "Ciclano",
                    UF = "SP",
                    Matricula = 3
                },
                new ForumProva.Acerto
                {
                    Acertos = 50,
                    Especialidade = new Especialidade{ Descricao = "Programador" },
                    Nome = "Beltrano",
                    UF = "SP",
                    Matricula = 4
                },
                new ForumProva.Acerto
                {
                    Acertos = 70,
                    Especialidade = new Especialidade{ Descricao = "Diagramador" },
                    Nome = "Volcano",
                    UF = "SP",
                    Matricula = 5
                }
            };
        }

        public static List<ForumProva.Comentario> GetComentariosForumProva()
        {
            return new List<ForumProva.Comentario>
            {
                new ForumProva.Comentario
                {
                    NickName = "Fulano de Tal",
                    Uf = "SP",
                    DataCadastro = Utilidades.FormatDataTime(DateTime.Now),
                    ComentarioTexto = "Comentario 1",
                    Especialidade = new Especialidade { Descricao = "Desenvolvedor" },
                    Matricula = 1
                },
                new ForumProva.Comentario
                {
                    NickName = "Fulano de Sal",
                    Uf = "SP",
                    DataCadastro = Utilidades.FormatDataTime(DateTime.Now),
                    ComentarioTexto = "Comentario 2",
                    Especialidade = new Especialidade { Descricao = "Desenvolvedor" },
                    Matricula = 2
                },
                new ForumProva.Comentario
                {
                    NickName = "Fulano de Cal",
                    Uf = "SP",
                    DataCadastro = Utilidades.FormatDataTime(DateTime.Now),
                    ComentarioTexto = "Comentario 3",
                    Especialidade = new Especialidade { Descricao = "Desenvolvedor" },
                    Matricula = 3
                }
            };
        }
    }
}