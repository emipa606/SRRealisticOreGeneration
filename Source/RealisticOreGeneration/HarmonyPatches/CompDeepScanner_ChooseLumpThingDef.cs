using HarmonyLib;
using RabiSquare.RealisticOreGeneration;
using RimWorld;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[HarmonyPatch(typeof(CompDeepScanner), "ChooseLumpThingDef")]
public class CompDeepScanner_ChooseLumpThingDef
{
    public static void Prefix(CompDeepScanner __instance)
    {
        var parent = __instance.parent;
        if (parent == null)
        {
            return;
        }

        var tile = parent.Tile;
        var tileOreData = BaseSingleTon<WorldOreDataGenerator>.Instance.GetTileOreData(tile);
        foreach (var item in tileOreData.undergroundDistribution)
        {
            var thingDef = ThingDef.Named(item.Key);
            if (thingDef == null)
            {
                Log.Error($"[RabiSquare.RealisticOreGeneration]can't find oreDef with defName: {item.Key}");
                return;
            }

            thingDef.deepCommonality = item.Value;
            if (SettingWindow.Instance.settingModel.needShuffleLumpSize)
            {
                thingDef.deepLumpSizeRange =
                    BaseSingleTon<VanillaOreInfoRecorder>.Instance.GetRandomUndergroundLumpSize();
            }
        }
    }
}