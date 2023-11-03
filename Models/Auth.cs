using System.Security.Claims;

namespace twizzle.Models;

public class Auth
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Session
    {
        public int Id;
        public string Username;
        public string Email;
    }
}
