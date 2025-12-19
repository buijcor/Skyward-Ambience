using HarmonyLib;
using RimWorld;
using Verse;

namespace SkywardAmbience
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("SkywardAmbience.Mod");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(Map), "FinalizeInit")]
    public static class Map_FinalizeInit_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Map __instance)
        {
            // Ensure our MapComponent exists on the map
            if (__instance.GetComponent<MapComponent_SkyAmbience>() == null)
            {
                __instance.components.Add(new MapComponent_SkyAmbience(__instance));
            }
        }
    }
}
