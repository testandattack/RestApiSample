using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
//using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extensions;
using Serilog;


namespace RestApiSample
{
    // ----------------------------------------------------------
    // The following classes are added to meet the requirements of the Collins OAS Linter.
    // Local Linter at "C:\Users\graygeol\Documents\DT API Publishing MVP Testing\collins-oaslinter"
    // Execution command: spectral lint http://localhost:5000/swagger/v1/swagger.json
    // ----------------------------------------------------------


    //// This filter is required because SwashBuckle made the "deprecated" item into a
    //// nullable boolean to restrict noise in the spec. This forces every operation to
    //// define the item explicitly.
    //public class DeprecatedOperationsFilter : IOperationFilter
    //{
    //    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    //    {
    //        var obsoleteOperation = context.ApiDescription.CustomAttributes().Any(attr => attr.GetType() == typeof(ObsoleteAttribute));
    //        operation.Deprecated = obsoleteOperation;
    //    }
    //}

    public class NonDeprecatedOperationsFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument openApiDocument, DocumentFilterContext documentFilterContext)
        {
            //if(openApiDocument.Paths.)
        }
    }

    public class AddXSpecClientType : IDocumentFilter
    {
        public void Apply(OpenApiDocument openApiDocument, DocumentFilterContext documentFilterContext)
        {
            if (!openApiDocument.Extensions.ContainsKey("x-spec-type"))
            {
                openApiDocument.Extensions.Add("x-spec-type", new OpenApiString("client"));
            }
        }
    }

    public class AddXSpecIntegrationType : IDocumentFilter
    {
        public void Apply(OpenApiDocument openApiDocument, DocumentFilterContext documentFilterContext)
        {
            if (!openApiDocument.Extensions.ContainsKey("x-spec-type"))
            {
                openApiDocument.Extensions.Add("x-spec-type", new OpenApiString("integration"));
            }
        }
    }

    // Lowercase all routes, for Swagger, as discussed here:
    //   https://github.com/domaindrivendev/Swashbuckle/issues/834
    // Issue has a reference to the original gist, which can be found here:
    //   https://gist.github.com/smaglio81/e57a8bdf0541933d7004665a85a7b198
    public class LowercasePathFilter : IDocumentFilter
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
    
    public class CamelCaseComponentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var components = swaggerDoc.Components.Schemas.ToDictionary(entry =>
                entry.Key.ToCamelCase(), entry => entry.Value);
            
            swaggerDoc.Components = new OpenApiComponents();
            foreach (var (key, value) in components)
            {
                swaggerDoc.Components.Schemas.Add(key, value);
            }
        }
    }

    public class CamelCaseSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema swaggerSchema, SchemaFilterContext context)
        {
            Log.ForContext<CamelCaseSchemaFilter>().Debug("ApplySchemaFilter found {type}", swaggerSchema.Type);
            if (swaggerSchema.Type != "object")
                return;

            //var referenceV3 = swaggerSchema.Reference.ReferenceV3.ToCamelCase();
            //var referenceV2 = swaggerSchema.Reference.ReferenceV2.ToCamelCase();

            //OpenApiReference openApiReference = new OpenApiReference();
            //openApiReference.
            //swaggerSchema.Reference = reference;
        }
    }

    //public class TagListSchemaFilter : ISchemaFilter
    //{
    //    public void Apply(OpenApiSchema swaggerSchema, SchemaFilterContext context)
    //    {
    //        Log.ForContext<TagListSchemaFilter>().Debug("ApplySchemaFilter found {type}", swaggerSchema.Type);
    //        if (swaggerSchema.Type != "object")
    //            return;

    //        //var referenceV3 = swaggerSchema.Reference.ReferenceV3.ToCamelCase();
    //        //var referenceV2 = swaggerSchema.Reference.ReferenceV2.ToCamelCase();
    //        var operations 
    //        OpenApiReference openApiReference = new OpenApiReference();
    //        openApiReference.
    //        swaggerSchema.Reference = reference;
    //    }
    //}
}
