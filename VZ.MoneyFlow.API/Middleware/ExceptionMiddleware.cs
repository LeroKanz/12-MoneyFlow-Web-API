using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using VZ.MoneyFlow.EFData.Exceptions;

namespace VZ.MoneyFlow.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            var errorDetails = new ErrorDetails
            {
                ErrorType = "Failure",
                ErrorMessage = ex.Message,
            };

            switch (ex)
            {
                case NotFoundException notFoundException:
                    httpStatusCode = HttpStatusCode.NotFound;
                    errorDetails.ErrorType = "Not Found";
                    break;
                default:
                    break;
            }

            string response = JsonConvert.SerializeObject(errorDetails);
            httpContext.Response.StatusCode = (int)httpStatusCode;
            return httpContext.Response.WriteAsync(response);
        }
    }

    public class ErrorDetails
    {
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}
