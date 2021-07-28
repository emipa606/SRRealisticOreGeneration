// ******************************************************************
//       /\ /|       @file       PatchMapGeneratorGenerateMap.cs
//       \ V/        @brief      to patch Map.GenerateMap()
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
        /// hook mapgen with new params
        /// </summary>
        [UsedImplicitly]
        [HarmonyPrefix]
        public static bool Prefix(MapParent parent)
        {
            var tileId = parent.Tile;
            var surfaceOreAbundant = OreInfoRecoder.Instance.GetSurfaceOreAbundant(tileId);
            if (surfaceOreAbundant == null)
            {
                Log.Warning($"{CoreDef.LogTag}can't find ore info in tile: {tileId}");
                return true;
            }
            //todo test
            foreach (var kvp in surfaceOreAbundant)
            {
                Log.Warning($"surface ore: {kvp.Key}\ncommonality: {kvp.Value}");
            }

            var result1 = OreInfoRecoder.Instance.GetUndergroundOreAbundant(tileId);
            foreach (var kvp in result1)
            {
                Log.Warning($"underground ore: {kvp.Key}\ncommonality: {kvp.Value}");
            }
            //
            foreach (var abundance in surfaceOreAbundant)
            {
                var rawOreDef = ThingDef.Named(abundance.Key);
                if (rawOreDef == null)
                {
                    Log.Error($"{CoreDef.LogTag}can't find rawOreDef with defName: {abundance.Key}");
                    return true;
                }

                var buildingProperties = rawOreDef.building;
                if (buildingProperties == null)
                {
                    Log.Error($"{CoreDef.LogTag}can't find buildingProperties with defName: {abundance.Key}");
                    return true;
                }

                buildingProperties.mineableScatterCommonality = abundance.Value;
            }

            if (Prefs.DevMode)
            {
                Log.Message($"hook mapgen success in tile: {tileId}");
            }

            return true;
        }
    }
}
