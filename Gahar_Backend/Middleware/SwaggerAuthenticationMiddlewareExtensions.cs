namespace Gahar_Backend.Middleware
{
  /// <summary>
    /// Extension methods for Swagger authentication middleware
    /// </summary>
    public static class SwaggerAuthenticationMiddlewareExtensions
    {
        /// <summary>
        /// Adds Swagger authentication middleware to protect Swagger endpoints
      /// </summary>
   public static IApplicationBuilder UseSwaggerAuthentication(this IApplicationBuilder app)
  {
         app.UseMiddleware<SwaggerAuthenticationMiddleware>();
  return app;
        }
    }
}
