using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiSample
{
    // Lowercase all routes, for Swagger, as discussed here:
    //   https://github.com/domaindrivendev/Swashbuckle/issues/834
    // Issue has a reference to the original gist, which can be found here:
    //   https://gist.github.com/smaglio81/e57a8bdf0541933d7004665a85a7b198
    public class LowercasePathDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths.ToDictionary(entry => LowercaseEverythingButParameters(entry.Key),
                entry => entry.Value);
            swaggerDoc.Paths = new OpenApiPaths();
            foreach (var (key, value) in paths)
            {
                swaggerDoc.Paths.Add(key, value);
            }

        }

        private static string LowercaseEverythingButParameters(string key)
        {
            return string.Join('/', key.Split('/')
                .Select(x => x.Contains("{")
                    ? x
                    : x.ToLower()));
        }
    }
}
