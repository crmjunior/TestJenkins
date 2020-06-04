using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using System.Linq;
using System;
using MedCore_DataAccess.Contracts.Enums;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Repository
{
    public class RevalidaEntity : IRevalidaData
    {
        public List<ProgressoSemana> GetProgressoRevalida(int idCliente, int produto)
        {
            using (var ctx = new DesenvContext())
            {
                var percents = (from a in ctx.tblRevalidaAulaTemaProfessorAssistido
                                join b in ctx.tblLessonTitleRevalida on a.intLessonTitleID equals b.intLessonTitleRevalidaId
                                where a.intClientID == idCliente && a.intPercentVisualizado > 0
                                select new
                                {
                                    IdEntidade = b.intLessonTitleRevalidaId,
                                    Percent = a.intPercentVisualizado
                                })
                                .GroupBy(x => x.IdEntidade)
                                .Select(x => new ProgressoSemana()
                                {
                                    IdEntidade = x.Key,
                                    PercentLido = x.Max(y => y.Percent)
                                })
                                .ToList();

                return percents;
            }
        }

        public List<EspecialRevalida> GetEspecialRevalida(int ano, int matricula, int idAplicacao = (int)Aplicacoes.MsProMobile)
        {
            try
            {
                var lsRevalida = new List<EspecialRevalida>();
                var mednet = new MednetEntity();
                var aluno = new AlunoEntity();

                new Util.Log().SetLog(new LogMsPro
                {
                    Matricula = matricula,
                    IdApp = Aplicacoes.MsProMobile,
                    Tela = Util.Log.MsProLog_Tela.Revalida,
                    Acao = Util.Log.MsProLog_Acao.Abriu
                });

                var rev = mednet.GetTemasVideoRevalida();

                var revAgrp = rev.GroupBy(x => x.Assunto.Id).Select(grp => new { GrpID = grp.Key, RevalidaLst = grp.ToList() }).ToList();

                //var permissao = aluno.GetIsCortesiaRevalida(matricula);

                using (MiniProfiler.Current.Step("Carregando lista revalida obtendo o máximo progresso de aula revalida por item da lista"))
                {
                    foreach (var item in revAgrp)
                    {
                        var itemRevalida = new EspecialRevalida();
                        itemRevalida.Numero = item.GrpID;
                        itemRevalida.Ativa = 1;

                        var apostilas = item.RevalidaLst.Select(x => new Apostila
                        {
                            IdEntidade = x.Apostila.Especialidade.Id,
                            Nome = x.Apostila.Descricao,
                            PercentLido = GetMaximoProgressoAulaRevalida(x.IdTema, matricula),
                            Temas = new List<AulaTema> { new AulaTema { TemaID = x.IdTema } }
                        }
                        );

                        itemRevalida.Apostilas.AddRange(apostilas);

                        lsRevalida.Add(itemRevalida);
                    }
                }

                return lsRevalida;

            }
            catch
            {
                throw;
            }
        }

        public int GetMaximoProgressoAulaRevalida(int idTema, int idCliente)
        {
            using (var ctx = new DesenvContext())
            {
                var percentualVisto = 0;
                var consulta = (from p in ctx.tblRevalidaAulaTemaProfessorAssistido
                                where p.intClientID == idCliente && p.intLessonTitleID == idTema
                                select p.intPercentVisualizado).ToList();
                if (consulta.Count() > 0)
                    percentualVisto = consulta.Max();

                return percentualVisto;
            }
        }

        public TemaApostila GetTemaRevalida(int ano, int matricula, int idAplicacao, int idTema)
        {
            try
            {
                var mednet = new MednetEntity();
                var videosRevalida = mednet.GetTemasVideoRevalida();

                var videoTema = videosRevalida.FirstOrDefault(x => x.IdTema == idTema);
                var logVideos = mednet.GetLogVideos(matricula, ETipoVideo.Revalida);

                if (videoTema != null)
                {
                    var idsVideos = videoTema.VideosRevisao.Select(v => v.IdRevisaoAula).ToList();

                    var ctx = new DesenvContext();

                    var totalVisualizacoesVideos = (from a in ctx.tblVideoLog
                                                    //join b in idsVideos on a.intOrigemVideoId equals b
                                                    where a.intTipoVideo == (int)ETipoVideo.Revalida
                                                       && idsVideos.Contains(a.intOrigemVideoId)
                                                    select a.intOrigemVideoId).ToList();

                    var todosProgressosVideos = (from a in ctx.tblRevalidaAulaVideoLogPosition
                                                 where a.intClientId == matricula
                                                 select new
                                                 {
                                                     a.intRevalidaAulaVideoId,
                                                     a.intSecond
                                                 }).ToList();

                    ctx.Dispose();

                    using (MiniProfiler.Current.Step("Set progresso aula revalida por professor"))
                    {
                        foreach (var professor in videoTema.Professores)
                        {
                            var segundosTotalVideos = 0;
                            var segundosTotalVideosAssistidos = 0;
                            var videosProf = videoTema.VideosRevisao.Where(v => v.IdProfessor == professor.ID).ToList();

                            foreach (var video in videosProf)
                            {
                                video.Visualizacoes = totalVisualizacoesVideos.Count(t => t == video.IdRevisaoAula);

                                segundosTotalVideos += Convert.ToInt32(TimeSpan.Parse(video.DuracaoFormatada).TotalSeconds);
                                video.Assistido = logVideos.Any(l => l == video.IdRevisaoAula);
                                var duracaoVideo = Convert.ToInt32(TimeSpan.Parse(video.DuracaoFormatada).TotalSeconds);

                                if (todosProgressosVideos.Any(v => v.intRevalidaAulaVideoId == video.IdRevisaoAula))
                                {
                                    var tempoDeVideoAssistido = todosProgressosVideos.FirstOrDefault(v => v.intRevalidaAulaVideoId == video.IdRevisaoAula).intSecond;
                                    segundosTotalVideosAssistidos += tempoDeVideoAssistido > duracaoVideo ? duracaoVideo : tempoDeVideoAssistido;

                                    if (duracaoVideo > 0)
                                        video.Progresso = (todosProgressosVideos.Where(v => v.intRevalidaAulaVideoId == video.IdRevisaoAula).FirstOrDefault().intSecond * 100) / duracaoVideo;
                                }
                            }

                            if (segundosTotalVideos != 0)
                                professor.PercentVisualizado = (segundosTotalVideosAssistidos * 100) / segundosTotalVideos;
                            else
                                professor.PercentVisualizado = 0;

                            mednet.SetProgressoAulaRevalida(idTema, professor.ID, matricula, professor.PercentVisualizado);
                        }
                    }
                }
                else
                {
                    videoTema = new TemaApostila();
                }

                return videoTema;
            }
            catch
            {
                throw;
            }
        }
    }
}