using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Business
{
    public class AulaAvaliacaoBusiness
    {
        private readonly IAulaAvaliacaoData _aulaAvaliacaoRepository;
        private readonly IVideoData _vimeoRepository;

        public AulaAvaliacaoBusiness(IAulaAvaliacaoData aulaAvaliacaoRepository, IVideoData vimeoRepository )
        {
            _aulaAvaliacaoRepository = aulaAvaliacaoRepository;
            _vimeoRepository = vimeoRepository;
        }

        public List<AulaAvaliacaoDTO> GetAvaliacaoAulaSlides(int matricula, int apostilaId = 0)
        {
            List<AulaAvaliacao> listAulaAvaliacao = new List<AulaAvaliacao>();

            if (apostilaId == 0)
            {
                listAulaAvaliacao = _aulaAvaliacaoRepository.GetAulaAvaliacaoPorAluno(matricula);
            }
            else
            {
                listAulaAvaliacao.Add(_aulaAvaliacaoRepository.GetAulaAvaliacao(matricula, apostilaId));
            }

            var temas = listAulaAvaliacao.SelectMany(x => x.Temas).Select(y => new AulaAvaliacaoTemaDTO { ProfessorId = y.ProfessorID, TemaId = y.TemaID }).ToList();

            var videos = GetVideosMapaMental(temas);

            var avaliacoes = GetAulaAvaliacaoDTO(listAulaAvaliacao, videos);

            return avaliacoes;
        }


        public List<VideoMapaMentalDTO> GetVideosMapaMental(List<AulaAvaliacaoTemaDTO> temas)
        {
            var videosMapaMental = _vimeoRepository.GetVideoMapaMentalIds(temas);

            var chaveamento = new ChaveamentoVimeoMapaMental();

            foreach (var v in videosMapaMental)
            {
                var urls = _vimeoRepository.GetVideoPorVideoID(v.VideoId, chaveamento);
                v.Links = urls.Links.ToList().Where(y => y.Altura > 0)
                    .Select(x => new AulaAvaliacaoConteudoDTO 
                        { 
                            Altura = x.Altura, 
                            Largura = x.Largura, 
                            Link = x.Link, 
                            Qualidade = x.Qualidade,
                            videosUrl = new string[] {x.Link}
                        }
                    ).ToList();
            }
            return videosMapaMental;
        }

        public List<AulaAvaliacaoDTO> GetAulaAvaliacaoDTO(List<AulaAvaliacao> aulaAvaliacaos, List<VideoMapaMentalDTO> videos)
        {
            var dtos = aulaAvaliacaos.Select(a => new AulaAvaliacaoDTO
            {
                ID = a.ID,
                Ano = a.Ano,
                IdApostila = a.IdApostila,
                Aprovada = a.Aprovada,
                IdGrandeArea = a.IdGrandeArea,
                IdProduto = a.IdProduto,
                IdSubEspecialidade = a.IdSubEspecialidade,
                NomeCompleto = a.NomeCompleto,
                Titulo = a.Titulo,
                Temas = GetAulaAvaliacaoTemaDTO(a.Temas, videos)
            }).ToList();

            return dtos;
        }

        public List<AulaAvaliacaoTemaDTO> GetAulaAvaliacaoTemaDTO(List<AulaTema> temas, List<VideoMapaMentalDTO> videos)
        {
            var temasDTO = temas.Select(
                a => new AulaAvaliacaoTemaDTO
                {
                    ID = a.AulaID,
                    TemaId = a.TemaID,
                    AvaliacaoId = a.AvaliacaoID,
                    Data = a.Data,
                    IsAvaliado = a.IsAvaliado,
                    Nome = a.Nome,
                    PodeAvaliar = a.PodeAvaliar,
                    ProfessorId = a.ProfessorID,
                    ProfessorFoto = a.ProfessorFoto,
                    ProfessorNome = a.ProfessorNome,
                    Rotulo = a.Rotulo,
                    Slides = GetAulaAvaliacaoSlideDTO(a.ProfessorID, a.TemaID, a.Slides, videos)
                }).ToList();

            return temasDTO;
        }

        public List<AulaAvaliacaoSlideDTO> GetAulaAvaliacaoSlideDTO(int professorId, int temaId, List<string> links, List<VideoMapaMentalDTO> videos)
        {
            List<AulaAvaliacaoSlideDTO> aulaAvaliacaoSlideDTOs = new List<AulaAvaliacaoSlideDTO>();

            var videoDoTema = videos.Where(x => x.TemaId == temaId && x.ProfessorId == professorId)
                .Select(a => new AulaAvaliacaoSlideDTO
                {
                    Tipo = (int)ETipoMidiaConteudoAvaliacao.Video,
                    Conteudo = a.Links
                }).ToList();

            var slides = links.Select(l =>
                    new AulaAvaliacaoSlideDTO
                    {
                        Tipo = (int)ETipoMidiaConteudoAvaliacao.Imagem,
                        Conteudo = new List<AulaAvaliacaoConteudoDTO>
                        {
                             new AulaAvaliacaoConteudoDTO
                             {
                                 Link = l
                             }
                        }
                    }).ToList();

            if (videoDoTema.Any())
                aulaAvaliacaoSlideDTOs.AddRange(videoDoTema);

            aulaAvaliacaoSlideDTOs.AddRange(slides);

           return aulaAvaliacaoSlideDTOs;
        }


        public enum ETipoMidiaConteudoAvaliacao
        {
            Imagem = 1,
            Video = 2
        }
    }
}