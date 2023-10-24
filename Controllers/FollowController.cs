using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using twizzle.Database;
using twizzle.Models;

namespace twizzle_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FollowController : ControllerBase
{
    private readonly TwizzleDbContext _context;

    public FollowController(TwizzleDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> GetFollows()
    {
        var follows = await _context.Follows.ToListAsync();

        return Ok(follows);
    }


    [HttpPost]
    public async Task<ActionResult<Follow>> FollowUser(Follow follow)
    {
        var followerId = follow.FollowerId;
        var followingId = follow.FollowingId;
        var follower = await _context.Users.FindAsync(followerId);
        var following = await _context.Users.FindAsync(followingId);

        if (follower == null || following == null)
            return BadRequest("Invalid follower or following.");

        follow.Follower = follower;
        follow.Following = following;

        var existingFollow = await _context.Follows
            .Where(f => f.FollowerId == followerId && f.FollowingId == followingId)
            .FirstOrDefaultAsync();

        if (existingFollow != null)
        {
            return BadRequest("The follow relationship already exists.");
        }

        _context.Follows.Add(follow);
        await _context.SaveChangesAsync();

        return CreatedAtAction("FollowUser", new { id = follow.Id }, follow);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> UnfollowUser(int id)
    {
        var follow = await _context.Follows.FindAsync(id);
        if (follow == null)
        {
            return NotFound();
        }

        _context.Follows.Remove(follow);
        await _context.SaveChangesAsync();

        return NoContent();
    }


    [HttpGet("{userId}/followers")]
    public async Task<ActionResult<IEnumerable<User>>> GetFollowers(int userId)
    {
        var followers = await _context.Follows
            .Where(f => f.FollowingId == userId)
            .Select(f => f.Follower)
            .ToListAsync();

        return Ok(followers);
    }

    [HttpGet("{userId}/following")]
    public async Task<ActionResult<IEnumerable<User>>> GetFollowings(int userId)
    {
        var followers = await _context.Follows
            .Where(f => f.FollowerId == userId)
            .Select(f => f.Following)
            .ToListAsync();

        return Ok(followers);
    }
}