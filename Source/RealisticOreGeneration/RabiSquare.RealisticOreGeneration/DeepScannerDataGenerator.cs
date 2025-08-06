using RimWorld.Planet;
using UnityEngine;

namespace RabiSquare.RealisticOreGeneration;

public static class DeepScannerDataGenerator
{
    private const int VanillaScanFindGuaranteedDays = 6;

    private const int VanillaScanFindMtbDays = 3;

    public static void GenerateDeepScannerFindDays(PlanetTile tileId, out int scanFindGuaranteedDays,
        out int scanFindMtbDays)
    {
        scanFindGuaranteedDays = VanillaScanFindGuaranteedDays;
        scanFindMtbDays = VanillaScanFindMtbDays;
        var tileOreData = BaseSingleTon<WorldOreDataGenerator>.Instance.GetTileOreData(tileId);
        var num = (int)tileOreData.FreeUndergroundCycleCount;
        var undergroundMiningCount = BaseSingleTon<WorldOreInfoRecorder>.Instance.GetUndergroundMiningCount(tileId);
        if (undergroundMiningCount < num)
        {
            return;
        }

        var num2 = Mathf.Log(10f) / num;
        var num3 = undergroundMiningCount - num;
        scanFindGuaranteedDays *= (int)Mathf.Exp(num2 * num3);
        scanFindMtbDays *= (int)Mathf.Exp(num2 * num3);
    }
}