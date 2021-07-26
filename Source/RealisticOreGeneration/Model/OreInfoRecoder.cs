// ******************************************************************
//       /\ /|       @file       OreInfoRecoder.cs
//       \ V/        @brief      Record the raw data of all ore
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-26 19:49:24
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    public class OreInfoRecoder : BaseSingleTon<OreInfoRecoder>
    {
        private readonly List<OreData> _oreDataList = new List<OreData>();

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
                _oreDataList.Add(oreData);
            }
        }

        public OreData GetOreDataByIndex(int index)
        {
            if (_oreDataList.Count > index)
            {
                return _oreDataList[index];
            }

            Log.Error($"[RabiSquare.RealisticOreGeneration]can't find oreData on index: {index}");
            return null;
        }

        public int GetOreDataListCount()
        {
            return _oreDataList.Count;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var oreData in _oreDataList)
            {
                stringBuilder.Append(oreData);
                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }
    }
}