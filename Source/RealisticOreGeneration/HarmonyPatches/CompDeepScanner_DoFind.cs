using HarmonyLib;
using RabiSquare.RealisticOreGeneration;
using RimWorld;

namespace RealisticOreGeneration.HarmonyPatches;

[HarmonyPatch(typeof(CompDeepScanner), "DoFind")]
public class CompDeepScanner_DoFind
{
    public static void Postfix(CompDeepScanner __instance)
    {
        var parent = __instance.parent;
        if (parent == null)
        {
            return;
        }

        BaseSingleTon<WorldOreInfoRecorder>.Instance.UndergroundMiningCountIncrease(parent.Tile);
    }
}