// ******************************************************************
//       /\ /|       @file       PatchCompScannerTickDoesFind.cs
//       \ V/        @brief      to patch CompScanner.TickDoesFind()
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-30 13:45:43
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    [HarmonyPatch(typeof(CompScanner), "TickDoesFind")]
    public class PatchCompScannerTickDoesFind
    {
        /// <summary>
        ///     hook deep scanner working speed
        /// </summary>
        [UsedImplicitly]
        [HarmonyPrefix]
        // ReSharper disable once InconsistentNaming
        public static bool Prefix(CompScanner __instance)
        {
            var parent = __instance.parent;
            if (!(parent is Building deepScanner))
            {
                Log.Warning($"{MsicDef.LogTag}parent is not building");
                return true;
            }

            if (deepScanner.def == null)
            {
                Log.Warning($"{MsicDef.LogTag}def is not null: {deepScanner.Label}");
                return true;
            }

            if (!deepScanner.def.defName.Equals(MsicDef.DeepScannerDefName)) return true;

            var compProps = __instance.Props;
            if (compProps == null)
            {
                Log.Warning($"{MsicDef.LogTag}comp properties are null");
                return true;
            }

            DeepScannerDataGenerator.GenerateDeepScannerFindDays(parent.Tile, out var scanFindGuaranteedDays,
                out var scanFindMtbDays);
            compProps.scanFindMtbDays = scanFindMtbDays;
            compProps.scanFindGuaranteedDays = scanFindGuaranteedDays;
            return true;
        }
    }
}