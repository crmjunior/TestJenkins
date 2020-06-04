using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Repository
{
    public class MediaEntity
    {
        public bool IsMediaDataMatrix(int apostila, int media)
        {
            using (var ctx = new DesenvContext())
            {
                var consulta = ctx.tblMedcode_DataMatrix.Any(b => b.intBookID == apostila && b.intDataMatrixID == media);
                return consulta;
            }
        }        

        public Medias GetMediaComentario(int idApostila, int idDataMatrix) {

            

            var lst = new Medias();

            var textos = new List<String>();
            var imagens = (new ImagemEntity()).GetMediaImagesByMedia(idApostila, idDataMatrix);
            var videos = (new VideoEntity()).GetVideosByMedia(idApostila, idDataMatrix);

            textos.ForEach(t => lst.Add(new Media { Texto = t }));

            if (imagens.Any())
            {
                lst.Add(new Media
                {
                    Titulo = imagens[0].Nome,
                    ThumbImagem = imagens[0].Thumb,
                    Imagens = imagens.Select(i => i.Url).ToList()
                });
            }

            videos.ForEach(v => lst.Add(new Media
            {
                Titulo = v.Nome,
                ThumbVideo = v.Thumb,
                Video = v.Url
            }));

            return lst;
        
        }

        public Medias GetMediaComentario(int idQuestao)
        {
            var ctx = new DesenvContext();
            var imagens = new List<Imagem>();
            var videos = new List<Video>();
            var consulta = from q in ctx.tblConcursoQuestoes
                           where q.intQuestaoID == idQuestao
                           select q.txtComentario;

            var lst = new Medias();

            var textos = consulta.ToList();
            using(MiniProfiler.Current.Step("Obtendo imagens do comentário"))
            {
                imagens = (new ImagemEntity()).GetMediaComentarioImages(idQuestao);
            }

            using(MiniProfiler.Current.Step("Obtendo videos do comentario"))
            {
                videos = (new VideoEntity()).GetVideoQuestaoConcurso(idQuestao);
            }
            
            

            textos.ForEach(t => lst.Add(new Media { Texto = t }));

            if (imagens.Any())
            {
                lst.Add(new Media
                {
                    Titulo = imagens[0].Nome,
                    ThumbImagem = imagens[0].Thumb,
                    Imagens = imagens.Select(i => i.Url).ToList()
                });
            }
            
            videos.ForEach(v => lst.Add(new Media
                {
                    Titulo = v.Nome,
                    ThumbVideo = v.Thumb,
                    Video = v.Url
                }));

            return lst;
        }

    }
}