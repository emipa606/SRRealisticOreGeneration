// ******************************************************************
//       /\ /|       @file       WorldOreDataGenerator.cs
//       \ V/        @brief      to calc info of ore in some tile
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-26 21:10:22
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    public class WorldOreDataGenerator : BaseSingleTon<WorldOreDataGenerator>
    {
        private const float Relief = 15;
        private readonly Dictionary<int, TileOreData> _cacheTileOreDataHashmap = new Dictionary<int, TileOreData>();

        /// <summary>
        ///     get ore data of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <returns></returns>
        [NotNull]
        public TileOreData GetTileOreData(int tileId)
        {
            if (_cacheTileOreDataHashmap.ContainsKey(tileId))
            {
                return _cacheTileOreDataHashmap[tileId];
            }

            var worldGrid = Find.WorldGrid;
            if (worldGrid == null) throw new Exception("can't find world grid");
            var surfaceDistribution = GenerateSurfaceDistribution(tileId);
            var undergroundDistribution = GenerateUndergroundDistribution(tileId);
            var surfaceValueFactor = CalcSurfaceValueFactor(surfaceDistribution);
            var surfaceBerlinFactor = CalcBerlinFactor(tileId, worldGrid, true);
            var undergroundBerlinFactor = CalcBerlinFactor(tileId, worldGrid, false);
            var tileOreData = new TileOreData(tileId, surfaceBerlinFactor, undergroundBerlinFactor, surfaceValueFactor)
            {
                surfaceDistribution = surfaceDistribution,
                undergroundDistribution = undergroundDistribution
            };
            _cacheTileOreDataHashmap.Add(tileId, tileOreData);
            return tileOreData;
        }

        private static Dictionary<string, float> GenerateSurfaceDistribution(int tileId)
        {
            var mapCommonality = new Dictionary<string, float>();
            //generate random ore distribution
            const float qMin = 1f;
            var n = VanillaOreInfoRecorder.Instance.GetSurfaceOreDataListCount();
            var q = qMin + Rand.ValueSeeded(tileId) * ((float) n / 2 - qMin);

            var arrayNewCommonality = new float[n];
            for (var i = 0; i < n; i++)
                arrayNewCommonality[i] = 1 / (q * Mathf.Sqrt(2 * 3.14f)) *
                                         Mathf.Exp(-Mathf.Pow(i - n / 2, 2) / (2 * Mathf.Pow(q, 2)));

            //shuffle
            arrayNewCommonality.Shuffle(tileId);
            //normalized
            arrayNewCommonality.Normalized();
            //record ore distribution
            for (var i = 0; i < n; i++)
            {
                var oreData = VanillaOreInfoRecorder.Instance.GetSurfaceOreDataByIndex(i);
                mapCommonality.Add(oreData.defName, arrayNewCommonality[i]);
            }

            return mapCommonality;
        }

        private static Dictionary<string, float> GenerateUndergroundDistribution(int tileId)
        {
            var mapCommonality = new Dictionary<string, float>();
            //generate random ore distribution
            const float qMin = 1f;
            var n = VanillaOreInfoRecorder.Instance.GetUndergroundOreDataListCount();
            var q = qMin + Rand.ValueSeeded(tileId) * ((float) n / 2 - qMin);

            var arrayNewCommonality = new float[n];
            for (var i = 0; i < n; i++)
                arrayNewCommonality[i] = 1 / (q * Mathf.Sqrt(2 * 3.14f)) *
                                         Mathf.Exp(-Mathf.Pow(i - n / 2, 2) / (2 * Mathf.Pow(q, 2)));
            //shuffle
            arrayNewCommonality.Shuffle(tileId);
            //normalized
            arrayNewCommonality.Normalized();
            //record ore distribution
            for (var i = 0; i < n; i++)
            {
                var oreData = VanillaOreInfoRecorder.Instance.GetUndergroundOreDataByIndex(i);
                mapCommonality.Add(oreData.defName, arrayNewCommonality[i]);
            }

            return mapCommonality;
        }

        /// <summary>
        /// random by tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <param name="worldGrid"></param>
        /// <param name="isSurface"></param>
        /// <returns></returns>
        private static float CalcBerlinFactor(int tileId, WorldGrid worldGrid, bool isSurface)
        {
            //get grid to find tiles
            var tile = worldGrid[tileId];
            if (tile == null)
            {
                Log.Error($"{MsicDef.LogTag}can't find tile: {tileId}");
                return 1f;
            }

            var pos = worldGrid.GetTileCenter(tileId);
            var seed = isSurface ? tile.GetHashCode() : tile.GetHashCode() / 2;
            var berlinFactor =
                Mathf.PerlinNoise((pos.x + seed % 100) / Relief, (pos.z + seed % 100) / Relief);
            return berlinFactor;
        }

        /// <summary>
        ///     how many times the total value of vanilla is the current total value in the entire map
        /// </summary>
        /// <param name="oreDistribution">commonality of each ore. key:defName,value:commonality</param>
        /// <returns></returns>
        private static float CalcSurfaceValueFactor(IReadOnlyDictionary<string, float> oreDistribution)
        {
            if (oreDistribution.Count == 0)
            {
                Log.Error($"{MsicDef.LogTag}there is no ore in defList");
                return 1f;
            }

            var vanillaTotalValue = 0f;
            var currentTotalValue = 0f;
            //normalization by total resource value
            for (var i = 0; i < oreDistribution.Count; i++)
            {
                var oreData = VanillaOreInfoRecorder.Instance.GetSurfaceOreDataByIndex(i);
                if (oreData == null)
                {
                    Log.Error($"{MsicDef.LogTag}can't find ore data by index: {i}");
                    return 1f;
                }

                vanillaTotalValue += VanillaOreInfoRecorder.Instance.GetNormalizedSurfaceCommonality(i) *
                                     oreData.yield *
                                     oreData.marketValue * oreData.lumpSize.Average;
                if (!oreDistribution.ContainsKey(oreData.defName))
                {
                    Log.Error($"{MsicDef.LogTag}can't find ore data by defName: {oreData.defName}");
                    return 1f;
                }

                var currentCommonality = oreDistribution[oreData.defName];
                currentTotalValue += currentCommonality * oreData.yield *
                                     oreData.marketValue * oreData.lumpSize.Average;
            }

            //scale total value to vanilla
            var valueFactor = vanillaTotalValue / currentTotalValue;
            return valueFactor;
        }
    }
}