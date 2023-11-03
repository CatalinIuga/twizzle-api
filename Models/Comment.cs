using System.Net.NetworkInformation;
using System.Text.Json.Serialization;

namespace twizzle.Models;

public class Comment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TwizzId { get; set; }
    public string Content { get; set; }
    public int? ParentCommentId { get; set; } // can be null since this is options if its in the mai comment thread
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore]
    public Comment Reply { get; set; }

    [JsonIgnore]
    public Twizz Twizz { get; set; }

    [JsonIgnore]
    public Comment ParentComment { get; set; }

    [JsonIgnore]
    public List<Like> Likes = new List<Like>();
}
