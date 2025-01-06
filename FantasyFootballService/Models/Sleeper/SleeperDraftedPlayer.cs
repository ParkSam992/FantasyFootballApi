using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FantasyFootballService.Models.Sleeper;

public class SleeperGraphqlResponse
{
    [JsonPropertyName("data")]
    public SleeperDraftPicks Data { get; set; }
}

public class SleeperDraftPicks
{
    [JsonPropertyName("draftPicks")]
    public List<SleeperDraftedPlayer> DraftPicks { get; set; }
}

public class SleeperDraftedPlayer
{
    [JsonPropertyName("sleeperId")]
    public string SleeperId { get; set; }
    [JsonPropertyName("draftId")]
    public string DraftId { get; set; }
    [JsonPropertyName("pickNumber")]
    public int PickNumber { get; set; }
    [JsonPropertyName("pickedBy")]
    public string PickedBy { get; set; }
    [JsonPropertyName("isKeeper")]
    public bool? IsKeeper { get; set; }
}