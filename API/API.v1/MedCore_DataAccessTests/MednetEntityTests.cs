using System;
using System.Linq;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccessTests.EntitiesDataTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class MednetEntityTests
    {
        private static TemasApostila videosRevalida = new MednetEntity().GetTemasVideoRevalidaPorGrupoID();
        private static int qntVideosRevalidaAnoAtual = videosRevalida.Count;
        private static DateTime dataToleranciaTestesAtual = Utilidades.DataToleranciaTestes();

        [TestMethod]
        [TestCategory("Basico")]
        public void GetTemasVideoRevalida_SolicitandoTodosOsTemasVideo_RetornaTemasVideo()
        {
            var ret = new MednetEntity().GetTemasVideoRevalida();
            Assert.IsNotNull(ret);
        }

        // [TestMethod]
        // public void VerificaAlunoSomenteAnoAtual_ComRevalida()
        // {
        //     // TODO: Procurar o responsável para validar o teste
        //     Assert.Inconclusive("Procurar o responsável para reavalidar o teste");

        //     var aluno = new PerfilAlunoEntityTestData().GetAlunoExtensivoSomenteAnoAtualAtivoSelecionouEspecialidadeRevalida();
        //     var listaTotalVideosSomenteAnoAtual = new MednetEntity().GetTemasVideoRevalida();
        //     TemasApostila retVal = new MednetEntity().GetVideosRevalida(aluno.ID);
        //     Assert.IsTrue(retVal.Count == qntVideosRevalidaAnoAtual);
        // }

        [TestMethod]
        public void VerificaAlunoAnosAterioresEAnoAtual_ComRevalida()
        {
            if (DateTime.Now <= dataToleranciaTestesAtual)
            {
                Assert.Inconclusive();
            }

            var matricula = 53707;
            TemasApostila retVal = new MednetEntity().GetVideosRevalida(matricula);
            Assert.IsTrue(retVal.Count > qntVideosRevalidaAnoAtual);
        }

        [TestMethod]
        public void VerificaSomenteAluno2017_ComRevalida()
        {
            if (DateTime.Now <= dataToleranciaTestesAtual)
            {
                Assert.Inconclusive();
            }

            var matricula = 228528;
            TemasApostila retVal = new MednetEntity().GetVideosRevalida(matricula);
            Assert.IsTrue(retVal.Count > qntVideosRevalidaAnoAtual);
        }

        [TestMethod]
        public void VerificaAluno2017EAnoAtual_ComRevalida()
        {
            if (DateTime.Now <= dataToleranciaTestesAtual)
            {
                Assert.Inconclusive();
            }

            var matricula = 229901;
            TemasApostila retVal = new MednetEntity().GetVideosRevalida(matricula);
            Assert.IsTrue(retVal.Count > qntVideosRevalidaAnoAtual);
        }

        [TestMethod]
        public void VerificaAluno2018E2019_ComRevalida()
        {
            if (DateTime.Now <= dataToleranciaTestesAtual)
            {
                Assert.Inconclusive();
            }

            var matricula = 257958;
            TemasApostila retVal = new MednetEntity().GetVideosRevalida(matricula);
            Assert.IsTrue(retVal.Count == qntVideosRevalidaAnoAtual);
        }

        [TestMethod]
        public void VerificaAluno2015E2016_ComRevalida()
        {
            if (DateTime.Now <= dataToleranciaTestesAtual)
            {
                Assert.Inconclusive();
            }

            var matricula = 201868;
            TemasApostila retVal = new MednetEntity().GetVideosRevalida(matricula);
            Assert.IsTrue(retVal.Count > qntVideosRevalidaAnoAtual);
        }

        [TestMethod]
        public void VerificaAluno2016E2017_ComRevalida()
        {
            if (DateTime.Now <= dataToleranciaTestesAtual)
            {
                Assert.Inconclusive();
            }

            var matricula = 226443;
            TemasApostila retVal = new MednetEntity().GetVideosRevalida(matricula);
            Assert.IsTrue(retVal.Count > qntVideosRevalidaAnoAtual);
        }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void Can_GetByFilters()
        // {
        //     var e = new MednetEntity();
        //     var produtos = e.GetProdutos(119300);
        //     Assert.IsNotNull(produtos);
        // }


        // [TestMethod]
        // [Ignore]
        // [TestCategory("Basico")]
        // public void Can_LimparCacheExerciciosPermitidos()
        // {
        //     var idCliente = 119300;
        //     var retorno = (new MednetEntity()).LimpaCacheExerciciosPermitidos(idCliente);

        //     Assert.AreEqual(1, retorno);

        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void Can_GetListaVideosAprovacao()
        // {
        //     var matriculaAcad = 96409;
        //     var prodId = 16;
        //     var retorno = new MednetEntity().GetAulaRevisaoVideosAprovacao(prodId, matriculaAcad);
        //     Console.WriteLine(retorno.Count());
        //     Assert.IsNotNull(retorno);
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void Can_GetTipoAprovador()
        // {
        //     var acad = 96557;
        //     var retorno = new MednetEntity().GetTipoAprovadorAulaRevisao(acad);
        //     Assert.AreEqual(Convert.ToInt32(retorno), 1);
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void Can_GetHistoricoAprovacaoVideoAulaRevisao()
        // {
        //     var videoAprovacao = new AulaRevisaoVideoAprovacao
        //     {
        //         Aprovado = true,
        //         Funcionario = new Funcionario { ID = Constants.CONTACTID_ACADEMICO },
        //         IdAulaRevisaoVideo = 2293
        //     };

        //     var retorno = new MednetEntity().GetHistoricoAprovacoesVideos(videoAprovacao.IdAulaRevisaoVideo.ToString());
        //     Assert.IsNotNull(retorno);
        // }

        // [TestMethod]
        // [Ignore]
        // [TestCategory("Basico")]
        // public void Can_AprovarVideoInsert()
        // {
        //     var videoAprovacao = new AulaRevisaoVideoAprovacao
        //     {
        //         Aprovado = true,
        //         Funcionario = new Funcionario { ID = Constants.CONTACTID_ACADEMICO },
        //         IdAulaRevisaoVideo = 2293
        //     };

        //     var insertAprovacao = new MednetEntity().InserirVideoAprovacao(videoAprovacao);
        //     Assert.IsNotNull(insertAprovacao);
        //     Assert.AreEqual(insertAprovacao, 1);
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void Can_GetTodasEspecialidades()
        // {
        //     var retorno = new EspecialidadeEntity().GetEspecialidadesGrandesAreas();
        //     foreach (var item in retorno)
        //         Console.WriteLine(item.Descricao);

        //     Assert.IsNotNull(retorno);
        // }

        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetAulasBonus_MedcursoAtual()
        {

            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var temasApostilas = new MednetEntity().GetTemasVideos(16, 43164, 0, 0, false);

            var temaBonus = 4157;
            var temaLiberadoAntesCronograma = 3315;

            Assert.IsTrue(temasApostilas.Any(x => x.IdTema == temaBonus));
            Assert.IsTrue(!temasApostilas.Any(x => x.IdTema == temaLiberadoAntesCronograma));
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetAulasBonus_MedAtualMedcursoAnteriores()
        {
            var temasApostilas = new MednetEntity().GetTemasVideos(16, 222511, 0, 0, false);
            var temaBonus = 4160;
            var temaLiberadoAntesCronograma = 2681;
            Assert.IsTrue(temasApostilas.Any(x => x.IdTema == temaBonus));
            Assert.IsTrue(!temasApostilas.Any(x => x.IdTema == temaLiberadoAntesCronograma));
        }



        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetAulasBonus_MedAtualMedcursoAnterior()
        {
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }

            var temasApostilas = new MednetEntity().GetTemasVideos(16, 239231, 0, 0, false);

            var temaBonus = 4157;
            var temaLiberadoAntesCronograma = 2225;
            Assert.IsTrue(temasApostilas.Any(x => x.IdTema == temaBonus));
            Assert.IsTrue(temasApostilas.Any(x => x.IdTema == temaLiberadoAntesCronograma));
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetAulasRevisao_SomenteCPMEDAnoAtual_Brunao()
        {
            var dataPrimeiraAulaMedTP = new DateTime(2017, 10, 25, 21, 00, 00);
            var anoInscricaoCpMed = Utilidades.ObterAnoInscricao((int)Aplicacoes.INSCRICAO_CPMED);
            if (DateTime.Now < dataPrimeiraAulaMedTP && !Utilidades.IsAntesDatalimite(anoInscricaoCpMed))
            {
                Assert.Inconclusive("Ainda não Há alunos CpMED Ano Atual");
            }
            var temasApostilas = new MednetEntity().GetTemasVideos(17, 212734, 0, 0, false);

            var outroTema = 3536;
            var temaLiberadoAntesCronograma = 3446;
            Assert.IsTrue(!temasApostilas.Any(x => x.IdTema == outroTema));
            Assert.IsTrue(temasApostilas.Any(x => x.IdTema == temaLiberadoAntesCronograma));
        }

        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_GetAulasRevisao_SomenteCPMEDTurmaConvidada_Brunao()
        {
            var dataPrimeiraAulaMedTP = new DateTime(2017, 10, 25, 21, 00, 00);
            var anoInscricaoCpMed = Utilidades.ObterAnoInscricao((int)Aplicacoes.INSCRICAO_CPMED);
            if (DateTime.Now < dataPrimeiraAulaMedTP && !Utilidades.IsAntesDatalimite(anoInscricaoCpMed))
            {
                Assert.Inconclusive("Ainda não Há alunos CpMED Ano Atual");
            }
            var temasApostilas = new MednetEntity().GetTemasVideos(17, 239776, 0, 0, false);

            var outroTema = 3536;
            var temaLiberadoAntesCronograma = 3446;
            Assert.IsTrue(!temasApostilas.Any(x => x.IdTema == outroTema));
            Assert.IsTrue(temasApostilas.Any(x => x.IdTema == temaLiberadoAntesCronograma));
        }


        // [TestMethod]
        // [TestCategory("Basico")]
        // public void Can_GetAnaliseEspecialidadesTemasHierarquicos()
        // {
        //     var especialidades = new EspecialidadeEntity().GetEspecialidadesGrandesAreas();
        //     var temasApostilas = new MednetEntity().GetTemasVideos(16, Constants.CONTACTID_ACADEMICO, 0, 0, false);
        //     var listaEspecialidades = new Especialidades();
        //     foreach (var tema in temasApostilas)
        //     {
        //         var especialidadeTema = especialidades.FirstOrDefault(e => e.Id == tema.Apostila.Especialidade.Id);
        //         var especialidade = new Especialidade { Id = tema.Apostila.Especialidade.Id, Descricao = especialidadeTema.Descricao };
        //         if (!listaEspecialidades.Any(e => e.Id == especialidade.Id))
        //         {
        //             listaEspecialidades.Add(especialidade);
        //             Console.WriteLine(especialidade.Id + "   " + especialidade.Descricao);
        //         }
        //     }

        //     Assert.IsNotNull(especialidades);
        //     Assert.IsNotNull(temasApostilas);
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void Can_GetListaAulaRevisaoLogVideosReprovados()
        // {
        //     var idTipoAprovador = 1;
        //     var retorno = new MednetEntity().GetAulaRevisaoLogReprovacao(idTipoAprovador);
        //     Console.WriteLine(retorno.Count());
        //     Assert.IsNotNull(retorno);
        // }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void Can_GetAprovacoes()
        // {
        //     var retorno = new MednetEntity().GetAprovacoes(2448, 15958);
        //     Console.WriteLine(retorno.Count());
        //     Assert.IsNotNull(retorno);
        // }


        // [TestMethod]
        // [TestCategory("Basico")]
        // public void Can_GetListPendingApprovalVideos()
        // {
        //     var retorno = new MednetEntity().GetVideosAprovacoesPendentes(ETipoVideo.Revisao);
        //     Console.WriteLine(retorno.Count());
        //     Assert.IsNotNull(retorno);
        // }
        //[TestMethod]
        //[TestCategory("Basico")]
        //public void Can_GetListPendingApprovalVideos()
        //{
        //    var retorno = new MednetEntity().GetPendingApprovalVideos();
        //    Console.WriteLine(retorno.Count);
        //    Assert.IsNotNull(retorno);
        //}

        [Ignore]
        [TestMethod]
        [TestCategory("Basico")]
        public void Can_InsertLogVideoAssistido()
        {
            var log = new VideoMednet
            {
                Matricula = 145408,
                IdRevisaoAula = 2027,
            };

            var ret = new MednetEntity().SetLogAssitido(log);
            Assert.IsNotNull(ret);
        }
        [TestMethod]
        [TestCategory("Basico")]
        public void GetVideos_TemaExistente_RetornaTemaPopulado()
        {
            var idTema = 2174;
            var retorno = new MednetEntity().GetVideos(16, 96409, idTema, 17);
            Assert.AreEqual(retorno.IdTema, idTema);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetVideos_TemaInexistente_NaoRetornaTemaPopulado()
        {
            var retorno = new MednetEntity().GetVideos(16, 119300, -1, 17);
            Assert.AreEqual(0, retorno.Id);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetVideos_TemaExistenteAluno_TemaPopulado()
        {
            var idTema = 2174;
            var retorno = new MednetEntity().GetVideos(16, 227142, idTema, 17);
            Assert.IsNotNull(retorno);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetAvaliacaoAulo_AlunoPedro_Tema90_RetornaNotaPopulada()
        {
            var retorno = new MednetEntity().GetAvaliacaoRealizada(119300, 90, 17);
            Assert.IsNotNull(retorno.NotaFinal);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetAvaliacaoAulo_AlunoPedro_TemaInexistente_RetornaNotaZerada()
        {
            var retorno = new MednetEntity().GetAvaliacaoRealizada(119300, -1, 17);
            Assert.AreEqual(0, retorno.NotaFinal);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void GetTemasVideosRevisao__AlunoMedcurso_QueAulaCronogramaPassou__DeveVisualizarRespectivaAulaRevisao()
        {
            #region ScriptSQL para pegar cenário de até que data a aula será exibida
            //select * from mview_Cronograma mc 
            //inner join tblProducts pr on mc.intCourseID = pr.intProductID
            //inner join tblLessonTitles lt on mc.intLessonTitleID = lt.intLessonTitleID
            //where mc.intYear = 2018 and pr.intProductGroup1 = 1 and mc.dteDateTime > GETDATE() and lt.txtLessonTitleName = 'Trauma I – Primeiro atendimento, cabeça e tórax'
            //order by mc.dteDateTime
            #endregion

            var business = new PerfilAlunoEntityTestData();
            var aluno = business.GetAlunoMedcursoAnoAtualAtivo();
            var matriculaMedcurso = aluno.ID;

            if (DateTime.Now.Month >= 1 && DateTime.Now.Day > 20)
            {
                var ret = new MednetEntity().GetTemasVideosRevisao(16, matriculaMedcurso, 0, 0, false);
                Assert.IsTrue(ret.Exists(x => x.Descricao.Contains("Trauma I – Primeiro atendimento, cabeça e tórax")));
            }
            else
            {
                Assert.Inconclusive();
            }
        }

        // [TestMethod]
        // [TestCategory("Basico")]
        // public void GetTemasVideosRevisao__AlunoPacoteAnoAtual_Medcurso2014_QueAulaCronogramaPassou__DeveVisualizarRespectivaAulaRevisao()
        // {
        //     if (DateTime.Now.Month <= 3)
        //     {
        //         var matricula = new PerfilAlunoEntityTestData().GetAlunoAnoAtualComAnosAnteriores();
        //         var ret = new MednetEntity().GetTemasVideosRevisao((int)Produto.Cursos.MEDCURSO, matricula, 0, 0, false);
        //         Assert.IsTrue(ret.Exists(x => x.Descricao.Contains("Gota;Febre Reumática")));              
        //     }
        //     else
        //     {
        //         Assert.Inconclusive();
        //     }
        // }


        [TestMethod]
        [TestCategory("Basico")]
        public void GetTemasVideosRevisao__AlunoMedcursoAnoAtual__NaoDeveVisualizarForaDeEpoca()
        {
            if (DateTime.Now.Month <= 2)
            {

                //select* from tblSellORDERS so
                //inner join tblClients c on c.intClientID = so.intClientID
                //WHERE intStatus = 2 and txtComment like '2019 MEDCURSO%

                var matriculaMedcurso2018 = 262319;

                var ret = new MednetEntity().GetTemasVideosRevisao(16, matriculaMedcurso2018, 0, 0, false);
                Assert.IsFalse(ret.Exists(x => x.Descricao.Contains("Gota;Febre Reumática")));
            }
            else
            {
                Assert.Inconclusive();
            }
        }

        // [TestMethod]
        // public void IsAlunoExtensivoSomenteAnoAtual_MatriculaComOvSomenteAnoAtual_RetornaTrue()
        // {
        //     var aluno = new PerfilAlunoEntityTestData().GetMatriculaAlunoExtensivoAnoAtualAtivo_SomenteUmaOV();
        //     bool ret = new MednetEntity().IsAlunoSomenteAnoAtualComDireitoRevalida(aluno);            
        //     Assert.IsTrue(ret);
        // }


        // [TestMethod]
        // public void VerificaAlunoSomenteAnoAtual_ComRevalida()
        // {
        //     // TODO: Procurar o responsável para validar o teste
        //     Assert.Inconclusive("Procurar o responsável para reavalidar o teste");

        //     var aluno = new PerfilAlunoEntityTestData().GetAlunoExtensivoSomenteAnoAtualAtivoSelecionouEspecialidadeRevalida();
        //     var listaTotalVideosSomenteAnoAtual = new MednetEntity().GetTemasVideoRevalida();
        //     TemasApostila retVal = new MednetEntity().GetVideosRevalida(aluno.ID);
        //     Assert.IsTrue(retVal.Count == qntVideosRevalidaAnoAtual);
        // }

        [TestMethod]
        public void VerificaVideoResumoAulaDesabilitado_RetornaTrue()
        {
            var matricula = 226443;
            var produto = 16;
            var tema = 0;
            var isAdmin = false;
            var idProfessor = 0;
            var idAula = 0;
            var temasApostila = new MednetEntity().GetTemasVideoResumo(produto, matricula, idProfessor, idAula, isAdmin, tema);
            Assert.AreEqual(temasApostila.Count, 0);
        }

        //  [TestMethod]
        //  public void SetProgressoVideo_PassandoProgressoMenorQueVideo_True()
        //  {
        //      var segundosParaAtualizacao = 60;
        //      var progresso = new ProgressoVideo();
        //      var alunos = new PerfilAlunoEntityTestData().GetAlunosAcademico();
        //      int aulaID = 0;
        //      foreach (var aluno in alunos)
        //      {
        //          aulaID = new AulaEntity().GetIdAulaComLogAlunoTeste(aluno.ID);
        //          if (aulaID != 0)
        //          {
        //              progresso.Matricula = aluno.ID;
        //              progresso.IdResumoAula = aulaID;
        //              progresso.IdRevalidaAula = aulaID;
        //              progresso.IdRevisaoAula = aulaID;
        //              progresso.ProgressoSegundo = segundosParaAtualizacao;
        //              progresso.UltimaAtualizacao = 0;
        //              var retorno = new MednetEntity().SetProgressoVideo(progresso);
        //              Assert.AreEqual(retorno, 1);
        //              break;
        //          }
        //      }
        //  }

        //  [TestMethod]
        //  public void GetProgressoVideoDevido_PassandoProgressoMaiorQueVideo_RetornandoProgressoTotalVideo()
        //  {
        //      var segundosParaAtualizacao = 999999999;
        //      var progresso = new ProgressoVideo();
        //      var alunos = new PerfilAlunoEntityTestData().GetAlunosAcademico();
        //      int aulaID = 0;
        //     var bus = new MednetBusiness(new MednetEntity());
        //      foreach (var aluno in alunos)
        //      {
        //          aulaID = new AulaEntity().GetIdAulaComLogAlunoTeste(aluno.ID);
        //          if (aulaID != 0)
        //          {
        //              progresso.Matricula = aluno.ID;
        //              progresso.IdResumoAula = aulaID;
        //              progresso.IdRevalidaAula = aulaID;
        //              progresso.IdRevisaoAula = aulaID;
        //              progresso.ProgressoSegundo = segundosParaAtualizacao;
        //              progresso.UltimaAtualizacao = 0;
        //              var retorno = bus.GetProgressoVideoDevido(progresso);
        //              Assert.AreNotEqual(retorno, segundosParaAtualizacao);
        //              break;
        //          }
        //      }
        //  }

        //  [TestMethod]
        //  public void SetProgressoVideo_PassandoProgressoMaiorQueVideo_NaoGravaProgressoMaior()
        //  {
        //      var segundosParaAtualizacao = 999999999;
        //      var progresso = new ProgressoVideo();
        //      var alunos = new PerfilAlunoEntityTestData().GetAlunosAcademico();
        //     var bus = new MednetBusiness(new MednetEntity());
        //     int aulaID = 0;
        //      foreach(var aluno in alunos)
        //      {
        //          aulaID = new AulaEntity().GetIdAulaComLogAlunoTeste(aluno.ID);
        //          if (aulaID != 0)
        //          {
        //              progresso.Matricula = aluno.ID;
        //              progresso.IdResumoAula = aulaID;
        //              progresso.IdRevalidaAula = aulaID;
        //              progresso.IdRevisaoAula = aulaID;
        //              progresso.ProgressoSegundo = segundosParaAtualizacao;
        //              progresso.UltimaAtualizacao = 0;

        //              var retorno = bus.SetProgressoVideo(progresso);
        //              Assert.AreNotEqual(progresso.ProgressoSegundo, segundosParaAtualizacao);
        //              break;
        //          }
        //      } 
        //  }
    }
}

