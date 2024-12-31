using System.Text.Json.Serialization;

namespace FantasyFootballService.Models;

public class LeagueRoster
{
    [JsonPropertyName("owner_id")]
    public string OwnerId { get; set; }
}