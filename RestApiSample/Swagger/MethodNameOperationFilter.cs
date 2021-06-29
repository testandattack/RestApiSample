using Microsoft.OpenApi;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiSample
{
    public class MethodNameOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            MethodNameExtension method = new MethodNameExtension();

            method.AddName(context.MethodInfo.Name);

            operation.Extensions.Add("x-method-name", method);
        }
    }

    public class MethodNameExtension : IOpenApiExtension
    {
        public string MethodName { get; private set; }

        public MethodNameExtension()
        {
            MethodName = string.Empty;
        }

        public void AddName(string methodName)
        {
            MethodName = methodName;
        }

        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            writer.WriteValue(MethodName);
        }
    }
}
