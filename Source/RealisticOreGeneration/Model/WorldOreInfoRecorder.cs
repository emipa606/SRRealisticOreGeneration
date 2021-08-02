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
        private HashSet<int> _worldAbandonedTile = new HashSet<int>(); //which tile has been abandoned
        private HashSet<int> _worldSurfaceScannedTile = new HashSet<int>(); //which tile has been scanned

        private Dictionary<int, int>
            _worldTileUndergroundOreMiningCount = new Dictionary<int, int>(); //tileId,mingingCount

        private HashSet<int> _worldUndergroundScannedTile = new HashSet<int>(); //which tile has been scanned

        public void ExposeData()
        {
            Scribe_Collections.Look(ref _worldTileUndergroundOreMiningCount, "_worldTileUndergroundOreMiningCount",
                LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref _worldSurfaceScannedTile, "_worldSurfaceScannedTile", LookMode.Value);
            Scribe_Collections.Look(ref _worldUndergroundScannedTile, "_worldUndergroundScannedTile", LookMode.Value);
            Scribe_Collections.Look(ref _worldAbandonedTile, "_worldAbandonedTile", LookMode.Value);
            //if the mod is added halfway, the reverse sequence will cause the parameter to become null
            if (_worldTileUndergroundOreMiningCount == null)
                _worldTileUndergroundOreMiningCount = new Dictionary<int, int>();

            if (_worldSurfaceScannedTile == null) _worldSurfaceScannedTile = new HashSet<int>();

            if (_worldUndergroundScannedTile == null) _worldUndergroundScannedTile = new HashSet<int>();

            if (_worldAbandonedTile == null) _worldAbandonedTile = new HashSet<int>();
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

        public void RecordAbandonedTile(int tileId)
        {
            if (_worldAbandonedTile.Contains(tileId))
            {
                Log.Warning($"{MsicDef.LogTag}repeat abandon");
                return;
            }

            _worldAbandonedTile.Add(tileId);
        }

        public void RecordScannedTileSurface(int tileId)
        {
            if (_worldSurfaceScannedTile.Contains(tileId))
            {
                Log.Warning($"{MsicDef.LogTag}repeat scan");
                return;
            }

            _worldSurfaceScannedTile.Add(tileId);
        }

        public void RecordScannedTileUnderground(int tileId)
        {
            if (_worldUndergroundScannedTile.Contains(tileId))
            {
                Log.Warning($"{MsicDef.LogTag}repeat scan");
                return;
            }

            _worldUndergroundScannedTile.Add(tileId);
        }


        public bool IsTileAbandoned(int tileId)
        {
            return _worldAbandonedTile.Contains(tileId);
        }

        public bool IsTileScannedSurface(int tileId)
        {
            return _worldSurfaceScannedTile.Contains(tileId);
        }

        public bool IsTileScannedUnderground(int tileId)
        {
            return _worldUndergroundScannedTile.Contains(tileId);
        }
    }
}