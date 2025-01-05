using System.ComponentModel;
using System.Text.Json.Serialization;

namespace FantasyFootballService.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MarketEnum
{
    // TODO: no data for commented out yet, get data then finish
    [Description("Average Rankings for Redraft League")]
    STD_AVERAGE,
    [Description("Sleeper Rankings for Redraft League")]
    STD_SLEEPER,
    [Description("Average of all Dynasty Daddy Rankings for Redraft League")]
    STD_DYNASTY_DADDY,
    // STD_ESPN,
    // STD_KEEP_TRADE_CUT,
    [Description("Average Rankings for Dynasty League")]
    DYN_AVERAGE,
    [Description("Average of all Dynasty Daddy rankings for Dynasty League")]
    DYN_DYNASTY_DADDY,
    [Description("Sleeper Rankings for Dynasty League")]
    DYN_SLEEPER
}