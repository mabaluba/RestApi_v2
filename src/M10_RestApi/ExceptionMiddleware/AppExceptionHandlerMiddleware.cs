using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace M10_RestApi.ExceptionMiddleware
{
    internal class AppExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AppExceptionHandlerMiddleware> _logger;

        public AppExceptionHandlerMiddleware(RequestDelegate next, ILogger<AppExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception, exception.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var result = $"Service is unavailable right now. Error code: {(int)HttpStatusCode.BadRequest}";
                await httpContext.Response.WriteAsync(result);
            }
            catch (MissingMemberException exception)
            {
                _logger.LogError(exception, exception.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                var result = $"{exception.Message} Error code: {(int)HttpStatusCode.NotFound}";
                await httpContext.Response.WriteAsync(result);
            }
            catch (NullReferenceException exception)
            {
                _logger.LogError(exception, exception.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                var result = $"Given information is not valid. Error code: {(int)HttpStatusCode.NotFound}";
                await httpContext.Response.WriteAsync(result);
            }
            catch (ValidationException exception)
            {
                _logger.LogError(exception, exception.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                var result = $"Given information is not valid for service. Error code: {(int)HttpStatusCode.NotFound}";
                await httpContext.Response.WriteAsync(result);
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError(exception, exception.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                var result = $"Сhanges cannot be made. Check the correctness of the provided data. Error code: {(int)HttpStatusCode.Conflict}";
                await httpContext.Response.WriteAsync(result);
            }
            catch (InvalidOperationException exception)
            {
                _logger.LogError(exception, exception.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                var result = $"Check the correctness of the provided data or contact to technical support. Error code: {(int)HttpStatusCode.Conflict}";
                await httpContext.Response.WriteAsync(result);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, exception.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsync($"Sevice stopped working, try again later or contact to technical support. Error code: {(int)HttpStatusCode.BadRequest}");
            }
        }
    }
}