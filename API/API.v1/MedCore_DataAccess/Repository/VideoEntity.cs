using System;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Entidades;
using Microsoft.EntityFrameworkCore;
using MedCore_DataAccess.Business;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_API.Academico;
using System.Collections;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Contracts.Enums;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Repository
{
    public class VideoEntity : IVideoData
    {
        private static string _domainCloud = Constants.DOMAINCLOUDFRONT.Replace("BUCKET", "dji4beh2iqsf0");
        private static string _domainCloudVideosIntro = Constants.DOMAINCLOUDFRONT.Replace("BUCKET", "d1jxjhan96lp8n") + "intro/";
        private static string _domainS3 = Constants.DOMAINS3.Replace("BUCKET", Constants.DEFAULTBUCKET);
        private static string _domainBaixaS3 = Constants.DOMAINS3US.Replace("BUCKET", Constants.CONCURSOSFLVBUCKETBAIXA);
        private Contracts.Business.IChaveamentoVimeo _chaveamentoVimeoQuestao = new ChaveamentoComentarioQuestao();
        private static bool _chaveamentoVimeoApostila = Convert.ToBoolean(ConfigurationProvider.Get("Settings:AtivarVimeoApostila") ?? "false");

        public static bool IsVideoINTRO(string codigo)
        {
            return codigo.ToUpper().Contains("INTRO_");
        }

        public static bool IsVideoMEDi(string codigo)
        {
            return codigo.ToUpper().StartsWith("VIDEO_") && !codigo.ToUpper().Contains("ADAPTA");
        }

        public Video GetVideoIntro(int apostilaID = 0, string guid = "", string param = "")
        {
            var retorno = new Video();

            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    int apostilaVideoID = 0;

                    if (guid == "" && apostilaID != 0)
                    {
                        using(MiniProfiler.Current.Step("Obtendo dados do vídeo"))
                        {
                            apostilaVideoID = (from b in ctxMatDir.tblBooks
                                                join vbi in ctxMatDir.tblVideo_Book_Intro on b.intBookID equals vbi.intBookID
                                                //join v in ctxMatDir.tblVideos on vbi.intVideoID equals v.intVideoID
                                                where b.intBookID == apostilaID
                                                //select v.guidVideoID
                                                select vbi.intVideoID).FirstOrDefault();

                            var g = (from v in ctx.tblVideo
                                    where v.intVideoID == apostilaVideoID
                                    select v.guidVideoID).FirstOrDefault();

                            guid = g.ToString();
                        }
                    }
                    if (guid != "" && guid != new Guid().ToString())
                    {
                        var v = GetVideo(guid, param, _chaveamentoVimeoApostila);
                        retorno.Url = v.Url;
                        retorno.KeyVideo = v.KeyVideo;
                        retorno.VideoId = apostilaVideoID;
                    }

                    return retorno;
                }
            }
        }

        public Video GetVideo(string guid, string param = "", bool chaveamentoVimeo = true, int idAplicacao = (int)Aplicacoes.MsProMobile)
        {
            var retorno = new Video();
            if (idAplicacao != (int)Aplicacoes.MsProMobile && idAplicacao != (int)Aplicacoes.MsProDesktop)
            {
                using(MiniProfiler.Current.Step("Obtendo dados do vídeono bor"))
                {
                    var borkey = GetBorKey(guid);
                    retorno.KeyVideo = borkey;
                }
            }
            retorno.Url = GetUrlVideoPorGuid(guid, chaveamentoVimeo, idAplicacao);
            return retorno;
        }

        public string GetBorKey(string txtVideoCode, int? intYear, int intBookID)
        {
            //DesenvEntities ctx;
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    var apostila = (from a in ctxMatDir.tblVideo_Book
                                    join c in ctxMatDir.tblBooks on a.intBookID equals c.intBookID
                                    where a.txtVideoCode == txtVideoCode
                                            && c.intYear == intYear
                                            && c.intBookID == intBookID
                                    select new
                                    {
                                        intVideoID = a.intVideoID
                                    }).ToList();

                    List<int?> videoIds = apostila.Select(x => x.intVideoID).ToList();

                    return (from v in ctx.tblVideo
                            where v.bitActive != false
                            && videoIds.Contains(v.intVideoID)
                            select v.txtFileName.Trim().Replace(".xml", "")).AsQueryable().SingleOrDefault() ?? string.Empty;
                }
            }
        }

        public List<VideoMiolo> GetByFilters(VideoMiolo registro)
        {
            var ctx = new DesenvContext();
            var lstVideosMiolo = new VideosMiolo();

            var consulta = from a in ctx.tblBooks
                           join b in ctx.tblVideo_Book on a.intBookID equals b.intBookID
                           join c in ctx.tblProducts on a.intBookID equals c.intProductID
                           orderby a.intYear ascending

                           select new VideoMiolo
                           {
                               IDApostila = b.intBookID,
                               CodigoVideo = b.txtVideoCode,
                               VideoID = b.intVideoID,
                               Ano = a.intYear,
                           };


            if (registro.IDApostila > 0)
            {
                consulta = consulta.Where(b => b.IDApostila == registro.IDApostila
                                         && b.CodigoVideo == registro.CodigoVideo);
            }
            else if (!string.IsNullOrEmpty(registro.CodigoVideo))
            {
                consulta = consulta.Where(b => b.CodigoVideo == registro.CodigoVideo);
            }

            return consulta.ToList();
        }       

        public string GetUrlVideoPorGuid(string guid, bool chaveamentoVimeo, int idAplicacao)
        {
            var url = string.Empty;

            using (var ctx = new AcademicoContext())
            {
                tblVideo video = ctx.tblVideo.Where(b => b.guidVideoID == new Guid(guid)).FirstOrDefault();
                if (chaveamentoVimeo)
                    url = video.txtUrlVimeo;

                if (String.IsNullOrEmpty(url))
                    using(MiniProfiler.Current.Step("Obtendo dados do vídeo no vimeo"))
                    {   
                        url = GetUrlPlataformaVideo(video, chaveamentoVimeo, idAplicacao);
                    }
            }

            return url;
        }

        public string GetUrlPlataformaVideo(tblVideo video, bool chaveamentoVimeo, int idAplicacao)
        {
            var url = string.Empty;

            //Se o video não tiver no banco mas tem o VimeoID, ele busca no VIMEO, grava no banco e retorna a URL do video
            if (chaveamentoVimeo && video.intVimeoID.HasValue)
                url = new VideoBusiness(new VideoEntity()).ObterURLVimeoPorIntVimeoID(video.intVimeoID, idAplicacao, 0, video.txtName);

            //Se o video ainda não existir no VIMEO ele verifica se é BitsOnTheRun ou AmazonAWS e retorna a URL
            if (String.IsNullOrEmpty(url) && video.txtPath.ToLower().Contains("bitsontherun"))
                url = Criptografia.GetSignedPlayer(String.Concat(video.txtFileName.Replace(".xml", string.Empty).Trim(), "-720"));
            else if (String.IsNullOrEmpty(url) && video.txtPath.ToLower().Contains("amazonaws"))
                url = Criptografia.GetS3SignedPlayer(String.Concat(_domainS3, video.txtName.Trim(), '.', Constants.FORMATOVIDEO));

            return url;
        }

        public string GetBorKey(string guid)
        {

            var ht = new Hashtable();
            ht.Add("search", guid);
            var S3VideosList = (new BorApi(Constants.BORACCESSKEY, Constants.BORSECRETKEY)).LoadS3VideosList("", ht);
            var borkey = S3VideosList.Any() ? S3VideosList[0].KeyVideo : "";
            return borkey;
        }
        public int SetAvaliacaoVideo(AvaliacaoVideoApostila avaliacao)
        {
            var avaliacaoAnterior = GetAvaliacaoVideo(avaliacao);
            if (avaliacaoAnterior != null)
            {
                if (avaliacaoAnterior.TipoVote == avaliacao.TipoVote)
                {
                    return DeleteAvaliacaoApostila(avaliacaoAnterior);
                }
                else
                {
                    DeleteAvaliacaoApostila(avaliacaoAnterior);
                }
            }
            return InsertAvaliacaoApostila(avaliacao);
        }

        public AvaliacaoVideoApostila GetAvaliacaoVideo(AvaliacaoVideoApostila avaliacao)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var contactID = Convert.ToInt32(avaliacao.Matricula);
                    var avaliacaoUsuario = (from a in ctx.tblVideoVote
                                            where a.intContactID == contactID && a.intVideoID == avaliacao.VideoID && a.intBookID == avaliacao.MaterialID
                                            select new AvaliacaoVideoApostila()
                                            {
                                                ID = a.intVideoVoteID,
                                                VideoID = a.intVideoID.Value,
                                                MaterialID = a.intBookID.Value,
                                                Matricula = a.intContactID.Value,
                                                TipoVote = a.intTipoInteracaoId.Value
                                            }).FirstOrDefault();
                    return avaliacaoUsuario;
                }
            }
            catch
            {
                return null;
            }
        }

        public int DeleteAvaliacaoApostila(AvaliacaoVideoApostila avaliacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var entity = new tblVideoVote
                    {
                        intVideoVoteID = avaliacao.ID,
                        intBookID = avaliacao.MaterialID,
                        intContactID = Convert.ToInt32(avaliacao.Matricula),
                        intTipoInteracaoId = avaliacao.TipoVote,
                        intVideoID = avaliacao.VideoID
                    };

                    ctx.Entry(entity).State = EntityState.Deleted;
                    return ctx.SaveChanges();
                }
                catch
                {
                    return -1;
                }
            }
        }

        public int InsertAvaliacaoApostila(AvaliacaoVideoApostila avaliacao)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var entity = new tblVideoVote
                    {
                        intContactID = avaliacao.Matricula,
                        intTipoInteracaoId = avaliacao.TipoVote,
                        dteDataCriacao = DateTime.Now,
                        intBookID = avaliacao.MaterialID,
                        intVideoID = avaliacao.VideoID
                    };

                    ctx.tblVideoVote.Add(entity);
                    return ctx.SaveChanges();
                }
                catch
                {
                    return -1;
                }
            }
        }

        public VideoQualidadeDTO[] GetLinksVideoVariasQualidades(string videoInfo, string urlPadrao)
        {
            try
            {
                if (!string.IsNullOrEmpty(videoInfo))
                {
                    JObject info = JObject.Parse(videoInfo);
                    JArray files = (JArray)info["files"];
                    IList<VideoQualidadeDTO> videoQualidadeDTOs = files.ToObject<IList<VideoQualidadeDTO>>();
                    return videoQualidadeDTOs.ToArray();
                }
                else
                {
                    return VideoQualidadeLinkPadrão(urlPadrao);
                }
            }
            catch
            {
                return VideoQualidadeLinkPadrão(urlPadrao);
            }
        }

        public VideoQualidadeDTO[] VideoQualidadeLinkPadrão(string urlPadrao)
        {
            return new VideoQualidadeDTO[]
            {
                new VideoQualidadeDTO
                {
                    Link = urlPadrao,
                    Qualidade = "hd",
                    Altura = 720,
                    Largura = 1280
                }
            };
        }

        public Videos GetVideosApostilaCodigoJson(string codigo, string matricula = null, Aplicacoes IdAplicacao = Aplicacoes.MsProMobile, string versao = "")
        {
            if (IsVideoINTRO(codigo))
            {
                var v = Convert.ToInt32(codigo.ToUpper().Replace("INTRO_", "").Trim());
                return (new Videos { GetVideoIntro(v) });
            }
            else
            {
                var vm = new VideoMiolo();

                if (codigo.Contains("-"))
                {
                    string[] CodigoApostilaID = codigo.Split('-');

                    vm.IDApostila = int.Parse(CodigoApostilaID[0]);
                    vm.CodigoVideo = CodigoApostilaID[1];
                }
                else
                    vm.CodigoVideo = codigo;

                string versaoapp = versao == "" ? ConfigurationProvider.Get("Settings:ChaveamentoVersaoMinimaMsPro") : versao;
                return (Videos)GetByVideoMiolo(vm, matricula, IdAplicacao, versaoapp);
            }
        }

        public List<Video> GetByFilters(Video registro)
        {
            var strID = registro.ID.ToString();

            using (var ctx = new AcademicoContext())
            {
                return (from v in ctx.tblVideo
                        let intVideoIDToStr = Convert.ToString(v.intVideoID).Trim()
                        where (registro.ID == 0 || intVideoIDToStr.StartsWith(strID)) && (string.IsNullOrEmpty(registro.Nome) || v.txtName.Contains(registro.Nome))
                        orderby v.txtName
                        select new Video()
                        {
                            ID = v.intVideoID,
                            Nome = v.txtName.Trim(),
                            DteDataModificacao = v.dteLastModifyDate
                        }).ToList();
            }
        }

        public List<Video> GetByVideoMiolo(VideoMiolo vm, string matricula = null, Aplicacoes IdAplicacao = Aplicacoes.MsProMobile, string VersaoApp = "")
        {
            VideoMioloEntity vme = new VideoMioloEntity();
            vme.setVersaoAplicacao((int)IdAplicacao, VersaoApp);
            vme.setChaveamentoVimeoApostila(new ChaveamentoVimeoMediMiolo());

            var lst = vme.GetByFilters(vm).ToList();

            var lstv = new Videos();
            foreach (var valor in lst)
            {
                if (!lstv.Any(i => i.Url.Equals(valor.HTTPURL)))
                {
                    var video = new Video();
                    video.ID = Convert.ToInt32(valor.ID);
                    video.VideoId = valor.VideoID;
                    video.Url = valor.HTTPURL;
                    video.Thumb = valor.URLThumb;
                    video.Links = valor.Links;
                    video = CreateVideoObject(video, matricula);
                    lstv.Add(video);
                }
            }
            return lstv;
        }

        public Video CreateVideoObject(Video video, string matricula)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var id = Convert.ToInt32(video.VideoId);
                    var contactID = Convert.ToInt32(matricula);
                    video.UpVote = (from a in ctx.tblVideoVote
                                    where a.intVideoID == id && a.intTipoInteracaoId == (int)ETipoVideoVote.Upvote
                                    select a).Count();
                    video.DownVote = (from a in ctx.tblVideoVote
                                      where a.intVideoID == id && a.intTipoInteracaoId == (int)ETipoVideoVote.Downvote
                                      select a).Count();
                    video.VotadoUpvote = (from a in ctx.tblVideoVote
                                          where a.intVideoID == id && a.intTipoInteracaoId == (int)ETipoVideoVote.Upvote && a.intContactID == contactID
                                          select a).Any();
                    video.VotadoDownvote = (from a in ctx.tblVideoVote
                                             where a.intVideoID == id && a.intTipoInteracaoId == (int)ETipoVideoVote.Downvote && a.intContactID == contactID
                                             select a).Any();
                    return video;
                }
            }
            catch
            {
                return null;
            }
        }

        public int InserirRegistroVideoDefeito(jsonVimeo e, string url, string nome, int videoId, int? vimeoId)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    var video = new tblAcademicoVideoEmail();
                    video.dteOcorrencia = DateTime.Now;
                    video.intVideoId = videoId;
                    video.intVimeoId = vimeoId;
                    video.txtName = nome;
                    video.txtVideoStatus = e != null ? e.status : null;

                    ctx.tblAcademicoVideoEmail.Add(video);
                    ctx.SaveChanges();

                    return video.intAcademicoVideoEmail;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public bool TemVideoRegistradoParaEnvio(int intVideoId, int? intVimeoId)
        {
            using (var ctx = new DesenvContext())
            {
                var data = DateTime.Now.Date;
                var temRegistro = (from v in ctx.tblAcademicoVideoEmail
                                  where (v.intVimeoId == intVimeoId || v.intVideoId == intVideoId) &&
                                  v.dteOcorrencia.Value == data
                                  select v.intVideoId).Any();

                return temRegistro;
            }
        }

        public void UpdateVideo(tblVideo video)
        {
            using (var ctx = new AcademicoContext())
            {
                ctx.Entry(video).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public string GetUrlThumb(tblVideo video, Contracts.Business.IChaveamentoVimeo ObjChaveamentoVimeo, string versaoApp = "")
        {
            var url = string.Empty;

            url = video.txtUrlThumbVimeo;

            if (String.IsNullOrEmpty(url))
                url = "https://cdn.jwplayer.com/thumbs/" + video.txtFileName.Replace(".xml", "").Trim() + "-480.jpg";

            return url;
        }

        public tblVideo GetVideoVimeo(int? intVimeoID, int intVideoID = 0)
        {
            using (var ctx = new AcademicoContext())
            {
                tblVideo video = new tblVideo();

                if (intVimeoID != null)
                {
                    video = ctx.tblVideo.Where(x => x.intVimeoID == intVimeoID).FirstOrDefault();
                }

                if (video.intVimeoID == null)
                    video = ctx.tblVideo.Where(x => x.intVideoID == intVideoID).FirstOrDefault();

                return video;
            }
        }

        public string GetUrlVideoPorVideoID(int idVideo, Contracts.Business.IChaveamentoVimeo ObjChaveamentoVimeo, int idAplicacao = (int)Aplicacoes.MsProMobile, string versaoApp = "")
        {
            using(MiniProfiler.Current.Step("Obtendo url de vídeo por ID"))
            {
                var video = GetVideoPorVideoID(idVideo, ObjChaveamentoVimeo, idAplicacao, versaoApp);
                return video.Url;
            }
        }

        [Obsolete("Este método resupera somente a url thum do BOR. Para recuperar a thumb do Vimeo utilize o método GetUrlThumbVideo disponibilizado na Business")]
        public static string GetUrlThumb(string key, Utilidades.VideoThumbSize size)
        {
            // REF: https://developer.jwplayer.com/jw-platform/reference/v1/urls/thumbs.html
            return string.Format(@"http://content.jwplatform.com/thumbs/{0}-{1}.jpg", key, Convert.ToInt32(size));
        }

        public VideoUrlDTO GetVideoPorVideoID(int idVideo, Contracts.Business.IChaveamentoVimeo ObjChaveamentoVimeo, int idAplicacao = (int)Aplicacoes.MsProMobile, string versaoApp = "")
        {
            VideoUrlDTO videoRetorno = new VideoUrlDTO
            {
                Url = string.Empty
            };

            tblVideo video = new tblVideo();

            using (var ctx = new AcademicoContext())
            {
                video = ctx.tblVideo.Where(b => b.intVideoID == idVideo).FirstOrDefault();
            }

            bool chaveamentoVimeo;
            chaveamentoVimeo = ObjChaveamentoVimeo.GetChaveamento();

            if (chaveamentoVimeo)
            {
                if (idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON || idAplicacao == (int)Aplicacoes.AreaRestrita)
                    videoRetorno.Url = video.txtUrlStreamVimeo;      
                else
                    videoRetorno.Url = video.txtUrlVimeo;
            }

            if (String.IsNullOrEmpty(videoRetorno.Url))
                videoRetorno.Url = GetUrlPlataformaVideo(video, chaveamentoVimeo, idAplicacao);

            if (idAplicacao == (int)(Aplicacoes.MsProMobile))
                videoRetorno.Links = GetLinksVideoVariasQualidades(video.txtVideoInfo, videoRetorno.Url);


            int duracao = 0;
            Int32.TryParse(video.intDuracao, out duracao);
            videoRetorno.Duracao = duracao;

            return videoRetorno;
        }

        public virtual VideoDTO ConvertToVideoDTO(Video video)
        {
            VideoDTO videoDto = new VideoDTO();
            videoDto.ID = video.ID;
            videoDto.Guid = video.Guid;
            videoDto.StatusID = video.StatusID;
            videoDto.Duracao = video.Duracao;
            videoDto.Tema = video.Tema;
            videoDto.Descricao = video.Descricao;
            videoDto.KeyVideo = video.KeyVideo;
            videoDto.Ativo = video.Ativo;
            videoDto.Nome = video.Nome;
            videoDto.Thumb = video.Thumb;
            videoDto.Url = video.Url;
            videoDto.QuestaoDoVideo = video.QuestaoDoVideo;
            videoDto.ExerciciosVideo = video.ExerciciosVideo;
            videoDto.Tamanho = video.Tamanho;
            videoDto.UnixCriacao = video.UnixCriacao;
            videoDto.ExisteAmazon = video.ExisteAmazon;
            videoDto.DataModificacao = video.DataModificacao;
            videoDto.DteDataModificacao = video.DteDataModificacao;
            videoDto.VimeoId = video.VimeoId;
            videoDto.VideoId = video.VideoId;
            videoDto.UpVote = video.UpVote;
            videoDto.DownVote = video.DownVote;
            videoDto.VotadoUpvote = video.VotadoUpvote;
            videoDto.VotadoDownvote = video.VotadoDownvote;
            videoDto.Links = video.Links;
            return videoDto;
        }

        public Video GetVideo(int idQuestao, int idTipo, int idAplication, string versaoApp)
        {
            using(MiniProfiler.Current.Step("Obtendo video"))
            {
            // ======================== LOG
                new Util.Log().SetLog(new LogMsPro
                {
                    IdApp = (Aplicacoes)idAplication,
                    Tela = Util.Log.MsProLog_Tela.RealizaProvaComentario,
                    Acao = Util.Log.MsProLog_Acao.Abriu,
                    Obs = string.Format("{0} Vídeo - ID: {0}", idTipo == 1 ? "Simulado" : "CI", idQuestao)
                });
                // ======================== 

                switch (idTipo)
                {
                    case 1:
                        return GetVideoQuestaoSimulado(idQuestao, idAplication, versaoApp).FirstOrDefault();

                    case 2:
                    case 3:
                        return GetVideoQuestaoConcurso(idQuestao, idAplication, versaoApp).FirstOrDefault();
                    case 4:
                    //TODO return GetVideosQuestaoMontaProva();

                    default:
                        return new Video();
                }
            }

        }

        public List<Video> GetVideoQuestaoConcurso(int idQuestao, int idAplicacao = 5, string appVersion = "")
        {
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    var lst = new Videos();

                    int VideoId = (from qv in ctxMatDir.tblVideo_Questao_Concurso
                                   where qv.intQuestaoID == idQuestao
                                   select qv.intVideoID).FirstOrDefault();


                    var consulta = (from v in ctx.tblVideo
                                    where v.intVideoID == VideoId
                                    select v).FirstOrDefault();


                    if (consulta != null)
                    {
                        //TODO: Refatorar para a classe de business
                        VideoBusiness videoBusiness = new VideoBusiness(this);
                        string url = videoBusiness.ObterUrlVideo(idAplicacao, consulta, new ChaveamentoQuestaoConcurso(), appVersion);
                        lst.Add(new Video
                        {
                            ID = consulta.intVideoID,
                            Url = url,
                            Thumb = !string.IsNullOrEmpty(consulta.txtUrlThumbVimeo) ? consulta.txtUrlThumbVimeo : videoBusiness.ObterURLThumb(consulta.intVideoID, consulta.txtUrlThumbVimeo),
                            Nome = consulta.txtName.Trim(),
                            ExisteAmazon = videoBusiness.UrlVideoValida(url),
                            Links = GetLinksVideoVariasQualidades(consulta.txtVideoInfo, url)
                        }); ;
                    }
                    else
                        lst.Add(new Video { Url = "http://iosstream.s3.amazonaws.com/videosemcomentario.mp4", Nome = string.Empty });

                    return lst;
                }
            }
        }

        public int ObterVimeoID(int intVideoID)
        {
            using (var ctx = new AcademicoContext())
            {
                var vimeoId = (from v in ctx.tblVideo
                               where v.intVideoID == intVideoID
                               select v.intVimeoID
                                       ).FirstOrDefault();
                int intVimeoId = 0;
                Int32.TryParse(vimeoId.ToString(), out intVimeoId);
                return intVimeoId;
            }

        }

        public Videos GetVideoQuestaoSimulado(int QuestaoID, int idAplicacao = 0, string versaoApp = "")
        {
            string versao = versaoApp != "" ? versaoApp : ConfigurationProvider.Get("Settings:ChaveamentoVersaoMinimaMsPro");

            using (var ctx = new AcademicoContext())
            {
                var lst = new Videos();

                var consulta = (from v in ctx.tblVideo
                                join qv in ctx.tblVideo_Questao_Simulado on v.intVideoID equals qv.intVideoID
                                where qv.intQuestaoID == QuestaoID
                                select new { v.intVideoID, v.txtFileName, v.txtPath, v.guidVideoID, v.txtVideoInfo }).FirstOrDefault();

                if (consulta != null)
                {
                    VideoBusiness videoBusiness = new VideoBusiness(new VideoEntity());
                    var url = GetUrlVideoPorVideoID(consulta.intVideoID, new ChaveamentoVimeoSimulado(), idAplicacao, versao);
                    lst.Add(new Video
                    {
                        ID = consulta.intVideoID,
                        Url = url,
                        Thumb = videoBusiness.ObterURLThumb(consulta.intVideoID, ""),
                        Nome = consulta.txtFileName.Replace(".xml", "").Trim(),
                        Links = GetLinksVideoVariasQualidades(consulta.txtVideoInfo, url)

                    });
                }
                else
                    lst.Add(new Video { Url = "http://iosstream.s3.amazonaws.com/videosemcomentario.mp4", Nome = string.Empty });

                return lst;
            }
        }

        public int GetDuracao(int idQuestao)
        {
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    int idVideo = (from vqc in ctxMatDir.tblVideo_Questao_Concurso
                                   where vqc.intQuestaoID == idQuestao
                                   select vqc.intVideoID).FirstOrDefault();

                    var duracao = (from v in ctx.tblVideo
                                   where v.intVideoID == idVideo
                                   select v.intDuracao).FirstOrDefault();

                    return string.IsNullOrEmpty(duracao) ? 0 : Convert.ToInt32(duracao);
                }
            }
        }

        public List<Video> GetVideosByMedia(int idApostila, int idDataMatrix)
        {
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {

                    List<int?> videoIds = ((
                                            from m in ctxMatDir.tblMedcode_DataMatrix
                                            join mtp in ctxMatDir.tblMedcode_DataMatrix_Tipo on m.intMediaTipo equals mtp.intMediaTipoID
                                            join vb in ctxMatDir.tblVideo_Book on m.txtMediaCode equals vb.txtVideoCode
                                            where m.intDataMatrixID == idDataMatrix
                                              && vb.intBookID == idApostila
                                              && m.intBookID == idApostila
                                              && (mtp.intMediaTipoID == (int)Media.Tipo.VideoECG || mtp.intMediaTipoID == (int)Media.Tipo.VideoApostila || mtp.intMediaTipoID == (int)Media.Tipo.MEDi)
                                            select vb.intVideoID
                                           ).Union(
                                            from m in ctxMatDir.tblMedcode_DataMatrix
                                            join mtp in ctxMatDir.tblMedcode_DataMatrix_Tipo on m.intMediaTipo equals mtp.intMediaTipoID
                                            join q in ctxMatDir.tblVideo_Questao_Concurso on m.intMediaID equals q.intQuestaoID
                                            where m.intDataMatrixID == idDataMatrix
                                            && mtp.intMediaTipoID == (int)Media.Tipo.VideoQuestão
                                            select (int?)q.intVideoID)
                                            ).ToList();

                    var consulta = (
                      from v in ctx.tblVideo
                      where videoIds.Contains(v.intVideoID)
                      select new
                      {
                          v.intVideoID,
                          v.txtName,
                          v.txtPath,
                          v.txtFileName
                      });

                    var lst = new Videos();
                    foreach (var valor in consulta)
                    {
                        var v = new Video
                        {
                            ID = valor.intVideoID,
                            Url = GetUrlVideoPorVideoID(valor.intVideoID, _chaveamentoVimeoQuestao)
                        };

                        if (valor.txtPath.ToLower().Contains("bitsontherun"))
                            v.Thumb = String.Concat(valor.txtPath.Replace("jwp/", "").Trim(), "thumbs/", valor.txtFileName.Replace(".xml", "").Trim(), "-240.jpg");
                        else
                            v.Thumb = Constants.URLTHUMBVIDEO.Replace("IDVIDEO", valor.intVideoID.ToString()).Replace("FORMATO", "cellthumb");

                        //v.Thumb = Constants.URLCOMENTARIOIMAGEM.Replace("IDCOMENTARIOIMAGEM", valor.intVideoID.ToString()).Replace("FORMATO", "cellthumb");
                        //v.Thumb = Constants.URLTHUMBVIDEO.Replace("IDVIDEO", v.ID.ToString());
                        lst.Add(v);
                    }

                    return lst;
                }
            }
        }

        public List<int> GetVideoByCode(string CodeApostila)
        {
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    var products = new List<int?>() { (int)Produto.Cursos.MEDCURSO, (int)Produto.Cursos.MEDMEDCURSO };

                    List<int?> videoIds = (from ri in ctxMatDir.tblRevisaoAulaIndice
                                           join rv in ctxMatDir.tblRevisaoAulaVideo on ri.intRevisaoAulaIndiceId equals rv.intRevisaoAulaIndiceId
                                           join p in ctxMatDir.tblProducts on ri.intBookId equals p.intProductID
                                           join b in ctxMatDir.tblBooks on p.intProductID equals b.intBookID
                                           join a in ctxMatDir.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on p.intProductGroup3 equals a.intProductGroupID
                                           join lt in ctxMatDir.tblLessonTitles on ri.intLessonTitleId equals lt.intLessonTitleID
                                           join prof in ctxMatDir.tblPersons on rv.intProfessorId equals prof.intContactID

                                           where
                                               products.Contains(p.intProductGroup2)
                                               && p.txtCode.Contains(CodeApostila)
                                           select rv.intVideoId).ToList();

                    return (from v in ctx.tblVideo
                            where videoIds.Contains(v.intVideoID)
                            select v).Select(a => a.intVideoID).Distinct().ToList();
                }
            }
        }


        public List<VideoMapaMentalDTO> GetVideoMapaMentalIds(List<AulaAvaliacaoTemaDTO> temas)
        {
            var temasIds = temas.Select(x => x.TemaId).ToList();
            var professores = temas.Select(x => x.ProfessorId).ToList();

            using (var ctx = new DesenvContext())
            {
                var videos = (from v in ctx.tblMapaMentalVideos
                                where temasIds.Contains(v.intLessonTitleID)
                                && professores.Contains(v.intProfessorID)
                                select new VideoMapaMentalDTO
                                {
                                    Id = v.intMapaMentalVideoID,
                                    VideoId = v.intVideoID,
                                    ProfessorId = v.intProfessorID,
                                    TemaId = v.intLessonTitleID
                                }).ToList();
                                
                return videos;
            }
        }
    }
}