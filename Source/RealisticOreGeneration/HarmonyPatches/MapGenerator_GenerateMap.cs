using HarmonyLib;
using RabiSquare.RealisticOreGeneration;
using RimWorld.Planet;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[HarmonyPatch(typeof(MapGenerator), nameof(MapGenerator.GenerateMap))]
public class MapGenerator_GenerateMap
{
    public static void Prefix(MapParent parent)
    {
        if (parent == null)
        {
            return;
        }

        var tile = parent.Tile;
        var tileOreData = BaseSingleTon<WorldOreDataGenerator>.Instance.GetTileOreData(tile);
        foreach (var item in tileOreData.surfaceDistribution)
        {
            var buildingProperties = ThingDef.Named(item.Key)?.building;
            if (buildingProperties == null)
            {
                Log.Error($"[RabiSquare.RealisticOreGeneration]can't find buildingProperties with defName: {item.Key}");
                return;
            }

            buildingProperties.mineableScatterCommonality = item.Value;
            if (SettingWindow.Instance.settingModel.needShuffleLumpSize)
            {
                buildingProperties.mineableScatterLumpSizeRange =
                    BaseSingleTon<VanillaOreInfoRecorder>.Instance.GetRandomSurfaceLumpSize();
                return;
            }

            var surfaceOreDataByDefName =
                BaseSingleTon<VanillaOreInfoRecorder>.Instance.GetSurfaceOreDataByDefName(item.Key);
            if (surfaceOreDataByDefName == null)
            {
                return;
            }

            buildingProperties.mineableScatterLumpSizeRange = surfaceOreDataByDefName.lumpSize;
        }
    }
}