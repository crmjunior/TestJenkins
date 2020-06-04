using System.Collections.Generic;
using System.IO;
using System.Net;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;
using Newtonsoft.Json;
using System.Linq;
using MedCore_DataAccess.Repository;
using MedCore_API.Academico;
using System;
using AutoMapper;
using System.Threading.Tasks;
using MedCore_DataAccess.Contracts.Enums;

namespace MedCore_DataAccess.Business
{
    public class VideoBusiness
    {
        public IVideoData _vimeoRepository;

        public VideoBusiness(IVideoData vimeoRepository)
        {
            _vimeoRepository = vimeoRepository;
        }

        public Videos GetVideoApostila(string codigo, string matricula, Aplicacoes IdAplicacao = Aplicacoes.MsProMobile, string versao = "")
        {
            var videos = _vimeoRepository.GetVideosApostilaCodigoJson(codigo, matricula, IdAplicacao, versao);
            return videos;
        }

        public string ObterURLVimeoPorIntVimeoID(int? intVimeoID, int idAplicacao, int intVideoID = 0, string nome = null)
        {

            var links = GetVideoUrls(intVimeoID, idAplicacao, nome, intVideoID);

            if (idAplicacao == (int)Aplicacoes.AreaRestrita || idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON)
            {
                return GetStreamUrl(links);
            }
            else //outras aplicações, como Mobile
            {
                return GetMp4Url(links);
            }

        }

        public string ObterURLThumb(int intVideoID, string txtUrThumbVimeo)
        {
            if (ChaveamentoVimeo())
            {
                string urlThumbVimeo;

                if (txtUrThumbVimeo != null)
                    urlThumbVimeo = txtUrThumbVimeo;
                else
                {
                    int intVimeoId = _vimeoRepository.ObterVimeoID(intVideoID);
                    urlThumbVimeo = this.ObterURLThumbVimeo(intVimeoId);
                }

                if (urlThumbVimeo != null)
                {
                    return urlThumbVimeo;
                }
            }
            return this.ObterURLThumbCloudFrontPorIntVideoID(intVideoID);
        }

        public string ObterURLThumbCloudFrontPorIntVideoID(int intVideoID)
        {
            return Constants.URLTHUMBVIDEO.Replace("IDVIDEO", intVideoID.ToString()).Replace("FORMATO", "cellthumb");
        }

        public bool ChaveamentoVimeo()
        {
            string chaveUsarVimeo = ConfigurationProvider.Get("Settings:AtivarVimeoQuestaoConcurso");

            return (!string.IsNullOrEmpty(chaveUsarVimeo) && Convert.ToBoolean(chaveUsarVimeo));
        }

        public string ObterUrlVideo(int idAplicacao, tblVideo video, Contracts.Business.IChaveamentoVimeo chaveamentoVimeo, string versaoApp = "")
        {
            //if (ChaveamentoVimeo())
            if (chaveamentoVimeo.GetChaveamento())
            {
                if (video != null)
                {
                    string urlVimeo;

                    Business.VersaoAppPermissaoBusiness versaoAppPermissao = new VersaoAppPermissaoBusiness(new VersaoAppPermissaoEntity());

                    //if (idAplicacao == (int)Produto.Aplicacoes.AreaRestrita)
                    
                    if (idAplicacao == (int)Aplicacoes.AreaRestrita || (idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON && !(versaoAppPermissao.VersaoMenorOuIgual(versaoApp, ConfigurationProvider.Get("Settings:ChaveamentoVersaoMinimaMsProDesktop")))))
                        urlVimeo = video.txtUrlStreamVimeo;
                    else
                        urlVimeo = video.txtUrlVimeo;

                    if (urlVimeo == null)
                    {
                        urlVimeo = this.ObterURLVimeoPorIntVideoID(video.intVideoID, idAplicacao);
                    }

                    if (urlVimeo != null)
                    {
                        return urlVimeo;
                    }
                }
            }

            return this.ObterURLCloudFrontPorNomeVideo(video.txtName);
        }

        public string ObterURLVimeoPorIntVideoID(int intVideoID, int idAplicacao)
        {
            string urlVimeo = null;
            int intVimeoId = _vimeoRepository.ObterVimeoID(intVideoID);
            if (intVimeoId != 0)
            {
                urlVimeo = ObterURLVimeoPorIntVimeoID(intVimeoId, idAplicacao);
            }
            return urlVimeo;
        }

        public bool UrlVideoValida(string url)
        {
            if (url.IndexOf("player.vimeo.com") > -1)
                return true;
            if (url.IndexOf("cloudfront.net") > -1)
                return true;
            if (url.IndexOf("content.bitsontherun.com") > -1)
                return true;
            if (url.IndexOf(".amazonaws.com") > -1)
                return true;
            return false;
        }

        public string ObterURLCloudFrontPorNomeVideo(string txtName)
        {
            string domainCloud = Constants.DOMAINCLOUDFRONT.Replace("BUCKET", "dji4beh2iqsf0");
            return Criptografia.GetCloudFrontSignedPlayer(String.Concat(txtName.Trim(), '.', Constants.FORMATOVIDEO), domainCloud);
        }

        public List<JsonFileVimeoDTO> GetVideoUrls(int? intVimeoID, int idAplicacao, string nome, int intVideoID = 0)
        {
            string json = string.Empty;
            
            string token = "Bearer " + ConfigurationProvider.Get("Settings:VimeoToken");
            string url = Constants.URLAPIVIMEO + intVimeoID + "?fields=duration,files.link,files.quality,files.width,files.height,files.duration,pictures.sizes.link,pictures.sizes,status,upload.status,transcode.status,date";
            string urlMp4;
            string urlStream;
            string urlThumb;
            var fileLinks = new List<JsonFileVimeoDTO>();

            try
            {
                jsonVimeo jsonVimeo;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.Headers.Add("Authorization", token);

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)Constants.TLS12;
                ServicePointManager.ServerCertificateValidationCallback = (s, c, n, p) => { return true; };

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    json = reader.ReadToEnd();
                    jsonVimeo = JsonConvert.DeserializeObject<jsonVimeo>(json);
                }

                string duracaoVideo = jsonVimeo.duration;
                urlThumb = jsonVimeo.pictures.sizes.LastOrDefault().link;
                urlMp4 = GetMp4Url(jsonVimeo.files);
                urlStream = GetStreamUrl(jsonVimeo.files);

                SetURLsVimeo(intVimeoID.Value, urlMp4, urlStream, intVideoID, urlThumb, duracaoVideo, json);

                foreach (var link in jsonVimeo.files)
                {
                    var vimeoJson = new JsonFileVimeoDTO();
                    vimeoJson.link = link.link;
                    vimeoJson.height = link.height;
                    vimeoJson.width = link.width;
                    vimeoJson.quality = link.quality;
                    fileLinks.Add(vimeoJson);
                }
                return fileLinks;
            }
            catch
            {
                return null;
            }
        }

        private string GetMp4Url(List<JsonFileVimeoDTO> links)
        {
            try
            {
                if (links.Any(x => x.quality == "hd"))
                    return links.FirstOrDefault(f => f.quality == "hd").link + "&autoplay=1";
                else
                    return links.FirstOrDefault(f => f.width == (links.Max(x => x.width))).link + "&autoplay=1";
            }
            catch
            {
                return null;
            }

        }

        private string GetStreamUrl(List<JsonFileVimeoDTO> links)
        {
            try
            {
                if (links.Any(x => x.quality == "hd"))
                    return links.FirstOrDefault(f => f.quality == "hls").link + "&p=high&autoplay=1";
                else
                    return links.FirstOrDefault(f => f.quality == "hls").link + "&p=standard&autoplay=1";
            }
            catch
            {
                return null;
            }
        }

        private void SetURLsVimeo(int intVimeoID, string urlVideoMP4, string urlStream, int intVideoID, string urlThumb, string Duracao, string videoInfo)
        {
            VideoEntity videoEntity = new VideoEntity();
            tblVideo video = videoEntity.GetVideoVimeo(intVimeoID, intVideoID);
            if (video != null)
            {
                video.intDuracao = Duracao;
                video.txtUrlVimeo = urlVideoMP4 != null ? urlVideoMP4 : video.txtUrlVimeo;
                video.txtUrlStreamVimeo = urlStream != null ? urlStream : video.txtUrlStreamVimeo;
                video.txtUrlThumbVimeo = urlThumb != null ? urlThumb : video.txtUrlThumbVimeo;
                video.txtVideoInfo = videoInfo != null ? videoInfo : video.txtVideoInfo;
                videoEntity.UpdateVideo(video);
            }
        }


        public string ObterURLThumbVimeo(int intVimeoID)
        {
            string token = "Bearer " + ConfigurationProvider.Get("Settings:VimeoToken");
            string html = string.Empty;

            try
            {
                string url = Constants.URLTHUMBVIDEOVIMEO.Replace("VIMEOID", intVimeoID.ToString());
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.Headers.Add("Authorization", token);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                    JsonVimeoVideoDTO result = JsonConvert.DeserializeObject<JsonVimeoVideoDTO>(html);

                    SetThumbVimeo(intVimeoID, result.data.FirstOrDefault().sizes.LastOrDefault().link);

                    return result.data.FirstOrDefault().sizes.LastOrDefault().link;
                }
            }
            catch
            {
                return null;
            }
        }

        private void SetThumbVimeo(int intVimeoID, string urlThumb)
        {
            VideoEntity videoEntity = new VideoEntity();
            tblVideo video = videoEntity.GetVideoVimeo(intVimeoID);
            if (video != null)
            {
                video.txtUrlThumbVimeo = urlThumb != null ? urlThumb : video.txtUrlThumbVimeo;
                videoEntity.UpdateVideo(video);
            }
        }

        public IList<VideoDTO> ObterVideosApostila(VideoApostilaFiltroDTO filtro)
        {
            List<VideoDTO> v = new List<VideoDTO>();
            var videos = (IList<VideoDTO>)v;
            Parallel.For(0, filtro.IdsVideos.Count(), i =>
            {
                var codigo = filtro.IdsVideos[i];
                try
                {
                    if (IsVideoINTRO(codigo))
                    {
                        var v = Convert.ToInt32(codigo.ToUpper().Replace("INTRO_", "").Trim());
                        var videoRep = _vimeoRepository.GetVideoIntro(v);
                        videoRep = _vimeoRepository.CreateVideoObject(videoRep, filtro.Matricula.ToString());
                        var video = _vimeoRepository.ConvertToVideoDTO(videoRep);
                        video.KeyVideo = codigo;
                        videos.Add(video);
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

                        var listVideos = GetByVideoMiolo(vm, filtro.Matricula.ToString(), (Aplicacoes)filtro.IdAplicacao, filtro.AppVersion);

                        foreach(var v in listVideos)
                        {
                            v.KeyVideo = codigo;
                            var videoDTO = new VideoDTO()
                            {
                                Ativo = v.Ativo,
                                DataModificacao = v.DataModificacao,
                                Descricao = v.Descricao,
                                DownVote = v.DownVote,
                                DteDataModificacao = v.DteDataModificacao,
                                Duracao = v.Duracao,
                                ExerciciosVideo = v.ExerciciosVideo,
                                ExisteAmazon = v.ExisteAmazon,
                                Guid = v.Guid,
                                ID = v.ID,
                                KeyVideo = v.KeyVideo,
                                Links = v.Links,
                                Nome = v.Nome,
                                QuestaoDoVideo = v.QuestaoDoVideo,
                                StatusID = v.StatusID,
                                Tamanho = v.Tamanho,
                                Tema = v.Tema,
                                Thumb = v.Thumb,
                                UnixCriacao = v.UnixCriacao,
                                UpVote = v.UpVote,
                                Url = v.Url,
                                VideoId = v.VideoId,
                                VimeoId = v.VimeoId,
                                VotadoDownvote = v.VotadoDownvote,
                                VotadoUpvote = v.VotadoUpvote
                            };
                            videos.Add(videoDTO);
                        }
                    }
                }
                catch
                {
                    var vid = new Video();
                    vid.KeyVideo = filtro.IdsVideos[i];
                    videos.Add(_vimeoRepository.ConvertToVideoDTO(vid));
                }
            });
            return videos;
        }

        public List<Video> GetByVideoMiolo(VideoMiolo vm, string matricula = null, Aplicacoes IdAplicacao = Aplicacoes.MsProMobile, string VersaoApp = "")
        {
            var lstVideosMiolo = new VideosMiolo();
            var videos = _vimeoRepository.GetByFilters(vm).ToList();
            var chaveamento = new ChaveamentoVimeoMediMiolo();

            foreach (var item in videos)
            {
                VideoMiolo v = new VideoMiolo()
                {
                    ID = item.IDApostila.ToString(),
                    VideoID = item.VideoID,
                    IDApostila = item.IDApostila,
                    CodigoVideo = item.CodigoVideo,
                    BorKey = _vimeoRepository.GetBorKey(item.CodigoVideo, item.Ano, item.IDApostila),
                    Qualidade = vm.Qualidade,
                };

                tblVideo video = _vimeoRepository.GetVideoVimeo(null, Convert.ToInt32(item.VideoID));
                VideoUrlDTO videoRetorno = new VideoUrlDTO
                {
                    Url = string.Empty
                };

                var chaveamentoVimeo = chaveamento.GetChaveamento();
                if (chaveamentoVimeo)
                {
                    if ((int)IdAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON || (int)IdAplicacao == (int)Aplicacoes.AreaRestrita)
                        v.URL = video.txtUrlStreamVimeo;
                    else
                        v.URL = video.txtUrlVimeo;
                }

                if (video.intVimeoID == null)
                {
                    ValidarEnvioEmailVideoDefeituoso(video);
                }
                else if(v.URL == null)
                {
                    v.URL = _vimeoRepository.GetUrlPlataformaVideo(video, chaveamentoVimeo, (int)IdAplicacao);
                    if(v.URL == string.Empty)
                    {
                        ValidarEnvioEmailVideoDefeituoso(video);
                    }
                }
                        
                v.HTTPURL = v.URL;
                v.URLThumb = _vimeoRepository.GetUrlThumb(video, new ChaveamentoVimeoMediMiolo(), VersaoApp);
                v.Links = _vimeoRepository.GetLinksVideoVariasQualidades(video.txtVideoInfo, v.URL);

                lstVideosMiolo.Add(v);
                
            }

            var lstv = new Videos();
            foreach (var valor in lstVideosMiolo)
            {
                if (!lstv.Any(i => i.Url.Equals(valor.HTTPURL)))
                {
                    var video = new Video();
                    video.ID = Convert.ToInt32(valor.ID);
                    video.VideoId = valor.VideoID;
                    video.Url = valor.HTTPURL;
                    video.Thumb = valor.URLThumb;
                    video.Links = valor.Links;
                    video = _vimeoRepository.CreateVideoObject(video, matricula);
                    lstv.Add(video);
                }
            }

            return lstv;
        }

        private void ValidarEnvioEmailVideoDefeituoso(tblVideo video)
        {
            var temRegistro = _vimeoRepository.TemVideoRegistradoParaEnvio(video.intVideoID, video.intVimeoID);
            if (!temRegistro)
                _vimeoRepository.InserirRegistroVideoDefeito(null, null, video.txtName, video.intVideoID, video.intVimeoID);
        }


        private bool IsVideoINTRO(string codigo)
        {
            return codigo.ToUpper().Contains("INTRO_");
        }

        public string GetUrlVideoRevisao(string nomeArquivoXml, Utilidades.VideoThumbSize resolucao, string UrlStream, string UrlMp4, int? videoId, int idAplicacao = (int)Aplicacoes.MEDSOFT, string versaoApp = "")
        {
            string urlRetorno = null;
            string versao = versaoApp != "" ? versaoApp : ConfigurationProvider.Get("ChaveamentoVersaoMinimaMsPro");

            int intAplicacao = idAplicacao == Aplicacoes.MEDSOFT.GetHashCode() ? Aplicacoes.MEDSOFT.GetHashCode() : idAplicacao;

            if (ChaveamentoVimeoAulaDeRevisao())
            {
                if (intAplicacao == (int)Aplicacoes.AreaRestrita)
                {
                    urlRetorno = !string.IsNullOrEmpty(UrlStream) ? UrlStream : this.ObterURLVimeoPorIntVideoID(Convert.ToInt32(videoId), intAplicacao);
                }
                else if (intAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON)
                {
                    Business.VersaoAppPermissaoBusiness versaoAppPermissao = new VersaoAppPermissaoBusiness(new VersaoAppPermissaoEntity());

                    if (versaoAppPermissao.VersaoMenorOuIgual(versao, ConfigurationProvider.Get("ChaveamentoVersaoMinimaMsProDesktop")))
                        urlRetorno = !string.IsNullOrEmpty(UrlMp4) ? UrlMp4 : this.ObterURLVimeoPorIntVideoID(Convert.ToInt32(videoId), intAplicacao);
                    else
                        urlRetorno = !string.IsNullOrEmpty(UrlStream) ? UrlStream : this.ObterURLVimeoPorIntVideoID(Convert.ToInt32(videoId), intAplicacao);
                }
                else //outras aplicações, como Mobile
                {
                    urlRetorno = !string.IsNullOrEmpty(UrlMp4) ? UrlMp4 : this.ObterURLVimeoPorIntVideoID(Convert.ToInt32(videoId), intAplicacao);
                }
            }

            if (string.IsNullOrEmpty(urlRetorno))
            {
                var nomeConcatenado = string.Format("{0}-{1}", nomeArquivoXml.Replace(".xml", string.Empty), Convert.ToInt32(resolucao));
                urlRetorno = Criptografia.GetSignedPlayer(nomeConcatenado);
            }

            return urlRetorno;
        }

        public bool ChaveamentoVimeoAulaDeRevisao()
        {
            string chaveUsarVimeoAuladeRevisao = ConfigurationProvider.Get("Settings:AtivarVimeoAulaDeRevisao");
            return (!string.IsNullOrEmpty(chaveUsarVimeoAuladeRevisao) && Convert.ToBoolean(chaveUsarVimeoAuladeRevisao));
        }

        public string GetUrlThumbVideoRevisao(string nomeArquivoXml, Utilidades.VideoThumbSize resolucao, string UrlThumb, int? vimeoId)
        {
            string urlRetorno = null;

            if (ChaveamentoVimeoAulaDeRevisao())
            {
                if (!string.IsNullOrEmpty(UrlThumb))
                    urlRetorno = UrlThumb;
                else if (vimeoId != null)
                    this.ObterURLThumbVimeo(Convert.ToInt32(vimeoId));
            }

            if (string.IsNullOrEmpty(urlRetorno))
                urlRetorno = string.Format(@"http://content.jwplatform.com/thumbs/{0}-{1}.jpg", nomeArquivoXml.Replace(".xml", string.Empty), Convert.ToInt32(resolucao));

            return urlRetorno;
        }
    }
}