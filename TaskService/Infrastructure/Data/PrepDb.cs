using Microsoft.EntityFrameworkCore;

namespace TaskService.Data
{
    public static class PrepDb
    {
        public static void PreparingPopulation(IApplicationBuilder app, bool isProd = false)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                SeedData(dbContext, isProd);
            }
        }

        private static void SeedData(AppDbContext dbContext, bool isProd)
        {
            //if (isProd)
            //{
            //    try
            //    {
            //        dbContext.Database.Migrate();
            //    }
            //    catch(Exception ex)
            //    {
            //        Console.WriteLine($"Cannot apply migrations: {ex.Message}");
            //    }
                
            //}

            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot apply migrations: {ex.Message}");
            }


            if (!dbContext.Tasks.Any())
            {
                Console.WriteLine("Seeding data");

                dbContext.Tasks.AddRange(
                    new Entities.Task() { Name = "Clean Kitchen" },
                    new Entities.Task() { Name = "Clean Kitchen" },
                    new Entities.Task() { Name = "Clean Kitchen" }
                );

                dbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("We already have data");
            }
        }
    }
}
