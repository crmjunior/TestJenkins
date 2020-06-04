using System;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Model;
using System.Linq;
using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using Microsoft.EntityFrameworkCore;
using MedCore_API.Academico;
using System.IO;
using MedCore_DataAccess.Util;
using StackExchange.Profiling;
using System.Net;

namespace MedCore_DataAccess.Repository
{
    public class ImagemEntity : IImagemData
    {

        public List<Imagem> GetMediaComentarioImages(int idQuestao)
        {
            var ctx = new DesenvContext();
            var consulta = from q in ctx.tblConcursoQuestoes
                           join i in ctx.tblQuestoesConcursoImagem_Comentario on q.intQuestaoID equals i.intQuestaoID
                           where q.intQuestaoID == idQuestao
                           select new Imagem { ID = i.intImagemComentarioID, Nome = i.txtName };

            var imagens = consulta.ToList();
            imagens.ForEach(i =>
                            {
                                i.Thumb = Constants.URLCOMENTARIOIMAGEM.Replace("IDCOMENTARIOIMAGEM", i.ID.ToString()).Replace("FORMATO", "cellthumb");
                                i.Url = Constants.URLCOMENTARIOIMAGEM.Replace("IDCOMENTARIOIMAGEM", i.ID.ToString()).Replace("FORMATO", "cellimage"); ;
                            });

            return imagens;

        }
        public string GetConcursoBase64(Int32 imagemID)
        {
            using(MiniProfiler.Current.Step("Obter imagem do sconcurso em Base64"))
            {
                var ctx = new DesenvContext();

                var image = (from img in ctx.tblQuestaoConcurso_Imagem
                                where img.intID == imagemID
                                select img.imgImagemOtimizada)
                    .FirstOrDefault();

                return image == null
                    ? String.Empty
                    : Convert.ToBase64String(image);
            }
        }

        public List<Imagem> GetConcursoImagemComentario(int questaoID)
        {
            var lstComentarioImagens = new List<Imagem>();
            var ctx = new DesenvContext();
            
            var retornoProc = ctx.Set<msp_Medsoft_SelectImagensComentProfessor_Result>().FromSqlRaw("msp_Medsoft_SelectImagensComentProfessor @intQuestaoID = {0}", questaoID).ToList();

            foreach (var valor in retornoProc)
            {
                var img = new Imagem() { Url = valor.imgUrl };
                lstComentarioImagens.Add(img);
            }

            return lstComentarioImagens;
        }

        public List<Int32> GetImagensQuestaoConcurso(int questaoID)
        {
            var ctx = new DesenvContext();

            var lista = (from img in ctx.tblQuestaoConcurso_Imagem
                         where img.intQuestaoID == questaoID
                         select img.intID).ToList();

            return lista;
        }

        public List<Int32> GetImagensQuestaoSimulado(Int32 questaoID)
        {
            using (var ctx = new AcademicoContext())
            {
                List<Int32> questaoImagensIds = (from img in ctx.tblQuestao_Imagem
                                                 where img.intQuestaoID == questaoID
                                                 select img.intID).ToList();
                return questaoImagensIds;
            }
        }

        public Stream GetConcurso(int imagemID)
        {
            var ctx = new DesenvContext();

            Byte[] image = (from img in ctx.tblQuestaoConcurso_Imagem
                            where img.intID == imagemID
                            select img.imgImagemOtimizada).FirstOrDefault();

            Stream imageStream = new MemoryStream(image);

            return imageStream;
        }

        public List<Int32> GetImagensQuestaoConcursoCache(int questaoID)
        {
            try
            {
                var key = String.Format("{0}:{1}", RedisCacheConstants.Questao.KeyGetImagensQuestaoConcurso, questaoID);

                if (RedisCacheManager.CannotCache(RedisCacheConstants.Questao.KeyGetImagensQuestaoConcurso))
                    return GetImagensQuestaoConcurso(questaoID);

                if (RedisCacheManager.HasAny(key))
                    return RedisCacheManager.GetItemObject<List<Int32>>(key);
                else
                {
                    var lista = GetImagensQuestaoConcurso(questaoID);
                    RedisCacheManager.SetItemObject(key, lista);
                    return lista;
                }
            }
            catch (Exception)
            {
                return GetImagensQuestaoConcurso(questaoID);
            }
        }

        public string GetSimuladoBase64(Int32 imagemID)
        {
            using(MiniProfiler.Current.Step("Obtendo simulado base64"))
            {
                using (var ctx = new AcademicoContext())
                {
                    Byte[] image = (from img in ctx.tblQuestao_Imagem
                                    where img.intID == imagemID
                                    select img.imgImagemOtimizada).FirstOrDefault();

                    return Convert.ToBase64String(image);
                }
            }
        }

        public Stream GetSlideAulaImageMsPro(int idSlideAula, string formato)
        {
            using(MiniProfiler.Current.Step("Obtendo imagens de slides de aula"))
            {
                try
                {
                    var ctx = new DesenvContext();
                    var sGuid = (from i in ctx.tblRevisaoAula_Slides
                                where i.intSlideAulaID == idSlideAula
                                select i.guidSlide).First();
                    var g = sGuid.ToString().ToUpper();
                    var r = new Util.AmazonManager(Constants.SlidesAulaAmazon_key, Constants.SlidesAulaAmazon_secret).GetUrlFile(Constants.SlidesAulaAmazon_bucket, string.Concat(g, ".png"));
                    HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(r);
                    HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();
                    return aResponse.GetResponseStream();
                }
                catch
                {
                    throw;
                }
            }
        }       

        public List<Imagem> GetMediaImagesByMedia(int idApostila, int idDataMatrix)
        {
            /* var ctx = new materiaisDireitoEntities(true);
             var consulta = (from q in ctx.tblMedcode_DataMatrix
                             join i in ctx.tblQuestoesConcursoImagem_Comentario on q.intMediaID equals i.intQuestaoID
                             where q.DataMatrixID == idDataMatrix
                             && q.tblMedcode_DataMatrix_Tipo.intMediaTipoID == (int)Media.Tipo.ImagemComentario
                             select new Imagem { ID = i.intImagemComentarioID, Nome = i.txtName }).Union(
                             from q in ctx.tblMedcode_DataMatrix
                             join i in ctx.tblBook_Imagens on q.intMediaID equals i.intBookImagemID
                             where q.DataMatrixID == idDataMatrix
                             && q.tblMedcode_DataMatrix_Tipo.intMediaTipoID == (int)Media.Tipo.ImagemComentario
                             select new Imagem { ID = i.intBookImagemID, Nome = i.txtDescription }
                             );

             var imagens = consulta.ToList();
             imagens.ForEach(i =>
             {
                 i.Thumb = Constants.URLCOMENTARIOIMAGEM.Replace("IDCOMENTARIOIMAGEM", i.ID.ToString()).Replace("FORMATO", "cellthumb");
                 i.Url = Constants.URLCOMENTARIOIMAGEM.Replace("IDCOMENTARIOIMAGEM", i.ID.ToString()).Replace("FORMATO", "cellimage"); ;
             });
             */
            return GetMediaComentarioImages(idApostila, idDataMatrix).Union(GetMediaApostilaImages(idApostila, idDataMatrix)).ToList();

        }

        public IEnumerable<Imagem> GetMediaComentarioImages(int idApostila, int idDataMatrix)
        {
            var ctx = new DesenvContext();
            var consulta = (from q in ctx.tblMedcode_DataMatrix
                            join d in ctx.tblMedcode_DataMatrix_Tipo on q.intMediaTipo equals d.intMediaTipoID
                            join i in ctx.tblQuestoesConcursoImagem_Comentario on q.intMediaID equals i.intQuestaoID
                            where q.intDataMatrixID == idDataMatrix
                                  && d.intMediaTipoID == (int)Media.Tipo.ImagemComentario
                            select new Imagem { ID = i.intImagemComentarioID, Nome = i.txtName });

            var imagens = consulta.ToList();
            imagens.ForEach(i =>
                            {
                                i.Thumb = Constants.URLCOMENTARIOIMAGEM.Replace("IDCOMENTARIOIMAGEM", i.ID.ToString()).Replace("FORMATO", "cellthumb");
                                i.Url = Constants.URLCOMENTARIOIMAGEM.Replace("IDCOMENTARIOIMAGEM", i.ID.ToString()).Replace("FORMATO", "cellimage");
                            });

            return imagens;
        }

        public IEnumerable<Imagem> GetMediaApostilaImages(int idApostila, int idDataMatrix)
        {
            var ctx = new DesenvContext();
            var consulta = (from q in ctx.tblMedcode_DataMatrix
                            join mtp in ctx.tblMedcode_DataMatrix_Tipo on q.intMediaTipo equals mtp.intMediaTipoID
                            join i in ctx.tblBook_Imagens on q.intMediaID equals i.intBookImagemID
                            where q.intDataMatrixID == idDataMatrix
                                  && mtp.intMediaTipoID == (int)Media.Tipo.ImagemApostila
                            select new Imagem { ID = i.intBookImagemID, Nome = i.txtDescription.Trim() }
            );

            var imagens = consulta.ToList();
            imagens.ForEach(i =>
                            {
                                i.Thumb = Constants.URLMEDIAIMAGEM.Replace("IDMEDIAIMAGEM", i.ID.ToString()).Replace("FORMATO", "cellthumb");
                                i.Url = Constants.URLMEDIAIMAGEM.Replace("IDMEDIAIMAGEM", i.ID.ToString()).Replace("FORMATO", "cellimage");

                            });

            return imagens;
        }
    }
}