using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Xml.Linq;

namespace app.enchantedair.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class externalcontroller : ControllerBase
    {
        public string GetIdentityKey() =>
            HttpContext
            .User
            .Claims
            .Where(claim => claim.Type.Contains("identity/claims/nameidentifier", StringComparison.CurrentCultureIgnoreCase))
            .FirstOrDefault()
            ?.Value
            ?.Replace("-", @"/") ?? "no/no";
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(new
            {
                urlobjectkey = $"{GetIdentityKey()}/{Guid.NewGuid().ToString("N")}",
            });
        }
    }
}
