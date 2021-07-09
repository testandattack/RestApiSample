using GtcRest.Models.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace RestApiSample
{
    public class SwaggerConfigOptions
    {
        #region -- Public Methods -----
        public static void ConfigureSwagger(SwaggerGenOptions c, Settings settings, IWebHostEnvironment environment)
        {
            // Add Base Info
            AddInfoSection(c);

            // Add Auth if needed
            if(settings.Core.UseBasicAuth)
            {
                // Add Security Info
                AddSecurityItems(c);
            }

            // Adds the XML Documentation
            GetXmlDocumentation(c);

            // Add the Document and Operation Filter methods to the swagger pipe.
            ApplyFilters(c);

            // Gets the proper URL to display in the Server dropdown of the Swagger docs
            GetServerUrl(c, settings.Core.SwaggerUrl, environment);
        }

        public static void ConfigureSwaggerUI(IApplicationBuilder app)
        {
            Log.ForContext<SwaggerConfigOptions>().Information("executing ConfigureSwaggerUI");
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Testing the Testing Strategy API");
                //c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Testing the Testing Strategy API");
                c.RoutePrefix = string.Empty;

                // How far to expand each model in the request when switching to Schema from Example
                c.DefaultModelExpandDepth(5);

                // Collapse each controller on the main page by default
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);

                // Adds the request duration to the response output when executing requests
                c.DisplayRequestDuration();

                // Adds a filter box that allows you to narrow down what operations show up
                c.EnableFilter();
            });
        }
        #endregion

        #region -- Private Methods -----
        private static void AddInfoSection(SwaggerGenOptions c)
        {
            Log.ForContext<SwaggerConfigOptions>().Information("executing AddInfoSection");

            // Setup the Swagger Documentation to include the build and environmental info
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            // Add all of that info to the top of the generated Swagger HTML
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "GtcRest API",
                Version = $"{version.Major}.{version.Minor}.{version.Build}",
                Description = "Sample REST API for understanding API design and testing",
                Contact = new OpenApiContact
                {
                    Name = "Geoff Gray",
                    Email = "geoffrey.gray@collins.com",
                },
            });
        }

        private static void AddSecurityItems(SwaggerGenOptions c)
        {

            Log.ForContext<SwaggerConfigOptions>().Information("executing AddSecurityItems");
            c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Name = "Authorization",
                Scheme = "basic",
                Description = "Input your username and password to access this API",
                In = ParameterLocation.Header,
            });


            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "basic" }
                    }, new List<string>() }
            });
        }

        private static void GetXmlDocumentation(SwaggerGenOptions c)
        {
            Log.ForContext<SwaggerConfigOptions>().Information("executing GetXmlDocumentation");
            // Get any XML documentation from the entire application. This is needed for picking up
            // example values and other info from projects like Ascentia.Domain
            // Get all XML files (one for each project in the solution where an object is used in a controller
            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            foreach (var xmlFile in xmlFiles)
            {
                Log.ForContext<SwaggerConfigOptions>().Debug("[{method}] Adding {xmlFile} to Swagger Documentation", "SwaggerGenConfigurationOptions", xmlFile);
                c.IncludeXmlComments(xmlFile);
            }

            // Enable the annotations from XML Comments and Swagger Extensions               
            c.EnableAnnotations();
        }

        private static void ApplyFilters(SwaggerGenOptions c)
        {
            Log.ForContext<SwaggerConfigOptions>().Information("executing ApplyFilters");
            //c.OperationFilter<MethodNameOperationFilter>();

            //c.OperationFilter<BasicAuthOperationsFilter>();
            c.DocumentFilter<AddCompanyInfoDocumentFilter>();
            c.DocumentFilter<LowercasePathFilter>();
            //c.DocumentFilter<CamelCaseComponentFilter>();
            c.DocumentFilter<AddXSpecIntegrationType>();
        }

        private static void GetServerUrl(SwaggerGenOptions c, string baseUrl, IWebHostEnvironment environment)
        {
            // This allows for configuring multiple Server URLs which will help in situations
            // where you need to control the overall path to either include or exclude firewalls
            // etc. 

            Log.ForContext<SwaggerConfigOptions>().Information("executing GetServerUrl");
            // Sets the UI to read the base Server from the App Config settings.
            if (environment.IsDevelopment())
            {
                Log.ForContext<SwaggerConfigOptions>().Debug("[{method}] detected a local site. Skipping the addition of a configured base path.", "GetServerUrl");
            }
            else
            {
                if (baseUrl.ToLower().Contains("donotset"))
                {
                    Log.ForContext<SwaggerConfigOptions>().Debug("[{method}] detected a dev slot site. Skipping the addition of a configured base path.", "GetServerUrl");
                }
                else
                {
                    Log.ForContext<SwaggerConfigOptions>().Debug("[{method}] Setting Swagger Server to {basePath}", "GetServerUrl", baseUrl);
                    c.AddServer(new OpenApiServer { Url = baseUrl });
                }
            }
        }
        #endregion
    }
}
