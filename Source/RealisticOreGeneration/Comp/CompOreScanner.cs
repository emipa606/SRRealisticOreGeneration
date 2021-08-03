// ******************************************************************
//       /\ /|       @file       CompOreScanner.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-30 17:19:09
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [StaticConstructorOnStartup]
    public class CompOreScanner : CompScanner
    {
        private const int RangeModeRadius = 5;
        private const int SingleModeRadius = 15;

        private static readonly Texture2D SingleScanModeCommand =
            ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan");

        private static readonly Texture2D
            RangeScanModeCommand = ContentFinder<Texture2D>.Get("UI/Commands/AbandonHome");

        private static readonly Texture2D SurfaceScanModeCommand =
            ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan");

        private static readonly Texture2D UndergroundScanModeCommand =
            ContentFinder<Texture2D>.Get("UI/Commands/AbandonHome");

        private static readonly Texture2D TileSelectedCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan");

        private static readonly Texture2D TileUnselectedCommand =
            ContentFinder<Texture2D>.Get("UI/Commands/AbandonHome");

        private OreScanMode _oreScanMode = OreScanMode.RangeSurface;
        private Dictionary<int, int> _ringMap = new Dictionary<int, int>();
        private int _selectedTile = -1;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            GetRingMap(SingleModeRadius);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref _selectedTile, "_selectedTile");
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var baseGizmo in base.CompGetGizmosExtra()) yield return baseGizmo;
            yield return GetScanAreaGizmo();
            foreach (var scanModeGizmo in GetScanModeGizmo()) yield return scanModeGizmo;
        }

        protected override void DoFind(Pawn worker)
        {
            switch (_oreScanMode)
            {
                case OreScanMode.SingleUnderground:
                case OreScanMode.RangeUnderground:
                    OnUndergroundFind();
                    break;
                default:
                    OnSurfaceFind();
                    break;
            }

            if (Prefs.DevMode)
            {
                Log.Warning($"{MsicDef.LogTag}scanning complete: {_selectedTile}");
            }
        }

        public override void CompTickRare()
        {
            Log.Warning("tick");
            base.CompTickRare();
            //works well
            if (_selectedTile != -1) return;
            //no target
            UpdateDefaultTarget();
            //still no target
            if (_selectedTile == -1)
            {
                //disable
                var comp = parent.GetComp<CompFlickable>();
                if (comp == null)
                {
                    Log.Warning($"{MsicDef.LogTag}can't find comp");
                    return;
                }

                Messages.Message("SrNoTargetTile".Translate(parent.Label), MessageTypeDefOf.NeutralEvent);
                comp.SwitchIsOn = false;
                return;
            }

            UpdateCostTime();
        }

        private void UpdateDefaultTarget()
        {
            Log.Warning("SetDefaultTarget");
            switch (_oreScanMode)
            {
                case OreScanMode.SingleSurface:
                    UpdateDefaultTargetSingleSurface();
                    break;
                case OreScanMode.SingleUnderground:
                    UpdateDefaultTargetSingleUnderground();
                    break;
                case OreScanMode.RangeSurface:
                    UpdateDefaultTargetRangeSurface();
                    break;
                case OreScanMode.RangeUnderground:
                    UpdateDefaultTargetRangeUnderground();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateDefaultTargetSingleSurface()
        {
            var list = _ringMap.Keys.ToList();
            if (list.Count <= 0)
            {
                _selectedTile = -1;
                return;
            }

            list.Shuffle();
            if (!WorldOreInfoRecorder.Instance.IsTileScannedSurface(list[0]))
            {
                _selectedTile = list[0];
            }

            _selectedTile = -1;
        }

        private void UpdateDefaultTargetSingleUnderground()
        {
            var list = _ringMap.Keys.ToList();
            if (list.Count <= 0)
            {
                _selectedTile = -1;
                return;
            }

            list.Shuffle();
            if (!WorldOreInfoRecorder.Instance.IsTileScannedUnderground(list[0]))
            {
                _selectedTile = list[0];
            }

            _selectedTile = -1;
        }

        private void UpdateDefaultTargetRangeSurface()
        {
            foreach (var kvp in _ringMap.Where(kvp =>
                !WorldOreInfoRecorder.Instance.IsTileScannedSurface(kvp.Key) && kvp.Value <= RangeModeRadius))
            {
                _selectedTile = kvp.Key;
                return;
            }

            _selectedTile = -1;
        }

        private void UpdateDefaultTargetRangeUnderground()
        {
            foreach (var kvp in _ringMap.Where(kvp =>
                !WorldOreInfoRecorder.Instance.IsTileScannedUnderground(kvp.Key) && kvp.Value <= RangeModeRadius))
            {
                _selectedTile = kvp.Key;
                return;
            }

            _selectedTile = -1;
        }

        private void GetRingMap(int radius)
        {
            var worldGrid = Find.WorldGrid;
            if (worldGrid == null) throw new Exception($"{MsicDef.LogTag}can't find world grid");
            _ringMap = new Dictionary<int, int>(); // the result we want
            var currentRing = new List<int> {parent.Tile}; //to calc outer ring
            var outerRing = new List<int>(); //to calc which tile can be scanned
            var innerCircleSet = new HashSet<int> {parent.Tile}; //searched tiles
            var tempNeighborList = new List<int>();
            for (var i = 1; i <= radius; i++)
            {
                //calc outer ring
                outerRing.Clear();
                foreach (var tileId in currentRing)
                {
                    worldGrid.GetTileNeighbors(tileId, tempNeighborList);
                    foreach (var neighbor in tempNeighborList.Where(neighbor => !innerCircleSet.Contains(neighbor)))
                    {
                        outerRing.Add(neighbor);
                        innerCircleSet.Add(neighbor);
                    }
                }

                currentRing.Clear();
                currentRing.AddRange(outerRing);
                foreach (var tileId in outerRing) _ringMap.Add(tileId, i);
            }

            if (!Prefs.DevMode) return;
            Log.Message($"{MsicDef.LogTag}ring map:");
            foreach (var kvp in _ringMap) Log.Message($"{MsicDef.LogTag}k:{kvp.Key} v:{kvp.Value}");
        }

        /// <summary>
        ///     distance of target tile will affect cost
        /// </summary>
        private void UpdateCostTime()
        {
            //todo set time 
        }

        private int GetTargetDistance(int tileId)
        {
            if (_ringMap.ContainsKey(tileId)) return _ringMap[tileId];
            Log.Error($"{MsicDef.LogTag}can't find target tile in circle tileId: {tileId}");
            return 1;
        }

        private void OnSurfaceFind()
        {
            WorldOreInfoRecorder.Instance.RecordScannedTileSurface(_selectedTile);
            UpdateDefaultTarget();
        }

        private void OnUndergroundFind()
        {
            WorldOreInfoRecorder.Instance.RecordScannedTileUnderground(_selectedTile);
            UpdateDefaultTarget();
        }

        private IEnumerable<Gizmo> GetScanModeGizmo()
        {
            var commandChange = new Command_Action
            {
                defaultLabel = "SrScanModeChange".Translate(),
                icon = (_oreScanMode & OreScanMode.RangeSurface) == OreScanMode.RangeSurface
                    ? RangeScanModeCommand
                    : SingleScanModeCommand,
                action = OnClickScanModeChange
            };

            yield return commandChange;
            if ((_oreScanMode & OreScanMode.RangeSurface) == OreScanMode.RangeSurface) yield break;

            var commandSelectTile = new Command_Action
            {
                defaultLabel = $"{"SrSelect".Translate()}",
                icon = _selectedTile == -1 ? TileUnselectedCommand : TileSelectedCommand,
                action = OnClickTileSelect
            };

            yield return commandSelectTile;
        }

        private Gizmo GetScanAreaGizmo()
        {
            var commandAction = new Command_Action
            {
                defaultLabel = "SrScanAreaChange".Translate(),
                icon = (_oreScanMode & OreScanMode.SingleUnderground) == OreScanMode.SingleUnderground
                    ? UndergroundScanModeCommand
                    : SurfaceScanModeCommand,
                action = OnClickScanAreaChange
            };
            return commandAction;
        }

        private void OnClickScanModeChange()
        {
            _oreScanMode = (_oreScanMode & OreScanMode.RangeSurface) == OreScanMode.RangeSurface
                ? _oreScanMode & ~OreScanMode.RangeSurface
                : _oreScanMode | OreScanMode.RangeSurface;
            _selectedTile = -1;
        }

        private void OnClickScanAreaChange()
        {
            _oreScanMode = (_oreScanMode & OreScanMode.SingleUnderground) == OreScanMode.SingleUnderground
                ? _oreScanMode & ~OreScanMode.SingleUnderground
                : _oreScanMode | OreScanMode.SingleUnderground;
            _selectedTile = -1;
        }

        private void OnClickTileSelect()
        {
            //todo 
        }
    }
}