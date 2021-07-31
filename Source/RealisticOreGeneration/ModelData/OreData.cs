// ******************************************************************
//       /\ /|       @file       OreData.cs
//       \ V/        @brief      vanilla data of each raw ore
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-26 19:51:04
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    public class OreData
    {
        public readonly string defName;

        public readonly float commonality; //relative chance to generate lump by mapgen (may should be called weight)
        public readonly IntRange lumpSize; //size of lump
        public readonly float yield; //how many ore we can get in each raw ore
        public readonly float marketValue;

        public OreData(string defName, float commonality, IntRange lumpSize,
            float @yield, float marketValue)
        {
            this.defName = defName;
            this.commonality = commonality;
            this.lumpSize = lumpSize;
            this.yield = yield;
            this.marketValue = marketValue;
        }

        public override string ToString()
        {
            return $"{MsicDef.LogTag}\ndefName: {defName}\nmineableScatterCommonality: {commonality}\n" +
                   $"lumpSize: ({lumpSize.min},{lumpSize.max})\nmineableYield: {yield}\nmarketValue: {marketValue}";
        }
    }
}