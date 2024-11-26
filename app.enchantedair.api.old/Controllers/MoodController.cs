using Microsoft.AspNetCore.Mvc;
using models.enchantedair.app;
using System.Data;
using Dapper;
using persistence.modernsatyrmedia.com;

namespace app.enchantedair.api.Controllers
{
    [Route("api/journal/[controller]")]
    public class MoodController : Controller
    {
        IConfiguration Config { get; }
        ILogger<MoodController> Logger { get; }
        public IGenericController<Guid, mood> Repo { get; }

        public MoodController(IConfiguration config, ILogger<MoodController> logger, IGenericController<Guid, mood> repo)
        {
            Config = config;
            Logger = logger;
            Repo = repo;

        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] mood user) 
            => await Repo.CreateAsync(user);
        [HttpDelete]
        public async Task<IActionResult> DeleteSync(Guid id) 
            => await Repo.DeleteSync(id);
        [HttpGet]
        public async Task<IActionResult> GetAsync() 
            => await Repo.GetAsync();
        [HttpGet]
        [Route("/api/journal/[controller]/{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
            => await Repo.GetAsync(id);
        [HttpPut]
        public async Task<IActionResult> UpdateSync(Guid id, [FromBody] mood user) 
            => await Repo.UpdateSync(id, user);
            
    }
}
