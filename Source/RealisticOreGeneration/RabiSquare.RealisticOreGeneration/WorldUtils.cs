using Verse;

namespace RabiSquare.RealisticOreGeneration;

public static class WorldUtils
{
    public static void SetWorldLayerDirty()
    {
        Find.World?.renderer?.SetAllLayersDirty();
    }
}