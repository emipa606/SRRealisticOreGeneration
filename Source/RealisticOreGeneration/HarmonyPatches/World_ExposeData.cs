using HarmonyLib;
using JetBrains.Annotations;
using RabiSquare.RealisticOreGeneration;
using RimWorld.Planet;

namespace RealisticOreGeneration.HarmonyPatches;

[UsedImplicitly]
[HarmonyPatch(typeof(World), nameof(World.ExposeData))]
public class World_ExposeData
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static bool Prefix()
    {
        BaseSingleTon<WorldOreInfoRecorder>.Instance.ExposeData();
        BaseSingleTon<MiningOutpostRecorder>.Instance.ExposeData();
        return true;
    }
}