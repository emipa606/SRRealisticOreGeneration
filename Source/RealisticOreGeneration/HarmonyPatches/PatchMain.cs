using System.Reflection;
using HarmonyLib;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[StaticConstructorOnStartup]
public static class PatchMain
{
    static PatchMain()
    {
        new Harmony("[RabiSquare.RealisticOreGeneration]").PatchAll(Assembly.GetExecutingAssembly());
    }
}