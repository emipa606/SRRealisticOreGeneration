// ******************************************************************
//       /\ /|       @file       PatchGameLoadGame.cs
//       \ V/        @brief      to patch Game.LoadGame()
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-02 13:39:14
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    [HarmonyPatch(typeof(Game), "LoadGame")]
    public class PatchGameLoadGame
    {
        /// <summary>
        /// if the mod is added halfway, world tile ore data need to be generate
        /// </summary>
        [UsedImplicitly]
        [HarmonyPostfix]
        // ReSharper disable once InconsistentNaming
        public static void Postfix()
        {
            if (WorldOreInfoRecorder.Instance.Initialized)
            {
                return;
            }

            var worldGrid = Find.WorldGrid;
            if (worldGrid == null)
            {
                Log.Error($"{MsicDef.LogTag}can't find world grid");
                return;
            }
            
            WorldOreDataGenerator.GenerateWorldOreInfo(Find.WorldGrid);
            Log.Warning($"{MsicDef.LogTag}join the game halfway, try to supplement the generated world ore data");
        }
    }
}
