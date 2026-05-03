using ToDoListChallenge.Domain.Services.Hash;

namespace ToDoListChallenge.Services.Api.StartupExtensions
{
    public static class HashExtension
    {
        public static IServiceCollection AddCustomizedHash(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HashingOptions>(configuration.GetSection(HashingOptions.Hashing));
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
