// ******************************************************************
//       /\ /|       @file       SettingWindow.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-29 17:42:35
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    [StaticConstructorOnStartup]
    public class SettingWindow : Mod
    {
        public readonly SettingModel settingModel;

        public SettingWindow(ModContentPack content) : base(content)
        {
            settingModel = GetSettings<SettingModel>();
            Instance = this;
        }

        public static SettingWindow Instance { get; private set; }

        public override string SettingsCategory()
        {
            return "RealisticOreGeneration";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var ls = new Listing_Standard();
            ls.Begin(inRect);
            if (ls.ButtonText("Default")) settingModel.SetDefault();
            Text.Font = GameFont.Medium;
            ls.CheckboxLabeled("SrShuffleLumpSize".Translate(), ref settingModel.needShuffleLumpSize,
                "SrDescriptionShuffleLumpSize".Translate());
            ls.GapLine(20f);
            ls.CheckboxLabeled("SrDisableScanner".Translate(), ref settingModel.disableScanner,
                "SrDescriptionDisableScanner".Translate());
            ls.GapLine(20f);
            ls.Label($"{"SrVanillaSimilarityPercentage".Translate()}: {settingModel.vanillaPercent.ToStringPercent()}",
                tooltip: "SrVanillaSimilarityPercentageDesc".Translate());
            settingModel.vanillaPercent = ls.Slider(settingModel.vanillaPercent, 0f, 1f);
            ls.GapLine(20f);
            ls.Label($"{"SrSurfaceMultiplier".Translate()}: {settingModel.surfaceMultiplier}",
                tooltip: "SrDescriptionSurfaceMultiplier".Translate());
            settingModel.surfaceMultiplier = ls.Slider(settingModel.surfaceMultiplier, 1f, 10f);
            ls.GapLine(20f);
            ls.Label($"{"SrUndergroundMultiplier".Translate()}: {settingModel.undergroundMultiplier}",
                tooltip: "SrDescriptionUndergroundMultiplier".Translate());
            settingModel.undergroundMultiplier = ls.Slider(settingModel.undergroundMultiplier, 1f, 10f);
            ls.GapLine(20f);
            ls.Label($"{"SrOutpostMapSize".Translate()}: {settingModel.outpostMapSize}",
                tooltip: "SrDescriptionOutpostMapSize".Translate());
            settingModel.outpostMapSize = Mathf.RoundToInt(ls.Slider(settingModel.outpostMapSize, 100, 300));
            ls.GapLine(20f);
            ls.Label($"{"SrMaxOutpostCount".Translate()}: {settingModel.maxOutpostCount}",
                tooltip: "SrDescriptionMaxOutpostCount".Translate());
            settingModel.maxOutpostCount = Mathf.RoundToInt(ls.Slider(settingModel.maxOutpostCount, 1, 10));
            if (Prefs.DevMode)
            {
                ls.GapLine(20f);
                ls.Label($"sigmaSeed: {settingModel.sigmaSeed}", tooltip: "sigma seed");
                settingModel.sigmaSeed = ls.Slider(settingModel.sigmaSeed, 0.1f, 10f);
            }

            ls.End();
        }
    }
}