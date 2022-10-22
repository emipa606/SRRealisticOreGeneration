// ******************************************************************
//       /\ /|       @file       PatchGenStepScatterLumpsMineableGenerate.cs
//       \ V/        @brief      to patch GenStep_ScatterLumpsMineable.Generate()
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-29 12:54:24
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    [HarmonyPatch(typeof(GenStep_ScatterLumpsMineable), "Generate")]
    public class PatchGenStepScatterLumpsMineableGenerate
    {
        /// <summary>
        /// hook countPer10kCellsRange with my abundance
        /// </summary>
        [UsedImplicitly]
        [HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        public static bool Prefix(GenStep_ScatterLumpsMineable __instance, Map map)
        {
            if (map == null) return true;
            if (Prefs.DevMode)
            {
                Log.Message($"{MsicDef.LogTag}vanilla countPer10kCellsRange: {__instance.countPer10kCellsRange}");
            }

            var tileId = map.Tile;
            var tileOreData = WorldOreDataGenerator.Instance.GetTileOreData(tileId);
            __instance.countPer10kCellsRange *=
                WorldOreInfoRecorder.Instance.IsTileAbandoned(tileId) ? 0 : tileOreData.OreGenerationFactor;
            //if no ore generated in vanilla, don't record it 
            if (__instance.maxValue > 0) WorldOreInfoRecorder.Instance.RecordAbandonedTile(tileId);
            if (!Prefs.DevMode) return true;
            Log.Message($"{MsicDef.LogTag}hook abundance success in tile: {tileId}");
            tileOreData.DebugShowSurfaceFactors();
            Log.Message($"{MsicDef.LogTag}surfaceMultiplier: {SettingWindow.Instance.settingModel.surfaceMultiplier}");
            Log.Message($"{MsicDef.LogTag}current countPer10kCellsRange: {__instance.countPer10kCellsRange}");
            return true;
        }
    }
}