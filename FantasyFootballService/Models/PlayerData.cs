using System.Collections.Generic;

namespace FantasyFootballService.Models;

public class PlayerData
{
    public string SleeperId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Position { get; set; }
    public Dictionary<string, Ranking> Rankings { get; set; }
}

public class Ranking
{
    public float OneQBRanking { get; set; }
    public float TwoQBRanking { get; set; }
}