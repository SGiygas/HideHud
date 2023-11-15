using HarmonyLib;

namespace HideHud.Patches
{
    [HarmonyPatch(typeof(Reptile.BaseModule), nameof(Reptile.BaseModule.SetupNewStage))]
    public class BaseModulePatches
    {
        public static void Prefix()
        {
            Plugin.Instance.SetNotReady();
        }
    }
}
