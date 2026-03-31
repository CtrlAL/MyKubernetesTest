using Microsoft.EntityFrameworkCore;

namespace GraphService.Infrastructure.Data
{
    public static class PrepDb
    {
        public static void PreparingPopulation(IApplicationBuilder app, bool applyMigrations)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (applyMigrations)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }
        }
    }
}
