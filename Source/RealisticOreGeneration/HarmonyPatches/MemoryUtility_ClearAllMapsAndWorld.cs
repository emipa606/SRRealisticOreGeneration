using HarmonyLib;
using RabiSquare.RealisticOreGeneration;
using Verse.Profile;

namespace RealisticOreGeneration.HarmonyPatches;

[HarmonyPatch(typeof(MemoryUtility), nameof(MemoryUtility.ClearAllMapsAndWorld))]
public static class MemoryUtility_ClearAllMapsAndWorld
{
    public static void Postfix()
    {
        BaseSingleTon<WorldOreInfoRecorder>.Instance.Clear();
        BaseSingleTon<MiningOutpostRecorder>.Instance.Clear();
    }
}