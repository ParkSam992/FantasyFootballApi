using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FantasyFootballService.Models.Sleeper;

public class SleeperGraphqlResponse
{
    [JsonPropertyName("data")]
    public SleeperData Data { get; set; }
}

public class SleeperData
{
    [JsonPropertyName("draftPicks")]
    public List<SleeperDraftedPlayer> DraftPicks { get; set; }
    [JsonPropertyName("draftInfo")]
    public SleeperDraft DraftInfo { get; set; }
    
}