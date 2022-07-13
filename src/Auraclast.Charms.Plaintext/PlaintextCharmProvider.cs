using System.Threading.Tasks;
using System.Collections;
using System;
using Microsoft.Extensions.Logging;
using Auraclast;
using System.Text.RegularExpressions;
using System.IO;

namespace Auraclast.Charms.Plaintext;
public class PlaintextCharmProvider : ICharmProvider
{    
    private ILogger _logger;
    private List<Charm> _charms = new List<Charm>();
    private Task<IEnumerable<Charm>> _results = Task.FromResult<IEnumerable<Charm>>(Array.Empty<Charm>());
    private PlaintextCharmParser _parser;

    public PlaintextCharmProvider(ILogger logger)
    {
        _logger = logger;
        _parser = new PlaintextCharmParser(logger);
    }

    public Task<IEnumerable<Charm>> GetAllCharmsAsync() => _results;

    // probably have to end up calling this automagically via a configuration telling the api service where to look
    public async Task LoadFileAsync(string splatName, string location)
    {
        using var fs = new FileStream(location, FileMode.Open);
        var novel = await _parser.ReadStreamAsync(splatName, fs);
        lock(_charms)
        {
            _charms.AddRange(novel);
            _results = Task.FromResult<IEnumerable<Charm>>(_charms.ToArray());
        }
    }
}