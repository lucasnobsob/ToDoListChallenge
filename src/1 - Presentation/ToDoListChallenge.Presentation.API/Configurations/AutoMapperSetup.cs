using ToDoListChallenge.Application.AutoMapper;

namespace ToDoListChallenge.Services.Api.Configurations
{
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(cfg =>
            {
                foreach (var profile in AutoMapperConfig.RegisterMappings())
                {
                    cfg.AddProfile(profile);
                }
            });
        }
    }
}
