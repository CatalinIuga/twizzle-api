using System.Text.Json.Serialization;

namespace twizzle.Models;

public class Twizz
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public List<string> MediaURLs { get; set; } = new List<string>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore]
    public List<Comment> Comments { get; set; } = new List<Comment>();

    [JsonIgnore]
    public List<Like> Likes { get; set; } = new List<Like>();
}
