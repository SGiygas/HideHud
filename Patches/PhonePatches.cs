using HarmonyLib;

namespace HideHud.Patches
{
    [HarmonyPatch(typeof(Reptile.Phone.Phone), nameof(Reptile.Phone.Phone.PhoneInit))]
    public class PhoneInitPatch
    {
        public static void Postfix(Reptile.Phone.Phone __instance)
        {
            Plugin.Instance.SetPhoneReferences(__instance);
        }
    }
}
