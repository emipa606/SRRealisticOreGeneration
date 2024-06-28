using RimWorld;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

public static class TileExtension
{
    public static bool IsTileOceanOrLake(this int tileId)
    {
        var biomeDef = Find.WorldGrid?.tiles[tileId]?.biome;
        if (biomeDef == null)
        {
            return false;
        }

        return biomeDef == BiomeDefOf.Ocean || biomeDef == BiomeDefOf.Lake;
    }
}