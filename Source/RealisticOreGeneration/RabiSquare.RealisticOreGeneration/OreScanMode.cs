using System;

namespace RabiSquare.RealisticOreGeneration;

[Flags]
public enum OreScanMode : byte
{
    SingleSurface = 0,
    SingleUnderground = 1,
    RangeSurface = 2,
    RangeUnderground = 3
}