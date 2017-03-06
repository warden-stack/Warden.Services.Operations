using Warden.Common.Host;
using Warden.Services.Operations.Framework;
using Warden.Messages.Events.Features;
using Warden.Messages.Commands.Organizations;
using Warden.Messages.Events.Organizations;
using Warden.Messages.Commands.Users;
using Warden.Messages.Events.Users;
using Warden.Messages.Commands.WardenChecks;
using Warden.Messages.Events.WardenChecks;

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
                .SubscribeToCommand<ChangeUsername>()
                .SubscribeToEvent<ApiKeyCreated>()
                .SubscribeToEvent<CreateApiKeyRejected>()
                .SubscribeToEvent<OrganizationCreated>()
                .SubscribeToEvent<CreateOrganizationRejected>()
                .SubscribeToEvent<WardenCreated>()
                .SubscribeToEvent<CreateWardenRejected>()
                .SubscribeToEvent<WardenCheckResultProcessed>()
                .SubscribeToEvent<FeatureRejected>()
                .SubscribeToEvent<SignedIn>()
                .SubscribeToEvent<SignedUp>()
                .SubscribeToEvent<SignedOut>()
                .SubscribeToEvent<SignInRejected>()
                .SubscribeToEvent<SignUpRejected>()
                .SubscribeToEvent<SignOutRejected>()
                .SubscribeToEvent<UsernameChanged>()
                .SubscribeToEvent<ChangeUsernameRejected>()
                .Build()
                .Run();
        }
    }
}
