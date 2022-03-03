using Microsoft.AspNetCore.Http;
using Reservation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Reservation.Middleware
{
    /// <summary>
    /// Hata yönetimi için bir ara katman oluşturulması
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        // Private olarak hazırlanan istek işleminin setlenmesi
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // bir hata olmaması durumunda ara katmandan başarılı şekilde çıkılıp işleme devam edilmesi
                await _next(context);
            }
            catch (Exception error)
            {
                // ajax işlemlerinde karşılaşılacak olası hataların işlenmesi
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case AppException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                // hata sonucunun error mesage olarak geri dönülmesi işlemi
                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}