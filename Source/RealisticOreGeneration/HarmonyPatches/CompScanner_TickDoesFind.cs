using HarmonyLib;
using RabiSquare.RealisticOreGeneration;
using RimWorld;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[HarmonyPatch(typeof(CompScanner), "TickDoesFind")]
public class CompScanner_TickDoesFind
{
    public static void Prefix(CompScanner __instance)
    {
        var parent = __instance.parent;
        if (parent is not Building building)
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]parent is not building");
            return;
        }

        if (building.def == null)
        {
            Log.Warning($"[RabiSquare.RealisticOreGeneration]def is not null: {building.Label}");
            return;
        }

        if (!building.def.defName.Equals("GroundPenetratingScanner"))
        {
            return;
        }

        var props = __instance.Props;
        if (props == null)
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]comp properties are null");
            return;
        }

        DeepScannerDataGenerator.GenerateDeepScannerFindDays(parent.Tile, out var scanFindGuaranteedDays,
            out var scanFindMtbDays);
        props.scanFindMtbDays = scanFindMtbDays;
        props.scanFindGuaranteedDays = scanFindGuaranteedDays;
    }
}