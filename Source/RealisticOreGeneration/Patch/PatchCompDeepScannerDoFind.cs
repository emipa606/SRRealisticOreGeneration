// ******************************************************************
//       /\ /|       @file       PatchCompDeepScannerDoFind.cs
//       \ V/        @brief      to patch CompDeepScanner.DoFind()
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-30 21:37:02
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    [HarmonyPatch(typeof(CompDeepScanner), "DoFind")]
    public class PatchCompDeepScannerDoFind
    {
        /// <summary>
        /// hook deep scanner working speed
        /// </summary>
        [UsedImplicitly]
        [HarmonyPostfix]
        // ReSharper disable once InconsistentNaming
        public static void Postfix(CompDeepScanner __instance)
        {
            var parent = __instance.parent;
            if (parent == null) return;
            WorldOreInfoRecorder.Instance.UndergroundMiningCountIncrease(parent.Tile);
            if (!Prefs.DevMode) return;
            var oreData = WorldOreDataGenerator.Instance.GetTileOreData(parent.Tile);
            Log.Message($"{MsicDef.LogTag}underground mining count increase. tile: {parent.Tile}. " +
                        $"count: {WorldOreInfoRecorder.Instance.GetUndergroundMiningCount(parent.Tile)}");
            Log.Message($"{MsicDef.LogTag}freeCycleCount: {(int) oreData.FreeUndergroundCycleCount}");
        }
    }
}