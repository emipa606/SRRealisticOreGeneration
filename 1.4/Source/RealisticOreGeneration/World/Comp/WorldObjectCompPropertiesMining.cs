// ******************************************************************
//       /\ /|       @file       WorldObjectCompPropertiesMining.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-25 22:30:59
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using RimWorld;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    public class WorldObjectCompPropertiesMining : WorldObjectCompProperties
    {
        public WorldObjectCompPropertiesMining()
        {
            compClass = typeof(WorldObjectCompMining);
        }
    }
}