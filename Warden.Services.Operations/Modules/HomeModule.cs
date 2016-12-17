namespace Warden.Services.Operations.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule()
        {
            Get("", args => "Welcome to the Warden.Services.Operations API!");
        }
    }
}