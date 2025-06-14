using wow.tools.local.Services;

namespace wow.tools.local
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // this will override the config.json values if the relevant command line flags are present
                SettingsManager.ParseCommandLineArguments(args);

                if (SettingsManager.UseTACTSharp)
                    CASC.InitTACT(SettingsManager.WoWFolder, SettingsManager.WoWProduct);
                else
                    CASC.InitCasc(SettingsManager.WoWFolder, SettingsManager.WoWProduct);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception initializing CASC: " + e.Message);
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.Limits.MaxConcurrentConnections = 500;
                    serverOptions.Limits.MaxConcurrentUpgradedConnections = 500;
                })
                .UseStartup<Startup>();
            });
    }
}