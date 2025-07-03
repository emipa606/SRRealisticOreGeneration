using HarmonyLib;
using RabiSquare.RealisticOreGeneration;
using RimWorld.Planet;

namespace RealisticOreGeneration.HarmonyPatches;

[HarmonyPatch(typeof(World), nameof(World.ExposeData))]
public class World_ExposeData
{
    public static void Prefix()
    {
        BaseSingleTon<WorldOreInfoRecorder>.Instance.ExposeData();
        BaseSingleTon<MiningOutpostRecorder>.Instance.ExposeData();
    }
}