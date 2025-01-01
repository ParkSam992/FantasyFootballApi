using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FantasyFootballService.Models.Sleeper;

public class SleeperMemberRoster
{
    [JsonPropertyName("owner_id")]
    public string OwnerId { get; set; }
    
    [JsonPropertyName("players")]
    public List<string> Roster { get; set; }
}