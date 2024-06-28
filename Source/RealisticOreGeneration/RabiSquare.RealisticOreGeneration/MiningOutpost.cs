using JetBrains.Annotations;
using RimWorld.Planet;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

[UsedImplicitly]
public class MiningOutpost : MapParent
{
    public override MapGeneratorDef MapGeneratorDef => MapGeneratorDefOf.SrMiningOutpost;

    public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
    {
        alsoRemoveWorldObject = Destroyed;
        return Destroyed;
    }
}