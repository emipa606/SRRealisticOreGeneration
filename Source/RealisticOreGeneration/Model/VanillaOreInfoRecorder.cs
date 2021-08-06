// ******************************************************************
//       /\ /|       @file       VanillaOreInfoRecorder.cs
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
    public class VanillaOreInfoRecorder : BaseSingleTon<VanillaOreInfoRecorder>
    {
        private float _vanillaTotalSurfaceCommonality;
        private float _vanillaTotalUndergroundCommonality;

        private readonly List<OreData>
            _vanillaSurfaceOreDataList = new List<OreData>(); //vanilla data of all surface ores

        private readonly List<OreData>
            _vanillaUndergroundOreDataList = new List<OreData>(); //vanilla data of all underground ores

        /// <summary>
        /// set vanilla data of each surface ore
        /// </summary>
        /// <param name="thingDefList"></param>
        public void SetSurfaceOreDataList(IEnumerable<ThingDef> thingDefList)
        {
            foreach (var thingDef in thingDefList)
            {
                var buildingProperties = thingDef.building;
                if (buildingProperties == null)
                {
                    Log.Warning($"{MsicDef.LogTag}Unexpected buildingProperties: {thingDef.defName}");
                    continue;
                }

                var mineableThing = buildingProperties.mineableThing;
                if (mineableThing == null)
                {
                    Log.Warning($"{MsicDef.LogTag}Unexpected mineableThing: {thingDef.defName}");
                    continue;
                }

                var oreData = new OreData(thingDef.defName, buildingProperties.mineableScatterCommonality,
                    buildingProperties.mineableScatterLumpSizeRange, buildingProperties.mineableYield,
                    mineableThing.BaseMarketValue);
                _vanillaSurfaceOreDataList.Add(oreData);
                _vanillaTotalSurfaceCommonality = 0f;
                _vanillaTotalSurfaceCommonality += oreData.commonality;
            }
        }

        /// <summary>
        /// get vanilla data of surface ore by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public OreData GetSurfaceOreDataByIndex(int index)
        {
            if (_vanillaSurfaceOreDataList != null && _vanillaSurfaceOreDataList.Count > index)
            {
                return _vanillaSurfaceOreDataList[index];
            }

            Log.Error($"{MsicDef.LogTag}can't find surface oreData on index: {index}");
            return null;
        }

        /// <summary>
        /// how many surface ores can be generated
        /// </summary>
        /// <returns></returns>
        public int GetSurfaceOreDataListCount()
        {
            return _vanillaSurfaceOreDataList.Count;
        }

        public IntRange GetRandomSurfaceLumpSize()
        {
            var randomIndex = new IntRange(0, _vanillaSurfaceOreDataList.Count - 1).RandomInRange;
            var oreData = GetSurfaceOreDataByIndex(randomIndex);
            if (oreData != null)
            {
                return oreData.lumpSize;
            }

            Log.Error($"{MsicDef.LogTag}cant find ore data by random index: {randomIndex}");
            return new IntRange(1, 20);
        }

        public float GetNormalizedSurfaceCommonality(int index)
        {
            if (_vanillaSurfaceOreDataList != null && _vanillaSurfaceOreDataList.Count > index)
            {
                return _vanillaSurfaceOreDataList[index].commonality / _vanillaTotalSurfaceCommonality;
            }

            Log.Error($"{MsicDef.LogTag}can't find surface oreData on index: {index}");
            return 0f;
        }

        public float GetNormalizedUndergroundCommonality(int index)
        {
            if (_vanillaUndergroundOreDataList != null && _vanillaUndergroundOreDataList.Count > index)
            {
                return _vanillaUndergroundOreDataList[index].commonality / _vanillaTotalUndergroundCommonality;
            }

            Log.Error($"{MsicDef.LogTag}can't find underground oreData on index: {index}");
            return 0f;
        }

        /// <summary>
        /// set vanilla data of each underground ore 
        /// </summary>
        /// <param name="thingDefList"></param>
        public void SetUndergroundOreDataList(IEnumerable<ThingDef> thingDefList)
        {
            foreach (var thingDef in thingDefList)
            {
                var oreData = new OreData(thingDef.defName, thingDef.deepCommonality,
                    thingDef.deepLumpSizeRange, thingDef.deepCountPerPortion,
                    thingDef.BaseMarketValue);
                _vanillaUndergroundOreDataList.Add(oreData);
                _vanillaTotalUndergroundCommonality = 0f;
                _vanillaTotalUndergroundCommonality += oreData.commonality;
            }
        }

        /// <summary>
        /// get vanilla data of underground ore by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public OreData GetUndergroundOreDataByIndex(int index)
        {
            if (_vanillaUndergroundOreDataList != null && _vanillaUndergroundOreDataList.Count > index)
            {
                return _vanillaUndergroundOreDataList[index];
            }

            Log.Error($"{MsicDef.LogTag}can't find underground oreData on index: {index}");
            return null;
        }
        
        /// <summary>
        /// how many underground ores can be generated
        /// </summary>
        /// <returns></returns>
        public int GetUndergroundOreDataListCount()
        {
            return _vanillaUndergroundOreDataList.Count;
        }

        public IntRange GetRandomUndergroundLumpSize()
        {
            var randomIndex = new IntRange(0, _vanillaUndergroundOreDataList.Count - 1).RandomInRange;
            var oreData = GetUndergroundOreDataByIndex(randomIndex);
            if (oreData != null)
            {
                return oreData.lumpSize;
            }

            Log.Error($"{MsicDef.LogTag}cant find ore data by random index: {randomIndex}");
            return new IntRange(1, 20);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("surface vanilla data:");
            stringBuilder.Append("\n");
            foreach (var oreData in _vanillaSurfaceOreDataList)
            {
                stringBuilder.Append(oreData);
                stringBuilder.Append("\n");
            }

            stringBuilder.Append("\n");
            stringBuilder.Append("underground vanilla data:");
            stringBuilder.Append("\n");
            foreach (var oreData in _vanillaUndergroundOreDataList)
            {
                stringBuilder.Append(oreData);
                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }
    }
}