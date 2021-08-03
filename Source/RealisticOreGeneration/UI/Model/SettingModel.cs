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
        public int maxOutpostCount = 2;
        public bool needShuffleLumpSize;
        public bool disableScanner;
        public int outpostMapSize = 200;
        public float surfaceMultiplier = 1.5f;
        public float undergroundMultiplier = 1.5f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref surfaceMultiplier, "surfaceMultiplier", 1.5f);
            Scribe_Values.Look(ref undergroundMultiplier, "undergroundMultiplier", 1.5f);
            Scribe_Values.Look(ref outpostMapSize, "outpostMapSize", 250);
            Scribe_Values.Look(ref maxOutpostCount, "maxOutpostCount", 2);
            Scribe_Values.Look(ref needShuffleLumpSize, "needShuffleLumpSize");
            Scribe_Values.Look(ref disableScanner, "disableScanner");
        }

        public void SetDefault()
        {
            surfaceMultiplier = 1.5f;
            undergroundMultiplier = 1.5f;
            outpostMapSize = 200;
            maxOutpostCount = 2;
            needShuffleLumpSize = false;
            disableScanner = false;
        }
    }
}