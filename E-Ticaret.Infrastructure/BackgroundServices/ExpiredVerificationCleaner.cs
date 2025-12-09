using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace E_Ticaret.Infrastructure.BackgroundServices
{
    public sealed class ExpiredVerificationCleaner : BackgroundService
    {

        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<ExpiredVerificationCleaner> logger;

        public ExpiredVerificationCleaner(ILogger<ExpiredVerificationCleaner> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = serviceProvider.CreateScope();
                var emailRepo = scope.ServiceProvider.GetRequiredService<IEmailRepository>();
                var userRepo = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var expiredCodes = emailRepo.Where(x => x.ExpirationTime < DateTime.UtcNow).ToList();


             
                if (expiredCodes.Any())
                {
                    emailRepo.DeleteRange(expiredCodes);
                    logger.LogInformation($"{expiredCodes.Count} adet süresi dolmuş doğrulama kodu silindi.");
                }


               var expiredUsers = userRepo.Users.Where(x => !x.EmailConfirmed && x.CreatedAt < DateTime.UtcNow.AddMinutes(3)).ToList();




                foreach(var user in expiredUsers)
                {
                    var result = await userRepo.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        logger.LogInformation($"{user.Email} adlı doğrulanmamış kullanıcı silindi.");
                    }
                    else
                    {
                        logger.LogWarning($"{user.Email} adlı kullanıcı silinemedi: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }


                }

                await unitOfWork.SaveChangesAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); 

            }
        }
    }
}
