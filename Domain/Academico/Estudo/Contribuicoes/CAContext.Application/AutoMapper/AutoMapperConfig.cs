using AutoMapper;

namespace CAContext.Application.AutoMapper
{
    public class AutoMapperConfig
    {
        public static IMapper Register()
        {
            var configuration = new MapperConfiguration(config => {
                config.AddProfile(new DomainToViewModelProfile());
                config.AddProfile(new ViewModelToDomainProfile());
            });
            return configuration.CreateMapper();
        }
    }
}