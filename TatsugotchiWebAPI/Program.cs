#define Managed

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace TatsugotchiWebAPI {
    public class Program {
        public static void Main(string[] args) {
          TestConfig(args).Build().Run();
        }

        public static IWebHostBuilder TestConfig(string[] args) {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) => {
                    var builtConfig = config.Build();

                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    var keyVaultClient = new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(
                            azureServiceTokenProvider.KeyVaultTokenCallback));

                    config.AddAzureKeyVault(
                        $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                        keyVaultClient,
                        new DefaultKeyVaultSecretManager());

                }).UseStartup<Startup>();
    }
}
