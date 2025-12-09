
using E_Ticaret.Application.Behaviors;
using E_Ticaret.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_Ticaret.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {

        public static IServiceCollection AddApplication(
            this IServiceCollection services
            )
        {
            services.AddMediatR(cfr =>
            {
                cfr.RegisterServicesFromAssemblies(typeof(ApplicationServiceRegistration).Assembly,
                    typeof(AppUser).Assembly);
                cfr.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

        

            return services;

        }
    }
}
