namespace Auraclast;
public class Charm
{
    public string Splat {get;set;} = string.Empty;
    public string Category {get;set;} = string.Empty;
    public string Name {get;set;} = string.Empty;
    public int Dots {get;set;} = 0;
    public int Essence {get;set;} = 1;
    public string Type {get;set;} = string.Empty;
    public string[] Resonance {get;set;} = Array.Empty<string>();
    public string Flavor {get;set;} = string.Empty;
    public string System {get;set;} = string.Empty;
    public CharmEnhancement[] Enhancements {get;set;} = Array.Empty<CharmEnhancement>();
}
