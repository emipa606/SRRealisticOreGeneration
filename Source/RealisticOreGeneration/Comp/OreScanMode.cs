using System;

namespace RabiSquare.RealisticOreGeneration
{
    [Flags]
    public enum OreScanMode : byte
    {
        SingleSurface = 0x00,
        SingleUnderground = 0x01,//underground on
        RangeSurface = 0x02, //range on
        RangeUnderground = 0x03
    }
}
