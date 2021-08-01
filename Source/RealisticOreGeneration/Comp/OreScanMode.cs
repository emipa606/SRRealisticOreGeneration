// ******************************************************************
//       /\ /|       @file       OreScanMode.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-30 14:26:05
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************
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
