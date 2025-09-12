using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KS
{
    public enum cDeviceTypes { Keyboard = 0, Gamepad = 1};

    public class UIInputPrompt : MonoBehaviour
    {
        [SerializeField] private SettingsUIManager settings;
        [Space]
        [SerializeField] private InputActionReference inputReference;
        [Range(0, 10), SerializeField] private int selectBinding;
        [SerializeField] private InputBinding.DisplayStringOptions displayStringOptions;

        [SerializeField] private cDeviceTypes device = cDeviceTypes.Keyboard;

        [Header("Binding Info")]
        [SerializeField] private InputBinding inputBinding;
        private int bindingIndex;
        private string actionName;

        [Header("prompt setup")]
        [SerializeField] private UISpriteAssetSO spriteAssets;
        [SerializeField] private TMP_Text textBox;
        [TextArea(2, 3), SerializeField] private string message = "";
        [SerializeField] private bool dynamic = true;

        private void OnValidate()
        {
            if (Application.isPlaying)
                if (PlayerInputManager.controls == null)
                    return;

            if (inputReference == null)
                return;

            textBox = GetComponent<TMP_Text>();
            GetBindingInfo();
            SetPromptText();

        }

        private void OnEnable()
        {
            PlayerInputManager.ActiveDeviceChanged += SetPromptText;
            UIOptionsGamepadUI.GamepadUIChanged += SetPromptText;
        }

        private void Awake()
        {
            settings = FindObjectOfType<SettingsUIManager>();
        }

        private void OnDestroy()
        {
            PlayerInputManager.ActiveDeviceChanged -= SetPromptText;
            UIOptionsGamepadUI.GamepadUIChanged -= SetPromptText;
        }

        private void Start()
        {

            textBox = GetComponent<TMP_Text>();
            if (inputReference != null)
            {
                GetBindingInfo();
                SetPromptText();
            }

        }

        private void GetBindingInfo()
        {
            if (inputReference.action != null)
                actionName = inputReference.action.name;

            if (inputReference.action.bindings.Count > selectBinding)
            {
                inputBinding = inputReference.action.bindings[selectBinding];
                bindingIndex = selectBinding;
            }
        }

        private void SetPromptText()
        {
            if (dynamic)
            {
                device = PlayerInputManager.GetActiveDevice();
            }

            int spriteAssetIndex = (int)device;

            if (device == cDeviceTypes.Gamepad)
            {
                if(dynamic)
                    bindingIndex = inputReference.action.bindings.Count - 1;
                
                switch (SettingsUIManager.uiPrompt)
                {
                    case SettingsUIManager.UIGamepadPrompts.xbox:
                        spriteAssetIndex = 1;
                        break;
                    case SettingsUIManager.UIGamepadPrompts.playstation:
                        spriteAssetIndex = 2;
                        break;
                    default:
                        break;
                }

            }
            else if (device == cDeviceTypes.Keyboard)
            {
                if (dynamic)
                    bindingIndex = 0;

                spriteAssetIndex = 0;
            }

            //Debug.Log("device: " + device + " - " + (int)device);

            if ((int)device > spriteAssets.spriteAssets.Count - 1)
            {
                Debug.LogFormat("Missing sprite asset for {0}", device);
                return;
            }

            textBox.text = UITextPromptReplacer.ReplaceBinding(
                message,
                inputReference.action.bindings[bindingIndex],
                spriteAssets.spriteAssets[spriteAssetIndex]
                );

        }

    }
}