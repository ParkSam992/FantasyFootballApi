namespace FantasyFootballService.Helpers;

public class SqlStrings
{
    public const string GET_AVERAGE_RANKINGS = "select get_average_rankings(@market);";

    public const string GET_SLEEPER_RANKINGS = "select get_sleeper_rankings(@market);";

    public const string GET_DYNASTY_DADDY_AVERAGE_RANKINGS = "select get_dynasty_daddy_average_rankings(@market);";

    public const string GET_KEEP_TRADE_CUT_RANKINGS = "select get_keep_trade_cut_rankings(@market);";

    public const string GET_PLAYER_TRADE_VALUE = "select get_player_trade_value();";
}