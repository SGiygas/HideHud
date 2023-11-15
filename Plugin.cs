using BepInEx;
using HarmonyLib;
using Reptile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HideHud
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }

        private CanvasGroup _dieMenu;
        private CanvasGroup _pauseUI;
        private CanvasGroup _effectsUI;
        private CanvasGroup _gameplayUI;
        private CanvasGroup _dialogueUI;
        private CanvasGroup _characterSelectUI;
        private CanvasGroup _danceAbilityUI;
        private CanvasGroup _styleSwitchUI;
        private CanvasGroup _outfitSwitchUI;
        private CanvasGroup _taxiUI;

        private Canvas _dynamicPhoneCanvas;
        private Canvas _openPhoneCanvas;
        private Canvas _closedPhoneCanvas;

        private List<LineRenderer> _graffitiLines = new List<LineRenderer>();
        private List<GameObject> _graffitiTargets = new List<GameObject>();

        private List<GameObject> _nameplates = new List<GameObject>();

        private bool _uiReady = false;
        private bool _phoneReady = false;
        private bool _hudIsVisible = true;

        private PluginConfig _config;

        private void Awake()
        {
            Instance = this;
            _config = new PluginConfig(Config);

            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            Harmony patches = new Harmony("sgiygas.hideHud");
            patches.PatchAll();
        }

        private void Update()
        {
            if (!_uiReady || !_phoneReady)
            {
                return;
            }

            if (Input.GetKeyDown(_config.ToggleKey.Value))
            {
                ToggleHudVisibility();
            }
        }

        private void ToggleHudVisibility()
        {
            _hudIsVisible = !_hudIsVisible;

            float alpha = _hudIsVisible ? 1.0f : 0.0f;

            if (_config.HideDeathUI.Value)
            {
                _dieMenu.alpha = alpha;
            }
            if (_config.HidePauseMenu.Value)
            {
                _pauseUI.alpha = alpha;
            }
            if (_config.HideScreenEffects.Value)
            {
                _effectsUI.alpha = alpha;
            }
            _gameplayUI.alpha = alpha;
            if (_config.HideDialogue.Value)
            {
                _dialogueUI.alpha = alpha;
            }
            if (_config.HideCypher.Value)
            {
                _characterSelectUI.alpha = alpha;
            }
            if (_config.HideDanceWheel.Value)
            {
                _danceAbilityUI.alpha = alpha;
            }
            if (_config.HideGearSelect.Value)
            {
                _styleSwitchUI.alpha = alpha;
            }
            if (_config.HideOutfitSelect.Value)
            {
                _outfitSwitchUI.alpha = alpha;
            }
            if (_config.HideTaxiUI.Value)
            {
                _taxiUI.alpha = alpha;
            }

            if (_config.HidePhone.Value)
            {
                _dynamicPhoneCanvas.enabled = _hudIsVisible;
                _closedPhoneCanvas.enabled = _hudIsVisible;
                _openPhoneCanvas.enabled = _hudIsVisible;
            }

            if (_config.HideSlopCrewNameplates.Value)
            {
                foreach (var nameplate in _nameplates)
                {
                    nameplate.SetActive(_hudIsVisible);
                }
            }

            if (_config.HideGraffitiUI.Value)
            {
                foreach (var gameObject in _graffitiTargets)
                {
                    gameObject.SetActive(_hudIsVisible);
                }
                foreach (var line in _graffitiLines)
                {
                    line.enabled = _hudIsVisible;
                }
            }
        }

        public void SetMainReferences(Reptile.UIManager uiManager)
        {
            Traverse traverse = Traverse.Create(uiManager);

            _dieMenu = CreateCanvasGroupForUI<DieMenu>(traverse, "dieMenu");
            _pauseUI = CreateCanvasGroupForUI<PauseMenu>(traverse, "pauseMenu");
            _effectsUI = CreateCanvasGroupForUI<EffectsUI>(traverse, "effects");
            _gameplayUI = CreateCanvasGroupForUI<GameplayUI>(traverse, "gameplay");
            _dialogueUI = CreateCanvasGroupForUI<DialogueUI>(traverse, "dialogueUI");
            _characterSelectUI = CreateCanvasGroupForUI<CharacterSelectUI>(traverse, "characterSelectUI");
            _danceAbilityUI = CreateCanvasGroupForUI<DanceAbilityUI>(traverse, "danceAbilityUI");
            _styleSwitchUI = CreateCanvasGroupForUI<StyleSwitchMenu>(traverse, "styleSwitchUI");
            _outfitSwitchUI = CreateCanvasGroupForUI<OutfitSwitchMenu>(traverse, "outfitSwitchUI");
            _taxiUI = CreateCanvasGroupForUI<TaxiUI>(traverse, "taxiUI");

            _uiReady = true;
        }

        private CanvasGroup CreateCanvasGroupForUI<T>(Traverse traverse, string fieldName) where T : Component
        {
            var uiObject = traverse.Field(fieldName).GetValue() as T;
            return uiObject.gameObject.AddComponent<CanvasGroup>();
        }

        public void SetPhoneReferences(Reptile.Phone.Phone phone)
        {
            Traverse traverse = Traverse.Create(phone);

            _dynamicPhoneCanvas = phone.dynamicGameplayScreen.transform.parent.GetComponent<Canvas>();

            var openPhoneCanvas = traverse.Field("openPhoneCanvas").GetValue() as GameObject;
            _openPhoneCanvas = openPhoneCanvas.GetComponent<Canvas>();

            var closedPhoneCanvas = traverse.Field("closedPhoneCanvas").GetValue() as GameObject;
            _closedPhoneCanvas = closedPhoneCanvas.GetComponent<Canvas>();

            _phoneReady = true;
        }

        public void AddGraffitiGameReference(Reptile.GraffitiGame game)
        {
            var targetParent = game.transform.Find("Targets").gameObject;
            targetParent.SetActive(_hudIsVisible);
            _graffitiTargets.Add(targetParent);

            var backgroundCircle = game.transform.Find("BackgroundCircle").gameObject;
            backgroundCircle.SetActive(_hudIsVisible);
            _graffitiTargets.Add(backgroundCircle);
        }
        public void RemoveGraffitiGameReference(Reptile.GraffitiGame game)
        {
            var targetParent = game.transform.Find("Targets").gameObject;
            _graffitiTargets.Remove(targetParent);

            var backgroundCircle = game.transform.Find("BackgroundCircle").gameObject;
            _graffitiTargets.Remove(backgroundCircle);
        }
        public void AddGraffitiLineReference(LineRenderer line)
        {
            _graffitiLines.Add(line);
            line.enabled = _hudIsVisible;
        }
        public void ClearGraffitiLineReferences(Reptile.GraffitiGame game)
        {
            _graffitiLines.RemoveAll(line => line.transform.parent == game.transform);
        }
        public void AddPlayerReference(Reptile.Player player)
        {
            // The player list is only relevant for slop crew
            if (!_config.HideSlopCrewNameplates.Value)
            {
                return;
            }

            // We have to wait until the end of the current frame because the nameplate doesn't exist on initialization
            StartCoroutine(SetInitialNameplateState(player));
        }
        private IEnumerator SetInitialNameplateState(Reptile.Player player)
        {
            yield return new WaitForEndOfFrame();

            var nameplate = player.interactionCollider.transform.Find("SlopCrew_NameplateContainer");
            if (nameplate != null)
            {
                GameObject nameplateObject = nameplate.gameObject;
                _nameplates.Add(nameplateObject);
                nameplateObject.SetActive(_hudIsVisible);
            }
        }
        public void RemovePlayerReference(Reptile.Player player)
        {
            // The player list is only relevant for slop crew
            if (!_config.HideSlopCrewNameplates.Value)
            {
                return;
            }

            var nameplate = player.interactionCollider.transform.Find("SlopCrew_NameplateContainer");
            if (nameplate != null)
            {
                GameObject nameplateObject = nameplate.gameObject;
                _nameplates.Remove(nameplateObject);
            }
        }

        public void SetNotReady()
        {
            _phoneReady = false;
            _hudIsVisible = true;
        }
    }
}
