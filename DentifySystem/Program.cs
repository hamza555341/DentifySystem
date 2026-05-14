
using DentifySystem.BackgroundJobs;
using DentifySystem.Extentions;
using DentifySystem.Hubs;
using Domain.Entites.IdentityModule;
using Domain.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.DbContexts;
using Persistence.IdentityData.DataSeed;
using Persistence.IdentityData.IdentityModule;
using Persistence.Repositories;
using Persistence.Services;
using Service;
using Service.Abstraction;
using Service.MappingProfile;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using IAuthenticationService = Service.Abstraction.IAuthenticationService;

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
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Write the token here as follows : Bearer {your token}"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });


            builder.Services.AddDbContext<DentifyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //builder.Services.AddIdentityCore<ApplicationUser>()
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<DentifyDbContext>();


            builder.Services.AddHangfire(config =>
            config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHangfireServer();


            builder.Services.AddAutoMapper(typeof(ServiceAssemplyRefrence).Assembly);

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<DentifyDbContext>()
                            .AddDefaultTokenProviders();

          builder.Services.Configure<EmailSettings>
                (builder.Configuration.GetSection("EmailSettings"));
       
            builder.Services.AddScoped<IEmailService,EmailService>();
            builder.Services.AddScoped<IDataIntializer, DataIntializer>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthenticationService,Service.AuthenticationService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAttachmentService,AttachmentService>();
            builder.Services.AddScoped<ICaseService,CaseService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IStudentRatingService,StudentRatingService>();
            builder.Services.AddScoped<IBackgroundJobService, HangfireJobService>();
            builder.Services.AddScoped<IAppointmentService,AppointmentService>();
            builder.Services.AddScoped<ITreatmentRequestService, TreatmentRequestService>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IPatientService, PatientService>();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  options.SaveToken = true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                      ValidAudience = builder.Configuration["JWTOptions:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(
                          Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"]!)
                      ),
                  };

                  
                  options.Events = new JwtBearerEvents
                  {
                      OnMessageReceived = context =>
                      {
                          var accessToken = context.Request.Query["access_token"];
                          var path = context.HttpContext.Request.Path;

                          if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                          {
                              context.Token = accessToken;
                          }
                          return Task.CompletedTask;
                      }
                  };
              });



            builder.Services.AddSignalR();


            var app = builder.Build();

            app.UseHangfireDashboard("/hangfire");


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

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<ChatHub>("/hubs/chat");

            app.UseStaticFiles();   

            app.MapControllers();

           await  app.RunAsync();
        }
    }
}
