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
        private readonly List<OreData> _vanillaOreDataList = new List<OreData>(); //vanilla data of all ores
        private Dictionary<int, Dictionary<string, float>> worldOreAbundant =
            new Dictionary<int, Dictionary<string, float>>(); //commonality of each ore in each tile <tileId,<defName,commonality>>

        /// <summary>
        /// set vanilla data of each ore
        /// </summary>
        /// <param name="thingDefs"></param>
        public void SetOreDataList(IEnumerable<ThingDef> thingDefs)
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
                _vanillaOreDataList.Add(oreData);
            }
        }

        /// <summary>
        /// get vanilla data of ore by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public OreData GetOreDataByIndex(int index)
        {
            if (_vanillaOreDataList.Count > index)
            {
                return _vanillaOreDataList[index];
            }

            Log.Error($"[RabiSquare.RealisticOreGeneration]can't find oreData on index: {index}");
            return null;
        }

        /// <summary>
        /// set ore abundant of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <param name="oreAbundant"></param>
        public void SetOreAbundant(int tileId, Dictionary<string, float> oreAbundant)
        {
            if (worldOreAbundant.ContainsKey(tileId))
            {
                Log.Warning($"[RabiSquare.RealisticOreGeneration]this tile: {tileId} has been calced");
                return;
            }

            worldOreAbundant.Add(tileId, oreAbundant);
        }

        /// <summary>
        /// get ore abundant of tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <returns></returns>
        public Dictionary<string, float> GetOreAbundant(int tileId)
        {
            if (worldOreAbundant.ContainsKey(tileId))
            {
                return worldOreAbundant[tileId];
            }

            Log.Error($"[RabiSquare.RealisticOreGeneration]can't find oreAbundant of tile: {tileId}");
            return null;
        }

        /// <summary>
        /// how many ores
        /// </summary>
        /// <returns></returns>
        public int GetOreDataListCount()
        {
            return _vanillaOreDataList.Count;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var oreData in _vanillaOreDataList)
            {
                stringBuilder.Append(oreData);
                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref worldOreAbundant, "worldOreAbundant", LookMode.Value,
                LookMode.Deep);
        }
    }
}
