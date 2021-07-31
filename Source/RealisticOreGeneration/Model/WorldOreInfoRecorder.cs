// ******************************************************************
//       /\ /|       @file       WorldOreInfoRecorder.cs
//       \ V/        @brief      record the current ore data of all tiles
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-30 11:55:30
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    public class WorldOreInfoRecorder : BaseSingleTon<WorldOreInfoRecorder>, IExposable
    {
        private Dictionary<int, TileOreData>
            _worldTileOreDataHashmap = new Dictionary<int, TileOreData>(); //tileId,tileOreData

        private Dictionary<int, int>
            _worldTileUndergroundOreMiningCount = new Dictionary<int, int>(); //tileId,mingingCount

        /// <summary>
        /// set surface ore data of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <param name="surfaceBerlinFactor"></param>
        /// <param name="undergroundBerlinFactor"></param>
        /// <param name="surfaceValueFactor"></param>
        /// <param name="surfaceDistrubtion"></param>
        /// <param name="undergroundDistrubtion"></param>
        public void SetTileOreData(int tileId, float surfaceBerlinFactor, float undergroundBerlinFactor,
            float surfaceValueFactor, Dictionary<string, float> surfaceDistrubtion,
            Dictionary<string, float> undergroundDistrubtion)
        {
            if (_worldTileOreDataHashmap.ContainsKey(tileId))
            {
                Log.Warning($"{MsicDef.LogTag}ore data of tile: {tileId} has been calced");
                return;
            }

            var tileOreData =
                new TileOreData(tileId, surfaceBerlinFactor, undergroundBerlinFactor, surfaceValueFactor)
                {
                    surfaceDistrubtion = surfaceDistrubtion,
                    undergroundDistrubtion = undergroundDistrubtion
                };

            _worldTileOreDataHashmap.Add(tileId, tileOreData);
        }

        /// <summary>
        /// get ore data of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <returns></returns>
        public TileOreData GetTileOreData(int tileId)
        {
            if (_worldTileOreDataHashmap.ContainsKey(tileId))
            {
                return _worldTileOreDataHashmap[tileId];
            }

            Log.Error($"{MsicDef.LogTag}can't find ore data of tile: {tileId}");
            return null;
        }

        public void UndergroundMiningCountIncrease(int tileId)
        {
            // no info
            if (!_worldTileUndergroundOreMiningCount.ContainsKey(tileId))
            {
                _worldTileUndergroundOreMiningCount.Add(tileId, 1);
                return;
            }

            _worldTileUndergroundOreMiningCount[tileId]++;
        }

        public int GetUndergroundMiningCount(int tileId)
        {
            return !_worldTileUndergroundOreMiningCount.ContainsKey(tileId)
                ? 0
                : _worldTileUndergroundOreMiningCount[tileId];
        }

        public override string ToString()
        {
            return $"tile count: {_worldTileOreDataHashmap.Count}";
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref _worldTileOreDataHashmap, "_worldTileOreDataHashmap", LookMode.Value,
                LookMode.Reference);
            Scribe_Collections.Look(ref _worldTileUndergroundOreMiningCount, "_worldTileUndergroundOreMiningCount",
                LookMode.Value,
                LookMode.Value);
        }
    }
}