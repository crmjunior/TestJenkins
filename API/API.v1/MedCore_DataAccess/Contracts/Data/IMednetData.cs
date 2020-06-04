using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IMednetData
    {
         List<ProgressoSemana> GetProgressoAulas(int matricula, int ano, int produto);

         TemasApostila GetVideosRevalida(int matricula, int idAplicacao = (int)MedCore_DataAccess.Contracts.Enums.Aplicacoes.MsProMobile);

         VideoUrlDTO GetVideoInfo(int idVideo, int idAplicacao = (int)Aplicacoes.MsProMobile, string versaoApp = "");

         int GetUltimaPosicaoVideoAulaRevisaoAluno(int clientId, int videoId);

         int SetProgressoVideo(ProgressoVideo progresso);

         decimal GetVideoDuracaoPorIdRevisaoAula(int IdRevisaoAula);
        TemaApostila CalculaProgressosVideosTemaProva(TemaApostila videoTema, int matricula);

        TemaApostila CalculaProgressosVideosTemaRevisao(TemaApostila videoTema, int matricula);
        TemasApostila GetTemasVideos(int idProduto, int matricula, int intProfessor, int intAula, bool isAdmin, int idTema = 0, int idApostila = 0, ETipoVideo tipoVideo = ETipoVideo.Revisao, int idAplicacao = (int)Aplicacoes.MEDSOFT, string versaoApp = "");
    }
}