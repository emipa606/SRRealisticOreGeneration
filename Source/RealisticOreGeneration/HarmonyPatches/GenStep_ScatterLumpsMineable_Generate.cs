using HarmonyLib;
using RabiSquare.RealisticOreGeneration;
using RimWorld;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[HarmonyPatch(typeof(GenStep_ScatterLumpsMineable), nameof(GenStep_ScatterLumpsMineable.Generate))]
public class GenStep_ScatterLumpsMineable_Generate
{
    public static void Prefix(GenStep_ScatterLumpsMineable __instance, Map map)
    {
        if (map == null)
        {
            return;
        }

        var tile = map.Tile;
        var tileOreData = BaseSingleTon<WorldOreDataGenerator>.Instance.GetTileOreData(tile);
        __instance.countPer10kCellsRange *= BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileAbandoned(tile)
            ? 0f
            : tileOreData.OreGenerationFactor;
        if (__instance.maxValue > 0f)
        {
            BaseSingleTon<WorldOreInfoRecorder>.Instance.RecordAbandonedTile(tile);
        }
    }
}