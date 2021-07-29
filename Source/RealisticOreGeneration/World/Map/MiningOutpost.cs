// ******************************************************************
//       /\ /|       @file       MiningOutpost.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-25 23:53:18
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using RimWorld.Planet;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    public class MiningOutpost : MapParent
    {
        public override MapGeneratorDef MapGeneratorDef => MapGeneratorDefOf.SrMiningOutpost;

        /// <summary>
        /// map destroy occur on ticking
        /// </summary>
        /// <param name="alsoRemoveWorldObject"></param>
        /// <returns></returns>
        public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
        {
            alsoRemoveWorldObject = Destroyed;
            return Destroyed;
        }
    }
}
