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
using System;
using System.Reflection.Emit;
using FantasyFootballService.Extensions;

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

    [HttpGet]
    [Route("/getMarkets")]
    [ProducesResponseType(typeof(MarketEnum), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public IActionResult GetMarkets()
    {
        var enumValues = Enum.GetValues(typeof(MarketEnum)).Cast<MarketEnum>().ToList();

        return Ok(enumValues.Select(value =>
        {
            var enumDesc = value.GetDescription();
            return new
            {
                Label = enumDesc,
                Value = value
            };
        }));
    }

    [HttpGet]
    [Route("/getDraftInfo/{draftId}")]
    [ProducesResponseType(typeof(List<SleeperDraftedPlayer>), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> GetDraftInfo(string draftId)
    {
        var draft = await _sleeperService.GetDraftInfo(draftId);
        return Ok(new Draft(draft));
    }
    
    [HttpGet]
    [Route("/getDraftedPlayers/{draftId}")]
    [ProducesResponseType(typeof(List<SleeperDraftedPlayer>), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> GetDraftedPlayers(string draftId)
    {
        return Ok(await _sleeperService.GetAlreadyDraftedPlayers(draftId));
    }

    [HttpGet]
    [Route("/playerSearch")]
    [ProducesResponseType(typeof(List<SleeperDraftedPlayer>), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public IActionResult PlayerSearch(string name)
    {
        NpgsqlConnection conn = null;

        try
        {
            conn = OpenConnection();
            return Ok(_queriesService.PlayerSearch(name, conn));
        }
        finally
        {
            CloseConnection(conn);
        }
    }

    // [HttpGet]
    // [Route("/{sleeperId}")]
    // [ProducesResponseType(typeof(List<SleeperDraftedPlayer>), 200)]
    // [ProducesResponseType(typeof(BadRequestResult), 400)]
    // public IActionResult GetPlayerData(string sleeperId)
    // {
    //     NpgsqlConnection conn = null;
    //
    //     try
    //     {
    //         conn = OpenConnection();
    //         return Ok(_queriesService.PlayerSearch(name, conn));
    //     }
    //     finally
    //     {
    //         CloseConnection(conn);
    //     }
    // }

    // TODO: Endpoint to add/remove preferred players (they could be highlighted on the FE)
    
    // TODO: Endpoint to manually enter player rankings (in case I wanted to copy over rankings that arent scrapable)
    // could this be based off a mock draft I fill out myself? give a draftId and it scrapes it to enter rankings?
}