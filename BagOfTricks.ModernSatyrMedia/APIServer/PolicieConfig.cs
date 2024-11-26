using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagOfTricks.ModernSatyrMedia.APIServer
{
    public record CorsPolicyOptions(string[] AllowedOrigins, string[] AllowedMethods, string[] AllowedHeaders, bool Enabled,bool AllowAll = false);
    public class CorsPoliciesConfig() 
    {
        public IDictionary<string, CorsPolicyOptions> Policies { get; set; }
    }

}
