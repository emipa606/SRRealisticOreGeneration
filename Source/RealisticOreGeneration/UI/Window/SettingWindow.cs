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
        public static SettingWindow Instance { get; private set; }

        public SettingWindow(ModContentPack content) : base(content)
        {
            settingModel = GetSettings<SettingModel>();
            Instance = this;
        }

        public override string SettingsCategory()
        {
            return "RealisticOreGeneration";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var ls = new Listing_Standard();
            ls.Begin(inRect);
            if (ls.ButtonText("Default"))
            {
                settingModel.SetDefault();
            }

            ls.CheckboxLabeled("SrShuffleLumpSize".Translate(), ref settingModel.needShuffleLumpSize,
                "SrDescriptionShuffleLumpSize");
            Text.Font = GameFont.Medium;
            ls.GapLine(20f);
            ls.Label($"{"SrSurfaceMutilpier".Translate()}: {settingModel.surfaceMutilpier}",
                tooltip: "SrDescriptionSurfaceMutilpier".Translate());
            settingModel.surfaceMutilpier = ls.Slider(settingModel.surfaceMutilpier, 1f, 99f);
            ls.GapLine(20f);
            ls.Label($"{"SrUndergroundMutilpier".Translate()}: {settingModel.undergroundMutilpier}",
                tooltip: "SrDescriptionUndergroundMutilpier".Translate());
            settingModel.undergroundMutilpier = ls.Slider(settingModel.undergroundMutilpier, 1f, 99f);
            ls.GapLine(20f);
            ls.Label($"{"SrOutpostMapSize".Translate()}: {settingModel.outpostMapSize}",
                tooltip: "SrDescriptionOutpostMapSize".Translate());
            settingModel.outpostMapSize = Mathf.RoundToInt(ls.Slider(settingModel.outpostMapSize, 100, 300));
            ls.GapLine(20f);
            ls.Label($"{"SrMaxOutpostCount".Translate()}: {settingModel.maxOutpostCount}",
                tooltip: "SrDescriptionMaxOutpostCount".Translate());
            settingModel.maxOutpostCount = Mathf.RoundToInt(ls.Slider(settingModel.maxOutpostCount, 1, 10));
            ls.End();
        }
    }
}