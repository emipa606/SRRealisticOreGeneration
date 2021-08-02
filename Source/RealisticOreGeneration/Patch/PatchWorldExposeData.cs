// ******************************************************************
//       /\ /|       @file       PatchWorldExposeData.cs
//       \ V/        @brief      to patch World.ExposeData()
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-01 14:07:27
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using HarmonyLib;
using JetBrains.Annotations;
using RimWorld.Planet;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    [HarmonyPatch(typeof(World), "ExposeData")]
    public class PatchWorldExposeData
    {
        /// <summary>
        ///     save our param
        /// </summary>
        [UsedImplicitly]
        [HarmonyPrefix]
        public static bool Prefix()
        {
            WorldOreInfoRecorder.Instance.ExposeData();
            MiningOutpostRecorder.Instance.ExposeData();
            return true;
        }
    }
}