// ******************************************************************
//       /\ /|       @file       WorldUtils.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-04 06:58:29
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using RimWorld.Planet;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    public static class WorldUtils
    {
        public static void SetWorldLayerDirty<T>() where T : WorldLayer
        {
            var world = Find.World;
            var renderer = world?.renderer;
            renderer?.SetDirty<T>();
        }
    }
}