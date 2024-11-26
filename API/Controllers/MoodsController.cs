
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using models.enchantedair.app;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MoodsController : ControllerBase
{
    private readonly UserDbContext _context;

    public MoodsController(UserDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<mood>>> GetMoods()
    {
        return await _context.Moods.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<mood>> CreateMood(mood mood)
    {
        _context.Moods.Add(mood);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMood), new { id = mood.Id }, mood);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDeleteMood(Guid id)
    {
        var mood = await _context.Moods.FindAsync(id);
        if (mood == null) return NotFound();

        var updatedMood = mood with { State = RecordState.Deleted };
        _context.Moods.Update(updatedMood);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
