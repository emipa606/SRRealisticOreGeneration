using HarmonyLib;
using JetBrains.Annotations;
using RabiSquare.RealisticOreGeneration;
using Verse.Profile;

namespace RealisticOreGeneration.HarmonyPatches;

[UsedImplicitly]
[HarmonyPatch(typeof(MemoryUtility), nameof(MemoryUtility.ClearAllMapsAndWorld))]
public class MemoryUtility_ClearAllMapsAndWorld
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Postfix()
    {
        BaseSingleTon<WorldOreInfoRecorder>.Instance.Clear();
        BaseSingleTon<MiningOutpostRecorder>.Instance.Clear();
    }
}