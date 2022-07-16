using System.Threading.Tasks;
using System.Collections;
using System;
using Microsoft.Extensions.Logging;
using Auraclast;
using System.Text.RegularExpressions;

namespace Auraclast.Charms.Plaintext;

/// <summary>
///  this class probably doesn't have to exist? can be combined with its ParserState
/// </summary>
public class PlaintextCharmParser
{
    private ILogger _logger;

    private static Regex _captureCategory = new Regex(CATEGORY_CAPTURE, RegexOptions.Compiled);
    private static Regex _captureName = new Regex(NAME_CAPTURE, RegexOptions.Compiled);
    private static Regex _captureTypeResonance = new Regex(ALCHEMICAL_TYPE_RESONANCE_CAPTURE, RegexOptions.Compiled);
    private static Regex _replaceResonance = new Regex(ALCHEMICAL_RESONANCE_PREFIX, RegexOptions.Compiled);
    private static Regex _captureResonance = new Regex(RESONANCE_CAPTURE, RegexOptions.Compiled);
    private static Regex _captureSystem = new Regex(SYSTEM_CAPTURE, RegexOptions.Compiled);
    private static Regex _captureEnhancement = new Regex(ENHANCEMENT_CAPTURE, RegexOptions.Compiled);
    private static Regex _captureEssence = new Regex(ESSENCE_CAPTURE, RegexOptions.Compiled);

    private const string CATEGORY_CAPTURE = "^(.{0,40}) Charms$"; // the 40 character limit is a total hack
    private const string NAME_CAPTURE = "^(.+)\\((•+)\\)\\s*$";
    private const string ALCHEMICAL_TYPE_RESONANCE_CAPTURE = "^Type:\\s*(\\w+)(;\\s*Resonance:.+)?";
    private const string ALCHEMICAL_RESONANCE_PREFIX = ";\\s*Resonance:\\s*";
    private const string RESONANCE_CAPTURE = "^Resonance:\\s*(.+)";

    private const string SYSTEM_CAPTURE = "^System:\\s*(.+)$";
    private const string ENHANCEMENT_CAPTURE = "^(.+) \\(([0-9]+)xp\\):\\s*(.+)";
    private const string ESSENCE_CAPTURE = "^A.+must have Essence ([0-9])\\+ to purchase this Charm.";

    private delegate void StageParser(string line, ParserState state);

    private StageParser[] _parseStages;

    public PlaintextCharmParser(ILogger logger) 
    {
        _logger = logger;
        _parseStages = new StageParser[] {
            StageZero,
            StageOne,
            StageTwo,
            StageThree,
            StageFour,
            StageFive,

        };
    }

    public async Task<List<Charm>> ReadStreamAsync(string splatName, Stream inputStream)
    {
        try
        {
            return await InnerReadAsync(splatName, inputStream);
        }
        catch(Exception exc)
        {
            _logger.LogError(exc, "Unexpected exception in PlaintextCharmParser.ReadStreamAsync");
            throw;
        }
    }

    private async Task<List<Charm>> InnerReadAsync(string splatName, Stream inputStream)
    {
        var retVal = new List<Charm>();
        var state = new ParserState() { Stage = 0, Splat = splatName };

        using var reader = new StreamReader(inputStream);
        while(!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if(string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            if(state.Stage > 0 && UnexpectedCategory(line, out var category))
            {
                retVal.Add(state.FinalizeCharm());
                state.Category = category;
                state.Stage = 1;
                continue;
            }
            if(state.Stage > 1 && UnexpectedName(line))
            {
                retVal.Add(state.FinalizeCharm());
                state.Stage = 1;
            }
            _parseStages[state.Stage](line, state);
        }
        // the final charm is wherever we find EOF
        retVal.Add(state.FinalizeCharm());
        return retVal;
    }

    private void StageZero(string line, ParserState state) 
    {
        var match = _captureCategory.Match(line);
        if(match.Success)
        {
            state.Category = match.Groups[1].Captures[0].Value;
            state.Stage = 1;
        }
        else
        {
            _logger.LogWarning("Found non-category line in StageZero: {line}", line);
            state.Stage = 0;
        }
    }

    private void StageOne(string line, ParserState state) 
    {
        var match = _captureName.Match(line);
        if(match.Success)
        {
            state.Charm.Name = match.Groups[1].Captures[0].Value;
            var dots = match.Groups[2].Captures[0].Value;
            state.Charm.Dots = dots.Length;
            state.Stage = 2;
        }
        else
        {
            _logger.LogWarning("Found non-name line in StageOne: {line}", line);
            state.Stage = 1;
        }
    }

    private bool UnexpectedCategory(string line, out string category)
    {
        var match = _captureCategory.Match(line);
        if(match.Success)
        {
            category = match.Groups[1].Captures[0].Value;
            return true;
        }
        else
        {
            category = default!;
            return false;
        }
    }
    private bool UnexpectedName(string line) => _captureName.Match(line).Success;

    private void StageTwo(string line, ParserState state) 
    {
        var match = _captureResonance.Match(line);
        if(match.Success) 
        {
            var res = match.Groups[1].Captures[0].Value;
            state.Charm.Resonance = res.Split(',').Select(x => x.Trim()).ToArray();
            state.Stage = 3;
            return;
        }
        var alchMatch = _captureTypeResonance.Match(line);
        if(alchMatch.Success)
        {
            state.Charm.Type = alchMatch.Groups[1].Captures[0].Value;
            if(alchMatch.Groups[2].Success)
            {
                var res = _replaceResonance.Replace(alchMatch.Groups[2].Captures[0].Value, string.Empty);
                state.Charm.Resonance = res.Split(',').Select(x => x.Trim()).ToArray();
            }
            state.Stage = 3;
        }
        else
        {
            // _logger.LogInformation("Found non-resonance, non-type line in StageTwo: {line}", line);
            state.Charm.Flavor = ExtendTextBlock(state.Charm.Flavor, line);
            state.Stage = 3;
        }
    }

    private static string ExtendTextBlock(string orig, string novel)
    {
        // with pdf export, we don't actually know where intended newlines are
        // we'll simply assume that they exist wherever we find a '.' at the end of the existing content
        if(orig.EndsWith('.')) {
            return orig + "\n" + novel;
        } else {
            return (string.IsNullOrEmpty(orig)) ? novel : (orig + " " + novel);
        }
    }

    private void StageThree(string line, ParserState state) 
    {
        var match = _captureSystem.Match(line);
        if(match.Success)
        {
            // we've transitioned to the System: block
            state.Charm.System = match.Groups[1].Captures[0].Value;
            state.Stage = 4;
        }
        else
        {
            state.Charm.Flavor = ExtendTextBlock(state.Charm.Flavor, line);
        }
    }

    private void StageFour(string line, ParserState state) 
    {
        if(TryGetEssenceRequirement(line, out var ess)) {
            state.Charm.Essence = ess;
        } else {
            var match = _captureEnhancement.Match(line);
            if(match.Success)
            {
                // we've transitioned to a new enhancement
                var name = match.Groups[1].Captures[0].Value;
                var xp = int.TryParse(match.Groups[2].Captures[0].Value, out int val) ? val : 0;
                state.Enhancements.Add(new CharmEnhancement { Name = name, Cost = xp, System = match.Groups[3].Captures[0].Value });
                state.Stage = 5;
            }
            else
            {
                state.Charm.System = ExtendTextBlock(state.Charm.System, line);
            }
        }
    }

    private void StageFive(string line, ParserState state) 
    {
        if(TryGetEssenceRequirement(line, out var ess)) {
            state.Charm.Essence = ess;
        }
        else {
            var match = _captureEnhancement.Match(line);
            if(match.Success)
            {
                // we've transitioned to a new enhancement
                var name = match.Groups[1].Captures[0].Value;
                var xp = int.TryParse(match.Groups[2].Captures[0].Value, out int val) ? val : 0;
                state.Enhancements.Add(new CharmEnhancement { Name = name, Cost = xp, System = match.Groups[3].Captures[0].Value });
            }
            else
            {
                var last = state.Enhancements.Last();
                last.System = ExtendTextBlock(last.System, line);
            }
        }
    }

    private bool TryGetEssenceRequirement(string line, out int Essence) {
        var match = _captureEssence.Match(line);
        if(match.Success) {
            return int.TryParse(match.Groups[1].Captures[0].Value, out Essence);
        } else {
            Essence = default;
            return false;
        }
    }
}
