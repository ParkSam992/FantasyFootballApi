using System.Text.Json.Serialization;

namespace FantasyFootballService.Models;

public class SelectOption
{
    [JsonPropertyName("label")]
    public string Label { get; set; }
    [JsonPropertyName("value")]
    public string Value { get; set; }
}