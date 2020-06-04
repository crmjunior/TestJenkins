using System;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccessTests.EntitiesMockData
{
    public class SimuladoEntityTestData
    {
        public static IEnumerable<ExercicioDTO> GetSimuladosAgrupados525()
        {
            List<ExercicioDTO> exercicios = new List<ExercicioDTO>();
            exercicios.Add(new ExercicioDTO() { Ano = 2017, ID = 0, Descricao = "2017 SIM01 - Geral 1" });
            exercicios.Add(new ExercicioDTO() { Ano = 2017, ID = 2, Descricao = "2017 SIM01 - Geral 1" });
            exercicios.Add(new ExercicioDTO() { Ano = 2017, ID = 4, Descricao = "2017 SIM01 - Geral 1" });
            exercicios.Add(new ExercicioDTO() { Ano = 2017, ID = 2, Descricao = "2017 SIM01 - Geral 1" });
            exercicios.Add(new ExercicioDTO() { Ano = 2017, ID = 1, Descricao = "2017 SIM02 - Clínica Médica" });
            exercicios.Add(new ExercicioDTO() { Ano = 2017, ID = 3, Descricao = "2017 SIM03 - Cirugia Geral" });
            exercicios.Add(new ExercicioDTO() { Ano = 2017, ID = 1, Descricao = "2017 SIM03 - Cirugia Geral" });
            return exercicios;
        }

        public static List<Exercicio> GetListaSimuladosEsperada(){
            var listaEsperada = new List<Exercicio>();


            var simulado = new Exercicio(){
                         Ano = 2016,
                         ExercicioName = "Test",
                         Descricao = "Test",
                         ID = 0,
                         DataInicio = (double)45644545345,
                         DataFim = (double)43453453353,
                         Online = (int)0,
                         Ativo = true,
                         IdTipoRealizacao = (int)1,
                         Duracao = (int)300,
                         Realizado = (int)1
                     };

            listaEsperada.Add(simulado);
            return listaEsperada;
        }

        public static List<SimuladoDTO> GetSimuladosAnoAtual()
        {
            List<SimuladoDTO> simulados = new List<SimuladoDTO>();
            simulados.Add(new SimuladoDTO() { ID = 641, TemaID = 4343, LivroID = 17901, Origem = "M", Nome = "2019 R1 SIM 01", Descricao = "2019 R1 SIM 01 - Geral 01", Ordem = 1, Duracao = 240, ConcursoID = null, Ano = DateTime.Now.Year, ParaWeb = null, QuantidadeQuestoes = 100, });
            simulados.Add(new SimuladoDTO() { ID = 642, TemaID = 4867, LivroID = null, Origem = "M", Nome = "2019 R3 SIM 01 - Clínica Médic", Descricao = "2019 R3 SIM 01 - Clínica Médic", Ordem = 1, Duracao = 180, ConcursoID = null, Ano = DateTime.Now.Year, ParaWeb = null, QuantidadeQuestoes = 50, });
            simulados.Add(new SimuladoDTO() { ID = 643, TemaID = 4874, LivroID = null, Origem = "M", Nome = "2019 R3 SIM 01 - Cirurgia Gera", Descricao = "2019 R3 SIM 01 - Cirurgia Gera", Ordem = 1, Duracao = 180, ConcursoID = null, Ano = DateTime.Now.Year, ParaWeb = null, QuantidadeQuestoes = 50, });
            simulados.Add(new SimuladoDTO() { ID = 644, TemaID = 4881, LivroID = null, Origem = "M", Nome = "2019 R3 SIM 01 - Pediatria", Descricao = "2019 R3 SIM 01 - Pediatria", Ordem = 1, Duracao = 180, ConcursoID = null, Ano = DateTime.Now.Year, ParaWeb = null, QuantidadeQuestoes = 50, });
            simulados.Add(new SimuladoDTO() { ID = 645, TemaID = 4888, LivroID = null, Origem = "M", Nome = "2019 R4 SIM 01 - GO", Descricao = "2019 R4 SIM 01 - GO", Ordem = 1, Duracao = 180, ConcursoID = null, Ano = DateTime.Now.Year, ParaWeb = null, QuantidadeQuestoes = 50, });
            simulados.Add(new SimuladoDTO() { ID = 661, TemaID = 4344, LivroID = 17903, Origem = "M", Nome = "2019 R1 SIM 02", Descricao = "2019 R1 SIM 02 - Geral 02", Ordem = 2, Duracao = 240, ConcursoID = null, Ano = DateTime.Now.Year, ParaWeb = null, QuantidadeQuestoes = 100, });
            simulados.Add(new SimuladoDTO() { ID = 668, TemaID = 4868, LivroID = 18868, Origem = "M", Nome = "2019 R3 SIM 02 - Clínica Médic", Descricao = "2019 R3 SIM 02 - Clínica Médic", Ordem = 2, Duracao = 180, ConcursoID = null, Ano = DateTime.Now.Year, ParaWeb = null, QuantidadeQuestoes = 50, });
            simulados.Add(new SimuladoDTO() { ID = 669, TemaID = 4875, LivroID = 18867, Origem = "M", Nome = "2019 R3 SIM 02 - Cirurgia Gera", Descricao = "2019 R3 SIM 02 - Cirurgia Gera", Ordem = 2, Duracao = 180, ConcursoID = null, Ano = DateTime.Now.Year, ParaWeb = null, QuantidadeQuestoes = 50, });
            simulados.Add(new SimuladoDTO() { ID = 670, TemaID = 4882, LivroID = 18869, Origem = "M", Nome = "2019 R3 SIM 02 - Pediatria", Descricao = "2019 R3 SIM 02 - Pediatria", Ordem = 2, Duracao = 180, ConcursoID = null, Ano = DateTime.Now.Year, ParaWeb = null, QuantidadeQuestoes = 50, });

            return simulados;
        }

        public static List<TemaSimuladoDTO> GetTemasSimuladoAnoAtual()
        {
            List<TemaSimuladoDTO> temas = new List<TemaSimuladoDTO>();
            temas.Add(new TemaSimuladoDTO() { ID = 2578, Nome = "Intensivão Cirurgia - Urologia", Ano = DateTime.Now.Year });
            temas.Add(new TemaSimuladoDTO() { ID = 4173, Nome = "Arritmias I (Taquiarritmias) -", Ano = DateTime.Now.Year });
            temas.Add(new TemaSimuladoDTO() { ID = 4174, Nome = "Glomerulopatias I (Nefrítica, ", Ano = DateTime.Now.Year });
            temas.Add(new TemaSimuladoDTO() { ID = 4175, Nome = "Trauma I – Primeiro atendiment", Ano = DateTime.Now.Year });
            temas.Add(new TemaSimuladoDTO() { ID = 4176, Nome = "Glomerulopatias II (Síndrome N", Ano = DateTime.Now.Year });
            temas.Add(new TemaSimuladoDTO() { ID = 4177, Nome = "Trauma II – Abdomen e TCE", Ano = DateTime.Now.Year });
            temas.Add(new TemaSimuladoDTO() { ID = 4178, Nome = "Doenças Tubulointersticiais e ", Ano = DateTime.Now.Year });
            temas.Add(new TemaSimuladoDTO() { ID = 4179, Nome = "Ciclo Menstrual e Anticoncepçã", Ano = DateTime.Now.Year });
            temas.Add(new TemaSimuladoDTO() { ID = 4180, Nome = "Distúrbio Hidroeletrolítico;Di", Ano = DateTime.Now.Year });
            temas.Add(new TemaSimuladoDTO() { ID = 4181, Nome = "Doença Vascular Renal", Ano = DateTime.Now.Year });

            return temas;
        }

        public static List<TipoSimuladoDTO> GetTiposSimulado()
        {
            List<TipoSimuladoDTO> tipos = new List<TipoSimuladoDTO>();
            tipos.Add(new TipoSimuladoDTO() { ID = 1, Nome = "Extensivo" });
            tipos.Add(new TipoSimuladoDTO() { ID = 2, Nome = "CP-MED" });
            tipos.Add(new TipoSimuladoDTO() { ID = 3, Nome = "Intensivo" });
            tipos.Add(new TipoSimuladoDTO() { ID = 4, Nome = "R3 PEDIATRIA" });
            tipos.Add(new TipoSimuladoDTO() { ID = 5, Nome = "R3 CLINICA" });
            tipos.Add(new TipoSimuladoDTO() { ID = 6, Nome = "R3 CIRURGIA" });
            tipos.Add(new TipoSimuladoDTO() { ID = 7, Nome = "R4 GO" });

            return tipos;
        }

        public static List<EspecialidadeDTO> GetEspecialidadesSimulado()
        {
            List<EspecialidadeDTO> especialidades = new List<EspecialidadeDTO>();
            especialidades.Add(new EspecialidadeDTO() { ID = -1, Nome = "Desconhecido" });
            especialidades.Add(new EspecialidadeDTO() { ID = 1, Nome = "Não Informada" });
            especialidades.Add(new EspecialidadeDTO() { ID = 2, Nome = "R3 CIR.VASC.(ANG.CIR.END.)" });
            especialidades.Add(new EspecialidadeDTO() { ID = 3, Nome = "ACUPUNTURA" });
            especialidades.Add(new EspecialidadeDTO() { ID = 4, Nome = "ANGIOLOGIA E CIR. VASCULAR" });
            especialidades.Add(new EspecialidadeDTO() { ID = 5, Nome = "R3 MED.FAM.COM.(ADM.SAÚDE)" });
            especialidades.Add(new EspecialidadeDTO() { ID = 6, Nome = "ALERGIA E IMUNOLOGIA" });
            especialidades.Add(new EspecialidadeDTO() { ID = 7, Nome = "ALERGIA E IMUNOLOGIA PEDIÁTRIC" });
            especialidades.Add(new EspecialidadeDTO() { ID = 8, Nome = "ALERGOLOGIA" });
            especialidades.Add(new EspecialidadeDTO() { ID = 9, Nome = "ANESTESIOLOGIA" });

            return especialidades;
        }        
    }
}