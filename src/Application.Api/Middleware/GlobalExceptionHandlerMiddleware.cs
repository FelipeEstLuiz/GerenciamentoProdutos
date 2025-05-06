using Application.Domain.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace Application.Api.Middleware;

public class GlobalExceptionHandlerMiddleware(
    ILogger<GlobalExceptionHandlerMiddleware> logger
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
        IEnumerable<string> erros = [];

        if (exception is FluentValidation.ValidationException validationException)
        {
            httpStatusCode = HttpStatusCode.BadRequest;

            foreach (FluentValidation.Results.ValidationFailure failure in validationException.Errors)
            {
                string message = $"{failure.PropertyName} | {failure.ErrorMessage} | Valor: {failure.AttemptedValue}";
                erros = [.. erros, message];
            }
        }
        else if (exception is UnauthorizedAccessException)
        {
            httpStatusCode = HttpStatusCode.Unauthorized;
            erros = ["Usuário nao autorizado"];
        }
        else if (exception is ValidacaoException validacaoException)
        {
            httpStatusCode = validacaoException.HttpStatusCode;
            erros = [validacaoException.Message];
        }
        else
        {
            logger.LogError(exception, "Erro inesperado: {Message}", exception.Message);
            erros = ["Erro ao processar requisição"];
        }

        context.Response.StatusCode = (int)httpStatusCode;

        JsonSerializerSettings settings = new()
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(erros, settings));
    }
}
