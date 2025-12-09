using E_Ticaret.Domain.Entities;
using E_Ticaret.Infrastructure.Abstractions;
using E_Ticaret.Infrastructure.Options;
using E_Ticaret.Persistence.Context;
using E_Ticaret.Persistence.Services;
using GenericRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
namespace E_Ticaret.Persistence.DependencyInjection
{
    public static class PersistenceServiceRegistration
    {


        public static IServiceCollection AddPersistance(    
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            string connectionString = configuration.GetConnectionString("SqlServer");


            services.AddScoped<IEncryptionService, EncryptionService>();

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString);

               
            });

            services.AddIdentity<AppUser,AppRole>(cfr =>
            {
                cfr.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                cfr.Lockout.MaxFailedAccessAttempts = 5;
                cfr.Lockout.AllowedForNewUsers = true;


                cfr.Password.RequireNonAlphanumeric = true;
                cfr.Password.RequireLowercase = true;
                cfr.Password.RequireUppercase = true;
                cfr.Password.RequiredLength = 8;
                cfr.Password.RequireDigit = true;


                cfr.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            services.Configure<Jwt>(configuration.GetSection("Jwt"));
            services.ConfigureOptions<JwtOptionsSetup>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
   .AddJwtBearer();
            services.Configure<KeycloakConfiguration>(configuration.GetSection("KeycloakConfiguration"));


            

            services.AddScoped<KeycloakService>();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));


            services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<AppDbContext>());

            services.Scan(s => s.FromAssemblies(
                typeof(PersistenceServiceRegistration).Assembly)
            .AddClasses(publicOnly:false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            );
            return services;

        }
    }
}
