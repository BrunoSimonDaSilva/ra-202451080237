using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LojaRevisao.Api;

public sealed class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var method = context.MethodInfo;
        var controller = method.DeclaringType;

        if (method.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
            || controller?.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) == true)
            return;

        if (!method.IsDefined(typeof(AuthorizeAttribute), inherit: true)
            && controller?.IsDefined(typeof(AuthorizeAttribute), inherit: true) != true)
            return;

        var scheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };
        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new() { [scheme] = Array.Empty<string>() }
        };
        operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Token ausente ou inválido." });
    }
}
