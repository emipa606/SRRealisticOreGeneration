// ******************************************************************
//       /\ /|       @file       PatchCompDeepScannerChooseLumpThingDef.cs
//       \ V/        @brief      to patch CompDeepScanner.ChooseLumpThingDef()
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-30 13:18:05
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    [HarmonyPatch(typeof(CompDeepScanner), "ChooseLumpThingDef")]
    public class PatchCompDeepScannerChooseLumpThingDef
    {
        /// <summary>
        /// hook underground ore choosing
        /// </summary>
        [UsedImplicitly]
        [HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        public static bool Prefix(CompDeepScanner __instance)
        {
            var parent = __instance.parent;
            if (parent == null) return true;
            var tileId = parent.Tile;
            var tileOreData = WorldOreDataGenerator.Instance.GetTileOreData(tileId);
            foreach (var kvp in tileOreData.undergroundDistribution)
            {
                var oreDef = ThingDef.Named(kvp.Key);
                if (oreDef == null)
                {
                    Log.Error($"{MsicDef.LogTag}can't find oreDef with defName: {kvp.Key}");
                    return true;
                }

                oreDef.deepCommonality = kvp.Value;
                if (SettingWindow.Instance.settingModel.needShuffleLumpSize)
                    oreDef.deepLumpSizeRange = VanillaOreInfoRecorder.Instance.GetRandomUndergroundLumpSize();
            }

            if (!Prefs.DevMode) return true;
            Log.Message($"hook underground ore gen success in tile: {tileId}");
            tileOreData.DebugShowUndergroundDistribution();
            return true;
        }
    }
}