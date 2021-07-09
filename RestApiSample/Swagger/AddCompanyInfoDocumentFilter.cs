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


    // Sample that demonstrates how to add extra info to a generated swagger document
    public class AddCompanyInfoDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument openApiDocument, DocumentFilterContext documentFilterContext)
        {
            if (!openApiDocument.Info.Contact.Extensions.ContainsKey("x-company-name"))
            {
                openApiDocument.Info.Contact.Extensions.Add("x-company-name", new OpenApiString("GTC"));
                openApiDocument.Info.Contact.Extensions.Add("x-owner-name", new OpenApiString("Geoff Gray"));
                openApiDocument.Info.Contact.Extensions.Add("x-owner-emailaddress", new OpenApiString("Geoffrey.Gray@collins.com"));
                openApiDocument.Info.Contact.Extensions.Add("x-owner-phonenumber", new OpenApiString("7048070011"));
                openApiDocument.Info.Contact.Extensions.Add("x-administrator-name", new OpenApiString("Geoff Gray"));
                openApiDocument.Info.Contact.Extensions.Add("x-administrator-phonenumber", new OpenApiString("7048070011"));
                openApiDocument.Info.Contact.Extensions.Add("x-administrator-emailaddress", new OpenApiString("Geoffrey.Gray@collins.com"));
                openApiDocument.Info.Contact.Extensions.Add("x-administrator-responsibilities", new OpenApiString("brew good coffee"));
            }
        }
    }

}
