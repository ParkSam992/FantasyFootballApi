using System.Collections.Generic;
using FantasyFootballService.Models.Sleeper;

namespace FantasyFootballService.Models;

public class LeagueMember
{
    public LeagueMember(SleeperMemberRoster memberRoster, List<Player> players)
    {
        OwnerId = memberRoster.OwnerId;
        Roster = players;
    }
    
    public string OwnerId { get; set; }
    public string DisplayName { get; set; }
    public string TeamName { get; set; }
    public List<Player> Roster { get; set; }
}

