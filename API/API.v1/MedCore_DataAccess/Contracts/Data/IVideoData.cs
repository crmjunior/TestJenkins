using System.Collections.Generic;
using MedCore_API.Academico;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IVideoData
    {
        Videos GetVideosApostilaCodigoJson(string codigo, string matricula = null, Aplicacoes IdAplicacao = Aplicacoes.MsProMobile, string versao = "");

        VideoDTO ConvertToVideoDTO(Video video);

        List<VideoMiolo> GetByFilters(VideoMiolo registro);

        string GetBorKey(string txtVideoCode, int? intYear, int intBookID);

        tblVideo GetVideoVimeo(int? intVimeoID, int intVideoID = 0);

        string GetUrlPlataformaVideo(tblVideo video, bool chaveamentoVimeo, int idAplicacao);

        string GetUrlThumb(tblVideo video, Contracts.Business.IChaveamentoVimeo ObjChaveamentoVimeo, string versaoApp = "");

        VideoQualidadeDTO[] GetLinksVideoVariasQualidades(string videoInfo, string urlPadrao);

        Video CreateVideoObject(Video miolo, string matricula);

        Video GetVideoIntro(int apostilaID = 0, string guid = "", string param = "");

        int InserirRegistroVideoDefeito(jsonVimeo e, string url, string nome, int videoId, int? vimeoId);

        bool TemVideoRegistradoParaEnvio(int intVideoId, int? intVimeoId);
        List<Video> GetVideoQuestaoConcurso(int idQuestao, int idAplicacao = 5, string appVersion = "");
        int ObterVimeoID(int intVideoID);

        Videos GetVideoQuestaoSimulado(int QuestaoID, int idAplicacao = 0, string versaoApp = "");

        int GetDuracao(int idQuestao);

        List<VideoMapaMentalDTO> GetVideoMapaMentalIds(List<AulaAvaliacaoTemaDTO> temas);

        VideoUrlDTO GetVideoPorVideoID(int idVideo, Contracts.Business.IChaveamentoVimeo ObjChaveamentoVimeo, int idAplicacao = (int)Aplicacoes.MsProMobile, string versaoApp = "");
    }
}