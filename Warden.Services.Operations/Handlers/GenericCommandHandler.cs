using System;
using System.Threading.Tasks;
using RawRabbit;
using Warden.Common.Commands;
using Warden.Services.Operations.Domain;
using Warden.Services.Operations.Services;
using Warden.Services.Operations.Shared.Events;
using Warden.Services.Organizations.Shared.Commands;
using Warden.Services.Users.Shared.Commands;
using Warden.Services.WardenChecks.Shared.Commands;

namespace Warden.Services.Operations.Handlers
{
    public class GenericCommandHandler : ICommandHandler<RequestNewApiKey>, ICommandHandler<CreateApiKey>,
        ICommandHandler<RequestNewOrganization>, ICommandHandler<CreateOrganization>,
        ICommandHandler<RequestNewWarden>, ICommandHandler<CreateWarden>,
        ICommandHandler<RequestProcessWardenCheckResult>, ICommandHandler<ProcessWardenCheckResult>,
        ICommandHandler<SignIn>, ICommandHandler<SignUp>,
        ICommandHandler<SignOut>
    {
        private readonly IBusClient _bus;
        private readonly IOperationService _operationService;

        public GenericCommandHandler(IBusClient bus, IOperationService operationService)
        {
            _bus = bus;
            _operationService = operationService;
        }

        public async Task HandleAsync(RequestNewApiKey command)
            => await CreateForAuthenticatedUserAsync(command);

        public async Task HandleAsync(CreateApiKey command)
            => await ProcessAsync(command);

        public async Task HandleAsync(RequestNewOrganization command)
            => await CreateForAuthenticatedUserAsync(command);

        public async Task HandleAsync(CreateOrganization command)
            => await ProcessAsync(command);

        public async Task HandleAsync(RequestNewWarden command)
            => await CreateForAuthenticatedUserAsync(command);

        public async Task HandleAsync(CreateWarden command)
            => await ProcessAsync(command);

        public async Task HandleAsync(RequestProcessWardenCheckResult command)
            => await CreateForAuthenticatedUserAsync(command);

        public async Task HandleAsync(ProcessWardenCheckResult command)
            => await ProcessAsync(command);

        public async Task HandleAsync(SignIn command)
            => await CreateAsync(command);

        public async Task HandleAsync(SignUp command)
            => await CreateAsync(command);

        public async Task HandleAsync(SignOut command)
            => await CreateForAuthenticatedUserAsync(command);

        private async Task CreateForAuthenticatedUserAsync(IAuthenticatedCommand command)
            => await CreateAsync(command, command.UserId);

        private async Task CreateAsync(ICommand command, string userId = null)
        {
            await _operationService.CreateAsync(command.Request.Id, command.Request.Name, userId,
                command.Request.Origin, command.Request.Resource, command.Request.CreatedAt);
            await _bus.PublishAsync(new OperationCreated(command.Request.Id, command.Request.Name,
                userId, command.Request.Origin, command.Request.Resource, States.Accepted,
                command.Request.CreatedAt));
        }

        private async Task ProcessAsync(IAuthenticatedCommand command)
        {
            await _operationService.ProcessAsync(command.Request.Id);
            await _bus.PublishAsync(new OperationUpdated(command.Request.Id, command.Request.Name,
                command.UserId, States.Processing, string.Empty, string.Empty, DateTime.UtcNow));
        }
    }
}