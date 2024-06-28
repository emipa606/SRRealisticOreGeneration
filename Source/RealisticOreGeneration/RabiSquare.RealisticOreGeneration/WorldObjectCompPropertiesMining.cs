using JetBrains.Annotations;
using RimWorld;

namespace RabiSquare.RealisticOreGeneration;

[UsedImplicitly]
public class WorldObjectCompPropertiesMining : WorldObjectCompProperties
{
    public WorldObjectCompPropertiesMining()
    {
        compClass = typeof(WorldObjectCompMining);
    }
}