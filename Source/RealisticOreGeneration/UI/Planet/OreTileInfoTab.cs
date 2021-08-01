// ******************************************************************
//       /\ /|       @file       OreTileInfoTab.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-29 18:27:50
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration.UI.Planet
{
    public class OreTileInfoTab : WITab
    {
        private Vector2 _scrollPosition;
        private float _scrollViewHeight;
        private static readonly Vector2 WinSize = new Vector2(440f, 540f);
        private const float VerticalMargin = 15f;
        private const float FrameMargin = 10f;
        private const float BarWidth = 16f;
        public override bool IsVisible => SelTileID >= 0;

        public OreTileInfoTab()
        {
            size = WinSize;
            labelKey = "SrTabOreTileInfo";
        }

        protected override void FillTab()
        {
            var winRect = new Rect(0, VerticalMargin, WinSize.x, WinSize.y - VerticalMargin).ContractedBy(FrameMargin);
            var viewRect = new Rect(0, 0, winRect.width - FrameMargin - BarWidth, _scrollViewHeight);
            var curY = 0f;
            Widgets.BeginScrollView(winRect, ref _scrollPosition, viewRect);
            var surfaceOreDistributionRect = new Rect
            {
                width = viewRect.width
            };
            DrawSurfaceOreDistribution(ref curY, surfaceOreDistributionRect);
            var undergroundOreDistributionRect = new Rect
            {
                y = curY, width = viewRect.width
            };
            DrawUndergroundOreDistribution(ref curY, undergroundOreDistributionRect);
            _scrollViewHeight = curY;
            Widgets.EndScrollView();
        }

        private static void DrawSurfaceOreDistribution(ref float curY, Rect rect)
        {
            GUI.color = Color.white;
            Text.Font = GameFont.Medium;
            rect.height = Text.LineHeight;
            Widgets.Label(rect, "SrSurfaceLumpDistribution".Translate());
            rect.y += rect.height;
            var selectedTile = Find.WorldSelector.selectedTile;
            var tileOreData = WorldOreInfoRecorder.Instance.GetTileOreData(selectedTile);
            if (tileOreData == null)
            {
                Log.Warning($"{MsicDef.LogTag}can't find ore info in tile: {selectedTile}");
                return;
            }

            GUI.color = MsicDef.BilibiliPink;
            Text.Font = GameFont.Small;
            rect.height = Text.LineHeight;
            foreach (var kvp in tileOreData.surfaceDistrubtion)
            {
                var rawOreDef = ThingDef.Named(kvp.Key);
                if (rawOreDef == null)
                {
                    Log.Error($"{MsicDef.LogTag}can't find rawOreDef with defName: {kvp.Key}");
                    return;
                }

                Widgets.Label(rect, rawOreDef.label);
                rect.y += rect.height;
                Widgets.FillableBar(rect, kvp.Value, MsicDef.BilibiliBlueTex);
                rect.y += rect.height;
            }

            curY += rect.y;
        }

        private static void DrawUndergroundOreDistribution(ref float curY, Rect rect)
        {
            GUI.color = Color.white;
            Text.Font = GameFont.Medium;
            rect.height = Text.LineHeight;
            Widgets.Label(rect, "SrUndergroundLumpDistribution".Translate());
            rect.y += rect.height;
            var selectedTile = Find.WorldSelector.selectedTile;
            var tileOreData = WorldOreInfoRecorder.Instance.GetTileOreData(selectedTile);
            if (tileOreData == null)
            {
                Log.Warning($"{MsicDef.LogTag}can't find ore info in tile: {selectedTile}");
                return;
            }

            GUI.color = MsicDef.BilibiliBlue;
            Text.Font = GameFont.Small;
            rect.height = Text.LineHeight;
            foreach (var kvp in tileOreData.undergroundDistrubtion)
            {
                var rawOreDef = ThingDef.Named(kvp.Key);
                if (rawOreDef == null)
                {
                    Log.Error($"{MsicDef.LogTag}can't find rawOreDef with defName: {kvp.Key}");
                    return;
                }

                Widgets.Label(rect, rawOreDef.label);
                rect.y += rect.height;
                Widgets.FillableBar(rect, kvp.Value, MsicDef.BilibiliPinkTex);
                rect.y += rect.height;
            }

            curY += rect.y;
        }
    }
}