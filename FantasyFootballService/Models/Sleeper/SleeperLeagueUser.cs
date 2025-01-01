using System.Text.Json.Serialization;

namespace FantasyFootballService.Models.Sleeper;

public class SleeperLeagueUser
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
    
    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; }
    
    [JsonPropertyName("metadata")]
    public SleeperLeagueUserMetaData MetaData { get; set; } 
}

public class SleeperLeagueUserMetaData
{
    [JsonPropertyName("team_name")]
    public string TeamName { get; set; }
}