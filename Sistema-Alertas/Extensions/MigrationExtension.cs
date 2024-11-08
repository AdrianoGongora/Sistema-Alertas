using Microsoft.EntityFrameworkCore;
using Sistema_Alertas.Database;

namespace Sistema_Alertas.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

        dbContext?.Database.Migrate();
    }
}
