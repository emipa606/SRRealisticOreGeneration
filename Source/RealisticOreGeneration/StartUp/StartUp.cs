// ******************************************************************
//       /\ /|       @file       StartUp.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-26 19:39:48
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Linq;
using JetBrains.Annotations;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [StaticConstructorOnStartup]
    [UsedImplicitly]
    public static class StartUp
    {
        static StartUp()
        {
            var oreList = DefDatabase<ThingDef>.AllDefs
                .Where(t => t.building != null && t.building.mineableScatterCommonality > 0);
            OreInfoRecoder.Instance.SetOreDataList(oreList);
            if (Prefs.DevMode)
            {
                Log.Message($"{OreInfoRecoder.Instance}");
            }
        }
    }
}