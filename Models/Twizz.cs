using System.Text.Json.Serialization;

namespace twizzle.Models;

public class Twizz
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public int? QuotedTwizzId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
    [JsonIgnore]
    public Twizz? QuotedTwizz { get; set; }
    [JsonIgnore]
    public List<Comment> Comments { get; set; } = new List<Comment>();
    [JsonIgnore]
    public List<Like> Likes { get; set; } = new List<Like>();
}