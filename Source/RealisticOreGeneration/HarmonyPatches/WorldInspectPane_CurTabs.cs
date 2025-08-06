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

    public static IEnumerable<InspectTabBase> Postfix(IEnumerable<InspectTabBase> values)
    {
        if (!values.Any())
        {
            yield break;
        }

        foreach (var value in values)
        {
            yield return value;
        }

        if (Find.WorldSelector == null)
        {
            Log.Error("[RabiSquare.RealisticOreGeneration]cant't find worldSelector");
            yield break;
        }

        var numSelectedObjects = Find.WorldSelector.NumSelectedObjects;
        var selectedTile = Find.WorldSelector.SelectedTile;
        if (numSelectedObjects != 0 || selectedTile < 0)
        {
            yield break;
        }

        yield return oreTileInfoTab;
    }
}