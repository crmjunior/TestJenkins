using System.Collections.Generic;
using MedCore_API.Academico;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Contracts;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Model;
using System.Linq;
using MedCore_DataAccess.Business;
using System;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Contracts.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Profiling;
using MedCore_DataAccess.Business.Enums;
using System.Globalization;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Repository
{
    public class MednetEntity : IMednetData
    {
        public static List<int> LogVideos { get; set; }

        private IChaveamentoVimeo _chaveamentoVimeoAulaRevisao = new ChaveamentoVimeoRevisao();

        public List<ProgressoSemana> GetProgressoAulas(int matricula, int ano, int produto)
        {
            using(MiniProfiler.Current.Step("Obtendo lista de progresso das aulas"))
            {
                using (var ctx = new DesenvContext())
                {
                    var percents = ctx.Set<msp_API_ProgressoAulaRevisaoAluno_Result>().FromSqlRaw("msp_API_ProgressoAulaRevisaoAluno @matricula = {0}, @produto = {1}", matricula, produto).ToList()
                                        .Select(x => new ProgressoSemana
                                        {
                                            IdEntidade = x.intLessonTitleId,
                                            PercentLido = x.intPercentAluno.Value
                                        })
                                        .ToList();

                    return percents;
                }
            }
        }

        public Dictionary<int, int> GetProgressoAulas(int[] temas, int idClient)
        {
            using(MiniProfiler.Current.Step("Obtendo dicionário de progresso das aulas"))
            {
                using (var ctx = new DesenvContext())
                {

                    var percents = ctx.tblRevisaoAulaTemaProfessorAssistido
                        .Where(p => p.intClientID == idClient && temas.Contains(p.intLessonTitleID))
                        .GroupBy(y => y.intLessonTitleID)
                        .Select(x => new { TemaId = x.Key, Percent = x.Max(d => d.intPercentVisualizado) })
                        .ToDictionary(y => y.TemaId, y => y.Percent);

                    return percents;
                }
            }

        }

        public TemasApostila GetVideosRevalida(int matricula, int idAplicacao = (int)MedCore_DataAccess.Contracts.Enums.Aplicacoes.MsProMobile)
        {
            using (MiniProfiler.Current.Step("Obtendo Videos de Revalida"))
            {
                //var naoPodeVerTodoConteudo = IsAlunoSomenteAnoAtualComDireitoRevalida(matricula);
                var naoPodeVerTodoConteudo = true;
                if (!naoPodeVerTodoConteudo)
                    return GetTemasVideoRevalida(idAplicacao);
                else
                    return GetTemasVideoRevalidaPorGrupoID(idAplicacao);
            }
        }

        public TemasApostila GetTemasVideoRevalidaPorGrupoID(int idAplicacao = (int)MedCore_DataAccess.Contracts.Enums.Aplicacoes.MsProMobile)
        {
            using (MiniProfiler.Current.Step("Obtendo Temas de vídeo Revalida por grupo"))
            using(MiniProfiler.Current.Step("Obtendo vídeos de revalida por ID de grupo"))
            {
                using (var ctx = new AcademicoContext())
                {
                    using (var ctxMatDir = new DesenvContext())
                    {
                        var temasRetorno = new TemasApostila();

                        var revalida = (from ri in ctxMatDir.tblRevalidaAulaIndice
                                        join rv in ctxMatDir.tblRevalidaAulaVideo on ri.intRevalidaAulaIndiceId equals rv.intRevalidaAulaIndiceId
                                        join lt in ctxMatDir.tblLessonTitleRevalida on ri.intLessonTitleRevalidaId equals lt.intLessonTitleRevalidaId
                                        join prof in ctxMatDir.tblPersons on rv.intProfessorId equals prof.intContactID
                                        join cr in ctxMatDir.tblCronogramaConteudoRevalida on lt.GrupoId equals cr.GrupoId
                                        where rv.intVideoId != 0 && cr.bitAtivo == true
                                        orderby ri.intOrdem, rv.intOrdem
                                        select new
                                        {
                                            TemaDescricao = lt.txtName,
                                            IdRevalidaIndice = ri.intRevalidaAulaIndiceId,
                                            IdTema = ri.intRevalidaAulaIndiceId,
                                            IdRevalidaVideo = rv.intRevalidaAulaVideoId,
                                            RevisaoVideoDescricao = rv.txtDescricao,
                                            IdVideo = rv.intVideoId,
                                            IdProfessor = rv.intProfessorId,
                                            ProfessorNome = prof.txtName.Trim(),
                                            IntOrdem = rv.intOrdem,
                                            IntOrdemRi = ri.intOrdem,
                                            GrupoId = lt.GrupoId,
                                            intEspecialidadeId = lt.intEspecialidadeId
                                        }).ToList();

                        List<int?> listaEspecialidadeId = revalida.Select(x => x.intEspecialidadeId).ToList();

                        var especialidade = (from e in ctx.tblEspecialidades
                                            where listaEspecialidadeId.Contains(e.intEspecialidadeID)
                                            select new
                                            {
                                                e.intEspecialidadeID,
                                                e.DE_ESPECIALIDADE,
                                                e.CD_ESPECIALIDADE,
                                            });

                        var temaRevalida = (from r in revalida
                                            join e in especialidade on r.intEspecialidadeId equals e.intEspecialidadeID
                                            orderby r.IntOrdemRi, r.IntOrdem
                                            select new
                                            {
                                                TemaDescricao = r.TemaDescricao,
                                                IdRevalidaIndice = r.IdRevalidaIndice,
                                                IdTema = r.IdTema,
                                                IdRevalidaVideo = r.IdRevalidaVideo,
                                                RevisaoVideoDescricao = r.RevisaoVideoDescricao,
                                                IdVideo = r.IdVideo,
                                                IdProfessor = r.IdProfessor,
                                                ProfessorNome = r.ProfessorNome,
                                                IntOrdem = r.IntOrdem,
                                                IntOrdemRi = r.IntOrdemRi,
                                                GrupoId = r.GrupoId,
                                                EspecialidadeId = e.intEspecialidadeID,
                                                EspecialidadeNome = e.DE_ESPECIALIDADE,
                                                EspecialidadeSigla = e.CD_ESPECIALIDADE,
                                            }).ToList();

                        List<int> videoIds = temaRevalida.Select(x => x.IdVideo).ToList();

                        var consultaVideo = (from v in ctx.tblVideo
                                            where videoIds.Contains(v.intVideoID)
                                            select new
                                            {
                                                intVideoID = v.intVideoID,
                                                KeyVideo = v.txtFileName,
                                                Duracao = v.intDuracao,
                                                intVimeoId = v.intVimeoID,
                                                TxtUrlVimeo = v.txtUrlVimeo,
                                                TxtUrlThumbVimeo = v.txtUrlThumbVimeo,
                                                TxtUrlStreamVimeo = v.txtUrlStreamVimeo
                                            }).ToList();

                        var consulta = (from t in temaRevalida
                                        join v in consultaVideo
                                        on t.IdVideo equals v.intVideoID
                                        orderby t.IntOrdemRi, t.IntOrdem
                                        select new
                                        {
                                            t.TemaDescricao,
                                            t.IdRevalidaIndice,
                                            t.IdTema,
                                            t.IdRevalidaVideo,
                                            t.RevisaoVideoDescricao,
                                            t.IdVideo,
                                            t.IdProfessor,
                                            t.ProfessorNome,
                                            v.KeyVideo,
                                            v.Duracao,
                                            t.IntOrdem,
                                            t.IntOrdemRi,
                                            t.GrupoId,
                                            t.EspecialidadeId,
                                            t.EspecialidadeNome,
                                            t.EspecialidadeSigla,
                                            v.intVimeoId,
                                            v.TxtUrlVimeo,
                                            v.TxtUrlThumbVimeo,
                                            v.TxtUrlStreamVimeo
                                        });

                        var permitidos = new List<long>();

                        var consultaTemas = (from c in consulta
                                            select new
                                            {
                                                c.TemaDescricao,
                                                c.IdTema,
                                                c.IdRevalidaIndice,
                                                c.EspecialidadeId,
                                                c.EspecialidadeSigla,
                                                c.GrupoId
                                            }).Distinct().ToList();

                        foreach (var tema in consultaTemas)
                        {
                            var temaRetorno = new TemaApostila()
                            {
                                Professores = new List<Pessoa>(),
                                Id = tema.IdRevalidaIndice,
                                IdTema = tema.IdTema,
                                Descricao = tema.TemaDescricao,
                                VideosRevisao = new VideosMednet(), //VideosRevisao = new VideosMednet(),
                                Apostila = new Exercicio
                                {
                                    ID = tema.IdTema,
                                    Descricao = tema.EspecialidadeId == 110 ? "GO" : tema.EspecialidadeSigla,
                                    Especialidade = new Especialidade { Id = tema.EspecialidadeId/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                                },
                                GrupoId = tema.GrupoId,
                                Assunto = new AssuntoTemaApostila
                                {
                                    Id = tema.GrupoId.Value,
                                    Descricao = string.Concat("Especial Revalida ", tema.GrupoId)
                                }
                            };

                            var videos = consulta.Where(tm => tm.IdRevalidaIndice == tema.IdRevalidaIndice).ToList();

                            foreach (var video in videos)
                            {
                                var videoAdd = new VideoMednet()
                                {
                                    IdProfessor = video.IdProfessor,
                                    Url = GetVideo(Convert.ToInt32(video.IdVideo), idAplicacao),
                                    KeyVideo = video.KeyVideo.Replace(".xml", string.Empty),
                                    ID = Convert.ToInt32(video.IdVideo),
                                    Descricao = video.RevisaoVideoDescricao,
                                    DuracaoFormatada = Utilidades.GetDuracaoFormatada(video.Duracao),
                                    IdRevisaoAula = video.IdRevalidaVideo, // IdRevisaoAula = video.IdRevisaoVideo,
                                    Ordem = video.IntOrdem ?? 0,
                                    Thumb = video.TxtUrlThumbVimeo != null ? video.TxtUrlThumbVimeo : (video.intVimeoId != null ? new VideoBusiness(new VideoEntity()).ObterURLThumbVimeo(video.IdVideo) : VideoEntity.GetUrlThumb(video.KeyVideo.Replace(".xml", string.Empty), Utilidades.VideoThumbSize.width_320))
                                    // VideoEntity.GetUrlThumb(video.KeyVideo.Replace(".xml", string.Empty), Utilidades.VideoThumbSize.width_320)
                                };

                                if (!temaRetorno.Professores.Any(p => p.ID == video.IdProfessor))
                                    temaRetorno.Professores.Add(new Pessoa
                                    {
                                        Nome = video.ProfessorNome,
                                        ID = video.IdProfessor,
                                        UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", video.IdProfessor)
                                    });

                                temaRetorno.VideosRevisao.Add(videoAdd);
                            }
                            if (temaRetorno.VideosRevisao.Count() > 0)
                                temasRetorno.Add(temaRetorno);
                        }
                        return temasRetorno;
                    }
                }
            }
        }


        public TemasApostila GetTemasVideoRevalida(int idAplicacao = (int)MedCore_DataAccess.Contracts.Enums.Aplicacoes.MsProMobile)
        {
            using(MiniProfiler.Current.Step("Obtendo vídeos de revalida"))
            {
                using (var ctx = new AcademicoContext())
                {
                    using (var ctxMatDir = new DesenvContext())
                    {
                        var temasRetorno = new TemasApostila();

                        var Revalida = (from ri in ctxMatDir.tblRevalidaAulaIndice
                                        join rv in ctxMatDir.tblRevalidaAulaVideo on ri.intRevalidaAulaIndiceId equals rv.intRevalidaAulaIndiceId
                                        join lt in ctxMatDir.tblLessonTitleRevalida on ri.intLessonTitleRevalidaId equals lt.intLessonTitleRevalidaId
                                        join prof in ctxMatDir.tblPersons on rv.intProfessorId equals prof.intContactID
                                        where rv.intVideoId != 0
                                        orderby ri.intOrdem, rv.intOrdem
                                        select new
                                        {
                                            TemaDescricao = lt.txtName,
                                            IdRevalidaIndice = ri.intRevalidaAulaIndiceId,
                                            IdTema = ri.intRevalidaAulaIndiceId,
                                            IdRevalidaVideo = rv.intRevalidaAulaVideoId,
                                            RevisaoVideoDescricao = rv.txtDescricao,
                                            IdVideo = rv.intVideoId,
                                            IdProfessor = rv.intProfessorId,
                                            ProfessorNome = prof.txtName.Trim(),
                                            IntOrdem = rv.intOrdem,
                                            IntOrdemRi = ri.intOrdem,
                                            GrupoId = lt.GrupoId,
                                            lt.intEspecialidadeId
                                        }).ToList();

                        List<int?> listaIdEspeciadade = Revalida.Select(x => x.intEspecialidadeId).ToList();

                        var especialidade = (from e in ctx.tblEspecialidades
                                            where listaIdEspeciadade.Contains(e.intEspecialidadeID)
                                            select new
                                            {
                                                e.intEspecialidadeID,
                                                e.DE_ESPECIALIDADE,
                                                e.CD_ESPECIALIDADE
                                            }).ToList();

                        var videoRevalida = (from r in Revalida
                                            join e in especialidade on r.intEspecialidadeId equals e.intEspecialidadeID
                                            orderby r.IntOrdemRi, r.IntOrdem
                                            select new
                                            {
                                                TemaDescricao = r.TemaDescricao,
                                                IdRevalidaIndice = r.IdRevalidaIndice,
                                                IdTema = r.IdTema,
                                                IdRevalidaVideo = r.IdRevalidaVideo,
                                                RevisaoVideoDescricao = r.RevisaoVideoDescricao,
                                                IdVideo = r.IdVideo,
                                                IdProfessor = r.IdProfessor,
                                                ProfessorNome = r.ProfessorNome.Trim(),
                                                IntOrdem = r.IntOrdem,
                                                IntOrdemRi = r.IntOrdemRi,
                                                GrupoId = r.GrupoId,
                                                EspecialidadeId = e.intEspecialidadeID,
                                                EspecialidadeNome = e.DE_ESPECIALIDADE,
                                                EspecialidadeSigla = e.CD_ESPECIALIDADE
                                            }).ToList();

                        List<int> videoIds = videoRevalida.Select(x => x.IdVideo).ToList();

                        var consultaVideo = (from v in ctx.tblVideo
                                            where videoIds.Contains(v.intVideoID)
                                            select new
                                            {
                                                intVideoID = v.intVideoID,
                                                KeyVideo = v.txtFileName,
                                                Duracao = v.intDuracao,
                                                intVimeoId = v.intVimeoID,
                                                TxtUrlVimeo = v.txtUrlVimeo,
                                                TxtUrlThumbVimeo = v.txtUrlThumbVimeo,
                                                TxtUrlStreamVimeo = v.txtUrlStreamVimeo,
                                                InfoVideo = v.txtVideoInfo
                                            }).ToList();

                        var consulta = (from r in videoRevalida
                                        join v in consultaVideo on r.IdVideo equals v.intVideoID
                                        orderby r.IntOrdemRi, r.IntOrdem
                                        select new
                                        {
                                            r.TemaDescricao,
                                            r.IdRevalidaIndice,
                                            r.IdTema,
                                            r.IdRevalidaVideo,
                                            r.RevisaoVideoDescricao,
                                            r.IdVideo,
                                            r.IdProfessor,
                                            r.ProfessorNome,
                                            v.KeyVideo,
                                            v.Duracao,
                                            r.IntOrdem,
                                            r.GrupoId,
                                            r.EspecialidadeId,
                                            r.EspecialidadeNome,
                                            r.EspecialidadeSigla,
                                            v.intVimeoId,
                                            v.TxtUrlVimeo,
                                            v.TxtUrlThumbVimeo,
                                            v.TxtUrlStreamVimeo,
                                            v.InfoVideo
                                        }).ToList();

                        var permitidos = new List<long>();

                        var consultaTemas = (from c in consulta
                                            select new
                                            {
                                                c.TemaDescricao,
                                                c.IdTema,
                                                c.IdRevalidaIndice,
                                                c.EspecialidadeId,
                                                c.EspecialidadeSigla,
                                                c.GrupoId
                                            }).Distinct().ToList();

                        foreach (var tema in consultaTemas)
                        {
                            var temaRetorno = new TemaApostila()
                            {
                                Professores = new List<Pessoa>(),
                                Id = tema.IdRevalidaIndice,
                                IdTema = tema.IdTema,
                                Descricao = tema.TemaDescricao,
                                VideosRevisao = new VideosMednet(), //VideosRevisao = new VideosMednet(),
                                Apostila = new Exercicio
                                {
                                    //ID = 7768,
                                    //Descricao = "CAR 1",
                                    //Especialidade = new Especialidade { Id = 11/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                                    ID = tema.IdTema,
                                    Descricao = tema.EspecialidadeId == 110 ? "GO" : tema.EspecialidadeSigla,
                                    Especialidade = new Especialidade { Id = tema.EspecialidadeId/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                                },
                                GrupoId = tema.GrupoId,
                                Assunto = new AssuntoTemaApostila
                                {
                                    Id = tema.GrupoId.Value,
                                    Descricao = string.Concat("Especial Revalida ", tema.GrupoId)
                                }
                            };

                            var videos = consulta.Where(tm => tm.IdRevalidaIndice == tema.IdRevalidaIndice).ToList();

                            var videoEntity = new VideoEntity();
                            var videoBus = new VideoBusiness(videoEntity);


                            foreach (var video in videos)
                            {
                                var url = GetVideo(Convert.ToInt32(video.IdVideo), idAplicacao);
                                var videoAdd = new VideoMednet()
                                {
                                    IdProfessor = video.IdProfessor,
                                    Url = url,
                                    KeyVideo = video.KeyVideo.Replace(".xml", string.Empty),
                                    ID = Convert.ToInt32(video.IdVideo),
                                    Descricao = video.RevisaoVideoDescricao,
                                    DuracaoFormatada = Utilidades.GetDuracaoFormatada(video.Duracao),
                                    IdRevisaoAula = video.IdRevalidaVideo,
                                    Ordem = video.IntOrdem ?? 0,
                                    Thumb = video.TxtUrlThumbVimeo != null ? video.TxtUrlThumbVimeo : (video.intVimeoId != null ? videoBus.ObterURLThumbVimeo(video.IdVideo) : VideoEntity.GetUrlThumb(video.KeyVideo.Replace(".xml", string.Empty), Utilidades.VideoThumbSize.width_320)),
                                    Links = videoEntity.GetLinksVideoVariasQualidades(video.InfoVideo, url)

                                };

                                if (!temaRetorno.Professores.Any(p => p.ID == video.IdProfessor))
                                    temaRetorno.Professores.Add(new Pessoa
                                    {
                                        Nome = video.ProfessorNome,
                                        ID = video.IdProfessor,
                                        UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", video.IdProfessor)
                                    });

                                temaRetorno.VideosRevisao.Add(videoAdd);
                            }
                            if (temaRetorno.VideosRevisao.Count() > 0)
                                temasRetorno.Add(temaRetorno);
                        }

                        return temasRetorno;
                    }
                }
            }
        }



        public string GetVideo(int idVideo, int idAplicacao = (int)MedCore_DataAccess.Contracts.Enums.Aplicacoes.MsProMobile, string versaoApp = "")
        {
            //return new VideoEntity().GetUrlVideoPorVideoID(idVideo, _chaveamentoVimeoAulaRevisao, idAplicacao, null);
            return new VideoEntity().GetUrlVideoPorVideoID(idVideo, _chaveamentoVimeoAulaRevisao, idAplicacao, versaoApp);
        }

        public AvaliacaoProfessor GetAvaliacaoRealizada(int matricula, int idRevisaoAulaIndice, int idAplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo avaliação realizada"))
            {
                try
                {
                    using (var ctx = new DesenvContext())
                    {
                        var avaliacao = new AvaliacaoProfessor();
                        var ultimaAvaliacao = ctx.tblLessonsEvaluationRevisaoAula.Where(e =>
                            e.intClientId == matricula
                            && e.intRevisaoAulaIndiceId == idRevisaoAulaIndice).ToList().FirstOrDefault();

                        if (ultimaAvaliacao != null)
                        {
                            avaliacao.NotaFinal = ultimaAvaliacao.intNota;
                            avaliacao.Professor = new Pessoa { ID = ultimaAvaliacao.intEmployeeId };
                        }

                        return avaliacao;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public TemaApostila GetVideos(int idProduto, int matricula, int idTema, int idAplicacao)
        {
            try
            {
                using(MiniProfiler.Current.Step("Obtendo vídeos"))
                {
                    var todosVideos = GetTemasVideos(idProduto, matricula, 0, 0, false, idTema, tipoVideo: ETipoVideo.Revisao);
                    var videoTema = todosVideos.Where(v => v.IdTema == idTema).FirstOrDefault();

                    if (videoTema != null)
                    {
                        var idsVideos = videoTema.VideosRevisao.Select(v => v.IdRevisaoAula).ToList();

                        var ctx = new DesenvContext();

                        var totalVisualizacoesVideos = ctx.tblRevisaoAulaVideoLog.Where(l => idsVideos.Contains(l.intRevisaoAulaId)).ToList();
                        var todosProgressosVideos = ctx.tblRevisaoAulaVideoLogPosition.Where(m => m.intClientId == matricula).ToList();

                        ctx.Dispose();

                        foreach (var professor in videoTema.Professores)
                        {
                            var segundosTotalVideos = 0;
                            var segundosTotalVideosAssistidos = 0;
                            var videosProf = videoTema.VideosRevisao.Where(v => v.IdProfessor == professor.ID).ToList();

                            foreach (var video in videosProf)
                            {
                                video.Visualizacoes = totalVisualizacoesVideos
                                    .Where(t => t.intRevisaoAulaId == video.IdRevisaoAula)
                                    .Count();

                                segundosTotalVideos += Convert.ToInt32(TimeSpan.Parse(video.DuracaoFormatada).TotalSeconds);

                                if (todosProgressosVideos.Any(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula))
                                    segundosTotalVideosAssistidos += todosProgressosVideos.FirstOrDefault(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula).intSecond;

                                var duracaoVideo = Convert.ToInt32(TimeSpan.Parse(video.DuracaoFormatada).TotalSeconds);

                                if (todosProgressosVideos.Any(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula) && duracaoVideo > 0)
                                    video.Progresso = (todosProgressosVideos.Where(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula).FirstOrDefault().intSecond * 100) / duracaoVideo;

                            }
                            if (segundosTotalVideos != 0)
                                professor.PercentVisualizado = (segundosTotalVideosAssistidos * 100) / segundosTotalVideos;
                            else
                                professor.PercentVisualizado = 0;

                            SetProgressoAulaRevisao(idTema, professor.ID, matricula, professor.PercentVisualizado);
                        }
                    }
                    else
                    {
                        videoTema = new TemaApostila();
                    }

                    return videoTema;
                }
            }
            catch
            {
                throw;
            }
        }

        public TemasApostila GetTemasVideos(int idProduto, int matricula, int intProfessor, int intAula, bool isAdmin, int idTema = 0, int idApostila = 0, ETipoVideo tipoVideo = ETipoVideo.Revisao, int idAplicacao = (int)Aplicacoes.MEDSOFT, string versaoApp = "")
        {
            var temas = new TemasApostila();

            switch (tipoVideo)
            {
                case ETipoVideo.Revisao:
                    temas = GetTemasVideosRevisao(idProduto, matricula, intProfessor, intAula, isAdmin, idTema, idApostila, idAplicacao, versaoApp);
                    break;
                case ETipoVideo.Resumo:
                    temas = GetTemasVideoResumo(idProduto, matricula, intProfessor, intAula, isAdmin, idTema);
                    break;
                case ETipoVideo.AdaptaMed:
                    temas = GetTemasVideoAdaptaMed(idProduto, matricula, intProfessor, intAula, isAdmin, idTema);
                    break;
                case ETipoVideo.ProvaVideo:
                    temas = GetTemasVideoProvaVideo(matricula, intProfessor, idTema, idAplicacao, versaoApp);
                    break;
            }

            return temas;
        }

        public TemasApostila GetTemasVideoProvaVideo(int matricula, int intProfessor, int idTema = 0, int idAplicacao = (int)Aplicacoes.MEDSOFT, string versaoApp = "")
        {
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    var acessoAntecipado = Utilidades.PossuiMaterialAntecipado(matricula);

                    var temasRetorno = new TemasApostila();

                    LogVideos = GetLogVideos(matricula, ETipoVideo.ProvaVideo);

                    //INFORMAÇÕES DOS VIDEOS QUE VEM DAS TABELAS NOVAS
                    var provaVideo = (from ri in ctxMatDir.tblProvaVideoIndice
                                      join rv in ctxMatDir.tblProvaVideo on ri.intProvaVideoIndiceId equals rv.intProvaVideoIndiceId
                                      join lt in ctxMatDir.tblLessonTitles on ri.intLessonTitleId equals lt.intLessonTitleID
                                      join prof in ctxMatDir.tblPersons on rv.intProfessorId equals prof.intContactID
                                      where rv.intVideoId != 0 && (idTema == 0 || (idTema != 0 && lt.intLessonTitleID == idTema))
                                      && (intProfessor == 0 || (intProfessor != 0 && rv.intProfessorId == intProfessor))


                                      select new
                                      {
                                          TemaDescricao = lt.txtLessonTitleName,
                                          IdProvaVideoIndice = ri.intProvaVideoIndiceId,
                                          IdTema = ri.intLessonTitleId,
                                          IdProvaVideo = rv.intProvaVideoId,
                                          ProvaVideoDescricao = rv.txtDescricao,
                                          IdVideo = rv.intVideoId,
                                          IdProfessor = rv.intProfessorId,
                                          ProfessorNome = prof.txtName.Trim(),
                                          intOrdemRi = ri.intOrdem,
                                          IntOrdem = rv.intOrdem,
                                          DataRelease = rv.dteLiberacao,
                                          Ativo = (acessoAntecipado || (rv.bitAtivo && rv.dteLiberacao < DateTime.Now)),
                                          PossueAnexo = rv.bitPossuiAnexo
                                      }).ToList();

                    List<int> videoIds = provaVideo.Select(x => x.IdVideo).ToList();

                    //TRAZER OS VIDEOS DO BANCO ACADEMICO
                    var consultaVideo = (from v in ctx.tblVideo
                                         where videoIds.Contains(v.intVideoID)
                                         select new
                                         {
                                             intVideoID = v.intVideoID,
                                             KeyVideo = v.txtFileName,
                                             Duracao = v.intDuracao,
                                             UrlStream = v.txtUrlStreamVimeo,
                                             UrlMp4 = v.txtUrlVimeo,
                                             UrlThumb = v.txtUrlThumbVimeo,
                                             VimeoId = v.intVimeoID,
                                             InfoVideo = v.txtVideoInfo,
                                             bitActive = v.bitActive,
                                             intDuracao = v.intDuracao
                                         }).ToList();

                    var consulta = (from r in provaVideo
                                    join v in consultaVideo
                                    on r.IdVideo equals v.intVideoID
                                    orderby r.intOrdemRi, r.IntOrdem
                                    select new
                                    {
                                        r.TemaDescricao,
                                        r.IdProvaVideoIndice,
                                        r.IdTema,
                                        r.IdProvaVideo,
                                        r.ProvaVideoDescricao,
                                        r.IdVideo,
                                        r.IdProfessor,
                                        r.ProfessorNome,
                                        v.KeyVideo,
                                        v.Duracao,
                                        r.IntOrdem,
                                        v.UrlStream,
                                        v.UrlMp4,
                                        v.UrlThumb,
                                        v.VimeoId,
                                        v.InfoVideo,
                                        v.bitActive,
                                        v.intDuracao,
                                        r.Ativo,
                                        r.DataRelease,
                                        r.PossueAnexo
                                    }).ToList();


                    var consultaTemas = (from c in consulta
                                         select new
                                         {
                                             c.TemaDescricao,
                                             c.IdTema,
                                             c.IdProvaVideoIndice
                                         }).Distinct().ToList();

                    //TODO: FAZER A APROVAÇÃO DO ACADEMICO PARA O PROVA
                    var aprovacoesAcademico = ctxMatDir.tblRevisaoAulaVideoAprovacao
                                                    .Where(r => r.intRevisaoAulaVideoTipoAprovadorId == (int)AulaRevisaoVideoAprovacao.TipoAprovadorEnum.Academico
                                                                && r.bitAprovado).ToList();

                    foreach (var tema in consultaTemas)
                    {
                        var temaRetorno = new TemaApostila()
                        {
                            Professores = new List<Pessoa>(),
                            Id = tema.IdProvaVideoIndice,
                            IdTema = tema.IdTema,
                            Descricao = tema.TemaDescricao,
                            Videos = new VideosMednet(),

                        };

                        var videos = consulta.Where(tm => tm.IdProvaVideoIndice == tema.IdProvaVideoIndice).ToList();

                        VideoEntity videoEntity = new VideoEntity();
                        VideoBusiness videobusiness = new VideoBusiness(videoEntity);

                        foreach (var video in videos)
                        {
                            var url = GetVideo(video.IdVideo, idAplicacao);
                            var notaVideo = GetNotaVideoProva(video.IdProvaVideo, matricula);
                            int intDuracao = 0;
                            int.TryParse(video.intDuracao, out intDuracao);
                            var videoAdd = new VideoMednet()
                            {
                                IdProfessor = video.IdProfessor,
                                Url = url,
                                KeyVideo = video.KeyVideo.Replace(".xml", string.Empty),
                                ID = Convert.ToInt32(video.IdVideo),
                                Descricao = video.ProvaVideoDescricao,
                                Assistido = LogVideos.Any(l => l == video.IdProvaVideo),
                                DuracaoFormatada = Utilidades.GetDuracaoFormatada(video.Duracao),
                                IdProvaVideo = video.IdProvaVideo,
                                Ordem = video.IntOrdem,
                                Thumb = videobusiness.GetUrlThumbVideoRevisao(video.KeyVideo, Utilidades.VideoThumbSize.width_320, video.UrlThumb, video.VimeoId),
                                TipoVideo = ETipoVideo.ProvaVideo,
                                Links = videoEntity.GetLinksVideoVariasQualidades(video.InfoVideo, url),
                                Ativo = video.Ativo,
                                Duracao = intDuracao,
                                NotaVideo = notaVideo,
                                DataRelease = video.DataRelease,
                                PossuiAnexo = video.PossueAnexo
                            };

                            if (!temaRetorno.Professores.Any(p => p.ID == video.IdProfessor))
                                temaRetorno.Professores.Add(new Pessoa
                                {
                                    Nome = video.ProfessorNome,
                                    ID = video.IdProfessor,
                                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", video.IdProfessor)
                                });

                            temaRetorno.Videos.Add(videoAdd);
                        }

                        if (temaRetorno.Videos.Count() > 0)
                        {
                            temasRetorno.Add(temaRetorno);
                        }
                    }

                    return temasRetorno;
                }
            }
        }

        public int? GetNotaVideoProva(int idProvaVideo, int matricula)
        {
            int? nota = new int?();
            using (var ctx = new DesenvContext())
            {
                var result = (from n in ctx.tblLessonsEvaluationProvaVideo
                              where n.intProvaVideoId == idProvaVideo
                              && n.intClientId == matricula
                              select n.intNota).ToList();
                if (result.Count() > 0) nota = result[0];
            }
            return nota;
        }

        public int SetProgressoAulaRevisao(int idTema, int idProfessor, int idCliente, int percentualVisualizado)
        {
            using(MiniProfiler.Current.Step("Marcando progresso aula de revisão"))
            {
                using (var ctx = new DesenvContext())
                {
                    var progressoAtual = ctx.tblRevisaoAulaTemaProfessorAssistido.FirstOrDefault(p => p.intClientID == idCliente
                                                                                            && p.intLessonTitleID == idTema
                                                                                            && p.intProfessorID == idProfessor);

                    if (progressoAtual == null)
                    {
                        ctx.tblRevisaoAulaTemaProfessorAssistido.Add(new tblRevisaoAulaTemaProfessorAssistido()
                        {
                            intClientID = idCliente,
                            intLessonTitleID = idTema,
                            intPercentVisualizado = percentualVisualizado,
                            intProfessorID = idProfessor
                        });
                    }
                    else
                    {
                        progressoAtual.intPercentVisualizado = percentualVisualizado;
                    }
                    ctx.SaveChanges();

                    return 1;
                }
            }
        }

    public TemasApostila GetTemasVideosRevisao(int idProduto, int matricula, int intProfessor, int intAula, bool isAdmin, int idTema = 0, int idEntidade = 0, int idAplicacao = (int)Aplicacoes.MEDSOFT, string versaoApp = "")
        {
            List<int> idProdutos = idProduto == (int)Produto.Cursos.MEDCURSO || idProduto == (int)Produto.Cursos.MED
                ? new List<int>() { idProduto, (int)Produto.Cursos.MEDMEDCURSO }
                : idProduto == (int)Produto.Cursos.MED_AULAS_ESPECIAIS
                ? new List<int>() { (int)Produto.Cursos.MED, (int)Produto.Cursos.MEDMEDCURSO }
                : idProduto == (int)Produto.Cursos.MEDCURSO_AULAS_ESPECIAIS
                ? new List<int>() { (int)Produto.Cursos.MEDCURSO, (int)Produto.Cursos.MEDMEDCURSO }
                : new List<int>() { idProduto };

            if (idProduto == (int)Produto.Cursos.REVALIDA)
            {
                return VideosRevalidaHardCode();
            }

            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    //isAdmin = isAdmin ? new FuncionarioEntity().PermissaoRole(66, matricula) : false;
                  
                    ctxMatDir.SetCommandTimeOut(120);

                    var temasRetorno = new TemasApostila();

                    LogVideos = GetLogVideos(matricula, ETipoVideo.Revisao);

                    var lstIdsTema = new List<int>() { idTema };
                    if (idTema != 0)
                    {
                        var lstIdsTemaEspecial = GetTemasAulaEspecial(idTema, matricula, idProduto, DateTime.Now.Year);
                        foreach (var item in lstIdsTemaEspecial)
                            lstIdsTema.Add(item);
                    }

                    var revisao = (from ri in ctxMatDir.tblRevisaoAulaIndice
                                   join rv in ctxMatDir.tblRevisaoAulaVideo on ri.intRevisaoAulaIndiceId equals rv.intRevisaoAulaIndiceId
                                   join p in ctxMatDir.tblProducts on ri.intBookId equals p.intProductID
                                   join b in ctxMatDir.tblBooks on p.intProductID equals b.intBookID
                                   join a in ctxMatDir.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on p.intProductGroup3 equals a.intProductGroupID
                                   join lt in ctxMatDir.tblLessonTitles on ri.intLessonTitleId equals lt.intLessonTitleID
                                   join prof in ctxMatDir.tblPersons on rv.intProfessorId equals prof.intContactID
                                   //join v in ctxMatDir.tblVideos on rv.intVideoId equals v.intVideoID
                                   where idProdutos.Contains(p.intProductGroup2.Value) && rv.intVideoId != 0
                                   //&& (idTema == 0 || (idTema != 0 && lt.intLessonTitleID == idTema)) 
                                   && (idTema == 0 || (idTema != 0 && lstIdsTema.Contains(lt.intLessonTitleID)))
                                   && (intProfessor == 0 || (intProfessor != 0 && rv.intProfessorId == intProfessor))
                                   && (intAula == 0 || (intAula != 0 && ri.intRevisaoAulaIndiceId == intAula))
                                   && (idEntidade == 0 || (idEntidade != 0 && b.intBookEntityID == idEntidade && lt.txtLessonTitleName.Contains("[AULA BÔNUS]")))
                                   //orderby ri.intOrdem, rv.intOrdem
                                   select new
                                   {
                                       CodeApostila = p.txtCode.Remove(0, 5),
                                       IdEspecialidade = (a.intTipoDeClassificacao == 3 ? 11 : a.intClassificacaoID),
                                       TemaDescricao = lt.txtLessonTitleName,
                                       IdRevisaoIndice = ri.intRevisaoAulaIndiceId,
                                       IdApostila = ri.intBookId,
                                       IdTema = ri.intLessonTitleId,
                                       IdRevisaoVideo = rv.intRevisaoAulaId,
                                       RevisaoVideoDescricao = rv.txtDescricao,
                                       RevisaoVideoCue = rv.intCuePoint,
                                       IdVideo = rv.intVideoId,
                                       Entidade = b.intBookEntityID,
                                       IdProfessor = rv.intProfessorId,
                                       ProfessorNome = prof.txtName.Trim(),
                                       IdVideoPai = rv.intRevisaoAulaIdPai,
                                       intOrdemRi = ri.intOrdem,
                                       IntOrdem = rv.intOrdem
                                       //KeyVideo = v.txtFileName,
                                       //Duracao = v.intDuracao,
                                       //IntOrdem = rv.intOrdem,
                                       //UrlStream = v.txtUrlStreamVimeo,
                                       //UrlMp4 = v.txtUrlVimeo,
                                       //UrlThumb = v.txtUrlThumbVimeo,
                                       //VimeoId = v.intVimeoID
                                   }).ToList();

         


                    List<int?> videoIds = revisao.Select(x => x.IdVideo).ToList();

                    var consultaVideo = (from v in ctx.tblVideo
                                         where videoIds.Contains(v.intVideoID)
                                         select new
                                         {
                                             intVideoID = v.intVideoID,
                                             KeyVideo = v.txtFileName,
                                             Duracao = v.intDuracao,
                                             UrlStream = v.txtUrlStreamVimeo,
                                             UrlMp4 = v.txtUrlVimeo,
                                             UrlThumb = v.txtUrlThumbVimeo,
                                             VimeoId = v.intVimeoID,
                                             InfoVideo = v.txtVideoInfo,
                                             bitActive = v.bitActive
                                         }).ToList();

                    var consulta = (from r in revisao
                                    join v in consultaVideo
                                    on r.IdVideo equals v.intVideoID
                                    orderby r.intOrdemRi, r.IntOrdem
                                    select new
                                    {
                                        r.CodeApostila,
                                        r.IdEspecialidade,
                                        r.TemaDescricao,
                                        r.IdRevisaoIndice,
                                        r.IdApostila,
                                        r.IdTema,
                                        r.IdRevisaoVideo,
                                        r.RevisaoVideoDescricao,
                                        r.RevisaoVideoCue,
                                        r.IdVideo,
                                        r.Entidade,
                                        r.IdProfessor,
                                        r.ProfessorNome,
                                        r.IdVideoPai,
                                        v.KeyVideo,
                                        v.Duracao,
                                        r.IntOrdem,
                                        v.UrlStream,
                                        v.UrlMp4,
                                        v.UrlThumb,
                                        v.VimeoId,
                                        v.InfoVideo,
                                        v.bitActive
                                    }).ToList();

                    var apostilaUnificada = GetApostilaUnificada(idTema);

                    var consultaTemas = (from c in consulta
                                         select new
                                         {
                                             CodeApostila = (apostilaUnificada != null && apostilaUnificada.Nome != null) ? apostilaUnificada.Nome : c.CodeApostila,
                                             c.IdEspecialidade,
                                             c.TemaDescricao,
                                             c.Entidade,
                                             c.IdTema,
                                             c.IdApostila,
                                             c.IdRevisaoIndice
                                         }).Distinct().ToList();

                    var permitidos = new List<long>();
                    var isEmployee = ctxMatDir.tblEmployees.Where(e => e.intEmployeeID == matricula).ToList().Any();
                    var isMeioDeAno = ctxMatDir.tblAlunosAnoAtualMaisAnterior.Where(x => x.intClientID == matricula).ToList().Any();
                    var anoVigente = Utilidades.GetYear();
                    var isAntesDataLimite = Utilidades.IsAntesDatalimite(anoVigente - 1);
                    var anoAtual = isAntesDataLimite ? anoVigente - 1 : anoVigente;

                    if (isAdmin || isEmployee)
                        permitidos = ctxMatDir.tblBooks
                                                .Where(b => b.intYear >= anoAtual && b.intBookEntityID != null)
                                                .Select(b => b.intBookEntityID.Value).ToList();
                    else
                        permitidos = (from p in ctxMatDir.msp_Medsoft_SelectPermissaoExercicios(false, false, matricula)
                                      where p.intExercicioTipo == (int)Exercicio.tipoExercicio.APOSTILA
                                      select (long)p.intExercicioID.Value).Distinct().ToList();

                    //___________Removido do HouveAulaCronograma________
                    var anoLetivo = Utilidades.GetYear();
                    var lstOvsAluno = OrdemVendaEntity.GetOvsAluno(matricula);
                    var temChamdoAntecipacaoDeMaterial = new ChamadoCallCenterEntity().VerificarSeClienteTemChamadoAntecipacaoMaterial(matricula);
                    var isApostilaMed = (int)Produto.Cursos.MED == idProduto || (int)Produto.Cursos.MED_AULAS_ESPECIAIS == idProduto;
                    var anosConsideradosComoAtuais = isAntesDataLimite ? new[] { anoLetivo, anoLetivo - 1 }.ToList() : new[] { anoLetivo }.ToList();
                    var chamadosTrocaTemporaria = ChamadoCallCenterEntity.GetChamadosDeTrocaTemporaria(matricula, anosConsideradosComoAtuais);
                    var temOvAnosAtuais = OrdemVendaEntity.TemOvsAnoAtual(lstOvsAluno, anosConsideradosComoAtuais, isApostilaMed, isMeioDeAno);
                    //___________________________________________________

                    var IsCPMedExtensivo = idProduto == (int)Produto.Cursos.CPMED_EXTENSIVO;

                    var IsProdutoEspecial = Utilidades.ProdutosAulasEspeciais().Contains(idProduto);

                    idProduto = Utilidades.GetCursoOrigemCursoAulaEspecial(idProduto);

                    var consultaPermitida = (from c in consultaTemas
                                             join tp in permitidos on c.Entidade equals tp
                                             where isEmployee || IsCPMedExtensivo || HouveAulaPeloCronogramaNaRestrita(c.IdApostila, c.TemaDescricao, matricula, isAntesDataLimite, isMeioDeAno, (int)c.Entidade, anoLetivo, lstOvsAluno, temChamdoAntecipacaoDeMaterial, idProduto, temOvAnosAtuais, anosConsideradosComoAtuais, chamadosTrocaTemporaria)
                                             select new
                                             {
                                                 c.CodeApostila,
                                                 c.IdEspecialidade,
                                                 c.TemaDescricao,
                                                 c.Entidade,
                                                 c.IdTema,
                                                 c.IdApostila,
                                                 c.IdRevisaoIndice,
                                                 permitido = true
                                             }).Distinct().ToList();


                    if (IsCPMedExtensivo || IsProdutoEspecial)
                    {
                        var consultaTemasNaoPermitidos = consultaTemas.Where(c => !consultaPermitida.Any(p => p.Entidade == c.Entidade))
                                                        .Select(c => new
                                                        {
                                                            c.CodeApostila,
                                                            c.IdEspecialidade,
                                                            c.TemaDescricao,
                                                            c.Entidade,
                                                            c.IdTema,
                                                            c.IdApostila,
                                                            c.IdRevisaoIndice,
                                                            permitido = false
                                                        }).Distinct().ToList();

                        consultaPermitida = consultaPermitida.Concat(consultaTemasNaoPermitidos).ToList();
                    }

                    var aprovacoesAcademico = ctxMatDir.tblRevisaoAulaVideoAprovacao
                                                    .Where(r => r.intRevisaoAulaVideoTipoAprovadorId == (int)AulaRevisaoVideoAprovacao.TipoAprovadorEnum.Academico
                                                                && r.bitAprovado).ToList();

                    var nomeAmigavel = ctxMatDir.tblProductCodes.Where(x => x.intLessonTitleID == idTema).Select(x => x.txtCode).FirstOrDefault();

                    foreach (var tema in consultaPermitida)
                    {
                        var temaRetorno = new TemaApostila()
                        {
                            Professores = new List<Pessoa>(),
                            Id = tema.IdRevisaoIndice,
                            IdTema = tema.IdTema,
                            Descricao = (idProduto == (int)Produto.Cursos.CPMED_EXTENSIVO && !string.IsNullOrEmpty(nomeAmigavel)) ||
                                        (idProduto == (int)Produto.Cursos.R3Cirurgia && !string.IsNullOrEmpty(nomeAmigavel)) ||
                                        (idProduto == (int)Produto.Cursos.R3Clinica && !string.IsNullOrEmpty(nomeAmigavel)) ||
                                        (idProduto == (int)Produto.Cursos.R3Pediatria && !string.IsNullOrEmpty(nomeAmigavel)) ||
                                        (idProduto == (int)Produto.Cursos.R4GO && !string.IsNullOrEmpty(nomeAmigavel)) ||
                                        (idProduto == (int)Produto.Cursos.TEGO && !string.IsNullOrEmpty(nomeAmigavel)) ||
                                        (idProduto == (int)Produto.Cursos.MASTO && !string.IsNullOrEmpty(nomeAmigavel))
                            ? nomeAmigavel : string.Concat(tema.CodeApostila, " - ", tema.TemaDescricao),
                            VideosRevisao = new VideosMednet(),
                            Apostila = new Exercicio
                            {
                                ID = tema.IdApostila,
                                Descricao = tema.CodeApostila,
                                EntidadeApostilaID = Convert.ToInt32(tema.Entidade),
                                Especialidade = new Especialidade { Id = GetClassificacao(idProduto, tema.IdEspecialidade) }
                            }
                        };

                        var videos = consulta.Where(tm => tm.IdRevisaoIndice == tema.IdRevisaoIndice).ToList();

                        VideoEntity videoEntity = new VideoEntity();
                        VideoBusiness videobusiness = new VideoBusiness(videoEntity);

                        foreach (var video in videos)
                        {
                            if (!isAdmin && !aprovacoesAcademico.Any(a => a.intRevisaoAulaVideoId == video.IdRevisaoVideo && a.intVideoId == video.IdVideo)) continue;
                            var url = videobusiness.GetUrlVideoRevisao(video.KeyVideo, Utilidades.VideoThumbSize.width_720, video.UrlStream, video.UrlMp4, video.IdVideo, idAplicacao, versaoApp);

                            var notaVideo = GetNotaVideoAulaRevisao(Convert.ToInt32(video.IdVideo), matricula);

                            var videoAdd = new VideoMednet()
                            {
                                IdProfessor = video.IdProfessor,
                                Cue = Convert.ToInt32(video.RevisaoVideoCue),
                                Url = url,
                                KeyVideo = video.KeyVideo.Replace(".xml", string.Empty),
                                ID = Convert.ToInt32(video.IdVideo),
                                Descricao = video.RevisaoVideoDescricao,
                                Assistido = LogVideos.Any(l => l == video.IdRevisaoVideo),
                                DuracaoFormatada = Utilidades.GetDuracaoFormatada(video.Duracao),
                                IdRevisaoAula = video.IdRevisaoVideo,
                                Ordem = video.IntOrdem ?? 0,
                                Thumb = videobusiness.GetUrlThumbVideoRevisao(video.KeyVideo, Utilidades.VideoThumbSize.width_320, video.UrlThumb, video.VimeoId),
                                TipoVideo = ETipoVideo.Revisao,
                                Links = videoEntity.GetLinksVideoVariasQualidades(video.InfoVideo, url),
                                Ativo = tema.permitido,
                                NotaVideo = notaVideo
                            };

                            if (!temaRetorno.Professores.Any(p => p.ID == video.IdProfessor))
                                temaRetorno.Professores.Add(new Pessoa
                                {
                                    Nome = video.ProfessorNome,
                                    ID = video.IdProfessor,
                                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", video.IdProfessor)
                                });

                            temaRetorno.VideosRevisao.Add(videoAdd);
                        }

                        if (temaRetorno.VideosRevisao.Count() > 0)
                        {
                            temasRetorno.Add(temaRetorno);
                        }
                    }

                    var temasApostilasOrdenados = new TemasApostila();
                    //____________________________________________________________________Colocar/Ordenar Aula Bonus no Final de cada Apostila
                    for (int i = 0; i < temasRetorno.Count; i++)
                    {
                        if (temasRetorno[i].Descricao.Contains("[AULA BÔNUS]"))
                        {
                            var nome = temasRetorno[i].Descricao.Substring(0, 5);
                            temasRetorno[i].Descricao = temasRetorno[i].Descricao.Replace(nome, nome + "z");
                            temasRetorno[i].VideosRevisao.ForEach(x => x.TipoVideo = ETipoVideo.Bonus);
                        }

                        else if (temasRetorno[i].Descricao.Contains("AULA ESPECIAL"))
                        {
                            temasRetorno[i].VideosRevisao.ForEach(x => x.TipoVideo = ETipoVideo.Especial);
                        }
                    }

                    var temasOrdenados = temasRetorno.OrderBy(x => x.Descricao).ToList();

                    foreach (var elementos in temasOrdenados)
                    {
                        var nome = string.Concat(elementos.Descricao.Substring(0, 5), "z");

                        if (elementos.Descricao.Contains(nome))
                            elementos.Descricao = elementos.Descricao.Replace(nome, elementos.Descricao.Substring(0, 5));

                        temasApostilasOrdenados.Add(elementos);
                    }
                    //____________________________________________________________________

                    return temasApostilasOrdenados;
                }
            }
        }

        public int? GetNotaVideoAulaRevisao(int idVideo, int matricula)
        {
            int? nota = new int?();
            using (var ctx = new DesenvContext())
            {
                var result = (from n in ctx.tblLessonsEvaluationRevisaoAula
                              where n.intVideoId == idVideo
                              && n.intClientId == matricula
                              select n.intNota).ToList();
                if (result.Count() > 0) nota = result[0];
            }
            return nota;
        }

        public TemasApostila VideosRevalidaHardCode()
        {
            using(MiniProfiler.Current.Step("Vídeos revalida hardcode"))
            {
                AssuntoTemaApostila assuntoRevalida1 = new AssuntoTemaApostila { Id = 1, Descricao = "Especial Revalida 1" };
                AssuntoTemaApostila assuntoRevalida2 = new AssuntoTemaApostila { Id = 2, Descricao = "Especial Revalida 2" };
                AssuntoTemaApostila assuntoRevalida3 = new AssuntoTemaApostila { Id = 3, Descricao = "Especial Revalida 3" };
                AssuntoTemaApostila assuntoRevalida4 = new AssuntoTemaApostila { Id = 4, Descricao = "Especial Revalida 4" };

                //ESPECIAL 1
                //TEMA[1]_________________________________________________________________________________________#

                var temaRetorno = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 1, //tema.IdRevisaoIndice,
                    IdTema = 1, //tema.IdTema,
                    Descricao = "Especial Revalida 1 - Clínica Médica",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "CLM", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 32/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida1

                };
                // 1.1.1___________VIDEO - Reumatologia
                var KeyVideo = "ac2r7QlW.xml";
                var idVideo = 160275;
                var RevisaoVideoDescricao = "Reumatologia";
                var videoAdd = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo),
                    Descricao = RevisaoVideoDescricao,
                    Assistido = false,//LogVideos.Any(l => l == video.IdRevisaoVideo),
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("2837"),
                    IdRevisaoAula = idVideo,
                    Ordem = 1
                };
                // 1.1.2___________VIDEO - Pneumologia
                var KeyVideo1_2 = "JUsLoRrM.xml";
                var idVideo1_2 = 160276;
                var RevisaoVideoDescricao1_2 = "Pneumologia";
                var videoAdd1_2 = new VideoMednet()
                {
                    IdProfessor = 208618, //video.IdProfessor,
                    Cue = 1,//Convert.ToInt32(video.RevisaoVideoCue),
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo1_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo1_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo1_2),
                    Descricao = RevisaoVideoDescricao1_2,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("2294"),
                    IdRevisaoAula = idVideo1_2,//video.IdRevisaoVideo,
                    Ordem = 1
                };
                //____________PROF
                temaRetorno.Professores.Add(new Pessoa
                {
                    Nome = "BERNARDO ALBUQUERQUE",//video.ProfessorNome,
                    ID = 208618,//video.IdProfessor,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 90154)
                });

                temaRetorno.VideosRevisao.Add(videoAdd);
                temaRetorno.VideosRevisao.Add(videoAdd1_2);

                //TEMA[2]_________________________________________________________________________________________#
                var temaRetorno2 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 2, //tema.IdRevisaoIndice,
                    IdTema = 2, //tema.IdTema,
                    Descricao = "Especial Revalida 1 - Cirurgia",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "CIR", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 30/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida1
                };
                // 1.2.1___________VIDEO - Trauma
                var KeyVideo2 = "XBC64BKe.xml";
                var idVideo2 = 160284;
                var RevisaoVideoDescricao2 = "Trauma";
                var videoAdd2 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo2),
                    Descricao = RevisaoVideoDescricao2,
                    Assistido = false,//LogVideos.Any(l => l == video.IdRevisaoVideo),
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("5626"),
                    IdRevisaoAula = idVideo2,
                    Ordem = 1
                };
                // 1.2.2___________VIDEO - Queimadura
                var KeyVideo2_2 = "PpXbHoea.xml";
                var idVideo2_2 = 160289;
                var RevisaoVideoDescricao2_2 = "Queimadura";

                var videoAdd2_2 = new VideoMednet()
                {
                    IdProfessor = 76972, //video.IdProfessor,
                    Cue = 1,//Convert.ToInt32(video.RevisaoVideoCue),
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo2_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo2_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo2_2),
                    Descricao = RevisaoVideoDescricao2_2,
                    Assistido = false,//LogVideos.Any(l => l == video.IdRevisaoVideo),
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("2427"),
                    IdRevisaoAula = idVideo2_2,//video.IdRevisaoVideo,
                    Ordem = 1//video.IntOrdem ?? 0
                };
                //____________PROF
                temaRetorno2.Professores.Add(new Pessoa
                {
                    Nome = "MOZART NETTO",//video.ProfessorNome,
                    ID = 208618,//video.IdProfessor,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 76975)
                });
                temaRetorno2.Professores.Add(new Pessoa
                {
                    Nome = "RODRIGO MELLO",//video.ProfessorNome,
                    ID = 76972,//video.IdProfessor,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 121433)
                });

                temaRetorno2.VideosRevisao.Add(videoAdd2);
                temaRetorno2.VideosRevisao.Add(videoAdd2_2);

                //TEMA[3]_________________________________________________________________________________________#
                var temaRetorno3 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 3, //tema.IdRevisaoIndice,
                    IdTema = 3, //tema.IdTema,
                    Descricao = "Especial Revalida 1 - Pediatria",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "PED", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 124/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida1
                };
                // 1.3.1___________VIDEO - Neonatologia
                var KeyVideo3 = "g6jpveLu.xml";
                var idVideo3 = 160419;
                var RevisaoVideoDescricao3 = "Neonatologia";
                var videoAdd3 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo3.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo3.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo3),
                    Descricao = RevisaoVideoDescricao3,
                    Assistido = false,//LogVideos.Any(l => l == video.IdRevisaoVideo),
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("3566"),
                    IdRevisaoAula = idVideo3,
                    Ordem = 1
                };
                // 1.3.2___________VIDEO - Condições cirúrgicas no recém-nascido e lactente
                var KeyVideo3_2 = "Tzd0KYdQ.xml";
                var idVideo3_2 = 160420;
                var RevisaoVideoDescricao3_2 = "Condições cirúrgicas no recém-nascido e lactente";
                var videoAdd3_2 = new VideoMednet()
                {
                    IdProfessor = 208618, //video.IdProfessor,
                    Cue = 1,//Convert.ToInt32(video.RevisaoVideoCue),
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo3_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo3_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo3_2),
                    Descricao = RevisaoVideoDescricao3_2,
                    Assistido = false,//LogVideos.Any(l => l == video.IdRevisaoVideo),
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1152"),
                    IdRevisaoAula = idVideo3_2,//video.IdRevisaoVideo,
                    Ordem = 1//video.IntOrdem ?? 0
                };
                //____________PROF
                temaRetorno3.Professores.Add(new Pessoa
                {
                    Nome = "JULIA PAES",//video.ProfessorNome,
                    ID = 208618,//video.IdProfessor,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 126342/*video.IdProfessor*/)
                });

                temaRetorno3.VideosRevisao.Add(videoAdd3);
                temaRetorno3.VideosRevisao.Add(videoAdd3_2);

                //TEMA[4]_________________________________________________________________________________________#
                var temaRetorno4 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 4, //tema.IdRevisaoIndice,
                    IdTema = 4, //tema.IdTema,
                    Descricao = "Especial Revalida 1 - GO",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "GO", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 60/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida1
                };
                // 1.4.1___________VIDEO - Ginecologia Endócrina
                var KeyVideo4 = "vnHzue5R.xml";
                var idVideo4 = 160271;
                var RevisaoVideoDescricao4 = "Ginecologia Endócrina";

                var videoAdd4 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo4.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo4.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo4),
                    Descricao = RevisaoVideoDescricao4,
                    Assistido = false,//LogVideos.Any(l => l == video.IdRevisaoVideo),
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("2310"),
                    IdRevisaoAula = idVideo4,
                    Ordem = 1
                };
                // 1.4.2___________VIDEO - Planejamento Familiar
                var KeyVideo4_2 = "DOgH6Ez1.xml";
                var idVideo4_2 = 160273;
                var RevisaoVideoDescricao4_2 = "Planejamento Familiar";

                var videoAdd4_2 = new VideoMednet()
                {
                    IdProfessor = 208618, //video.IdProfessor,
                    Cue = 1,//Convert.ToInt32(video.RevisaoVideoCue),
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo4_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo4_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo4_2),
                    Descricao = RevisaoVideoDescricao4_2,
                    Assistido = false,//LogVideos.Any(l => l == video.IdRevisaoVideo),
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1380"),
                    IdRevisaoAula = idVideo4_2,//video.IdRevisaoVideo,
                    Ordem = 1//video.IntOrdem ?? 0
                };
                //____________PROF
                temaRetorno4.Professores.Add(new Pessoa
                {
                    Nome = "VINÍCIUS AYUPE",//video.ProfessorNome,
                    ID = 208618,//video.IdProfessor,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 43342/*video.IdProfessor*/)
                });

                temaRetorno4.VideosRevisao.Add(videoAdd4);
                temaRetorno4.VideosRevisao.Add(videoAdd4_2);

                //TEMA[5]_________________________________________________________________________________________#
                var temaRetorno5 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 5, //tema.IdRevisaoIndice,
                    IdTema = 5, //tema.IdTema,
                    Descricao = "Especial Revalida 1 - Preventiva",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "PRE", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 131/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida1
                };
                // 1.5.1___________VIDEO -  Declaração de Óbito
                var KeyVideo5 = "SX2lS9yU.xml";
                var idVideo5 = 160286;
                var RevisaoVideoDescricao5 = " Declaração de Óbito";

                var videoAdd5 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo5.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo5.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo5),
                    Descricao = RevisaoVideoDescricao5,
                    Assistido = false,//LogVideos.Any(l => l == video.IdRevisaoVideo),
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1185"),
                    IdRevisaoAula = idVideo5,
                    Ordem = 1
                };
                // 1.5.2___________VIDEO - Medidas de Saúde Coletiva
                var KeyVideo5_2 = "IxByxmQB.xml";
                var idVideo5_2 = 160285;
                var RevisaoVideoDescricao5_2 = "Medidas de Saúde Coletiva";

                var videoAdd5_2 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo5_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo5_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo5_2),
                    Descricao = RevisaoVideoDescricao5_2,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1449"),
                    IdRevisaoAula = idVideo5_2,//video.IdRevisaoVideo,
                    Ordem = 1
                };
                // 1.5.3___________VIDEO - Transição Demográfica e Edpidemiológica
                var KeyVideo5_3 = "91eqmi3X.xml";
                var idVideo5_3 = 160288;
                var RevisaoVideoDescricao5_3 = "Transição Demográfica e Edpidemiológica";

                var videoAdd5_3 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo5_3.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo5_3.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo5_3),
                    Descricao = RevisaoVideoDescricao5_3,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("844"),
                    IdRevisaoAula = idVideo5_3,//video.IdRevisaoVideo,
                    Ordem = 1
                };
                // 1.5.4___________VIDEO - Estudos Epidemiológicos
                var KeyVideo5_4 = "DFl1D8fT.xml";
                var idVideo5_4 = 160269;
                var RevisaoVideoDescricao5_4 = "Estudos Epidemiológicos";

                var videoAdd5_4 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo5_4.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo5_4.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo5_4),
                    Descricao = RevisaoVideoDescricao5_4,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1349"),
                    IdRevisaoAula = idVideo5_4,//video.IdRevisaoVideo,
                    Ordem = 1
                };
                //____________PROF
                temaRetorno5.Professores.Add(new Pessoa
                {
                    Nome = "MÁRCIO ROCHA",//video.ProfessorNome,
                    ID = 208618,//video.IdProfessor,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 76974/*video.IdProfessor*/)
                });

                temaRetorno5.VideosRevisao.Add(videoAdd5);
                temaRetorno5.VideosRevisao.Add(videoAdd5_2);
                temaRetorno5.VideosRevisao.Add(videoAdd5_3);
                temaRetorno5.VideosRevisao.Add(videoAdd5_4);
                //__________________________________________________________________________________________________________

                var temasApostilasOrdenados = new TemasApostila();
                temasApostilasOrdenados.Add(temaRetorno);
                temasApostilasOrdenados.Add(temaRetorno2);
                temasApostilasOrdenados.Add(temaRetorno3);
                temasApostilasOrdenados.Add(temaRetorno4);
                temasApostilasOrdenados.Add(temaRetorno5);

                //_______________________________________________________________________________________________________________
                //ESPECIAL REVALIDA 2
                //_______________________________________________________________________________________________________________
                //

                //TEMA[6]_______________________________________________________________________________________________________#
                var temaRetorno6 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 6,
                    IdTema = 6, //tema.IdTema,
                    Descricao = "Especial Revalida 2 - Clínica Médica",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "CLM", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 32/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida2
                };
                // 2.1.1___________VIDEO - Nefrologia
                var KeyVideo6_1 = "WRyXPzp0.xml";
                var idVideo6_1 = 162369;
                var RevisaoVideoDescricao6_1 = "Nefrologia";

                var videoAdd6_1 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo6_1.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo6_1.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo6_1),
                    Descricao = RevisaoVideoDescricao6_1,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1899"),
                    IdRevisaoAula = idVideo6_1,
                    Ordem = 1
                };
                // 2.1.2___________VIDEO - Neurologia
                var KeyVideo6_2 = "okB51WQD.xml";
                var idVideo6_2 = 162371;
                var RevisaoVideoDescricao6_2 = "Neurologia";

                var videoAdd6_2 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo6_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo6_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo6_2),
                    Descricao = RevisaoVideoDescricao6_2,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1964"),
                    IdRevisaoAula = idVideo6_2,//video.IdRevisaoVideo,
                    Ordem = 1
                };
                // 2.1.3___________VIDEO - Dermatologia
                var KeyVideo6_3 = "L8timj7W.xml";
                var idVideo6_3 = 162493;
                var RevisaoVideoDescricao6_3 = "Dermatologia";

                var videoAdd6_3 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo6_3.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo6_3.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo6_3),
                    Descricao = RevisaoVideoDescricao6_3,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1236"),
                    IdRevisaoAula = idVideo6_3,//video.IdRevisaoVideo,
                    Ordem = 1
                };
                //____________PROF
                temaRetorno6.Professores.Add(new Pessoa
                {
                    Nome = "RAPHAEL MONTEIRO",
                    ID = 208618,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 85951)
                });
                temaRetorno6.Assunto = assuntoRevalida2;
                temaRetorno6.VideosRevisao.Add(videoAdd6_1);
                temaRetorno6.VideosRevisao.Add(videoAdd6_2);
                temaRetorno6.VideosRevisao.Add(videoAdd6_3);

                //TEMA[7]_________________________________________________________________________________________#
                var temaRetorno7 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 7, //tema.IdRevisaoIndice,
                    IdTema = 7, //tema.IdTema,
                    Descricao = "Especial Revalida 2 - Cirurgia ",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "CIR", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 30/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida2
                };
                // 2.2.1___________VIDEO - Dor Abdominal
                var KeyVideo7_1 = "87dFQJqx.xml";
                var idVideo7_1 = 161637;
                var RevisaoVideoDescricao7_1 = "Dor Abdominal";

                var videoAdd7_1 = new VideoMednet()
                {
                    IdProfessor = 76972,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo7_1.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo7_1.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo7_1),
                    Descricao = RevisaoVideoDescricao7_1,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("3768"),
                    IdRevisaoAula = idVideo7_1,
                    Ordem = 1
                };
                // 2.2.2___________VIDEO - Obstrução Intestinal
                var KeyVideo7_2 = "wT4kSN5U.xml";
                var idVideo7_2 = 162524;
                var RevisaoVideoDescricao7_2 = "Obstrução Intestinal";

                var videoAdd7_2 = new VideoMednet()
                {
                    IdProfessor = 76972,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo7_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo7_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo7_2),
                    Descricao = RevisaoVideoDescricao7_2,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1236"),
                    IdRevisaoAula = idVideo7_2,//video.IdRevisaoVideo,
                    Ordem = 1
                };

                // 2.2.3___________VIDEO - Rastreios Oncológicos
                var KeyVideo7_3 = "EzYEsWei.xml";
                var idVideo7_3 = 162707;
                var RevisaoVideoDescricao7_3 = "Rastreios Oncológicos";

                var videoAdd7_3 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo7_3.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo7_3.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo7_3),
                    Descricao = RevisaoVideoDescricao7_3,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1311"),
                    IdRevisaoAula = idVideo7_3,
                    Ordem = 1
                };

                //____________PROF
                temaRetorno7.Professores.Add(new Pessoa
                {
                    Nome = "RODRIGO MELLO",//video.ProfessorNome,
                    ID = 76972,//video.IdProfessor,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 121433)
                });
                temaRetorno7.Professores.Add(new Pessoa
                {
                    Nome = "MOZART NETTO",//video.ProfessorNome,
                    ID = 208618,//video.IdProfessor,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 76975)
                });

                temaRetorno7.VideosRevisao.Add(videoAdd7_1);
                temaRetorno7.VideosRevisao.Add(videoAdd7_2);
                temaRetorno7.VideosRevisao.Add(videoAdd7_3);

                //TEMA[8]_________________________________________________________________________________________#
                var temaRetorno8 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 8, //tema.IdRevisaoIndice,
                    IdTema = 8, //tema.IdTema,
                    Descricao = "Especial Revalida 2 - Pediatria ",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "PED", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 124/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida2
                };
                // 2.3.1___________VIDEO -Infecções Respiratórias
                var KeyVideo8_1 = "I1SB1cZv.xml";
                var idVideo8_1 = 162927;
                var RevisaoVideoDescricao8_1 = "Infecções Respiratórias";

                var videoAdd8_1 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo8_1.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo8_1.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo8_1),
                    Descricao = RevisaoVideoDescricao8_1,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("2839"),
                    IdRevisaoAula = idVideo8_1,
                    Ordem = 1
                };
                // 2.3.2___________VIDEO - ITU
                var KeyVideo8_2 = "J2OaQMGT.xml";
                var idVideo8_2 = 162928;
                var RevisaoVideoDescricao8_2 = "ITU";

                var videoAdd8_2 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo8_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo8_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo8_2),
                    Descricao = RevisaoVideoDescricao8_2,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1540"),
                    IdRevisaoAula = idVideo8_2,//video.IdRevisaoVideo,
                    Ordem = 1//video.IntOrdem ?? 0
                };

                //____________PROF
                temaRetorno8.Professores.Add(new Pessoa
                {
                    Nome = "JULIA PAES",//video.ProfessorNome,
                    ID = 208618,//video.IdProfessor,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 126342/*video.IdProfessor*/)
                });
                temaRetorno8.Assunto = assuntoRevalida2;
                temaRetorno8.VideosRevisao.Add(videoAdd8_1);
                temaRetorno8.VideosRevisao.Add(videoAdd8_2);

                //TEMA[9]_________________________________________________________________________________________#
                var temaRetorno9 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 9, //tema.IdRevisaoIndice,
                    IdTema = 9, //tema.IdTema,
                    Descricao = "Especial Revalida 2 - GO",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "GO", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 60/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida2
                };
                // 2.4.1___________VIDEO - Sangramentos da primeira metade da gestação
                var KeyVideo9_1 = "YWqjiHsQ.xml";
                var idVideo9_1 = 161654;
                var RevisaoVideoDescricao9_1 = "Sangramentos da primeira metade da gestação";

                var videoAdd9_1 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo9_1.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo9_1.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo9_1),
                    Descricao = RevisaoVideoDescricao9_1,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1336"),
                    IdRevisaoAula = idVideo9_1,
                    Ordem = 1
                };
                // 1.4.2___________VIDEO - Sangramentos da segunda metade da gestação e doença hemolítica perinatal
                var KeyVideo9_2 = "xJQjHBDx.xml";
                var idVideo9_2 = 161656;
                var RevisaoVideoDescricao9_2 = "Sangramentos da segunda metade da gestação e doença hemolítica perinatal";

                var videoAdd9_2 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo9_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo9_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo9_2),
                    Descricao = RevisaoVideoDescricao9_2,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1082"),
                    IdRevisaoAula = idVideo9_2,//video.IdRevisaoVideo,
                    Ordem = 1//video.IntOrdem ?? 0
                };

                // 1.4.3___________VIDEO - Doenças clínicas na gestação (pré-eclâmpsia, diabetes gestacional, infecção urinária e profilaxia da transmissão vertical do HIV)
                var KeyVideo9_3 = "voOlsaRV.xml";
                var idVideo9_3 = 161658;
                var RevisaoVideoDescricao9_3 = "Doenças clínicas na gestação (pré-eclâmpsia, diabetes gestacional, infecção urinária e profilaxia da transmissão vertical do HIV)";

                var videoAdd9_3 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo9_3.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo9_3.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo9_3),
                    Descricao = RevisaoVideoDescricao9_3,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("2063"),
                    IdRevisaoAula = idVideo9_3,
                    Ordem = 1
                };
                //____________PROF
                temaRetorno9.Professores.Add(new Pessoa
                {
                    Nome = "VINÍCIUS AYUPE",//video.ProfessorNome,
                    ID = 208618,//video.IdProfessor,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 43342/*video.IdProfessor*/)
                });
                temaRetorno9.Assunto = assuntoRevalida2;
                temaRetorno9.VideosRevisao.Add(videoAdd9_1);
                temaRetorno9.VideosRevisao.Add(videoAdd9_2);
                temaRetorno9.VideosRevisao.Add(videoAdd9_3);

                //TEMA[10]_________________________________________________________________________________________#
                var temaRetorno10 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 10, //tema.IdRevisaoIndice,
                    IdTema = 10, //tema.IdTema,
                    Descricao = "Especial Revalida 2 - Preventiva",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "PRE", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 131/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida2
                };
                // 10.5.1___________VIDEO -  Prevenção de doenças
                var KeyVideo10_1 = "HmfFEWD9.xml";
                var idVideo10_1 = 163741;
                var RevisaoVideoDescricao10_1 = "Prevenção de doenças";

                var videoAdd10_1 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo10_1.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo10_1.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo10_1),
                    Descricao = RevisaoVideoDescricao10_1,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("613"),
                    IdRevisaoAula = idVideo10_1,
                    Ordem = 1
                };
                // 10.5.2___________VIDEO - Ética médica
                var KeyVideo10_2 = "TlqX0Vkc.xml";
                var idVideo10_2 = 163736;
                var RevisaoVideoDescricao10_2 = "Ética médica";

                var videoAdd10_2 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo10_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo10_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo10_2),
                    Descricao = RevisaoVideoDescricao10_2,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1029"),
                    IdRevisaoAula = idVideo10_2,
                    Ordem = 1
                };
                // 10.5.3___________VIDEO - Saúde do trabalhador
                var KeyVideo10_3 = "Zj1EF1fe.xml";
                var idVideo10_3 = 163738;
                var RevisaoVideoDescricao10_3 = "Saúde do trabalhador";
                var videoAdd10_3 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo10_3.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo10_3.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo10_3),
                    Descricao = RevisaoVideoDescricao10_3,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1544"),
                    IdRevisaoAula = idVideo10_3,//video.IdRevisaoVideo,
                    Ordem = 1//video.IntOrdem ?? 0
                };
                // 10.5.4___________VIDEO - Epidemiologia clínica
                var KeyVideo10_4 = "nPV37ts5.xml";
                var idVideo10_4 = 163737;
                var RevisaoVideoDescricao10_4 = "Epidemiologia clínica";

                var videoAdd10_4 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo10_4.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo10_4.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo10_4),
                    Descricao = RevisaoVideoDescricao10_4,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("782"),
                    IdRevisaoAula = idVideo10_4,
                    Ordem = 1
                };

                // 10.5.5___________VIDEO - Medidas de tendências central
                var KeyVideo10_5 = "Xj6nF1Li.xml";
                var idVideo10_5 = 163740;
                var RevisaoVideoDescricao10_5 = "Medidas de tendências central";

                var videoAdd10_5 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo10_5.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo10_5.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo10_5),
                    Descricao = RevisaoVideoDescricao10_5,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("537"),
                    IdRevisaoAula = idVideo10_5,
                    Ordem = 1
                };

                //____________PROF
                temaRetorno10.Professores.Add(new Pessoa
                {
                    Nome = "MÁRCIO ROCHA",//video.ProfessorNome,
                    ID = 208618,//video.IdProfessor,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 76974/*video.IdProfessor*/)
                });

                temaRetorno10.VideosRevisao.Add(videoAdd10_1);
                temaRetorno10.VideosRevisao.Add(videoAdd10_2);
                temaRetorno10.VideosRevisao.Add(videoAdd10_3);
                temaRetorno10.VideosRevisao.Add(videoAdd10_4);
                temaRetorno10.VideosRevisao.Add(videoAdd10_5);

                //Retorno
                temasApostilasOrdenados.Add(temaRetorno6);
                temasApostilasOrdenados.Add(temaRetorno7);
                temasApostilasOrdenados.Add(temaRetorno8);
                temasApostilasOrdenados.Add(temaRetorno9);
                temasApostilasOrdenados.Add(temaRetorno10);

                //_______________________________________________________________________________________________________________
                //ESPECIAL REVALIDA 3
                //_______________________________________________________________________________________________________________
                //

                //TEMA[11]_______________________________________________________________________________________________________#
                var temaRetorno11 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 11, //tema.IdRevisaoIndice,
                    IdTema = 11, //tema.IdTema,
                    Descricao = "Especial Revalida 3 - Clínica Médica",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "CLM", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 32/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida3
                };
                // 11.1.1___________VIDEO - ENDOCRINOLOGIA
                var KeyVideo11_1 = "VIzHc1yC.xml";
                var idVideo11_1 = 166847;
                var RevisaoVideoDescricao11_1 = "Endocrinologia";
                var videoAdd11_1 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo11_1.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo11_1.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo11_1),
                    Descricao = RevisaoVideoDescricao11_1,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1429"),
                    IdRevisaoAula = idVideo11_1,
                    Ordem = 1
                };
                // 11.1.2___________VIDEO - INFECTOLOGIA
                var KeyVideo11_2 = "BiGvNDHa.xml";
                var idVideo11_2 = 166849;  //162371
                var RevisaoVideoDescricao11_2 = "Infectologia";

                var videoAdd11_2 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo11_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo11_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo11_2),
                    Descricao = RevisaoVideoDescricao11_2,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("2445"),
                    IdRevisaoAula = idVideo11_2,//video.IdRevisaoVideo,
                    Ordem = 1//video.IntOrdem ?? 0
                };

                //____________PROF
                temaRetorno11.Professores.Add(new Pessoa
                {
                    Nome = "RAPHAEL MONTEIRO",
                    ID = 208618,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 85951)
                });
                temaRetorno11.Assunto = assuntoRevalida3;
                temaRetorno11.VideosRevisao.Add(videoAdd11_1);
                temaRetorno11.VideosRevisao.Add(videoAdd11_2);

                //TEMA[12]_________________________________________________________________________________________#

                var temaRetorno12 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 12, //tema.IdRevisaoIndice,
                    IdTema = 12, //tema.IdTema,
                    Descricao = "Especial Revalida 3 - Cirurgia ",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "CIR", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 30/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida3
                };
                // 12.2.1___________VIDEO - DISTÚRBIOS BENIGNOS DAS VIAS BILIARE
                var KeyVideo12_1 = "uW2f619N.xml";
                var idVideo12_1 = 166920;
                var RevisaoVideoDescricao12_1 = "Distúrbios Benignos das Vias Biliares";

                var videoAdd12_1 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo12_1.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo12_1.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo12_1),
                    Descricao = RevisaoVideoDescricao12_1,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1596"),
                    IdRevisaoAula = idVideo12_1,
                    Ordem = 1
                };
                // 12.2.2___________VIDEO - GASTROLOGIA E PROCEDIMENTOS AMBULATORIAIS
                var KeyVideo12_2 = "YQoqBW0v.xml";
                var idVideo12_2 = 166919;
                var RevisaoVideoDescricao12_2 = "Gastrologia e Procedimentos Ambulatoriais";
                var videoAdd12_2 = new VideoMednet()
                {
                    IdProfessor = 76972,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo12_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo12_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo12_2),
                    Descricao = RevisaoVideoDescricao12_2,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("3722"),
                    IdRevisaoAula = idVideo12_2,//video.IdRevisaoVideo,
                    Ordem = 1//video.IntOrdem ?? 0
                };

                //____________PROF
                temaRetorno12.Professores.Add(new Pessoa
                {
                    Nome = "MOZART NETTO",//video.ProfessorNome,
                    ID = 208618,//video.IdProfessor,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 76975)
                });

                temaRetorno12.Professores.Add(new Pessoa
                {
                    Nome = "RODRIGO MELLO",//video.ProfessorNome,
                    ID = 76972,//video.IdProfessor,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 121433)
                });

                temaRetorno12.Assunto = assuntoRevalida3;
                temaRetorno12.VideosRevisao.Add(videoAdd12_1);
                temaRetorno12.VideosRevisao.Add(videoAdd12_2);

                //_________________________________________________________________________________________________
                //TEMA[13]_________________________________________________________________________________________#
                var temaRetorno13 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 13, //tema.IdRevisaoIndice,
                    IdTema = 13, //tema.IdTema,
                    Descricao = "Especial Revalida 3 - Pediatria ",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "PED", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 124/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida3
                };
                // 13.3.1___________VIDEO -DIARREIA
                var KeyVideo13_1 = "JbRQQlgR.xml";
                var idVideo13_1 = 166855;
                var RevisaoVideoDescricao13_1 = "Diarreia";

                var videoAdd13_1 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo13_1.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo13_1.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo13_1),
                    Descricao = RevisaoVideoDescricao13_1,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("2396"),
                    IdRevisaoAula = idVideo13_1,
                    Ordem = 1
                };
                // 2.3.2___________VIDEO - EXANTEMA
                var KeyVideo13_2 = "WghsGmAc.xml";
                var idVideo13_2 = 166858;
                var RevisaoVideoDescricao13_2 = "Exantema";

                var videoAdd13_2 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo13_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo13_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo13_2),
                    Descricao = RevisaoVideoDescricao13_2,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("2278"),
                    IdRevisaoAula = idVideo13_2,//video.IdRevisaoVideo,
                    Ordem = 1//video.IntOrdem ?? 0
                };

                //____________PROF
                temaRetorno13.Professores.Add(new Pessoa
                {
                    Nome = "JULIA PAES",
                    ID = 208618,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 126342/*video.IdProfessor*/)
                });
                temaRetorno13.Assunto = assuntoRevalida3;
                temaRetorno13.VideosRevisao.Add(videoAdd13_1);
                temaRetorno13.VideosRevisao.Add(videoAdd13_2);

                //_________________________________________----14
                //TEMA[14]_________________________________________________________________________________________#
                var temaRetorno14 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 14, //tema.IdRevisaoIndice,
                    IdTema = 14, //tema.IdTema,
                    Descricao = "Especial Revalida 3 - GO",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "GO", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 60/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida3
                };
                // 1.4.1___________VIDEO - ÚLCERAS GENITAIS E VIOLÊNCIA SEXUAL    2
                var KeyVideo14 = "kUuDLSrs.xml";
                var idVideo14 = 167326;
                var RevisaoVideoDescricao14 = "Úlceras Genitais e Violência Sexual";

                var videoAdd14 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo14.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo14.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo14),
                    Descricao = RevisaoVideoDescricao14,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1458"),
                    IdRevisaoAula = idVideo14,
                    Ordem = 1
                };
                // 1.4.2___________VIDEO - Síndrome Corrimento Genital Inferior e Superior    1
                var KeyVideo14_2 = "hjHSQG6D.xml";
                var idVideo14_2 = 167323;
                var RevisaoVideoDescricao14_2 = "Síndrome Corrimento Genital Inferior e Superior";

                var videoAdd14_2 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo14_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo14_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo14_2),
                    Descricao = RevisaoVideoDescricao14_2,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("2613"),
                    IdRevisaoAula = idVideo14_2,//video.IdRevisaoVideo,
                    Ordem = 1//video.IntOrdem ?? 0
                };

                // 1.4.3___________VIDEO - Infecção por HPV e vacinação lesões precursoras
                var KeyVideo14_3 = "BSm5xult.xml";
                var idVideo14_3 = 167648;
                var RevisaoVideoDescricao14_3 = "Infecção por HPV e vacinação lesões precursoras e câncer de colo uterino e câncer de mama";

                var videoAdd14_3 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo14_3.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo14_3.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo14_3),
                    Descricao = RevisaoVideoDescricao14_3,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("1450"),
                    IdRevisaoAula = idVideo14_3,
                    Ordem = 1
                };

                //____________PROF
                temaRetorno14.Professores.Add(new Pessoa
                {
                    Nome = "KAREN PANISSET",
                    ID = 208618,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 85957/*video.IdProfessor*/)
                });

                temaRetorno14.VideosRevisao.Add(videoAdd14_2);
                temaRetorno14.VideosRevisao.Add(videoAdd14);
                temaRetorno14.VideosRevisao.Add(videoAdd14_3);

                ////15
                //TEMA[15]_________________________________________________________________________________________#
                var temaRetorno15 = new TemaApostila()
                {
                    Professores = new List<Pessoa>(),
                    Id = 15, //tema.IdRevisaoIndice,
                    IdTema = 15, //tema.IdTema,
                    Descricao = "Especial Revalida 3 - Preventiva",
                    VideosRevisao = new VideosMednet(),
                    Apostila = new Exercicio
                    {
                        ID = 7768, //tema.IdApostila,
                        Descricao = "PRE", //tema.CodeApostila,
                        Especialidade = new Especialidade { Id = 131/*GetClassificacao(idProduto, tema.IdEspecialidade)*/ }
                    },
                    Assunto = assuntoRevalida3
                };
                // 15.5.1___________VIDEO -   PRINCÍPIOS DO SUS
                var KeyVideo15_1 = "86sZTeZ2.xml";
                var idVideo15_1 = 167164;
                var RevisaoVideoDescricao15_1 = "Princípios do SUS";

                var videoAdd15_1 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo15_1.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo15_1.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo15_1),
                    Descricao = RevisaoVideoDescricao15_1,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("678"),
                    IdRevisaoAula = idVideo15_1,
                    Ordem = 1
                };
                // 15.5.2___________VIDEO -EVOLUÇÃO DO SUS
                var KeyVideo15_2 = "jVUbwmOe.xml";
                var idVideo15_2 = 167165;
                var RevisaoVideoDescricao15_2 = "Evolução do SUS";

                var videoAdd15_2 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo15_2.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo15_2.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo15_2),
                    Descricao = RevisaoVideoDescricao15_2,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("3157"),
                    IdRevisaoAula = idVideo15_2,
                    Ordem = 1
                };
                // 10.5.3___________VIDEO - DECRETO 7508
                var KeyVideo15_3 = "Nl5wWhmr.xml";
                var idVideo15_3 = 167166;
                var RevisaoVideoDescricao15_3 = "Decreto 7508";

                var videoAdd15_3 = new VideoMednet()
                {
                    IdProfessor = 208618,
                    Cue = 1,
                    Url = Criptografia.GetSignedPlayer(String.Concat(KeyVideo15_3.Replace(".xml", string.Empty), "-720")),
                    KeyVideo = KeyVideo15_3.Replace(".xml", string.Empty),
                    ID = Convert.ToInt32(idVideo15_3),
                    Descricao = RevisaoVideoDescricao15_3,
                    Assistido = false,
                    DuracaoFormatada = Utilidades.GetDuracaoFormatada("624"),
                    IdRevisaoAula = idVideo15_3,//video.IdRevisaoVideo,
                    Ordem = 1//video.IntOrdem ?? 0
                };

                //____________PROF
                temaRetorno15.Professores.Add(new Pessoa
                {
                    Nome = "MÁRCIO ROCHA",
                    ID = 208618,
                    UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", 76974/*video.IdProfessor*/)
                });

                temaRetorno15.VideosRevisao.Add(videoAdd15_1);
                temaRetorno15.VideosRevisao.Add(videoAdd15_2);
                temaRetorno15.VideosRevisao.Add(videoAdd15_3);
                //___________________________

                temasApostilasOrdenados.Add(temaRetorno11);
                temasApostilasOrdenados.Add(temaRetorno12);
                temasApostilasOrdenados.Add(temaRetorno13);
                temasApostilasOrdenados.Add(temaRetorno14);
                temasApostilasOrdenados.Add(temaRetorno15);
                //____________________________________________________________________________________________________________________

                return temasApostilasOrdenados;
            }
        }

         public List<int> GetLogVideos(int matricula, ETipoVideo tipoVideo)
        {
            using(MiniProfiler.Current.Step("Obtendo logs dos vídeos"))
            {
                var logsVideo = new List<int>();

                if (tipoVideo == ETipoVideo.Revisao)
                {
                    logsVideo = GetLogVideosRevisao(matricula);
                }
                else
                {
                    logsVideo = GetLogVideosTipo(matricula, tipoVideo);
                }

                return logsVideo;
            }
        }

        public List<int> GetLogVideosRevisao(int matricula)
        {
            using(MiniProfiler.Current.Step("Obtendo logs dos vídeos"))
            {
                var ctx = new DesenvContext();
                var logsVideo = new List<int>();
                var consultaLogs = ctx.tblRevisaoAulaVideoLog.Where(l => l.intClientId == matricula).Distinct();

                foreach (var video in consultaLogs)
                    logsVideo.Add(video.intRevisaoAulaId);

                return logsVideo;
            }
        }

         public List<int> GetLogVideosTipo(int matricula, ETipoVideo tipo)
        {
            using(MiniProfiler.Current.Step("Obtendo logs do tipo dos vídeos"))
            {
                var ctx = new DesenvContext();
                var logsVideo = new List<int>();
                var consultaLogs = ctx.tblVideoLog.Where(l => l.intClientId == matricula && l.intTipoVideo == (int)tipo).Distinct();

                foreach (var video in consultaLogs)
                    logsVideo.Add(video.intOrigemVideoId);

                return logsVideo;
            }
        }

        public ApostilaEntidade GetApostilaUnificada(int LessonTitleId)
        {
            using(MiniProfiler.Current.Step("Obtendo apostila unificada"))
            {
                if (LessonTitleId == 0)
                    return new ApostilaEntidade();

                using (var ctx = new DesenvContext())
                {

                    var anoLetivo = Utilidades.GetYear();


                    var name = (from lt in ctx.tblLessonTitles
                                join ri in ctx.tblRevisaoAulaIndice on lt.intLessonTitleID equals ri.intLessonTitleId
                                where ri.intLessonTitleId == LessonTitleId
                                select lt.txtLessonTitleName
                                ).FirstOrDefault();

                    if (name == null)
                        return new ApostilaEntidade();


                    var query = (from c in ctx.tblCourses
                                join l in ctx.tblLessons on c.intCourseID equals l.intCourseID
                                join lm in ctx.tblLesson_Material on l.intLessonID equals lm.intLessonID
                                join b in ctx.tblBooks on lm.intMaterialID equals b.intBookID
                                join be in ctx.tblBooks_Entities on b.intBookEntityID equals be.intID
                                join lt in ctx.tblLessonTitles on l.intLessonTitleID equals lt.intLessonTitleID
                                join p in ctx.tblProducts on b.intBookID equals p.intProductID
                                orderby b.intBookID descending
                                where c.intYear == anoLetivo
                                && lt.txtLessonTitleName == name
                                select new ApostilaEntidade { ID = be.intID, Nome = p.txtCode.Remove(0, 5) }
                                ).FirstOrDefault();

                    return query;

                }
            }

        }

        public bool HouveAulaPeloCronogramaNaRestrita(int idApostila, string nomeTema, int matricula, bool isAntesDataLimite, bool isMeioDeAno, int idEntidade, int anoLetivo, List<OrdemVenda> ovsAluno, bool temChamdoAntecipacaoDeMaterial, int idProduto, bool temOvAnosAtuais, List<int> anosConsideradosComoAtuais, List<KeyValuePair<int, int?>> lstCursoanoTrocaTemporaria)
        {
            using(MiniProfiler.Current.Step("Verificando se houve aula pelo cronograma na restrita"))
            {
                using (var ctx = new DesenvContext())
                {
                    ctx.ChangeTracker.AutoDetectChangesEnabled = false;
                    ctx.SetCommandTimeOut(180);

                    var nomeTemaAulaNoAnoAtual = ctx.tblLessonTitles.AsEnumerable().Where(x => string.Compare(x.txtLessonTitleName, nomeTema, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace) == 0 && anosConsideradosComoAtuais.Contains(x.intAno ?? 0)).FirstOrDefault();
                    var isAulaCronogramaAnosAnteriores = nomeTemaAulaNoAnoAtual == null;

                    if (isAulaCronogramaAnosAnteriores) return true;
                    var isApostilaMed = (int)Produto.Cursos.MED == idProduto;

                    if (!temOvAnosAtuais) return false;

                    var entidadesApostilasCpMed = new[] { 938, 736 };
                    var isApostilaCPMED = entidadesApostilasCpMed.Contains(idEntidade);
                    var ovsAposAnoLancamentoAulaRevisaoGrupoApostila = OrdemVendaEntity.GetOvsAposAnoLancamentoAulaRevisao(ovsAluno, idProduto, isApostilaMed, isApostilaCPMED, anosConsideradosComoAtuais);
                    var isAulaAssistida = false;

                    var temOvAposAnoLancamentoAulaRevisao = ovsAposAnoLancamentoAulaRevisaoGrupoApostila.Any();

                    if (temOvAposAnoLancamentoAulaRevisao)
                    {
                        isAulaAssistida = HouveAulaNoCronogramaDoAlunoRestrita(ovsAluno, nomeTemaAulaNoAnoAtual.txtLessonTitleName, matricula, isApostilaCPMED, anosConsideradosComoAtuais, temChamdoAntecipacaoDeMaterial, anoLetivo, lstCursoanoTrocaTemporaria);
                    }
                    else
                    {
                        isAulaAssistida = HouveAulaDoProdutoNaFilial(nomeTemaAulaNoAnoAtual.txtLessonTitleName, ovsAluno, idProduto, temChamdoAntecipacaoDeMaterial);
                    }
                    return isAulaAssistida;
                }
            }
        }

        private int GetClassificacao(int idProduto, int idEspecialidade)
        {
            using(MiniProfiler.Current.Step("Obtendo classificação - Mednet"))
            {
                List<int> listSubEspecialidades = new List<int>() { 16, 17, 18, 20, 34 };

                if (idProduto == 17)
                    if (idEspecialidade == 40)
                        return 11;

                if (idProduto == 16)
                    if (listSubEspecialidades.Contains(idEspecialidade))
                        return 34;

                return idEspecialidade;
            }
        }

        public bool HouveAulaNoCronogramaDoAlunoRestrita(List<OrdemVenda> ovsAluno, string nomeTemaAulaNoAnoAtual, int matricula, bool isApostilaCPMED, List<int> anos, bool temAntecipacaoDeMaterial, int anoLetivo, List<KeyValuePair<int, int?>> lstCursoanoTrocaTemporaria)
        {
            using(MiniProfiler.Current.Step("Verificando se houve aula no cronograma do Aluno na restrita"))
            {
                var isAulaAssistida = false;
                List<KeyValuePair<int, int?>> courseAndYear = new List<KeyValuePair<int, int?>>();

                foreach (var ov in ovsAluno)
                    courseAndYear.Add(new KeyValuePair<int, int?>(ov.IdProduto, ov.Year));
                courseAndYear.AddRange(lstCursoanoTrocaTemporaria);

                foreach (var item in courseAndYear)
                {
                    if (!isAulaAssistida)
                        isAulaAssistida = IsAulaAssistidaRestrita(item.Key, nomeTemaAulaNoAnoAtual, (int)item.Value, temAntecipacaoDeMaterial, anoLetivo);
                }

                var ovsCpmed = ovsAluno.Where(x => x.GroupID == (int)Produto.Produtos.CPMED && x.Year <= anoLetivo).ToList();
                var idTurmaConvidada = 0;

                if (isApostilaCPMED && ovsCpmed.Count() > 0)
                {
                    var anoMaxCpMed = ovsCpmed.Max(x => x.Year);
                    var turmaCpMed = ovsCpmed.Where(z => z.Year == anoMaxCpMed);
                    idTurmaConvidada = new TurmaEntity().TurmaConvidadaAluno(matricula, (int)anoMaxCpMed).ID;
                    if (!isAulaAssistida)
                        isAulaAssistida = IsAulaAssistida(idTurmaConvidada, nomeTemaAulaNoAnoAtual, (int)anoMaxCpMed, temAntecipacaoDeMaterial);
                }

                return isAulaAssistida;
            }
        }

       public bool IsAulaAssistidaRestrita(int idTurma, string nomeAulaAnoLetivo, int ano, bool temMaterialAntecipado, int anoAtual)
        {
            using (var ctx = new DesenvContext())
            {
                var isAulaAssistida = (from lt in ctx.tblLessonTitles
                                        join mc in ctx.mview_Cronograma on new { intCourseID = idTurma, lt.intLessonTitleID } equals new { intCourseID = mc.intCourseID, intLessonTitleID = mc.intLessonTitleID }
                                        where (nomeAulaAnoLetivo.Equals(lt.txtLessonTitleName)
                                        && lt.intAno == (ano) && mc.intCourseID == idTurma) 
                                        && ((mc.dteDateTime < DateTime.Now) || temMaterialAntecipado)
                                        select mc.intLessonID).Any();
                return isAulaAssistida;
            }
        }

       public bool IsAulaAssistida(int idTurma, string nomeAulaAnoLetivo, int ano, bool temMaterialAntecipado)
        {
            using (var ctx = new DesenvContext())
            {
                var isTurmaAnosAnteriores = ano < Utilidades.GetYear();
                var isAulaAssistida =(from lt in ctx.tblLessonTitles
                                        join mc in ctx.mview_Cronograma on new { intCourseID = idTurma, lt.intLessonTitleID } equals new { intCourseID = mc.intCourseID, intLessonTitleID = mc.intLessonTitleID }
                                        where (nomeAulaAnoLetivo.Equals(lt.txtLessonTitleName)
                                        && lt.intAno == (ano) && mc.intCourseID == idTurma) 
                                        && ((mc.dteDateTime < DateTime.Now) || temMaterialAntecipado)
                                        select mc.intLessonID).Any();
                return isAulaAssistida;
            }
        }

        public bool HouveAulaDoProdutoNaFilial(string nomeTemaAulaNoAnoAtual, List<OrdemVenda> ovsAluno, int groupApostila, bool temAntecipacaoDeMaterial)
        {
            using(MiniProfiler.Current.Step("Verificando se houve aula do produto na filial"))
            {
                var anoLetivo = Utilidades.GetYear();
                int filial;
                filial = ovsAluno.FirstOrDefault().IdFilial;
                bool isAulaAssistida = HouveAulaNaFilial(nomeTemaAulaNoAnoAtual, filial, anoLetivo, temAntecipacaoDeMaterial);
                return isAulaAssistida;
            }
        }

         public bool HouveAulaNaFilial(string nomeTema, int filial, int ano, bool temMaterialAntecipado = false)
        {
            using(MiniProfiler.Current.Step("Verificando se houve aula na filial"))
            {
                var ctx = new DesenvContext();
                var consulta = (from lt in ctx.tblLessonTitles
                                join mc in ctx.mview_Cronograma on new { filial, lt.intLessonTitleID } equals new { filial = mc.intStoreID ?? 0, intLessonTitleID = mc.intLessonTitleID }
                                where (nomeTema.Equals(lt.txtLessonTitleName) && lt.intAno == ano) && ((mc.dteDateTime < DateTime.Now) || temMaterialAntecipado)
                                select mc.intLessonID).Any();
                return consulta;
            }
        }
         public bool HouveAulaNasFiliais(string nomeTema, List<int> filial, int ano, bool temMaterialAntecipado = false)
        {
            var ctx = new DesenvContext();
            var consulta = (from lt in ctx.tblLessonTitles
                            join mc in ctx.mview_Cronograma on lt.intLessonTitleID  equals  mc.intLessonTitleID 
                            where filial.Contains(mc.intStoreID ?? 0) && (nomeTema.Equals(lt.txtLessonTitleName) && lt.intAno == ano) && ((mc.dteDateTime < DateTime.Now) || temMaterialAntecipado)
                            select mc.intLessonID).Any();
            return consulta;
        }

        public TemasApostila GetTemasVideoResumo(int idProduto, int matricula, int intProfessor, int intAula, bool isAdmin, int idTema = 0)
        {
            //Return Incluído enquanto funcionalidade não está no ar
            return new TemasApostila();
        }

        public bool HouveAulaPeloCronograma(int idApostila, string nomeTema, int matricula, bool isAntesDataLimite, bool isMeioDeAno, int idEntidade)
        {
            using(MiniProfiler.Current.Step("Verificando se houve aula pelo cronograma"))
            {
                using (var ctx = new DesenvContext())
                {
                    ctx.ChangeTracker.AutoDetectChangesEnabled = false;
                    ctx.SetCommandTimeOut(180);


                    var anoLetivo = Utilidades.GetYear();
                    var anosConsideradosComoAtuais = isAntesDataLimite ? new[] { anoLetivo, anoLetivo - 1 }.ToList() : new[] { anoLetivo }.ToList();

                    var nomeTemaAulaNoAnoAtual = ctx.tblLessonTitles.Where(x => string.Compare(x.txtLessonTitleName, nomeTema, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace) == 0 && anosConsideradosComoAtuais.Contains(x.intAno ?? 0)).FirstOrDefault();
                    var isAulaCronogramaAnosAnteriores = nomeTemaAulaNoAnoAtual == null;

                    if (isAulaCronogramaAnosAnteriores) return true;

                    var produtoApostila = ctx.tblProducts.Where(x => x.intProductID == idApostila).FirstOrDefault().intProductGroup2 ?? 0;
                    var isApostilaMed = (int)Produto.Cursos.MED == produtoApostila;

                    var ovsAluno = OrdemVendaEntity.GetOvsAluno(matricula);
                    var temOvAnosAtuais = OrdemVendaEntity.TemOvsAnoAtual(ovsAluno, anosConsideradosComoAtuais, isApostilaMed, isMeioDeAno);

                    if (!temOvAnosAtuais) return false;

                    var entidadesApostilasCpMed = new[] { 938, 736 };
                    var isApostilaCPMED = entidadesApostilasCpMed.Contains(idEntidade);

                    var ovsAposAnoLancamentoAulaRevisaoGrupoApostila = OrdemVendaEntity.GetOvsAposAnoLancamentoAulaRevisao(ovsAluno, produtoApostila, isApostilaMed, isApostilaCPMED, anosConsideradosComoAtuais);

                    var isAulaAssistida = false;

                    var temChamdoAntecipacaoDeMaterial = new ChamadoCallCenterEntity().VerificarSeClienteTemChamadoAntecipacaoMaterial(matricula);

                    var temOvAposAnoLancamentoAulaRevisao = ovsAposAnoLancamentoAulaRevisaoGrupoApostila.Any();

                    if (temOvAposAnoLancamentoAulaRevisao)
                    {
                        isAulaAssistida = HouveAulaNoCronogramaDoAluno(ovsAluno, nomeTemaAulaNoAnoAtual.txtLessonTitleName, matricula, isApostilaCPMED, anosConsideradosComoAtuais, temChamdoAntecipacaoDeMaterial);
                    }
                    else
                    {
                        isAulaAssistida = HouveAulaDoProdutoNaFilial(nomeTemaAulaNoAnoAtual.txtLessonTitleName, ovsAluno, produtoApostila, temChamdoAntecipacaoDeMaterial);
                    }
                    return isAulaAssistida;
                }
            }
        }

        public bool HouveAulaNoCronogramaDoAluno(List<OrdemVenda> ovsAluno, string nomeTemaAulaNoAnoAtual, int matricula, bool isApostilaCPMED, List<int> anos, bool temAntecipacaoDeMaterial)
        {
            using(MiniProfiler.Current.Step("Verificando se houve aula no cronograma do aluno"))
            {
                var anoLetivo = Utilidades.GetYear();
                var isAulaAssistida = false;
                List<KeyValuePair<int, int?>> courseAndYear = new List<KeyValuePair<int, int?>>();

                foreach (var ov in ovsAluno)
                    courseAndYear.Add(new KeyValuePair<int, int?>(ov.IdProduto, ov.Year));

                var chamadosTrocaTemporaria = ChamadoCallCenterEntity.GetChamadosDeTrocaTemporaria(matricula, anos);
                courseAndYear.AddRange(chamadosTrocaTemporaria);

                foreach (var item in courseAndYear)
                {
                    if (!isAulaAssistida)
                        isAulaAssistida = IsAulaAssistida(item.Key, nomeTemaAulaNoAnoAtual, (int)item.Value, temAntecipacaoDeMaterial);
                }

                var ovsCpmed = ovsAluno.Where(x => x.GroupID == (int)Produto.Produtos.CPMED && x.Year <= anoLetivo).ToList();
                var idTurmaConvidada = 0;

                if (isApostilaCPMED && ovsCpmed.Count() > 0)
                {
                    var anoMaxCpMed = ovsCpmed.Max(x => x.Year);
                    var turmaCpMed = ovsCpmed.Where(z => z.Year == anoMaxCpMed);
                    idTurmaConvidada = new TurmaEntity().TurmaConvidadaAluno(matricula, (int)anoMaxCpMed).ID;
                    if (!isAulaAssistida)
                        isAulaAssistida = IsAulaAssistida(idTurmaConvidada, nomeTemaAulaNoAnoAtual, (int)anoMaxCpMed, temAntecipacaoDeMaterial);
                }

                return isAulaAssistida;
            }
        }

        public TemasApostila GetTemasVideoAdaptaMed(int idProduto, int matricula, int intProfessor, int intAula, bool isAdmin, int idTema = 0)
        {
            using(MiniProfiler.Current.Step("Obtendo temas do vídeo AdaptaMed"))
            {
                using (var ctx = new AcademicoContext())
                {
                    using (var ctxMatDir = new DesenvContext())
                    {
                        var temasRetorno = new TemasApostila();

                        LogVideos = GetLogVideos(matricula, ETipoVideo.AdaptaMed);

                        var temaVideo = (from ri in ctxMatDir.tblAdaptaMedAulaIndice
                                        join rv in ctxMatDir.tblAdaptaMedAulaVideo on ri.intAdaptaMedIndiceId equals rv.intAdaptaMedIndiceId
                                        join p in ctxMatDir.tblProducts on ri.intBookId equals p.intProductID
                                        join b in ctxMatDir.tblBooks on p.intProductID equals b.intBookID
                                        join a in ctxMatDir.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on p.intProductGroup3 equals a.intProductGroupID
                                        join lt in ctxMatDir.tblLessonTitles on ri.intLessonTitleId equals lt.intLessonTitleID
                                        join prof in ctxMatDir.tblPersons on rv.intProfessorId equals prof.intContactID
                                        //join v in ctx.tblVideos on rv.intVideoId equals v.intVideoID
                                        where p.intProductGroup2 == (int)Produto.Cursos.ADAPTAMED && rv.intVideoId != 0
                                        && (idTema == 0 || (idTema != 0 && lt.intLessonTitleID == idTema))
                                        && (intProfessor == 0 || (intProfessor != 0 && rv.intProfessorId == intProfessor))
                                        && (intAula == 0 || (intAula != 0 && ri.intAdaptaMedIndiceId == intAula))
                                        orderby ri.intOrdem, rv.intOrdem
                                        select new
                                        {
                                            CodeApostila = p.txtCode.Remove(0, 5),
                                            IdEspecialidade = (a.intTipoDeClassificacao == 3 ? 11 : a.intClassificacaoID),
                                            TemaDescricao = lt.txtLessonTitleName,
                                            IdAdaptaMedIndice = ri.intAdaptaMedIndiceId,
                                            IdApostila = ri.intBookId,
                                            IdTema = ri.intLessonTitleId,
                                            IdAdaptaMedVideo = rv.intAdaptaMedVideoId,
                                            AdaptaMedVideoDescricao = rv.txtDescricao,
                                            IdVideo = rv.intVideoId,
                                            Entidade = b.intBookEntityID,
                                            IdProfessor = rv.intProfessorId,
                                            ProfessorNome = prof.txtName.Trim(),
                                            //KeyVideo = v.txtFileName,
                                            //Duracao = v.intDuracao,
                                            IntOrdem = rv.intOrdem,
                                            IntOrdemRi = ri.intOrdem
                                        }).ToList();

                        List<int> videoIds = temaVideo.Select(x => x.IdVideo).ToList();

                        var consultaVideo = (from v in ctx.tblVideo
                                            where videoIds.Contains(v.intVideoID)
                                            select new
                                            {
                                                intVideoID = v.intVideoID,
                                                KeyVideo = v.txtFileName,
                                                Duracao = v.intDuracao
                                            }).ToList();

                        var consulta = (from t in temaVideo
                                        join v in consultaVideo on t.IdVideo equals v.intVideoID
                                        orderby t.IntOrdemRi, t.IntOrdem
                                        select new
                                        {
                                            t.CodeApostila,
                                            t.IdEspecialidade,
                                            t.TemaDescricao,
                                            t.IdAdaptaMedIndice,
                                            t.IdApostila,
                                            t.IdTema,
                                            t.IdAdaptaMedVideo,
                                            t.AdaptaMedVideoDescricao,
                                            t.IdVideo,
                                            t.Entidade,
                                            t.IdProfessor,
                                            t.ProfessorNome,
                                            v.KeyVideo,
                                            v.Duracao,
                                            t.IntOrdem
                                        }).ToList();

                        var consultaTemas = (from c in consulta
                                            select new
                                            {
                                                c.CodeApostila,
                                                c.IdEspecialidade,
                                                c.TemaDescricao,
                                                c.Entidade,
                                                c.IdTema,
                                                c.IdApostila,
                                                c.IdAdaptaMedIndice
                                            }).Distinct().ToList();

                        var permitidos = new List<long>();
                        var isEmployee = ctxMatDir.tblEmployees.Where(e => e.intEmployeeID == matricula).ToList().Any();
                        var isMeioDeAno = ctxMatDir.tblAlunosAnoAtualMaisAnterior.Where(x => x.intClientID == matricula).ToList().Any();
                        var anoVigente = Utilidades.GetYear();
                        var isAntesDataLimite = Utilidades.IsAntesDatalimite(anoVigente - 1);
                        var anoAtual = isAntesDataLimite ? anoVigente - 1 : anoVigente;

                        if (isAdmin || isEmployee)
                            permitidos = ctxMatDir.tblBooks
                                                    .Where(b => b.intYear >= anoAtual && b.intBookEntityID != null)
                                                    .Select(b => b.intBookEntityID.Value).ToList();
                        else
                            permitidos = (from p in ctxMatDir.msp_Medsoft_SelectPermissaoExercicios(false, false, matricula)
                                        join b in ctxMatDir.tblBooks on p.intExercicioID equals b.intBookEntityID
                                        where b.intBookEntityID != null && p.intExercicioTipo == 3
                                        select b.intBookEntityID.Value).Distinct().ToList();

                        var consultaPermitida = (from c in consultaTemas
                                                join tp in permitidos on c.Entidade equals tp
                                                where isEmployee || HouveAulaPeloCronograma(c.IdApostila, c.TemaDescricao, matricula, isAntesDataLimite, isMeioDeAno, (int)c.Entidade)
                                                select new
                                                {
                                                    c.CodeApostila,
                                                    c.IdEspecialidade,
                                                    c.TemaDescricao,
                                                    c.Entidade,
                                                    c.IdTema,
                                                    c.IdApostila,
                                                    c.IdAdaptaMedIndice
                                                }).Distinct().ToList();

                        var aprovacoesAcademico = ctxMatDir.tblAdaptaMedAulaVideoAprovacao
                                                        .Where(r => r.intAdaptaMedAulaVideoTipoAprovadorId == (int)AulaRevisaoVideoAprovacao.TipoAprovadorEnum.Academico
                                                                    && r.bitAprovado).ToList();

                        foreach (var tema in consultaPermitida)
                        {
                            var temaRetorno = new TemaApostila()
                            {
                                Professores = new List<Pessoa>(),
                                Id = tema.IdAdaptaMedIndice,
                                IdTema = tema.IdTema,
                                Descricao = string.Concat(tema.CodeApostila, " - ", tema.TemaDescricao),
                                VideosAdaptaMed = new VideosMednet(),
                                VideosRevisao = new VideosMednet(),
                                Apostila = new Exercicio
                                {
                                    ID = tema.IdApostila,
                                    Descricao = tema.CodeApostila,
                                    Especialidade = new Especialidade { Id = GetClassificacao(idProduto, tema.IdEspecialidade) }
                                }
                            };

                            var videos = consulta.Where(tm => tm.IdAdaptaMedIndice == tema.IdAdaptaMedIndice).ToList();

                            foreach (var video in videos)
                            {
                                if (!isAdmin && !aprovacoesAcademico.Any(a => a.intAdaptaMedAulaVideoId == video.IdAdaptaMedVideo && a.intVideoId == video.IdVideo)) continue;

                                var videoAdd = new VideoMednet()
                                {
                                    IdProfessor = video.IdProfessor,
                                    Url = Criptografia.GetSignedPlayer(String.Concat(video.KeyVideo.Replace(".xml", string.Empty), "-720")),
                                    KeyVideo = video.KeyVideo.Replace(".xml", string.Empty),
                                    ID = Convert.ToInt32(video.IdVideo),
                                    Descricao = video.AdaptaMedVideoDescricao,
                                    Assistido = LogVideos.Any(l => l == video.IdAdaptaMedVideo),
                                    DuracaoFormatada = Utilidades.GetDuracaoFormatada(video.Duracao),
                                    IdAdaptaMedAula = video.IdAdaptaMedVideo,
                                    IdRevisaoAula = video.IdAdaptaMedVideo,
                                    Ordem = video.IntOrdem ?? 0,
                                    Thumb = VideoEntity.GetUrlThumb(video.KeyVideo.Replace(".xml", string.Empty), Utilidades.VideoThumbSize.width_320),
                                    TipoVideo = ETipoVideo.AdaptaMed

                                };

                                if (!temaRetorno.Professores.Any(p => p.ID == video.IdProfessor))
                                    temaRetorno.Professores.Add(new Pessoa
                                    {
                                        Nome = video.ProfessorNome,
                                        ID = video.IdProfessor,
                                        UrlAvatar = string.Format("http://arearestrita.medgrupo.com.br/_static/images/professores/{0}.jpg", video.IdProfessor)
                                    });

                                temaRetorno.VideosAdaptaMed.Add(videoAdd);
                                temaRetorno.VideosRevisao.Add(videoAdd);
                            }

                            if (temaRetorno.VideosAdaptaMed.Count() > 0)
                                temasRetorno.Add(temaRetorno);
                        }

                        return temasRetorno;
                    }
                }
            }
        }

        public TemaApostila GetVideoAulas(int idProduto, int idTema, int idApostila, int matricula, int idAplicacao, string versaoApp = "")
        {
            try
            {
                var videoTema = new TemaApostila();
                videoTema.VideoAulas = new List<VideoAula>();

                if (idAplicacao == (int)Aplicacoes.MsProMobile || idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON)
                {
                    idProduto = idProduto == (int)Produto.Cursos.CPMED ? (int)Produto.Cursos.MED : idProduto;
                }


                //Get 3 tipos de video aulas

                var revisao = GetTemasVideos(idProduto, matricula, 0, 0, false, idTema, tipoVideo: ETipoVideo.Revisao, idAplicacao: idAplicacao, versaoApp: versaoApp).FirstOrDefault();
                var resumo = GetTemasVideos(idProduto, matricula, 0, 0, false, idTema, tipoVideo: ETipoVideo.Resumo, idAplicacao: idAplicacao, versaoApp: versaoApp).FirstOrDefault();
                var bonus = new TemaApostila();

                if (revisao != null)
                {
                    videoTema.Apostila = revisao.Apostila;
                    videoTema.Descricao = revisao.Descricao;
                    videoTema.Id = revisao.Id;

                    //videoTema.VideosRevisao[0].

                    bonus = GetTemasVideos(idProduto, matricula, 0, 0, false, 0, revisao.Apostila.EntidadeApostilaID, ETipoVideo.Revisao, idAplicacao: idAplicacao, versaoApp: versaoApp).FirstOrDefault();

                    //Get Log Progresso e Visualização

                    var ctx = new DesenvContext();

                    var idsVideosRevisao = revisao.VideosRevisao.Select(v => v.IdRevisaoAula).ToList();
                    var totalVisualizacoesVideos = ctx.tblRevisaoAulaVideoLog.Where(l => idsVideosRevisao.Contains(l.intRevisaoAulaId)).ToList();
                    var todosProgressosVideos = ctx.tblRevisaoAulaVideoLogPosition.Where(m => m.intClientId == matricula).ToList();

                    ctx.Dispose();

                    foreach (var professor in revisao.Professores)
                    {
                        var segundosTotalVideos = 0;
                        var segundosTotalVideosAssistidos = 0;
                        var videosProf = revisao.VideosRevisao.Where(v => v.IdProfessor == professor.ID).ToList();

                        foreach (var video in videosProf)
                        {
                            video.Visualizacoes = totalVisualizacoesVideos
                                .Count(t => t.intRevisaoAulaId == video.IdRevisaoAula);

                            segundosTotalVideos += Convert.ToInt32(TimeSpan.Parse(video.DuracaoFormatada).TotalSeconds);

                            if (todosProgressosVideos.Any(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula))
                                segundosTotalVideosAssistidos += todosProgressosVideos.FirstOrDefault(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula).intSecond;

                            var duracaoVideo = Convert.ToInt32(TimeSpan.Parse(video.DuracaoFormatada).TotalSeconds);

                            if (todosProgressosVideos.Any(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula) && duracaoVideo > 0)
                            {
                                var videoProgresso = todosProgressosVideos.FirstOrDefault(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula);

                                video.Progresso = (videoProgresso.intSecond * 100) / duracaoVideo;
                                video.UltimaPosicaoAluno = videoProgresso.intLastSecondViewed;
                            }
                        }
                        if (segundosTotalVideos != 0)
                            professor.PercentVisualizado = (segundosTotalVideosAssistidos * 100) / segundosTotalVideos;
                        else
                            professor.PercentVisualizado = 0;

                        SetProgressoAulaRevisao(idTema, professor.ID, matricula, professor.PercentVisualizado);
                    }

                    var aula = new VideoAula();
                    aula.TipoAula = ETipoVideo.Revisao;
                    aula.Videos = new VideosMednet();
                    aula.Videos.AddRange(revisao.VideosRevisao);
                    aula.Professores = new List<Pessoa>();
                    aula.Professores.AddRange(revisao.Professores);
                    videoTema.VideoAulas.Add(aula);
                }
                if (resumo != null)
                {
                    if (videoTema.Apostila == null)
                    {
                        videoTema.Apostila = resumo.Apostila;
                        videoTema.Descricao = resumo.Descricao;
                        videoTema.Id = resumo.Id;
                    }

                    var ctx = new DesenvContext();

                    var idsVideosResumo = resumo.VideosResumo.Select(v => v.IdResumoAula).ToList();
                    var totalVisualizacoesVideos = ctx.tblVideoLog.Where(l => idsVideosResumo.Contains(l.intOrigemVideoId)).ToList();
                    var todosProgressosVideos = ctx.tblResumoAulaVideoLogPosition.Where(m => m.intClientId == matricula).ToList();

                    ctx.Dispose();

                    foreach (var professor in resumo.Professores)
                    {
                        var segundosTotalVideos = 0;
                        var segundosTotalVideosAssistidos = 0;
                        var videosProf = resumo.VideosResumo.Where(v => v.IdProfessor == professor.ID).ToList();

                        foreach (var video in videosProf)
                        {
                            video.Visualizacoes = totalVisualizacoesVideos
                                .Count(t => t.intOrigemVideoId == video.IdResumoAula);

                            segundosTotalVideos += Convert.ToInt32(TimeSpan.Parse(video.DuracaoFormatada).TotalSeconds);

                            if (todosProgressosVideos.Any(v => v.intResumoAulaVideoId == video.IdResumoAula))
                                segundosTotalVideosAssistidos += todosProgressosVideos.FirstOrDefault(v => v.intResumoAulaVideoId == video.IdResumoAula).intSecond;

                            var duracaoVideo = Convert.ToInt32(TimeSpan.Parse(video.DuracaoFormatada).TotalSeconds);

                            if (todosProgressosVideos.Any(v => v.intResumoAulaVideoId == video.IdResumoAula) && duracaoVideo > 0)
                                video.Progresso = (todosProgressosVideos.FirstOrDefault(v => v.intResumoAulaVideoId == video.IdResumoAula).intSecond * 100) / duracaoVideo;
                        }
                        if (segundosTotalVideos != 0)
                            professor.PercentVisualizado = (segundosTotalVideosAssistidos * 100) / segundosTotalVideos;
                        else
                            professor.PercentVisualizado = 0;

                        SetProgressoAulaResumo(idTema, professor.ID, matricula, professor.PercentVisualizado);
                    }

                    var aula = new VideoAula();
                    aula.TipoAula = ETipoVideo.Resumo;
                    aula.Videos = new VideosMednet();
                    aula.Videos.AddRange(resumo.VideosResumo);
                    aula.Professores = new List<Pessoa>();
                    aula.Professores.AddRange(resumo.Professores);
                    videoTema.VideoAulas.Add(aula);
                    videoTema.IdResumo = resumo.Id;
                }
                if (bonus != null && bonus.Id != default(int))
                {
                    var ctx = new DesenvContext();

                    var idsVideosRevisao = bonus.VideosRevisao.Select(v => v.IdRevisaoAula).ToList();
                    var totalVisualizacoesVideos = ctx.tblRevisaoAulaVideoLog.Where(l => idsVideosRevisao.Contains(l.intRevisaoAulaId)).ToList();
                    var todosProgressosVideos = ctx.tblRevisaoAulaVideoLogPosition.Where(m => m.intClientId == matricula).ToList();

                    ctx.Dispose();

                    foreach (var professor in bonus.Professores)
                    {
                        var segundosTotalVideos = 0;
                        var segundosTotalVideosAssistidos = 0;
                        var videosProf = bonus.VideosRevisao.Where(v => v.IdProfessor == professor.ID).ToList();

                        foreach (var video in videosProf)
                        {
                            video.Visualizacoes = totalVisualizacoesVideos
                                .Count(t => t.intRevisaoAulaId == video.IdRevisaoAula);

                            segundosTotalVideos += Convert.ToInt32(TimeSpan.Parse(video.DuracaoFormatada).TotalSeconds);

                            if (todosProgressosVideos.Any(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula))
                                segundosTotalVideosAssistidos += todosProgressosVideos.FirstOrDefault(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula).intSecond;

                            var duracaoVideo = Convert.ToInt32(TimeSpan.Parse(video.DuracaoFormatada).TotalSeconds);

                            if (todosProgressosVideos.Any(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula) && duracaoVideo > 0)
                                video.Progresso = (todosProgressosVideos.FirstOrDefault(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula).intSecond * 100) / duracaoVideo;
                        }
                        if (segundosTotalVideos != 0)
                            professor.PercentVisualizado = (segundosTotalVideosAssistidos * 100) / segundosTotalVideos;
                        else
                            professor.PercentVisualizado = 0;

                        SetProgressoAulaRevisao(idTema, professor.ID, matricula, professor.PercentVisualizado);
                    }

                    var aula = new VideoAula();
                    aula.TipoAula = ETipoVideo.Bonus;
                    aula.Videos = new VideosMednet();
                    aula.Videos.AddRange(bonus.VideosRevisao);
                    aula.Professores = new List<Pessoa>();
                    aula.Professores.AddRange(bonus.Professores);
                    videoTema.VideoAulas.Add(aula);
                }

                return videoTema;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SetProgressoAulaResumo(int idTema, int idProfessor, int idCliente, int percentualVisualizado)
        {
            using(MiniProfiler.Current.Step("Marcando progresso aula resumo"))
            {
                using (var ctx = new DesenvContext())
                {
                    var progressoAtual = ctx.tblResumoAulaTemaProfessorAssistido.FirstOrDefault(p => p.intClientID == idCliente
                                                                                            && p.intLessonTitleID == idTema
                                                                                            && p.intProfessorID == idProfessor);

                    if (progressoAtual == null)
                    {
                        ctx.tblResumoAulaTemaProfessorAssistido.Add(new tblResumoAulaTemaProfessorAssistido()
                        {
                            intClientID = idCliente,
                            intLessonTitleID = idTema,
                            intPercentVisualizado = percentualVisualizado,
                            intProfessorID = idProfessor
                        });
                    }
                    else
                    {
                        progressoAtual.intPercentVisualizado = percentualVisualizado;
                    }
                    ctx.SaveChanges();

                    return 1;
                }
            }
        }

        public AvaliacoesProfessor GetAvaliacoesTemasVideosRevisao(int idTema, int idProfessor)
        {
            using(MiniProfiler.Current.Step("Obtendo avaliações temas vídeos revisão"))
            {
                var ctx = new DesenvContext();
                var avaliacoes = new AvaliacoesProfessor();
                var consulta = (from le in ctx.tblLessonsEvaluationRevisaoAula
                                join ri in ctx.tblRevisaoAulaIndice on le.intRevisaoAulaIndiceId equals ri.intRevisaoAulaIndiceId
                                join p in ctx.tblPersons on le.intEmployeeId equals p.intContactID
                                where ri.intLessonTitleId == idTema && (idProfessor == 0 || le.intEmployeeId == idProfessor)
                                select new { nota = le.intNota, NomeProfessor = p.txtName.Trim(), IdProfessor = p.intContactID }).ToList();

                var consultaProfAux = (from a in ctx.tblLessonsTotalEvaluationAuxiliar
                                    join lt in ctx.tblLessonTitles on a.intLessonTitleId equals lt.intLessonTitleID
                                    join p in ctx.tblPersons on a.intEmployeeId equals p.intContactID
                                    where lt.intLessonTitleID == idTema
                                    select new
                                    {
                                        nota1 = a.intNota1,
                                        nota2 = a.intNota2,
                                        nota3 = a.intNota3,
                                        nota4 = a.intNota4,
                                        NomeProfessor = p.txtName.Trim(),
                                        IdProfessor = p.intContactID
                                    }).ToList();

                var professores = consulta.GroupBy(p => new { p.IdProfessor, p.NomeProfessor });

                // Nota1 ótimo, nota2 bom, nota3 ruim, nota4 péssimo
                foreach (var prof in professores)
                {
                    var avaliacao = new AvaliacaoProfessor();
                    // Tbl Auxiliar de notas
                    if (consultaProfAux.Any(p => p.IdProfessor == prof.Key.IdProfessor))
                    {
                        var consultaProfessor = consultaProfAux.Where(c => c.IdProfessor == prof.Key.IdProfessor);

                        avaliacao.Nota1 = Convert.ToInt32(consultaProfessor.First().nota1);
                        avaliacao.Nota2 = Convert.ToInt32(consultaProfessor.First().nota2);
                        avaliacao.Nota3 = Convert.ToInt32(consultaProfessor.First().nota3);
                        avaliacao.Nota4 = Convert.ToInt32(consultaProfessor.First().nota4);
                        avaliacao.TotalNotas = avaliacao.Nota1 + avaliacao.Nota2 + avaliacao.Nota3 + avaliacao.Nota4;
                        avaliacao.NotaFinal = GetTotal(new List<int> { avaliacao.Nota1, avaliacao.Nota2, avaliacao.Nota3, avaliacao.Nota4 });
                        avaliacao.Professor = new Pessoa { Nome = prof.Key.NomeProfessor, ID = prof.Key.IdProfessor };

                        //avaliacoes.Add(avaliacao);
                    }
                    else
                    {
                        var consultaProfessor = consulta.Where(p => p.NomeProfessor == prof.Key.NomeProfessor);

                        //var avaliacao = new AvaliacaoProfessor();
                        avaliacao.Nota1 = consultaProfessor.Where(n => n.nota == 1).Count();
                        avaliacao.Nota2 = consultaProfessor.Where(n => n.nota == 2).Count();
                        avaliacao.Nota3 = consultaProfessor.Where(n => n.nota == 3).Count();
                        avaliacao.Nota4 = consultaProfessor.Where(n => n.nota == 4).Count();
                        avaliacao.TotalNotas = consultaProfessor.Count();
                        avaliacao.NotaFinal = GetTotal(new List<int> { avaliacao.Nota1, avaliacao.Nota2, avaliacao.Nota3, avaliacao.Nota4 });
                        avaliacao.Professor = new Pessoa { Nome = prof.Key.NomeProfessor, ID = prof.Key.IdProfessor };

                    }

                    if (avaliacao.Nota4 > 0)
                    {
                        avaliacao.Nota2 = avaliacao.Nota2 + avaliacao.Nota4;
                        avaliacao.Nota3 = avaliacao.Nota3 + avaliacao.Nota4;
                        avaliacao.TotalNotas = avaliacao.Nota1 + avaliacao.Nota2 + avaliacao.Nota3 + avaliacao.Nota4;
                        avaliacao.NotaFinal = GetTotal(new List<int> { avaliacao.Nota1, avaliacao.Nota2, avaliacao.Nota3, avaliacao.Nota4 });
                    }

                    /*if (avaliacao.NotaFinal < 75)
                    {
                        var N = 75.0; //constantes nota base
                        var M = 4.0;  //constante relação ótimo e bom
                        double o = (double)(65.0 * (double)avaliacao.Nota3 - N * (double)avaliacao.Nota3 - N * (double)avaliacao.Nota4) / (N + N * M - 100.0 - 85.0 * M);
                        double b = M *o;

                        avaliacao.Nota1 = (int)Math.Round(o, 0, MidpointRounding.AwayFromZero);
                        avaliacao.Nota2 = (int)Math.Round(b, 0, MidpointRounding.AwayFromZero);
                        avaliacao.TotalNotas = avaliacao.Nota1 + avaliacao.Nota2 + avaliacao.Nota3 + avaliacao.Nota4;
                        avaliacao.NotaFinal = GetTotal(new List<int> { avaliacao.Nota1, avaliacao.Nota2, avaliacao.Nota3, avaliacao.Nota4 });
                    }*/

                    avaliacoes.Add(avaliacao);
                }

                return avaliacoes;
            }
        }

        private int GetTotal(List<int> Notas)
        {
            using(MiniProfiler.Current.Step("Obtendo total - MednetEntity"))
            {
                int total = 0, soma = 0, k;
                List<int> Pesos = new List<int>();
                Pesos.Add(100); Pesos.Add(85); Pesos.Add(65); Pesos.Add(0);

                for (k = 0; k < 4; k++)
                {
                    total += Pesos[k] * Notas[k];
                    soma += Notas[k];
                }
                if (soma > 0)
                    return total / soma;

                return 0;
            }
        }

        public int SetAvaliacaoTemaVideoRevisao(AulaAvaliacaoPost avaliacao)
        {
            using(MiniProfiler.Current.Step("Salvando avaliação tema vídeo revisão"))
            {
                var ctx = new DesenvContext();
                //DefinirNotaCorretaAvaliacaoMedSoft(avaliacao);
                ctx.tblLessonsEvaluationRevisaoAula.Add(new tblLessonsEvaluationRevisaoAula
                {
                    dteEvaluation = DateTime.Now,
                    intApplicationId = avaliacao.ApplicationID,
                    intClientId = avaliacao.AlunoID,
                    intEmployeeId = avaliacao.ProfessorID,
                    intRevisaoAulaIndiceId = avaliacao.IdRevisaoIndice,
                    intNota = avaliacao.Nota,
                    txtObservacao = avaliacao.Observacao
                });
                ctx.SaveChanges();
                return 1;
            }
        }

        public int GetPermissaoAvaliacao(int matricula, int temaId, int professorId = 0)
        {
            using(MiniProfiler.Current.Step("Obtendo permissão para avaliação"))
            {
                var ctx = new DesenvContext();
                var result = (from le in ctx.tblLessonsEvaluationRevisaoAula
                            join ri in ctx.tblRevisaoAulaIndice on le.intRevisaoAulaIndiceId equals ri.intRevisaoAulaIndiceId
                            where ri.intLessonTitleId == temaId && le.intClientId == matricula && (professorId == 0 || le.intEmployeeId == professorId)
                            select le).Any();
                return result ? 0 : 1;
            }
        }

         public int SetLogAssitido(VideoMednet log)
        {
            try
            {
                using(MiniProfiler.Current.Step("Salvando log assistido"))
                {
                    using (var ctx = new DesenvContext())
                    {
                        ctx.tblRevisaoAulaVideoLog.Add(new tblRevisaoAulaVideoLog
                        {
                            intClientId = log.Matricula,
                            intRevisaoAulaId = log.IdRevisaoAula,
                            dteCadastro = DateTime.Now
                        });

                        ctx.SaveChanges();
                        return 1;
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public VideoUrlDTO GetVideoInfo(int idVideo, int idAplicacao = (int)Aplicacoes.MsProMobile, string versaoApp = "")
        {
            using(MiniProfiler.Current.Step("Obtendo informações do vídeo"))
            {
                return new VideoEntity().GetVideoPorVideoID(idVideo, _chaveamentoVimeoAulaRevisao, idAplicacao, versaoApp);
            }
        }

        public int GetUltimaPosicaoVideoAulaRevisaoAluno(int clientId, int videoId)
        {
            using(MiniProfiler.Current.Step("Obtendo última posição video aula revisão aluno"))
            {
                using (var ctx = new DesenvContext())
                {
                    var posicao = (from log in ctx.tblRevisaoAulaVideoLogPosition
                                join v in ctx.tblRevisaoAulaVideo
                                on log.intRevisaoAulaVideoId equals v.intRevisaoAulaId
                                where v.intVideoId == videoId
                                && log.intClientId == clientId
                                select log.intLastSecondViewed
                                        ).FirstOrDefault();

                    return posicao;
                }
            }
        }

        public int SetProgressoVideo(ProgressoVideo progresso)
        {
            try
            {
                using(MiniProfiler.Current.Step("Inserindo Progresso vídeo"))
                {
                    using (var ctx = new DesenvContext())
                    {

                        var posicao =  ctx.tblRevisaoAulaVideoLogPosition.FirstOrDefault(p => p.intClientId == progresso.Matricula && p.intRevisaoAulaVideoId == progresso.IdRevisaoAula);

                        if (posicao == null)
                        {
                            ctx.tblRevisaoAulaVideoLogPosition.Add(new tblRevisaoAulaVideoLogPosition
                            {
                                intClientId = progresso.Matricula,
                                dteLastUpdate = DateTime.Now,
                                intRevisaoAulaVideoId = progresso.IdRevisaoAula,
                                intSecond = progresso.ProgressoSegundo
                            });
                        }
                        else
                        {
                            if (progresso.ProgressoSegundo > posicao.intSecond) posicao.intSecond = progresso.ProgressoSegundo;
                            posicao.intLastSecondViewed = progresso.ProgressoSegundo;
                            posicao.dteLastUpdate = DateTime.Now;
                        }

                        ctx.SaveChanges();
                        return 1;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public decimal GetVideoDuracaoPorIdRevisaoAula(int IdRevisaoAula)
        {
            using(MiniProfiler.Current.Step("Obtendo duração do vídeo por idrevisaoaula"))
            {
                using (var ctx = new AcademicoContext())
                {
                    using (var ctxMatDir = new DesenvContext())
                    {
                        List<int?> videoIds = (from a in ctxMatDir.tblRevisaoAulaVideo
                                            where a.intRevisaoAulaId == IdRevisaoAula
                                            select a.intVideoId).ToList();

                        var video = (from v in ctx.tblVideo
                                    where videoIds.Contains(v.intVideoID)
                                    select new
                                    {
                                        IdVideo = v.intVideoID,
                                        duracao = v.intDuracao
                                    }).ToList().FirstOrDefault();

                        decimal duracao = 0;
                        if (video != null)
                        {
                            decimal.TryParse(video.duracao, NumberStyles.Any, CultureInfo.CreateSpecificCulture("en-us"), out duracao);
                        }
                        return duracao;
                    }
                }
            }
        }

        public int SetProgressoAulaRevalida(int idTema, int idProfessor, int idCliente, int percentualVisualizado)
        {
            using (var ctx = new DesenvContext())
            {
                var progressoAtual = ctx.tblRevalidaAulaTemaProfessorAssistido.FirstOrDefault(p => p.intClientID == idCliente
                                                                                           && p.intLessonTitleID == idTema
                                                                                           && p.intProfessorID == idProfessor);

                if (progressoAtual == null)
                {
                    ctx.tblRevalidaAulaTemaProfessorAssistido.Add(new tblRevalidaAulaTemaProfessorAssistido()
                    {
                        intClientID = idCliente,
                        intLessonTitleID = idTema,
                        intPercentVisualizado = percentualVisualizado,
                        intProfessorID = idProfessor
                    });
                }
                else
                {
                    progressoAtual.intPercentVisualizado = percentualVisualizado;
                }
                ctx.SaveChanges();

                return 1;
            }
        }

        public AvaliacoesProfessor GetAvaliacoesTemasVideosRevalida(int idTema, int idProfessor)
        {
            var ctx = new DesenvContext();
            var avaliacoes = new AvaliacoesProfessor();

            var consulta = (from le in ctx.tblLessonEvaluationVideoAula
                            join ri in ctx.tblRevalidaAulaIndice on le.intAulaIndiceId equals ri.intRevalidaAulaIndiceId
                            join p in ctx.tblPersons on le.intEmployeeId equals p.intContactID
                            where ri.intLessonTitleRevalidaId == idTema && (idProfessor == 0 || le.intEmployeeId == idProfessor)
                            select new { nota = le.intNota, NomeProfessor = p.txtName.Trim(), IdProfessor = p.intContactID }).ToList();

            var consultaProfAux = (from a in ctx.tblLessonsTotalEvaluationAuxiliar
                                   join lt in ctx.tblLessonTitles on a.intLessonTitleId equals lt.intLessonTitleID
                                   join p in ctx.tblPersons on a.intEmployeeId equals p.intContactID
                                   where lt.intLessonTitleID == idTema
                                   select new
                                   {
                                       nota1 = a.intNota1,
                                       nota2 = a.intNota2,
                                       nota3 = a.intNota3,
                                       nota4 = a.intNota4,
                                       NomeProfessor = p.txtName.Trim(),
                                       IdProfessor = p.intContactID
                                   }).ToList();

            var professores = consulta.GroupBy(p => new { p.IdProfessor, p.NomeProfessor });

            // Nota1 ótimo, nota2 bom, nota3 ruim, nota4 péssimo
            foreach (var prof in professores)
            {
                var avaliacao = new AvaliacaoProfessor();
                // Tbl Auxiliar de notas
                if (consultaProfAux.Any(p => p.IdProfessor == prof.Key.IdProfessor))
                {
                    var consultaProfessor = consultaProfAux.Where(c => c.IdProfessor == prof.Key.IdProfessor);

                    avaliacao.Nota1 = Convert.ToInt32(consultaProfessor.First().nota1);
                    avaliacao.Nota2 = Convert.ToInt32(consultaProfessor.First().nota2);
                    avaliacao.Nota3 = Convert.ToInt32(consultaProfessor.First().nota3);
                    avaliacao.Nota4 = Convert.ToInt32(consultaProfessor.First().nota4);
                    avaliacao.TotalNotas = avaliacao.Nota1 + avaliacao.Nota2 + avaliacao.Nota3 + avaliacao.Nota4;
                    avaliacao.NotaFinal = GetTotal(new List<int> { avaliacao.Nota1, avaliacao.Nota2, avaliacao.Nota3, avaliacao.Nota4 });
                    avaliacao.Professor = new Pessoa { Nome = prof.Key.NomeProfessor, ID = prof.Key.IdProfessor };

                    //avaliacoes.Add(avaliacao);
                }
                else
                {
                    var consultaProfessor = consulta.Where(p => p.NomeProfessor == prof.Key.NomeProfessor);

                    //var avaliacao = new AvaliacaoProfessor();
                    avaliacao.Nota1 = consultaProfessor.Where(n => n.nota == 1).Count();
                    avaliacao.Nota2 = consultaProfessor.Where(n => n.nota == 2).Count();
                    avaliacao.Nota3 = consultaProfessor.Where(n => n.nota == 3).Count();
                    avaliacao.Nota4 = consultaProfessor.Where(n => n.nota == 4).Count();
                    avaliacao.TotalNotas = consultaProfessor.Count();
                    avaliacao.NotaFinal = GetTotal(new List<int> { avaliacao.Nota1, avaliacao.Nota2, avaliacao.Nota3, avaliacao.Nota4 });
                    avaliacao.Professor = new Pessoa { Nome = prof.Key.NomeProfessor, ID = prof.Key.IdProfessor };
                }

                if (avaliacao.Nota4 > 0)
                {
                    avaliacao.Nota2 = avaliacao.Nota2 + avaliacao.Nota4;
                    avaliacao.Nota3 = avaliacao.Nota3 + avaliacao.Nota4;
                    avaliacao.TotalNotas = avaliacao.Nota1 + avaliacao.Nota2 + avaliacao.Nota3 + avaliacao.Nota4;
                    avaliacao.NotaFinal = GetTotal(new List<int> { avaliacao.Nota1, avaliacao.Nota2, avaliacao.Nota3, avaliacao.Nota4 });
                }

                /*if (avaliacao.NotaFinal < 75)
				{
					var N = 75.0; //constantes nota base
					var M = 4.0;  //constante relação ótimo e bom
					double o = (double)(65.0 * (double)avaliacao.Nota3 - N * (double)avaliacao.Nota3 - N * (double)avaliacao.Nota4) / (N + N * M - 100.0 - 85.0 * M);
					double b = M *o;

					avaliacao.Nota1 = (int)Math.Round(o, 0, MidpointRounding.AwayFromZero);
					avaliacao.Nota2 = (int)Math.Round(b, 0, MidpointRounding.AwayFromZero);
					avaliacao.TotalNotas = avaliacao.Nota1 + avaliacao.Nota2 + avaliacao.Nota3 + avaliacao.Nota4;
					avaliacao.NotaFinal = GetTotal(new List<int> { avaliacao.Nota1, avaliacao.Nota2, avaliacao.Nota3, avaliacao.Nota4 });
				}*/

                avaliacoes.Add(avaliacao);
            }

            return avaliacoes;
        }


        public AvaliacoesProfessor GetAvaliacoesTemasVideosResumo(int idTema, int idProfessor)
        {
            using(MiniProfiler.Current.Step("Obter Avaliações Temas Videos Resumo"))
            {
                var ctx = new DesenvContext();
                var avaliacoes = new AvaliacoesProfessor();

                var consulta = (from le in ctx.tblLessonEvaluationVideoAula
                                join ri in ctx.tblResumoAulaIndice on le.intAulaIndiceId equals ri.intResumoAulaIndiceId
                                join p in ctx.tblPersons on le.intEmployeeId equals p.intContactID
                                where ri.intLessonTitleId == idTema && (idProfessor == 0 || le.intEmployeeId == idProfessor)
                                select new { nota = le.intNota, NomeProfessor = p.txtName.Trim(), IdProfessor = p.intContactID }).ToList();

                var consultaProfAux = (from a in ctx.tblLessonsTotalEvaluationAuxiliar
                                    join lt in ctx.tblLessonTitles on a.intLessonTitleId equals lt.intLessonTitleID
                                    join p in ctx.tblPersons on a.intEmployeeId equals p.intContactID
                                    where lt.intLessonTitleID == idTema
                                    select new
                                    {
                                        nota1 = a.intNota1,
                                        nota2 = a.intNota2,
                                        nota3 = a.intNota3,
                                        nota4 = a.intNota4,
                                        NomeProfessor = p.txtName.Trim(),
                                        IdProfessor = p.intContactID
                                    }).ToList();

                var professores = consulta.GroupBy(p => new { p.IdProfessor, p.NomeProfessor });

                // Nota1 ótimo, nota2 bom, nota3 ruim, nota4 péssimo
                foreach (var prof in professores)
                {
                    var avaliacao = new AvaliacaoProfessor();
                    // Tbl Auxiliar de notas
                    if (consultaProfAux.Any(p => p.IdProfessor == prof.Key.IdProfessor))
                    {
                        var consultaProfessor = consultaProfAux.Where(c => c.IdProfessor == prof.Key.IdProfessor);

                        avaliacao.Nota1 = Convert.ToInt32(consultaProfessor.First().nota1);
                        avaliacao.Nota2 = Convert.ToInt32(consultaProfessor.First().nota2);
                        avaliacao.Nota3 = Convert.ToInt32(consultaProfessor.First().nota3);
                        avaliacao.Nota4 = Convert.ToInt32(consultaProfessor.First().nota4);
                        avaliacao.TotalNotas = avaliacao.Nota1 + avaliacao.Nota2 + avaliacao.Nota3 + avaliacao.Nota4;
                        avaliacao.NotaFinal = GetTotal(new List<int> { avaliacao.Nota1, avaliacao.Nota2, avaliacao.Nota3, avaliacao.Nota4 });
                        avaliacao.Professor = new Pessoa { Nome = prof.Key.NomeProfessor, ID = prof.Key.IdProfessor };

                        //avaliacoes.Add(avaliacao);
                    }
                    else
                    {
                        var consultaProfessor = consulta.Where(p => p.NomeProfessor == prof.Key.NomeProfessor);

                        //var avaliacao = new AvaliacaoProfessor();
                        avaliacao.Nota1 = consultaProfessor.Where(n => n.nota == 1).Count();
                        avaliacao.Nota2 = consultaProfessor.Where(n => n.nota == 2).Count();
                        avaliacao.Nota3 = consultaProfessor.Where(n => n.nota == 3).Count();
                        avaliacao.Nota4 = consultaProfessor.Where(n => n.nota == 4).Count();
                        avaliacao.TotalNotas = consultaProfessor.Count();
                        avaliacao.NotaFinal = GetTotal(new List<int> { avaliacao.Nota1, avaliacao.Nota2, avaliacao.Nota3, avaliacao.Nota4 });
                        avaliacao.Professor = new Pessoa { Nome = prof.Key.NomeProfessor, ID = prof.Key.IdProfessor };
                    }

                    if (avaliacao.Nota4 > 0)
                    {
                        avaliacao.Nota2 = avaliacao.Nota2 + avaliacao.Nota4;
                        avaliacao.Nota3 = avaliacao.Nota3 + avaliacao.Nota4;
                        avaliacao.TotalNotas = avaliacao.Nota1 + avaliacao.Nota2 + avaliacao.Nota3 + avaliacao.Nota4;
                        avaliacao.NotaFinal = GetTotal(new List<int> { avaliacao.Nota1, avaliacao.Nota2, avaliacao.Nota3, avaliacao.Nota4 });
                    }

                    /*if (avaliacao.NotaFinal < 75)
                    {
                        var N = 75.0; //constantes nota base
                        var M = 4.0;  //constante relação ótimo e bom
                        double o = (double)(65.0 * (double)avaliacao.Nota3 - N * (double)avaliacao.Nota3 - N * (double)avaliacao.Nota4) / (N + N * M - 100.0 - 85.0 * M);
                        double b = M *o;

                        avaliacao.Nota1 = (int)Math.Round(o, 0, MidpointRounding.AwayFromZero);
                        avaliacao.Nota2 = (int)Math.Round(b, 0, MidpointRounding.AwayFromZero);
                        avaliacao.TotalNotas = avaliacao.Nota1 + avaliacao.Nota2 + avaliacao.Nota3 + avaliacao.Nota4;
                        avaliacao.NotaFinal = GetTotal(new List<int> { avaliacao.Nota1, avaliacao.Nota2, avaliacao.Nota3, avaliacao.Nota4 });
                    }*/

                    avaliacoes.Add(avaliacao);
                }

                return avaliacoes;
            }
        }

        public int SetAvaliacaoTemaVideo(AulaAvaliacaoPost avaliacao)
        {
            using(MiniProfiler.Current.Step("Inserir Avaliação Tema Vídeo"))
            {
                using(MiniProfiler.Current.Step("Obter Avaliação Tema Vídeo"))
                {
                    var ctx = new DesenvContext();
                    ctx.tblLessonEvaluationVideoAula.Add(new tblLessonEvaluationVideoAula
                    {
                        dteEvaluation = DateTime.Now,
                        intApplicationId = avaliacao.ApplicationID,
                        intClientId = avaliacao.AlunoID,
                        intEmployeeId = avaliacao.ProfessorID,
                        intAulaIndiceId = avaliacao.IdAulaIndice,
                        intNota = avaliacao.Nota,
                        txtObservacao = avaliacao.Observacao,
                        intTipoVideo = avaliacao.TipoVideo
                    });
                    ctx.SaveChanges();
                    return 1;
                }
            }
        }

        public int GetPermissaoAvaliacaoVideoAulaRevalida(int matricula, int temaId, int professorId, int tipoVideo)
        {
            var ctx = new DesenvContext();
            var result = (from le in ctx.tblLessonEvaluationVideoAula
                          join ri in ctx.tblRevalidaAulaIndice on le.intAulaIndiceId equals ri.intRevalidaAulaIndiceId
                          where le.intTipoVideo == tipoVideo && ri.intLessonTitleRevalidaId == temaId && le.intClientId == matricula && le.intEmployeeId == professorId
                          select le).Any();
            return result ? 0 : 1;
        }

        public List<int> GetTemasAulaEspecial(int idTemaAulaRevisao, int matricula, int idProduto, int ano)
        {

            if (idProduto == (int)Produto.Cursos.MEDCURSO)
                idProduto = (int)Produto.Produtos.MEDCURSO;
            if (idProduto == (int)Produto.Cursos.MED)
                idProduto = (int)Produto.Produtos.MED;

            int MaterialEspecialLessonSubjectID = 200;

            using (var ctx = new DesenvContext())
            {
                var lessonTitleFiltro = (from lt in ctx.tblLessonTitles
                                         where lt.intLessonTitleID == idTemaAulaRevisao
                                         select new
                                         {
                                           nomeLessonTitle = lt.txtLessonTitleName
                                         }).FirstOrDefault();

                var nomeTemaOriginal = lessonTitleFiltro == null ?  "" : lessonTitleFiltro.nomeLessonTitle;

                var turma = (from so in ctx.tblSellOrders
                             join sd in ctx.tblSellOrderDetails on so.intOrderID equals sd.intOrderID
                             join p in ctx.tblProducts on sd.intProductID equals p.intProductID
                             join cu in ctx.tblCourses on p.intProductID equals cu.intCourseID
                             where so.intClientID == matricula
                                   && p.intProductGroup1 == idProduto
                                   && cu.intYear == ano
                                   && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                             select p.intProductID
                            ).FirstOrDefault();


                var resultado = (from lt in ctx.tblLessonTitles
                                 join le in ctx.tblLessons on lt.intLessonTitleID equals le.intLessonTitleID
                                 join cu in ctx.tblCourses on le.intCourseID equals cu.intCourseID
                                 where lt.intLessonSubjectID == MaterialEspecialLessonSubjectID
                                       && cu.intCourseID == turma
                                 && lt.txtLessonTitleName.Trim().Replace("AULA ESPECIAL ", "") == nomeTemaOriginal
                                 select lt.intLessonTitleID
                                 ).Distinct().ToList();

                return resultado;

            }
        }

        public int GetPermissaoAvaliacaoVideoAula(int matricula, int temaId, int professorId, int tipoVideo)
        {
            var ctx = new DesenvContext();
            var result = (from le in ctx.tblLessonEvaluationVideoAula
                          join ri in ctx.tblResumoAulaIndice on le.intAulaIndiceId equals ri.intResumoAulaIndiceId
                          where le.intTipoVideo == tipoVideo && ri.intLessonTitleId == temaId && le.intClientId == matricula && (professorId == 0 || le.intEmployeeId == professorId)
                          select le).Any();
            return result ? 0 : 1;
        }

        public int SetProgressoVideoRevalida(ProgressoVideo progresso)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var posicao = ctx.tblRevalidaAulaVideoLogPosition.FirstOrDefault(p => p.intClientId == progresso.Matricula && p.intRevalidaAulaVideoId == progresso.IdRevalidaAula);

                    if (posicao == null)
                    {
                        ctx.tblRevalidaAulaVideoLogPosition.Add(new tblRevalidaAulaVideoLogPosition
                        {
                            intClientId = progresso.Matricula,
                            dteLastUpdate = DateTime.Now,
                            intRevalidaAulaVideoId = progresso.IdRevalidaAula,
                            intSecond = progresso.ProgressoSegundo
                        });
                    }
                    else
                        if (progresso.ProgressoSegundo > posicao.intSecond)
                    {
                        posicao.dteLastUpdate = DateTime.Now;
                        posicao.intSecond = progresso.ProgressoSegundo;
                    }

                    ctx.SaveChanges();
                    return 1;
                }
            }
            catch
            {
                throw;
            }
        }

        public AvaliacaoProfessor GetAvaliacaoRealizadaRevalida(int matricula, int idRevalidaAulaIndice, int idAplicacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var avaliacao = new AvaliacaoProfessor();
                    var ultimaAvaliacao = ctx.tblLessonEvaluationVideoAula.Where(e =>
                        e.intClientId == matricula
                        && e.intTipoVideo == (int)ETipoVideo.Revalida
                        && e.intAulaIndiceId == idRevalidaAulaIndice).ToList().FirstOrDefault();

                    if (ultimaAvaliacao != null)
                    {
                        avaliacao.NotaFinal = ultimaAvaliacao.intNota;
                        avaliacao.Professor = new Pessoa { ID = ultimaAvaliacao.intEmployeeId };
                    }

                    return avaliacao;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public int SetProgressoVideoResumo(ProgressoVideo progresso)
        {
            using(MiniProfiler.Current.Step("Inserir Progresso Video Resumo"))
            {
                try
                {
                    using (var ctx = new DesenvContext())
                    {
                        var posicao = ctx.tblResumoAulaVideoLogPosition.FirstOrDefault(p => p.intClientId == progresso.Matricula && p.intResumoAulaVideoId == progresso.IdResumoAula);

                        if (posicao == null)
                        {
                            ctx.tblResumoAulaVideoLogPosition.Add(new tblResumoAulaVideoLogPosition
                            {
                                intClientId = progresso.Matricula,
                                dteLastUpdate = DateTime.Now,
                                intResumoAulaVideoId = progresso.IdResumoAula,
                                intSecond = progresso.ProgressoSegundo
                            });
                        }
                        else
                            if (progresso.ProgressoSegundo > posicao.intSecond)
                        {
                            posicao.dteLastUpdate = DateTime.Now;
                            posicao.intSecond = progresso.ProgressoSegundo;
                        }

                        ctx.SaveChanges();
                        return 1;
                    }
                }
                catch
                {
                    throw;
                }
            }   
        }

        public AvaliacaoProfessor GetAvaliacaoRealizadaResumo(int matricula, int idResumoAulaIndice, int idAplicacao)
        {
            using(MiniProfiler.Current.Step("Obter Avaliação Realizada Resumo"))
            {
                try
                {
                    using (var ctx = new DesenvContext())
                    {
                        var avaliacao = new AvaliacaoProfessor();
                        var ultimaAvaliacao = ctx.tblLessonEvaluationVideoAula.Where(e =>
                            e.intClientId == matricula
                            && e.intTipoVideo == (int)ETipoVideo.Resumo
                            && e.intAulaIndiceId == idResumoAulaIndice).ToList().FirstOrDefault();

                        if (ultimaAvaliacao != null)
                        {
                            avaliacao.NotaFinal = ultimaAvaliacao.intNota;
                            avaliacao.Professor = new Pessoa { ID = ultimaAvaliacao.intEmployeeId };
                        }

                        return avaliacao;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public int SetLogVideoAssitido(VideoMednet log)
        {
            using(MiniProfiler.Current.Step("Inserir Log Video Assistido"))
            {
                try
                {
                    using (var ctx = new DesenvContext())
                    {
                        ctx.tblVideoLog.Add(new tblVideoLog
                        {
                            intClientId = log.Matricula,
                            intOrigemVideoId = log.IdRevisaoAula,
                            dteCadastro = DateTime.Now,
                            intTipoVideo = (int)log.TipoVideo
                        });

                        ctx.SaveChanges();
                        return 1;
                    }
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public TemaApostila CalculaProgressosVideosTemaProva(TemaApostila videoTema, int matricula)
        {
            var idsVideos = videoTema.Videos.Select(v => v.IdProvaVideo).ToList();

            using (var ctx = new DesenvContext())
            {
                var todosProgressosVideos = ctx.tblProvaVideoLogPosition.Where(m => m.intClientId == matricula).ToList();

                foreach (var video in videoTema.Videos.Where(x => todosProgressosVideos.Any(y => y.intProvaVideoId == x.IdProvaVideo)))
                {
                    video.Assistido = true;
                    var duracaoVideo = Convert.ToInt32(TimeSpan.Parse(video.DuracaoFormatada).TotalSeconds);
                    if (duracaoVideo > 0)
                    {
                        var videoProgresso = todosProgressosVideos.Where(v => v.intProvaVideoId == video.IdProvaVideo).FirstOrDefault();
                        video.Progresso = (videoProgresso.intSecond * 100) / duracaoVideo;

                        if (video.Progresso > 100)
                            video.Progresso = 100;

                        video.UltimaPosicaoAluno = CalculaUltimaPosicaoVideoAluno(videoProgresso.intLastSecondViewed, video.Duracao);
                    }
                }
            }
            return videoTema;
        }

        private int CalculaUltimaPosicaoVideoAluno(int posicaoSegundos, int? duracaoVideo)
        {
            const int POSICAOINICIAL = 0;
            const int POSICAOMINIMA = 5;
            const int SEGUNDOSRETORNO = 5;
            const int TOLERANCIA = 5;

            if (posicaoSegundos < POSICAOMINIMA || posicaoSegundos >= (duracaoVideo ?? 0 - TOLERANCIA))
            {
                return POSICAOINICIAL;
            }
            else
            {
                return posicaoSegundos - SEGUNDOSRETORNO;
            }
        }

        public TemaApostila CalculaProgressosVideosTemaRevisao(TemaApostila videoTema, int matricula)
        {
            var idsVideos = videoTema.VideosRevisao.Select(v => v.IdRevisaoAula).ToList();

            var ctx = new DesenvContext();

            var totalVisualizacoesVideos = ctx.tblRevisaoAulaVideoLog.Where(l => idsVideos.Contains(l.intRevisaoAulaId)).ToList();
            var todosProgressosVideos = ctx.tblRevisaoAulaVideoLogPosition.Where(m => m.intClientId == matricula).ToList();

            ctx.Dispose();

            foreach (var professor in videoTema.Professores)
            {
                var segundosTotalVideos = 0;
                var segundosTotalVideosAssistidos = 0;
                var videosProf = videoTema.VideosRevisao.Where(v => v.IdProfessor == professor.ID).ToList();

                foreach (var video in videosProf)
                {
                    video.Visualizacoes = totalVisualizacoesVideos
                        .Where(t => t.intRevisaoAulaId == video.IdRevisaoAula)
                        .Count();

                    segundosTotalVideos += Convert.ToInt32(TimeSpan.Parse(video.DuracaoFormatada).TotalSeconds);

                    if (todosProgressosVideos.Any(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula))
                    {
                        video.Assistido = true;
                        segundosTotalVideosAssistidos += todosProgressosVideos.FirstOrDefault(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula).intSecond;
                    }
                    var duracaoVideo = Convert.ToInt32(TimeSpan.Parse(video.DuracaoFormatada).TotalSeconds);

                    if (todosProgressosVideos.Any(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula) && duracaoVideo > 0)
                        video.Progresso = (todosProgressosVideos.Where(v => v.intRevisaoAulaVideoId == video.IdRevisaoAula).FirstOrDefault().intSecond * 100) / duracaoVideo;

                }
                if (segundosTotalVideos != 0)
                    professor.PercentVisualizado = (segundosTotalVideosAssistidos * 100) / segundosTotalVideos;
                else
                    professor.PercentVisualizado = 0;

                SetProgressoAulaRevisao(videoTema.IdTema, professor.ID, matricula, professor.PercentVisualizado);
            }

            return videoTema;
        }

    }
}