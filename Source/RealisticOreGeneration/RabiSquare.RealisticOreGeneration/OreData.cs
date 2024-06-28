using Verse;

namespace RabiSquare.RealisticOreGeneration;

public class OreData(string defName, float commonality, IntRange lumpSize, float yield, float marketValue)
{
    public readonly float commonality = commonality;

    public readonly string defName = defName;

    public readonly IntRange lumpSize = lumpSize;

    public readonly float marketValue = marketValue;

    public readonly float yield = yield;

    public override string ToString()
    {
        return
            $"[RabiSquare.RealisticOreGeneration]defName: {defName}\nmineableScatterCommonality: {commonality}, lumpSize: ({lumpSize.min},{lumpSize.max}), mineableYield: {yield}, marketValue: {marketValue}\nlumpValue: {lumpSize.Average * yield * marketValue}, weightValue: {lumpSize.Average * yield * marketValue * commonality}";
    }
}