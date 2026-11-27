using System.Net;
using System.Text;
using Moq.Protected;

namespace Freelance.ApiGateway.UnitTests.TestData;

internal static class OpenApiDocuments
{
    public static string Service(string path, string tag, string schema) => $$"""
        {
          "openapi": "3.1.1",
          "info": { "title": "{{tag}}", "version": "v1" },
          "paths": {
            "{{path}}": {
              "get": {
                "tags": [ "{{tag}}" ],
                "responses": {
                  "200": {
                    "description": "OK",
                    "content": { "application/json": { "schema": { "$ref": "#/components/schemas/{{schema}}" } } }
                  }
                }
              }
            }
          },
          "components": {
            "schemas": { "{{schema}}": { "type": "object", "properties": { "id": { "type": "string" } } } },
            "securitySchemes": { "Bearer": { "type": "http", "scheme": "bearer" } }
          },
          "tags": [ { "name": "{{tag}}" } ]
        }
        """;

    /// <summary>Mocked HTTP handler answering each URL with the configured status code and body.</summary>
    public static Mock<HttpMessageHandler> Handler(IDictionary<string, (HttpStatusCode Status, string Body)> responses)
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage request, CancellationToken _) =>
                responses.TryGetValue(request.RequestUri!.ToString(), out var response)
                    ? new HttpResponseMessage(response.Status) { Content = new StringContent(response.Body, Encoding.UTF8, "application/json") }
                    : throw new HttpRequestException("service unavailable"));
        return handler;
    }
}
