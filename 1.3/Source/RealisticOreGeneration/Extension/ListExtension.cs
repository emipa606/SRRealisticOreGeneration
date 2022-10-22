// ******************************************************************
//       /\ /|       @file       ListExtension.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-02 22:23:10
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    public static class ListExtension
    {
        public static void Shuffle<T>(this IList<T> list, int seed)
        {
            var count = list.Count;
            while (count > 1)
            {
                --count;
                var index = Rand.RangeInclusiveSeeded(0, count, seed);
                var obj = list[index];
                list[index] = list[count];
                list[count] = obj;
            }
        }
    }
}