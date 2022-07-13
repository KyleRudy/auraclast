namespace Auraclast.Charms.Plaintext;

internal class ParserState
{
    public string Category {get;set;} = string.Empty;
    public Charm Charm {get;set;} = new Charm();
    public List<CharmEnhancement> Enhancements {get;set;} = new List<CharmEnhancement>();
    public int Stage {get;set;} = 0;
    public string Splat {get;set;} = string.Empty;

    public Charm FinalizeCharm()
    {
        var retVal = Charm;
        retVal.Category = Category;
        retVal.Enhancements = Enhancements.ToArray();
        retVal.Splat = Splat;
        Enhancements.Clear();
        Charm = new Charm();
        return retVal;
    }
}