using Verse;

namespace RabiSquare.RealisticOreGeneration;

public class MiningOutpostRecorder : BaseSingleTon<MiningOutpostRecorder>, IExposable
{
    private int _miningOutpostCount;

    public void ExposeData()
    {
        Scribe_Values.Look(ref _miningOutpostCount, "_miningOutpostCount");
    }

    public void MiningOutpostCountIncrease()
    {
        _miningOutpostCount++;
    }

    public void MiningOutpostCountDecrease()
    {
        _miningOutpostCount--;
    }

    public int GetOutpostCount()
    {
        return _miningOutpostCount;
    }

    public void Clear()
    {
        _miningOutpostCount = 0;
    }
}