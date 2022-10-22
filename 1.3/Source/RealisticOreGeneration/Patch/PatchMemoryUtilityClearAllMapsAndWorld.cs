// ******************************************************************
//       /\ /|       @file       PatchMemoryUtilityClearAllMapsAndWorld.cs
//       \ V/        @brief      to patch MemoryUtility.ClearAllMapsAndWorld()
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-06 03:18:05
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using HarmonyLib;
using JetBrains.Annotations;
using Verse.Profile;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    [HarmonyPatch(typeof(MemoryUtility), "ClearAllMapsAndWorld")]
    public class PatchMemoryUtilityClearAllMapsAndWorld
    {
        /// <summary>
        /// clear our data
        /// </summary>
        [UsedImplicitly]
        [HarmonyPostfix]
        public static void Postfix()
        {
            WorldOreInfoRecorder.Instance.Clear();
            MiningOutpostRecorder.Instance.Clear();
        }
    }
}