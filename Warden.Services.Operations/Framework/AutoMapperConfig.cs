using AutoMapper;
using Warden.Services.Operations.Domain;
using Warden.Services.Operations.Dto;

namespace Warden.Services.Operations.Framework
{
    public class AutoMapperConfig
    {
        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Operation, OperationDto>();
            });

            return config.CreateMapper();
        }
    }
}