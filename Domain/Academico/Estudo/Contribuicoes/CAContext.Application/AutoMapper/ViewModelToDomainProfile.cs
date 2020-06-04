using AutoMapper;
using CAContext.Application.ViewModels;
using CAContext.Domain.Entities;

namespace CAContext.Application.AutoMapper
{
    public class ViewModelToDomainProfile : Profile
    {
        public ViewModelToDomainProfile()
        {
            CreateMap<ContribuicaoViewModel, Contribuicao>();
            CreateMap<ContribuicaoArquivoViewModel, ContribuicaoArquivo>();
        }
        
    }
}