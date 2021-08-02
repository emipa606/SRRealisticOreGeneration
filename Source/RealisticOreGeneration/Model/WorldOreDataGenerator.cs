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
    public static class WorldOreDataGenerator
    {
        private const float Relief = 15;

        /// <summary>
        /// get ore data of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <returns></returns>
        [NotNull]
        public static TileOreData GetTileOreData(int tileId)
        {
            var worldGrid = Find.WorldGrid;
            if (worldGrid == null)
            {
                throw new Exception("can't find world grid");
            }

            var surfaceDistrubtion = GenerateSurfaceDistrubtion(tileId);
            var undergroundDistrubtion = GenerateUndergroundDistrubtion(tileId);
            var surfaceValueFactor = CalcSurfaceValueFactor(surfaceDistrubtion);
            var surfaceBerlinFactor = CalcBerlinFactor(tileId, worldGrid, true);
            var undergroundBerlinFactor = CalcBerlinFactor(tileId, worldGrid, false);
            var tileOreData = new TileOreData(tileId, surfaceBerlinFactor, undergroundBerlinFactor, surfaceValueFactor)
            {
                surfaceDistrubtion = surfaceDistrubtion,
                undergroundDistrubtion = undergroundDistrubtion
            };

            return tileOreData;
        }

        private static Dictionary<string, float> GenerateSurfaceDistrubtion(int tileId)
        {
            var mapCommonality = new Dictionary<string, float>();
            //generate random ore distrubtion
            const float qMin = 1f;
            var n = VanillaOreInfoRecoder.Instance.GetSurfaceOreDataListCount();
            var q = qMin + Rand.ValueSeeded(tileId) * ((float)n / 2 - qMin);

            var arrayNewCommonality = new float[n];
            for (var i = 0; i < n; i++)
            {
                arrayNewCommonality[i] = 1 / (q * Mathf.Sqrt(2 * 3.14f)) *
                                         Mathf.Exp(-Mathf.Pow((i - n / 2), 2) / (2 * Mathf.Pow(q, 2)));
            }

            //shuffle
            arrayNewCommonality.Shuffle();
            //normalized
            arrayNewCommonality.Normalized();
            //record ore distrubtion
            for (var i = 0; i < n; i++)
            {
                var oreData = VanillaOreInfoRecoder.Instance.GetSurfaceOreDataByIndex(i);
                mapCommonality.Add(oreData.defName, arrayNewCommonality[i]);
            }

            return mapCommonality;
        }

        private static Dictionary<string, float> GenerateUndergroundDistrubtion(int tileId)
        {
            var mapCommonality = new Dictionary<string, float>();
            //generate random ore distrubtion
            const float qMin = 1f;
            var n = VanillaOreInfoRecoder.Instance.GetUndergroundOreDataListCount();
            var q = qMin + Rand.ValueSeeded(tileId) * ((float)n / 2 - qMin);

            var arrayNewCommonality = new float[n];
            for (var i = 0; i < n; i++)
            {
                arrayNewCommonality[i] = 1 / (q * Mathf.Sqrt(2 * 3.14f)) *
                                         Mathf.Exp(-Mathf.Pow((i - n / 2), 2) / (2 * Mathf.Pow(q, 2)));
            }

            //shuffle
            arrayNewCommonality.Shuffle();
            //normalized
            arrayNewCommonality.Normalized();
            //record ore distrubtion
            for (var i = 0; i < n; i++)
            {
                var oreData = VanillaOreInfoRecoder.Instance.GetUndergroundOreDataByIndex(i);
                mapCommonality.Add(oreData.defName, arrayNewCommonality[i]);
            }

            return mapCommonality;
        }

        /// <summary>
        /// ramdom by tile
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
        /// how many times the total value of vanilla is the current total value in the entire map
        /// </summary>
        /// <param name="oreDistrubtion">commonality of each ore. key:defName,value:commonality</param>
        /// <returns></returns>
        private static float CalcSurfaceValueFactor(IReadOnlyDictionary<string, float> oreDistrubtion)
        {
            if (oreDistrubtion.Count == 0)
            {
                Log.Error($"{MsicDef.LogTag}there is no ore in defs");
                return 1f;
            }

            var vanillaTotalValue = 0f;
            var currentTotalValue = 0f;
            //notmalisation by total resource value
            for (var i = 0; i < oreDistrubtion.Count; i++)
            {
                var oreData = VanillaOreInfoRecoder.Instance.GetSurfaceOreDataByIndex(i);
                if (oreData == null)
                {
                    Log.Error($"{MsicDef.LogTag}cant't find ore data by index: {i}");
                    return 1f;
                }

                vanillaTotalValue += VanillaOreInfoRecoder.Instance.GetNormalizedSurfaceCommonality(i) * oreData.yield *
                                     oreData.marketValue * oreData.lumpSize.Average;
                if (!oreDistrubtion.ContainsKey(oreData.defName))
                {
                    Log.Error($"{MsicDef.LogTag}cant't find ore data by defName: {oreData.defName}");
                    return 1f;
                }

                var currentCommonality = oreDistrubtion[oreData.defName];
                currentTotalValue += currentCommonality * oreData.yield *
                                     oreData.marketValue * oreData.lumpSize.Average;
            }

            //scale total value to vanilla
            var valueFactor = vanillaTotalValue / currentTotalValue;
            return valueFactor;
        }
    }
}
