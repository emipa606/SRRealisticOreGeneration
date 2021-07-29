// ******************************************************************
//       /\ /|       @file       OreData.cs
//       \ V/        @brief      vanilla data of each raw ore
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-26 19:51:04
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

namespace RabiSquare.RealisticOreGeneration
{
    public class OreData
    {
        public readonly string defName;
        public readonly float mineableScatterCommonality; //relative chance to generate this resource lump by mapgen (may should be called weight)
        public readonly float mineableScatterLumpSize; //size of lump
        public readonly float mineableYield; //how many ore we can get in each raw ore
        public readonly float marketValue;

        public OreData(string defName, float mineableScatterCommonality, float mineableScatterLumpSize,
            float mineableYield, float marketValue)
        {
            this.defName = defName;
            this.mineableScatterCommonality = mineableScatterCommonality;
            this.mineableScatterLumpSize = mineableScatterLumpSize;
            this.mineableYield = mineableYield;
            this.marketValue = marketValue;
        }

        public override string ToString()
        {
            return
                $"{MsicDef.LogTag}\ndefName: {defName}\nmineableScatterCommonality: {mineableScatterCommonality}\n" +
                $"mineableScatterLumpSize: {mineableScatterLumpSize}\nmineableYield: {mineableYield}\nmarketValue: {marketValue}";
        }
    }
}