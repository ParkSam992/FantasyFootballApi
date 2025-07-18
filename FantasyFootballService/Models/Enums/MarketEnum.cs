using System.ComponentModel;
using System.Text.Json.Serialization;

namespace FantasyFootballService.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MarketEnum
{
    [Description("REDRAFT: Average Rankings")]
    STD_AVERAGE,
    [Description("REDRAFT: Sleeper Rankings")]
    STD_SLEEPER,
    [Description("REDRAFT: Dynasty Daddy Average Rankings")]
    STD_DYNASTY_DADDY_AVG,
    [Description("REDRAFT: Keep Trade Cut Rankings")]
    STD_KEEP_TRADE_CUT,
    [Description("REDRAFT: Fantasy Calc Rankings")]
    STD_FANTASY_CALC,
    [Description("DYNASTY: Average Rankings")]
    DYN_AVERAGE,
    [Description("DYNASTY: Dynasty Daddy Average Rankings")]
    DYN_DYNASTY_DADDY_AVG,
    [Description("DYNASTY: Sleeper Rankings")]
    DYN_SLEEPER,
    [Description("DYNASTY: Keep Trade Cut Rankings")]
    DYN_KEEP_TRADE_CUT,
    [Description("DYNASTY: Fantasy Calc Rankings")]
    DYN_FANTASY_CALC,
    [Description("COMBINED: Redraft and Dynasty Rankings Averaged")]
    COMBINED_AVERAGE,
}