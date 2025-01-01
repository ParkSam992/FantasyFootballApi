using System.Text.Json.Serialization;

namespace FantasyFootballService.Models.Sleeper;

public class SleeperPlayer
{
    protected SleeperPlayer(SleeperPlayer player)
    {
        SleeperId = player.SleeperId;
        FirstName = player.FirstName;
        LastName = player.LastName;
        Position = player.Position;
        Team = player.Team;
    }

    protected SleeperPlayer() {}

    // TODO: I might need the other player Id's at some point 
    [JsonPropertyName("player_id")]
    public string SleeperId { get; set; }
    
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }
    
    [JsonPropertyName("last_name")]
    public string LastName { get; set; }
    
    public string Position { get; set; }
    
    public string Team { get; set; }
}