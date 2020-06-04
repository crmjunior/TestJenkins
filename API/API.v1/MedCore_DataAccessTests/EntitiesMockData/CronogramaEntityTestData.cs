using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedCore_DataAccess.Business.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccessTests.EntitiesMockData
{
    public class CronogramaEntityTestData
    {

        public List<msp_API_ListaEntidades_Result> GetListaEntidadeCPMED()
        {
            var lista = new List<msp_API_ListaEntidades_Result>();
            lista.Add(new msp_API_ListaEntidades_Result
            {
                intSemana = 1,
                intID = 1,
                intLessonTitleID = 1,
                intMaterialID = 1,
                dataInicio = "09/03",
                intYear = 2020,
                datafim = "10/03",
                txtCode = "2020 CPMED Extensivo",
                entidade = "CPMED EXTENSIVO AULA 1",
                txtDescription = "Apostila CPMED Extensivo",
                txtLessonTitleName = "2020 CPMED Extensivo a prova pratica",
                txtName = "CPMED EXTENSIVO APOSTILA AULA 1"
            });

            return lista;
        }
        public List<msp_API_ListaEntidades_Result> GetListaEntidadeAULAO()
        {
            var lista = new List<msp_API_ListaEntidades_Result>();
            lista.Add(new msp_API_ListaEntidades_Result
            {
                intSemana = 1,
                intID = 1,
                intLessonTitleID = 1,
                intMaterialID = 1,
                dataInicio = "09/03",
                intYear = 2020,
                datafim = "10/03",
                txtCode = "2020 AULAO",
                entidade = "AULAO",
                txtDescription = "Apostila Aulao",
                txtLessonTitleName = "2020 Aulao",
                txtName = "AULAO"
            });

            return lista;
        }
        public List<msp_API_ListaEntidades_Result> GetListaEntidadeAulasEspeciaisDentroDataLimite()
        {
            var lista = new List<msp_API_ListaEntidades_Result>();
            var pipe = DateTime.Now.Month < 10 ? "/0" : "/";
            lista.Add(new msp_API_ListaEntidades_Result
            {
                intSemana = 1,
                intID = 1,
                intLessonTitleID = 1,
                intMaterialID = 1,
                dataInicio = DateTime.Now.Day + "/0" + DateTime.Now.Month,
                intYear = 2020,
                datafim = "10/03",
                txtCode = "SEMANA 12",
                entidade = "SEMANA 12",
                txtDescription = "SEMANA 12",
                txtLessonTitleName = "SEMANA 12",
                txtName = "SEMANA 12"
            });

            return lista;
        }

        public List<msp_API_ListaEntidades_Result> GetListaEntidadeAulasEspeciaisAposDataLimite()
        {
            var lista = new List<msp_API_ListaEntidades_Result>();
            var pipe = DateTime.Now.AddDays(-30).Month < 10 ? "/0" : "/";
            lista.Add(new msp_API_ListaEntidades_Result
            {
                intSemana = 1,
                intID = 1,
                intLessonTitleID = 1,
                intMaterialID = 1,
                dataInicio = DateTime.Now.AddDays(-30).Day + pipe + DateTime.Now.AddDays(-30).Month,
                intYear = 2020,
                datafim = "10/03",
                txtCode = "SEMANA 12",
                entidade = "SEMANA 12",
                txtDescription = "SEMANA 12",
                txtLessonTitleName = "SEMANA 12",
                txtName = "SEMANA 12"
            });

            return lista;
        }

        public List<CronogramaPrateleiraDTO> GetCronogramaPrateleira(TipoLayoutMainMSPro layout = TipoLayoutMainMSPro.WEEK_DOUBLE, string Nome = "CPMED")
        {
            var lista = new List<CronogramaPrateleiraDTO>();
            lista.Add(new CronogramaPrateleiraDTO
            {
                ID = 1,
                Ano = 2020,
                Data = new DateTime(2020, 09, 03),
                Descricao = Nome,
                EntidadeCodigo = "1",
                EntidadeID = 1,
                EspecialidadeId = 1,
                ExibeConformeCronograma = false,
                ExibeEspecialidade = false,
                LessonTitleID = 1,
                MaterialId = 1,
                Nome = Nome,
                Ordem = 0,
                Semana = 1,
                TipoLayout = layout
            });
            return lista;
        }

        public List<CronogramaPrateleiraDTO> GetCronogramaPrateleiraAulasEpeciaisDentroDataLimite(TipoLayoutMainMSPro layout = TipoLayoutMainMSPro.WEEK_DOUBLE)
        {
            var lista = new List<CronogramaPrateleiraDTO>();
            lista.Add(new CronogramaPrateleiraDTO
            {
                ID = 1,
                Ano = 2020,
                Data = DateTime.Now,
                Descricao = "CPMED",
                EntidadeCodigo = "1",
                EntidadeID = 1,
                EspecialidadeId = 1,
                ExibeConformeCronograma = false,
                ExibeEspecialidade = false,
                LessonTitleID = 1,
                MaterialId = 1,
                Nome = "CPMED",
                Ordem = 0,
                Semana = 1,
                TipoLayout = layout
            });
            return lista;
        }

        public List<CronogramaPrateleiraDTO> GetCronogramaPrateleiraAulasEpeciaisAposDataLimite(TipoLayoutMainMSPro layout = TipoLayoutMainMSPro.WEEK_DOUBLE)
        {
            var lista = new List<CronogramaPrateleiraDTO>();
            lista.Add(new CronogramaPrateleiraDTO
            {
                ID = 1,
                Ano = 2020,
                Data = DateTime.Now.AddDays(-10),
                Descricao = "CPMED",
                EntidadeCodigo = "1",
                EntidadeID = 1,
                EspecialidadeId = 1,
                ExibeConformeCronograma = false,
                ExibeEspecialidade = false,
                LessonTitleID = 1,
                MaterialId = 1,
                Nome = "CPMED",
                Ordem = 0,
                Semana = 1,
                TipoLayout = layout
            });
            return lista;
        }

        public async Task<List<CronogramaPrateleiraDTO>> GetCronogramaPrateleiraAsync(TipoLayoutMainMSPro layout = TipoLayoutMainMSPro.WEEK_DOUBLE, string Nome = "CPMED")
        {
            var lista = new List<CronogramaPrateleiraDTO>();
            lista.Add(new CronogramaPrateleiraDTO
            {
                ID = 1,
                Ano = 2020,
                Data = new DateTime(2020, 09, 03),
                Descricao = Nome,
                EntidadeCodigo = "1",
                EntidadeID = 1,
                EspecialidadeId = 1,
                ExibeConformeCronograma = false,
                ExibeEspecialidade = false,
                LessonTitleID = 1,
                MaterialId = 1,
                Nome = Nome,
                Ordem = 0,
                Semana = 1,
                TipoLayout = layout
            });
            return lista;
        }

        public async Task<List<CronogramaPrateleiraDTO>> GetCronogramaPrateleiraAulasEpeciaisDentroDataLimiteAsync(TipoLayoutMainMSPro layout = TipoLayoutMainMSPro.WEEK_DOUBLE)
        {
            var lista = new List<CronogramaPrateleiraDTO>();
            lista.Add(new CronogramaPrateleiraDTO
            {
                ID = 1,
                Ano = 2020,
                Data = DateTime.Now,
                Descricao = "CPMED",
                EntidadeCodigo = "1",
                EntidadeID = 1,
                EspecialidadeId = 1,
                ExibeConformeCronograma = false,
                ExibeEspecialidade = false,
                LessonTitleID = 1,
                MaterialId = 1,
                Nome = "CPMED",
                Ordem = 0,
                Semana = 1,
                TipoLayout = layout
            });
            return lista;
        }

        public async Task<List<CronogramaPrateleiraDTO>> GetCronogramaPrateleiraAulasEpeciaisAposDataLimiteAsync(TipoLayoutMainMSPro layout = TipoLayoutMainMSPro.WEEK_DOUBLE)
        {
            var lista = new List<CronogramaPrateleiraDTO>();
            lista.Add(new CronogramaPrateleiraDTO
            {
                ID = 1,
                Ano = 2020,
                Data = DateTime.Now.AddDays(-10),
                Descricao = "CPMED",
                EntidadeCodigo = "1",
                EntidadeID = 1,
                EspecialidadeId = 1,
                ExibeConformeCronograma = false,
                ExibeEspecialidade = false,
                LessonTitleID = 1,
                MaterialId = 1,
                Nome = "CPMED",
                Ordem = 0,
                Semana = 1,
                TipoLayout = layout
            });
            return lista;
        }

        public List<TurmaMatriculaBaseDTO> GetTurmaBaseAulasEspeciais()
        {
            var lista = new List<TurmaMatriculaBaseDTO>();
            lista.Add(new TurmaMatriculaBaseDTO
            {
                Ano = 2020,
                CourseId = -1,
                DataCadastro = DateTime.Now,
                Id = 1,
                MatriculaBase = 96409,
                DiasLimite = 7
            });
            return lista;
        }

        public List<ApostilaCodigoDTO> GetCodigosAmigaveisApostilas(string Nome = "CPMED E")
        {
            var lista = new List<ApostilaCodigoDTO>();

            lista.Add(new ApostilaCodigoDTO
            {
                Nome = Nome,
                ProdutoId = 1,
                TemaId = 1
            });


            return lista;
        }


        public TemaApostila GetTemaApostila(ETipoVideo tipoVideo = ETipoVideo.Revisao)
        {
            var temaApostila = new TemaApostila()
            {
                Apostila = null,
                Assunto = null,
                DataProximaAula = 0,
                Descricao = "",
                GrupoId = 1,
                Id = 1,
                IdResumo = 1,
                IdTema = 1,
                Professores = new List<Pessoa>(),
                Semana = 1,
                VideoAulas = new List<VideoAula>(),
                Videos = new VideosMednet()
                    { new VideoMednet
                        {
                            IdProfessor = 1,
                            Url = "",
                            KeyVideo = "",
                            ID = 1,
                            Descricao = "CPMED e",
                            Assistido = false,
                            DuracaoFormatada = "",
                            IdProvaVideo = 1,
                            Ordem = 0,
                            Thumb = "",
                            TipoVideo = tipoVideo,
                            Links = null
                        }
                    },
                VideosAdaptaMed = new VideosMednet(),
                VideosResumo = new VideosMednet(),
                VideosRevalida = new VideosMednet(),
                VideosRevisao = new VideosMednet()
                    { new VideoMednet
                        {
                            IdProfessor = 1,
                            Url = "",
                            KeyVideo = "",
                            ID = 1,
                            Descricao = "CPMED e",
                            Assistido = false,
                            DuracaoFormatada = "",
                            IdProvaVideo = 1,
                            Ordem = 0,
                            Thumb = "",
                            TipoVideo = tipoVideo,
                            Links = null
                        }
                    }
            };
            return temaApostila;
        }
    }
}