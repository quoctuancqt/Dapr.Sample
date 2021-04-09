using Microsoft.Extensions.Configuration;
using Serilog;

namespace SharedKernel.Extensions
{
    public static class LogginExtension
    {
        public static ILogger CreateSerilogLogger(IConfiguration configuration, string appName)
        {
            var seqServerUrl = configuration.GetSection("Serilog").GetValue<string>("SeqServerUrl");

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", appName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(seqServerUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
