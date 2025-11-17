namespace Prosperity.Api.Infrastructure.RulesEngine.Models;

public class Encounter
{
    public string NoteType { get; set; } = string.Empty;
    public int EncounterDuration { get; set; }
    public string ProviderCredentials { get; set; } = string.Empty;

    public Encounter(string noteType, int encounterDuration, string providerCredentials)
    {
        NoteType = noteType;
        EncounterDuration = encounterDuration;
        ProviderCredentials = providerCredentials;
    }
}
