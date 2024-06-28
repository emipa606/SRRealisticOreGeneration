using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

[StaticConstructorOnStartup]
public class WorldObjectCompMining : WorldObjectComp
{
    private static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/BuildMiningPost");

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

        if (BaseSingleTon<MiningOutpostRecorder>.Instance.GetOutpostCount() >=
            SettingWindow.Instance.settingModel.maxOutpostCount)
        {
            commandAction.Disable("SrCommandTooManyOutpostHere".Translate());
        }

        yield return commandAction;
    }

    private void OnClickMining()
    {
        if (Caravan == null)
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]can't find caravan");
            return;
        }

        var faction = Caravan.Faction;
        if (faction != Faction.OfPlayer)
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]caravan is not player");
            return;
        }

        var mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.SrMiningOutpost);
        mapParent.Tile = Caravan.Tile;
        mapParent.SetFaction(Faction.OfPlayer);
        Find.WorldObjects.Add(mapParent);
        LongEventHandler.QueueLongEvent(
            delegate
            {
                GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, Find.World.info.initialMapSize, null);
            }, "GeneratingMap", true, (Action<Exception>)GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap);
        LongEventHandler.QueueLongEvent(delegate
        {
            var pawn = Caravan.PawnsListForReading[0];
            CaravanEnterMapUtility.Enter(Caravan, mapParent.Map, CaravanEnterMode.Edge);
            CameraJumper.TryJump(pawn);
        }, "SpawningColonists", true, (Action<Exception>)GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap);
        BaseSingleTon<MiningOutpostRecorder>.Instance.MiningOutpostCountIncrease();
    }
}