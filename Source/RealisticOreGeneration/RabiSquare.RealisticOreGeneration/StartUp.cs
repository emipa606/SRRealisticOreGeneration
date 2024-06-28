using System.Linq;
using JetBrains.Annotations;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

[StaticConstructorOnStartup]
[UsedImplicitly]
public static class StartUp
{
    static StartUp()
    {
        var surfaceOreDataList =
            DefDatabase<ThingDef>.AllDefs.Where(t => t.building is { mineableScatterCommonality: > 0f });
        BaseSingleTon<VanillaOreInfoRecorder>.Instance.SetSurfaceOreDataList(surfaceOreDataList);
        var undergroundOreDataList = DefDatabase<ThingDef>.AllDefs.Where(t => t.deepCommonality > 0f);
        BaseSingleTon<VanillaOreInfoRecorder>.Instance.SetUndergroundOreDataList(undergroundOreDataList);
        if (Prefs.DevMode)
        {
            Log.Message($"{BaseSingleTon<VanillaOreInfoRecorder>.Instance}");
        }
    }
}