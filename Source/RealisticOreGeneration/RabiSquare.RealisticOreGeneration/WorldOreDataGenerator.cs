using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

public class WorldOreDataGenerator : BaseSingleTon<WorldOreDataGenerator>
{
    private const float Relief = 5f;

    private readonly Dictionary<int, TileOreData> _cacheTileOreDataHashmap = new();

    [NotNull]
    public TileOreData GetTileOreData(int tileId)
    {
        if (_cacheTileOreDataHashmap.TryGetValue(tileId, out var data))
        {
            return data;
        }

        var worldGrid = Find.WorldGrid;
        if (worldGrid == null)
        {
            throw new Exception("can't find world grid");
        }

        var dictionary = GenerateFixDistribution(tileId, true);
        var undergroundDistribution = GenerateFixDistribution(tileId, false);
        var surfaceValueFactor = CalcSurfaceValueFactor(dictionary);
        var surfaceBerlinFactor = CalcBerlinFactor(tileId, worldGrid, true);
        var undergroundBerlinFactor = CalcBerlinFactor(tileId, worldGrid, false);
        var tileOreData = new TileOreData(tileId, surfaceBerlinFactor, undergroundBerlinFactor, surfaceValueFactor)
        {
            surfaceDistribution = dictionary,
            undergroundDistribution = undergroundDistribution
        };
        _cacheTileOreDataHashmap.Add(tileId, tileOreData);
        return tileOreData;
    }

    private static float[] GenerateNormalizedRandomDistribution(int seed, int count)
    {
        var qMin = SettingWindow.Instance.settingModel.qMin;
        var qMax = SettingWindow.Instance.settingModel.qMax;
        var num = qMin + (Rand.ValueSeeded(seed) * ((count / qMax) - qMin));
        var num2 = qMin + (Rand.ValueSeeded(seed / 2) * ((count / qMax) - qMin));
        var array = new float[count];
        for (var i = 0; i < count; i++)
        {
            array[i] = 1f / (num * Mathf.Sqrt(6.28f)) *
                       Mathf.Exp((0f - Mathf.Pow(i - (count / 2f), 2f)) / (2f * Mathf.Pow(num, 2f)));
            array[i] *= 1f / (num2 * Mathf.Sqrt(6.28f)) *
                        Mathf.Exp((0f - Mathf.Pow(i - (Rand.ValueSeeded(seed) * (count / 2f)), 2f)) /
                                  (2f * Mathf.Pow(num2, 2f)));
        }

        array.Shuffle(seed);
        array.Normalized();
        return array;
    }

    private static void VanillaFix(ref float[] arrayCommonality)
    {
        for (var i = 0; i < arrayCommonality.Length; i++)
        {
            var normalizedSurfaceCommonality =
                BaseSingleTon<VanillaOreInfoRecorder>.Instance.GetNormalizedSurfaceCommonality(i);
            arrayCommonality[i] = (normalizedSurfaceCommonality * SettingWindow.Instance.settingModel.vanillaPercent) +
                                  (arrayCommonality[i] * (1f - SettingWindow.Instance.settingModel.vanillaPercent));
        }

        arrayCommonality.Normalized();
    }

    private static Dictionary<string, float> GenerateFixDistribution(int seed, bool isSurface)
    {
        var num = isSurface
            ? BaseSingleTon<VanillaOreInfoRecorder>.Instance.GetSurfaceOreDataListCount()
            : BaseSingleTon<VanillaOreInfoRecorder>.Instance.GetUndergroundOreDataListCount();
        var arrayCommonality = GenerateNormalizedRandomDistribution(isSurface ? seed : seed / 2, num);
        if (isSurface)
        {
            VanillaFix(ref arrayCommonality);
        }

        var dictionary = new Dictionary<string, float>();
        for (var i = 0; i < num; i++)
        {
            var oreData = isSurface
                ? BaseSingleTon<VanillaOreInfoRecorder>.Instance.GetSurfaceOreDataByIndex(i)
                : BaseSingleTon<VanillaOreInfoRecorder>.Instance.GetUndergroundOreDataByIndex(i);
            dictionary.Add(oreData.defName, arrayCommonality[i]);
        }

        return dictionary;
    }

    private static float CalcBerlinFactor(int tileId, WorldGrid worldGrid, bool isSurface)
    {
        var tile = worldGrid[tileId];
        if (tile == null)
        {
            Log.Error($"[RabiSquare.RealisticOreGeneration]can't find tile: {tileId}");
            return 1f;
        }

        var tileCenter = worldGrid.GetTileCenter(tileId);
        var num = isSurface ? tile.GetHashCode() : tile.GetHashCode() / 2;
        var num2 = Mathf.PerlinNoise((tileCenter.x + (num % 100)) / Relief, (tileCenter.z + (num % 100)) / Relief);
        num2 = Mathf.Clamp(num2, 0f, 1f);
        return num2;
    }

    private static float CalcSurfaceValueFactor(IReadOnlyDictionary<string, float> oreDistribution)
    {
        if (oreDistribution.Count == 0)
        {
            Log.Error("[RabiSquare.RealisticOreGeneration]there is no ore in defList");
            return 1f;
        }

        var num = 0f;
        var num2 = 0f;
        for (var i = 0; i < oreDistribution.Count; i++)
        {
            var surfaceOreDataByIndex = BaseSingleTon<VanillaOreInfoRecorder>.Instance.GetSurfaceOreDataByIndex(i);
            if (surfaceOreDataByIndex == null)
            {
                Log.Error($"[RabiSquare.RealisticOreGeneration]can't find ore data by index: {i}");
                return 1f;
            }

            num += BaseSingleTon<VanillaOreInfoRecorder>.Instance.GetNormalizedSurfaceCommonality(i) *
                   surfaceOreDataByIndex.yield * surfaceOreDataByIndex.marketValue *
                   surfaceOreDataByIndex.lumpSize.Average;
            if (!oreDistribution.TryGetValue(surfaceOreDataByIndex.defName, out var num3))
            {
                Log.Error(
                    $"[RabiSquare.RealisticOreGeneration]can't find ore data by defName: {surfaceOreDataByIndex.defName}");
                return 1f;
            }

            num2 += num3 * surfaceOreDataByIndex.yield * surfaceOreDataByIndex.marketValue *
                    surfaceOreDataByIndex.lumpSize.Average;
        }

        return num / num2;
    }
}