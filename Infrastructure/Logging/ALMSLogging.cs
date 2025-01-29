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
        private static string _LogPath = "logs/alms-logs.log";
        // allows for caching of the logger, so we don't have to use GetRequiredService every time
        private static Tuple<int, Serilog.ILogger?> _LoggerCache = new(0, null);

        /// <summary>
        /// Initializes the logger for the application
        /// </summary>
        /// <param name="builder">The builder to attach logging to</param>
        /// <param name="logtofile">if false, log files are not appended, and logging only goes to console
        /// if true, both console and file logging are enabled</param>
        /// <remarks>Must be used before any GetLogger() calls are made!</remarks>
        public static void InitLogger(this IHostBuilder builder, bool logtofile = false) {
            builder.UseSerilog((context, service, configuration) => {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(service)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
                if (logtofile) {
                    configuration.WriteTo.File(_LogPath, flushToDiskInterval: TimeSpan.FromMinutes(5));
                }
            });
        }

        /// <summary>
        /// Gets the logger from the application, caches the last used logger,
        /// in case of multiple different loggers, GetRequiredService is used,
        /// otherwise the cached logger is returned
        /// </summary>
        /// <param name="app">The app instance to retrieve the logger from</param>
        /// <returns>The Serilog.ILogger attached to the app</returns>
        /// <remarks>Can only be used after InitLogger() is called!</remarks>
        public static Serilog.ILogger GetLogger(this IApplicationBuilder app) {
            if (app.GetHashCode() == _LoggerCache.Item1) {
                return _LoggerCache.Item2!;
            }
            var logger = app.ApplicationServices.GetRequiredService<Serilog.ILogger>();
            _LoggerCache = new(app.GetHashCode(), logger);
            return logger!;
        }
    }
}
