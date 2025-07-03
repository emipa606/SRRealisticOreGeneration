using System.Reflection;
using HarmonyLib;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[StaticConstructorOnStartup]
public class PatchMain
{
    static PatchMain()
    {
        new Harmony("[RabiSquare.RealisticOreGeneration]").PatchAll(Assembly.GetExecutingAssembly());
    }
}