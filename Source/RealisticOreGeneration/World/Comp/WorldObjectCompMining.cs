// ******************************************************************
//       /\ /|       @file       WorldObjectCompMining.cs
//       \ V/        @brief      Give caravans the ability to enter the map mining
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-25 22:31:38
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************
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
        private Caravan Caravan => (Caravan)parent;

        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (Find.WorldSelector.SingleSelectedObject != Caravan)
            {
                yield break;
            }

            var commandAction = new Command_Action
            {
                defaultLabel = "SrBuildMiningOutpost".Translate(),
                icon = FormCaravanCommand,
                action = OnClickMining
            };

            if (Find.WorldObjects.AnyMapParentAt(parent.Tile))
            {
                commandAction.Disable("TileOccupied".Translate());
            }

            if (MiningOutpostRecorder.Instance.GetOutpostCount() >= SettingWindow.Instance.settingModel.maxOutpostCount)
            {
                commandAction.Disable("SrCommandTooManyOutpostHere".Translate());
            }

            yield return commandAction;
        }

        private void OnClickMining()
        {
            if (Caravan == null)
            {
                Log.Warning($"{MsicDef.LogTag}can't find caravan");
                return;
            }

            var faction = Caravan.Faction;
            if (faction != Faction.OfPlayer)
            {
                Log.Warning($"{MsicDef.LogTag}caravan is not player");
                return;
            }

            var mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.SrMiningOutpost);
            mapParent.Tile = Caravan.Tile;
            mapParent.SetFaction(Faction.OfPlayer);
            Find.WorldObjects.Add(mapParent);
            var map = GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile,
                new IntVec3(SettingWindow.Instance.settingModel.outpostMapSize, 1, SettingWindow.Instance.settingModel.outpostMapSize),
                null);
            CaravanEnterMapUtility.Enter(Caravan, map, CaravanEnterMode.Edge);
            Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate((NamedArgument)mapParent),
                "LetterCaravanEnteredMap".Translate((NamedArgument)Caravan.Label, (NamedArgument)mapParent)
                    .CapitalizeFirst(), LetterDefOf.NeutralEvent, Caravan.PawnsListForReading);
            MiningOutpostRecorder.Instance.MiningOutpostCountIncrease();
        }
    }
}
