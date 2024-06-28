using HarmonyLib;
using JetBrains.Annotations;
using RabiSquare.RealisticOreGeneration;
using RimWorld.Planet;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[UsedImplicitly]
[HarmonyPatch(typeof(MapGenerator), nameof(MapGenerator.GenerateMap))]
public class MapGenerator_GenerateMap
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static bool Prefix(MapParent parent)
    {
        if (parent == null)
        {
            return true;
        }

        var tile = parent.Tile;
        var tileOreData = BaseSingleTon<WorldOreDataGenerator>.Instance.GetTileOreData(tile);
        foreach (var item in tileOreData.surfaceDistribution)
        {
            var buildingProperties = ThingDef.Named(item.Key)?.building;
            if (buildingProperties == null)
            {
                Log.Error($"[RabiSquare.RealisticOreGeneration]can't find buildingProperties with defName: {item.Key}");
                return true;
            }

            buildingProperties.mineableScatterCommonality = item.Value;
            if (SettingWindow.Instance.settingModel.needShuffleLumpSize)
            {
                buildingProperties.mineableScatterLumpSizeRange =
                    BaseSingleTon<VanillaOreInfoRecorder>.Instance.GetRandomSurfaceLumpSize();
                return true;
            }

            var surfaceOreDataByDefName =
                BaseSingleTon<VanillaOreInfoRecorder>.Instance.GetSurfaceOreDataByDefName(item.Key);
            if (surfaceOreDataByDefName == null)
            {
                return true;
            }

            buildingProperties.mineableScatterLumpSizeRange = surfaceOreDataByDefName.lumpSize;
        }

        if (!Prefs.DevMode)
        {
            return true;
        }

        Log.Message($"hook surface ore gen success in tile: {tile}");
        tileOreData.DebugShowSurfaceDistribution();
        return true;
    }
}