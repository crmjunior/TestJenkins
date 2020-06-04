using System;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccessTests.EntitiesMockData
{
    public static class QuestaoEntityTestData
    {
        public static List<PPQuestao> ListaQuestoesComComentarioApostila()
        {
            return new List<PPQuestao>()
            {
                new PPQuestao()
                {
                    EmClassificacaoPor = new Professor() { DataCadastro = -62135596800.0 },
                    Id = 1,
                    Ano = 2019,
                    Especialidades = new List<Especialidade>(),
                    Apostilas = new List<Apostila>(),
                    Tipo = 1,
                    Ordem = 1,
                    MediaComentario = new Media() { Imagens = new List<string>()},
                    Prova = new Prova() { ID = 1 },
                    Concurso = new Concurso() {
                      Sigla =  "USP - SP",
                      UF = "SP",
                      Ano =  2019,
                      CartoesResposta =  null,
                      TipoProva = null,
                      PermissaoProva =  null
                    },
                    ProtocoladaPara = new Professor() {
                      ID = 85957,
                      Nome = "KAREN PANISSET",
                      DataCadastro = -62135596800.0
                    },
                    Premium = true,
                    PossuiComentario =  true,
                    OrdemPremium = 1
                },
                new PPQuestao()
                {
                    EmClassificacaoPor = new Professor() { DataCadastro = -62135596800.0 },
                    Id = 2,
                    Ano = 2019,
                    Especialidades = new List<Especialidade>(),
                    Apostilas = new List<Apostila>(),
                    Tipo = 1,
                    Ordem = 2,
                    MediaComentario = new Media() { Imagens = new List<string>()},
                    Prova = new Prova() { ID = 1 },
                    Concurso = new Concurso() {
                      Sigla =  "USP - SP",
                      UF = "SP",
                      Ano =  2019,
                      CartoesResposta =  null,
                      TipoProva = null,
                      PermissaoProva =  null
                    },
                    ProtocoladaPara = new Professor() {
                      ID = 85943,
                      Nome = "BRUNO MESSINA",
                      DataCadastro = -62135596800.0
                    },
                    Premium = true,
                    PossuiComentario =  true,
                    OrdemPremium = 1
                },
                new PPQuestao()
                {
                    EmClassificacaoPor = new Professor() { DataCadastro = -62135596800.0 },
                    Id = 3,
                    Ano = 2019,
                    Especialidades = new List<Especialidade>(),
                    Apostilas = new List<Apostila>(),
                    Tipo = 1,
                    Ordem = 3,
                    MediaComentario = new Media() { Imagens = new List<string>()},
                    Prova = new Prova() { ID = 1 },
                    Concurso = new Concurso() {
                      Sigla =  "USP - SP",
                      UF = "SP",
                      Ano =  2019,
                      CartoesResposta =  null,
                      TipoProva = null,
                      PermissaoProva =  null
                    },
                    ProtocoladaPara = new Professor() {
                      ID = 85957,
                      Nome = "KAREN PANISSET",
                      DataCadastro = -62135596800.0
                    },
                    Premium = true,
                    PossuiComentario =  true,
                    OrdemPremium = 1
                },
                new PPQuestao()
                {
                    EmClassificacaoPor = new Professor() { DataCadastro = -62135596800.0 },
                    Id = 4,
                    Ano = 2019,
                    Especialidades = new List<Especialidade>(),
                    Apostilas = new List<Apostila>(),
                    Tipo = 1,
                    Ordem = 4,
                    MediaComentario = new Media() { Imagens = new List<string>()},
                    Prova = new Prova() { ID = 1 },
                    Concurso = new Concurso() {
                      Sigla =  "USP - SP",
                      UF = "SP",
                      Ano =  2019,
                      CartoesResposta =  null,
                      TipoProva = null,
                      PermissaoProva =  null
                    },
                    ProtocoladaPara = new Professor() {
                      ID = 85957,
                      Nome = "KAREN PANISSET",
                      DataCadastro = -62135596800.0
                    },
                    Premium = true,
                    PossuiComentario =  true,
                    OrdemPremium = 1
                },
                new PPQuestao()
                {
                    EmClassificacaoPor = new Professor() { DataCadastro = -62135596800.0 },
                    Id = 5,
                    Ano = 2019,
                    Especialidades = new List<Especialidade>(),
                    Apostilas = new List<Apostila>(),
                    Tipo = 1,
                    Ordem = 5,
                    MediaComentario = new Media() { Imagens = new List<string>()},
                    Prova = new Prova() { ID = 1 },
                    Concurso = new Concurso() {
                      Sigla =  "USP - SP",
                      UF = "SP",
                      Ano =  2019,
                      CartoesResposta =  null,
                      TipoProva = null,
                      PermissaoProva =  null
                    },
                    ProtocoladaPara = new Professor() {
                      ID = 85957,
                      Nome = "KAREN PANISSET",
                      DataCadastro = -62135596800.0
                    },
                    Premium = true,
                    PossuiComentario =  true,
                    OrdemPremium = 1
                }
            };
        }

        public static List<ConcursoQuestoes_Alternativas> ListaMarcacoesObjetivasComGabarito()
        {
            return new List<ConcursoQuestoes_Alternativas>()
            {
                new ConcursoQuestoes_Alternativas()
                {
                    intAlternativaID = 1,
                    intQuestaoID = 11,
                    intQuestaoIDOld = null,
                    txtLetraAlternativa = "A",
                    txtAlternativa = "papiloma benigno. ",
                    bitCorreta = false,
                    bitCorretaPreliminar = false,
                    txtResposta = null,
                    txtImagem = null,
                    txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
                    intAlternativaID = 2,
                    intQuestaoID = 1,
                    intQuestaoIDOld = null,
                    txtLetraAlternativa = "B",
                    txtAlternativa = "necrose gordurosa.  ",
                    bitCorreta = false,
                    bitCorretaPreliminar = false,
                    txtResposta = null,
                    txtImagem = null,
                    txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
                    intAlternativaID = 3,
                    intQuestaoID = 1,
                    intQuestaoIDOld = null,
                    txtLetraAlternativa = "C",
                    txtAlternativa = "alterações fibrocísticas.  ",
                    bitCorreta = false,
                    bitCorretaPreliminar = false,
                    txtResposta = null,
                    txtImagem = null,
                    txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
                    intAlternativaID = 4,
                    intQuestaoID = 1,
                    intQuestaoIDOld = null,
                    txtLetraAlternativa = "D",
                    txtAlternativa = "fibroadenoma calcificado.  ",
                    bitCorreta = true,
                    bitCorretaPreliminar = true,
                    txtResposta = null,
                    txtImagem = null,
                    txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
                    intAlternativaID = 5,
                    intQuestaoID = 1,
                    intQuestaoIDOld = null,
                    txtLetraAlternativa = "E",
                    txtAlternativa = "",
                    bitCorreta = false,
                    bitCorretaPreliminar = false,
                    txtResposta = null,
                    txtImagem = null,
                    txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 6,
	                intQuestaoID = 2,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "A",
	                txtAlternativa = "investigação histológica. ",
	                bitCorreta = true,
	                bitCorretaPreliminar = true,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 7,
	                intQuestaoID = 2,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "B",
	                txtAlternativa = "realização de quadrantectomia.  ",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 8,
	                intQuestaoID = 2,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "C",
	                txtAlternativa = "complementação com ultrassonografia.  ",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 9,
	                intQuestaoID = 2,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "D",
	                txtAlternativa = "repetição da mamografia em 4 meses.  ",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 10,
	                intQuestaoID = 2,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "E",
	                txtAlternativa = "",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 11,
	                intQuestaoID = 3,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "A",
	                txtAlternativa = "Vesicouterina.",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 12,
	                intQuestaoID = 3,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "B",
	                txtAlternativa = "Vesicovaginal.",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 13,
	                intQuestaoID = 3,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "C",
	                txtAlternativa = "Vaginoperineal.",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 14,
	                intQuestaoID = 3,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "D",
	                txtAlternativa = "Ureterovaginal.",
	                bitCorreta = true,
	                bitCorretaPreliminar = true,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 15,
	                intQuestaoID = 3,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "E",
	                txtAlternativa = "",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 16,
	                intQuestaoID = 4,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "A",
	                txtAlternativa = "presença de cicatrizes pós-operatórias.\n",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 17,
	                intQuestaoID = 4,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "B",
	                txtAlternativa = "diferenciação de massas sólidas e cistos.\n",
	                bitCorreta = true,
	                bitCorretaPreliminar = true,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 18,
	                intQuestaoID = 4,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "C",
	                txtAlternativa = "avaliação de pacientes com prótese de silicone.\n",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 19,
	                intQuestaoID = 4,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "D",
	                txtAlternativa = "áreas de assimetria focal detectadas pela mamografia.",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 20,
	                intQuestaoID = 4,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "E",
	                txtAlternativa = "",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 21,
	                intQuestaoID = 5,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "A",
	                txtAlternativa = "dose única, sem reforço.",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 22,
	                intQuestaoID = 5,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "B",
	                txtAlternativa = "uma dose, repetida a cada dez anos.",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 23,
	                intQuestaoID = 5,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "C",
	                txtAlternativa = "duas doses, com intervalo de um ano.",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 24,
	                intQuestaoID = 5,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "D",
	                txtAlternativa = "três doses, em um período de seis meses.",
	                bitCorreta = true,
	                bitCorretaPreliminar = true,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
                new ConcursoQuestoes_Alternativas()
                {
	                intAlternativaID = 25,
	                intQuestaoID = 5,
	                intQuestaoIDOld = null,
	                txtLetraAlternativa = "E",
	                txtAlternativa = "",
	                bitCorreta = false,
	                bitCorretaPreliminar = false,
	                txtResposta = null,
	                txtImagem = null,
	                txtImagemOtimizada = null
                },
            };
        }
        public static CartoesResposta ListaCartaoResposta()
        { 
            return new CartoesResposta() 
            {
                Questoes = new List<Questao>() 
                {
                    new Questao 
                    {
                        Apostilas = {},
                        Especialidades = {},
                        ExercicioTipoID = 4,
                        Id = 7404,
                        MediaComentario = new Media()  {
                            Imagens = {}
                        },
                        Respondida = true,
                        Tipo = 1
                    },
                    new Questao
                    {
                        Apostilas = {},
                        Especialidades = {},
                        ExercicioTipoID = 2,
                        Id = 10210,
                        MediaComentario = new Media()  {
                            Imagens = {}
                        },
                        Respondida = true,
                        Tipo = 1
                    }
                }                
            };
        }
        public static List<QuestaoFiltroDTO> ListarMarcacoesQuestoesAluno()
        {
            return new List<QuestaoFiltroDTO>() {
                new QuestaoFiltroDTO()
                    {
                        QuestaoId = 7404,
                        TipoExercicioId = 4,
                        Favorita = true,
                        Anotacao = null
                    },
                new QuestaoFiltroDTO()
                    {
                        QuestaoId = 10210,
                        TipoExercicioId = 2,
                        Favorita = false,
                        Anotacao = "Comentario Teste"
                    }
            };
        }
        public static List<QuestaoFiltroDTO> ListarQuestoesIDs()
        {
            return new List<QuestaoFiltroDTO>() {
                new QuestaoFiltroDTO()
                    {
                       QuestaoId = 7404,
                       Ano = 2019,
                       Estado ="Rio de Janeiro",
                       ConcursoSigla = "UFES"
                    },
                new QuestaoFiltroDTO()
                    {
                        QuestaoId = 10210,
                        Ano = 2019,
                        Estado ="Rio de Janeiro",
                        ConcursoSigla = "UERj"
                    },
                new QuestaoFiltroDTO()
                    {
                        QuestaoId = 1,
                        Ano = 2019,
                        Estado ="Rio de Janeiro",
                        ConcursoSigla = "UFF"
                    }
            };
        }
        public static List<ConcursoQuestaoDTO> ListaConcursoQuestaoDTO()
        {
            return new List<ConcursoQuestaoDTO>()
            {
                new ConcursoQuestaoDTO()
                {
                    SiglaConcurso = "ABC",
                    OrdemQuestao = 1,
                    AnoConcurso = 2012,
                    NomeProva = "ACESSO DIRETO 1",
                    IdQuestao = 48336,
                    AnoQuestao = 2012,
                    DataQuestao = DateTime.Parse("2014-09-24T15:08:07.69")
                },
                new ConcursoQuestaoDTO()
                {
                    SiglaConcurso = "ABC",
                    OrdemQuestao = 2,
                    AnoConcurso = 2012,
                    NomeProva = "ACESSO DIRETO 1",
                    IdQuestao = 48338,
                    AnoQuestao = 2012,
                    DataQuestao = DateTime.Parse("2014-09-24T15:11:31.42")
                },
                new ConcursoQuestaoDTO()
                {
                    SiglaConcurso = "ABC",
                    OrdemQuestao = 3,
                    AnoConcurso = 2012,
                    NomeProva = "ACESSO DIRETO 1",
                    IdQuestao = 48340,
                    AnoQuestao = 2012,
                    DataQuestao = DateTime.Parse("2014-09-24T15:16:02.883")
                },
                new ConcursoQuestaoDTO()
                {
                    SiglaConcurso = "ABC",
                    OrdemQuestao = 4,
                    AnoConcurso = 2012,
                    NomeProva = "ACESSO DIRETO 1",
                    IdQuestao = 48344,
                    AnoQuestao = 2012,
                    DataQuestao = DateTime.Parse("2014-09-24T15:22:26.427")
                },
                new ConcursoQuestaoDTO()
                {
                    SiglaConcurso = "ABC",
                    OrdemQuestao = 5,
                    AnoConcurso = 2012,
                    NomeProva = "ACESSO DIRETO 1",
                    IdQuestao = 48348,
                    AnoQuestao = 2012,
                    DataQuestao = DateTime.Parse("2014-09-24T15:24:36.487")
                },
                new ConcursoQuestaoDTO()
                {
                    SiglaConcurso = "ABC",
                    OrdemQuestao = 6,
                    AnoConcurso = 2012,
                    NomeProva = "ACESSO DIRETO 1",
                    IdQuestao = 48356,
                    AnoQuestao = 2012,
                    DataQuestao = DateTime.Parse("2014-09-24T15:27:06.693")
                },
                new ConcursoQuestaoDTO()
                {
                    SiglaConcurso = "ABC",
                    OrdemQuestao = 7,
                    AnoConcurso = 2012,
                    NomeProva = "ACESSO DIRETO 1",
                    IdQuestao = 48363,
                    AnoQuestao = 2012,
                    DataQuestao = DateTime.Parse("2014-09-24T15:28:32.61")
                },
                new ConcursoQuestaoDTO()
                {
                    SiglaConcurso = "ABC",
                    OrdemQuestao = 8,
                    AnoConcurso = 2012,
                    NomeProva = "ACESSO DIRETO 1",
                    IdQuestao = 48365,
                    AnoQuestao = 2012,
                    DataQuestao = DateTime.Parse("2014-09-24T15:29:21.043")
                },
                new ConcursoQuestaoDTO()
                {
                    SiglaConcurso = "ABC",
                    OrdemQuestao = 9,
                    AnoConcurso = 2012,
                    NomeProva = "ACESSO DIRETO 1",
                    IdQuestao = 48373,
                    AnoQuestao = 2012,
                    DataQuestao = DateTime.Parse("2014-09-24T15:30:26.93")
                },
                new ConcursoQuestaoDTO()
                {
                    SiglaConcurso = "ABC",
                    OrdemQuestao = 10,
                    AnoConcurso = 2012,
                    NomeProva = "ACESSO DIRETO 1",
                    IdQuestao = 48377,
                    AnoQuestao = 2012,
                    DataQuestao = DateTime.Parse("2014-09-24T15:31:05.13")
                }
            };
        }

        public static QuestaoDTO GetQuestaoDTO()
        {
            var questaoDTO = new QuestaoDTO()
            {
                intQuestaoID = 1,
                bitAnulada = false,
                bitCasoClinico = "0",
                txtEnunciado = "Teste questão de simulado",
                intEspecialidadeID = 1                
            };

            return questaoDTO;
        }

        public static List<Alternativa> GetAlternativasQuestaoSimulado()
        {
            var alternativaA = new Alternativa()
            {
                Id = 1,
                IdQuestao = 1,
                Correta = true,
                Letra = 'A',
            };
            var alternativaB = new Alternativa()
            {
                Id = 2,
                IdQuestao = 1,
                Correta = false,
                Letra = 'B',
            };
            var alternativaC = new Alternativa()
            {
                Id = 3,
                IdQuestao = 1,
                Correta = false,
                Letra = 'C',
            };
            var alternativaD = new Alternativa()
            {
                Id = 4,
                IdQuestao = 1,
                Correta = false,
                Letra = 'D',
            };

            var listaAlternativas = new List<Alternativa>();
            listaAlternativas.Add(alternativaA);
            listaAlternativas.Add(alternativaB);
            listaAlternativas.Add(alternativaC);
            listaAlternativas.Add(alternativaD);

            return listaAlternativas;
        }

        public static TabelaQuestaoSimuladoDTO GetQuestaoSimuladoDTO()
        {
            var questao = new TabelaQuestaoSimuladoDTO()
            {
                intQuestaoID = 1,
                intSimuladoID = 1,
                bitAnulada = false,
                txtCodigoCorrecao = ""
            };

            return questao;
        }

        public static SimuladoVersaoDTO GetSimuladoVersaoDTO()
        {
            var simuladoVersao = new SimuladoVersaoDTO()
            {
                intSimuladoID = 1,
                intQuestaoID = 1,
                intOrdem = 1,
                intVersaoID = 1
            };

            return simuladoVersao;
        }

        public static List<CartaoRespostaDiscursivaDTO> GetRespostaDiscursivaSimulado()
        {
            var resposta = new CartaoRespostaDiscursivaDTO()
            {
                intID = 1,
                txtResposta = "Testando questão discursiva",
                intDicursivaId = 1
            };

            var lista = new List<CartaoRespostaDiscursivaDTO>();
            lista.Add(resposta);
            return lista;
        }
    }
}