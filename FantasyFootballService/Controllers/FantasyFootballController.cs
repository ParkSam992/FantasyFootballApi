using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFootballService.Helpers;
using FantasyFootballService.Interfaces;
using FantasyFootballService.Models;
using FantasyFootballService.Models.Enums;
using FantasyFootballService.Models.Sleeper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace FantasyFootballService.Controllers;

[ApiController]
[Route("/v1")]
public class FantasyFootballController : PostgresControllerBase
{
    private readonly ILogger _logger;
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
    [Route("/getUserLeagues/{username}/{sport}/{year}")]
    [ProducesResponseType(typeof(List<SleeperLeague>), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> GetUserLeagues(string username, string sport, string year)
    {
        var user = await _sleeperService.GetUserByUsername(username);
        var leagues = await _sleeperService.GetUserLeagues(user.UserId, sport, year);
        return Ok(leagues);
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
            var playerRankings = _queriesService.GetPlayerRankingByMarket(conn, market);

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
    public IActionResult GetPlayerRankings([FromQuery] MarketEnum market = MarketEnum.STD_SLEEPER)
    {
        // TODO?: Should I add sorting here? Or should all the sorting be done on the FE
        NpgsqlConnection conn = null;

        try
        {
            conn = OpenConnection();
            return Ok(_queriesService.GetPlayerRankingByMarket(conn, market));
        }
        finally
        {
            CloseConnection(conn);
        }
    }

    [HttpGet]
    [Route("/getPlayerTradeValue")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public IActionResult GetPlayerTradeValue()
    {
        NpgsqlConnection conn = null;

        try
        {
            conn = OpenConnection();
            return Ok(_queriesService.GetPlayerTradeValue(conn));
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