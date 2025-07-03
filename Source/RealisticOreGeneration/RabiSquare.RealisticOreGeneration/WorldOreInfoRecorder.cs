using System.Collections.Generic;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

public class WorldOreInfoRecorder : BaseSingleTon<WorldOreInfoRecorder>, IExposable
{
    private HashSet<int> _worldAbandonedTile = [];

    private HashSet<int> _worldSurfaceScannedTile = [];

    private Dictionary<int, int> _worldTileUndergroundOreMiningCount = new();

    private HashSet<int> _worldUndergroundScannedTile = [];

    public IEnumerable<int> WorldOreInfoTile
    {
        get
        {
            var hashSet = new HashSet<int>();
            hashSet.AddRange(_worldAbandonedTile);
            hashSet.AddRange(_worldSurfaceScannedTile);
            hashSet.AddRange(_worldUndergroundScannedTile);
            return hashSet;
        }
    }

    public void ExposeData()
    {
        Scribe_Collections.Look(ref _worldTileUndergroundOreMiningCount, "_worldTileUndergroundOreMiningCount",
            LookMode.Value, LookMode.Value);
        Scribe_Collections.Look(ref _worldSurfaceScannedTile, "_worldSurfaceScannedTile", LookMode.Value);
        Scribe_Collections.Look(ref _worldUndergroundScannedTile, "_worldUndergroundScannedTile", LookMode.Value);
        Scribe_Collections.Look(ref _worldAbandonedTile, "_worldAbandonedTile", LookMode.Value);
        _worldTileUndergroundOreMiningCount ??= new Dictionary<int, int>();

        _worldSurfaceScannedTile ??= [];

        _worldUndergroundScannedTile ??= [];

        _worldAbandonedTile ??= [];
    }

    public void UndergroundMiningCountIncrease(int tileId)
    {
        if (!_worldTileUndergroundOreMiningCount.TryAdd(tileId, 1))
        {
            _worldTileUndergroundOreMiningCount[tileId]++;
        }
    }

    public int GetUndergroundMiningCount(int tileId)
    {
        return _worldTileUndergroundOreMiningCount.GetValueOrDefault(tileId, 0);
    }

    public void RecordAbandonedTile(int tileId)
    {
        if (!_worldAbandonedTile.Add(tileId))
        {
            return;
        }

        WorldUtils.SetWorldLayerDirty();
    }

    public void RecordScannedTileSurface(int tileId)
    {
        if (!_worldSurfaceScannedTile.Add(tileId))
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]repeat scan");
            return;
        }

        WorldUtils.SetWorldLayerDirty();
    }

    public void RecordScannedTileUnderground(int tileId)
    {
        if (!_worldUndergroundScannedTile.Add(tileId))
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]repeat scan");
            return;
        }

        WorldUtils.SetWorldLayerDirty();
    }

    public bool IsTileAbandoned(int tileId)
    {
        return _worldAbandonedTile.Contains(tileId);
    }

    public bool IsTileScannedSurface(int tileId)
    {
        return _worldSurfaceScannedTile.Contains(tileId) || SettingWindow.Instance.settingModel.disableScanner;
    }

    public bool IsTileScannedUnderground(int tileId)
    {
        return _worldUndergroundScannedTile.Contains(tileId) || SettingWindow.Instance.settingModel.disableScanner;
    }

    public void Clear()
    {
        _worldAbandonedTile.Clear();
        _worldSurfaceScannedTile.Clear();
        _worldUndergroundScannedTile.Clear();
        _worldTileUndergroundOreMiningCount.Clear();
    }
}