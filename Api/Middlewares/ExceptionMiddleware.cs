using System.Net;
using Api.Exceptions;
using Newtonsoft.Json;

namespace Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode=HttpStatusCode.InternalServerError;
            string result=JsonConvert.SerializeObject(e.Message);
            switch (e)
            {
                case BadHttpRequestException badHttpRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(validationException.Errors);
                    break;
                default:
                    break;
            }
            context.Response.StatusCode=(int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
