using Microsoft.AspNetCore.Mvc;
using models.enchantedair.app;

namespace app.enchantedair.api.Controllers
{
    public interface IGenericController<tId,tModel>
    {
        Task<IActionResult> CreateAsync([FromBody] tModel user);
        Task<IActionResult> DeleteSync(tId id);
        Task<IActionResult> GetAsync();
        Task<IActionResult> GetAsync(tId id);
        Task<IActionResult> UpdateSync(tId id, [FromBody] tModel user);
    }
}