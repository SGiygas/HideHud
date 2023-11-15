using HarmonyLib;

namespace HideHud.Patches
{
    [HarmonyPatch(typeof(Reptile.Player), nameof(Reptile.Player.Init))]
    public class PlayerInitPatch
    {
        public static void Postfix(Reptile.Player __instance)
        {
            Plugin.Instance.AddPlayerReference(__instance);
        }
    }
    [HarmonyPatch(typeof(Reptile.Player), nameof(Reptile.Player.OnDestroy))]
    public class PlayerOnDestroyPatch
    {
        // Prefix because the player will already be destroyed on postfix
        public static void Prefix(Reptile.Player __instance)
        {
            Plugin.Instance.RemovePlayerReference(__instance);
        }
    }
}
