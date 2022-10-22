// ******************************************************************
//       /\ /|       @file       TileExtension.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-07 03:33:30
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using RimWorld;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    public static class TileExtension
    {
        public static bool IsTileOceanOrLake(this int tileId)
        {
            var worldGrid = Find.WorldGrid;
            var tile = worldGrid?.tiles[tileId];
            var biome = tile?.biome;
            if (biome == null)
            {
                return false;
            }

            return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
        }
    }
}