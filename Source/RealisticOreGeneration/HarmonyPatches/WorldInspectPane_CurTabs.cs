using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;
using RabiSquare.RealisticOreGeneration.UI.Planet;
using RimWorld.Planet;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[UsedImplicitly]
[HarmonyPatch(typeof(WorldInspectPane), nameof(WorldInspectPane.CurTabs), MethodType.Getter)]
public class WorldInspectPane_CurTabs
{
    private static readonly WITab oreTileInfoTab = new OreTileInfoTab();

    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Postfix(WorldInspectPane __instance, ref IEnumerable<InspectTabBase> __result,
        WITab[] ___TileTabs)
    {
        if (__result == null)
        {
            return;
        }

        if (Find.WorldSelector == null)
        {
            Log.Error("[RabiSquare.RealisticOreGeneration]cant't find worldSelector");
            return;
        }

        var numSelectedObjects = Find.WorldSelector.NumSelectedObjects;
        var selectedTile = Find.WorldSelector.selectedTile;
        if (numSelectedObjects != 0 || selectedTile < 0)
        {
            return;
        }

        if (___TileTabs == null || ___TileTabs.Length == 0)
        {
            Log.Error("[RabiSquare.RealisticOreGeneration]empty ___TileTabs");
            return;
        }

        var array = new WITab[___TileTabs.Length + 1];
        for (var i = 0; i < ___TileTabs.Length; i++)
        {
            array[i] = ___TileTabs[i];
        }

        array[___TileTabs.Length] = oreTileInfoTab;
        __result = array;
    }
}