// ******************************************************************
//       /\ /|       @file       DeepScannerDataGenerator.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-30 12:40:17
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    public class DeepScannerDataGenerator : BaseSingleTon<DeepScannerDataGenerator>
    {
        private const int VanillaScanFindGuaranteedDays = 6; //garanteed find time interval from Buildings_Misc.xml
        private const int VanillaScanFindMtbDays = 3; //random find time interval from Buildings_Misc.xml

        public static void GenerateDeepScannerFindDays(int tileId, out int scanFindGuaranteedDays, out int scanFindMtbDays)
        {
            //default value
            scanFindGuaranteedDays = VanillaScanFindGuaranteedDays;
            scanFindMtbDays = VanillaScanFindMtbDays;
            var tileOreData = WorldOreInfoRecorder.Instance.GetTileOreData(tileId);
            if (tileOreData == null)
            {
                Log.Warning($"{MsicDef.LogTag}can't find ore info in tile: {tileId}");
                return;
            }

            var freeCycleCount = (int)tileOreData.UndergroundAbundance;
            var currentCycleCount = WorldOreInfoRecorder.Instance.GetUndergroundMiningCount(tileId);
            //free underground ore find
            if (currentCycleCount < freeCycleCount)
            {
                return;
            }

            var factor1 = Mathf.Log(10) / freeCycleCount;
            var factor2 = currentCycleCount - freeCycleCount;
            scanFindGuaranteedDays *= (int)Mathf.Exp(factor1 * factor2);
            scanFindMtbDays *= (int)Mathf.Exp(factor1 * factor2);
        }
    }
}
