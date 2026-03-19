using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;
using Persistence.IdentityData.DataSeed;

namespace DentifySystem.Extentions
{
    public static class WebApplicationRegistration
    {
        public static async Task<WebApplication> MigrateDataBaseAsync(this WebApplication app)

        {
            await using var Scope = app.Services.CreateAsyncScope();

            var dbContext = Scope.ServiceProvider.GetRequiredService<DentifyDbContext>();
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                await dbContext.Database.MigrateAsync();

            return app;
        }

        public static async Task<WebApplication> SeedIdentityDataAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();
            var DataIntializerObj = Scope.ServiceProvider.GetRequiredService<IDataIntializer>();
            await DataIntializerObj.IntializeDataAsync();

            return app;

        }

    }
}
