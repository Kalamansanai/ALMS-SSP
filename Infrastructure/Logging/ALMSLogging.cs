using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logging {
    public static class ALMSLogging {
        private static string logPath = "logs/alms-logs.log";

        public static void InitLogger(this IHostBuilder builder, bool logtofile = false) {
            builder.UseSerilog((context, service, configuration) => {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(service)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
                if (logtofile) {
                    configuration.WriteTo.File(logPath, flushToDiskInterval: TimeSpan.FromMinutes(5));
                }
            });
        }

        public static Serilog.ILogger GetLogger(this IApplicationBuilder app) {
            return app.ApplicationServices.GetRequiredService<Serilog.ILogger>();
        }
    }
}
