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
    public class OreTileInfoTab : WITab
    {
        private static readonly Vector2 WinSize = new Vector2(400f, 150f);
        public override bool IsVisible => SelTileID >= 0;
        public OreTileInfoTab()
        {
            size = WinSize;
            labelKey = "SrTabOreTileInfo";
        }

        protected override void FillTab()
        {
            Log.Error("DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD");
            var outRect = new Rect(0.0f, 0.0f, WinSize.x, WinSize.y).ContractedBy(10f);
            var ls = new Listing_Standard();
            ls.Begin(outRect);
            if (ls.ButtonText("Default"))
            {
                SettingWindow.Instance.settingModel.SetDefault();
            }

            Text.Font = GameFont.Medium;
            ls.GapLine(20f);
            ls.Label($"{"SrSurfaceMutilpier".Translate()}: {SettingWindow.Instance.settingModel.surfaceMutilpier}");
            SettingWindow.Instance.settingModel.surfaceMutilpier =
                ls.Slider(SettingWindow.Instance.settingModel.surfaceMutilpier, 1f, 99f);
            ls.GapLine(20f);
            ls.Label($"{"SrUndergroundMutilpier".Translate()}: {SettingWindow.Instance.settingModel.undergroundMutilpier}");
            SettingWindow.Instance.settingModel.undergroundMutilpier =
                ls.Slider(SettingWindow.Instance.settingModel.undergroundMutilpier, 1f, 99f);
            ls.GapLine(20f);
            ls.Label($"{"SrOutpostMapSize".Translate()}: {SettingWindow.Instance.settingModel.outpostMapSize}");
            SettingWindow.Instance.settingModel.outpostMapSize =
                Mathf.RoundToInt(ls.Slider(SettingWindow.Instance.settingModel.outpostMapSize, 100, 300));
            ls.GapLine(20f);
            ls.Label($"{"SrMaxOutpostCount".Translate()}: {SettingWindow.Instance.settingModel.maxOutpostCount}");
            SettingWindow.Instance.settingModel.maxOutpostCount =
                Mathf.RoundToInt(ls.Slider(SettingWindow.Instance.settingModel.maxOutpostCount, 1, 10));
            ls.End();
        }
    }
}
