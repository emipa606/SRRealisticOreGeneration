using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

public class WorldOreInfoRecorder : BaseSingleTon<WorldOreInfoRecorder>, IExposable
{
    private HashSet<PlanetTile> _worldAbandonedTile = [];

    private HashSet<PlanetTile> _worldSurfaceScannedTile = [];

    private Dictionary<PlanetTile, int> _worldTileUndergroundOreMiningCount = new();

    private HashSet<PlanetTile> _worldUndergroundScannedTile = [];

    public IEnumerable<PlanetTile> WorldOreInfoTile
    {
        get
        {
            var hashSet = new HashSet<PlanetTile>();
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
        _worldTileUndergroundOreMiningCount ??= new Dictionary<PlanetTile, int>();

        _worldSurfaceScannedTile ??= [];

        _worldUndergroundScannedTile ??= [];

        _worldAbandonedTile ??= [];
    }

    public void UndergroundMiningCountIncrease(PlanetTile tileId)
    {
        if (!_worldTileUndergroundOreMiningCount.TryAdd(tileId, 1))
        {
            _worldTileUndergroundOreMiningCount[tileId]++;
        }
    }

    public int GetUndergroundMiningCount(PlanetTile tileId)
    {
        return _worldTileUndergroundOreMiningCount.GetValueOrDefault(tileId, 0);
    }

    public void RecordAbandonedTile(PlanetTile tileId)
    {
        if (!_worldAbandonedTile.Add(tileId))
        {
            return;
        }

        WorldUtils.SetWorldLayerDirty();
    }

    public void RecordScannedTileSurface(PlanetTile tileId)
    {
        if (!_worldSurfaceScannedTile.Add(tileId))
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]repeat scan");
            return;
        }

        WorldUtils.SetWorldLayerDirty();
    }

    public void RecordScannedTileUnderground(PlanetTile tileId)
    {
        if (!_worldUndergroundScannedTile.Add(tileId))
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]repeat scan");
            return;
        }

        WorldUtils.SetWorldLayerDirty();
    }

    public bool IsTileAbandoned(PlanetTile tileId)
    {
        return _worldAbandonedTile.Contains(tileId);
    }

    public bool IsTileScannedSurface(PlanetTile tileId)
    {
        return _worldSurfaceScannedTile.Contains(tileId) || SettingWindow.Instance.settingModel.disableScanner;
    }

    public bool IsTileScannedUnderground(PlanetTile tileId)
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