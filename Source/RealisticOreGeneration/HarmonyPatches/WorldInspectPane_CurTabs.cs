using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RabiSquare.RealisticOreGeneration.UI.Planet;
using RimWorld.Planet;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[HarmonyPatch(typeof(WorldInspectPane), nameof(WorldInspectPane.CurTabs), MethodType.Getter)]
public class WorldInspectPane_CurTabs
{
    private static readonly WITab oreTileInfoTab = new OreTileInfoTab();

    public static void Postfix(ref IEnumerable<InspectTabBase> __result, WorldInspectPane __instance)
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
        var selectedTile = Find.WorldSelector.SelectedTile;
        if (numSelectedObjects != 0 || selectedTile < 0)
        {
            return;
        }

        if (__instance.CurTabs == null || !__instance.CurTabs.Any())
        {
            Log.Error("[RabiSquare.RealisticOreGeneration]empty ___TileTabs");
            return;
        }

        var tileTabs = __instance.CurTabs.ToArray();

        var array = new InspectTabBase[tileTabs.Length + 1];
        for (var i = 0; i < tileTabs.Length; i++)
        {
            array[i] = tileTabs[i];
        }

        array[tileTabs.Length] = oreTileInfoTab;
        __result = array;
    }
}