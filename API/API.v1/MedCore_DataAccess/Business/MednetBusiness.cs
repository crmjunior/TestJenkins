using System;
using System.Threading.Tasks;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Business
{
    public class MednetBusiness
    {
        private readonly IMednetData _mednetRepository;
        public MednetBusiness(IMednetData mednetRepository)
        {
            _mednetRepository = mednetRepository;
        }

        private const int POSICAOINICIAL = 0;
        private const int POSICAOMINIMA = 5;
        private const int SEGUNDOSRETORNO = 5;

        public VideoUrlDTO GetVideoUrlComUltimaPosicao(int idVideo, int idAplicacao, string versaoApp, int clientId)
        {
            using(MiniProfiler.Current.Step("Obtendo url do video com útima posição"))
            {
                VideoUrlDTO retorno = new VideoUrlDTO();

                retorno = _mednetRepository.GetVideoInfo(idVideo, idAplicacao, versaoApp);

                retorno.UltimaPosicaoAluno = UltimaPosicaoVideoAulaRevisaoAluno(clientId, idVideo, retorno.Duracao);

                if (idAplicacao == (int)Aplicacoes.MsProMobile)
                {
                    retorno.Url += string.Concat("#t=",retorno.UltimaPosicaoAluno);
                }

                return retorno;
            }
        }

        private int UltimaPosicaoVideoAulaRevisaoAluno(int clientId, int idVideo, int? duracao)
        {
            var posicaoSegundos = _mednetRepository.GetUltimaPosicaoVideoAulaRevisaoAluno(clientId, idVideo);

            if (posicaoSegundos < POSICAOMINIMA || posicaoSegundos == duracao)
            {
                return POSICAOINICIAL;
            }
            else
            {
                return posicaoSegundos - SEGUNDOSRETORNO;
            }
        }

        public int SetProgressoVideoAsync(ProgressoVideo progresso)
        {
            Task.Factory.StartNew(() => SetProgressoVideo(progresso));
            return 1;
        }

        public int SetProgressoVideo(ProgressoVideo progresso)
        {

            if (progresso.DuracaoVideo == default(double))
            {
                progresso.ProgressoSegundo = GetProgressoVideoDevido(progresso);
            }
            else
            {
                progresso.ProgressoSegundo = progresso.ProgressoSegundo > (int)Math.Ceiling(progresso.DuracaoVideo) ? (int)Math.Ceiling(progresso.DuracaoVideo) : progresso.ProgressoSegundo;
            }
            return _mednetRepository.SetProgressoVideo(progresso);
        }

        public int GetProgressoVideoDevido(ProgressoVideo progresso)
        {
            decimal duracao = 0;
            try
            {
                if (RedisCacheManager.CannotCache(RedisCacheConstants.Video.KeyDuracaoVideoRevisaoAula))
                {
                    duracao = _mednetRepository.GetVideoDuracaoPorIdRevisaoAula(progresso.IdRevisaoAula);
                }
                else
                {
                    var key = String.Format("{0}:{1}", RedisCacheConstants.Video.KeyDuracaoVideoRevisaoAula, progresso.IdRevisaoAula);
                    duracao = RedisCacheManager.GetItemObject<decimal>(key);
                    if (duracao == 0)
                    {
                        duracao = _mednetRepository.GetVideoDuracaoPorIdRevisaoAula(progresso.IdRevisaoAula);
                        RedisCacheManager.SetItemObject(key, duracao);
                    }
                }
            }
            catch
            {
                duracao = _mednetRepository.GetVideoDuracaoPorIdRevisaoAula(progresso.IdRevisaoAula);
            }
            if (progresso.ProgressoSegundo > (int)Math.Ceiling(duracao)) progresso.ProgressoSegundo = (int)Math.Ceiling(duracao);
                return progresso.ProgressoSegundo;
        }
    }
}