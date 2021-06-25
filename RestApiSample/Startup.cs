using GtcRest.Database;
using GtcRest.Database.Interfaces;
using GtcRest.Database.Stores;
using GtcRest.Interfaces.Repository;
using GtcRest.Interfaces.Service;
using GtcRest.Models.Shared;
using GtcRest.Repository.Repos;
using GtcRest.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace RestApiSample
{
    public class Startup
    {
        #region Properties________________________________________________
        //private Microsoft.Extensions.Logging.ILogger _logger;
        public IConfiguration _configuration { get; }
        public IWebHostEnvironment _environment { get; }
        private Settings _settings { get; set; }

        //public IConfiguration Configuration { get; }
        #endregion

        #region Constructor_______________________________________________
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
            CheckForMissingSettings();
        }
        #endregion

        #region Configuration_____________________________________________
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureOptions(services);

            services.AddSwaggerGen(c =>
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Gtc API",
                    Version = $"{version.Major}.{version.Minor}.{version.Build}",
                });
            });

            services.AddControllers();
            ConfigureDbContexts(services);

            services.AddScoped<IGtcStore, GtcStore>();
            services.AddScoped<IGtcRepo, GtcRepo>();
            services.AddScoped<IGtcService, GtcService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            }
            else
            {
                app.UseExceptionHandler(options =>
                {
                    options.Run(async context =>
                    {
                        // Response defaults
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "text/html";
                        var result = "Unhandled Exception in API";

                        await context.Response.WriteAsync(result);
                    });
                });
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSerilogRequestLogging(options =>
            {
                // Customize the message template
                options.MessageTemplate = "SerilogRequestLogging: {RequestHost} {RequestMethod} {RequestPath} responded {StatusCode} with {ResponseLength} bytes in {Elapsed:N0} ms. {CorrelationId}";

                // Emit Information level events instead of the defaults
                options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Information;

                // Attach additional properties to the request completion event
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("ResponseLength", httpContext.Response.ContentLength);
                    diagnosticContext.Set("CorrelationId", httpContext.TraceIdentifier);
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                };
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Testing the Testing Strategy API");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureDbContexts(IServiceCollection services)
        {
            // Application Services DbContext
            services.AddDbContext<UserContext>(x => x.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            services.AddDbContext<AdminContext>(x => x.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        }
        #endregion

        #region Settings Methods__________________________________________
        private void CheckForMissingSettings()
        {
            if (_configuration.GetValue("MigrationBuild", false) || _configuration.GetValue("migrationBuild", false))
            {
                return;
            }
            IEnumerable<IConfigurationSection> settings = _configuration.GetChildren();
            string missingSettings = RecursiveSettingsCheck(settings);

            bool isMissingSettings = (missingSettings != string.Empty) ? true : false;
            if (isMissingSettings)
            {
                var msg = $"The API could not be started because the following app settings are not found:  {missingSettings} ";
                msg += "If hosting locally, check your secrets.json.  In Azure, check your web app environment settings.";
                throw new Exception(msg);
            }
        }

        private string RecursiveSettingsCheck(IEnumerable<IConfigurationSection> settings)
        {
            string missingSettings = "";

            foreach (IConfigurationSection setting in settings)
            {
                bool settingHasChildren = string.IsNullOrEmpty(setting.Value);
                if (settingHasChildren)
                {
                    missingSettings += RecursiveSettingsCheck(setting.GetChildren());
                }
                else
                {
                    if (setting?.Value == "<injected-from-env>")
                    {
                        missingSettings += $"{setting.Path}, ";
                    }
                }
            }

            return missingSettings;
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            _settings = new Settings();
            _configuration.Bind(_settings);

            services.AddOptions();
            services.Configure<Settings>(_configuration);
        }
        #endregion
    }
}
