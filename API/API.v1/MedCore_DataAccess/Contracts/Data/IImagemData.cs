using System.Collections.Generic;
using System.IO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IImagemData
    {
         string GetConcursoBase64(int imagemID);
         List<int> GetImagensQuestaoConcurso(int questaoID);
         List<Imagem> GetConcursoImagemComentario(int questaoID);

         List<int> GetImagensQuestaoSimulado(int questaoID);

         Stream GetConcurso(int imagemID);
    }
}