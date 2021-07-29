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
    public class TileOreData : IExposable
    {
        private float berlinFactor;
        private float surfaceValueFactor;
        private float undergroundValueFactor;

        //commonality of each surface ore in each tile <defName,commonality>
        public Dictionary<string, float> surfaceDistrubtion = new Dictionary<string, float>();
        //commonality of each underground ore in each tile <defName,commonality>
        public Dictionary<string, float> undergroundDistrubtion = new Dictionary<string, float>();
        //decide whether the overall ore of the area are more or less on surface
        public float SurfaceAbundance => berlinFactor * surfaceValueFactor * SettingWindow.Instance.settingModel.surfaceMutilpier;
        //decide whether the overall ore of the area are more or less in underground
        public float UndergroundAbundance =>
            berlinFactor * undergroundValueFactor * SettingWindow.Instance.settingModel.undergroundMutilpier;

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

        public void ExposeData()
        {
            Scribe_Values.Look(ref berlinFactor, "berlinFactor");
            Scribe_Values.Look(ref surfaceValueFactor, "surfaceValueFactor");
            Scribe_Values.Look(ref undergroundValueFactor, "undergroundValueFactor");
            Scribe_Collections.Look(ref surfaceDistrubtion, "surfaceDistrubtion", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref undergroundDistrubtion, "undergroundDistrubtion", LookMode.Value, LookMode.Value);
        }
    }
}
