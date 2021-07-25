// // ******************************************************************
// //       /\ /|       @file       PatchCaravanGetGizmos.cs
// //       \ V/        @brief      Added the command for the caravan to enter the map for mining
// //       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
// //       /  |                    
// //      /  \\        @Modified   2021-07-25 20:00:41
// //    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// // ******************************************************************
//
// using System.Collections.Generic;
// using HarmonyLib;
// using JetBrains.Annotations;
// using RimWorld.Planet;
// using Verse;
//
// // ReSharper disable InconsistentNaming
// namespace RabiSquare.RealisticOreGeneration
// {
//     [UsedImplicitly]
//     [HarmonyPatch(typeof(Caravan), "GetGizmos")]
//     public class PatchCaravanGetGizmos
//     {
//         /// <summary>
//         /// add mining
//         /// </summary>
//         /// <param name="__result"></param>
//         [HarmonyPostfix]
//         public static void Postfix(Caravan __instance, IEnumerable<Gizmo> __result)
//         {
//             // ReSharper disable once RedundantAssignment
//             __result.AddItem(new Command_Action
//             {
//                 icon = SettleUtility.SettleCommandTex,
//                 defaultLabel = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
//                 action = delegate { Log.Warning("generate map"); }
//             });
//         }
//
//         //
//         // private static IEnumerable<Gizmo> GetNewGizmos(ISelectable caravan)
//         // {
//         //     foreach (var gizmo in caravan.GetGizmos())
//         //     {
//         //         yield return gizmo;
//         //     }
//         //
//         //     yield return new Command_Action
//         //     {
//         //         icon = SettleUtility.SettleCommandTex,
//         //         defaultLabel = "Mining",
//         //         action = delegate { Log.Warning("generate map"); }
//         //     };
//         // }
//     }
// }