using System.Text;
using HarmonyLib;
using JetBrains.Annotations;
using RabiSquare.RealisticOreGeneration;
using RimWorld;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[UsedImplicitly]
[HarmonyPatch(typeof(CompScanner), nameof(CompScanner.CompInspectStringExtra))]
public class CompScanner_CompInspectStringExtra
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Postfix(ref string __result, CompScanner __instance)
    {
        var parent = __instance.parent;
        if (parent is not Building building)
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]parent is not building");
        }
        else if (building.def == null)
        {
            Log.Warning($"[RabiSquare.RealisticOreGeneration]def is not null: {building.Label}");
        }
        else if (building.def.defName.Equals("GroundPenetratingScanner"))
        {
            var tileOreData = BaseSingleTon<WorldOreDataGenerator>.Instance.GetTileOreData(parent.Tile);
            var num = BaseSingleTon<WorldOreInfoRecorder>.Instance.GetUndergroundMiningCount(parent.Tile) /
                      tileOreData.FreeUndergroundCycleCount;
            if (num > 0.99f)
            {
                num = 1f;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(__result);
            stringBuilder.Append("\n");
            stringBuilder.Append("SrUndergroundOreRevealed".Translate());
            stringBuilder.Append(num.ToStringPercent());
            if (num >= 0.99f)
            {
                stringBuilder.Append("\n");
                stringBuilder.Append("SrUndergroundOvermining".Translate());
            }

            __result = stringBuilder.ToString();
        }
    }
}