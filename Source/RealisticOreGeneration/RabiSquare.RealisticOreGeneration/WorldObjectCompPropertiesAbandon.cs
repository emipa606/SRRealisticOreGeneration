using JetBrains.Annotations;
using RimWorld;

namespace RabiSquare.RealisticOreGeneration;

[UsedImplicitly]
public class WorldObjectCompPropertiesAbandon : WorldObjectCompProperties
{
    public WorldObjectCompPropertiesAbandon()
    {
        compClass = typeof(WorldObjectCompAbandon);
    }
}