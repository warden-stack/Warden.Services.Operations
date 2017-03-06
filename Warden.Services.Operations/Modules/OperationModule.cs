using AutoMapper;
using Warden.Services.Operations.Domain;
using Warden.Services.Operations.Queries;
using Warden.Services.Operations.Services;
using Warden.Services.Operations.Dto;

namespace Warden.Services.Operations.Modules
{
    public class OperationModule : ModuleBase
    {
        public OperationModule(IOperationService operationService, IMapper mapper) 
            : base(mapper, "operations")
        {
            Get("{requestId}", args => Fetch<GetOperation, Operation>
                (async x => await operationService.GetAsync(x.RequestId))
                .MapTo<OperationDto>()
                .HandleAsync());
        }
    }
}