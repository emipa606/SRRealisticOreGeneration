using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

[StaticConstructorOnStartup]
public class CompOreScanner : CompScanner
{
    private const int RangeModeRadius = 6;

    private const int SingleModeRadius = 24;

    private static readonly Texture2D SingleScanModeCommand = ContentFinder<Texture2D>.Get("UI/Commands/SingleMode");

    private static readonly Texture2D RangeScanModeCommand = ContentFinder<Texture2D>.Get("UI/Commands/AutoRangeMode");

    private static readonly Texture2D SurfaceScanModeCommand = ContentFinder<Texture2D>.Get("UI/Commands/SurfaceMode");

    private static readonly Texture2D UndergroundScanModeCommand =
        ContentFinder<Texture2D>.Get("UI/Commands/UndergroundMode");

    private static readonly Texture2D TileSelectedCommand = ContentFinder<Texture2D>.Get("UI/Commands/SelectTarget");

    private static readonly Texture2D ScanCursor = ContentFinder<Texture2D>.Get("UI/Overlays/ScanCursor");

    private OreScanMode _oreScanMode = OreScanMode.RangeSurface;

    private Dictionary<int, int> _ringMap = new();

    private int _selectedTile = -1;

    private new CompPropertiesOreScanner Props => (CompPropertiesOreScanner)props;

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        base.PostSpawnSetup(respawningAfterLoad);
        getRingMap(SingleModeRadius);
        findDefaultTarget();
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref _selectedTile, "_selectedTile");
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        foreach (var item in base.CompGetGizmosExtra())
        {
            yield return item;
        }

        yield return getScanAreaGizmo();
        foreach (var item2 in getScanModeGizmo())
        {
            yield return item2;
        }
    }

    protected override void DoFind(Pawn worker)
    {
        var oreScanMode = _oreScanMode;
        if (oreScanMode is OreScanMode.SingleUnderground or OreScanMode.RangeUnderground)
        {
            OnUndergroundFind();
        }
        else
        {
            OnSurfaceFind();
        }

        Find.LetterStack.ReceiveLetter("SrScanComplete".Translate(), "SrScanCompleteDesc".Translate(),
            LetterDefOf.PositiveEvent, new GlobalTargetInfo(_selectedTile));
        if (Prefs.DevMode)
        {
            Log.Message(
                $"[RabiSquare.RealisticOreGeneration]scanning complete: {_selectedTile}");
        }

        findDefaultTarget();
    }

    public override void CompTickRare()
    {
        if (_selectedTile != -1)
        {
            return;
        }

        findDefaultTarget();
        if (_selectedTile != -1)
        {
            return;
        }

        if ((_oreScanMode & OreScanMode.RangeSurface) == OreScanMode.RangeSurface)
        {
            Messages.Message("SrAutomaticSwitchingMode".Translate(parent.Label), MessageTypeDefOf.NeutralEvent);
            _oreScanMode &= ~OreScanMode.RangeSurface;
            findDefaultTarget();
            if (_selectedTile != -1)
            {
                return;
            }
        }

        if (!CanUseNow)
        {
            return;
        }

        var comp = parent.GetComp<CompFlickable>();
        if (comp == null)
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]can't find comp");
            return;
        }

        comp.SwitchIsOn = false;
        Messages.Message("SrNoTargetTile".Translate(parent.Label), MessageTypeDefOf.NeutralEvent);
    }

    private void findDefaultTarget()
    {
        switch (_oreScanMode)
        {
            case OreScanMode.SingleSurface:
                findDefaultTargetSingle(true);
                break;
            case OreScanMode.SingleUnderground:
                findDefaultTargetSingle(false);
                break;
            case OreScanMode.RangeSurface:
                findDefaultTargetRange(true);
                break;
            case OreScanMode.RangeUnderground:
                findDefaultTargetRange(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (_selectedTile != -1)
        {
            updateCostTime();
        }

        if (Prefs.DevMode)
        {
            Log.Message($"{"[RabiSquare.RealisticOreGeneration]"}set default target: {_selectedTile}");
        }
    }

    private void findDefaultTargetSingle(bool isSurface)
    {
        var list = _ringMap.Keys.ToList();
        if (list.Count <= 0)
        {
            _selectedTile = -1;
            return;
        }

        list.Shuffle();
        foreach (var item in list)
        {
            if (isSurface)
            {
                if (BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileScannedSurface(item))
                {
                    continue;
                }
            }
            else if (BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileScannedUnderground(item))
            {
                continue;
            }

            if (item.IsTileOceanOrLake())
            {
                continue;
            }

            _selectedTile = item;
            return;
        }

        _selectedTile = -1;
    }

    private void findDefaultTargetRange(bool isSurface)
    {
        using (var enumerator = _ringMap.Where(delegate(KeyValuePair<int, int> kvp)
                   {
                       int result2;
                       if (isSurface)
                       {
                           var instance2 = BaseSingleTon<WorldOreInfoRecorder>.Instance;
                           result2 = !instance2.IsTileScannedSurface(kvp.Key) ? 1 : 0;
                       }
                       else
                       {
                           result2 = 1;
                       }

                       return (byte)result2 != 0;
                   }).Where(delegate(KeyValuePair<int, int> kvp)
                   {
                       int result;
                       if (!isSurface)
                       {
                           var instance = BaseSingleTon<WorldOreInfoRecorder>.Instance;
                           result = !instance.IsTileScannedUnderground(kvp.Key) ? 1 : 0;
                       }
                       else
                       {
                           result = 1;
                       }

                       return (byte)result != 0;
                   }).Where(kvp => !kvp.Key.IsTileOceanOrLake())
                   .Where(kvp => kvp.Value <= RangeModeRadius)
                   .GetEnumerator())
        {
            if (enumerator.MoveNext())
            {
                _selectedTile = enumerator.Current.Key;
                return;
            }
        }

        _selectedTile = -1;
    }

    private void getRingMap(int radius)
    {
        var worldGrid = Find.WorldGrid;
        if (worldGrid == null)
        {
            throw new Exception("[RabiSquare.RealisticOreGeneration]can't find world grid");
        }

        _ringMap = new Dictionary<int, int>();
        var list = new List<int> { parent.Tile };
        var list2 = new List<int>();
        var innerCircleSet = new HashSet<int> { parent.Tile };
        var list3 = new List<PlanetTile>();
        for (var i = 1; i <= radius; i++)
        {
            list2.Clear();
            foreach (var item in list)
            {
                worldGrid.GetTileNeighbors(item, list3);
                foreach (var item2 in list3.Where(neighbor => !innerCircleSet.Contains(neighbor)))
                {
                    innerCircleSet.Add(item2);
                    if (!item2.Tile.WaterCovered)
                    {
                        list2.Add(item2);
                    }
                }
            }

            list.Clear();
            list.AddRange(list2);
            foreach (var item3 in list2)
            {
                _ringMap.Add(item3, i);
            }
        }

        if (Prefs.DevMode)
        {
            Log.Message($"[RabiSquare.RealisticOreGeneration]ring map count: {_ringMap.Count}");
        }
    }

    private void updateCostTime()
    {
        var targetDistance = getTargetDistance(_selectedTile);
        Props.scanFindGuaranteedDays = 1.4f * targetDistance / (targetDistance + 1);
        Props.scanFindMtbDays = Props.scanFindGuaranteedDays / 2f;
        if ((_oreScanMode & OreScanMode.SingleUnderground) != OreScanMode.SingleUnderground)
        {
            return;
        }

        Props.scanFindGuaranteedDays *= 2f;
        Props.scanFindMtbDays *= 2f;
    }

    private int getTargetDistance(int tileId)
    {
        if (_ringMap.TryGetValue(tileId, out var distance))
        {
            return distance;
        }

        Log.Error($"[RabiSquare.RealisticOreGeneration]can't find target tile in circle tileId: {tileId}");
        return 1;
    }

    private void OnSurfaceFind()
    {
        if (_selectedTile == -1)
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]find tile with none on surface");
        }
        else
        {
            BaseSingleTon<WorldOreInfoRecorder>.Instance.RecordScannedTileSurface(_selectedTile);
        }
    }

    private void OnUndergroundFind()
    {
        if (_selectedTile == -1)
        {
            Log.Warning("[RabiSquare.RealisticOreGeneration]find tile with none in underground");
        }
        else
        {
            BaseSingleTon<WorldOreInfoRecorder>.Instance.RecordScannedTileUnderground(_selectedTile);
        }
    }

    private IEnumerable<Gizmo> getScanModeGizmo()
    {
        yield return new Command_Action
        {
            defaultLabel = "SrScanModeChange".Translate(),
            icon = (_oreScanMode & OreScanMode.RangeSurface) == OreScanMode.RangeSurface
                ? RangeScanModeCommand
                : SingleScanModeCommand,
            defaultDesc = (_oreScanMode & OreScanMode.RangeSurface) == OreScanMode.RangeSurface
                ? "SrChangeToSingleMode".Translate()
                : "SrChangeToRangeMode".Translate(),
            action = OnClickScanModeChange
        };
        if ((_oreScanMode & OreScanMode.RangeSurface) != OreScanMode.RangeSurface)
        {
            yield return new Command_Action
            {
                defaultLabel = "SrSelect".Translate(),
                icon = TileSelectedCommand,
                action = OnClickTileSelect
            };
        }
    }

    private Gizmo getScanAreaGizmo()
    {
        return new Command_Action
        {
            defaultLabel = "SrScanAreaChange".Translate(),
            icon = (_oreScanMode & OreScanMode.SingleUnderground) == OreScanMode.SingleUnderground
                ? UndergroundScanModeCommand
                : SurfaceScanModeCommand,
            defaultDesc = (_oreScanMode & OreScanMode.SingleUnderground) == OreScanMode.SingleUnderground
                ? "SrChangeToSurfaceMode".Translate()
                : "SrChangeToUndergroundMode".Translate(),
            action = OnClickScanAreaChange
        };
    }

    private void OnClickScanModeChange()
    {
        _oreScanMode = (_oreScanMode & OreScanMode.RangeSurface) == OreScanMode.RangeSurface
            ? _oreScanMode & ~OreScanMode.RangeSurface
            : _oreScanMode | OreScanMode.RangeSurface;
        findDefaultTarget();
    }

    private void OnClickScanAreaChange()
    {
        _oreScanMode = (_oreScanMode & OreScanMode.SingleUnderground) == OreScanMode.SingleUnderground
            ? _oreScanMode & ~OreScanMode.SingleUnderground
            : _oreScanMode | OreScanMode.SingleUnderground;
        findDefaultTarget();
    }

    private bool OnWorldTargetSelected(GlobalTargetInfo target)
    {
        if (!target.IsValid)
        {
            Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput,
                false);
            return false;
        }

        if (Find.WorldGrid.TraversalDistanceBetween(parent.Tile, target.Tile) > SingleModeRadius)
        {
            Messages.Message("TransportPodDestinationBeyondMaximumRange".Translate(), MessageTypeDefOf.RejectInput,
                false);
            return false;
        }

        if (BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileScannedUnderground(target.Tile) &&
            (_oreScanMode & OreScanMode.SingleUnderground) == OreScanMode.SingleUnderground ||
            BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileScannedSurface(target.Tile) &&
            (_oreScanMode & OreScanMode.SingleUnderground) != OreScanMode.SingleUnderground)
        {
            Messages.Message("SrRepeatScan".Translate(), MessageTypeDefOf.RejectInput, false);
            return false;
        }

        _selectedTile = target.Tile;
        updateCostTime();
        return true;
    }

    private TaggedString targetingLabelGetter(GlobalTargetInfo target)
    {
        if (!target.IsValid)
        {
            GUI.color = ColoredText.WarningColor;
            return "MessageTransportPodsDestinationIsInvalid".Translate();
        }

        if (Find.WorldGrid.TraversalDistanceBetween(parent.Tile, target.Tile) > SingleModeRadius)
        {
            GUI.color = ColoredText.WarningColor;
            return "TransportPodDestinationBeyondMaximumRange".Translate();
        }

        if (BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileScannedUnderground(target.Tile) &&
            (_oreScanMode & OreScanMode.SingleUnderground) == OreScanMode.SingleUnderground ||
            BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileScannedSurface(target.Tile) &&
            (_oreScanMode & OreScanMode.SingleUnderground) != OreScanMode.SingleUnderground)
        {
            GUI.color = ColoredText.WarningColor;
            return "SrRepeatScan".Translate();
        }

        GUI.color = ColoredText.ExpectationsColor;
        return "SrScan".Translate();
    }

    private void OnClickTileSelect()
    {
        CameraJumper.TryJump(CameraJumper.GetWorldTarget(parent));
        Find.WorldSelector.ClearSelection();
        Find.WorldTargeter.BeginTargeting(OnWorldTargetSelected, true, ScanCursor, true,
            delegate { GenDraw.DrawWorldRadiusRing(parent.Tile, SingleModeRadius); }, targetingLabelGetter);
    }
}