
using app.enchantedair.model;
using app.enchantedair.persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace app.enchantedair.api.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class MoodsController : ControllerBase
    {
        private readonly EnchantedAirDB _context;

        public MoodsController(EnchantedAirDB context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mood>>> Getmoods()
        {
            return await _context.Moods.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mood>> Getmood(int id)
        {
            var mood = await _context.Moods.FindAsync(id);
            if (mood == null) return NotFound();
            return mood;
        }

        [HttpPost]
        public async Task<ActionResult<Mood>> Postmood(Mood mood)
        {
            _context.Moods.Add(mood);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Getmood), new { id = mood.Id }, mood);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Putmood(int id, Mood mood)
        {
            if (id != mood.Id) return BadRequest();
            _context.Entry(mood).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Moods.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletemood(int id)
        {
            var mood = await _context.Moods.FindAsync(id);
            if (mood == null) return NotFound();
            _context.Moods.Remove(mood);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
