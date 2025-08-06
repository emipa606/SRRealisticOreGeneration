using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

public class TileOreData
{
    private readonly float _surfaceBerlinFactor;

    private readonly float _surfaceValueFactor;
    private readonly PlanetTile _tileId;

    public Dictionary<string, float> surfaceDistribution = new();

    public Dictionary<string, float> undergroundDistribution = new();

    public TileOreData()
    {
    }

    public TileOreData(PlanetTile tileId, float surfaceBerlinFactor, float undergroundBerlinFactor,
        float surfaceValueFactor)
    {
        _tileId = tileId;
        _surfaceBerlinFactor = surfaceBerlinFactor;
        UndergroundBerlinFactor = undergroundBerlinFactor;
        _surfaceValueFactor = surfaceValueFactor;
    }

    public float UndergroundBerlinFactor { get; }

    public float OreGenerationFactor => _surfaceBerlinFactor * _surfaceValueFactor *
                                        SettingWindow.Instance.settingModel.surfaceMultiplier;

    public float FreeUndergroundCycleCount =>
        UndergroundBerlinFactor * SettingWindow.Instance.settingModel.undergroundMultiplier * 20f;

    public float GetSurfaceAbundance()
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

        var num = 0f;
        switch (tile.hilliness)
        {
            case Hilliness.Flat:
                num = 0.27f;
                break;
            case Hilliness.SmallHills:
                num = 0.53f;
                break;
            case Hilliness.LargeHills:
                num = 0.73f;
                break;
            case Hilliness.Mountainous:
            case Hilliness.Impassable:
                num = 1f;
                break;
        }

        return Mathf.Clamp(num * _surfaceBerlinFactor * _surfaceValueFactor, 0f, 1f);
    }
}