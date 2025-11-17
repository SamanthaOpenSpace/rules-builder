namespace Prosperity.Api.Infrastructure.RulesEngine.Models;

public class CptCodeSuggestion
{
    public CptCodeSuggestion(Encounter encounter, string cptCodes, string ruleName)
    {
        Encounter = encounter;
        CptCodes = cptCodes;
        RuleName = ruleName;
    }

    public Encounter Encounter { get; }
    public string CptCodes { get; }
    public string RuleName { get; }
}
