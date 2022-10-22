// ******************************************************************
//       /\ /|       @file       PatchMapGeneratorGenerateMap.cs
//       \ V/        @brief      to patch MapGenerator.GenerateMap()
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-28 17:53:22
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using HarmonyLib;
using JetBrains.Annotations;
using RimWorld.Planet;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    [HarmonyPatch(typeof(MapGenerator), "GenerateMap")]
    public class PatchMapGeneratorGenerateMap
    {
        /// <summary>
        /// hook map gen with new params
        /// </summary>
        [UsedImplicitly]
        [HarmonyPrefix]
        public static bool Prefix(MapParent parent)
        {
            if (parent == null) return true;
            var tileId = parent.Tile;
            var tileOreData = WorldOreDataGenerator.Instance.GetTileOreData(tileId);
            foreach (var kvp in tileOreData.surfaceDistribution)
            {
                var rawOreDef = ThingDef.Named(kvp.Key);
                var buildingProperties = rawOreDef?.building;
                if (buildingProperties == null)
                {
                    Log.Error($"{MsicDef.LogTag}can't find buildingProperties with defName: {kvp.Key}");
                    return true;
                }

                buildingProperties.mineableScatterCommonality = kvp.Value;
                //if need to shuffle lump size
                if (SettingWindow.Instance.settingModel.needShuffleLumpSize)
                {
                    buildingProperties.mineableScatterLumpSizeRange =
                        VanillaOreInfoRecorder.Instance.GetRandomSurfaceLumpSize();
                    return true;
                }

                var vanillaOreData = VanillaOreInfoRecorder.Instance.GetSurfaceOreDataByDefName(kvp.Key);
                if (vanillaOreData == null)
                {
                    return true;
                }

                buildingProperties.mineableScatterLumpSizeRange = vanillaOreData.lumpSize;
            }

            if (!Prefs.DevMode) return true;
            Log.Message($"hook surface ore gen success in tile: {tileId}");
            tileOreData.DebugShowSurfaceDistribution();
            return true;
        }
    }
}