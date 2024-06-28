using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

public class VanillaOreInfoRecorder : BaseSingleTon<VanillaOreInfoRecorder>
{
    private readonly List<OreData> _vanillaSurfaceOreDataList = [];

    private readonly List<OreData> _vanillaUndergroundOreDataList = [];
    private float _vanillaTotalSurfaceCommonality;

    public void SetSurfaceOreDataList(IEnumerable<ThingDef> thingDefList)
    {
        _vanillaTotalSurfaceCommonality = 0f;
        foreach (var thingDef in thingDefList)
        {
            var building = thingDef.building;
            if (building == null)
            {
                Log.Warning($"[RabiSquare.RealisticOreGeneration]Unexpected buildingProperties: {thingDef.defName}");
                continue;
            }

            var mineableThing = building.mineableThing;
            if (mineableThing == null)
            {
                Log.Warning($"[RabiSquare.RealisticOreGeneration]Unexpected mineableThing: {thingDef.defName}");
                continue;
            }

            var oreData = new OreData(thingDef.defName, building.mineableScatterCommonality,
                building.mineableScatterLumpSizeRange, building.mineableYield, mineableThing.BaseMarketValue);
            _vanillaSurfaceOreDataList.Add(oreData);
            _vanillaTotalSurfaceCommonality += oreData.commonality;
        }
    }

    public OreData GetSurfaceOreDataByIndex(int index)
    {
        if (_vanillaSurfaceOreDataList != null && _vanillaSurfaceOreDataList.Count > index)
        {
            return _vanillaSurfaceOreDataList[index];
        }

        Log.Error($"[RabiSquare.RealisticOreGeneration]can't find surface oreData on index: {index}");
        return null;
    }

    public int GetSurfaceOreDataListCount()
    {
        return _vanillaSurfaceOreDataList.Count;
    }

    public IntRange GetRandomSurfaceLumpSize()
    {
        var randomInRange = new IntRange(0, _vanillaSurfaceOreDataList.Count - 1).RandomInRange;
        var surfaceOreDataByIndex = GetSurfaceOreDataByIndex(randomInRange);
        if (surfaceOreDataByIndex != null)
        {
            return surfaceOreDataByIndex.lumpSize;
        }

        Log.Error($"[RabiSquare.RealisticOreGeneration]cant find ore data by random index: {randomInRange}");
        return new IntRange(1, 20);
    }

    public float GetNormalizedSurfaceCommonality(int index)
    {
        if (_vanillaSurfaceOreDataList != null && _vanillaSurfaceOreDataList.Count > index)
        {
            return _vanillaSurfaceOreDataList[index].commonality / _vanillaTotalSurfaceCommonality;
        }

        Log.Error($"[RabiSquare.RealisticOreGeneration]can't find surface oreData on index: {index}");
        return 0f;
    }

    public void SetUndergroundOreDataList(IEnumerable<ThingDef> thingDefList)
    {
        foreach (var thingDef in thingDefList)
        {
            var item = new OreData(thingDef.defName, thingDef.deepCommonality, thingDef.deepLumpSizeRange,
                thingDef.deepCountPerPortion, thingDef.BaseMarketValue);
            _vanillaUndergroundOreDataList.Add(item);
        }
    }

    public OreData GetUndergroundOreDataByIndex(int index)
    {
        if (_vanillaUndergroundOreDataList != null && _vanillaUndergroundOreDataList.Count > index)
        {
            return _vanillaUndergroundOreDataList[index];
        }

        Log.Error($"[RabiSquare.RealisticOreGeneration]can't find underground oreData on index: {index}");
        return null;
    }

    public int GetUndergroundOreDataListCount()
    {
        return _vanillaUndergroundOreDataList.Count;
    }

    public IntRange GetRandomUndergroundLumpSize()
    {
        var randomInRange = new IntRange(0, _vanillaUndergroundOreDataList.Count - 1).RandomInRange;
        var undergroundOreDataByIndex = GetUndergroundOreDataByIndex(randomInRange);
        if (undergroundOreDataByIndex != null)
        {
            return undergroundOreDataByIndex.lumpSize;
        }

        Log.Error($"[RabiSquare.RealisticOreGeneration]cant find ore data by random index: {randomInRange}");
        return new IntRange(1, 20);
    }

    public OreData GetSurfaceOreDataByDefName(string defName)
    {
        using (var enumerator = _vanillaSurfaceOreDataList.Where(oreData => oreData.defName == defName).GetEnumerator())
        {
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
        }

        Log.Error($"[RabiSquare.RealisticOreGeneration]can't find surface oreData on defName: {defName}");
        return null;
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("surface vanilla data:");
        stringBuilder.Append("\n");
        foreach (var vanillaSurfaceOreData in _vanillaSurfaceOreDataList)
        {
            stringBuilder.Append(vanillaSurfaceOreData);
            stringBuilder.Append("\n");
        }

        stringBuilder.Append("\n");
        stringBuilder.Append("underground vanilla data:");
        stringBuilder.Append("\n");
        foreach (var vanillaUndergroundOreData in _vanillaUndergroundOreDataList)
        {
            stringBuilder.Append(vanillaUndergroundOreData);
            stringBuilder.Append("\n");
        }

        return stringBuilder.ToString();
    }
}