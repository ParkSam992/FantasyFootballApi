using System.Text.Json.Serialization;

namespace FantasyFootballService.Models.Sleeper;

public class SleeperLeague
{
    [JsonPropertyName("league_id")]
    public string LeagueIdPropertyName { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("season")]
    public string Season { get; set; }
    
    public string LeagueId
    {
        get => LeagueIdPropertyName;
        set => LeagueIdPropertyName = value;
    }
}