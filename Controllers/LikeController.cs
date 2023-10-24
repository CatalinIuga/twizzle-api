using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using twizzle.Database;
using twizzle.Models;

[Route("api/[controller]")]
[ApiController]
public class LikeController : ControllerBase
{
    private readonly TwizzleDbContext _context;

    public LikeController(TwizzleDbContext context)
    {
        _context = context;
    }

    // GET: api/Like
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Like>>> GetLikes()
    {
        return await _context.Likes.Include(l => l.User).Include(l => l.Twizz).ToListAsync();
    }

    // GET: api/Like/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Like>> GetLike(int id)
    {
        var like = await _context.Likes
            .Include(l => l.User)
            .Include(l => l.Twizz)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (like == null)
        {
            return NotFound();
        }

        return like;
    }

    // POST: api/Like
    [HttpPost]
    public async Task<ActionResult<Like>> PostLike(Like like)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check if the like already exists for the user and twizz
        var existingLike = await _context.Likes
            .Where(l => l.UserId == like.UserId && l.TwizzId == like.TwizzId)
            .FirstOrDefaultAsync();

        if (existingLike != null)
        {
            return BadRequest("The like already exists.");
        }

        var likedTwizz = await _context.Twizzs.FindAsync(like.TwizzId);
        if (likedTwizz == null)
            return BadRequest("User not found.");

        like.User = likedTwizz.User;
        like.Twizz = likedTwizz;

        like.CreatedAt = DateTime.UtcNow;
        like.UpdatedAt = DateTime.UtcNow;

        _context.Likes.Add(like);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetLike", new { id = like.Id }, like);
    }

    // DELETE: api/Like/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLike(int id)
    {
        var like = await _context.Likes.FindAsync(id);
        if (like == null)
        {
            return NotFound();
        }

        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool LikeExists(int id)
    {
        return _context.Likes.Any(e => e.Id == id);
    }
}
