// ******************************************************************
//       /\ /|       @file       OreInfoRecoder.cs
//       \ V/        @brief      record the vanilla data of all ores
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-26 19:49:24
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using System.Text;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    public class OreInfoRecoder : BaseSingleTon<OreInfoRecoder>, IExposable
    {
        private readonly List<OreData>
            _vanillaSurfaceOreDataList = new List<OreData>(); //vanilla data of all surface ores

        private readonly List<OreData>
            _vanillaUndergroundOreDataList = new List<OreData>(); //vanilla data of all underground ores

        private Dictionary<int, Dictionary<string, float>> worldSurfaceOreAbundant =
            new Dictionary<int, Dictionary<string, float>>(); //commonality of each surface ore in each tile <tileId,<defName,commonality>>

        private Dictionary<int, Dictionary<string, float>> worldUndergroundOreAbundant =
            new Dictionary<int, Dictionary<string, float>>(); //commonality of each underground ore in each tile <tileId,<defName,commonality>>

        /// <summary>
        /// set vanilla data of each surface ore
        /// </summary>
        /// <param name="thingDefs"></param>
        public void SetSurfaceOreDataList(IEnumerable<ThingDef> thingDefs)
        {
            foreach (var thingdef in thingDefs)
            {
                var buildingProperties = thingdef.building;
                if (buildingProperties == null)
                {
                    Log.Warning(
                        $"[RabiSquare.RealisticOreGeneration]Unexpected buildingProperties: {thingdef.defName}");
                    continue;
                }

                var mineableThing = buildingProperties.mineableThing;
                if (mineableThing == null)
                {
                    Log.Warning($"[RabiSquare.RealisticOreGeneration]Unexpected mineableThing: {thingdef.defName}");
                    continue;
                }

                var oreData = new OreData(thingdef.defName, buildingProperties.mineableScatterCommonality,
                    buildingProperties.mineableScatterLumpSizeRange.Average, buildingProperties.mineableYield,
                    mineableThing.BaseMarketValue);
                _vanillaSurfaceOreDataList.Add(oreData);
            }
        }

        /// <summary>
        /// get vanilla data of surface ore by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public OreData GetSurfaceOreDataByIndex(int index)
        {
            if (_vanillaSurfaceOreDataList.Count > index)
            {
                return _vanillaSurfaceOreDataList[index];
            }

            Log.Error($"[RabiSquare.RealisticOreGeneration]can't find surface oreData on index: {index}");
            return null;
        }

        /// <summary>
        /// how many surface ores can be genetated
        /// </summary>
        /// <returns></returns>
        public int GetSurfaceOreDataListCount()
        {
            return _vanillaSurfaceOreDataList.Count;
        }

        /// <summary>
        /// set vanilla data of each underground ore 
        /// </summary>
        /// <param name="thingDefs"></param>
        public void SetUndergroundOreDataList(IEnumerable<ThingDef> thingDefs)
        {
            foreach (var thingdef in thingDefs)
            {
                var oreData = new OreData(thingdef.defName, thingdef.deepCommonality,
                    thingdef.deepLumpSizeRange.Average, thingdef.deepCountPerPortion,
                    thingdef.BaseMarketValue);
                _vanillaUndergroundOreDataList.Add(oreData);
            }
        }

        /// <summary>
        /// get vanilla data of underground ore by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public OreData GetUndergroundOreDataByIndex(int index)
        {
            if (_vanillaUndergroundOreDataList.Count > index)
            {
                return _vanillaUndergroundOreDataList[index];
            }

            Log.Error($"[RabiSquare.RealisticOreGeneration]can't find underground oreData on index: {index}");
            return null;
        }

        /// <summary>
        /// how many underground ores can be genetated
        /// </summary>
        /// <returns></returns>
        public int GetUndergroundOreDataListCount()
        {
            return _vanillaUndergroundOreDataList.Count;
        }

        /// <summary>
        /// set underground ore abundant of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <param name="oreAbundant"></param>
        public void SetSurfaceOreAbundant(int tileId, Dictionary<string, float> oreAbundant)
        {
            if (worldSurfaceOreAbundant.ContainsKey(tileId))
            {
                Log.Warning(
                    $"[RabiSquare.RealisticOreGeneration]surfaceOreAbundant of tile: {tileId} has been calced");
                worldSurfaceOreAbundant[tileId] = oreAbundant;
                return;
            }

            worldSurfaceOreAbundant.Add(tileId, oreAbundant);
        }

        /// <summary>
        /// get underground ore abundant of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <returns></returns>
        public Dictionary<string, float> GetSurfaceOreAbundant(int tileId)
        {
            if (worldSurfaceOreAbundant.ContainsKey(tileId))
            {
                return worldSurfaceOreAbundant[tileId];
            }

            Log.Error($"[RabiSquare.RealisticOreGeneration]can't find surfaceOreAbundant of tile: {tileId}");
            return null;
        }

        /// <summary>
        /// set underground ore abundant of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <param name="oreAbundant"></param>
        public void SetUndergroundOreAbundant(int tileId, Dictionary<string, float> oreAbundant)
        {
            if (worldUndergroundOreAbundant.ContainsKey(tileId))
            {
                Log.Warning(
                    $"[RabiSquare.RealisticOreGeneration]undergroundOreAbundant of tile: {tileId} has been calced");
                worldUndergroundOreAbundant[tileId] = oreAbundant;
                return;
            }

            worldUndergroundOreAbundant.Add(tileId, oreAbundant);
        }

        /// <summary>
        /// get underground ore abundant of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <returns></returns>
        public Dictionary<string, float> GetUndergroundOreAbundant(int tileId)
        {
            if (worldUndergroundOreAbundant.ContainsKey(tileId))
            {
                return worldUndergroundOreAbundant[tileId];
            }

            Log.Error($"[RabiSquare.RealisticOreGeneration]can't find undergroundOreAbundant of tile: {tileId}");
            return null;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("surface:");
            stringBuilder.Append("\n");
            foreach (var oreData in _vanillaSurfaceOreDataList)
            {
                stringBuilder.Append(oreData);
                stringBuilder.Append("\n");
            }

            stringBuilder.Append("deep:");
            stringBuilder.Append("\n");
            foreach (var oreData in _vanillaUndergroundOreDataList)
            {
                stringBuilder.Append(oreData);
                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref worldSurfaceOreAbundant, "worldSurfaceOreAbundant", LookMode.Value,
                LookMode.Deep);
            Scribe_Collections.Look(ref worldUndergroundOreAbundant, "worldUndergroundOreAbundant", LookMode.Value,
                LookMode.Deep);
        }
    }
}