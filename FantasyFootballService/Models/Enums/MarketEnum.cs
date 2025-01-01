using System.Text.Json.Serialization;

namespace FantasyFootballService.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MarketEnum
{
    // TODO: no data for commented out yet, get data then finish
    STD_AVERAGE,
    STD_SLEEPER,
    STD_DYNASTY_DADDY,
    // STD_ESPN,
    // STD_KEEP_TRADE_CUT,
    DYN_AVERAGE,
    DYN_DYNASTY_DADDY,
    DYN_SLEEPER
}