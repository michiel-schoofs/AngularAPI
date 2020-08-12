#define Managed

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace TatsugotchiWebAPI {
    public class Program {
        public static void Main(string[] args) {
          TestConfig(args).Build().Run();
        }

        public static IWebHostBuilder TestConfig(string[] args) {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }
    }
}
