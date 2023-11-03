using System.Text.Json.Serialization;

namespace twizzle.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Bio { get; set; }
    public string? AvatarURL { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public List<Twizz> Twizzs { get; set; } = new List<Twizz>();
    [JsonIgnore]
    public List<Comment> Comments { get; set; } = new List<Comment>();
    [JsonIgnore]
    public List<Like> Likes { get; set; } = new List<Like>();
    [JsonIgnore]
    public List<Follow> Followers { get; set; } = new List<Follow>();
    [JsonIgnore]
    public List<Follow> Followings { get; set; } = new List<Follow>();
}