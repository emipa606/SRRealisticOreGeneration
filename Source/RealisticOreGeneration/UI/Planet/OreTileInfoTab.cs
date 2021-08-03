// ******************************************************************
//       /\ /|       @file       OreTileInfoTab.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-29 18:27:50
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration.UI.Planet
{
    [StaticConstructorOnStartup]
    public class OreTileInfoTab : WITab
    {
        private const float VerticalMargin = 15f;
        private const float FrameMargin = 10f;
        private const float BarWidth = 16f;
        private static readonly Vector2 WinSize = new Vector2(440f, 540f);
        private Vector2 _scrollPosition;
        private float _scrollViewHeight;

        public OreTileInfoTab()
        {
            size = WinSize;
            labelKey = "SrTabOreTileInfo";
        }

        public override bool IsVisible => SelTileID >= 0;

        protected override void FillTab()
        {
            var winRect = new Rect(0, VerticalMargin, WinSize.x, WinSize.y - VerticalMargin).ContractedBy(FrameMargin);
            var viewRect = new Rect(0, 0, winRect.width - FrameMargin - BarWidth, _scrollViewHeight);
            var curY = 0f;
            Widgets.BeginScrollView(winRect, ref _scrollPosition, viewRect);
            DrawWarning(ref curY, viewRect.width);
            DrawSurfaceInfo(ref curY, viewRect.width);
            DrawUndergroundInfo(ref curY, viewRect.width);
            _scrollViewHeight = curY;
            Widgets.EndScrollView();
        }

        private void DrawWarning(ref float curY, float width)
        {
            if (!WorldOreInfoRecorder.Instance.IsTileAbandoned(SelTileID)) return;
            var rect = new Rect {width = width, y = curY};
            //label
            GUI.color = Color.red;
            Text.Font = GameFont.Medium;
            rect.height = Text.LineHeight;
            Widgets.Label(rect, "SrResourceDepletion".Translate());
            rect.y += rect.height;
            curY = rect.y;
        }

        private static void DrawInfo(ref float curY, float width, string text)
        {
            var rect = new Rect {width = width, y = curY};
            //label
            GUI.color = Color.white;
            Text.Font = GameFont.Medium;
            rect.height = Text.LineHeight;
            Widgets.Label(rect, text);
            rect.y += rect.height;
            curY = rect.y;
        }

        private void DrawSurfaceInfo(ref float curY, float width)
        {
            if (!WorldOreInfoRecorder.Instance.IsTileScannedSurface(SelTileID))
            {
                DrawInfo(ref curY, width, "SrNoSurfaceInfo".Translate());
                return;
            }

            DrawSurfaceAbundance(ref curY, width);
            DrawSurfaceOreDistribution(ref curY, width);
        }

        private void DrawUndergroundInfo(ref float curY, float width)
        {
            if (!WorldOreInfoRecorder.Instance.IsTileScannedUnderground(SelTileID))
            {
                DrawInfo(ref curY, width, "SrNoUndergroundInfo".Translate());
                return;
            }

            DrawUndergroundAbundance(ref curY, width);
            DrawUndergroundOreDistribution(ref curY, width);
        }

        private void DrawSurfaceAbundance(ref float curY, float width)
        {
            var tileOreData = WorldOreDataGenerator.Instance.GetTileOreData(SelTileID);
            var rect = new Rect {width = width, y = curY};
            //label
            GUI.color = Color.white;
            Text.Font = GameFont.Medium;
            rect.height = Text.LineHeight;
            Widgets.Label(rect, "SrSurfaceAbundance".Translate());
            rect.y += rect.height;
            //bar
            GUI.color = Color.green;
            Widgets.FillableBar(rect, tileOreData.GetSurfaceAbundance(), Texture2D.whiteTexture);
            rect.y += rect.height;
            curY = rect.y;
        }

        private void DrawSurfaceOreDistribution(ref float curY, float width)
        {
            var tileOreData = WorldOreDataGenerator.Instance.GetTileOreData(SelTileID);
            var rect = new Rect {width = width, y = curY};
            //label
            GUI.color = Color.white;
            Text.Font = GameFont.Medium;
            rect.height = Text.LineHeight;
            Widgets.Label(rect, "SrSurfaceLumpDistribution".Translate());
            rect.y += rect.height;
            //lump progress
            Text.Font = GameFont.Small;
            rect.height = Text.LineHeight;
            foreach (var kvp in tileOreData.surfaceDistribution)
            {
                var rawOreDef = ThingDef.Named(kvp.Key);
                if (rawOreDef == null)
                {
                    Log.Error($"{MsicDef.LogTag}can't find rawOreDef with defName: {kvp.Key}");
                    return;
                }

                GUI.color = MsicDef.BilibiliPink;
                Widgets.Label(rect, rawOreDef.label);
                rect.y += rect.height;
                GUI.color = MsicDef.BilibiliBlue;
                Widgets.FillableBar(rect, kvp.Value, Texture2D.whiteTexture);
                rect.y += rect.height;
            }

            curY = rect.y;
        }

        private void DrawUndergroundAbundance(ref float curY, float width)
        {
            var tileOreData = WorldOreDataGenerator.Instance.GetTileOreData(SelTileID);
            var rect = new Rect {width = width, y = curY};
            //label
            GUI.color = Color.white;
            Text.Font = GameFont.Medium;
            rect.height = Text.LineHeight;
            Widgets.Label(rect, "SrUndergroundAbundance".Translate());
            rect.y += rect.height;
            //bar
            GUI.color = Color.green;
            Widgets.FillableBar(rect, tileOreData.UndergroundBerlinFactor, Texture2D.whiteTexture);
            rect.y += rect.height;
            curY = rect.y;
        }

        private void DrawUndergroundOreDistribution(ref float curY, float width)
        {
            var tileOreData = WorldOreDataGenerator.Instance.GetTileOreData(SelTileID);
            var rect = new Rect {width = width, y = curY};
            //label
            GUI.color = Color.white;
            Text.Font = GameFont.Medium;
            rect.height = Text.LineHeight;
            Widgets.Label(rect, "SrUndergroundLumpDistribution".Translate());
            rect.y += rect.height;
            //ore lump progress
            Text.Font = GameFont.Small;
            rect.height = Text.LineHeight;
            foreach (var kvp in tileOreData.undergroundDistribution)
            {
                var rawOreDef = ThingDef.Named(kvp.Key);
                if (rawOreDef == null)
                {
                    Log.Error($"{MsicDef.LogTag}can't find rawOreDef with defName: {kvp.Key}");
                    return;
                }

                GUI.color = MsicDef.BilibiliBlue;
                Widgets.Label(rect, rawOreDef.label);
                rect.y += rect.height;
                GUI.color = MsicDef.BilibiliPink;
                Widgets.FillableBar(rect, kvp.Value, Texture2D.whiteTexture);
                rect.y += rect.height;
            }

            curY = rect.y;
        }
    }
}