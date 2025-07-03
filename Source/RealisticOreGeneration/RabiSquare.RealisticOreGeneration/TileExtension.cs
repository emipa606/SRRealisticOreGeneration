using System.Linq;
using RimWorld;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

public static class TileExtension
{
    public static bool IsTileOceanOrLake(this int tileId)
    {
        var biomeDef = Find.WorldGrid?.Tiles.FirstOrDefault(tile => tile.tile.tileId == tileId)?.PrimaryBiome;
        if (biomeDef == null)
        {
            return false;
        }

        return biomeDef == BiomeDefOf.Ocean || biomeDef == BiomeDefOf.Lake;
    }
}