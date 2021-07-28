// ******************************************************************
//       /\ /|       @file       PatchMain.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-25 20:33:02
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    [StaticConstructorOnStartup]
    public class PatchMain
    {
        static PatchMain()
        {
            var instance = new Harmony(CoreDef.LogTag);
            instance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
