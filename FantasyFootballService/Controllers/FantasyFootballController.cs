using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyFootballService.Implementations;
using FantasyFootballService.Interfaces;
using FantasyFootballService.Models;
using Microsoft.AspNetCore.Mvc;

namespace FantasyFootballService.Controllers;

[ApiController]
[Route("/v1")]
public class FantasyFootballController : ControllerBase
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

    private readonly ISleeperApi _sleeperApi;

    public FantasyFootballController()
    {
        _sleeperApi = new SleeperApi();
    }

    [HttpGet]
    [Route("/getLeagueRosters/{leagueId}")]
    [ProducesResponseType(typeof(List<LeagueRoster>), 200)]
    [ProducesResponseType(typeof(BadRequestResult), 400)]
    public async Task<IActionResult> GetLeagueRosters(string leagueId)
    {
        // TODO: I want to manipulate the data so that I am returning player data here as well
        //       maybe should cache all player data somewhere bc Ill need it on a bunch of endpoints
        var roster = await _sleeperApi.GetLeagueRosters(leagueId);
        return Ok(roster);
    }
}