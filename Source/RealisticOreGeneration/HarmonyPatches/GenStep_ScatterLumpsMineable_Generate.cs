using HarmonyLib;
using JetBrains.Annotations;
using RabiSquare.RealisticOreGeneration;
using RimWorld;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[UsedImplicitly]
[HarmonyPatch(typeof(GenStep_ScatterLumpsMineable), nameof(GenStep_ScatterLumpsMineable.Generate))]
public class GenStep_ScatterLumpsMineable_Generate
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static bool Prefix(GenStep_ScatterLumpsMineable __instance, Map map)
    {
        if (map == null)
        {
            return true;
        }

        if (Prefs.DevMode)
        {
            Log.Message(
                $"[RabiSquare.RealisticOreGeneration]vanilla countPer10kCellsRange: {__instance.countPer10kCellsRange}");
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

        if (!Prefs.DevMode)
        {
            return true;
        }

        Log.Message($"[RabiSquare.RealisticOreGeneration]hook abundance success in tile: {tile}");
        tileOreData.DebugShowSurfaceFactors();
        Log.Message(
            $"[RabiSquare.RealisticOreGeneration]surfaceMultiplier: {SettingWindow.Instance.settingModel.surfaceMultiplier}");
        Log.Message(
            $"[RabiSquare.RealisticOreGeneration]current countPer10kCellsRange: {__instance.countPer10kCellsRange}");
        return true;
    }
}