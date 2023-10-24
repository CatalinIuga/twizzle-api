using System.Text.Json.Serialization;

namespace twizzle.Models;

public class Like
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TwizzId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
    [JsonIgnore]
    public Twizz? Twizz { get; set; }
}