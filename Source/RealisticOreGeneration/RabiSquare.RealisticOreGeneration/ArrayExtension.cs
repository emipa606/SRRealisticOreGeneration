using System.Linq;

namespace RabiSquare.RealisticOreGeneration;

public static class ArrayExtension
{
    public static void Normalized(this float[] array)
    {
        if (array == null || array.Length == 0)
        {
            return;
        }

        var num = array.Sum();
        for (var i = 0; i < array.Length; i++)
        {
            array[i] /= num;
        }
    }
}