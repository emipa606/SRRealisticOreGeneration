// ******************************************************************
//       /\ /|       @file       TileOreData.cs
//       \ V/        @brief      ore info in tile
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-29 11:33:38
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************
using System.Collections.Generic;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    public class TileOreData
    {
        private readonly float berlinFactor;
        private readonly float surfaceValueFactor;
        private readonly float undergroundValueFactor;
        public Dictionary<string, float> surfaceDistrubtion =
            new Dictionary<string, float>(); //commonality of each surface ore in each tile <defName,commonality>
        public Dictionary<string, float> undergroundDistrubtion =
            new Dictionary<string, float>(); //commonality of each underground ore in each tile <defName,commonality>
        public float SurfaceAbundance =>
            berlinFactor * surfaceValueFactor * 1.5f; //decide whether the overall ore of the area are more or less on surface
        public float UndergroundAbundance =>
            berlinFactor * undergroundValueFactor * 1.5f; //decide whether the overall ore of the area are more or less in underground

        public TileOreData(float berlinFactor, float surfaceValueFactor, float undergroundValueFactor)
        {
            this.berlinFactor = berlinFactor;
            this.surfaceValueFactor = surfaceValueFactor;
            this.undergroundValueFactor = undergroundValueFactor;
        }

        public void DebugShowSurfaceDistrubtion()
        {
            foreach (var kvp in surfaceDistrubtion)
            {
                Log.Message($"surface ore: {kvp.Key}\ncommonality: {kvp.Value}");
            }
        }

        public void DebugShowUndergroundDistrubtion()
        {
            foreach (var kvp in undergroundDistrubtion)
            {
                Log.Message($"underground ore: {kvp.Key}\ncommonality: {kvp.Value}");
            }
        }

        public void DebugShowFactors()
        {
            Log.Message($"berlinFactor: {berlinFactor}");
            Log.Message($"surfaceValueFactor: {surfaceValueFactor}");
            Log.Message($"undergroundValueFactor: {undergroundValueFactor}");
        }
    }
}
