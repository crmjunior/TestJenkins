using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccessTests.EntitiesMockData
{
    public class RankingSimuladoEntityTestData
    {
        public static List<DadosOrdemVendaDTO> ListOrdemVendaTodosClientes(Constants.TipoSimulado tipo)
        {
            string name = string.Empty;
            string description = string.Empty;
            string storeName = string.Empty;
            int productGroup1ID = 0;
            int especialidadeID = 0;

            switch (tipo)
            {
                case Constants.TipoSimulado.R3_Pediatria:
                    name = "2019 R3 PEDIATRIA";
                    description = "R3 Pediatria";
                    storeName = "R3 MEDERI";
                    productGroup1ID = 82;
                    especialidadeID = 239;
                    break;
                case Constants.TipoSimulado.R3_Clinica:
                    name = "2019 R3 CLÍNICA MÉDICA";
                    description = "R3 Clinica";
                    storeName = "R3 MEDWRITERS";
                    productGroup1ID = 76;
                    especialidadeID = 236;
                    break;
                case Constants.TipoSimulado.R3_Cirurgia:
                    name = "2019 R3 CIRURGIA";
                    description = "R3 Cirurgia";
                    storeName = "R3 MEDERI";
                    productGroup1ID = 81;
                    especialidadeID = 18;
                    break;
                case Constants.TipoSimulado.R4_GO:
                    name = "2019 R4 GO";
                    description = "R4 GO";
                    storeName = "R3 MEDERI";
                    productGroup1ID = 83;
                    especialidadeID = 429;
                    break;
            }

            return new List<DadosOrdemVendaDTO>()
            {
                new DadosOrdemVendaDTO(){
                    intOrderID = 632819,
                    intClientID = 1,
                    personName = "Cliente Teste 01",
                    txtName = name,
                    intProductGroup1ID = productGroup1ID,
                    txtDescription = description,
                    txtStoreName = storeName,
                    cityIntState = -1,
                    intEspecialidadeID = especialidadeID
                },
                new DadosOrdemVendaDTO(){
                    intOrderID = 648082,
                    intClientID = 2,
                    personName = "Cliente Teste 02",
                    txtName = name,
                    intProductGroup1ID = productGroup1ID,
                    txtDescription = description,
                    txtStoreName = storeName,
                    cityIntState = -1,
                    intEspecialidadeID = especialidadeID
                },
                new DadosOrdemVendaDTO(){
                    intOrderID = 632202,
                    intClientID = 3,
                    personName = "Cliente Teste 03",
                    txtName = name,
                    intProductGroup1ID = productGroup1ID,
                    txtDescription = description,
                    txtStoreName = storeName,
                    cityIntState = -1,
                    intEspecialidadeID = especialidadeID
                },
                new DadosOrdemVendaDTO(){
                    intOrderID = 666775,
                    intClientID = 4,
                    personName = "Cliente Teste 04",
                    txtName = name,
                    intProductGroup1ID = productGroup1ID,
                    txtDescription = description,
                    txtStoreName = storeName,
                    cityIntState = -1,
                    intEspecialidadeID = especialidadeID
                },new DadosOrdemVendaDTO(){
                    intOrderID = 638418,
                    intClientID = 5,
                    personName = "Cliente Teste 05",
                    txtName = name,
                    intProductGroup1ID = productGroup1ID,
                    txtDescription = description,
                    txtStoreName = storeName,
                    cityIntState = -1,
                    intEspecialidadeID = especialidadeID
                }
            };
        }

        public static List<LogSimuladoAlunoTurmaDTO> ListLogSimuladoAlunoTurma(Constants.TipoSimulado tipo)
        {
            string especialidade = string.Empty;
            string unidade = string.Empty;
            string turma = string.Empty;
            switch (tipo)
            {
                case Constants.TipoSimulado.R3_Pediatria:
                    turma = "2019 R3 PEDIATRIA";
                    especialidade = "PEDIATRIA (ANO OPCIONAL)";
                    unidade = "R3 MEDERI";
                    break;
                case Constants.TipoSimulado.R3_Clinica:
                    turma = "2019 R3 CLÍNICA MÉDICA";
                    especialidade = "CLÍNICA MÉDICA (ANO OPCIONAL)";
                    unidade = "R3 MEDWRITERS";
                    break;
                case Constants.TipoSimulado.R3_Cirurgia:
                    turma = "2019 R3 CIRURGIA";
                    especialidade = "CIRURGIA GERAL (PROGRAMA AVANÇADO)";
                    unidade = "R3 MEDERI";
                    break;
                case Constants.TipoSimulado.R4_GO:
                    turma = "2019 R4 GO";
                    especialidade = "GINECOLOGIA E OBSTETRÍCIA (ANO OPCIONAL)";
                    unidade = "R3 MEDERI";
                    break;
            }

            return new List<LogSimuladoAlunoTurmaDTO>()
            {
                new LogSimuladoAlunoTurmaDTO()
                {
                    intSimuladoID = 1,
                    intClientID = 1,
                    intOrderID = 632819,
                    txtUnidade = "MEDREADER",
                    intState = 1,
                    txtTurma = "2019 MED MEDICINE MEDREADER",
                    txtEspecialidade = "Revalidação de Diploma",
                },
                new LogSimuladoAlunoTurmaDTO()
                {
                    intSimuladoID = 1,
                    intClientID = 2,
                    intOrderID = 648082,
                    txtUnidade = unidade,
                    intState = -1,
                    txtTurma = turma,
                    txtEspecialidade = especialidade,
                },
                new LogSimuladoAlunoTurmaDTO()
                {
                    intSimuladoID = 1,
                    intClientID = 3,
                    intOrderID = 632202,
                    txtUnidade = unidade,
                    intState = -1,
                    txtTurma = turma,
                    txtEspecialidade = especialidade,
                },
                new LogSimuladoAlunoTurmaDTO()
                {
                    intSimuladoID = 1,
                    intClientID = 4,
                    intOrderID = 666775,
                    txtUnidade = unidade,
                    intState = -1,
                    txtTurma = turma,
                    txtEspecialidade = especialidade,
                },
                new LogSimuladoAlunoTurmaDTO()
                {
                    intSimuladoID = 1,
                    intClientID = 5,
                    intOrderID = 638418,
                    txtUnidade = unidade,
                    intState = -1,
                    txtTurma = turma,
                    txtEspecialidade = especialidade,
                }
            };

        }

        public static List<Especialidade> ListEspecialidade()
        {
            return new List<Especialidade>()
            {
                new Especialidade() {
                    Id = 18,
                    CodigoArea = "R3",
                    Descricao = "CIRURGIA GERAL (PROGRAMA AVANÇADO)",
                },
                new Especialidade() {
                    Id = 236,
                    CodigoArea = "R3",
                    Descricao = "CLÍNICA MÉDICA (ANO OPCIONAL)",
                },
                new Especialidade() {
                    Id = 239,
                    CodigoArea = "R3",
                    Descricao = "PEDIATRIA (ANO OPCIONAL)",
                },
                new Especialidade() {
                    Id = 429,
                    CodigoArea = "R4",
                    Descricao = "GINECOLOGIA E OBSTETRÍCIA (ANO OPCIONAL)",
                }
            };
        }

        public static List<SimuladoResultadosDTO> ListResultado()
        {
            return new List<SimuladoResultadosDTO>()
            {
                new SimuladoResultadosDTO()
                {
                    intClientID = 1,
                    intSimuladoID = 1,
                    intVersaoID = 1,
                    intAcertos = 9,
                    intArquivoID = 1
                },
                new SimuladoResultadosDTO()
                {
                    intClientID = 2,
                    intSimuladoID = 1,
                    intVersaoID = 1,
                    intAcertos = 7,
                    intArquivoID = 1
                },
                new SimuladoResultadosDTO()
                {
                    intClientID = 3,
                    intSimuladoID = 1,
                    intVersaoID = 1,
                    intAcertos = 6,
                    intArquivoID = 1
                },
                new SimuladoResultadosDTO()
                {
                    intClientID = 5,
                    intSimuladoID = 1,
                    intVersaoID = 1,
                    intAcertos = 7,
                    intArquivoID = 1
                }
            };
        }

        public static List<SimuladoRankingFase01DTO> ListRankingSimulado_20PorcentoGabaritou()
        {
            return new List<SimuladoRankingFase01DTO>()
            {
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 100,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12345,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 100,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12344,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12343,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12342,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12341,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                }
                ,
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12342,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12341,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                }
                ,
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12340,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12339,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                }
            };
        }

        public static List<SimuladoRankingFase01DTO> ListRankingSimulado_10PorcentoGabaritou()
        {
            return new List<SimuladoRankingFase01DTO>()
            {
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 100,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12345,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12344,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12343,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12342,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12341,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                }
                ,
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12342,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12341,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                }
                ,
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12340,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12339,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                },
                new SimuladoRankingFase01DTO()
                {
                    intSimuladoID = 1, 
                    txtPosicao = "",
                    intAcertos = 97,
                    dblNotaObjetiva = 0,
                    dblNotaDiscursiva = 0, 
                    dblNotaFinal = 0,
                    intClientID = 12338,
                    txtUnidade = "",
                    txtLocal = "",
                    txtName = "",
                    txtEspecialidade = "",
                    intStateID = 1,
                    intArquivoID = 1
                }
            };
        }        
    }
}