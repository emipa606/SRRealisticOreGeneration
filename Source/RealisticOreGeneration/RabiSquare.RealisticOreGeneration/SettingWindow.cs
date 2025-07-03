using JetBrains.Annotations;
using Mlie;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

[UsedImplicitly]
[StaticConstructorOnStartup]
public class SettingWindow : Mod
{
    private static string currentVersion;
    public readonly SettingModel settingModel;

    public SettingWindow(ModContentPack content)
        : base(content)
    {
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
        settingModel = GetSettings<SettingModel>();
        Instance = this;
    }

    public static SettingWindow Instance { get; private set; }

    public override string SettingsCategory()
    {
        return "Realistic Ore Generation";
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(inRect);
        Text.Font = GameFont.Small;
        listing_Standard.CheckboxLabeled("SrShuffleLumpSize".Translate(), ref settingModel.needShuffleLumpSize,
            "SrDescriptionShuffleLumpSize".Translate());
        listing_Standard.GapLine(15f);
        listing_Standard.CheckboxLabeled("SrDisableScanner".Translate(), ref settingModel.disableScanner,
            "SrDescriptionDisableScanner".Translate());
        listing_Standard.GapLine(15f);
        listing_Standard.Label(
            (TaggedString)
            $"{"SrVanillaSimilarityPercentage".Translate()}: {settingModel.vanillaPercent.ToStringPercent()}", -1f,
            "SrVanillaSimilarityPercentageDesc".Translate());
        settingModel.vanillaPercent = listing_Standard.Slider(settingModel.vanillaPercent, 0f, 1f);
        listing_Standard.GapLine(15f);
        listing_Standard.Label((TaggedString)$"{"SrSurfaceMultiplier".Translate()}: {settingModel.surfaceMultiplier}",
            -1f,
            "SrDescriptionSurfaceMultiplier".Translate());
        settingModel.surfaceMultiplier = listing_Standard.Slider(settingModel.surfaceMultiplier, 0f, 10f);
        listing_Standard.GapLine(15f);
        listing_Standard.Label(
            (TaggedString)$"{"SrUndergroundMultiplier".Translate()}: {settingModel.undergroundMultiplier}", -1f,
            "SrDescriptionUndergroundMultiplier".Translate());
        settingModel.undergroundMultiplier = listing_Standard.Slider(settingModel.undergroundMultiplier, 0f, 10f);
        listing_Standard.GapLine(15f);
        listing_Standard.Label((TaggedString)$"{"SrOutpostMapSize".Translate()}: {settingModel.outpostMapSize}", -1f,
            "SrDescriptionOutpostMapSize".Translate());
        settingModel.outpostMapSize =
            Mathf.RoundToInt(listing_Standard.Slider(settingModel.outpostMapSize, 100f, 300f));
        listing_Standard.GapLine(15f);
        listing_Standard.Label((TaggedString)$"{"SrMaxOutpostCount".Translate()}: {settingModel.maxOutpostCount}", -1f,
            "SrDescriptionMaxOutpostCount".Translate());
        settingModel.maxOutpostCount = Mathf.RoundToInt(listing_Standard.Slider(settingModel.maxOutpostCount, 1f, 10f));
        if (Prefs.DevMode)
        {
            listing_Standard.GapLine(15f);
            listing_Standard.Label((TaggedString)$"qMin: {settingModel.qMin}", -1f, "qMin");
            settingModel.qMin = listing_Standard.Slider(settingModel.qMin, 0.1f, 10f);
            listing_Standard.GapLine(15f);
            listing_Standard.Label((TaggedString)$"qMax: {settingModel.qMax}", -1f, "qMax");
            settingModel.qMax = listing_Standard.Slider(settingModel.qMax, 0.1f, 10f);
        }

        if (listing_Standard.ButtonText("Default"))
        {
            settingModel.SetDefault();
        }

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("SrCurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }
}