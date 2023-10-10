using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Data.Entity.Core;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using System.Web.Http;

namespace TestTaskUser.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, "Bad request", ex.Message);
            }
            catch (ObjectNotFoundException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, "Not found", ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Internal server error", "An internal server error has occured");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string title, string message)
        {
            Log.Error($"Something went wrong: {message}");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            ProblemDetails details = new()
            {
                Status = (int)statusCode,
                Title = title,
                Detail = message
            };
            var json = JsonSerializer.Serialize(details);
            await context.Response.WriteAsync(json);
        }
    }
}
