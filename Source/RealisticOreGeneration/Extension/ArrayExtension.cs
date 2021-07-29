// ******************************************************************
//       /\ /|       @file       ArrayExtension.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-29 13:26:29
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************
using System.Linq;

namespace RabiSquare.RealisticOreGeneration
{
    public static class ArrayExtension
    {
        public static void Normalized(this float[] array)
        {
            if (array == null || array.Length <= 0)
            {
                return;
            }

            var totalValue = array.Sum();
            for (var i = 0; i < array.Length; i++)
            {
                array[i] /= totalValue;
            }
        }
    }
}
