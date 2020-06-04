using System;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccessTests.EntitiesMockData
{
    public static class AulaEntityTestData
    {
        public static List<CursoAulaDTO> GetPrimeiraAulaTurmaMockData()
        {
            var lstCursoAulaDTO = new List<CursoAulaDTO>();

            var cursoAulaDto = new CursoAulaDTO
            {
                CourseId = 18040,
                LessonDatetime = new DateTime(2019, 1, 11, 13, 0, 0)
            };
            var cursoAulaDto2 = new CursoAulaDTO
            {
                CourseId = 18102,
                LessonDatetime = new DateTime(2019, 1, 11, 8, 30, 0)
            };
            var cursoAulaDto3 = new CursoAulaDTO
            {
                CourseId = 15969,
                LessonDatetime = new DateTime(2019, 1, 11, 17, 0, 0)
            };

            lstCursoAulaDTO.Add(cursoAulaDto);

            lstCursoAulaDTO.Add(cursoAulaDto2);

            lstCursoAulaDTO.Add(cursoAulaDto3);

            return lstCursoAulaDTO;

        }

        public static long[] GetEntidadesMedCurso2019()
        {
            return new long[] { 731, 732, 733, 734, 735 };
        }

        public static List<QuestaoExercicioDTO> GetListaQuestoesApostila_ExercicioUnico()
        {
            var questoesApostila = new List<QuestaoExercicioDTO>();
            questoesApostila.Add(new QuestaoExercicioDTO
            {
                ExercicioID = 600,
                QuestaoID = 10
            });

            questoesApostila.Add(new QuestaoExercicioDTO
            {
                ExercicioID = 600,
                QuestaoID = 20
            });

            return questoesApostila;            
        }
        public static Generica GetEspecialidadeAluno()
        {
          return new Generica()
            {
                Descricao = "ACUPUNTURA",
                Valor = 3
            };

        }
         public static List<AlunoEspecialidadeDTO> GetEspecialiddesByConcursoIdAnoProva(){
             var result = new List<AlunoEspecialidadeDTO>();
             result.Add(new AlunoEspecialidadeDTO{
                intEspecialidadeID = 9,
                DE_ESPECIALIDADE = "ANESTESIOLOGIA",
                txtDescription = "SISTEMA ÚNICO DE SAÚDE - SUS - SÃO PAULO"
            });
             result.Add(new AlunoEspecialidadeDTO{
                intEspecialidadeID = 14,
                DE_ESPECIALIDADE = "CIRURGIA DO APARELHO DIGESTIVO",
                txtDescription = "SISTEMA ÚNICO DE SAÚDE - SUS - SÃO PAULO"
            });
             result.Add(new AlunoEspecialidadeDTO{
                intEspecialidadeID = 17,
                DE_ESPECIALIDADE = "CARDIOLOGIA",
                txtDescription = "SISTEMA ÚNICO DE SAÚDE - SUS - SÃO PAULO"
            });
             return result;
            }
        public static List<int> GetIdRespostaQuestaoApostila()
        {
            var respostas = new List<int>();
            respostas.Add(10);
            respostas.Add(20);

            return respostas;
        }

        public static List<long?> GetListaExercicioApostilaPermitidos_Ids()
        {
            var lista = new List<long?>();
            lista.Add(600);
            lista.Add(610);
            lista.Add(611);
            return lista;
        }


    }
}