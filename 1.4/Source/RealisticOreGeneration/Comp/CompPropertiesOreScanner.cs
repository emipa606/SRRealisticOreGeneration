// ******************************************************************
//       /\ /|       @file       CompPropertiesOreScanner.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-30 17:19:31
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using RimWorld;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    public class CompPropertiesOreScanner : CompProperties_Scanner
    {
        public CompPropertiesOreScanner()
        {
            compClass = typeof(CompOreScanner);
        }
    }
}