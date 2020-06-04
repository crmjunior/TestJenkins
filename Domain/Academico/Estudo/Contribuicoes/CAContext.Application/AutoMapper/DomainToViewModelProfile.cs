using AutoMapper;
using CAContext.Application.ViewModels;
using CAContext.Domain.Entities;

namespace CAContext.Application.AutoMapper
{
    public class DomainToViewModelProfile : Profile
    {
        public DomainToViewModelProfile()
        {
            CreateMap<Contribuicao, ContribuicaoViewModel>();
            CreateMap<ContribuicaoArquivo, ContribuicaoArquivoViewModel>();
        }
        
    }
}