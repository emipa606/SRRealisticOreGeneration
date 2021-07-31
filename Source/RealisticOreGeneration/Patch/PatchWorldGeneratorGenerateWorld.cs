// ******************************************************************
//       /\ /|       @file       PatchWorldGeneratorGenerateWorld.cs
//       \ V/        @brief      to patch WorldGenerator.GenerateWorld()
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-28 17:25:23
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using HarmonyLib;
using JetBrains.Annotations;
using RimWorld.Planet;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    [HarmonyPatch(typeof(WorldGenerator), "GenerateWorld")]
    public class PatchWorldGeneratorGenerateWorld
    {
        /// <summary>
        /// calc info of surface ore and underground ore in each tile
        /// </summary>
        [UsedImplicitly]
        [HarmonyPostfix]
        // ReSharper disable once InconsistentNaming
        public static void Postfix(World __result)
        {
            var world = __result;
            if (world == null)
            {
                Log.Error($"{MsicDef.LogTag}world in generating is null");
                return;
            }

            var worldGrid = world.grid;
            if (worldGrid == null)
            {
                Log.Error($"{MsicDef.LogTag}world grid in generating is null");
                return;
            }

            var allTiles = worldGrid.tiles;
            if (allTiles == null || allTiles.Count <= 0)
            {
                Log.Error($"{MsicDef.LogTag}wrong tile count");
                return;
            }

            for (var i = 0; i < allTiles.Count; i++)
            {
                var surfaceDistrubtion = TileOreDataGenerator.GenerateSurfaceDistrubtion();
                var undergroundDistrubtion = TileOreDataGenerator.GenerateUndergroundDistrubtion();
                var surfaceValueFactor = TileOreDataGenerator.CalcSurfaceValueFactor(surfaceDistrubtion);
                var surfaceBerlinFactor = TileOreDataGenerator.CalcBerlinFactor(i, worldGrid, true);
                var undergroundBerlinFactor = TileOreDataGenerator.CalcBerlinFactor(i, worldGrid, false);
                WorldOreInfoRecorder.Instance.SetTileOreData(i, surfaceBerlinFactor, undergroundBerlinFactor,
                    surfaceValueFactor, surfaceDistrubtion, undergroundDistrubtion);
            }

            if (Prefs.DevMode)
            {
                Log.Message($"hook worldgen success with tile count: {allTiles.Count}");
            }
        }
    }
}