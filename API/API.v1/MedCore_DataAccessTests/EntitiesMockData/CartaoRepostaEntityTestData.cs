using System;
using System.Linq;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace MedCore_DataAccessTests.EntitiesMockData
{
    public static class CartaoRepostaEntityTestData
    {
        public static List<OrdemSimuladoDTO> ObterOrdemSimuladoMaiorQueZero(int ExercicioID)
        {
            var list = new List<OrdemSimuladoDTO>();

            var item = new OrdemSimuladoDTO()
            {
                IntOrdem = 1,
                IntQuestaoID = 1,
                IntSimuladoID = ExercicioID,
                IntVersaoID = 1
            };

            list.Add(item);

            return list;
        }

        public static List<OrdemSimuladoDTO> ObterOrdemSimuladoVazia()
        {
            var list = new List<OrdemSimuladoDTO>();

            return list;
        }

        public static List<Questao> ListaCom1Questao()
        {
            var list = new List<Questao>();

            var item = new Questao()
            {
                Id = 1
            };

            list.Add(item);

            return list;
        }

        public static List<Questao> ListaCom2Questoes()
        {
            var list = new List<Questao>();

            var item = new Questao()
            {
                Id = 1
            };

            var item2 = new Questao()
            {
                Id = 2
            };

            list.Add(item);
            list.Add(item2);

            return list;
        }

        public static CartoesResposta GetCartaoRespostaImpressasEMarcacaoes()
        {
            var cartaoReposta = new CartoesResposta();

            cartaoReposta.Questoes = GetMockQuestoes();


            return cartaoReposta;
        }
        public static List<MarcacoesObjetivaDTO> UltimasMarcacoesObjetiva() {
            return new List<MarcacoesObjetivaDTO>(){
                new MarcacoesObjetivaDTO() {
                    IntQuestaoID = 11,
                    ID = 1,
                    Resposta = "Teste"
                }
           };
        
        }

        public static List<Questao> GetQuestoesSomenteImpressasComOuSemVideo()
        {
            return new List<Questao>() {
                new Questao() {
                      Id = 11,
                      Anulada = false,
                      Ano = 2019,
                      Tipo = 1,
                      Ordem = 1,
                      Impressa = true
                
                }
                ,
                new Questao() {
                      Id = 12,
                      Anulada = false,
                      Ano = 2019,
                      Tipo = 1,
                      Ordem = 2,
                      Impressa = true
                
                }
                 ,
                new Questao() {
                      Id = 13,
                      Anulada = false,
                      Ano = 2019,
                      Tipo = 1,
                      Ordem = 3,
                      Impressa = true
                
                }
            
            };
        
        }
        public static List<csp_ListaMaterialDireitoAluno_Result> ListaMaterialDireitoAluno()
        {
            return new List<csp_ListaMaterialDireitoAluno_Result>()
            {
                new csp_ListaMaterialDireitoAluno_Result()
                {
                    intMaterialID = 17715,
                    intBookEntityID = 566,
                    intSemana = 1,
                    dataInicio = "16/01",
                    datafim = "22/01",
                    horaInicio = "20:00:00",
                    anoCronograma = 2019,
                    anoCursado = 2019,
                    blnPermitido = 1,
                    txtName = "CIR 1",
                    intLessonTitleID = 3587
                },
                new csp_ListaMaterialDireitoAluno_Result()
                {
                    intMaterialID = 17715,
                    intBookEntityID = 566,
                    intSemana = 1,
                    dataInicio = "17/01",
                    datafim = "23/01",
                    horaInicio = "20:00:00",
                    anoCronograma = 2018,
                    anoCursado = 2019,
                    blnPermitido = 1,
                    txtName = "CIR 1",
                    intLessonTitleID = 3587
                },
                new csp_ListaMaterialDireitoAluno_Result()
                {
                    intMaterialID = 17850,
                    intBookEntityID = 673,
                    intSemana = 1,
                    dataInicio = "16/01",
                    datafim = "22/01",
                    horaInicio = "17:30:00",
                    anoCronograma = 2019,
                    anoCursado = 2019,
                    blnPermitido = 1,
                    txtName = "NEF 1",
                    intLessonTitleID = 2142
                  },
                  new csp_ListaMaterialDireitoAluno_Result()
                  {
                    intMaterialID = 17850,
                    intBookEntityID = 673,
                    intSemana = 1,
                    dataInicio = "17/01",
                    datafim = "23/01",
                    horaInicio = "17:30:00",
                    anoCronograma = 2018,
                    anoCursado = 2019,
                    blnPermitido = 1,
                    txtName = "NEF 1",
                    intLessonTitleID = 2142
                },
            };
        }

        private static List<Questao> GetMockQuestoes()
        {
            var questoes = new List<Questao>
            {
                new Questao
                {
                    Enunciado = "Favorita",
                    Id = 001,
                    Anotacoes = new List<QuestaoAnotacao> { new QuestaoAnotacao { Favorita = true } },
                    Respondida = true,
                    Correta = true

                },
                new Questao
                {
                    Enunciado = "Anotada",
                    Id = 002,
                    Anotacoes = new List<QuestaoAnotacao> { new QuestaoAnotacao { Anotacao = "Anotacao Teste" } },
                    Respondida = true,
                    Correta = true
                },
                new Questao
                {
                    Enunciado = "Impressa",
                    Id = 003,
                    Anotacoes = new List<QuestaoAnotacao> { new QuestaoAnotacao()},
                    Respondida = true,
                    Correta = true
                },
                new Questao
                {
                    Enunciado = "Errada",
                    Id = 004,
                    Anotacoes = new List<QuestaoAnotacao> { new QuestaoAnotacao()},
                    Respondida = true,
                    Correta = false
                },
                 new Questao
                {
                    Enunciado = "NãoRespondida",
                    Id = 004,
                     Anotacoes = new List<QuestaoAnotacao> { new QuestaoAnotacao()}
                },
                new Questao
                {
                    Enunciado = "Favorita e Anotada",
                    Id = 005,
                    Anotacoes = new List<QuestaoAnotacao> { new QuestaoAnotacao { Favorita = true , Anotacao = "Anotacao Teste" } },
                    Respondida = true,
                    Correta = true
                },
                new Questao
                {
                    Enunciado = "Favorita e Impressa",
                    Id = 005,
                    Anotacoes = new List<QuestaoAnotacao> { new QuestaoAnotacao { Favorita = true } },
                    Impressa = true,
                    Respondida = true,
                    Correta = true

                },
                new Questao
                {
                    Enunciado = "Anotada e Impressa",
                    Id = 006,
                    Anotacoes = new List<QuestaoAnotacao> { new QuestaoAnotacao { Anotacao = "Anotacao Teste" } },
                    Impressa = true,
                    Respondida = true,
                    Correta = true
                },
                new Questao
                {
                    Enunciado = "Favorita, Anotada e Impressa",
                    Id = 007,
                    Anotacoes = new List<QuestaoAnotacao> { new QuestaoAnotacao { Favorita = true , Anotacao = "Anotacao Teste" } },
                    Impressa = true,
                    Respondida = true,
                    Correta = true

                }
            };

            return questoes;
        }

        public static List<Questao> GetQuestaoSimulado()
        {
            var alternativas = new List<Alternativa>();
            var listaQuestao = new List<Questao>();
            listaQuestao.Add(new Questao()
            {
                Id = 1,
                Alternativas = alternativas,
                Ano = DateTime.Now.Year,
                Enunciado = "Questão de teste",
                Respondida = false
            });

            listaQuestao.Add(new Questao()
            {
                Id = 2,
                Alternativas = alternativas,
                Ano = DateTime.Now.Year,
                Enunciado = "Questão de teste",
                Respondida = true
            });

            return listaQuestao;
        }

        public static List<RespostasObjetivasCartaoRespostaSimuladoDTO> GetRespostasObjetivasSimulado()
        {
            var listaRespostasObjetivas = new List<RespostasObjetivasCartaoRespostaSimuladoDTO>();
            listaRespostasObjetivas.Add(new RespostasObjetivasCartaoRespostaSimuladoDTO()
            {
                cartaoRespostaObjetiva = new CartaoRespostaObjetivaDTO()
                {
                    intID = 1,
                    intQuestaoID = 1,
                    txtLetraAlternativa = "A"
                },
                questaoAlternativa = new QuestaoSimuladoAlternativaDTO()
                {
                    intQuestaoID = 1,
                    txtLetraAlternativa = "A",
                    txtResposta = "",
                    txtAlternativa = "A"
                },
                respostas = new RetornoAnonimo<int, int>
                {
                    Valor1 = 1,
                    Valor2 = 1
                }
            });

            return listaRespostasObjetivas;
        }

        public static int GetExercicioIDValidoQuestoesApostila()
        {
            using (var ctx = new DesenvContext())
            {
                
                var matriculaAcademico = 96409;
                var exercicio = ctx.Set<msp_API_ListaEntidades_Result>().FromSqlRaw("msp_API_ListaEntidades @p1 = {0}, @p2 = {1}", null, null, matriculaAcademico).ToList().FirstOrDefault();
                return (int)exercicio.intID;
            }
        }

        public static int GetExercicioIDValidoQuestoesApostilabyAnoMatriculaProduto(int id_Imed, int matricula, int ano)
        {
            using (var ctx = new DesenvContext())
            {
                var exercicio = ctx.Set<msp_API_ListaEntidades_Result>().FromSqlRaw("msp_API_ListaEntidades @intProductGroup = {0}, @intYear = {1}, @matricula = {2}", id_Imed, ano, matricula).ToList().FirstOrDefault();
                return (int)exercicio.intID;
            }
        }

        public static List<RespostasDiscursivasCartaoRespostaSimuladoDTO> GetRespostasDiscursivasSimulado()
        {
            var listaRespostasDiscursivas = new List<RespostasDiscursivasCartaoRespostaSimuladoDTO>();
            listaRespostasDiscursivas.Add(new RespostasDiscursivasCartaoRespostaSimuladoDTO()
            {
                cartaoRespostaDiscursiva = new CartaoRespostaDiscursivaDTO()
                {
                    intID = 2,
                    txtResposta = "Resposta de Teste"                    
                },
                questaoAlternativa = new QuestaoSimuladoAlternativaDTO()
                {
                    intQuestaoID = 2,
                    txtLetraAlternativa = "A",
                    txtResposta = "",
                    txtAlternativa = "A"
                }
            });

            return listaRespostasDiscursivas;
        }
    }
}