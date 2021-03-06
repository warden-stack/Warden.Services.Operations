﻿using System;
using System.Threading.Tasks;
using Humanizer;
using RawRabbit;
using Warden.Messages.Events;
using Warden.Services.Operations.Domain;
using Warden.Services.Operations.Services;
using Warden.Messages.Events.Features;
using Warden.Messages.Events.Operations;
using Warden.Messages.Events.Organizations;
using Warden.Messages.Events.Users;
using Warden.Messages.Events.WardenChecks;

namespace Warden.Services.Operations.Handlers
{
    public class GenericEventHandler : IEventHandler<ApiKeyCreated>, IEventHandler<CreateApiKeyRejected>, 
        IEventHandler<OrganizationCreated>, IEventHandler<CreateOrganizationRejected>, 
        IEventHandler<WardenCreated>, IEventHandler<CreateWardenRejected>, 
        IEventHandler<WardenCheckResultProcessed>,
        IEventHandler<FeatureRejected>, IEventHandler<SignedUp>,
        IEventHandler<SignedIn>, IEventHandler<SignedOut>,
        IEventHandler<SignUpRejected>, IEventHandler<SignInRejected>,
        IEventHandler<SignOutRejected>,
        IEventHandler<UsernameChanged>, IEventHandler<ChangeUsernameRejected>
    {
        private readonly IBusClient _bus;
        private readonly IOperationService _operationService;

        public GenericEventHandler(IBusClient bus, IOperationService operationService)
        {
            _bus = bus;
            _operationService = operationService;
        }

        public async Task HandleAsync(ApiKeyCreated @event)
            => await CompleteForAuthenticatedUserAsync(@event);

        public async Task HandleAsync(CreateApiKeyRejected @event)
            => await RejectAsync(@event);

        public async Task HandleAsync(WardenCreated @event)
            => await CompleteForAuthenticatedUserAsync(@event);
         public async Task HandleAsync(CreateWardenRejected @event)
            => await RejectAsync(@event);           

        public async Task HandleAsync(OrganizationCreated @event)
            => await CompleteForAuthenticatedUserAsync(@event);

        public async Task HandleAsync(CreateOrganizationRejected @event)
            => await RejectAsync(@event);

        public async Task HandleAsync(WardenCheckResultProcessed @event)
            => await CompleteForAuthenticatedUserAsync(@event);

        public async Task HandleAsync(FeatureRejected @event)
            => await RejectAsync(@event);

        public async Task HandleAsync(SignedUp @event)
            => await CompleteAsync(@event);

        public async Task HandleAsync(SignedIn @event)
            => await CompleteAsync(@event);

        public async Task HandleAsync(SignedOut @event)
            => await CompleteAsync(@event);

        public async Task HandleAsync(SignUpRejected @event)
            => await RejectAsync(@event);

        public async Task HandleAsync(SignInRejected @event)
            => await RejectAsync(@event);

        public async Task HandleAsync(SignOutRejected @event)
            => await RejectAsync(@event);

        public async Task HandleAsync(UsernameChanged @event)
            => await CompleteForAuthenticatedUserAsync(@event);

        public async Task HandleAsync(ChangeUsernameRejected @event)
            => await RejectAsync(@event);

        private async Task CompleteForAuthenticatedUserAsync(IAuthenticatedEvent @event)
            => await CompleteAsync(@event, @event.UserId);

        private async Task CompleteAsync(IEvent @event, string userId = null)
        {
            await _operationService.CompleteAsync(@event.RequestId);
            await _bus.PublishAsync(new OperationUpdated(@event.RequestId, userId, 
                @event.GetType().Name.Humanize(LetterCasing.LowerCase).Underscore(),
                States.Completed, OperationCodes.Success, string.Empty, DateTime.UtcNow));
        }

        private async Task RejectAsync(IRejectedEvent @event)
        {
            await _operationService.RejectAsync(@event.RequestId, @event.Code, @event.Reason);
            await _bus.PublishAsync(new OperationUpdated(@event.RequestId, @event.UserId,
                @event.GetType().Name.Humanize(LetterCasing.LowerCase).Underscore(),
                States.Rejected, @event.Code, @event.Reason, DateTime.UtcNow));
        }
    }
}