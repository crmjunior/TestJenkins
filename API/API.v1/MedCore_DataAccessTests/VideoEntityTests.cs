using System;
using System.Configuration;
using System.IO;
using System.Net;
using MedCore_API.Academico;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class VideoEntityTests
    {
        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetVideo()
        {
            var urlVideo = new MednetEntity().GetVideo(113616);
            Assert.IsNotNull(urlVideo);
        }

        [TestMethod]
        public void GetVideoRevisao_MsProDesktopVersaoMaiorQueChaveVersao_RetornaStream()
        {
            string url = new MednetEntity().GetVideo(380115, (int)Aplicacoes.MEDSOFT_PRO_ELECTRON, "1.6.1");
            Assert.AreEqual(true, url.Contains(".m3u8"));
        }

        [TestMethod]
        public void GetVideoRevisao_PassandoAulaRevisaoNEF5_RetornarURLPreenchida()
        {
            var lista = new VideoEntity().GetVideoByCode("NEF 5");
            foreach (var item in lista)
            {
                string url = new MednetEntity().GetVideo(item, (int)Aplicacoes.MsProMobile, "3.0.37");
                Assert.AreNotEqual(string.Empty, url);
            }
        }

        [TestMethod]
        [Ignore]
        [TestCategory("Video_Vimeo")]
        public void RetornaVideoVimeoThumbPorIntVimeoId()
        {
            VideoEntity entity = new VideoEntity();
            VideoBusiness videoBusiness = new VideoBusiness(entity);
            if (videoBusiness.ChaveamentoVimeo())
            {
                var idVideoValido = RetornaIdVimeoValido();
                bool videoVimeo = false;

                var result = videoBusiness.ObterURLThumbVimeo(idVideoValido);
                if (result != null)
                    if (result.IndexOf("https://i.vimeocdn.com") >= 0)
                    {
                        videoVimeo = true;
                    }
                Assert.AreEqual(videoVimeo, true);
            }
        }
        [TestMethod]
        [Ignore]
        [TestCategory("Video_Vimeo")]
        public void RetornaVideoVimeoThumbPorstringUrlInvalida()
        {
            VideoEntity entity = new VideoEntity();
            VideoBusiness videoBusiness = new VideoBusiness(entity);
            if (videoBusiness.ChaveamentoVimeo())
            {
                var idVideoValido = RetornaIntVideoIdComVimeoIdValido();
                var result = videoBusiness.ObterURLThumbVimeo(idVideoValido);
                bool videoVimeo = false;
                if (result.IndexOf("https://i.vimeocdn.com") >= 0)
                {
                    videoVimeo = true;
                }
                Assert.AreEqual(videoVimeo, true);
            }
        }
        
        private int RetornaIdVimeoValido()
        {
            using (var ctx = new AcademicoContext())
            {
                var consulta = (from v in ctx.tblVideo
                                where v.intVimeoID != null
                                select new { v.intVimeoID }).Take(10).ToList();
                int idvimeo = 0;
                foreach (var item in consulta)
                {
                    Int32.TryParse(item.intVimeoID.ToString(), out idvimeo);
                    if (ValidaVideoExistenteVimeo(idvimeo))
                    {
                        return idvimeo;
                    }
                }
                return idvimeo;
            }
        }

        public bool ValidaVideoExistenteVimeo(int intVimeoId)
        {
            string token = "Bearer " + ConfigurationManager.AppSettings["VimeoToken"];
            string url = @"https://api.vimeo.com/videos/" + intVimeoId + "?fields=link";
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)Constants.TLS12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.Headers.Add("Authorization", token);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private int RetornaIntVideoIdComVimeoIdValido()
        {
            using (var ctx = new AcademicoContext())
            {
                var consulta = (from v in ctx.tblVideo
                                where v.intVimeoID != null
                                select new { v.intVideoID, v.intVimeoID }).Take(10).ToList();
                int intVideoId = 0;
                int intVimeoId = 0;
                foreach (var item in consulta)
                {
                    Int32.TryParse(item.intVimeoID.ToString(), out intVimeoId);
                    Int32.TryParse(item.intVideoID.ToString(), out intVideoId);
                    if (ValidaVideoExistenteVimeo(intVimeoId))
                    {
                        return intVideoId;
                    }
                }
                return intVideoId;
            }
        }
    }
}