using System.Reflection;
using GraphService.Presentation.Base;

namespace GraphService.Presentation.Extensions
{
    public static class EndpointExtensions
    {
        public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
        {
            var endpointTypes = assembly
                .DefinedTypes
                .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                              typeof(IEndpoint).IsAssignableFrom(type));

            foreach (var type in endpointTypes)
            {
                services.AddTransient(typeof(IEndpoint), type);
            }

            services.AddEndpointsApiExplorer();

            return services;
        }

        public static IApplicationBuilder MapEndpoints(this WebApplication app)
        {
            var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

            foreach (var endpoint in endpoints)
            {
                endpoint.MapEndpoint(app);
            }

            return app;
        }
    }
}
