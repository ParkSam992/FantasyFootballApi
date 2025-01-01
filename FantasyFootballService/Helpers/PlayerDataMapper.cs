using System.Collections.Generic;
using System.Linq;
using FantasyFootballService.Models;
using FantasyFootballService.Models.Sleeper;

namespace FantasyFootballService.Helpers;

public class PlayerDataMapper
{
    public static List<Player> GetPlayerListFromIdList(List<Player> players, List<string> playerIds)
    {
        return playerIds.Select(pId =>
        {
            return players.FirstOrDefault(p => p.SleeperId == pId);
        }).Where(p => p != null).ToList();
    }

    public static List<LeagueMember> GetLeagueMemberNames(List<LeagueMember> leagueMembers,
        List<SleeperLeagueUser> sleeperLeagueUsers)
    {
        var updatedLeagueMembers = leagueMembers.Select(lm =>
        {
            var sleeperUser = sleeperLeagueUsers.FirstOrDefault(slu => slu.UserId == lm.OwnerId);
            lm.DisplayName = sleeperUser?.DisplayName;
            lm.TeamName = sleeperUser?.MetaData.TeamName;
            return lm;
        });

        return updatedLeagueMembers.ToList();
    }
}