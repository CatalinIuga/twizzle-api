using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using twizzle.Database;
using static twizzle.Models.Auth;

namespace twizzle.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TwizzleDbContext _context;

    public AuthController(TwizzleDbContext context)
    {
        _context = context;
    }


    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> SignIn(Login loginModel)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == loginModel.Email);

        if (user == null)
            return NotFound("Email not found!");

        if (user.Password != loginModel.Password)
            return BadRequest("Invalid Password!");

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)

            };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return Ok("Login successfull!");
    }

    [HttpGet]
    [Route("session")]
    public async Task<IActionResult> GetSession()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
            return Unauthorized("User not authenticated.");

        var userIdClaim = authenticateResult.Principal.FindFirst(ClaimTypes.Sid);
        var userNameClaim = authenticateResult.Principal.FindFirst(ClaimTypes.Name);
        var userEmailClaim = authenticateResult.Principal.FindFirst(ClaimTypes.Email);

        if (userNameClaim == null || userEmailClaim == null || userIdClaim == null)
            return Unauthorized("User claims not found.");

        var session = new
        {
            Id = userIdClaim.Value,
            Username = userNameClaim.Value,
            Email = userEmailClaim.Value
        };


        return Ok(session);
    }


    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Ok("Signed out successfully!");
    }

}