using HarmonyLib;
using JetBrains.Annotations;
using RabiSquare.RealisticOreGeneration;
using RimWorld;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[UsedImplicitly]
[HarmonyPatch(typeof(CompDeepScanner), "DoFind")]
public class CompDeepScanner_DoFind
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Postfix(CompDeepScanner __instance)
    {
        var parent = __instance.parent;
        if (parent == null)
        {
            return;
        }

        BaseSingleTon<WorldOreInfoRecorder>.Instance.UndergroundMiningCountIncrease(parent.Tile);
        if (!Prefs.DevMode)
        {
            return;
        }

        var tileOreData = BaseSingleTon<WorldOreDataGenerator>.Instance.GetTileOreData(parent.Tile);
        Log.Message(
            $"[RabiSquare.RealisticOreGeneration]underground mining count increase. tile: {parent.Tile}. count: {BaseSingleTon<WorldOreInfoRecorder>.Instance.GetUndergroundMiningCount(parent.Tile)}");
        Log.Message(
            $"[RabiSquare.RealisticOreGeneration]freeCycleCount: {(int)tileOreData.FreeUndergroundCycleCount}");
    }
}