// ******************************************************************
//       /\ /|       @file       TileOreData.cs
//       \ V/        @brief      ore info in tile
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-29 11:33:38
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    public class TileOreData : IExposable
    {
        private int _tileId;
        private float _surfaceBerlinFactor;
        private float _surfaceValueFactor;
        private float _undergroundBerlinFactor;

        //commonality of each surface ore in each tile <defName,commonality>
        public Dictionary<string, float> surfaceDistrubtion = new Dictionary<string, float>();

        //commonality of each underground ore in each tile <defName,commonality>
        public Dictionary<string, float> undergroundDistrubtion = new Dictionary<string, float>();

        public TileOreData()
        {
        }

        public TileOreData(int tileId, float surfaceBerlinFactor, float undergroundBerlinFactor,
            float surfaceValueFactor)
        {
            _tileId = tileId;
            _surfaceBerlinFactor = surfaceBerlinFactor;
            _undergroundBerlinFactor = undergroundBerlinFactor;
            _surfaceValueFactor = surfaceValueFactor;
        }

        public float UndergroundBerlinFactor => _undergroundBerlinFactor;

        //decide whether the overall ore of the area are more or less on surface
        public float OreGenerationFactor =>
            _surfaceBerlinFactor * _surfaceValueFactor * SettingWindow.Instance.settingModel.surfaceMutilpier;

        //beyond this amount, the cost of finding underground ore will increase
        public float FreeUndergroundCycleCount =>
            _undergroundBerlinFactor * SettingWindow.Instance.settingModel.undergroundMutilpier * 20;

        public void ExposeData()
        {
            Scribe_Values.Look(ref _tileId, "_tileId");
            Scribe_Values.Look(ref _surfaceBerlinFactor, "_surfaceBerlinFactor");
            Scribe_Values.Look(ref _undergroundBerlinFactor, "_undergroundBerlinFactor");
            Scribe_Values.Look(ref _surfaceValueFactor, "_surfaceValueFactor");
            Scribe_Collections.Look(ref surfaceDistrubtion, "surfaceDistrubtion", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref undergroundDistrubtion, "undergroundDistrubtion", LookMode.Value,
                LookMode.Value);
        }

        public float GetSurfaceAbondance()
        {
            var worldGrid = Find.WorldGrid;
            if (worldGrid == null)
            {
                Log.Error("can't find world grid");
                return 0f;
            }

            var tile = worldGrid[_tileId];
            if (tile == null)
            {
                Log.Error($"can't find tile :{_tileId}");
                return 0f;
            }

            var terrainFactor = 0f;
            switch (tile.hilliness)
            {
                case Hilliness.Flat:
                    terrainFactor = 0.27f;
                    break;
                case Hilliness.SmallHills:
                    terrainFactor = 0.53f;
                    break;
                case Hilliness.LargeHills:
                    terrainFactor = 0.73f;
                    break;
                case Hilliness.Mountainous:
                case Hilliness.Impassable:
                    terrainFactor = 1f;
                    break;
            }

            return terrainFactor * _surfaceBerlinFactor;
        }

        public void DebugShowSurfaceDistrubtion()
        {
            foreach (var kvp in surfaceDistrubtion) Log.Message($"surface ore: {kvp.Key}\ncommonality: {kvp.Value}");
        }

        public void DebugShowUndergroundDistrubtion()
        {
            foreach (var kvp in undergroundDistrubtion)
                Log.Message($"underground ore: {kvp.Key}\ncommonality: {kvp.Value}");
        }

        public void DebugShowSurfaceFactors()
        {
            Log.Message($"surfaceBerlinFactor: {_surfaceBerlinFactor}");
            Log.Message($"surfaceValueFactor: {_surfaceValueFactor}");
        }
    }
}