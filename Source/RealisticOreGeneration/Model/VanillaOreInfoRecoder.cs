// ******************************************************************
//       /\ /|       @file       VanillaOreInfoRecoder.cs
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
    public class VanillaOreInfoRecoder : BaseSingleTon<VanillaOreInfoRecoder>, IExposable
    {
        private float vanallaTotalSurfaceComonality;
        private float vanallaTotalUndergroundComonality;
        private readonly List<OreData>
            _vanillaSurfaceOreDataList = new List<OreData>(); //vanilla data of all surface ores
        private readonly List<OreData>
            _vanillaUndergroundOreDataList = new List<OreData>(); //vanilla data of all underground ores
        private Dictionary<int, TileOreData> worldTileOreDataHashmap = new Dictionary<int, TileOreData>();
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
                        $"{MsicDef.LogTag}Unexpected buildingProperties: {thingdef.defName}");
                    continue;
                }

                var mineableThing = buildingProperties.mineableThing;
                if (mineableThing == null)
                {
                    Log.Warning($"{MsicDef.LogTag}Unexpected mineableThing: {thingdef.defName}");
                    continue;
                }

                var oreData = new OreData(thingdef.defName, buildingProperties.mineableScatterCommonality,
                    buildingProperties.mineableScatterLumpSizeRange.Average, buildingProperties.mineableYield,
                    mineableThing.BaseMarketValue);
                _vanillaSurfaceOreDataList.Add(oreData);
                vanallaTotalSurfaceComonality = 0f;
                vanallaTotalSurfaceComonality += oreData.commonality;
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
        /// how many surface ores can be genetated
        /// </summary>
        /// <returns></returns>
        public int GetSurfaceOreDataListCount()
        {
            return _vanillaSurfaceOreDataList.Count;
        }

        public float GetNormalizedSurfaceCommonality(int index)
        {
            if (_vanillaSurfaceOreDataList != null && _vanillaSurfaceOreDataList.Count > index)
            {
                return _vanillaSurfaceOreDataList[index].commonality / vanallaTotalSurfaceComonality;
            }

            Log.Error($"{MsicDef.LogTag}can't find surface oreData on index: {index}");
            return 0f;
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
                vanallaTotalUndergroundComonality = 0f;
                vanallaTotalUndergroundComonality += oreData.commonality;
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
        /// how many underground ores can be genetated
        /// </summary>
        /// <returns></returns>
        public int GetUndergroundOreDataListCount()
        {
            return _vanillaUndergroundOreDataList.Count;
        }

        public float GetNormalizedUndergroundCommonality(int index)
        {
            if (_vanillaUndergroundOreDataList != null && _vanillaUndergroundOreDataList.Count > index)
            {
                return _vanillaUndergroundOreDataList[index].commonality / vanallaTotalUndergroundComonality;
            }

            Log.Error($"{MsicDef.LogTag}can't find underground oreData on index: {index}");
            return 0f;
        }

        /// <summary>
        /// set surface ore data of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <param name="berlinFactor"></param>
        /// <param name="surfaceValueFactor"></param>
        /// <param name="undergroundValueFactor"></param>
        /// <param name="surfaceDistrubtion"></param>
        /// <param name="undergroundDistrubtion"></param>
        public void SetTileOreData(int tileId, float berlinFactor, float surfaceValueFactor, float undergroundValueFactor,
            Dictionary<string, float> surfaceDistrubtion, Dictionary<string, float> undergroundDistrubtion)
        {
            if (worldTileOreDataHashmap.ContainsKey(tileId))
            {
                Log.Warning($"{MsicDef.LogTag}ore data of tile: {tileId} has been calced");
                return;
            }

            var tileOreData = new TileOreData(berlinFactor, surfaceValueFactor, undergroundValueFactor)
            {
                surfaceDistrubtion = surfaceDistrubtion,
                undergroundDistrubtion = undergroundDistrubtion
            };

            worldTileOreDataHashmap.Add(tileId, tileOreData);
        }

        /// <summary>
        /// get ore data of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <returns></returns>
        public TileOreData GetTileOreData(int tileId)
        {
            if (worldTileOreDataHashmap.ContainsKey(tileId))
            {
                return worldTileOreDataHashmap[tileId];
            }

            Log.Error($"{MsicDef.LogTag}can't find ore data of tile: {tileId}");
            return null;
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

            stringBuilder.Append("underground vanilla data:");
            stringBuilder.Append("\n");
            foreach (var oreData in _vanillaUndergroundOreDataList)
            {
                stringBuilder.Append(oreData);
                stringBuilder.Append("\n");
            }

            stringBuilder.Append("tile count: ");
            stringBuilder.Append(worldTileOreDataHashmap.Count);
            return stringBuilder.ToString();
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref worldTileOreDataHashmap, "worldTileOreDataHashmap", LookMode.Value, LookMode.Deep);
        }
    }
}
