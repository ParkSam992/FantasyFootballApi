using System.Text.Json.Serialization;

namespace FantasyFootballService.Models.Sleeper;

public class SleeperDraft
{
    [JsonPropertyName("draftId")]
    public string DraftId { get; set; }
    [JsonPropertyName("metaData")]
    public DraftMetaData MetaData { get; set; }
}

public class DraftMetaData
{
    [JsonPropertyName("scoring_type")]
    public string ScoringType { get; set; }
}