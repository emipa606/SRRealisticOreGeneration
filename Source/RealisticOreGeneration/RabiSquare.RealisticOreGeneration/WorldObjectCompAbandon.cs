using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RabiSquare.RealisticOreGeneration;

[StaticConstructorOnStartup]
public class WorldObjectCompAbandon : WorldObjectComp
{
    private static readonly Texture2D AbandonCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/AbandonMiningPost");

    private MapParent MapParent => parent as MapParent;

    private Map Map => MapParent?.Map;

    public override IEnumerable<Gizmo> GetGizmos()
    {
        if (Find.WorldSelector.SingleSelectedObject != MapParent)
        {
            yield break;
        }

        var commandAction = new Command_Action
        {
            defaultLabel = "SrCommandAbandonMiningOutpost".Translate(),
            icon = AbandonCommandTex,
            action = OnClickAbandon
        };
        if (Map == null)
        {
            Log.Error($"[RabiSquare.RealisticOreGeneration]can't find map: {parent.Label}");
            yield break;
        }

        var mapPawns = Map.mapPawns;
        if (mapPawns == null)
        {
            Log.Error($"[RabiSquare.RealisticOreGeneration]can't find mapPawns: {parent.Label}");
            yield break;
        }

        if (mapPawns.AnyColonistSpawned)
        {
            commandAction.Disable("SrCommandAbandonFailAnyColonistsThere".Translate());
        }

        yield return commandAction;
    }

    private void OnClickAbandon()
    {
        abandon();
        SoundDefOf.Tick_High.PlayOneShotOnCamera();
    }

    private void abandon()
    {
        MapParent.Destroy();
        BaseSingleTon<MiningOutpostRecorder>.Instance.MiningOutpostCountDecrease();
        Find.GameEnder.CheckOrUpdateGameOver();
    }
}