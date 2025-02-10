using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParameters = context.ApiDescription.ParameterDescriptions
            .Where(p => p.ModelMetadata?.ModelType == typeof(IFormFile) ||
                        p.ModelMetadata?.ModelType == typeof(IFormFileCollection))
            .ToList();

        if (!fileParameters.Any())
            return;

        operation.RequestBody = new OpenApiRequestBody
        {
            Content =
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = GenerateMultipartSchema(context)
                }
            }
        };
    }

    private static OpenApiSchema GenerateMultipartSchema(OperationFilterContext context)
    {
        var schema = new OpenApiSchema
        {
            Type = "object",
            Properties = context.ApiDescription.ParameterDescriptions
                .ToDictionary(
                    p => p.Name,
                    p => p.ModelMetadata?.ModelType == typeof(IFormFile)
                        ? new OpenApiSchema { Type = "string", Format = "binary" }
                        : context.SchemaGenerator.GenerateSchema(p.ModelMetadata.ModelType, context.SchemaRepository)
                )
        };

        return schema;
    }
}