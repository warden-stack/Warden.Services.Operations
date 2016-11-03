﻿using Warden.Common.Commands.ApiKeys;
using Warden.Common.Commands.Organizations;
using Warden.Common.Commands.WardenChecks;
using Warden.Common.Commands.Wardens;
using Warden.Common.Events.ApiKeys;
using Warden.Common.Events.Features;
using Warden.Common.Events.Organizations;
using Warden.Common.Events.Wardens;
using Warden.Common.Host;
using Warden.Services.Operations.Framework;

namespace Warden.Services.Operations
{
    public class Program
    {
        //TODO: Fix subscription to commands by this and other services.
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 5090)
                .UseAutofac(Bootstrapper.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToCommand<RequestNewApiKey>()
                .SubscribeToCommand<CreateApiKey>()
                .SubscribeToCommand<RequestNewOrganization>()
                .SubscribeToCommand<CreateOrganization>()
                .SubscribeToCommand<RequestNewWarden>()
                .SubscribeToCommand<CreateWarden>()
                .SubscribeToCommand<RequestWardenCheckResultProcessing>()
                .SubscribeToCommand<ProcessWardenCheckResult>()
                .SubscribeToEvent<ApiKeyCreated>()
                .SubscribeToEvent<OrganizationCreated>()
                .SubscribeToEvent<WardenCreated>()
                .SubscribeToEvent<WardenCheckResultProcessed>()
                .SubscribeToEvent<FeatureRejected>()
                .Build()
                .Run();
        }
    }
}
