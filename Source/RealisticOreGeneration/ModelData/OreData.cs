// ******************************************************************
//       /\ /|       @file       OreData.cs
//       \ V/        @brief      origin data of each ore
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-26 19:51:04
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

namespace RabiSquare.RealisticOreGeneration
{
    public class OreData
    {
        public string defName;
        public float mineableScatterCommonality; //relative chance to generate this resource lump by mapgen
        public float mineableScatterLumpSize; //size of lump
        public float mineableYield;
        public float marketValue;

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
                $"[RabiSquare.RealisticOreGeneration]\ndefName: {defName}\nmineableScatterCommonality: {mineableScatterCommonality}\n" +
                $"mineableScatterLumpSize: {mineableScatterLumpSize}\nmineableYield: {mineableYield}\nmarketValue: {marketValue}";
        }
    }
}