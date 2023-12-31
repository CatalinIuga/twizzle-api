using System.Text.Json.Serialization;

namespace twizzle.Models;

public class Follow
{
    public int Id { get; set; }
    public int FollowerId { get; set; }
    public int FollowingId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public User? Follower { get; set; }

    [JsonIgnore]
    public User? Following { get; set; }
}
