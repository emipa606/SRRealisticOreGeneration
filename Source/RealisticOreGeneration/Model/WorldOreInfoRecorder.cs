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
        private Dictionary<int, int> _worldTileUndergroundOreMiningCount = new Dictionary<int, int>(); //tileId,mingingCount
        
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

        public void ExposeData()
        {
            Scribe_Collections.Look(ref _worldTileUndergroundOreMiningCount, "_worldTileUndergroundOreMiningCount",
                LookMode.Value,
                LookMode.Value);
            //if the mod is added halfway, the reverse sequence will cause the parameter to become null
            if (_worldTileUndergroundOreMiningCount == null)
            {
                _worldTileUndergroundOreMiningCount = new Dictionary<int, int>();
            }
        }
    }
}
