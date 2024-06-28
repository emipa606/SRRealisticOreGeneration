using JetBrains.Annotations;
using RimWorld;

namespace RabiSquare.RealisticOreGeneration;

[UsedImplicitly]
public class CompPropertiesOreScanner : CompProperties_Scanner
{
    public CompPropertiesOreScanner()
    {
        compClass = typeof(CompOreScanner);
    }
}