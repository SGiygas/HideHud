

using HarmonyLib;
using UnityEngine;

namespace HideHud.Patches
{
    [HarmonyPatch(typeof(Reptile.GraffitiGame), nameof(Reptile.GraffitiGame.Init))]
    public class GraffitiGameInitPatch
    {
        public static void Postfix(Reptile.GraffitiGame __instance)
        {
            Plugin.Instance.AddGraffitiGameReference(__instance);
        }
    }
    [HarmonyPatch(typeof(Reptile.GraffitiGame), nameof(Reptile.GraffitiGame.End))]
    public class GraffitiGameEndPatch
    {
        public static void Postfix(Reptile.GraffitiGame __instance)
        {
            Plugin.Instance.RemoveGraffitiGameReference(__instance);
            Plugin.Instance.ClearGraffitiLineReferences(__instance);
        }
    }
    [HarmonyPatch(typeof(Reptile.GraffitiGame), "HitTarget")]
    public class GraffitiGameHitTargetPatch
    {
        public static void Postfix(LineRenderer ___line)
        {
            Plugin.Instance.AddGraffitiLineReference(___line);
        }
    }
    [HarmonyPatch(typeof(Reptile.GraffitiGame), nameof(Reptile.GraffitiGame.InitVisual))]
    public class GraffitiGameInitVisualPatch
    {
        public static void Postfix(LineRenderer ___line)
        {
            Plugin.Instance.AddGraffitiLineReference(___line);
        }
    }
}
