using System.Text.Json.Serialization;

namespace FantasyFootballService.Models.Sleeper;

public class SleeperLeague
{
    [JsonPropertyName("league_id")]
    public string LeagueId { get; set; }
}