using System;
using FluentResults;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using Infrastructure.Logging;

namespace Infrastructure.Database {
    public static class DbInit {
        static String? _ConStr = null;

        /// <summary>
        /// Adds database configuration to the application builder
        /// All logic related to configuring and connecting to the database is done
        /// To actually initialize the database, use DbInit.InitializeDatabase on the built application
        /// </summary>
        /// <param name="app">The WebApplicationBuilder to modify</param>
        /// <param name="config">The configuration to load the connection string from (usually app.Configuration)</param>
        /// <returns>Result.Ok containing this on success, Result.Fail on failure</returns>
        /// <remarks>To see how the DB connection string is created/handled, see DbInit.GetConnectionString</remarks>
        public static Result AddDatabase(this WebApplicationBuilder app, IConfiguration config) {
            // Loading the database connection string
            // it's done through an internal method to avoid code duplication and allow for lazy loading
            Result<String> conStringRes = GetConnectionString(config, true);
            if (conStringRes.IsFailed) return conStringRes.ToResult();

            return Result.Try(() => {
                String connectionString = conStringRes.Value;

#if DEBUG
                Console.WriteLine($"Connection string: {connectionString}");
#endif

                Action<DbContextOptionsBuilder> dbOpts = options => {
                    options.EnableSensitiveDataLogging();

                    var serverVersion = ServerVersion.AutoDetect(connectionString);
                    options.UseMySql(connectionString, serverVersion, opt => opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                };

                IServiceCollection services = app.Services;

                services.AddDbContext<ALMSDbContext>(dbOpts);
            });
        }

        /// <summary>
        /// Creates a scope and initializes the database configured in DbInit.AddDatabase
        /// Can fail if an ALMSDbContext can not be initialized
        /// </summary>
        /// <param name="app">The Application to initialise the DB of
        /// (needs AddDatabase to be run on its builder first)</param>
        /// <returns>Result.Ok if successful, Result.Fail if any exception is thrown</returns>
        public static Result InitializeDatabase(this IApplicationBuilder app) {
            var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ALMSDbContext>();
            var db = context.Database;
            var logger = app.GetLogger();

            return Result.Try(() => {
                if (context == null) {
                    logger.Error("Could not get ALMSDbContext from services, DB initialisation unsuccessful");
                    throw new Exception("Critical error encountered during DB initialisation");
                }
                if (!db.CanConnect()) {
                    logger.Information("Could not connect to database, attempting to create it");
                    if (db.EnsureCreated())
                        logger.Information("DB didn't exist before, created it");

                    if (!db.CanConnect()) {
                        logger.Error("Could not connect or create database, DB initialisation unsuccessful");
                        throw new Exception("Critical error encountered during DB initialisation");
                    }
                }
                logger.Debug("DB initialised successfully");
            });
        }

        /// <summary>
        /// Gets the connection string from the configuration,
        /// lazily loads the connection string, only if ignoreLazy is set to true, or if the value is not yet set,
        /// it will reload the connection
        /// by default, it loads the string from appsettings.json
        /// </summary>
        /// <param name="config">the IConfiguration used to load the ConnectionString parameter</param>
        /// <param name="ignoreLazy">if the ConnectionString was already set, reload it anyway using config</param>
        /// <returns>Result.Ok if a connection string was found or lazy loaded, Result.Fail otherwise</returns>
        [return: NotNull]
        public static Result<String> GetConnectionString(IConfiguration config, bool ignoreLazy = false) {
            if (_ConStr != "" && _ConStr != null && !ignoreLazy) {
                return Result.Ok(_ConStr!);
            }
            String? connectionString = config["ConnectionString"];
            connectionString = connectionString == "" ? null : connectionString;
            if (connectionString == null) {
                return Result.Fail("ConnectionString not found in configuration");
            }
            _ConStr = connectionString;
            return Result.Ok(connectionString!);
        }

        /// <summary>
        /// Gets the connection string from the configuration if it was already loaded
        /// </summary>
        /// <returns>Result.Ok if a connection string was already successfully loaded, Result.Fail otherwise</returns>
        [return: NotNull]
        public static Result<String> GetConnectionString() {
            if (_ConStr == "" || _ConStr == null) {
                return Result.Fail("ConnectionString not found in configuration or not set");
            }
            return Result.Ok(_ConStr!);
        }
    }
}
