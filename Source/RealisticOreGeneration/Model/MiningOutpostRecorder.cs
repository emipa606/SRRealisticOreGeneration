// ******************************************************************
//       /\ /|       @file       MiningOutpostRecorder.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-29 21:18:34
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    public class MiningOutpostRecorder : BaseSingleTon<MiningOutpostRecorder>, IExposable
    {
        private int _miningOutpostCount;

        public void ExposeData()
        {
            Scribe_Values.Look(ref _miningOutpostCount, "_miningOutpostCount");
        }

        public void MiningOutpostCountIncrease()
        {
            _miningOutpostCount++;
        }

        public void MiningOutpostCountDecrease()
        {
            _miningOutpostCount--;
        }

        public int GetOutpostCount()
        {
            return _miningOutpostCount;
        }
    }
}