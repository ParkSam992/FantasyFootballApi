using FantasyFootballService.Models.Sleeper;

namespace FantasyFootballService.Models;

public class Player
{
    public Player(MarketPlayerRanking ranking, SleeperPlayer player)
    {
        Market = ranking.Market;
        OneQbRanking = ranking.OneQbRanking;
        TwoQbRanking = ranking.TwoQbRanking;
        SleeperId = player.SleeperId;
        FirstName = player.FirstName;
        LastName = player.LastName;
        Position = player.Position;
    }

    public Player() {}

    // TODO: I might need the other player Id's at some point 
    public string SleeperId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Position { get; set; }
    public string Market { get; set; }
    public string OneQbRanking { get; set; }
    public string TwoQbRanking { get; set; }
}