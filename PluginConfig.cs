

using BepInEx.Configuration;
using UnityEngine;

namespace HideHud
{
    public class PluginConfig
    {
        private const string ConfigTitle = "Settings";

        public ConfigEntry<KeyCode> ToggleKey { get; private set; }

        public ConfigEntry<bool> HidePauseMenu { get; private set; }
        public ConfigEntry<bool> HidePhone { get; private set; }
        public ConfigEntry<bool> HideDialogue { get; private set; }
        public ConfigEntry<bool> HideCypher { get; private set; }
        public ConfigEntry<bool> HideDanceWheel { get; private set; }
        public ConfigEntry<bool> HideGearSelect { get; private set; }
        public ConfigEntry<bool> HideOutfitSelect { get; private set; }
        public ConfigEntry<bool> HideDeathUI { get; private set; }
        public ConfigEntry<bool> HideTaxiUI { get; private set; }
        public ConfigEntry<bool> HideSlopCrewNameplates { get; private set; }

        public PluginConfig(ConfigFile file)
        {
            ToggleKey = file.Bind(ConfigTitle, "Toggle Key", KeyCode.O);
            HidePauseMenu = file.Bind(ConfigTitle, "Hide Pause Menu", false);
            HidePhone = file.Bind(ConfigTitle, "Hide Phone UI", true);
            HideDialogue = file.Bind(ConfigTitle, "Hide Dialogue UI", true);
            HideCypher = file.Bind(ConfigTitle, "Hide Character Select UI", true);
            HideDanceWheel = file.Bind(ConfigTitle, "Hide Dance Wheel", true);
            HideGearSelect = file.Bind(ConfigTitle, "Hide Gear Skin UI", false);
            HideOutfitSelect = file.Bind(ConfigTitle, "Hide Outfit Select UI", false);
            HideDeathUI = file.Bind(ConfigTitle, "Hide Death UI", false);
            HideTaxiUI = file.Bind(ConfigTitle, "Hide Taxi UI", false);
            HideSlopCrewNameplates = file.Bind(ConfigTitle, "Hide Slop Crew Nameplates", true);
        }
    }
}
