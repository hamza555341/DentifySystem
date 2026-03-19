
using DentifySystem.Extentions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;
using Persistence.IdentityData.DataSeed;
using Persistence.IdentityData.IdentityModule;
using System.Threading.Tasks;

namespace DentifySystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            builder.Services.AddDbContext<DentifyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DentifyDbContext>();

            builder.Services.AddScoped<IDataIntializer, DataIntializer>();

            var app = builder.Build();

            #region UpdateDb_Pending_Migrations And DataSeeding

            await app.MigrateDataBaseAsync();
            await app.SeedIdentityDataAsync();

            # endregion



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

           await  app.RunAsync();
        }
    }
}
