using System;
using System.Collections.Generic;
using MedCore_API.Academico;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using System.Linq;

namespace MedCore_DataAccess.Repository
{
    public class VideoMioloEntity
    {
        private int _idAplicacao;
        private string _versaoApp;

        //private static bool _chaveamentoVimeoApostila = Convert.ToBoolean(ConfigurationManager.AppSettings["AtivarVimeoApostila"] ?? "false");
        private Contracts.Business.IChaveamentoVimeo _chaveamentoVimeoApostila;
        public void setChaveamentoVimeoApostila(Contracts.Business.IChaveamentoVimeo chaveamento) {
            _chaveamentoVimeoApostila = chaveamento;
        }

        public void setVersaoAplicacao(int idaplicacao, string versaoapp)
        { 
            _idAplicacao = idaplicacao;
            _versaoApp = versaoapp;

        }

        public List<VideoMiolo> GetByFilters(VideoMiolo registro)
        {
            var ctx = new DesenvContext();
            var lstVideosMiolo = new VideosMiolo();
            var videoEntity = new VideoEntity();

            var consulta = from a in ctx.tblBooks
                           join b in ctx.tblVideo_Book on a.intBookID equals b.intBookID
                           join c in ctx.tblProducts on a.intBookID equals c.intProductID
                           orderby a.intYear ascending

                           select new
                           {
                               IDApostila = b.intBookID,
                               CodigoVideo = b.txtVideoCode,
                               Year = a.intYear,
                               IdVideo = b.intVideoID
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

            foreach (var item in consulta)
            {
                VideoMiolo v = new VideoMiolo()
                {
                    ID = item.IDApostila.ToString(),
                    VideoID = item.IdVideo,
                    IDApostila = item.IDApostila,
                    CodigoVideo = item.CodigoVideo,
                    BorKey = Criptografia.GetBorKey(item.CodigoVideo, item.Year, item.IDApostila),
                    Qualidade = registro.Qualidade,
                };

                if (!String.IsNullOrEmpty(v.Xml))
                {
                                         

                    tblVideo video = videoEntity.GetVideoVimeo(null, Convert.ToInt32(item.IdVideo));

                    v.URL = videoEntity.GetUrlVideoPorVideoID(Convert.ToInt32(item.IdVideo), _chaveamentoVimeoApostila, _idAplicacao, _versaoApp);
                    v.URLThumb = videoEntity.GetUrlThumb(video, _chaveamentoVimeoApostila, _versaoApp);                    
                    v.HTTPURL = v.URL;
                    v.Links = videoEntity.GetLinksVideoVariasQualidades(video.txtVideoInfo, v.URL);

                    lstVideosMiolo.Add(v);
                }
            }

            return lstVideosMiolo;
        }
    }
}