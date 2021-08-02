// ******************************************************************
//       /\ /|       @file       WorldObjectCompPropertiesAbandon.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-29 16:52:11
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using RimWorld;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    public class WorldObjectCompPropertiesAbandon : WorldObjectCompProperties
    {
        public WorldObjectCompPropertiesAbandon()
        {
            compClass = typeof(WorldObjectCompAbandon);
        }
    }
}