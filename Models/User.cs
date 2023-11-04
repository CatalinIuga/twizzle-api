using System.Text.Json.Serialization;

namespace twizzle.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Bio { get; set; }

    // this is gonna be sent from the
    // frontend cloudify library, if its not in the form its just gonna use a default Image.
    // or just get a file insted as input and use the SDK to get the url back
    public string? AvatarURL { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

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
