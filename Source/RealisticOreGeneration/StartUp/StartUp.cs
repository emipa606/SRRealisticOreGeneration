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
            var surfaceOreList = DefDatabase<ThingDef>.AllDefs
                .Where(t => t.building != null && t.building.mineableScatterCommonality > 0);
            VanillaOreInfoRecoder.Instance.SetSurfaceOreDataList(surfaceOreList);
            var undergroundOreList = DefDatabase<ThingDef>.AllDefs.Where(t => t.deepCommonality > 0);
            VanillaOreInfoRecoder.Instance.SetUndergroundOreDataList(undergroundOreList);
            if (Prefs.DevMode)
            {
                Log.Message($"{VanillaOreInfoRecoder.Instance}");
            }
        }
    }
}