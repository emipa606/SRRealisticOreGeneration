using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration.UI.Planet;

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
        var outRect = new Rect(0f, VerticalMargin, WinSize.x, WinSize.y - VerticalMargin).ContractedBy(FrameMargin);
        var viewRect = new Rect(0f, 0f, outRect.width - FrameMargin - BarWidth, _scrollViewHeight);
        var curY = 0f;
        Widgets.BeginScrollView(outRect, ref _scrollPosition, viewRect);
        DrawWarning(ref curY, viewRect.width);
        DrawSurfaceInfo(ref curY, viewRect.width);
        DrawUndergroundInfo(ref curY, viewRect.width);
        _scrollViewHeight = curY;
        Widgets.EndScrollView();
    }

    private void DrawWarning(ref float curY, float width)
    {
        if (!BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileAbandoned(SelTileID))
        {
            return;
        }

        var rect = default(Rect);
        rect.width = width;
        rect.y = curY;
        var rect2 = rect;
        GUI.color = Color.red;
        Text.Font = GameFont.Medium;
        rect2.height = Text.LineHeight;
        Widgets.Label(rect2, "SrResourceDepletion".Translate());
        rect2.y += rect2.height;
        curY = rect2.y;
        Text.Font = GameFont.Small;
        rect2.height = Text.LineHeight;
        Widgets.Label(rect2, "SrResourceDepletionDesc".Translate());
        rect2.y += rect2.height;
        curY = rect2.y;
    }

    private static void DrawInfo(ref float curY, float width, string text)
    {
        var rect = default(Rect);
        rect.width = width;
        rect.y = curY;
        var rect2 = rect;
        GUI.color = Color.white;
        Text.Font = GameFont.Medium;
        rect2.height = Text.LineHeight;
        Widgets.Label(rect2, text);
        rect2.y += rect2.height;
        curY = rect2.y;
    }

    private void DrawSurfaceInfo(ref float curY, float width)
    {
        if (!BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileScannedSurface(SelTileID))
        {
            DrawInfo(ref curY, width, "SrNoSurfaceInfo".Translate());
            return;
        }

        DrawSurfaceAbundance(ref curY, width);
        DrawSurfaceOreDistribution(ref curY, width);
    }

    private void DrawUndergroundInfo(ref float curY, float width)
    {
        if (!BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileScannedUnderground(SelTileID))
        {
            DrawInfo(ref curY, width, "SrNoUndergroundInfo".Translate());
            return;
        }

        DrawUndergroundAbundance(ref curY, width);
        DrawUndergroundOreDistribution(ref curY, width);
    }

    private void DrawSurfaceAbundance(ref float curY, float width)
    {
        var tileOreData = BaseSingleTon<WorldOreDataGenerator>.Instance.GetTileOreData(SelTileID);
        var rect = default(Rect);
        rect.width = width;
        rect.y = curY;
        var rect2 = rect;
        GUI.color = Color.white;
        Text.Font = GameFont.Medium;
        rect2.height = Text.LineHeight;
        Widgets.Label(rect2, "SrSurfaceAbundance".Translate());
        rect2.y += rect2.height;
        GUI.color = Color.green;
        Widgets.FillableBar(rect2, tileOreData.GetSurfaceAbundance(), Texture2D.whiteTexture);
        rect2.y += rect2.height;
        curY = rect2.y;
    }

    private void DrawSurfaceOreDistribution(ref float curY, float width)
    {
        var tileOreData = BaseSingleTon<WorldOreDataGenerator>.Instance.GetTileOreData(SelTileID);
        var rect = default(Rect);
        rect.width = width;
        rect.y = curY;
        var rect2 = rect;
        GUI.color = Color.white;
        Text.Font = GameFont.Medium;
        rect2.height = Text.LineHeight;
        Widgets.Label(rect2, "SrSurfaceLumpDistribution".Translate());
        rect2.y += rect2.height;
        Text.Font = GameFont.Small;
        rect2.height = Text.LineHeight;
        foreach (var item in tileOreData.surfaceDistribution)
        {
            var thingDef = ThingDef.Named(item.Key);
            if (thingDef == null)
            {
                Log.Error($"[RabiSquare.RealisticOreGeneration]can't find rawOreDef with defName: {item.Key}");
                return;
            }

            GUI.color = MsicDef.BilibiliPink;
            Widgets.Label(rect2, thingDef.label);
            rect2.y += rect2.height;
            GUI.color = MsicDef.BilibiliBlue;
            Widgets.FillableBar(rect2, item.Value, Texture2D.whiteTexture);
            rect2.y += rect2.height;
        }

        curY = rect2.y;
    }

    private void DrawUndergroundAbundance(ref float curY, float width)
    {
        var tileOreData = BaseSingleTon<WorldOreDataGenerator>.Instance.GetTileOreData(SelTileID);
        var rect = default(Rect);
        rect.width = width;
        rect.y = curY;
        var rect2 = rect;
        GUI.color = Color.white;
        Text.Font = GameFont.Medium;
        rect2.height = Text.LineHeight;
        Widgets.Label(rect2, "SrUndergroundAbundance".Translate());
        rect2.y += rect2.height;
        GUI.color = Color.green;
        Widgets.FillableBar(rect2, tileOreData.UndergroundBerlinFactor, Texture2D.whiteTexture);
        rect2.y += rect2.height;
        curY = rect2.y;
    }

    private void DrawUndergroundOreDistribution(ref float curY, float width)
    {
        var tileOreData = BaseSingleTon<WorldOreDataGenerator>.Instance.GetTileOreData(SelTileID);
        var rect = default(Rect);
        rect.width = width;
        rect.y = curY;
        var rect2 = rect;
        GUI.color = Color.white;
        Text.Font = GameFont.Medium;
        rect2.height = Text.LineHeight;
        Widgets.Label(rect2, "SrUndergroundLumpDistribution".Translate());
        rect2.y += rect2.height;
        Text.Font = GameFont.Small;
        rect2.height = Text.LineHeight;
        foreach (var item in tileOreData.undergroundDistribution)
        {
            var thingDef = ThingDef.Named(item.Key);
            if (thingDef == null)
            {
                Log.Error($"[RabiSquare.RealisticOreGeneration]can't find rawOreDef with defName: {item.Key}");
                return;
            }

            GUI.color = MsicDef.BilibiliBlue;
            Widgets.Label(rect2, thingDef.label);
            rect2.y += rect2.height;
            GUI.color = MsicDef.BilibiliPink;
            Widgets.FillableBar(rect2, item.Value, Texture2D.whiteTexture);
            rect2.y += rect2.height;
        }

        curY = rect2.y;
    }
}