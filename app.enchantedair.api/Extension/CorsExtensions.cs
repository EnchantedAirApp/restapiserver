using app.enchantedair.api.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.enchantedair.extensions
{
    public static class CorsExtensions
    {
       public static WebApplicationBuilder AddConfigBasedCors(this WebApplicationBuilder builder) 
       {
            var corsPoliciesConfig = new CorsPoliciesConfig();
            builder.Configuration.GetSection("CorsPolicies").Bind(corsPoliciesConfig);
            var policies = corsPoliciesConfig.Policies.Where(a => a.Value.Enabled).ToDictionary(a=> a.Key,a=>a.Value);
            builder.Services.AddCors(options =>
            {
                foreach (var policy in policies)
                {
                    options.AddPolicy(policy.Key, builder =>
                    {
                        builder.WithOrigins(policy.Value.AllowedOrigins)
                               .WithMethods(policy.Value.AllowedMethods)
                               .WithHeaders(policy.Value.AllowedHeaders)
                               .AllowCredentials()
                               .Build();
                    });
                }
            });
            return builder;     
        }
    }
}
