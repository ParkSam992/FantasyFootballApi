using System.Collections.Generic;
using FantasyFootballService.Models.Sleeper;

namespace FantasyFootballService.Models;

public class Draft
{
    private List<string> OneQbScoringTypes = new() { "std", "ppr", "half_ppr", "idp", "dynasty_std", "dynasty_ppr", "dynasty_half_ppr" };
    private List<string> DynastyScoringTypes = new() { "dynasty_std", "dynasty_ppr", "dynasty_half_ppr", "dynasty_2qb" };
    
    public Draft(SleeperDraft sleeperDraft)
    {
        DraftId = sleeperDraft.DraftId;
        IsOneQb = OneQbScoringTypes.Contains(sleeperDraft.MetaData.ScoringType.ToLower());
        IsDynasty = DynastyScoringTypes.Contains(sleeperDraft.MetaData.ScoringType.ToLower());
    }
    
    public Draft() {}
    
    public string DraftId { get; set; }
    public bool IsDynasty { get; set; }
    public bool IsOneQb { get; set; }
}