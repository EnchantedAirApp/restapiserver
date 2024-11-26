using Microsoft.Extensions.DependencyInjection;

namespace BagOfTricks.ModernSatyrMedia.Extensions
{
    /// <summary>
    /// Extensions To make my life easier with E
    /// </summary>
    public static class DevelopmentExtensions
    {
        public static IServiceCollection AddReactCors(this IServiceCollection services) =>
             services.AddCors(options =>
            {
                options.AddPolicy("ReactDev",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")  // Allow the React app's origin
                              .AllowAnyMethod()                     // Allow all methods (GET, POST, etc.)
                              .AllowAnyHeader()                     //  
                              .AllowCredentials()
                              .SetIsOriginAllowed(origin => true)
                              .Build();


    });
            });
    }
}
