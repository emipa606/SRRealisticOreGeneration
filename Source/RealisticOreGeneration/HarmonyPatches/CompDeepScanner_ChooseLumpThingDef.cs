using HarmonyLib;
using JetBrains.Annotations;
using RabiSquare.RealisticOreGeneration;
using RimWorld;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[UsedImplicitly]
[HarmonyPatch(typeof(CompDeepScanner), "ChooseLumpThingDef")]
public class CompDeepScanner_ChooseLumpThingDef
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static bool Prefix(CompDeepScanner __instance)
    {
        var parent = __instance.parent;
        if (parent == null)
        {
            return true;
        }

        var tile = parent.Tile;
        var tileOreData = BaseSingleTon<WorldOreDataGenerator>.Instance.GetTileOreData(tile);
        foreach (var item in tileOreData.undergroundDistribution)
        {
            var thingDef = ThingDef.Named(item.Key);
            if (thingDef == null)
            {
                Log.Error($"[RabiSquare.RealisticOreGeneration]can't find oreDef with defName: {item.Key}");
                return true;
            }

            thingDef.deepCommonality = item.Value;
            if (SettingWindow.Instance.settingModel.needShuffleLumpSize)
            {
                thingDef.deepLumpSizeRange =
                    BaseSingleTon<VanillaOreInfoRecorder>.Instance.GetRandomUndergroundLumpSize();
            }
        }

        if (!Prefs.DevMode)
        {
            return true;
        }

        Log.Message($"hook underground ore gen success in tile: {tile}");
        tileOreData.DebugShowUndergroundDistribution();
        return true;
    }
}