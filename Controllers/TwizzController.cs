using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using twizzle.Database;
using twizzle.Models;

namespace twizzle.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TwizzController : ControllerBase
{
    private readonly TwizzleDbContext _context;

    public TwizzController(TwizzleDbContext context)
    {
        _context = context;
    }

    // GET: api/Twizz
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Twizz>>> GetTwizzes()
    {
        return await _context.Twizzs.Include(t => t.User).ToListAsync();
    }

    // GET: api/Twizz/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Twizz>> GetTwizz(int id)
    {
        var twizz = await _context.Twizzs
            .Include(t => t.User)
            .Include(t => t.Comments)
            .Include(t => t.Likes)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (twizz == null)
        {
            return NotFound();
        }

        return twizz;
    }

    // POST: api/Twizz
    [HttpPost]
    public async Task<ActionResult<Twizz>> PostTwizz(Twizz twizz)
    {
        var user = await _context.Users.FindAsync(twizz.UserId);
        if (user == null)
            return BadRequest("User not found.");
        twizz.User = user;
        Console.WriteLine($"THIS IS USER: {twizz.User.Username}");

        if (twizz.QuotedTwizzId != null)
            twizz.QuotedTwizz = await _context.Twizzs.FindAsync(twizz.QuotedTwizzId);

        twizz.CreatedAt = DateTime.UtcNow;
        twizz.UpdatedAt = DateTime.UtcNow;

        _context.Twizzs.Add(twizz);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTwizz", new { id = twizz.Id }, twizz);
    }

    // PUT: api/Twizz/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTwizz(int id, Twizz twizz)
    {
        if (id != twizz.Id)
        {
            return BadRequest();
        }

        twizz.UpdatedAt = DateTime.UtcNow;
        _context.Entry(twizz).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TwizzExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Twizz/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTwizz(int id)
    {
        var twizz = await _context.Twizzs.FindAsync(id);
        if (twizz == null)
        {
            return NotFound();
        }

        _context.Twizzs.Remove(twizz);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TwizzExists(int id)
    {
        return _context.Twizzs.Any(e => e.Id == id);
    }
}
