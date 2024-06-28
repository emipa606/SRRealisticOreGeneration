using RimWorld.Planet;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

public static class WorldUtils
{
    public static void SetWorldLayerDirty<T>() where T : WorldLayer
    {
        Find.World?.renderer?.SetDirty<T>();
    }
}