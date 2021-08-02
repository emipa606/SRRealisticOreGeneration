// ******************************************************************
//       /\ /|       @file       PatchCompScannerCompInspectStringExtra.cs
//       \ V/        @brief      to patch CompScanner.CompInspectStringExtra()
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-31 14:32:40
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************
using System.Text;
using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;

// ReSharper disable All

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    [HarmonyPatch(typeof(CompScanner), "CompInspectStringExtra")]
    public class PatchCompScannerCompInspectStringExtra
    {
        /// <summary>
        /// hook inspection
        /// </summary>
        [UsedImplicitly]
        [HarmonyPostfix]
        public static void Postfix(ref string __result, CompScanner __instance)
        {
            var parent = __instance.parent;
            if (!(parent is Building deepScanner))
            {
                Log.Warning($"{MsicDef.LogTag}parent is not building");
                return;
            }

            if (deepScanner.def == null)
            {
                Log.Warning($"{MsicDef.LogTag}def is not null: {deepScanner.Label}");
                return;
            }

            if (!deepScanner.def.defName.Equals(MsicDef.DeepSannerDefName))
            {
                return;
            }

            var oreData = WorldOreDataGenerator.GetTileOreData(parent.Tile);
            var progress = WorldOreInfoRecorder.Instance.GetUndergroundMiningCount(parent.Tile) /
                           oreData.FreeUndergroundCycleCount;
            if (progress > 0.99f)
            {
                progress = 1f;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(__result);
            stringBuilder.Append("\n");
            stringBuilder.Append("SrUndergroundOreRevealed".Translate());
            stringBuilder.Append(progress.ToStringPercent());
            if (progress >= 0.99f)
            {
                stringBuilder.Append("\n");
                stringBuilder.Append("SrUndergroundOvermining".Translate());
            }

            __result = stringBuilder.ToString();
            return;
        }
    }
}
