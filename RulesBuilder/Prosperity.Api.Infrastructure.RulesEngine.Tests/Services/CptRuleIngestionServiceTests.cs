using NUnit.Framework.Legacy;
using Prosperity.Api.Infrastructure.RulesEngine;

namespace Prosperity.Api.Infrastructure.Storages.Tests.Services;

public class CptRuleIngestionServiceTests
{
    private InMemoryRuleStore _ruleStore = null!;
    private IDynamicRulesEngine<Encounter, CptCodeOutput> _engine = null!;

    [SetUp]
    public void SetUp()
    {
        var converter = new SqlToLinqConverter();
        var builder = new DynamicRuleBuilder(converter);
        _ruleStore = new InMemoryRuleStore();
        _engine = new DynamicRulesEngine<Encounter, CptCodeOutput>(builder, _ruleStore);
    }

    [Test]
    public async Task IngestAsync_WhenStoreEmpty_AddsAllDefaultRules()
    {
        var ingestionService = new CptRuleIngestionService(_engine, _ruleStore);

        await ingestionService.IngestAsync();

        var storedRules = await _ruleStore.GetAllAsync(DefaultCptRules.RuleSetKey);
        Assert.That(storedRules.Count, Is.EqualTo(DefaultCptRules.All.Count));

        var encounter = new Encounter("Therapy Progress Note", 45, "LCSW");
        var evaluation = await _engine.EvaluateAsync(DefaultCptRules.RuleSetKey, encounter);

        Assert.That(evaluation.HasMatches, Is.True);
        var match = evaluation.Matches.Single(m => m.RuleName == "Therapy 45 min");
        CollectionAssert.AreEquivalent(new[] { "90834", "90836" }, match.Output.CptCodes);
    }

    [Test]
    public async Task IngestAsync_WhenRuleAlreadyExists_DoesNotOverride()
    {
        var existingOutput = new CptCodeOutput(["00000"]);
        await _engine.CreateRuleAsync(
            DefaultCptRules.RuleSetKey,
            "(noteType = 'Group Note')",
            existingOutput,
            "Group Therapy");

        var ingestionService = new CptRuleIngestionService(_engine, _ruleStore);

        await ingestionService.IngestAsync();

        var storedRules = await _ruleStore.GetAllAsync(DefaultCptRules.RuleSetKey);
        Assert.That(storedRules.Count, Is.EqualTo(DefaultCptRules.All.Count));

        var evaluation = await _engine.EvaluateAsync(DefaultCptRules.RuleSetKey, new Encounter("Group Note", 30, "LCSW"));
        var match = evaluation.Matches.Single(m => m.RuleName == "Group Therapy");
        CollectionAssert.AreEquivalent(new[] { "00000" }, match.Output.CptCodes);
    }
}
