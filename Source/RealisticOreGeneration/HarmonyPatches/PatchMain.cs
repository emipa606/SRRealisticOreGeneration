using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace RealisticOreGeneration.HarmonyPatches;

[UsedImplicitly]
[StaticConstructorOnStartup]
public class PatchMain
{
    static PatchMain()
    {
        new Harmony("[RabiSquare.RealisticOreGeneration]").PatchAll(Assembly.GetExecutingAssembly());
    }
}