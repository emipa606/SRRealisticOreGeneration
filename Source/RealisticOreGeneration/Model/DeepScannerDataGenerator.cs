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
    public static class DeepScannerDataGenerator
    {
        private const int VanillaScanFindGuaranteedDays = 6; //garanteed find time interval from Buildings_Misc.xml
        private const int VanillaScanFindMtbDays = 3; //random find time interval from Buildings_Misc.xml

        public static void GenerateDeepScannerFindDays(int tileId, out int scanFindGuaranteedDays, out int scanFindMtbDays)
        {
            //default value
            scanFindGuaranteedDays = VanillaScanFindGuaranteedDays;
            scanFindMtbDays = VanillaScanFindMtbDays;
            var tileOreData = WorldOreDataGenerator.GetTileOreData(tileId);
            var freeCycleCount = (int)tileOreData.FreeUndergroundCycleCount;
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
