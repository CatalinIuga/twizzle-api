using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Like>>> GetLikes()
    {
        return await _context.Likes.Include(l => l.User).Include(l => l.Twizz).ToListAsync();
    }

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

    [HttpPost]
    public async Task<ActionResult<Like>> PostLike(Like like)
    {
        var user = await _context.Users.FindAsync(like.UserId);
        if (user == null)
            return BadRequest("User not found.");

        var twizz = await _context.Twizzs.FindAsync(like.TwizzId);
        if (twizz == null)
            return BadRequest("Twizz not found.");

        like.User = user;
        like.Twizz = twizz;

        // Check if the like already exists for the user and twizz
        var existingLike = await _context.Likes
            .Where(l => l.UserId == like.UserId && l.TwizzId == like.TwizzId)
            .FirstOrDefaultAsync();

        if (existingLike != null)
        {
            return BadRequest("The like already exists.");
        }

        var likedTwizz = await _context.Twizzs.FindAsync(like.TwizzId);
        var likedComment = await _context.Comments.FindAsync(like.TwizzId);
        if (likedTwizz != null)
        {
            like.User = likedTwizz.User;
            like.Twizz = likedTwizz;
        }
        else if (likedComment != null)
        {
            like.User = likedComment.User;
            like.Twizz = likedComment.Twizz;
        }
        else
            return BadRequest("User not found.");

        like.CreatedAt = DateTime.UtcNow;
        like.UpdatedAt = DateTime.UtcNow;

        _context.Likes.Add(like);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetLike", new { id = like.Id }, like);
    }

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
}
