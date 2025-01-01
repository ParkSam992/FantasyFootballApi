using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFootballService.Helpers;
using FantasyFootballService.Interfaces;
using FantasyFootballService.Models;
using FantasyFootballService.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace FantasyFootballService.Controllers;

[ApiController]
[Route("/v1")]
public class FantasyFootballController : PostgresControllerBase
{
    /*
     * Current thoughts:
     * Have an endpoint that refreshes the database with all the current / relevant info that I might want
     * Then have endpoints that query that data to search for players, or look for people who fit a stat, etc.
     * Those queries can combo postgresql queries, and proxy calls to sleeper depending on whats needed
     *
     * Could I turn this into a draft assistant?
     * Gather a bunch of different ADP's and create an average
     * Based on Sleeper API looking at who has been drafted and who hasnt
     * calculate the best draft pick for me to grab by comparing Sleeper ADP with average ADP
     *
     * Note: Sleeper API seems to run on a 4-5 minute delay from live
     *  might need to either scrape the draft, or manually enter each pick
     */

    private ILogger _logger;
    private readonly ISleeperService _sleeperService;
    private readonly IQueriesService _queriesService;
    private readonly ICacheService _cache;

    public FantasyFootballController(IConfiguration config, ILogger logger, ISleeperService sleeperService, IQueriesService queriesService, ICacheService cache) : base(config)
    {
        _logger = logger;
        _sleeperService = sleeperService;
        _queriesService = queriesService;
        _cache = cache;
    }

    [HttpGet]
    [Route("/getLeagueRosterRankings/{leagueId}")]
    [ProducesResponseType(typeof(List<LeagueMember>), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> GetLeagueRosterRankings(string leagueId,
        [FromQuery] MarketEnum market = MarketEnum.STD_SLEEPER)
    {
        NpgsqlConnection conn = null;

        try
        {
            conn = OpenConnection();

            var roster = await _sleeperService.GetLeagueRosters(leagueId);
            var playerRankings = await _queriesService.GetPlayerRankingByMarket(conn, market);

            var leagueMembers = roster.Select(lm =>
                new LeagueMember(lm, PlayerDataMapper.GetPlayerListFromIdList(playerRankings, lm.Roster))).ToList();

            leagueMembers =
                PlayerDataMapper.GetLeagueMemberNames(leagueMembers, await _cache.GetSleeperLeagueUsersAsync(leagueId));

            return Ok(leagueMembers);

        }
        finally
        {
            CloseConnection(conn);
        }
    }


    [HttpGet]
    [Route("/getPlayerRankings")]
    [ProducesResponseType(typeof(List<Player>), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> GetPlayerRankings([FromQuery] MarketEnum market = MarketEnum.STD_SLEEPER)
    {
        // TODO?: Should I add sorting here? Or should all the sorting be done on the FE
        NpgsqlConnection conn = null;

        try
        {
            conn = OpenConnection();
            return Ok(await _queriesService.GetPlayerRankingByMarket(conn, market));
        }
        finally
        {
            CloseConnection(conn);
        }
    }

    [HttpPost]
    [Route("/refreshPlayerData")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> RefreshPlayerData()
    {
        var exitCode = await ShellHelper.Bash("bash Scripts/RefreshPlayerData.sh", _logger);
        return Ok(exitCode == 0);
    }

}