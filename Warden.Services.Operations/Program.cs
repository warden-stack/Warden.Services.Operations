using Warden.Common.Host;
using Warden.Services.Operations.Framework;
using Warden.Services.Features.Shared.Events;
using Warden.Services.Organizations.Shared.Commands;
using Warden.Services.Organizations.Shared.Events;
using Warden.Services.Users.Shared.Commands;
using Warden.Services.Users.Shared.Events;
using Warden.Services.WardenChecks.Shared.Commands;
using Warden.Services.WardenChecks.Shared.Events;

namespace Warden.Services.Operations
{
    public class Program
    {
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
                .SubscribeToCommand<RequestProcessWardenCheckResult>()
                .SubscribeToCommand<ProcessWardenCheckResult>()
                .SubscribeToCommand<SignIn>()
                .SubscribeToCommand<SignUp>()
                .SubscribeToCommand<SignOut>()
                .SubscribeToEvent<ApiKeyCreated>()
                .SubscribeToEvent<OrganizationCreated>()
                .SubscribeToEvent<WardenCreated>()
                .SubscribeToEvent<WardenCheckResultProcessed>()
                .SubscribeToEvent<FeatureRejected>()
                .SubscribeToEvent<SignedIn>()
                .SubscribeToEvent<SignedUp>()
                .SubscribeToEvent<SignedOut>()
                .SubscribeToEvent<SignInRejected>()
                .SubscribeToEvent<SignUpRejected>()
                .SubscribeToEvent<SignOutRejected>()
                .Build()
                .Run();
        }
    }
}
