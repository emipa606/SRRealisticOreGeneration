// ******************************************************************
//       /\ /|       @file       SettingModel.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-29 17:37:16
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    public class SettingModel : ModSettings
    {
        public bool needShuffleLumpSize;
        public bool disableScanner;
        public float vanillaPercent = 0.5f;
        public float surfaceMultiplier = 1.5f;
        public float undergroundMultiplier = 1.5f;
        public int outpostMapSize = 200;
        public int maxOutpostCount = 2;
        public float sigmaSeed = 1f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref needShuffleLumpSize, "needShuffleLumpSize");
            Scribe_Values.Look(ref disableScanner, "disableScanner");
            Scribe_Values.Look(ref vanillaPercent, "vanillaPercent", 0.5f);
            Scribe_Values.Look(ref surfaceMultiplier, "surfaceMultiplier", 1.5f);
            Scribe_Values.Look(ref undergroundMultiplier, "undergroundMultiplier", 1.5f);
            Scribe_Values.Look(ref outpostMapSize, "outpostMapSize", 250);
            Scribe_Values.Look(ref maxOutpostCount, "maxOutpostCount", 2);
            Scribe_Values.Look(ref sigmaSeed, "sigmaSeed", 1f);
        }

        public void SetDefault()
        {
            needShuffleLumpSize = false;
            disableScanner = false;
            vanillaPercent = 0.5f;
            surfaceMultiplier = 1.5f;
            undergroundMultiplier = 1.5f;
            outpostMapSize = 200;
            maxOutpostCount = 2;
            sigmaSeed = 1f;
        }
    }
}