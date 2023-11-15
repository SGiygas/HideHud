using HarmonyLib;

namespace HideHud.Patches
{
    [HarmonyPatch(typeof(Reptile.UIManager), "InitGameMenu")]
    public class UIManagerInitPatch
    {
        public static void Postfix(Reptile.UIManager __instance)
        {
            Plugin.Instance.SetMainReferences(__instance);
        }
    }
}
