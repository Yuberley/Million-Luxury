namespace MillionLuxury.Web.API.Extensions;

#region Usings
using Microsoft.EntityFrameworkCore;
using MillionLuxury.Infrastructure.Database;
using MillionLuxury.Web.API.Middlewares; 
#endregion

public static class ApplicationBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }

    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}