// ******************************************************************
//       /\ /|       @file       CompOreScanner.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-30 17:19:09
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [StaticConstructorOnStartup]
    public class CompOreScanner : CompScanner
    {
        private static readonly Texture2D SingleScanModeCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan");
        private static readonly Texture2D RangeScanModeCommand = ContentFinder<Texture2D>.Get("UI/Commands/AbandonHome");
        private static readonly Texture2D SurfaceScanModeCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan");
        private static readonly Texture2D UndergroundScanModeCommand = ContentFinder<Texture2D>.Get("UI/Commands/AbandonHome");
        private static readonly Texture2D TileSelectedCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan");
        private static readonly Texture2D TileUnselectedCommand = ContentFinder<Texture2D>.Get("UI/Commands/AbandonHome");

        private OreScanMode _oreScanMode = OreScanMode.RangeSurface;
        private int selectedTile = -1;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var baseGizmo in base.CompGetGizmosExtra())
            {
                yield return baseGizmo;
            }
            yield return GetScanAreaGizmo();
            foreach (var scanModeGizmo in GetScanModeGizmo())
            {
                yield return scanModeGizmo;
            }
        }

        protected override void DoFind(Pawn worker)
        {
            var a = new List<CompOreScanner>();

            switch (_oreScanMode)
            {
                case OreScanMode.SingleSurface:
                    OnSingleSurfaceFind();
                    break;
                case OreScanMode.SingleUnderground:
                    OnSingleUndergroundFind();
                    break;
                case OreScanMode.RangeUnderground:
                    OnRangeUnderGroundFind();
                    break;
                default:
                    OnRangeSurfaceFind();
                    break;
            }

            //todo single
            //todo range 
            Find.LetterStack.ReceiveLetter("TEST", "test", LetterDefOf.PositiveEvent, parent);
        }

        private void OnSingleSurfaceFind()
        {
        }

        private void OnRangeSurfaceFind()
        {
        }

        private void OnSingleUndergroundFind()
        {
        }

        private void OnRangeUnderGroundFind()
        {
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
            if ((_oreScanMode & OreScanMode.RangeSurface) == OreScanMode.RangeSurface)
            {
                yield break;
            }

            var commandSelectTile = new Command_Action
            {
                defaultLabel = selectedTile == -1 ? "SrUnselect".Translate() : "SrSelect".Translate(),
                icon = selectedTile == -1 ? TileUnselectedCommand : TileSelectedCommand,
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
        }

        private void OnClickScanAreaChange()
        {
            _oreScanMode = (_oreScanMode & OreScanMode.SingleUnderground) == OreScanMode.SingleUnderground
                ? _oreScanMode & ~OreScanMode.SingleUnderground
                : _oreScanMode | OreScanMode.SingleUnderground;
        }

        private void OnClickTileSelect()
        {
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref selectedTile, "selectedTile");
        }
    }
}
