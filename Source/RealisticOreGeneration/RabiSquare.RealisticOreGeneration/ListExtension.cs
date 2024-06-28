using System.Collections.Generic;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

public static class ListExtension
{
    public static void Shuffle<T>(this IList<T> list, int seed)
    {
        var num = list.Count;
        while (num > 1)
        {
            num--;
            var index = Rand.RangeInclusiveSeeded(0, num, seed);
            (list[index], list[num]) = (list[num], list[index]);
        }
    }
}