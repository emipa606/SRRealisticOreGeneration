// ******************************************************************
//       /\ /|       @file       WorldObjectCompMining.cs
//       \ V/        @brief      Give caravans the ability to enter the map mining
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-25 22:31:38
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [StaticConstructorOnStartup]
    public class WorldObjectCompMining : WorldObjectComp
    {
        private static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan");

        public WorldObjectCompPropertiesMining Props => (WorldObjectCompPropertiesMining) props;

        private readonly Caravan _caravan;

        public WorldObjectCompMining()
        {
            _caravan = (Caravan) parent;
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (Prefs.DevMode)
            {
                Command_Action commandAction = new Command_Action
                {
                    defaultLabel = "Mining",
                    icon = FormCaravanCommand,
                    action = OnClickMining
                };
                yield return commandAction;
            }
        }

        private void OnClickMining()
        {
            var faction = _caravan.Faction;
            if (faction != Faction.OfPlayer)
            {
                Log.Warning("caravan is not player");
                return;
            }

            LongEventHandler.QueueLongEvent(
                delegate
                {
                    GetOrGenerateMapUtility.GetOrGenerateMap(_caravan.Tile, Find.World.info.initialMapSize, null);
                }, "GeneratingMap", true,
                GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap);
            // LongEventHandler.QueueLongEvent(delegate()
            //     {
            //         Map map = newHome.Map;
            //         Thing t = caravan.PawnsListForReading[0];
            //         CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Center,
            //             CaravanDropInventoryMode.DropInstantly, false, (IntVec3 x) => x.GetRoom(map).CellCount >= 600);
            //         CameraJumper.TryJump(t);
            //     }, "SpawningColonists", true,
            //     new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true);
        }
    }
}