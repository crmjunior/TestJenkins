using System;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccessTests.EntitiesMockData
{
    public static class QuestaoRecursoEntityTestData
    {
        public static RecursoQuestaoConcursoDTO ObterQuestaoForumRecursoExisteAnaliseProfCabeRecurso()
        {
            return new RecursoQuestaoConcursoDTO
            {
                Questao = new QuestaoConcursoRecursoDTO
                {
                    Id = 1,
                    Enunciado = "Esta é a pergunta",
                    Numero = 2
                },
                Prova = new ProvaConcursoDTO
                {
                    Ano = 2019,
                    Tipo = "Objetiva",
                    DataRecursoAte = new System.DateTime(2019, 04, 22)
                },
                Concurso = new ConcursoDTO
                {
                    Sigla = "IFSP",
                    SiglaEstado = "SP"
                },
                ForumRecurso = new ForumRecursoDTO
                {
                    ExisteAnaliseProfessor = true,
                    IdAnaliseProfessorStatus = 4,
                    IdRecursoStatusBanca = null,
                    ForumPreAnalise = new ForumPreRecursoDTO(),
                    ForumPosAnalise = new ForumPosRecursoDTO()
                    
                }
            };
        }

        public static IEnumerable<AlternativaQuestaoConcursoDTO> ObterAlternativasGabaritoOficialLetraC()
        {
            return new List<AlternativaQuestaoConcursoDTO>
            {
                new AlternativaQuestaoConcursoDTO
                {
                    CorretaPreliminar = true,
                    Letra = "A",
                },
                new AlternativaQuestaoConcursoDTO
                {
                    Letra = "B",
                },
                new AlternativaQuestaoConcursoDTO
                {
                    CorretaOficial = true,
                    Letra = "C",
                },
                new AlternativaQuestaoConcursoDTO
                {
                    Letra = "D",
                },
                new AlternativaQuestaoConcursoDTO
                {
                    Letra = "E",
                }
            };
        }

        public static List<RecursoQuestaoConcursoDTO> GetQuestoesConcurso()
        {
            return new List<RecursoQuestaoConcursoDTO>
            {
                new RecursoQuestaoConcursoDTO
                {
                    Questao = new QuestaoConcursoRecursoDTO
                    {
                        Id = 1,
                        Enunciado = "Esta é a pergunta",
                        Numero = 2
                    },
                    Prova = new ProvaConcursoDTO
                    {
                        Ano = 2019,
                        Tipo = "Objetiva",
                        DataRecursoAte = new System.DateTime(2019, 05, 22),
                        PainelAvisoTitulo = "Titulo",
                        PainelAviso = "Painel de avisos",
                        Comunicado = "Comunicado oficial",
                        ComunicadoAtivo = false,
                        QtdQuestoes = 80
                    },
                    Concurso = new ConcursoDTO
                    {
                        Sigla = "IFSP",
                        SiglaEstado = "SP"
                    },
                    ForumRecurso = new ForumRecursoDTO
                    {
                        ExisteAnaliseProfessor = true,
                        IdAnaliseProfessorStatus = 4,
                        IdRecursoStatusBanca = null,
                        ForumPreAnalise = new ForumPreRecursoDTO(),
                        ForumPosAnalise = new ForumPosRecursoDTO()

                    }
                },
                new RecursoQuestaoConcursoDTO
                {
                    Questao = new QuestaoConcursoRecursoDTO
                    {
                        Id = 2,
                        Enunciado = "Esta é a pergunta",
                        Numero = 3
                    },
                    Prova = new ProvaConcursoDTO
                    {
                        Ano = 2019,
                        Tipo = "Objetiva",
                        DataRecursoAte = new System.DateTime(2019, 05, 22),
                        PainelAviso = "Painel de avisos",
                        Comunicado = "Comunicado oficial",
                        ComunicadoAtivo = true,
                        DataLimiteComunicado = DateTime.Now.AddDays(-1)
                    },
                    Concurso = new ConcursoDTO
                    {
                        Sigla = "IFSP",
                        SiglaEstado = "SP"
                    },
                    ForumRecurso = new ForumRecursoDTO
                    {
                        ExisteAnaliseProfessor = true,
                        IdAnaliseProfessorStatus = 3,
                        IdRecursoStatusBanca = null,
                        ForumPreAnalise = new ForumPreRecursoDTO(),
                        ForumPosAnalise = new ForumPosRecursoDTO()

                    }
                },
                new RecursoQuestaoConcursoDTO
                {
                    Questao = new QuestaoConcursoRecursoDTO
                    {
                        Id = 3,
                        Enunciado = "Esta é a pergunta",
                        Numero = 4
                    },
                    Prova = new ProvaConcursoDTO
                    {
                        Ano = 2019,
                        Tipo = "Objetiva",
                        DataRecursoAte = new System.DateTime(2019, 05, 22),
                        PainelAviso = "Painel de avisos",
                        Comunicado = "Comunicado oficial",
                        ComunicadoAtivo = true,
                        DataLimiteComunicado = DateTime.Now.AddDays(1)
                    },
                    Concurso = new ConcursoDTO
                    {
                        Sigla = "IFSP",
                        SiglaEstado = "SP"
                    },
                    ForumRecurso = new ForumRecursoDTO
                    {
                        ExisteAnaliseProfessor = false,
                        IdAnaliseProfessorStatus = 8,
                        IdRecursoStatusBanca = null,
                        ForumPreAnalise = new ForumPreRecursoDTO(),
                        ForumPosAnalise = new ForumPosRecursoDTO()

                    }
                },
                new RecursoQuestaoConcursoDTO
                {
                    Questao = new QuestaoConcursoRecursoDTO
                    {
                        Id = 4,
                        Enunciado = "Esta é a pergunta",
                        Numero = 5
                    },
                    Prova = new ProvaConcursoDTO
                    {
                        Ano = 2019,
                        Tipo = "Objetiva",
                        DataRecursoAte = new System.DateTime(2019, 05, 22),
                        PainelAviso = "Painel de avisos",
                        Comunicado = "Comunicado oficial",
                        ComunicadoAtivo = true
                    },
                    Concurso = new ConcursoDTO
                    {
                        Sigla = "IFSP",
                        SiglaEstado = "SP"
                    },
                    ForumRecurso = new ForumRecursoDTO
                    {
                        ExisteAnaliseProfessor = false,
                        IdAnaliseProfessorStatus = 5,
                        IdRecursoStatusBanca = null,
                        ForumPreAnalise = new ForumPreRecursoDTO(),
                        ForumPosAnalise = new ForumPosRecursoDTO()

                    }
                }
            };
        }

        public static List<QuestaoConcursoVotosDTO> GetQuestaoConcursoVotos(bool maiorQueUm, params int[] idQuestaoList)
        {
            var votos = new List<QuestaoConcursoVotosDTO>();
            var random = new Random(DateTime.Now.Millisecond);
            foreach (var id in idQuestaoList)
            {
                votos.Add(new QuestaoConcursoVotosDTO
                {
                    IdQuestao = id,
                    QtdCabeRecurso = random.Next(maiorQueUm ? 1 : 0, 5)
                });
            }
            return votos;
        }

        public static List<ForumComentarioDTO> GetComentariosVotosQuestaoUltimoVotoAfirma()
        {
            return new List<ForumComentarioDTO>
            {
                new ForumComentarioDTO
                {
                    Afirma = true,
                    Autor = true,
                    DataInclusao = DateTime.Now,
                    Matricula = 1,
                    Nome = "Rafael"
                },
                new ForumComentarioDTO
                {
                    Afirma = false,
                    Autor = false,
                    DataInclusao = DateTime.Now.AddSeconds(20),
                    Matricula = 2,
                    Nome = "Fulano"
                },
                new ForumComentarioDTO
                {
                    Afirma = false,
                    Autor = true,
                    DataInclusao = DateTime.Now.AddSeconds(5),
                    Matricula = 1,
                    Nome = "Rafael"
                },
                new ForumComentarioDTO
                {
                    Afirma = true,
                    Autor = true,
                    DataInclusao = DateTime.Now.AddSeconds(10),
                    Matricula = 1,
                    Nome = "Rafael"
                },
                new ForumComentarioDTO
                {
                    Afirma = true,
                    Autor = false,
                    DataInclusao = DateTime.Now.AddSeconds(1),
                    Matricula = 2,
                    Nome = "Fulano"
                },
                new ForumComentarioDTO
                {
                    Afirma = true,
                    Autor = false,
                    DataInclusao = DateTime.Now.AddSeconds(1),
                    Matricula = 3,
                    Nome = "Ciclano"
                },
            };
        }
    }
}