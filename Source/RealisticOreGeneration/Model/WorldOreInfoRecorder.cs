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
        private Dictionary<int, TileOreData> worldTileOreDataHashmap = new Dictionary<int, TileOreData>(); //tileId,tileOreData
        private Dictionary<int, int> worldTileUndergroundOreMiningCount = new Dictionary<int, int>(); //tileId,mingingCount

        /// <summary>
        /// set surface ore data of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <param name="berlinFactor"></param>
        /// <param name="surfaceValueFactor"></param>
        /// <param name="undergroundValueFactor"></param>
        /// <param name="surfaceDistrubtion"></param>
        /// <param name="undergroundDistrubtion"></param>
        public void SetTileOreData(int tileId, float berlinFactor, float surfaceValueFactor, float undergroundValueFactor,
            Dictionary<string, float> surfaceDistrubtion, Dictionary<string, float> undergroundDistrubtion)
        {
            if (worldTileOreDataHashmap.ContainsKey(tileId))
            {
                Log.Warning($"{MsicDef.LogTag}ore data of tile: {tileId} has been calced");
                return;
            }

            var tileOreData = new TileOreData(berlinFactor, surfaceValueFactor, undergroundValueFactor)
            {
                surfaceDistrubtion = surfaceDistrubtion,
                undergroundDistrubtion = undergroundDistrubtion
            };

            worldTileOreDataHashmap.Add(tileId, tileOreData);
        }

        /// <summary>
        /// get ore data of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <returns></returns>
        public TileOreData GetTileOreData(int tileId)
        {
            if (worldTileOreDataHashmap.ContainsKey(tileId))
            {
                return worldTileOreDataHashmap[tileId];
            }

            Log.Error($"{MsicDef.LogTag}can't find ore data of tile: {tileId}");
            return null;
        }

        public void UndergroundMiningCountIncrease(int tileId)
        {
            // no info
            if (!worldTileUndergroundOreMiningCount.ContainsKey(tileId))
            {
                worldTileUndergroundOreMiningCount.Add(tileId, 1);
                return;
            }

            worldTileUndergroundOreMiningCount[tileId]++;
        }

        public int GetUndergroundMiningCount(int tileId)
        {
            return !worldTileUndergroundOreMiningCount.ContainsKey(tileId) ? 0 : worldTileUndergroundOreMiningCount[tileId];
        }

        public override string ToString()
        {
            return $"tile count: {worldTileOreDataHashmap.Count}";
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref worldTileOreDataHashmap, "worldTileOreDataHashmap", LookMode.Value, LookMode.Reference);
            Scribe_Collections.Look(ref worldTileUndergroundOreMiningCount, "worldTileUndergroundOreMiningCount", LookMode.Value,
                LookMode.Value);
        }
    }
}
