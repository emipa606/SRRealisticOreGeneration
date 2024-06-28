using HarmonyLib;
using JetBrains.Annotations;
using RabiSquare.RealisticOreGeneration;
using RimWorld;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[UsedImplicitly]
[HarmonyPatch(typeof(CompScanner), "TickDoesFind")]
public class CompScanner_TickDoesFind
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static bool Prefix(CompScanner __instance)
    {
        var parent = __instance.parent;
        if (parent is not Building building)
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]parent is not building");
            return true;
        }

        if (building.def == null)
        {
            Log.Warning($"[RabiSquare.RealisticOreGeneration]def is not null: {building.Label}");
            return true;
        }

        if (!building.def.defName.Equals("GroundPenetratingScanner"))
        {
            return true;
        }

        var props = __instance.Props;
        if (props == null)
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]comp properties are null");
            return true;
        }

        DeepScannerDataGenerator.GenerateDeepScannerFindDays(parent.Tile, out var scanFindGuaranteedDays,
            out var scanFindMtbDays);
        props.scanFindMtbDays = scanFindMtbDays;
        props.scanFindGuaranteedDays = scanFindGuaranteedDays;
        return true;
    }
}